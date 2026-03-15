using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged.Flares;

public class IchorFlareProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Projectile.netImportant = true;
        Projectile.width = 6;
        Projectile.height = 6;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.alpha = 255;
        Projectile.timeLeft = 36000;
        DrawOriginOffsetY = -10;

    }


    public override void AI()
    {
        //vanilla flare ai
        if (Projectile.alpha > 0)
        {
            Projectile.alpha -= 50;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }
        float num279 = 4f;
        float num280 = Projectile.ai[0];
        float num281 = Projectile.ai[1];
        if (num280 == 0f && num281 == 0f)
        {
            num280 = 1f;
        }
        float num282 = (float)Math.Sqrt(num280 * num280 + num281 * num281);
        num282 = num279 / num282;
        num280 *= num282;
        num281 *= num282;
        if (Projectile.alpha < 70)
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 2f), 6, 6, DustID.Ichor, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1f);
            Main.dust[dust].velocity *= 0.5f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position.X -= num280 * 1f;
            Main.dust[dust].position.Y -= num281 * 1f;
            Main.dust[dust].velocity.X -= num280;
            Main.dust[dust].velocity.Y -= num281;
        }

        if (Projectile.localAI[0] == 0f)
        {
            Projectile.ai[0] = Projectile.velocity.X;
            Projectile.ai[1] = Projectile.velocity.Y;
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 30f)
            {
                Projectile.velocity.Y += 0.09f;
                Projectile.localAI[1] = 30f;
            }
        }
        else
        {
            if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.localAI[0] = 0f;
                Projectile.localAI[1] = 30f;
            }
            Projectile.damage = 0;
        }
        if (Projectile.velocity.Y > 16f)
        {
            Projectile.velocity.Y = 16f;
        }
        Projectile.rotation = (float)Math.Atan2(Projectile.ai[1], Projectile.ai[0]) + 1.57f;
        
        //light
        Lighting.AddLight(Projectile.position + Vector2.UnitY.RotatedBy(Projectile.rotation)*16, new Vector3(5.5f,3f,0.4f));
        
        
        
        //Shimmer interaction
        if (Projectile.shimmerWet)
        {
            int num3 = (int)(Projectile.Center.X / 16f);
            int num4 = (int)(Projectile.position.Y / 16f);
            if (WorldGen.InWorld(num3, num4) && Main.tile[num3, num4] != null && Main.tile[num3, num4].LiquidAmount == byte.MaxValue && Main.tile[num3, num4].LiquidType == LiquidID.Shimmer && WorldGen.InWorld(num3, num4 - 1) && Main.tile[num3, num4 - 1] != null && Main.tile[num3, num4 - 1].LiquidAmount > 0 && Main.tile[num3, num4 - 1].LiquidType == LiquidID.Shimmer)
            {
                Projectile.Kill();
            }
            else if (Projectile.velocity.Y > 0f)
            {
                Projectile.velocity.Y *= -1f;
                Projectile.netUpdate = true;
                Projectile.shimmerWet = false;
                Projectile.wet = false;
            }
        }
        
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        //movement handling
        if (Projectile.localAI[0] == 0f)
        {
            if (Projectile.wet)
            {
                Projectile.position += Projectile.oldVelocity / 2f;
            }
            else
            {
                Projectile.position += Projectile.oldVelocity;
            }
            Projectile.velocity *= 0f;
            Projectile.localAI[0] = 1f;
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.Next(3) == 0)
        {
            target.AddBuff(BuffID.Ichor, 600);
        }
        else
        {
            target.AddBuff(BuffID.Ichor, 300);
        }
    }
}