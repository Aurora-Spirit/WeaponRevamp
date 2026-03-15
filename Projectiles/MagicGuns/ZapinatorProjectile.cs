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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using Terraria.GameContent;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class ZapinatorProjectile : ModProjectile
    {
        private int frameNum = 0;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 8;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
            Projectile.scale = 1.4f;
            Projectile.timeLeft = 3600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, new Vector3(0.2f,0.6f,0.75f));
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1]++;
            if (Projectile.ai[2] == 1 && Main.rand.NextBool(4))
            {
                Dust fire = Dust.NewDustDirect(Projectile.position + new Vector2(Main.rand.NextFloat(-5,5)*Projectile.scale,Main.rand.NextFloat(-5,5)*Projectile.scale),0,0,DustID.Torch);
                fire.scale = Main.rand.NextFloat(0.5f, 1.5f) * Projectile.scale;
                fire.velocity *= Projectile.scale;
                fire.noGravity = Main.rand.NextBool(3, 4);
                if (Main.rand.NextBool(1, 20))
                {
                    fire.velocity *= 10f;
                }
            }
            
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            RollRandomEffects(target, Projectile.ai[0]);
        }

        private void RollRandomEffects(NPC target, float recursiveChance)
        {
            
            //duplicate
            if (Main.rand.NextBool(1, 30))
            {
                Projectile.damage /= 2;
                Projectile.knockBack /= 2;
                
                Projectile newLaser = Projectile.NewProjectileDirect(null, Projectile.position, Projectile.velocity.RotatedByRandom(Math.PI/12), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1], Projectile.ai[2]);
                newLaser.timeLeft = Projectile.timeLeft;
                newLaser.penetrate = Projectile.penetrate;
                newLaser.scale = Projectile.scale;
                newLaser.Damage();
                //((ZapinatorProjectile)newLaser.ModProjectile).RollRandomEffects(target);
                Glitch(4,5);
            }
            
            
            //teleport randomly
            if (Main.rand.NextBool(1, 6))
            {
                Glitch(3,1);
                Vector2 tempPos = Projectile.position;
                Projectile.position.X += Main.rand.Next(-6*16, 6*16+1);
                GlitchLine(tempPos, Projectile.position, 2);
                Projectile.tileCollide = false;
                //Glitch(3,1);
            }
            if (Main.rand.NextBool(1, 6))
            {
                Glitch(3,1);
                Vector2 tempPos = Projectile.position;
                Projectile.position.Y += Main.rand.Next(-6*16, 6*16+1);
                GlitchLine(tempPos, Projectile.position, 2);
                Projectile.tileCollide = false;
                //Glitch(3,1);
            }
            
            //teleport backwards
            if (Main.rand.NextBool(2, 3))
            {
                Glitch(3,1);
                Vector2 tempPos = Projectile.position;
                Projectile.position -= Projectile.velocity * 25f;
                GlitchLine(tempPos, Projectile.position, 3);
                //Glitch(3,1);
                //Collision.SolidTiles(Projectile.position,1,1);
                if (!Collision.EmptyTile((int)Projectile.position.X/16, (int)Projectile.position.Y/16))
                {
                    Projectile.tileCollide = false;
                }
            }
            
            //rotate
            if (Main.rand.NextBool(1, 7))
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(Math.PI / 2);
                Glitch(4,1);
            }
            
            //accelerate or decelerate
            if (Main.rand.NextBool(1, 7))
            {
                Projectile.velocity *= Main.rand.NextFloat(0.1f, 3f);
                Glitch(4,1);
            }
            
            //mirror velocity
            if (Main.rand.NextBool(1, 12))
            {
                Projectile.velocity.X *= -1;
                Glitch(4,1);
            }
            if (Main.rand.NextBool(1, 12))
            {
                Projectile.velocity.Y *= -1;
                Glitch(4,1);
            }
            
            //jackpot
            if (Main.rand.NextBool(1, 50))
            {
                Projectile.damage *= 10;
                Projectile.scale *= 2;
                Glitch(4,6);
            }
            
            //jackpot if you like knockback
            if (Main.rand.NextBool(1, 20))
            {
                Projectile.knockBack *= 10;
                Projectile.scale *= 1.4f;
                Glitch(4,3);
            }
            
            //set on fire
            if (Main.rand.NextBool(1, 15))
            {
                Projectile.ai[2] = 1; //enable fire
                Glitch(4,1);
            }


            if (Main.rand.NextFloat() < recursiveChance)
            {
                RollRandomEffects(target, recursiveChance-1);
            }

            Projectile.netUpdate = true;
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            //stuff that happens every time
            Projectile.velocity *= 0.6f;
            Projectile.damage = (int)(Projectile.damage * 0.9f);
            Projectile.knockBack *= 0.9f;
            
            if (Projectile.ai[2] == 1)
            {
                target.AddBuff(BuffID.OnFire3, Main.rand.Next(30,600));
            }
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.oldVelocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Glitch(5,10);
        }
        
        private void Glitch(float radius, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Dust glitch = Dust.NewDustDirect(Projectile.position + Vector2.UnitX.RotatedByRandom(Math.PI*2)*16f*radius*Main.rand.NextFloat(),0,0,ModContent.DustType<GlitchDust>());
                glitch.velocity *= 0f;
            }
        }

        private void GlitchLine(Vector2 start, Vector2 end, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Dust glitch = Dust.NewDustDirect(Vector2.Lerp(start,end,Main.rand.NextFloat()),0,0,ModContent.DustType<GlitchDust>());
                glitch.velocity *= 0f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 4);
            Vector2 origin = new Vector2(sourceRectangle.Width, sourceRectangle.Height/2f);
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int stretch;
            if (frameNum != 0 && Main.rand.NextBool(5))
            {
                frameNum = 0;
            }
            if (frameNum == 0 && Main.rand.NextBool(15))
            {
                frameNum = Main.rand.Next(1, 4);
            }
            /*if (Projectile.ai[0] == spectre) //for electric bullet later
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }*/
            Color renderAlpha;
            if (Projectile.ai[1] > 4)
            {
                renderAlpha = new Color(255,255,255,0);
            }
            else
            {
                renderAlpha = new Color(0,0,0,0);
            }
            
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, frameNum), renderAlpha, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            

            return false;
        }
    }
}
