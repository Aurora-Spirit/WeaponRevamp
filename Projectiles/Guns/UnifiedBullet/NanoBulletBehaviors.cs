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
    
    public class NanoSmokeTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.2f, 0.4f, 0.5f, 0.35f);
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

    public class Richochet : BulletBehavior
    {
        private bool netUpdateNextTick = false;
        public override void AI(ref Projectile projectile)
        {
            if (netUpdateNextTick)
            {
                projectile.netUpdate = true;
                netUpdateNextTick = false;
            }
            base.AI(ref projectile);
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            //projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            NPC newTarget = projectile.FindTargetWithinRange(800, true);
            if (newTarget != null && newTarget.active)
            {
                float speed = projectile.velocity.Length();
                projectile.velocity = Vector2.Normalize(newTarget.Center - projectile.Center) * speed;
                
            }
            
            //makes em a little less op with guns that boost pierce
            projectile.damage = (int)(projectile.damage * 0.95f);
            
            Dust bigRing = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<RingBurstDust>());
            bigRing.scale = 0f;
            bigRing.velocity *= 0;
            bigRing.color = new Color(0.3f, 0.3f, 0.3f, 0);
            Dust smallRing = Dust.NewDustDirect(projectile.position, 0, 0, ModContent.DustType<RingBurstDust>());
            smallRing.scale = 0f;
            smallRing.velocity *= 0;
            smallRing.color = new Color(0.3f, 0.3f, 0.3f, 0);
            smallRing.fadeIn = 8;

            for (int i = 0; i < 10; i++)
            {
                Dust spark = Dust.NewDustDirect(projectile.position,0,0,DustID.Electric);
                spark.noGravity = true;
                spark.velocity *= 2f;
            }

            netUpdateNextTick = true;
        }
    }

}

