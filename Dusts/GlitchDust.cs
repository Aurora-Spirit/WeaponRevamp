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
    public class GlitchDust : ModDust
    {
        
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            //dust.noGravity = true; // Makes the dust have no gravity.
            //dust.noLight = true; // Makes the dust emit no light.
            //dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
            dust.rotation = Main.rand.Next(0, 8*2);
            if ((int)dust.rotation / 8 == 1)
            {
                dust.color = new Color(Main.rand.Next(0, 256),Main.rand.Next(0, 256),Main.rand.Next(0, 256),255);
                switch (Main.rand.Next(0, 3))
                {
                    case 0:
                        dust.color.R = 255;
                        break;
                    case 1:
                        dust.color.G = 255;
                        break;
                    case 2:
                        dust.color.B = 255;
                        break;
                }
            }
            else
            {
                dust.color = new Color(255, 255, 255, 255);
            }
        }

        public override bool Update(Dust dust)
        {
            /*dust.position.X -= (int)(dust.position.X/16)*16;
            dust.position.Y -= (int)(dust.position.Y/16)*16;*/
            dust.velocity *= 0.95f;
            dust.scale -= Main.rand.NextFloat(0, 0.1f);
            if (dust.scale <= 0)
            {
                dust.active = false;
            }
            Lighting.AddLight(dust.position,new Vector3(0.2f,0.2f,0.2f));
            
            
            
            
            return false; // Return false to prevent vanilla behavior.
        }

        public override bool PreDraw(Dust dust)
        {
            Vector2 truePosition = new Vector2((int)(dust.position.X/16)*16+8,(int)(dust.position.Y/16)*16+8);
            Vector2 position = truePosition - Main.screenPosition;
            //Texture2D texture = TextureAssets.Projectile[Type].Value;
            //Rectangle sourceRectangle = texture.Frame(1, 1);
            //Vector2 origin = sourceRectangle.Size() / 2f;
            float lightingColor = Lighting.GetColor(dust.position.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);

            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, Texture2D.Value.Frame(2, 8, (int)dust.rotation/8, (int)dust.rotation%8), dust.color, 0, new Vector2(8f,8f), 1f, SpriteEffects.None, 0f);
            //Main.EntitySpriteDraw(Texture2D.Value, truePosition, Texture2D.Value.Frame(1, 8, 0, (int)dust.rotation), new Color(255,255,255,255), 0, new Vector2(8f,8f), 1f, SpriteEffects.None, 0f);
            

            return false;

        }
        

    }

    
}
