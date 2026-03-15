using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class CursedFlareProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
    {

        return projectile.type == ProjectileID.CursedFlare;

    }
    public override void AI(Projectile projectile)
    {
        Lighting.AddLight(projectile.position + Vector2.UnitY.RotatedBy(projectile.rotation)*16, new Vector3(4f,6f,1f));
    }
    
    
    
}