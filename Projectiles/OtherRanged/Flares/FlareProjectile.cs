using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class FlareProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
    {

        return projectile.type == ProjectileID.Flare;

    }
    public override void AI(Projectile projectile)
    {
        Lighting.AddLight(projectile.position + Vector2.UnitY.RotatedBy(projectile.rotation)*16, new Vector3(6f,1f,0.4f));
    }
    
    
    
}