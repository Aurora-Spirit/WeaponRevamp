using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using Microsoft.Xna.Framework;

namespace WeaponRevamp.Projectiles.Boomerangs
{
    public class WoodenBoomerangProjectile : BoomerangBaseAI
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.WoodenBoomerang;
        }


    }
}