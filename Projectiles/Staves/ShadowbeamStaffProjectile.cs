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

namespace WeaponRevamp.Projectiles.Staves
{
    public class ShadowbeamStaffProjectile : GlobalProjectile
    {
        int bounces = 0;
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.ShadowBeamFriendly;
        }
        public override bool PreAI(Projectile projectile)
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 9f)
            {
                int dustID = Dust.NewDust(projectile.position + new Vector2(-1, -1), 0, 0, DustID.ShadowbeamStaff);
                Dust dust = Main.dust[dustID];
                dust.scale = 1.5f + (float)Math.Sqrt(10f * (float)bounces) * 0.1f;
                dust.velocity += projectile.velocity * 0.5f;
                dust.velocity *= 0.2f;
                
            }
            return false;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X || projectile.velocity.Y != oldVelocity.Y)
            {
                int plusDamage = 25;
                if(bounces > 10)
                {
                    plusDamage /= bounces-10;
                }

                projectile.damage = (int)((double)projectile.damage + plusDamage);
                bounces += 1;
            }
            /*if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X += projectile.velocity.X;
                projectile.velocity.X = 0f - oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y += projectile.velocity.Y;
                projectile.velocity.Y = 0f - oldVelocity.Y;
            }*/

            projectile.netUpdate = true;
            return true;
        }
        public override bool InstancePerEntity => true;
    }
}
