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
    public class MythrilManaArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 1; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 1200; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override void Load() //this method AND the following one together allow this projectile to be reflected by biome mimics and the like.
        {
            On_Projectile.CanBeReflected += CanBeReflected2;
        }
        private bool CanBeReflected2(On_Projectile.orig_CanBeReflected orig, Projectile self)
        {
            bool validReflectable = self.active && self.friendly && !self.hostile && self.damage > 0;
            return orig(self) || (self.type == Projectile.type && validReflectable);
        }

        // Custom AI
        public override void PostAI()
        {
            UnifiedArrowProjectile.CreateArrowTrail(Projectile, new Color(0f,0.1f,0.3f,0f));
            
            if (Main.rand.NextBool(5))
            {
                Dust flightDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFlare, 0, 0, 0, default(Color), 1.5f);
                flightDust.noGravity = true;
            }
            Lighting.AddLight(Projectile.position, 0.2f, 0.6f, 1f);
            
        }

        public override void OnKill(int timeLeft)
        {
            for(int i=0;i<5;i++)
            {
                Dust flightDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFlare, 0, 0, 0, default(Color), 1.5f);
                flightDust.noGravity = true;
                flightDust.velocity += Projectile.velocity * 0.1f;
            }
            base.OnKill(timeLeft);
        }

        /*public override bool PreDraw(ref Color lightColor)
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

        }*/

    }
}
