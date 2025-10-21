using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace WeaponRevamp.Projectiles.Guns
{
    public class CandyCornYellowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = 1;


        }

        public override void AI()
        {
            if (Projectile.shimmerWet)
            {
                Projectile.velocity.Y = -Math.Abs(Projectile.velocity.Y);
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f + (float)Main.rand.Next(-10, 11) * 0.025f;
            Projectile.velocity.Y += 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 7;
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            for (int num451 = 0; num451 < 5; num451++)
            {
                int num453 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 189);
                Main.dust[num453].scale = 0.85f;
                Main.dust[num453].noGravity = true;
                Dust dust247 = Main.dust[num453];
                Dust dust334 = dust247;
                dust334.velocity += Projectile.velocity * 0.5f;
            }
            base.OnKill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = 0f - Projectile.oldVelocity.X;
                Projectile.ai[1] += 1f;
            }
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = 0f - Projectile.oldVelocity.Y;
                Projectile.ai[1] += 1f;
            }
            if (Projectile.ai[1] > 4f)
            {
                Projectile.Kill();
            }
            return false;
        }



    }
}
