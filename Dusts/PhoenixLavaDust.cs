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
    public class PhoenixLavaDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            //dust.noGravity = true; // Makes the dust have no gravity.
            //dust.noLight = true; // Makes the dust emit no light.
            //dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
            dust.frame = Texture2D.Value.Frame(1, 3, 0, Main.rand.Next(3));
            dust.rotation = Main.rand.NextFloat((float)Math.PI * 2);
        }
        
        public override bool Update(Dust dust)
        {
            
            Lighting.AddLight(dust.position, new Vector3(0.5f, 0.2f, 0.05f) * dust.scale);
            
            dust.velocity.Y += 1f;
            dust.scale -= 0.01f;
            Math.Clamp(dust.velocity.Y, -15, 15);

            bool eitherBumpSucceeded = false;
            //-----------CHECK Y COLLISION---------------
            dust.position.Y += dust.velocity.Y;
            if (Collision.SolidCollision(dust.position, 2, 2))
            {
                dust.scale -= 0.04f;
                bool bumpSucceeded = false;
                for (int i=0; i<=dust.velocity.Y; i++)
                {
                    dust.position.Y -= Math.Sign(dust.velocity.Y);
                    if (!Collision.SolidCollision(dust.position, 2, 2))
                    {
                        bumpSucceeded = true;
                        break;
                    }
                }

                if (bumpSucceeded)
                {
                    dust.velocity.Y = 0;
                    dust.velocity.X *= 0.87f;
                    dust.rotation += dust.velocity.X;
                    eitherBumpSucceeded = true;
                }
                else
                {
                    
                }
            }
            else
            {
                eitherBumpSucceeded = true;
                dust.position.Y -= dust.velocity.Y;
            }
            
            //-----------CHECK X COLLISION---------------
            dust.position.X += dust.velocity.X;
            if (Collision.SolidCollision(dust.position, 2, 2))
            {
                dust.scale -= 0.04f;
                bool bumpSucceeded = false;
                for (int i=0; i<=dust.velocity.X; i++)
                {
                    dust.position.X -= Math.Sign(dust.velocity.X);
                    if (!Collision.SolidCollision(dust.position, 2, 2))
                    {
                        bumpSucceeded = true;
                        break;
                    }
                }

                if (bumpSucceeded)
                {
                    dust.velocity.X = 0;
                    dust.velocity.Y *= 0.87f;
                    dust.rotation += dust.velocity.Y;
                    eitherBumpSucceeded = true;
                }
                else
                {
                    
                }

            }
            else
            {
                eitherBumpSucceeded = true;
                dust.position.X -= dust.velocity.X;
            }
            
            //-------------CHECK CORNER CASE---------------
            if (!eitherBumpSucceeded)
            {
                dust.velocity = Vector2.Zero;
            }
            
            
            

            dust.position += dust.velocity;
            
            
            
            

            if (dust.scale < 0.3f)
            {
                //Main.NewText("1");
                dust.active = false;
                //Main.NewText("2");
            }
            
            if(Collision.WetCollision(dust.position, 2, 2))
            {
                dust.velocity.Y = 0;
                dust.active = false;
            }

            if (!dust.active)
            {
                //Main.NewText("X: "+dust.position.X+ " Y: "+dust.position.Y);

                //Dust.NewDust(Main.player[Main.myPlayer].position, 0, 0, ModContent.DustType<RisingSmokeDust>());
                float dCountTemp = Dust.dCount;
                Dust.dCount = 0f;
                int smokeIndex = Dust.NewDust(dust.position, 0, 0, ModContent.DustType<RisingSmokeDust>());
                Main.dust[smokeIndex].velocity = dust.velocity;
                Main.dust[smokeIndex].scale = 0.3f;
                Dust.dCount = dCountTemp;

                //Main.dust[dust.dustIndex] = new Dust();
                //Main.NewText(smokeIndex);

            }
            
            /*if (dust.dustIndex > 50 && dust.dustIndex < 100)
            {
                Main.NewText(dust.dustIndex + ": " + dust.scale);
            }*/
            
            return false; // Return false to prevent vanilla behavior.
        }
        

        public override bool PreDraw(Dust dust)
        {
            Vector2 position = dust.position - Main.screenPosition;
            //Texture2D texture = TextureAssets.Projectile[Type].Value;
            //Rectangle sourceRectangle = texture.Frame(1, 1);
            //Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = dust.scale;
            //float lightingColor = Lighting.GetColor(dust.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(Texture2D.Value, position, dust.frame, new Color(192,192,192,128), dust.rotation, new Vector2(5f,5f), scale, SpriteEffects.None, 0f);



            return false;

        }

    }
}
