using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Projectiles.Guns
{
    public class OnyxBlasterProjectile : GlobalProjectile
    {
        public EntitySource_ItemUse_WithAmmo source;
        public int ammoType = -1;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.BlackBolt;
        }

        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.extraUpdates = 2;
            entity.MaxUpdates = 3;
            entity.timeLeft = 30;
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            int bulletcount = 18;
            if (source != null && ammoType != -1)
            {
                Vector2 baseVel = Vector2.UnitX*12f;
                for (int i = 0; i < bulletcount; i++)
                {
                    baseVel = baseVel.RotatedBy(Math.PI * 2 / bulletcount);
                    Vector2 vel = baseVel.RotatedByRandom(Math.PI * 2 / bulletcount);
                    Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, projectile.Center - projectile.velocity, vel, ModContent.ProjectileType<UnifiedBulletProjectile>(), projectile.damage/2, projectile.knockBack, ammoType, projectile.owner);
                }
                
            }
        }


        //might make it actually lose the white part after the first hit

        /*public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Vector2 position = projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 14);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            
            Color renderAlpha;
            
            position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 14, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            


            return false;
        }*/

    }
}
