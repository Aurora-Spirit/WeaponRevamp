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
    public class CrystalSerpentProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.CrystalPulse2;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            for (int num287 = 0; num287 < 7; num287++)
            {
                int num288 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.CrystalPulse, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.5f);
                Dust dust199;
                Dust dust334;
                if (Main.rand.NextBool(3))
                {
                    Main.dust[num288].fadeIn = 1.1f + (float)Main.rand.Next(-10, 11) * 0.01f;
                    Main.dust[num288].scale = 0.35f + (float)Main.rand.Next(-10, 11) * 0.01f;
                    dust199 = Main.dust[num288];
                    dust334 = dust199;
                    dust334.type++;
                }
                else
                {
                    Main.dust[num288].scale = 1.2f + (float)Main.rand.Next(-10, 11) * 0.01f;
                }
                Main.dust[num288].noGravity = true;
                dust199 = Main.dust[num288];
                dust334 = dust199;
                dust334.velocity *= 5f;
                dust199 = Main.dust[num288];
                dust334 = dust199;
                dust334.velocity -= projectile.oldVelocity / 5f;
            }



            projectile.ai[1] = -40;
            projectile.netUpdate = true;
            base.OnSpawn(projectile, source);
        }
    }
}
