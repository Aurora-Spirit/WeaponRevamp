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
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;


namespace WeaponRevamp.Dusts
{
    public class HighVelocityDust : ModDust
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
            float fadeOut = dust.fadeIn/5;
            if (fadeOut > 1)
            {
                fadeOut = 1;
            }
            if (fadeOut < 0)
            {
                fadeOut = 0;
            }
            Lighting.AddLight(dust.position, new Vector3(1,1,0)*fadeOut);
            dust.fadeIn -= 1f;
            dust.scale *= 0.9f;

            if (dust.fadeIn <= 0f)
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
            float fadeOut = dust.fadeIn/5;
            if (fadeOut > 1)
            {
                fadeOut = 1;
            }

            if (fadeOut < 0)
            {
                fadeOut = 0;
            }
            float lightingColor = Lighting.GetColor(dust.position.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, Texture2D.Value.Frame(1, 1, 0, 0), new Color(1,0.5f + 0.5f * dust.scale,0.5f + 0.5f * dust.scale,1) * fadeOut, dust.rotation, new Vector2(8f,3f), 1f, SpriteEffects.None, 0f);



            return false;

        }
        

    }

    
}
