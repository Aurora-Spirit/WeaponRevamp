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
    public class LuminiteEyeHoming : BulletBehavior
    {
        public override bool? CanHitNPC(ref Projectile projectile, ref NPC target)
        {
            if (projectile.ai[1] < 30)
            {
                return false;
            }
            else
            {
                return base.CanHitNPC(ref projectile, ref target);
            }
        }

        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] < 30)
            {
                projectile.velocity += Vector2.Normalize(projectile.velocity)*0.1f;
                projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[2]);
                projectile.netUpdate = true;
            }
            else
            {
                int targetIndex = projectile.FindTargetWithLineOfSight(160);
                if (targetIndex >= 0 && Main.npc[targetIndex] != null)
                {
                    NPC target = Main.npc[targetIndex];
                    Vector2 homingDirection = Vector2.Normalize(target.Center - projectile.position);
                    
                    Vector2 homingStrength = homingDirection * projectile.velocity.Length() * 0.15f;
                    projectile.velocity += homingStrength;
                    projectile.velocity *= 0.97f;
                    projectile.netUpdate = true;

                }
            }
            
        }
    }

    public class EyeTrail : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, DustID.Vortex);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += projectile.velocity * 0.25f;
                
            }
        }
    }
    
    public class EyeRender : BulletBehavior
    {
        private Color shineColor;
        private Texture2D sprite;
        private Color glowColor;

        public EyeRender(Color shineColor, Color glowColor, Texture2D sprite)
        {
            this.shineColor = shineColor;
            this.sprite = sprite;
            this.glowColor = glowColor;

        }
        public override void AI(ref Projectile projectile)
        {
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
                Vector2 position = projectile.position - Main.screenPosition;
                Main.EntitySpriteDraw(sprite, position, sprite.Frame(1, 1, 0, 0), new Color(255,255,255,255), projectile.rotation, new Vector2(6f,3f), projectile.scale, SpriteEffects.None, 0f);
                
            }
        }
    }
    
    public class EyeSummonTrail : BulletBehavior
    {
        private float direction = (float)Math.PI/90;
        private int offset = 0;
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] == 0)
            {
                offset = Main.rand.Next(0, (int)(90/projectile.velocity.Length()));
                if (Main.rand.NextBool()) direction *= -1;
            }
            if (projectile.ai[1]>5 && projectile.ai[1] % (int)(90/projectile.velocity.Length()) == offset)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    Vector2 velocity = projectile.velocity*1f;
                    velocity = Vector2.Normalize(velocity)*1f;
                    Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.position, velocity, projectile.type, (int)(projectile.damage*0.2f), projectile.knockBack, projectile.owner, UnifiedBulletProjectile.luminiteEyes);
                    proj.ai[2] = direction;
                    direction *= -1;
                    proj.ArmorPenetration = projectile.ArmorPenetration + 50;
                }
            }
            
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            projectile.damage = (int)(projectile.damage * 0.9f);
        }
    }
    
    public class MoonlordTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.1f, 0.5f, 0.4f, 0.35f);
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

