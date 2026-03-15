using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class BlueFlareProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.BlueFlare);
        AIType = ProjectileID.BlueFlare;
        DrawOriginOffsetY = -10;

    }


    public override void AI()
    {
        Lighting.AddLight(Projectile.position + Vector2.UnitY.RotatedBy(Projectile.rotation)*16, new Vector3(0.4f,1f,6f));
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.Next(3) == 0)
        {
            target.AddBuff(BuffID.Frostburn, 600);
        }
        else
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}