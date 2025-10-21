using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using WeaponRevamp.Dusts;


namespace WeaponRevamp.Projectiles.Guns.UnifiedBullet
{
    public class ChlorophyteHoming : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (projectile.penetrate != projectile.maxPenetrate)
            {
                projectile.netUpdate = true;
            }
            int targetIndex = projectile.FindTargetWithLineOfSight(160);
            if (targetIndex >= 0 && Main.npc[targetIndex] != null && projectile.ai[1] > 7)
            {
                NPC target = Main.npc[targetIndex];
                Vector2 homingDirection = Vector2.Normalize(target.Center - projectile.position);
                /*projectile.velocity += homingDirection * 2f;
                projectile.velocity *= 0.94f;*/
                projectile.netUpdate = true;
                //experimental - this causes the projectile to loop widely around the enemy! fun!
                Vector2 homingStrength = homingDirection * projectile.velocity.Length() * 0.15f;
                projectile.velocity += homingStrength;
                projectile.velocity *= 0.97f;
                if (projectile.velocity.Length() > 16f)
                {
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * 16f;
                }
                
                //experimental 2 - this causes the projectile to orbit the target in a circle
                /*Vector2 homingStrength = homingDirection * 3f;
                if (Vector2.Dot(projectile.velocity, homingDirection) < 0)
                {
                    projectile.velocity *= 0.95f;
                    projectile.velocity += homingStrength;
                }*/
                
                //experimental 3 - causes bullets to miss when in range sometimes.
                /*Vector2 homingStrength = homingDirection * (0.9f-Vector2.Dot(Vector2.Normalize(projectile.velocity), homingDirection))*0.5f;
                projectile.velocity += homingStrength;
                projectile.velocity *= 0.98f;*/
                
                //experimental 4 - projectile never hits and tbh it just looks worse
                /*Vector2 homingStrength = homingDirection * (160-Vector2.Distance(projectile.position, target.position)) / 160f * 3f;
                projectile.velocity += homingStrength;*/
                
                //experimental 5 - also spirals the enemy
                /*float initialVelocity = projectile.velocity.Length();
                Vector2 homingStrength = homingDirection * 2f;
                if (Vector2.Dot(projectile.velocity, homingDirection) < 0.5)
                {
                    projectile.velocity += homingStrength;
                    if (projectile.velocity.Length() > initialVelocity)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity)*initialVelocity;
                    }

                    projectile.velocity *= 0.98f;
                }*/
                
                
                for (int i = 0; i < homingStrength.Length()*0.4f; i++)
                {
                    float distanceToTarget = Main.rand.NextFloat()*0.5f;
                    Dust dust = Dust.NewDustDirect(Vector2.Lerp(projectile.position,target.Center,distanceToTarget), 0, 0,DustID.ChlorophyteWeapon);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += homingDirection*4f*distanceToTarget;
                    dust.velocity += projectile.velocity * 0.2f;
                    dust.position += projectile.velocity * i / homingStrength.Length();

                }
            }
        }

        /*public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            //this is to prevent it from being op with pierce
            projectile.damage = (int)(projectile.damage * 0.5f);
        }*/
    }
    
    public class ChlorophyteTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.0f, 0.5f, 0.1f, 0.35f);
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        public override void AI(ref Projectile projectile)
        {
            
            if (projectile.ai[1] > 5)
            {
                
                
                int dustCount = (int)(projectile.velocity.Length() * 0.3f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, dustColor);
                    smoke.velocity *= 0f;
                    smoke.position -= projectile.velocity * 2f;
                    smoke.position += projectile.velocity * i / dustCount;
                    smoke.velocity += projectile.velocity * 0.1f;
                    //smoke.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
                    smoke.rotation = (projectile.ai[1]+i / dustCount) * -0.5f * Math.Sign(projectile.velocity.X) + Main.rand.NextFloat()*0.1f;
                    //smoke.fadeIn = 0.15f * Math.Abs(Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.UnitY.RotatedBy(smoke.rotation))) + 0.00f;
                    smoke.fadeIn = 0.1f + Main.rand.NextFloat()*0.05f;
                    smoke.scale = 0.6f + Main.rand.NextFloat()*0.2f;
                    
                    Dust flame = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2),0,0,DustID.CursedTorch);
                    flame.velocity *= 0.5f;
                    flame.velocity += projectile.velocity * 0.0f;
                    flame.noGravity = true;
                    flame.scale = 0.5f;
                    flame.position -= projectile.velocity * 2f;
                    flame.position += projectile.velocity * i / dustCount;
                    flame.noLight = true;
                }
                
                
            }
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            if (projectile.ai[1] > 5)
            {
                int dustCount = (int)(projectile.velocity.Length() * 0.3f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.oldPosition + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, dustColor);
                    smoke.velocity *= 0f;
                    smoke.position -= projectile.oldVelocity * 1f;
                    smoke.position += projectile.oldVelocity * i / dustCount;
                    smoke.velocity += projectile.oldVelocity * 0.1f;
                    //smoke.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
                    smoke.rotation = (projectile.ai[1]+i / dustCount) * -0.5f * Math.Sign(projectile.oldVelocity.X) + Main.rand.NextFloat()*0.1f;
                    //smoke.fadeIn = 0.15f * Math.Abs(Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.UnitY.RotatedBy(smoke.rotation))) + 0.00f;
                    smoke.fadeIn = 0.1f + Main.rand.NextFloat()*0.05f;
                    smoke.scale = 0.6f + Main.rand.NextFloat()*0.2f;
                }
                
                
            }
        }
    }
    
}

