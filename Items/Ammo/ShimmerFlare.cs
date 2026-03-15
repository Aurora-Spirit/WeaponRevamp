using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using WeaponRevamp.Projectiles.OtherRanged.Flares;

namespace WeaponRevamp.Items.Ammo
{
    public class ShimmerFlare : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShimmerFlare;
        }

        public override void SetDefaults(Item entity)
        {
            entity.shoot = ModContent.ProjectileType<ShimmerFlareProjectile>();
            entity.damage = 4;
        }
    }
}
