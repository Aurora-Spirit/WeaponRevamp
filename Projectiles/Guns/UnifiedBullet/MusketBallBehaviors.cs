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
    public class SmokeTrail : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 5)
            {
                int dustCount = (int)(projectile.velocity.Length() * 0.3f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, new Color(0.5f,0.5f,0.5f,0.35f));
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
                    Dust smoke = Dust.NewDustDirect(projectile.oldPosition + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, new Color(0.5f,0.5f,0.5f,0.35f));
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

    public class BulletGlow : BulletBehavior
    {
        private Color shineColor;
        private Texture2D sprite;
        private Color glowColor;

        public BulletGlow(Color shineColor, Color glowColor, Texture2D sprite)
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
                Vector2 position = projectile.position + (Vector2.Normalize(projectile.velocity)*10f*projectile.scale) - Main.screenPosition + new Vector2(2,2);
                Main.EntitySpriteDraw(sprite, position, sprite.Frame(1, 1, 0, 0), new Color(shineColor.R, shineColor.G, shineColor.B, 0), projectile.ai[1]*0.05f, new Vector2(7f,7f), 0.8f, SpriteEffects.None, 0f);
                
            }
        }
    }

    public class HitTiles : BulletBehavior
    {
        public override bool OnTileCollide(ref Projectile projectile, ref Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, projectile.position);
            Collision.HitTiles(projectile.position,projectile.oldVelocity,0,0);
            return true;
        }
    }
}

