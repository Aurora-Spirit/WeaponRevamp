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
using WeaponRevamp.Gores;


namespace WeaponRevamp.Projectiles.Guns.UnifiedBullet
{
    public class ExplodeInRange : BulletBehavior
    {
        private int range;
        private bool exploding = false;

        /*public override void SendExtraAI(ref BinaryWriter writer)
        {
            writer.Write(exploding);
        }

        public override void ReceiveExtraAI(ref BinaryReader reader)
        {
            exploding = reader.r;
        }*/

        public ExplodeInRange(int range)
        {
            this.range = range;
        }

        public override void AI(ref Projectile projectile)
        {
            NPC target = projectile.FindTargetWithinRange((int)(range*1.5));
            if (target != null && target.active)
            {
                if (projectile.penetrate <= 1)
                {
                    if (Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.Normalize(target.Center - projectile.Center)) > 0.6f || projectile.timeLeft <= 30)
                    {
                        //Main.NewText(Vector2.Dot(Vector2.Normalize(projectile.velocity),Vector2.Normalize(target.Center-projectile.Center)));
                        if (projectile.timeLeft > 30)
                        {
                            projectile.timeLeft = 30;
                        }
                        projectile.scale += 0.02f;
                        float multiplier = 7f/8f;
                        projectile.velocity *= multiplier;
                        projectile.oldVelocity *= multiplier;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.Normalize(target.Center - projectile.Center)) < 0f)
                    {
                        if (!exploding)
                        {
                            if (target.Hitbox.ClosestPointInRect(projectile.Center).Distance(projectile.Center) < range) //only explode if its gonna hit
                            {
                                projectile.penetrate -= 1;
                                projectile.netUpdate = true;
                                Explode(ref projectile);
                            }
                        }
                    }
                }
                
                
            }
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            if (!exploding)
            {
                Explode(ref projectile);
            }
            else
            {
                target.netUpdate = true;
                projectile.damage /= 2;
                projectile.netUpdate = true;
            }
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            if (!exploding)
            {
                Explode(ref projectile);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (exploding)
            {
                return targetHitbox.ClosestPointInRect(projHitbox.Center.ToVector2()).Distance(projHitbox.Center.ToVector2()) < range;
            }
            else
            {
                return null;
            }
        }

        public void Explode(ref Projectile projectile)
        {
            exploding = true;
            projectile.netUpdate = true;
            int tempPenetrate = projectile.penetrate;
            int tempDamage = projectile.damage;
            projectile.penetrate = -1;
            projectile.Damage();
            projectile.penetrate = tempPenetrate;
            projectile.damage = tempDamage;
            if(projectile.penetrate == 0) projectile.Kill();
            exploding = false;
            SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            for (int i = 0; i < 20; i++)
            {
                Dust smoke = Dust.NewDustDirect(projectile.position, 0, 0, DustID.Smoke);
                smoke.scale *= Main.rand.NextFloat(1, 1.5f);
                smoke.velocity *= range / 50f;
                smoke.color *= Main.rand.NextFloat(0.2f,0.5f);
            }
            for (int i = 0; i < 12; i++)
            {
                Dust fire = Dust.NewDustDirect(projectile.position, 0, 0, DustID.Torch);
                fire.scale *= Main.rand.NextFloat(1, 2f);
                fire.velocity *= range / 20f;
            }

            for (int i = 0; i < Main.rand.Next(1, 3+1); i++)
            {
                Gore smokeGore = Gore.NewGoreDirect(projectile.GetSource_FromThis(),projectile.position,new Vector2(0,0),Main.rand.Next(GoreID.Smoke1,GoreID.Smoke3+1));
                smokeGore.velocity *= 0.7f;
                smokeGore.position -= new Vector2(smokeGore.Width / 2, smokeGore.Height / 2);
            }

            Shockwave.NewShockwave(projectile.Center, range, 18, new Color(255, 150, 100, 128));
        }
    }
    
    
    
    public class RedLightRender : BulletBehavior
    {
        private Color shineColor = new Color(255,255,255,128);
        private Texture2D sprite;
        private Color glowColor;

        public RedLightRender(Color glowColor, Texture2D sprite)
        {
            this.sprite = sprite;
            this.glowColor = glowColor;

        }
        public override void AI(ref Projectile projectile)
        {
            int blinkOnTime = 3 * projectile.MaxUpdates;
            int blinkTime = 6 * projectile.MaxUpdates;
            if (((int)projectile.ai[1])%blinkTime<blinkOnTime)
            {
                shineColor = new Color(255,0,0,128);
                Lighting.AddLight(projectile.Center, new Vector3(1,0,0));
            }
            else
            {
                shineColor = new Color(0,0,0,0);
            }
            /*if (projectile.ai[1] == 0)
            {
                shine = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2),0,0,ModContent.DustType<ShineDust>(),0,0,0,new Color(255,200,100));
            }*/
            //Lighting.AddLight(projectile.Center, new Vector3(0.7f,0.3f,0f));
            Lighting.AddLight(projectile.Center, glowColor.ToVector3());
        }
        
        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            Dust shine = Dust.NewDustDirect(projectile.position,0,0,ModContent.DustType<ShineDust>(),0,0,0,shineColor);
            shine.velocity *= 0f;
            shine.scale = 0.4f;
            shine.fadeIn = 0.2f;
            //shine.position -= projectile.oldVelocity;
        }
        public override void Draw(ref Projectile projectile, ref Color lightColor)
        {
            if (sprite != null) //should never happen, just in case...
            {
                Vector2 position = projectile.position + (Vector2.Normalize(projectile.velocity)*10f*projectile.scale) - Main.screenPosition + new Vector2(2,2);
                Main.EntitySpriteDraw(sprite, position, sprite.Frame(1, 1, 0, 0), shineColor, projectile.ai[1]*0.05f, new Vector2(7f,7f), 0.8f, SpriteEffects.None, 0f);
                
            }
        }
    }
    
    public class ExplosiveSmokeTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.4f, 0.3f, 0.2f, 0.4f);
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

