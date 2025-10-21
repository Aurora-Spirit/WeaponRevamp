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
    public class SpectreStaffProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.LostSoulFriendly;
        }

        public override void SetDefaults(Projectile entity)
        {
            entity.tileCollide = false;
            base.SetDefaults(entity);
        }
        public override void PostAI(Projectile projectile)
        {
            /*NPC target = projectile.FindTargetWithinRange(400);
            if (target != null)
            {
                projectile.velocity -= Vector2.Normalize(projectile.position-target.position) * 0.2f;
            }*/
            Random rand = new Random();
            projectile.velocity.X += (float)rand.NextDouble() * 2 - 1;
            projectile.velocity.Y += (float)rand.NextDouble() * 2 - 1;

        }
    }
}
