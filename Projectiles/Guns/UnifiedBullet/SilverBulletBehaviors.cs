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
    
    public class SilverBulletGlow : BulletBehavior
    {
        private Color shineColor;
        private Texture2D sprite;
        private Color glowColor;

        public SilverBulletGlow(Color shineColor, Color glowColor, Texture2D sprite)
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
            Lighting.AddLight(projectile.Center, glowColor.ToVector3());
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            Dust shine = Dust.NewDustDirect(projectile.position,0,0,ModContent.DustType<ShineDust>(),0,0,0,shineColor);
            shine.velocity *= 0f;
            shine.scale = 0.6f;
            shine.fadeIn = 0.3f;
            //shine.position -= projectile.oldVelocity;
        }

        public override void Draw(ref Projectile projectile, ref Color lightColor)
        {
            if (sprite != null) //should never happen, just in case...
            {
                Vector2 position = projectile.position + (Vector2.Normalize(projectile.velocity)*10f*projectile.scale) - Main.screenPosition + new Vector2(2,2);
                Main.EntitySpriteDraw(sprite, position, sprite.Frame(1, 1, 0, 0), new Color(shineColor.R, shineColor.G, shineColor.B, 0), projectile.ai[1]*0.05f, new Vector2(7f,7f), 1f, SpriteEffects.None, 0f);
                
            }
        }
    }

}

