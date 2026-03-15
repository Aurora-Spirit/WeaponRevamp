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
using Microsoft.Xna.Framework.Graphics.PackedVector;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class SpaceGunProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.penetrate = -1;
        }
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.GreenLaser;
        }
        public override bool PreAI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 3f)
            {
                int totalTrailDusts = 4;
                for (int i = 0; i < totalTrailDusts; i++){
                    Dust trailDust = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<SpaceGunDust>());
                    trailDust.position += projectile.velocity / totalTrailDusts * i;
                    trailDust.noGravity = true;
                    trailDust.scale = 1f + ((float)i/(float)totalTrailDusts)*0.1f;
                    trailDust.velocity *= 0f;
                    trailDust.velocity += projectile.velocity * 0.1f;
                }

                int totalRingDusts = 20;
                if (projectile.localAI[0] % 5 == 0)
                {
                    for (int i = 1; i < totalRingDusts; i++)
                    {
                        Dust ringDust = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<SpaceGunDust>());
                        ringDust.rotation = (float)(Math.PI * 2)*(float)i/(float)totalRingDusts;
                        ringDust.scale = 1f;
                        ringDust.velocity = new Vector2(1,0).RotatedBy(ringDust.rotation);
                        //Main.NewText("Before: " + ringDust.velocity.Length());
                        ringDust.velocity.X *= 0.4f;
                        ringDust.fadeIn = ringDust.velocity.Length() * -1.5f;
                        //Main.NewText("After: " + ringDust.velocity.Length());

                        ringDust.velocity = ringDust.velocity.RotatedBy(projectile.rotation) * 6f;
                        ringDust.rotation = ringDust.velocity.ToRotation();

                    }
                }
            }
            return false;


        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            CreateHitEffect(projectile);
            base.OnKill(projectile, timeLeft);
        }
        
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[projectile.owner] = 2;
            CreateHitEffect(projectile);
            projectile.damage = (int)((float)projectile.damage*0.70f );
            
            base.OnHitNPC(projectile, target, hit, damageDone);
        }

        private void CreateHitEffect(Projectile projectile)
        {
            projectile.rotation = Main.rand.NextFloat() * (float)Math.PI * 2;
            int totalSmallRingDusts = 24;
            for (int i = 1; i < totalSmallRingDusts; i++)
            {
                Dust ringDust = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<SpaceGunDust>());
                ringDust.rotation = (float)(Math.PI * 2) * (float)i / (float)totalSmallRingDusts;
                ringDust.scale = 1f;
                ringDust.velocity = new Vector2(1, 0).RotatedBy(ringDust.rotation);
                //Main.NewText("Before: " + ringDust.velocity.Length());
                ringDust.velocity.X *= 0.2f + Main.rand.NextFloat() * 0.4f;
                ringDust.fadeIn = ringDust.velocity.Length() * -2f;
                //Main.NewText("After: " + ringDust.velocity.Length());

                ringDust.velocity = ringDust.velocity.RotatedBy(projectile.rotation) * 8f;
                ringDust.rotation = ringDust.velocity.ToRotation();

            }
            projectile.rotation = Main.rand.NextFloat()*(float)Math.PI*2;
            int totalLargeRingDusts = 32;
            for (int i = 1; i < totalLargeRingDusts; i++)
            {
                Dust ringDust = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<SpaceGunDust>());
                ringDust.rotation = (float)(Math.PI * 2)*(float)i/(float)totalLargeRingDusts;
                ringDust.scale = 1f;
                ringDust.velocity = new Vector2(1,0).RotatedBy(ringDust.rotation);
                //Main.NewText("Before: " + ringDust.velocity.Length());
                ringDust.velocity.X *= 0.2f + Main.rand.NextFloat()*0.4f;
                ringDust.fadeIn = ringDust.velocity.Length() * -2.5f;
                //Main.NewText("After: " + ringDust.velocity.Length());

                ringDust.velocity = ringDust.velocity.RotatedBy(projectile.rotation) * 10f;
                ringDust.rotation = ringDust.velocity.ToRotation();
                    
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            
            
            return true;
        }
        //public override bool InstancePerEntity => true;
    }
}
