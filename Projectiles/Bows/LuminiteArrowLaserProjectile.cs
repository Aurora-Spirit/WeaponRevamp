using System;
using System.Collections;
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
    public class LuminiteArrowLaserProjectile : ModProjectile
    {
        private float rotate = 0;

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
            Projectile.timeLeft = 60; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            
        }

        public override bool? CanDamage()
        {
            return false;
        }

        // Custom AI
        public override void PostAI()
        {
            Lighting.AddLight(Projectile.position, 0.2f, 1f, 1f);
            Projectile.rotation += 0.1f;
            if (Projectile.timeLeft != 60)
            {
                Projectile.position = Projectile.oldPosition;
            }
            else
            {
                Projectile.scale = 0;
            }

            if (Projectile.timeLeft > 55)
            {
                Projectile.scale += 0.2f;
            }
            
            if (Projectile.timeLeft == 20)
            {
                //forgive me for my coding sins. i hope this is relatively readable.
                rotate = (float)Math.PI / 8f * (Main.rand.NextBool() ? 1 : -1);
                UnifiedArrowProjectile.NewUnifiedArrow(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity.RotatedBy(rotate), (int)Projectile.ai[0], Projectile.damage/2, Projectile.knockBack * 0.5f, ProjectileID.MoonlordArrow + 1, Projectile.owner, 0, Projectile.ai[1], Projectile.ai[2]);
                rotate *= -1;
            }
            if (Projectile.timeLeft == 10)
            {
                UnifiedArrowProjectile.NewUnifiedArrow(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity.RotatedBy(rotate), (int)Projectile.ai[0], Projectile.damage/2, Projectile.knockBack * 0.5f, ProjectileID.MoonlordArrow + 1, Projectile.owner, 0, Projectile.ai[1], Projectile.ai[2]);
            }

            if (Projectile.timeLeft < 10)
            {
                Projectile.scale *= 0.8f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            
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
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 0), new Color(255,255,255,128) * lightingColor*0.3f, Projectile.rotation*0.5f, origin, scale*1.3f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 1), new Color(255,255,255,128) * lightingColor*0.5f, Projectile.rotation*-0.5f, origin, scale*1.1f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 2, 0, 0), new Color(255,255,255,128) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);


            return false;

        }

    }
}
