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
using Terraria.GameContent.Drawing;

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisShimmerArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?
            Projectile.timeLeft = 300; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = 6;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        // Custom AI
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.position, new Vector3(Main.DiscoR / 256.0f, Main.DiscoG / 256.0f, Main.DiscoB / 256.0f));
            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item92, Projectile.position);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 40)
            {
                NPC target = Projectile.FindTargetWithinRange(800);
                if (target != null) Projectile.velocity += Vector2.Normalize(target.Center - Projectile.Center) * 0.45f * Projectile.penetrate;
                if (Projectile.velocity.Length() > 8)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12;
                }
            }
            if (((int)Projectile.ai[0])%5 == 0)
            {
                float num678 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                for (float num679 = 0f; num679 < 1f; num679 += 1f)
                {
                    float num680 = num678 + (float)Math.PI * 2f * num679;
                    Vector2 vector55 = Vector2.UnitX.RotatedBy(num680);
                    Vector2 center = Projectile.Center;
                    float num681 = 0.4f;
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
                    {
                        PositionInWorld = center,
                        MovementVector = vector55 * num681
                    }, Projectile.owner);
                }
            }
            if (((int)Projectile.ai[0]) % 2 == 0)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(4f, 4f), 0, 0, 306, 0, 0, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), 1f + Main.rand.NextFloat() * 0.4f);
                dust13.noGravity = true;
                dust13.velocity *= 1f;
                dust13.velocity += Projectile.oldVelocity;
                dust13.fadeIn = dust13.scale + 0.05f;
                Dust dust14 = Dust.CloneDust(dust13);
                dust14.color = Color.White;
                dust14.scale -= 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            float num678 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
            for (float num679 = 0f; num679 < 2f; num679 += 0.5f)
            {
                float num680 = num678 + (float)Math.PI * 2f * num679;
                Vector2 vector55 = Vector2.UnitX.RotatedBy(num680);
                Vector2 center = Projectile.Center;
                float num681 = 0.4f;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
                {
                    PositionInWorld = center,
                    MovementVector = vector55 * num681
                }, Projectile.owner);
            }
            for(int i=0;i<20f;i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(4f, 4f), 0, 0, 306, 0, 0, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), 1f + Main.rand.NextFloat() * 0.4f);
                dust13.noGravity = true;
                dust13.velocity *= 4f;
                dust13.velocity += Projectile.oldVelocity;
                dust13.fadeIn = dust13.scale + 0.05f;
                Dust dust14 = Dust.CloneDust(dust13);
                dust14.color = Color.White;
                dust14.scale -= 0.3f;
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
            //float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 0), new Color(255, 255, 255, 255), Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 1), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255), Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            



            return false;

        }


    }
}
