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
    public class GenesisBoneArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 42; // The width of projectile hitbox
            Projectile.height = 42; // The height of projectile hitbox

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

        }
        // Custom AI
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Lighting.AddLight(Projectile.position, new Vector3(Main.DiscoR / 256.0f, Main.DiscoG / 256.0f, Main.DiscoB / 256.0f));
            
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 35)
            {
                Projectile.velocity.Y += 0.07f;
            }
            if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;
            if (((int)Projectile.ai[0])%5 == 0)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.BoneTorch);
                dust13.velocity *= 0.1f;
                dust13.velocity += Projectile.oldVelocity * 0.5f;
                dust13.noGravity = true;
                dust13.scale = 1.5f;
            }
            if (((int)Projectile.ai[0]) % 2 == 0)
            {
                Dust dust1 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Bone);
                dust1.velocity *= 0.1f;
                dust1.velocity += Projectile.oldVelocity * 0.5f;
            }
            Dust eye = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(0,200,255));
            eye.scale = 0.3f;
            eye.velocity *= 0f;
            eye.velocity = Projectile.velocity;
            Vector2 direction = Vector2.Normalize(Projectile.velocity);
            eye.position += direction * 24f + direction.RotatedBy(Math.PI * 0.5f) * -5f; //align dust with eye
            eye.position += new Vector2(-2f, -2f); //offset for dust size
        }

        public override void OnKill(int timeLeft)
        {
            for(int i=0;i<20f;i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Bone);
                dust13.velocity *= 2f;
                dust13.velocity += Projectile.oldVelocity * 0.5f;
            }
            for (int i = 0; i < 10f; i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.BoneTorch);
                dust13.velocity *= 2f;
                dust13.velocity += Projectile.oldVelocity * 0.5f;
                dust13.noGravity = true;
                dust13.scale = 1.5f;
            }

            base.OnKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
            for (int i = 0; i < 8f; i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Bone);
                dust13.velocity *= 2f;
                dust13.velocity += Projectile.oldVelocity * 0.1f;
            }
            for (int i = 0; i < 4f; i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.BoneTorch);
                dust13.velocity *= 2f;
                dust13.velocity += Projectile.oldVelocity * 0.1f;
                dust13.noGravity = true;
                dust13.scale = 1.5f;
            }
            for (int i = 0; i < 2f; i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.BoneTorch);
                dust13.velocity *= 0.05f;
                dust13.noGravity = true;
                dust13.scale = 2f;
            }
            target.immune[Projectile.owner] = 2;
            Projectile.position -= Projectile.velocity * 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, 0), new Color(255, 255, 255, 255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            


            return false;

        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width /= 3;
            height /= 3;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }


    }
}
