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
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class HeatRayProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.timeLeft = 600;
            entity.extraUpdates = 200;
        }
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.HeatRay;
        }
        public override bool PreAI(Projectile projectile)
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 6f)
            {
                for (int num381 = 0; num381 < 3; num381++)
                {
                    Vector2 vector106 = projectile.position;
                    vector106 -= projectile.velocity * ((float)num381 * 0.25f);
                    projectile.alpha = 255;
                    int num382 = Dust.NewDust(vector106, 1, 1, DustID.HeatRay);
                    Main.dust[num382].position = vector106;
                    Main.dust[num382].position.X += projectile.width / 2;
                    Main.dust[num382].position.Y += projectile.height / 2;
                    Main.dust[num382].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Dust dust141 = Main.dust[num382];
                    Dust dust212 = dust141;
                    dust212.velocity *= 0.2f;
                    dust212.velocity += projectile.velocity * 0.6f;
                }
            }
            return false;


        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.oldPosition + projectile.oldVelocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= projectile.oldVelocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            base.OnKill(projectile, timeLeft);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.position + projectile.velocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= projectile.velocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            if(target.HasBuff(BuffID.OnFire3))
            {
                if(target.buffTime[target.FindBuffIndex(BuffID.OnFire3)] < 600)
                {
                    target.AddBuff(BuffID.OnFire3, target.buffTime[target.FindBuffIndex(BuffID.OnFire3)] + 20);
                }
            } else
            {
                target.AddBuff(BuffID.OnFire3, 20);
            }
            
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.oldPosition + oldVelocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= oldVelocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            return true;
        }
        //public override bool InstancePerEntity => true;
    }
}
