using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WeaponRevamp.Projectiles.Swords

{
    public class InfluxWaverLaserProjectile:ModProjectile
    {
        public override void SetDefaults(){
            Projectile.width=8;
            Projectile.height=8;
            Projectile.aiStyle=0;
            Projectile.friendly=true;
            Projectile.hostile=false;
            Projectile.penetrate=1;
            Projectile.timeLeft=600;
            Projectile.light = 0.2f;
            Projectile.ignoreWater=true;
            Projectile.tileCollide=true;
            Projectile.extraUpdates=100;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            /*int dustID = Dust.NewDust(Projectile.Center, 1, 1, DustID.InfluxWaver, 0f, 0f);
            Dust dust = Main.dust[dustID];
            dust.velocity *= 0.1f;*/
            for (int i = 0; i < 2; i++)
            {
                int dustType = Utils.SelectRandom<int>(Main.rand, 226, 229);
                Vector2 center19 = Projectile.position;
                Vector2 spinningpoint4 = new Vector2(-16f, 16f);
                float num716 = 1f;
                spinningpoint4 += new Vector2(-16f, 16f);
                spinningpoint4 = spinningpoint4.RotatedBy(Projectile.rotation);
                int num717 = 4;
                int num718 = Dust.NewDust(center19 + ((float)i * Projectile.velocity * 0.5f), num717 * 2, num717 * 2, dustType, 0f, 0f, 100, default(Color), num716);
                // + spinningpoint4 + Vector2.One * -num717
                Dust dust97 = Main.dust[num718];
                Dust dust212 = dust97;
                dust212.velocity *= 0.1f;
                dust212.velocity += Projectile.velocity * 0.25f;
                if (Main.rand.Next(6) != 0)
                {
                    Main.dust[num718].noGravity = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Swords.InfluxWaverSlashProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedBy(-0.5f), ModContent.ProjectileType<Projectiles.Swords.InfluxWaverSlashProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedBy(0.5f), ModContent.ProjectileType<Projectiles.Swords.InfluxWaverSlashProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}