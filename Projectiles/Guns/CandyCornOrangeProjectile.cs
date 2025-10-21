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
using WeaponRevamp.Dusts;
using WeaponRevamp.Gores;

namespace WeaponRevamp.Projectiles.Guns
{
    public class CandyCornOrangeProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.penetrate = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.shimmerWet)
            {
                Projectile.velocity.Y = -Math.Abs(Projectile.velocity.Y);
            }
            Lighting.AddLight(Projectile.Center, 1f * 0.3f, 0.8f * 0.3f, 0.6f * 0.3f);
            int num240 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.6f);
            Main.dust[num240].noGravity = true;
            num240 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
            Main.dust[num240].noGravity = true;
            Projectile.rotation += Projectile.velocity.X * 0.1f + (float)Main.rand.Next(-10, 11) * 0.025f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 7;
            target.AddBuff(BuffID.OnFire3, 60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Item14, Projectile.position);

            
            Shockwave.NewShockwave(Projectile.Center, 32, 18, new Color(255, 150, 100, 128));

            for (int num731 = 0; num731 < 10; num731++)
            {
                int num732 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
            }
            for (int num733 = 0; num733 < 5; num733++)
            {
                int num734 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num734].noGravity = true;
                Dust dust144 = Main.dust[num734];
                Dust dust334 = dust144;
                dust334.velocity *= 3f;
                num734 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
                dust144 = Main.dust[num734];
                dust334 = dust144;
                dust334.velocity *= 2f;
            }
            int num735 = Gore.NewGore(null, Projectile.position, default(Vector2), Main.rand.Next(61, 64));
            Gore gore36 = Main.gore[num735];
            Gore gore64 = gore36;
            gore64.velocity *= 0.4f;
            Main.gore[num735].velocity.X += (float)Main.rand.Next(-10, 11) * 0.1f;
            Main.gore[num735].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.1f;
            num735 = Gore.NewGore(null, Projectile.position, default(Vector2), Main.rand.Next(61, 64));
            gore36 = Main.gore[num735];
            gore64 = gore36;
            gore64.velocity *= 0.4f;
            Main.gore[num735].velocity.X += (float)Main.rand.Next(-10, 11) * 0.1f;
            Main.gore[num735].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.1f;
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.penetrate = -1;
                Projectile.position.X += Projectile.width / 2;
                Projectile.position.Y += Projectile.height / 2;
                Projectile.width = 64;
                Projectile.height = 64;
                Projectile.position.X -= Projectile.width / 2;
                Projectile.position.Y -= Projectile.height / 2;
                Projectile.Damage();
            }
            base.OnKill(timeLeft);
        }



    }
}
