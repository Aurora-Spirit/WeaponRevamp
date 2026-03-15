using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.Sentries;

public class FrostHydraIceChunk : ModProjectile
{
    public const int updatesPerTick = 2;
    public const float gravity = 0.1f;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.SentryShot[Type] = true;
    }

    public override void SetDefaults()
    {
        //SetDefaultDefaults();
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = 1;
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.aiStyle = 0;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.timeLeft = 600;
        Projectile.extraUpdates = updatesPerTick-1;
        Projectile.alpha = 255;
        Projectile.scale = 1f;
    }

    public override void AI()
    {
        Projectile.ai[0]++;
        if (Projectile.ai[0] > 0f)
        {
            Projectile.velocity.Y += gravity;
        }
        if (Projectile.alpha > 0)
        {
            Projectile.alpha -= 32;
        }

        if (Projectile.ai[0] == 1)
        {
            Projectile.localAI[0] = Main.rand.NextFloat(-1, 1);
        }

        Projectile.rotation += Projectile.localAI[0];
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.perIDStaticNPCImmunity[ModContent.ProjectileType<FrostHydraIceShard>()][target.whoAmI] = 12 + Main.GameUpdateCount;
    }

    public override void OnKill(int timeLeft)
    {
        
        SoundEngine.PlaySound(SoundID.Item50 with { Volume = 0.6f }, Projectile.Center);
        for (int i = 0; i < 3; i++)
        { //spawn ice shards
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(12, 0).RotatedByRandom(Math.PI*2), ModContent.ProjectileType<FrostHydraIceShard>(), Projectile.damage/2, Projectile.knockBack/2, Projectile.owner);
        }
        for (int i = 0; i < 15; i++)
        { //spawn snow dust
            Dust snow = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
            snow.noGravity = true;
            snow.velocity *= 3f;

        }
    }
}