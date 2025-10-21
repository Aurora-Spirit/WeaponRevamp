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
    public class TracerDust : ModDust
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
            if (dust.alpha < 255)
            {
                dust.scale -= 0.05f;
                dust.position += dust.velocity;
                dust.velocity *= 0.95f;
            }
            //Lighting.AddLight(dust.position, 0.2f, 1f, 0f);
            if (!dust.noGravity && dust.alpha == 0)
            {
                dust.velocity += new Vector2(1,0).RotatedBy(dust.rotation) * dust.fadeIn;
                dust.velocity.X += Main.windSpeedCurrent;
            }

            if (dust.alpha > 0)
            {
                dust.alpha -= 64;
                if (dust.alpha < 0)
                {
                    dust.alpha = 0;
                }
            }

            if (dust.scale < 0.05f)
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
            float alpha = (255f - dust.alpha)/256f;
            if (alpha < 0)
            {
                alpha = 0;
            }
            float scale = dust.scale * 0.5f + 0.3f;
            float lightingColor = Lighting.GetColor(dust.position.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, dust.frame, dust.color * lightingColor * scale * alpha, dust.rotation, new Vector2(4f,4f), scale, SpriteEffects.None, 1f);



            return false;

        }

    }
}
