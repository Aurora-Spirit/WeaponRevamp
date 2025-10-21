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
    public class BlizzardStaffProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.Blizzard;
        }
        public override void PostAI(Projectile projectile)
        {

            for(int i=0;i<3;i++)
            {
                int dust = Dust.NewDust(new Vector2(projectile.Center.X - 120, projectile.Center.Y - 120), 240, 240, DustID.SnowSpray);
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].velocity += projectile.velocity;
                Main.dust[dust].velocity.X += Main.windSpeedCurrent * 15;
                Main.dust[dust].noGravity = true;
            }


        }
    }
}
