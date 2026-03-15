using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Microsoft.CodeAnalysis.Diagnostics;
using WeaponRevamp.Buffs.Bows;
using WeaponRevamp.Dusts;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace WeaponRevamp.Projectiles.Bows
{
    public class BloodRainBowRainbow : ModProjectile
    {
        private float rotationVel = 0;
        private int expansionTime = 0;
        private int fadeTime = 20;
        public Color color = new Color(36, 36, 36, 8);

        public override bool? CanDamage()
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            //ai[0] refers to time since creation
            //ai[1] refers to maximum size
            //ai[2] refers to total time until animation finishes (and projectile is killed)
            if (Projectile.ai[0] == 0)
            {
                
            }
            
            Projectile.ai[0]++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 9);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            int stretch;
            Color renderAlpha = color;
            /*float distance = Projectile.ai[1]-100;
            if (distance < 0) distance = 0;
            if (distance > 250) distance = 250;
            float distanceBrightness = 1 - (distance / 250);
            renderAlpha = renderAlpha*distanceBrightness;*/
            if (Projectile.timeLeft < fadeTime)
            {
                renderAlpha *= ((float)Projectile.timeLeft/fadeTime);
            }if (Projectile.ai[0] < fadeTime)
            {
                renderAlpha *= ((float)Projectile.ai[0]/fadeTime);
            }
            /*if (Projectile.timeLeft > fadeOutTime)
            {
                frameNum = 3;
            }
            else
            {
                frameNum = (int)((1f - (float)Projectile.timeLeft / (float)(fadeOutTime))*5f)+4;
                renderAlpha *= 1 - (frameNum/9f)+0.5f;
            }*/
            /*if (Projectile.ai[0] == spectre) //for electric bullet later
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }*/
            float arcLength = 384;
            float halfArcLength = arcLength / 2;
            //float angle = (float)Math.Tan(2f/104f)*2;
            //float segmentLength = 1f*(float)Math.PI / 80f;
            //float segmentLength = angle;
            int radius = 800;
            //origin.Y += radius;
            //position.Y += radius;
            float xDistance = Projectile.ai[1];
            position.X += Main.rand.NextFloat(1);
            position.Y += Main.rand.NextFloat(1);
            
            for (float xOffset = -halfArcLength; xOffset < halfArcLength; xOffset += 4)
            {
                float yOffset = -(float)Math.Cos(Math.Asin((xOffset+xDistance) / radius))*radius + radius+64;
                Vector2 newPos = position + new Vector2(xOffset+xDistance, yOffset);
                Color newRenderAlpha = renderAlpha * (1-(float)(Math.Abs(xOffset)/halfArcLength));
                Main.EntitySpriteDraw(texture, newPos, texture.Frame(1, 1, 0, 0), newRenderAlpha * (lightingColor+0.0f), Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
                
            }
            

            return false;
        }
    }

}
