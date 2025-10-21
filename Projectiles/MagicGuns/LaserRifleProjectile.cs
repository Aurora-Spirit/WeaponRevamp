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
    public class LaserRifleProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.penetrate = -1;
            entity.usesLocalNPCImmunity = true;
            entity.localNPCHitCooldown = 12;
        }
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.PurpleLaser;
        }
        public override bool PreAI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 2f)
            {
                int totalTrailDusts = 10;
                for (int i = 0; i < totalTrailDusts; i++){
                    Dust trailDust = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<LaserRifleDust>());
                    trailDust.position += projectile.velocity / totalTrailDusts * i;
                    trailDust.noGravity = true;
                    trailDust.scale = 1f + ((float)i/(float)totalTrailDusts)*0.1f;
                    trailDust.velocity *= 0f;
                }

                if (Main.rand.NextBool(3))
                {
                    CreateHitEffect(projectile.oldPosition + projectile.oldVelocity, projectile.oldVelocity.RotatedByRandom(Math.PI) * 0.5f);
                }
                /*int totalRingDusts = 20;
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
                }*/
            }
            return false;


        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                CreateHitEffect(projectile.oldPosition + projectile.oldVelocity, projectile.oldVelocity.RotatedByRandom(Math.PI) * 2f);
            }
            base.OnKill(projectile, timeLeft);
        }
        
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            projectile.damage = (int)((float)projectile.damage*0.80f );
            NPC newTarget = projectile.FindTargetWithinRange(128, true);
            if (newTarget != null)
            {
                projectile.velocity = Vector2.Normalize(newTarget.Center - projectile.position) * projectile.velocity.Length();
                projectile.netUpdate = true;
                for (int i = 0; i < 2; i++)
                {
                    CreateHitEffect(projectile.oldPosition + projectile.oldVelocity, projectile.oldVelocity.RotatedByRandom(Math.PI) * 1f);
                }
            }
            else
            {
                projectile.Kill();
            }
            
            base.OnHitNPC(projectile, target, hit, damageDone);
        }

        private void CreateHitEffect(Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < velocity.Length(); i+=2)
            {
                Dust trailDust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<LaserRifleDust>());
                trailDust.position += velocity / velocity.Length() * i;
                trailDust.noGravity = true;
                trailDust.scale = 1f;
                trailDust.velocity *= 0f;
            }

            if (velocity.Length() > 2)
            {
                if (Main.rand.NextBool(5, 6))
                {
                    for (int i = 0; i < Main.rand.Next(1, 3); i++)
                    {
                        CreateHitEffect(position+velocity, velocity.RotatedByRandom(Math.PI * 0.7f) * (Main.rand.NextFloat()*0.3f+0.5f));
                    }
                    
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            
            
            return true;
        }
        //public override bool InstancePerEntity => true;
    }
}
