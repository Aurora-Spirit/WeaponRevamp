using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class RainbowFlareProjectile : ModProjectile
{

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.RainbowFlare);
        AIType = ProjectileID.RainbowFlare;
        DrawOriginOffsetY = -10;

    }


    public override void AI()
    {
        Color rainbowTorchColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.5f);
        Lighting.AddLight(Projectile.position + Vector2.UnitY.RotatedBy(Projectile.rotation)*16, rainbowTorchColor.ToVector3()*6f);
    }

    
}