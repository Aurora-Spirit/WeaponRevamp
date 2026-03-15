using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class SpelunkerFlareProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.SpelunkerFlare);
        AIType = ProjectileID.SpelunkerFlare;
        DrawOriginOffsetY = -10;

    }


    public override void AI()
    {
        Lighting.AddLight(Projectile.position + Vector2.UnitY.RotatedBy(Projectile.rotation)*16, new Vector3(5f,5f,0.2f));
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.Next(3) == 0)
        {
            target.AddBuff(BuffID.Midas, 600);
        }
        else
        {
            target.AddBuff(BuffID.Midas, 300);
        }
    }
}