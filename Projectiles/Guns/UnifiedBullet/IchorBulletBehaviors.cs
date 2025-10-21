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
    
    public class PissSmokeTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.5f, 0.2f, 0.0f, 0.2f);
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 5)
            {
                if (Main.rand.NextBool())
                {
                    Dust piss = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2),0,0,DustID.Ichor);
                    piss.velocity *= 0.3f;
                    piss.velocity += projectile.velocity * 0.1f;
                    piss.velocity += Vector2.UnitY * Main.rand.NextFloat() * -0.3f;
                    piss.scale = 0.5f;
                    piss.position -= projectile.velocity * 2f;
                    piss.noLight = true;
                }
                
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

    public class IchorSplatter : BulletBehavior
    {
        private int range;

        public IchorSplatter(int range)
        {
            this.range = range;
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 600);
            IchorExplosion(projectile);
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            IchorExplosion(projectile);
        }

        private void IchorExplosion(Projectile projectile)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i] != null && Main.npc[i].active)
                {
                    if (Main.npc[i].Center.Distance(projectile.position) < range)
                    {
                        Main.npc[i].AddBuff(BuffID.Ichor, 600);
                    }
                }
            }

            for (int i = 0; i < 30; i++)
            {
                Dust piss = Dust.NewDustDirect(projectile.position, 0, 0, DustID.Ichor);
                piss.velocity *= 1.5f;
                piss.noGravity = Main.rand.NextBool();
                piss.velocity += projectile.velocity * 0.1f;
                piss.scale = Main.rand.NextFloat(0.9f, 1.2f);
                if (!piss.noGravity) piss.scale *= 0.5f;
            }
        }
    }

}

