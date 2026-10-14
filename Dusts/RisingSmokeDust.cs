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
    public class RisingSmokeDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            //dust.noGravity = true; // Makes the dust have no gravity.
            //dust.noLight = true; // Makes the dust emit no light.
            //dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.

            dust.frame = Texture2D.Value.Frame(1, 3, 0, Main.rand.Next(3));
        }
        
        public override bool Update(Dust dust)
        {
            dust.velocity.Y -= 0.2f;
            dust.velocity *= 0.9f;
            dust.scale += 0.05f;
            dust.position += dust.velocity;
            dust.alpha += 6;
            dust.rotation += dust.velocity.X * 0.1f;
            if (dust.alpha > 224)
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
            float lightingColor = Lighting.GetColor(dust.position.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            float drawAlpha = (255f - (float)dust.alpha) / 255f;
            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, dust.frame, new Color(255,255,255,255) * drawAlpha * lightingColor, dust.rotation, new Vector2(5f,5f), scale, SpriteEffects.None, 0f);



            return false;

        }

    }
}
