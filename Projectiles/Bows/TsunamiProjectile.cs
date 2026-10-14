using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using System.CodeDom;

namespace WeaponRevamp.Projectiles.Bows

{
    public class TsunamiProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            //Main.NewText(Projectile.ai[0]);
            Projectile.localNPCHitCooldown*=3;
            useArrowTrail = false;


            PostSpawn(source);
        }

        

        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }*/
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 16);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            float waterOffset = 4f;
            float waterRotation = Projectile.timeLeft*0.1f;
            switch (Projectile.ai[0]) //projectile id corresponding to location in sprite
            {
                case wooden: frameNum = 0; break; //wooden
                case fire: frameNum = 1; break; //fire
                case 4: frameNum = 2; break; //unholy
                case 5: frameNum = 3; break; //jester
                case 41: frameNum = 4; break; //hellfire
                case 91: frameNum = 5; break; //holy
                case 103: frameNum = 6; break; //cursed
                case 172: frameNum = 7; break; //frostburn
                case 225: frameNum = 8; break; //chlorophyte
                case 278: frameNum = 9; break; //ichor
                case 282: frameNum = 10; break; //venom
                case 474: frameNum = 11; break; //bone
                case 639: frameNum = 12; break; //luminite
                case 1006: frameNum = 13; break; //shimmer
                default: frameNum = 0; break;
            }
            if (Projectile.ai[0] == spectre) //spectre
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }
            Color renderAlpha;
            switch (Projectile.ai[0]) //projectile rendering alpha
            {
                case wooden: //wooden
                case fire: //fire
                case 4: //unholy
                case 41: //hellfire
                case 103: //cursed
                case 172: //frostburn
                case 225: //chlorophyte
                case 278: //ichor
                case 282: //venom
                case 474: //bone
                case 639: //luminite
                case 1006: //shimmer
                    renderAlpha = new Color(128, 192, 255, 192); break;
                case 5: //jester
                case 91: //holy
                    renderAlpha = new Color(128, 192, 255, 0); break;
                //renderAlpha = new Color(200, 200, 200, 0); break;
                case luminiteEye:
                    renderAlpha = new Color(0, 0, 0, 0); break;
                default:
                    renderAlpha = new Color(128, 192, 255, 192); break;
            }
            
            if (Projectile.ai[0] == ProjectileID.MoonlordArrow)
            {
                float tempLightingColor = lightingColor - 0.1f;
                if (tempLightingColor < 0)
                {
                    tempLightingColor = 0;
                }
                Vector2 fakeVelocity = Projectile.velocity;
                for (int i = 230; i > Projectile.alpha; i -= 25)
                {
                    position -= fakeVelocity;
                    if (useAmmoPhysics) {
                        fakeVelocity.Y -= 0.1f;
                    }
                    
                    
                    Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), new Color(i, i, i, i/2) * tempLightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
                }

            }
            position = Projectile.Center - Main.screenPosition;

            Vector2 waterOffsetPos = new Vector2(waterOffset, 0).RotatedBy(waterRotation);
            int numWateryImages = 3;
            int numWateryLayers = 2;
            float waterScale = 1.1f;
            Color waterColor = new Color(64, 128, 256, 0);
            for (int j = 0; j < numWateryLayers; j++)
            {
                for (int i = 0; i < numWateryImages; i++)
                {
                    Main.EntitySpriteDraw(texture, position+waterOffsetPos, texture.Frame(1, 16, 0, frameNum), waterColor * lightingColor, Projectile.rotation, origin, waterScale, SpriteEffects.None, 0f);
                    waterOffsetPos = waterOffsetPos.RotatedBy(Math.PI * 2f / numWateryImages);
                }

                waterOffsetPos *= 2f;
                waterColor *= 0.2f;
                waterOffsetPos = waterOffsetPos.RotatedBy(Math.PI * 1f / numWateryImages);
                waterScale *= 1.5f;
            }
            
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            
            
            if (Projectile.ai[0] == luminiteEye)
            {
                Main.EntitySpriteDraw(luminiteEyeTexture.Value, position, luminiteEyeTexture.Value.Frame(1, 1, 0, 0), new Color(255, 255, 255, 128) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            


            return false;
        }
    }
}