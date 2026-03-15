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
    public class TintableBloodDust : ModDust
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
            dust.position += dust.velocity;
            dust.velocity *= 0.99f;
            dust.velocity.Y += 0.1f;
            Lighting.AddLight(dust.position, dust.color.ToVector3()*0.3f);
            dust.scale += dust.fadeIn;
            dust.fadeIn -= 0.01f;
            if (dust.fadeIn < -0.04f) dust.fadeIn = -0.04f;
            dust.rotation += 0.2f;

            if (dust.scale < 0.05f)
            {
                dust.active = false;
            }

            dust.color = Color.Lerp(dust.color, new Color(190, 30, 10), 0.13f);
            return false; // Return false to prevent vanilla behavior.
        }

        /*public override bool PreDraw(Dust dust)
        {
            Vector2 position = dust.position - Main.screenPosition;
            //Texture2D texture = TextureAssets.Projectile[Type].Value;
            //Rectangle sourceRectangle = texture.Frame(1, 1);
            //Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = dust.scale;
            float lightingColor = Lighting.GetColor(dust.position.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, Texture2D.Value.Frame(1, 1, 0, 0), new Color(dust.color.R, dust.color.G, dust.color.B, 0), dust.rotation, new Vector2(7f,7f), scale, SpriteEffects.None, 0f);



            return false;

        }*/

    }
}
