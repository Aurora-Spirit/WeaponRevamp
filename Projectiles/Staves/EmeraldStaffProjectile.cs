using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.Staves
{
    public class EmeraldStaffProjectile : GlobalProjectile
    {

        public override bool AppliesToEntity(Projectile proj, bool lateInstatiation)
        {
            return proj.type == ProjectileID.EmeraldBolt;
        }

        public override bool PreAI(Projectile projectile)
        {
            for (int num256 = 0; num256 < 1; num256++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position/* + new Vector2(projectile.width/2,projectile.height/2)*/, 0, 0, DustID.GemEmerald, 0, 0, 50, default(Color), 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.5f;
            }
            for (int num256 = 0; num256 < 2; num256++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position/* + new Vector2(projectile.width/2,projectile.height/2)*/, 0, 0, DustID.GemEmerald, 0, 0, 50, default(Color), 0.8f);
                dust.noGravity = true;
                dust.velocity *= 0.05f;
                dust.velocity += projectile.velocity * 0.2f;
                dust.velocity += dust.velocity.RotatedBy(Math.PI / 2) * (Main.rand.NextFloat() * 2 - 1);
                dust.velocity += projectile.velocity * 0.5f;
            }
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                SoundEngine.PlaySound(in SoundID.Item8, projectile.position);
            }
            return false;
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, projectile.position);
            for (int num256 = 0; num256 < 10; num256++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position/* + new Vector2(projectile.width/2,projectile.height/2)*/, 0, 0, DustID.GemEmerald, 0, 0, 50, default(Color), 1.1f);
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust.velocity += projectile.oldVelocity * 0.5f;
            }
            return false;
        }

    }
}
