using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.Sentries;

public class FrostHydraIceShard : ModProjectile
{
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
        Projectile.width = 8;
        Projectile.height = 12;
        Projectile.aiStyle = 0;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.timeLeft = 10;
        Projectile.scale = 1f;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 12;
        Projectile.appliesImmunityTimeOnSingleHits = true;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void OnKill(int timeLeft)
    {
        
        SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.2f }, Projectile.Center);
        for (int i = 0; i < 3; i++)
        { //spawn snow dust
            Dust snow = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
            snow.noGravity = true;
            snow.velocity *= 1f;
            snow.velocity += Projectile.velocity * 0.3f;
        }
    }
}