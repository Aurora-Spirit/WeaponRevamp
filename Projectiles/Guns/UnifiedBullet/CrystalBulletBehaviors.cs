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
    public class AirResistance : BulletBehavior
    {
        private float resistance = 1f;

        public AirResistance(float resistance)
        {
            this.resistance = resistance;
        }
        public override void AI(ref Projectile projectile)
        {
            projectile.velocity *= resistance;
            projectile.oldVelocity *= resistance;
        }
    }

    public class ShardTrail : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 0, 0, DustID.BlueCrystalShard+Main.rand.Next(0,2)*2);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += projectile.velocity * 0.25f;
                
            }
        }
    }
    
    public class ShardRender : BulletBehavior
    {
        private Color shineColor;
        private Texture2D sprite;
        private Color glowColor;

        public ShardRender(Color shineColor, Color glowColor, Texture2D sprite)
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
                Main.EntitySpriteDraw(sprite, position, sprite.Frame(1, 1, 0, 0), new Color(shineColor.R, shineColor.G, shineColor.B, 128), projectile.rotation, new Vector2(5f,3f), 1.3f, SpriteEffects.None, 0f);
                
            }
        }
    }
    
    public class CrystalShatter : BulletBehavior
    {
        private int latestHit;

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            latestHit = target.whoAmI;
            projectile.penetrate += 1;
            projectile.Kill();
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            
            if (projectile.velocity.X != projectile.oldVelocity.X)
            {
                projectile.velocity.X = projectile.oldVelocity.X * -1;
            }
            if (projectile.velocity.Y != projectile.oldVelocity.Y)
            {
                projectile.velocity.Y = projectile.oldVelocity.Y * -1;
            }
            int shardCount = Main.rand.Next(3,5);
            shardCount += projectile.maxPenetrate-1;
            float spreadWidth = (float)Math.PI * 0.2f;
            for (float i = 0; i < shardCount; i++)
            {
                float angle = (i/(shardCount-1))*spreadWidth - (spreadWidth/2);
                Vector2 velocity = projectile.velocity.RotatedBy(angle)*1f;
                velocity = Vector2.Normalize(velocity)*10f;
                if (Main.myPlayer == projectile.owner)
                {
                    Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.position, velocity, projectile.type, (int)(projectile.damage*0.5f), projectile.knockBack, projectile.owner, ProjectileID.CrystalShard,0,Main.rand.NextFloat());

                    proj.ArmorPenetration = projectile.ArmorPenetration + 35;
                    if (latestHit != null)
                    {
                        proj.localNPCImmunity[latestHit] = 12;
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 0, 0, DustID.BlueCrystalShard+Main.rand.Next(0,2)*2);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.velocity += projectile.velocity * 0.2f;
            }
        }
    }
    
    public class CrystalTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.0f, 0.2f, 0.5f, 0.35f);

        private float offset = 0;
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] == 0)
            {
                offset = Main.rand.NextFloat()*(float)Math.PI*2;
            }
            dustColor = new Color((float)Math.Sin(projectile.ai[1]*0.2f+offset)*0.7f, 0.3f, 0.7f, 0.35f);
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

