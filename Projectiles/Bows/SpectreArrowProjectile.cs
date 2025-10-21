using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace WeaponRevamp.Projectiles.Bows
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class SpectreArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 1200; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {

            Projectile.ai[0] = 90;
        }
        // Custom AI
        public override void AI()
        {
            if (Projectile.ai[0] >= 90 && Projectile.ai[1] > 15)
            {
                Projectile.velocity.Y += 0.1f;
            }
            else if (Projectile.ai[0] > 0)
            {
                Projectile.velocity.Y += 0.2f;
            }

            if (Projectile.ai[0] >= 90 || Projectile.ai[0] <= 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.rotation += 0.2f;
                Projectile.ai[0] -= 1;
            }
            Projectile.ai[1] += 1;


            if (Projectile.ai[0] > 0) //arrow has not shattered yet
            {
                if (Main.rand.NextBool(8))
                {
                    Dust spectreFlightDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit);
                    spectreFlightDust.noGravity = true;
                }
                if (Main.rand.NextBool(16))
                {
                    Dust spectreFlightDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);
                    spectreFlightDust.velocity *= 0.5f;
                }
                Lighting.AddLight(Projectile.position, 0.1f, 0.3f, 0.5f);
            }
            else //arrow has shattered
            {

                Projectile.netUpdate = true;
                if (Projectile.penetrate < 0)
                {

                    Projectile.penetrate = 1;
                    for (int i = 0; i < 10; i++)
                    {
                        Dust spectreShatterDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);
                        spectreShatterDust.velocity *= 0.5f;
                        spectreShatterDust.velocity += Projectile.velocity * 0.5f;
                    }
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                    Projectile.tileCollide = false;
                    Projectile.ignoreWater = true;
                }
                Dust spectreFlightDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit);
                spectreFlightDust.noGravity = true;
                NPC target = Projectile.FindTargetWithinRange(800);
                if (target == null)
                {
                    if (Projectile.timeLeft > 5)
                    {
                        Projectile.timeLeft -= 5;
                    }
                }
                else
                {
                    Projectile.velocity += Vector2.Normalize(target.Center - Projectile.Center) * 0.5f;
                    Projectile.velocity *= 0.98f;
                }
                Lighting.AddLight(Projectile.position, 0.2f, 0.6f, 1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0] -= 1; //start the shatter process
                Projectile.velocity += Vector2.Normalize(Projectile.Center - target.Center) * Projectile.velocity.Length() * 2f * Vector2.Dot(Vector2.Normalize(-Projectile.Center + target.Center), Vector2.Normalize(Projectile.velocity));
                Projectile.velocity *= 0.7f;
                Projectile.netUpdate = true;
            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.netUpdate = true;
            Projectile.ai[0] -= 1; //start the shatter process
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = Projectile.oldVelocity.X * -1f;
                //Projectile.position.X += Projectile.velocity.X;
            }
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = Projectile.oldVelocity.Y * -1f;
                //Projectile.position.Y += Projectile.velocity.Y;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundStyle soundStyle = SoundID.Zombie53;
            soundStyle.MaxInstances = 5;
            SoundEngine.PlaySound(soundStyle, Projectile.position);
            for (int j = 0; j < 20; j++)
            {
                Dust spectreKillDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0, 0, 0, default, 1.5f);
                spectreKillDust.velocity *= 2;
                spectreKillDust.velocity += Projectile.velocity;
                spectreKillDust.noGravity = true;
            }
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 2);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            position = Projectile.Center - Main.screenPosition;
            if (Projectile.ai[0] > 0)
            {
                Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 0), new Color(255,255,255,255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            } else
            {
                Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 1), new Color(255,255,255,255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            


            return false;

        }

    }
}
