using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.Staves
{
    public class RubyStaffProjectile : GlobalProjectile
    {

        public override bool AppliesToEntity(Projectile proj, bool lateInstatiation)
        {
            return proj.type == ProjectileID.RubyBolt;
        }

        public override bool PreAI(Projectile projectile)
        {
            for (int num256 = 0; num256 < 2; num256++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position/* + new Vector2(projectile.width/2,projectile.height/2)*/, 0, 0, DustID.GemRuby, 0, 0, 0, default(Color), 1f);
                dust.scale = Main.rand.NextFloat()*0.5f + 1.2f;
                dust.velocity *= 0.5f;
                dust.velocity += projectile.velocity * 0.8f;
                dust.noGravity = !Main.rand.NextBool(10);
                if(!dust.noGravity) dust.velocity.Y -= 1f;
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
            for (int num256 = 0; num256 < 20; num256++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position/* + new Vector2(projectile.width/2,projectile.height/2)*/, 0, 0, DustID.GemRuby, 0, 0, 50, default(Color), 1.6f);
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust.velocity += projectile.oldVelocity * 0.8f;
                dust.noGravity = !Main.rand.NextBool(10);
                if (!dust.noGravity) dust.velocity.Y -= 1f;
            }
            return false;
        }

    }
}
