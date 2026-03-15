using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class ShimmerFlareProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.ShimmerFlare);
        AIType = ProjectileID.ShimmerFlare;
        DrawOriginOffsetY = -10;

    }


    public override void AI()
    {
        Lighting.AddLight(Projectile.position + Vector2.UnitY.RotatedBy(Projectile.rotation)*16, new Vector3(5f,0.8f,5f));
    }
    

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (oldVelocity.Length() > 5f)
        {
            if (Projectile.velocity.X < oldVelocity.X)
            {
                Projectile.velocity = (Vector2.UnitX * -0.80f * oldVelocity.Length()).RotatedByRandom(Math.PI/1.5f);
            } else if (Projectile.velocity.X > oldVelocity.X)
            {
                Projectile.velocity = (Vector2.UnitX * 0.80f * oldVelocity.Length()).RotatedByRandom(Math.PI/1.5f);
            } else if (Projectile.velocity.Y > oldVelocity.Y)
            {
                Projectile.velocity = (Vector2.UnitY * 0.80f * oldVelocity.Length()).RotatedByRandom(Math.PI/1.5f);
            } else if (Projectile.velocity.Y < oldVelocity.Y)
            {
                Projectile.velocity = (Vector2.UnitY * -0.80f * oldVelocity.Length()).RotatedByRandom(Math.PI/1.5f);
            }
            return false;
        }
        else
        {
            return base.OnTileCollide(oldVelocity);
        }
    }
}