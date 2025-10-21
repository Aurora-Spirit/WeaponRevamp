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

    public class ElectricBurst : BulletBehavior
    {
        private int range;
        private int count;
        private bool arcing = false;
        private SoundStyle sound;

        public ElectricBurst(int range, int count, SoundStyle sound)
        {
            this.range = range;
            this.count = count;
            this.sound = sound;
        }
        

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            if (!arcing)
            {
                for (int i = 0; i < count; i++)
                {
                    Arc(ref projectile);
                }
            }
        }

        private void Arc(ref Projectile projectile)
        {
            arcing = true;
            SoundEngine.PlaySound(sound, projectile.position);
            Vector2 tempPosition = projectile.position;
            int tempPenetrate = projectile.penetrate;
            int tempDamage = projectile.damage;
            projectile.penetrate = -1;
            projectile.damage = (int)(projectile.damage * 0.7f);
            NPC target = projectile.FindTargetWithinRange(range, true);
            if (target != null && target.active)
            {
                Vector2 arcAngle = target.Center - projectile.Center;
                Vector2[] arcPoints = new Vector2[5];
                arcPoints[0] = projectile.Center;
                arcPoints[4] = target.Center;
                arcPoints[2] = Vector2.Lerp(arcPoints[0], arcPoints[4], 0.5f) + Vector2.Normalize(arcAngle.RotatedBy(Math.PI/2))*Main.rand.NextFloat(-1f,1f)*32f;
                arcPoints[1] = Vector2.Lerp(arcPoints[0], arcPoints[2], 0.5f) + Vector2.Normalize(arcAngle.RotatedBy(Math.PI/2))*Main.rand.NextFloat(-1f,1f)*16f;
                arcPoints[3] = Vector2.Lerp(arcPoints[2], arcPoints[4], 0.5f) + Vector2.Normalize(arcAngle.RotatedBy(Math.PI/2))*Main.rand.NextFloat(-1f,1f)*16f;
                for (int i = 0; i < arcPoints.Length - 1; i++)
                {
                    CreateZapEffect(arcPoints[i],Vector2.UnitX.RotatedByRandom(Math.PI*2)*10f);
                    for (int j = 0; j < 10; j++)
                    {
                        Vector2 position = Vector2.Lerp(arcPoints[i],arcPoints[i+1],j/10f);
                        Dust lightning = Dust.NewDustDirect(position, 0,0,ModContent.DustType<ElectricBulletDust>());
                        lightning.noGravity = true;
                        lightning.velocity *= 0f;
                        lightning.scale *= 0.7f;
                        //lightning.velocity = (arcPoints[i + 1] - arcPoints[i])*0.1f;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    CreateZapEffect(target.Center,Vector2.UnitX.RotatedByRandom(Math.PI*2)*15f);
                }
                
                projectile.Center = target.Center;
                projectile.Damage();
                for (int i = 0; i < 5; i++)
                {
                    Dust lightning = Dust.NewDustDirect(projectile.Center, 0,0,DustID.Electric);
                    lightning.velocity *= 2f;
                    lightning.noGravity = false;
                    lightning.scale *= 0.5f;
                }
            }
            
            

            projectile.penetrate = tempPenetrate;
            projectile.position = tempPosition;
            projectile.damage = tempDamage;
            for (int i = 0; i < 2; i++)
            {
                CreateZapEffect(projectile.Center,Vector2.UnitX.RotatedByRandom(Math.PI*2)*20f);
            }
            

            for (int i = 0; i < 5; i++)
            {
                Dust lightning = Dust.NewDustDirect(projectile.Center, 0,0,DustID.Electric);
                lightning.velocity *= 2f;
                lightning.noGravity = false;
                lightning.scale *= 0.5f;
            }

            if (projectile.penetrate <= 1)
            {
                projectile.Kill();
            }
            arcing = false;
            




        }
        
        private void CreateZapEffect(Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < velocity.Length(); i+=2)
            {
                Dust trailDust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<ElectricBulletDust>());
                trailDust.position += velocity / velocity.Length() * i;
                trailDust.noGravity = true;
                trailDust.scale = 0.7f;
                trailDust.velocity *= 0f;
            }

            if (velocity.Length() > 2)
            {
                if (Main.rand.NextBool(5, 6))
                {
                    for (int i = 0; i < Main.rand.Next(1, 3); i++)
                    {
                        CreateZapEffect(position+velocity, velocity.RotatedByRandom(Math.PI * 0.7f) * (Main.rand.NextFloat()*0.3f+0.5f));
                    }
                    
                }
            }
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            if (!arcing)
            {
                for (int i = 0; i < count; i++)
                {
                    Arc(ref projectile);
                }
            }
        }
    }
    
    public class ElectricTrail : BulletBehavior
    {
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        private float frequency;
        private float magnitude;
        public ElectricTrail()
        {
            frequency = Main.rand.NextFloat(0.8f,1.5f);
            magnitude = Main.rand.NextFloat(1f,2f);


        }
        

        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 5)
            {
                float lightningAngle = (projectile.ai[1]*(int)(projectile.velocity.Length())%(100*frequency))*0.01f*4;
                if (lightningAngle >= 2)
                {
                    lightningAngle = 4 - lightningAngle;
                }

                lightningAngle -= 0.8f;
                
                int dustCount = (int)(projectile.velocity.Length() * 0.2f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, DustID.Electric);
                    smoke.position -= projectile.velocity * 2f;
                    smoke.position += projectile.velocity * i / dustCount;
                    smoke.velocity = projectile.velocity.RotatedBy(Math.PI/2)*lightningAngle*0.2f*magnitude;
                    smoke.position -= smoke.velocity * 2f;
                    smoke.scale = Main.rand.NextFloat(0.5f);
                    smoke.noGravity = true;
                }
                
                
            }
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            
        }
    }
    
}

