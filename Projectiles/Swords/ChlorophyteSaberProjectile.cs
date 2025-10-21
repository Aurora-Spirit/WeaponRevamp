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
    public class ChlorophyteSaberProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.SporeCloud;
        }
        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.scale = 2;
        }
    }
}
