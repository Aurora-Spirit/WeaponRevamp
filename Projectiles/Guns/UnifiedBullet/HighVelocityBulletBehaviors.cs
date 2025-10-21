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
    
    public class HighVelocityTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

        private float leftovers = 0;
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 4)
            {
                //int lineCount = (int)(projectile.velocity.Length() / 10f);
                int extraLifetime = (int)(projectile.ai[1] / projectile.MaxUpdates * 2)+5;
                float i = 0;
                for (i = leftovers; i < projectile.velocity.Length(); i+=16)
                {
                    Dust yellow = Dust.NewDustDirect(projectile.position + new Vector2(-2, -2), 0, 0, ModContent.DustType<HighVelocityDust>());
                    yellow.fadeIn = extraLifetime;
                    yellow.scale = 1f;
                    yellow.velocity *= 0f;
                    yellow.position += Vector2.Normalize(projectile.velocity) * i;
                    yellow.rotation = projectile.velocity.ToRotation();
                }

                leftovers = i - projectile.velocity.Length();
            }
            
            if (projectile.ai[1] > 5)
            {
                
                int dustCount = (int)(projectile.velocity.Length() * 0.2f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, dustColor);
                    smoke.velocity *= 0f;
                    smoke.position -= projectile.velocity * 2f;
                    smoke.position += projectile.velocity * i / dustCount;
                    smoke.velocity -= projectile.velocity * 0.1f;
                    //smoke.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
                    smoke.rotation = (projectile.ai[1]+i / dustCount) * -0.5f * Math.Sign(projectile.velocity.X) + Main.rand.NextFloat()*0.1f;
                    //smoke.fadeIn = 0.15f * Math.Abs(Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.UnitY.RotatedBy(smoke.rotation))) + 0.00f;
                    smoke.fadeIn = 0.1f + Main.rand.NextFloat()*0.05f;
                    smoke.scale = 1f + Main.rand.NextFloat()*0.2f;
                    smoke.alpha = 512;
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
                    smoke.alpha = 512;
                }
                
                
            }
        }
    }
    
    public class HighVelocityHitSpark : BulletBehavior
    {
        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 0, 0, DustID.FireworksRGB,0,0,0,new Color(255,255,0),0.4f);
                dust.velocity += projectile.velocity * 0.7f;
                dust.noGravity = true;
            }
        }
    }
    
}

