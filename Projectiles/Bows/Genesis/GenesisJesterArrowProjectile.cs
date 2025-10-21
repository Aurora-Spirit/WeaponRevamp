using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisJesterArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 240; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 240;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        // Custom AI
        public override void AI()
        {
            //Lighting.AddLight(Projectile.position, new Vector3(1f, 1f, 1f));
            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
            }
            Projectile.ai[0]++;
            
            /*if (Projectile.ai[0] > 15)
            {
                Projectile.velocity.Y += 0.07f;
            }*/
            //if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;
            //Projectile.rotation = Projectile.velocity.ToRotation();
            int dustType = Main.rand.Next(3);
            Dust stardust = Dust.NewDustDirect(Projectile.position, 0, 0, dustType switch
            {
                0 => 15,
                1 => 57,
                _ => 58,
            });
            stardust.scale += (240f - Projectile.timeLeft) / 80f - 0.5f;
            stardust.velocity *= 0.3f;
            stardust.velocity += Projectile.velocity * 0.6f;

            Dust traildust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
            traildust.scale += (240f-Projectile.timeLeft)/80f - 0.5f;
            traildust.velocity *= 0.05f;
            traildust.velocity += Projectile.velocity * 1f;
            traildust.noGravity = true;
            if (Main.rand.NextBool(10))
            {
                Dust gravdust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                gravdust.scale += (240f - Projectile.timeLeft) / 80f - 0.5f;
                gravdust.velocity *= 2f;
                gravdust.velocity += Projectile.velocity * 1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                int dustType = Main.rand.Next(3);
                Dust starDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType switch
                {
                    0 => 15,
                    1 => 57,
                    _ => 58,
                });
                starDust.scale += Main.rand.NextFloat();
                starDust.velocity += Projectile.velocity * 0.6f;
                starDust.velocity += target.velocity * 0.2f;

                Dust traildust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                traildust.scale += Main.rand.NextFloat();
                traildust.velocity *= 2f;
                traildust.velocity += Projectile.velocity * 0.5f;
                traildust.velocity += target.velocity * 0.3f;

                Dust burstdust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                burstdust.scale += Main.rand.NextFloat();
                burstdust.velocity *= 2f;
                burstdust.velocity += Projectile.velocity * 0.5f;
                burstdust.velocity += target.velocity * 0.3f;
                burstdust.noGravity = true;

            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dustType = Main.rand.Next(3);
                Dust starDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType switch
                {
                    0 => 15,
                    1 => 57,
                    _ => 58,
                });
                starDust.scale += Main.rand.NextFloat() * 2;
                starDust.velocity += Projectile.oldVelocity * 0.6f;

                Dust traildust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                traildust.scale += Main.rand.NextFloat();
                traildust.velocity *= 1.5f;
                traildust.velocity += Projectile.oldVelocity * -1f;

                Dust burstdust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                burstdust.scale += Main.rand.NextFloat() * 2;
                burstdust.velocity *= 3f;
                burstdust.velocity += Projectile.oldVelocity * 1f;
                burstdust.noGravity = true;

            }
            base.OnKill(timeLeft);
        }
        

    }
}
