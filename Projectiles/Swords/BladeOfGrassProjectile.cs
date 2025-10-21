using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.Swords
{
    public class BladeOfGrassProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.BladeOfGrass;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            projectile.damage = (int)((float)projectile.damage * 0.8f);

            projectile.netUpdate = true;

        }
        public override void PostAI(Projectile projectile)
        {
            projectile.velocity *= 0.8f;
        }
    }
}
