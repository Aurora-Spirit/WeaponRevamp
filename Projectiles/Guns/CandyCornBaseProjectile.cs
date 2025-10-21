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

namespace WeaponRevamp.Projectiles.Guns
{
    public class CandyCornBaseProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.CandyCorn;
        }

        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.penetrate = 1;
            entity.appliesImmunityTimeOnSingleHits = true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.type != NPCID.ShimmerSlime)
            {
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity * 2, ModContent.ProjectileType<CandyCornWhiteProjectile>(), projectile.damage / 2, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity, ModContent.ProjectileType<CandyCornReducedProjectile>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);

            }


            base.OnHitNPC(projectile, target, hit, damageDone);
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (target.immune[projectile.owner] > 0)
            {
                return false;
            }
            return base.CanHitNPC(projectile, target);
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
