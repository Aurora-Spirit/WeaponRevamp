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
    public class GenesisCursedFireballProjectile : ModProjectile
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
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        // Custom AI
        public override void AI()
        {
            Projectile.velocity *= 1.005f;
            Projectile.rotation += 0.5f;
            Lighting.AddLight(Projectile.position, new Vector3(0.25f,1f,0f));
            Projectile.ai[0]++;
            Projectile.velocity.Y += 0.1f;
            if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;

            int target = Projectile.FindTargetWithLineOfSight(400);
            if (target >= 0 && target <= 200 && Main.npc[target] != null)
            {
                Projectile.velocity += Vector2.Normalize(Main.npc[target].position - Projectile.position) * 1f;
                Projectile.velocity *= 0.96f;
            }
            
            if (((int)Projectile.ai[0])%1 == 0)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.CursedTorch);
                dust13.velocity *= 2f;
                dust13.velocity += Projectile.oldVelocity * 0.5f;
                dust13.noGravity = true;
                dust13.scale = 1.5f;
            }
            
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20f; i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.CursedTorch);
                dust13.velocity *= 5f;
                dust13.velocity += Projectile.oldVelocity * 0.1f;
                dust13.noGravity = true;
                dust13.scale = 1.5f;
            }

            base.OnKill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }
            if (Projectile.oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno,90);
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
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, 0), new Color(255, 255, 255, 255), Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            


            return false;

        }


    }
}
