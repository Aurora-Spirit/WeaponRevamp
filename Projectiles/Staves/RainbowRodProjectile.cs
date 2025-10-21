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
    public class RainbowRodProjectile : GlobalProjectile
    {
        double cycle = 0;
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.RainbowRodBullet;
        }
        public override void PostAI(Projectile projectile)
        {

            cycle += Math.PI/15;
            if (cycle >= Math.PI * 2)
            {
                cycle -= Math.PI * 2;
            }
            Random random = new Random();
            Vector2 addedVelocity = new Vector2((float)Math.Cos(cycle + projectile.ai[2]) * 5, (float)Math.Sin(cycle + projectile.ai[2]) * 5);
            Vector2 newVelocity = Vector2.Add(projectile.velocity, addedVelocity);
            projectile.velocity = newVelocity;


        }
        public override bool InstancePerEntity => true;
    }
}
