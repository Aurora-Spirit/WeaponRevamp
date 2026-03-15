using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Guns;

namespace WeaponRevamp.Projectiles.Sentries
{
	// ExampleSentry is an example sentry.
	// ExampleSentry demonstrates both floating and grounded sentry behaviors. Use ExampleSentryItem to the left of the player spawn a grounded sentry and use it to the right to spawn a floating sentry.
	// Sentries are similar to Minions, but typically don't move, are limited by the sentry limit instead of the minion limit, don't have a corresponding buff, and last for 10 minutes instead of despawning when the player dies.
	// The most critical fields necessary for a projectile to count as a sentry will be noted in this file and in ExampleSentryItem.cs. See also ExampleSentryShot.cs.
	
	public class FrostHydraSentry : ModProjectile
	{
		private static Asset<Texture2D> headsTexture;
		public ref float barrelRotation => ref Projectile.ai[0];
		public ref float rotationSpeed => ref Projectile.ai[1];
		public bool hasTarget = false;

		public int directionButBetter = 1;

		public Vector2 headOffset = new Vector2(6f, -2f);
		public Vector2 headRotationOffset = new Vector2(-10f, 11f);
		public Vector2 firingOffset = new Vector2(0f, -22f);

		public Vector2 exhaustOffset = new Vector2(6f, -2f);
		public Vector2 exhaustRotatedOffset = new Vector2(18f, -22f);
		//public Vector2 headRotationOffset = new Vector2(0f, 0f);

		public bool JustSpawned {
			get => Projectile.localAI[0] == 0;
			set => Projectile.localAI[0] = value ? 0 : 1;
		}

		public override void Load()
		{
			headsTexture = ModContent.Request<Texture2D>("WeaponRevamp/Projectiles/Sentries/FrostHydraSentry_Heads");
		}

		public override void SetStaticDefaults() {
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.minionSlots = 0.5f;
			Projectile.width = 21*2;
			Projectile.height = 13*2;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true; // Sets the weapon as a sentry for sentry accessories to properly work.
			Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
			Projectile.ignoreWater = true;
			Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players

			// The texture is 54 pixels wide, but we set width to 42 and DrawOffsetX to -6 so it doesn't look weird hanging off the edge of tiles (because it is oval shaped).
			//DrawOffsetX = -6;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = false; // Allow this projectile to collide with platforms
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false; // Prevent tile collision from killing the projectile
		}

		public override Color? GetAlpha(Color lightColor) {
			// Always draw fully bright. This is important because sentries can usually be placed inside tiles where it would be dark.
			return Color.White;
		}

		// This AI will function as a static sentry, and will not move. If you would like to know how to do more advanced minion AI, check out ExampleSimpleMinion.cs.
		public override void AI()
		{
			const float idleRotationSpeedTarget = 1.5f; //sentry switches heads thrice a second while idle.
			const float firingRotationSpeedTarget = 4; //sentry switches heads thrice a second while idle.
			const float TargetingRange = 63 * 16;

			// Code to run when spawned
			if (JustSpawned) {
				JustSpawned = false;

				// The sound that Frost Hydra, Spider Turret, and Houndius Shootius play when spawned. Optional.
				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
				rotationSpeed = idleRotationSpeedTarget;
				barrelRotation = 0;

				// Dust indicating the sentry spawned. Optional.
				/*for (int i = 0; i < 50; i++) {
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BlueCrystalShard, speed * 4, Scale: 1.5f);
					d.noGravity = true;
				}*/
			}

			// Spawn dust randomly
			/*if (Main.rand.NextBool(10)) {
				Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Firework_Blue, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
				dust.noGravity = true;
				dust.velocity *= 0.8f;
			}*/

			// Gravity
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			
			//lighting and passive dust
			Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.3f, 0.8f));
			if (Main.rand.NextBool(10))
			{
				Dust snow = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SnowSpray);
				snow.noGravity = false;
				snow.velocity *= 0.2f;
			}
			
			// Find an enemy to target.
			float closestTargetDistance = TargetingRange;
			NPC targetNPC = null;
			// Prioritize the owner's minion attack target. (Right click or whip feature)
			if (Projectile.OwnerMinionAttackTargetNPC != null) {
				TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);
			}

			// If no minion attack target or if it was out of range, find the closest enemy to target.
			if (targetNPC == null) {
				foreach (var npc in Main.ActiveNPCs) {
					TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
				}
			}
			
			
			hasTarget = targetNPC != null;

			if (targetNPC != null)
			{
				if (rotationSpeed < firingRotationSpeedTarget)
				{
					rotationSpeed += 1/60f;
					if (rotationSpeed > firingRotationSpeedTarget)
					{
						rotationSpeed = firingRotationSpeedTarget;
					}
				}
			}
			else
			{
				if (rotationSpeed > idleRotationSpeedTarget)
				{
					rotationSpeed -= 1/60f;
					if (rotationSpeed < idleRotationSpeedTarget)
					{
						rotationSpeed = idleRotationSpeedTarget;
					}
				}
			}

			if (targetNPC != null)
			{
				directionButBetter = Math.Sign(targetNPC.Center.X - Projectile.Center.X);
				Vector2 modifiedFiringOffset = new Vector2(firingOffset.X * directionButBetter, firingOffset.Y);
				Projectile.rotation = (targetNPC.Center - Projectile.Center - modifiedFiringOffset).ToRotation();
			}
			else
			{
				if (directionButBetter == 1)
				{
					Projectile.rotation = float.Lerp(Projectile.rotation, 0, 0.1f);
				}
				else
				{
					Projectile.rotation = float.Lerp(Projectile.rotation, (float)Math.PI, 0.1f);
				}
			}

			barrelRotation += rotationSpeed/60;
			if (barrelRotation > 1)
			{
				barrelRotation -= 1;
				if (targetNPC != null)
				{
					//fire!
					SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.4f }, Projectile.Center);

					// Actually spawning the projectile only runs if the local player is the owner
					if (Main.myPlayer == Projectile.owner)
					{
						float shootSpeed = 12;
						Vector2 modifiedFiringOffset = new Vector2(firingOffset.X * directionButBetter, firingOffset.Y);
						// The direction the projectile will fire.
						
						//the relative position of the target to the player
						Vector2 relativeTarget = (targetNPC.Center - Projectile.Center - modifiedFiringOffset);
						//the relative position of where the target will be when the projectile reaches it
						Vector2 relativePredictiveTarget = relativeTarget + targetNPC.velocity * (relativeTarget.Length()/(shootSpeed*FrostHydraIceChunk.updatesPerTick));
						//the angle that this needs to fire at in order to reach its target
						float g = FrostHydraIceChunk.gravity;
						float v = shootSpeed;
						float x = relativePredictiveTarget.X;
						float y = -relativePredictiveTarget.Y;
						float firingAngleAccountingForDrop = (float)( ( (v*v) - Math.Sqrt( (v*v*v*v) - g*(g*x*x + 2*y*v*v) ) ) / (g*x) );
						//Main.NewText(firingAngleAccountingForDrop);
						Vector2 shootDirection = new Vector2(1, -firingAngleAccountingForDrop).SafeNormalize(Vector2.Zero)*directionButBetter;
						if (shootDirection.Equals(Vector2.Zero)) //if it can't actually hit its target, fire in their general direction anyway
						{
							shootDirection = (targetNPC.Center - Projectile.Center - modifiedFiringOffset).SafeNormalize(Vector2.UnitX);
						}
						
						// The final velocity vector
						Vector2 shootVelocity = shootDirection * shootSpeed;
						Vector2 modifiedShootVelocity = shootVelocity.RotatedByRandom(Math.PI / 36);
						//Vector2 modifiedShootVelocity = shootVelocity;

						// The type of projectile the sentry will shoot. It is important that sentry shots are included in ProjectileID.Sets.SentryShot, so reusing unrelated vanilla projectiles as-is won't work 100%.
						int type = ModContent.ProjectileType<FrostHydraIceChunk>();

						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + modifiedFiringOffset, modifiedShootVelocity, type, Projectile.damage, Projectile.knockBack, Projectile.owner);
						// Note that Projectile.damage will take into account current equipment damage bonuses automatically for sentries and minions, so there is no need to calculate that here to take advantage of current equipment bonuses.
						// See Projectile.ContinuouslyUpdateDamageStats docs for more information.
						Vector2 modifiedExhaustOffset = new Vector2(exhaustOffset.X * directionButBetter, exhaustOffset.Y);
						Vector2 modifiedExhaustRotatedOffset = new Vector2(exhaustRotatedOffset.X, exhaustRotatedOffset.Y * directionButBetter);
						for (int i = 0; i < 5; i++)
						{
							Dust exhaust = Dust.NewDustDirect(Projectile.Center + modifiedExhaustOffset + modifiedExhaustRotatedOffset.RotatedBy(Projectile.rotation) + new Vector2(-2, -2), 0, 0, DustID.IceTorch);
							exhaust.noGravity = true;
							exhaust.velocity *= 1.5f;
							exhaust.velocity += shootVelocity * 0.2f;
							exhaust.scale = 1.2f;
						}
						for (int i = 0; i < 10; i++)
						{
							Dust exhaust = Dust.NewDustDirect(Projectile.Center + modifiedExhaustOffset + modifiedExhaustRotatedOffset.RotatedBy(Projectile.rotation) + new Vector2(-2, -2), 0, 0, DustID.Snow);
							exhaust.noGravity = true;
							exhaust.velocity *= 1f;
							exhaust.velocity += shootVelocity * 0.4f;
							exhaust.scale = 1f;
						}
					}
				}
			}

			Projectile.frameCounter += 1;
			if (Projectile.frameCounter >= 10)
			{
				Projectile.frame += 1;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 6)
				{
					Projectile.frame = 0;
				}
			}


		}

		// Checks if npc is closer than current targetNPC. If so, adjust targetNPC and closestTargetDistance.
		private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC) {
			if (npc.CanBeChasedBy(this)) {
				float distanceToTargetNPC = Vector2.Distance(Projectile.Center, npc.Center);
				// Is this enemy closer than others? Is it in line of sight?
				Vector2 modifiedFiringOffset = new Vector2(firingOffset.X * Math.Sign(npc.Center.X - Projectile.Center.X), firingOffset.Y);
				if (distanceToTargetNPC < closestTargetDistance && Collision.CanHit(Projectile.position + modifiedFiringOffset, Projectile.width, Projectile.height, npc.position, npc.width, npc.height)) {
					closestTargetDistance = distanceToTargetNPC; // Set a new closest distance value
					targetNPC = npc;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D hTexture = headsTexture.Value;
            Rectangle sourceRectangle = texture.Frame(1, 6);
            Rectangle hSourceRectangle = hTexture.Frame(1, 6);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            int stretch;

            frameNum = (int)(barrelRotation * 3);
            if (frameNum == 0 && hasTarget)
            {
	            frameNum = 3;
            }
            /*if (Projectile.ai[0] == spectre) //for electric bullet later
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }*/
            Vector2 modifiedHeadOffset = new Vector2(headOffset.X * directionButBetter, headOffset.Y);
            Color renderAlpha = new Color(255,255,255,255);
            SpriteEffects flipped;
            float extraRotation;
            if (directionButBetter == -1)
            {
	            flipped = SpriteEffects.FlipHorizontally;
	            extraRotation = (float)Math.PI;
            }
            else
            {
	            flipped = SpriteEffects.None;
	            extraRotation = 0;
            }
            Vector2 hOrigin = new Vector2(hSourceRectangle.Width/2f + (headRotationOffset.X)*directionButBetter, headRotationOffset.Y + hSourceRectangle.Height);
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 6, 0, Projectile.frame), renderAlpha, 0, origin, scale, flipped, 0f);
            Main.EntitySpriteDraw(hTexture, position + modifiedHeadOffset, hTexture.Frame(1, 4, 0, frameNum), renderAlpha, Projectile.rotation + extraRotation, hOrigin, scale, flipped, 0f);
            

            return false;
		}

		public override void OnKill(int timeLeft) {
			// Some sentries play a sound when despawned:
			//SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

			// Dust indicating the sentry despawned
			/*for (int i = 0; i < 50; i++) {
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(Projectile.Center + speed * 50, DustID.BlueCrystalShard, speed * -5, Scale: 1.5f);
				d.noGravity = true;
			}*/
		}
	}
}