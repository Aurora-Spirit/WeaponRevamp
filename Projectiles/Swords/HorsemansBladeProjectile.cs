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
    public class HorsemansBladeProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.TheHorsemansBlade;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            projectile.scale *= 1.2f;

            projectile.netUpdate = true;

        }
    }
}
