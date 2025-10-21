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

namespace WeaponRevamp.Gores
{
    public class Shockwave : ModProjectile
    {
        private float rotationVel = 0;
        private int expansionTime = 0;
        private int fadeOutTime = 0;
        public Color color = new Color(255, 255, 255, 128);

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteRGB(color);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            color = reader.ReadRGB();
            base.ReceiveExtraAI(reader);
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            //ai[0] refers to time since creation
            //ai[1] refers to maximum size
            //ai[2] refers to total time until animation finishes (and projectile is killed)
            if (Projectile.ai[0] == 0)
            {
                rotationVel = Main.rand.NextFloat(-0.1f, 0.1f);
                expansionTime = (int)(1*Projectile.ai[2]/2);
                //Main.NewText("old expansionTime: "+expansionTime);
                float maxScale = Projectile.ai[1] * expansionTime;
                expansionTime *= (int)(1f-Projectile.scale / maxScale);
                fadeOutTime = (int)Projectile.ai[2] / 2;
                Projectile.timeLeft = expansionTime + fadeOutTime;
                //Main.NewText("maxScale: "+maxScale+", expansionTime: "+expansionTime+", fadeOutTime: "+fadeOutTime+", timeLeft: "+Projectile.timeLeft);
            }

            if (Projectile.timeLeft > fadeOutTime)
            {
                Projectile.scale += Projectile.ai[1];
            }
            Projectile.rotation += rotationVel;
            
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
            Color renderAlpha = color*0.1f;
            /*if (Projectile.timeLeft > fadeOutTime)
            {
                frameNum = 3;
            }
            else
            {
                frameNum = (int)((1f - (float)Projectile.timeLeft / (float)(fadeOutTime))*5f)+4;
                renderAlpha *= 1 - (frameNum/9f)+0.5f;
            }*/
            frameNum = (int)((1f - (float)Projectile.timeLeft / (float)(fadeOutTime+expansionTime))*8f)+1;
            renderAlpha *= 1 - (frameNum/9f)+0.5f;
            /*if (Projectile.ai[0] == spectre) //for electric bullet later
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }*/
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 9, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            

            return false;
        }

        public static Projectile NewShockwave(Vector2 position, float size, int duration, Color color)
        {
            Projectile shockwave = Projectile.NewProjectileDirect(null,position,Vector2.Zero,ModContent.ProjectileType<Shockwave>(),0,0,-1,0,((float)size/64f)/(duration*0.25f),duration);
            shockwave.scale = 0;
            shockwave.netUpdate = true;
            ((Shockwave)shockwave.ModProjectile).color = color;
            return shockwave;
        }
    }

}
