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
    
    public class VenomSmokeTrail : BulletBehavior
    {
        private Color dustColor = new Color(0.4f, 0.0f, 0.5f, 0.35f);
        //private Color dustColor = new Color(0.6f, 0.0f, 0.2f, 0.35f);
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 5 && projectile.velocity.Length() > 1.5f)
            {
                Dust venom = Dust.NewDustDirect(projectile.position+ new Vector2(-2,-2),0,0,DustID.Venom,0,0,100);
                venom.noGravity = true;
                venom.velocity *= 0.3f;
                venom.fadeIn = Main.rand.NextFloat(0.5f,1f);
                venom.scale = Main.rand.NextFloat(0.1f,1f);
                venom.velocity += projectile.velocity * 0.3f;
                
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

    public class StickIntoEnemy : BulletBehavior
    {
        private int targetNPC = -1;
        private bool netUpdateNextTick = false;
        private Vector2 stickOffset = Vector2.Zero;

        public override void SendExtraAI(ref BinaryWriter writer)
        {
            writer.Write7BitEncodedInt(targetNPC);
            writer.WritePackedVector2(stickOffset);
        }

        public override void ReceiveExtraAI(ref BinaryReader reader)
        {
            //Main.NewText("the extra ai actually fuckin works");
            targetNPC = reader.Read7BitEncodedInt();
            stickOffset = reader.ReadPackedVector2();
        }

        public override void AI(ref Projectile projectile)
        {
            if (netUpdateNextTick)
            {
                projectile.netUpdate = true;
                netUpdateNextTick = false;
            }
            if (projectile.ai[1] < 5)
            {
                projectile.localNPCHitCooldown = 30 * projectile.MaxUpdates;
            }
            if (targetNPC != -1)
            {
                if (Main.npc[targetNPC] != null && Main.npc[targetNPC].active)
                {
                    projectile.Center = Main.npc[targetNPC].Center + stickOffset;
                    projectile.velocity = Vector2.Normalize(projectile.velocity);
                    projectile.scale *= 1f - (0.01f/projectile.MaxUpdates/(projectile.penetrate+1));
                    projectile.ai[1] -= 1;


                    if (Main.rand.NextBool(3))
                    {
                        Dust venom = Dust.NewDustDirect(projectile.position+ new Vector2(-2,-2),0,0,DustID.Venom,0,0,100);
                        venom.noGravity = true;
                        venom.velocity *= 0.3f;
                        venom.fadeIn = Main.rand.NextFloat(0.5f,1f);
                        venom.scale = Main.rand.NextFloat(0.1f,1f);
                        venom.velocity -= projectile.velocity * 3f*projectile.penetrate;
                    }
                }
                else
                {
                    projectile.Kill();
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetNPC == -1)
            {
                return base.Colliding(projHitbox, targetHitbox);
            }
            else
            {
                return true;
            }
        }

        public override bool? CanHitNPC(ref Projectile projectile, ref NPC target)
        {
            if (targetNPC != -1)
            {
                return target.whoAmI == targetNPC;
            }
            else
            {
                return null;
            }
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            netUpdateNextTick = true;
            if (targetNPC == -1)
            {
                targetNPC = target.whoAmI;
                while (projectile.getRect().Intersects(target.getRect()))
                {
                    projectile.position -= Vector2.Normalize(projectile.velocity)*2;
                }
                projectile.position -= Vector2.Normalize(projectile.velocity)*2;
                stickOffset = (projectile.Center - target.Center);
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                projectile.damage = (int)(projectile.damage * 0.2f);
                projectile.timeLeft = projectile.maxPenetrate * projectile.localNPCHitCooldown + 1;
                projectile.knockBack = 0;
            }
            else
            {
                projectile.damage = (int)(projectile.damage * 0.5f);
                if (projectile.damage < 1)
                {
                    projectile.damage = 1;
                }
            }
            
        }

        public override void DrawBehind(ref Projectile projectile, ref int index, ref List<int> behindNPCsAndTiles, ref List<int> behindNPCs,
            ref List<int> behindProjectiles, ref List<int> overPlayers, ref List<int> overWiresUI)
        {
            /*if (targetNPC != -1)
            {
                if (Main.npc[targetNPC].behindTiles) {
                    behindNPCsAndTiles.Add(index);
                }
                else {
                    behindNPCsAndTiles.Add(index);
                }

                return;
            }*/
            behindNPCsAndTiles.Add(index);
        }
    }
    
    public class AcidVenom : BulletBehavior
    {
        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            target.AddBuff(BuffID.Venom, 600);
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            for (int num664 = 0; num664 < 10; num664++)
            {
                int num665 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Venom, 0f, 0f, 100);
                Main.dust[num665].scale = (float)Main.rand.Next(1, 10) * 0.1f;
                Main.dust[num665].noGravity = true;
                Main.dust[num665].fadeIn = 1.5f;
                Dust dust44 = Main.dust[num665];
                Dust dust334 = dust44;
                dust334.velocity *= 0.75f;
            }
        }
    }

}

