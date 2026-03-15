using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace WeaponRevamp.Dusts
{
    public class ArrowTrailDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            //dust.noGravity = true; // Makes the dust have no gravity.
            //dust.noLight = true; // Makes the dust emit no light.
            //dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
        }

        public override bool Update(Dust dust)
        {
            dust.scale -= dust.fadeIn;
            dust.position += dust.velocity;
            dust.velocity *= 0.90f;
            //Lighting.AddLight(dust.position, 0.2f, 1f, 0f);
            if (!dust.noGravity)
            {
                dust.velocity.X += Main.windSpeedCurrent;
            }

            if (dust.alpha < 255)
            {
                dust.alpha += 8;
            }

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }
            return false; // Return false to prevent vanilla behavior.
        }

        public override bool PreDraw(Dust dust)
        {
            Vector2 position = dust.position - Main.screenPosition;
            //Texture2D texture = TextureAssets.Projectile[Type].Value;
            //Rectangle sourceRectangle = texture.Frame(1, 1);
            //Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = dust.scale;
            Color lightingColorColor = Lighting.GetColor(dust.position.ToTileCoordinates());
            float lightingColor = lightingColorColor.ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Color color = dust.color;
            if (dust.noLight)
            {
                /*color.A = Math.Max(color.A, color.R);
                color.A = Math.Max(color.A, color.G);
                color.A = Math.Max(color.A, color.B);*/
                color = color.MultiplyRGBA(lightingColorColor);
                //Main.NewText("R:"+lightingColorColor.R+" G:"+lightingColorColor.G+" B:"+lightingColorColor.B+" A:"+lightingColorColor.A);
            }
            Main.spriteBatch.Draw(Texture2D.Value, position, dust.frame, color, dust.rotation, new Vector2(4f,4f), scale, SpriteEffects.None, 0f);



            return false;

        }
        

    }
}
