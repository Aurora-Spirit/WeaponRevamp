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
    public class HallowedRepeaterProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            base.SetDefaults();
        }
        

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            useAmmoPhysics = false;
            Projectile.penetrate *= 2;
            Projectile.maxPenetrate *= 2;
            Projectile.extraUpdates *= 2;
            Projectile.extraUpdates += 1;
            useArrowTrail = false;
            //Main.NewText(Projectile.ai[0]);


            PostSpawn(source);
        }
        

        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            /*if (Projectile.ai[0]!=ProjectileID.JestersArrow)
            {
                if (Projectile.ai[0] == ProjectileID.ShimmerArrow)
                {
                    Projectile.velocity.Y -= 0.04f;
                }
                else
                {
                    Projectile.velocity.Y += 0.04f;
                }
            }*/
            Color dustColor;
            switch (Projectile.ai[0]) //projectile id corresponding to location in sprite
            {
                case wooden: dustColor = new Color(255, 255, 0, 0); break; //wooden
                case fire: dustColor = new Color(255, 192, 0, 0); break; //fire
                case 4: dustColor = new Color(100, 0, 128, 0); break; //unholy
                case 5: dustColor = new Color(255, 255, 255, 0); break; //jester
                case 41: dustColor = new Color(255, 64, 0, 0); break; //hellfire
                case 91: dustColor = new Color(255, 128, 255, 0); break; //holy
                case 103: dustColor = new Color(64, 255, 0, 0); break; //cursed
                case 172: dustColor = new Color(0, 255, 255, 0); break; //frostburn
                case 225: dustColor = new Color(0, 255, 0, 0); break; //chlorophyte
                case 278: dustColor = new Color(255, 255, 0, 0); break; //ichor
                case 282: dustColor = new Color(100, 0, 255, 0); break; //venom
                case 474: dustColor = new Color(255, 224, 200, 0); break; //bone
                case 639: dustColor = new Color(0, 255, 150, 0); break; //luminite
                case luminiteEye: dustColor = new Color(0, 255, 150, 0); break; //luminite
                case 1006: dustColor = new Color(255, 128, 255, 0); break; //shimmer
                default: dustColor = new Color(255, 255, 255, 0); break;
            }
            if (Projectile.ai[0] == spectre)
            {
                dustColor = new Color(24, 180, 252, 0); //spectre

            }


            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, 0f, 0f, 150, dustColor, 0.9f);
            dust.noGravity = true;
            dust.velocity += Projectile.velocity * 0.5f;
            //Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
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

        public override void OnKill(int timeLeft)
        {
            Color dustColor;
            switch (Projectile.ai[0]) //projectile id corresponding to location in sprite
            {
                case wooden: dustColor = new Color(255, 255, 0, 0); break; //wooden
                case fire: dustColor = new Color(255, 192, 0, 0); break; //fire
                case 4: dustColor = new Color(100, 0, 128, 0); break; //unholy
                case 5: dustColor = new Color(255, 255, 255, 0); break; //jester
                case 41: dustColor = new Color(255, 64, 0, 0); break; //hellfire
                case 91: dustColor = new Color(255, 128, 255, 0); break; //holy
                case 103: dustColor = new Color(64, 255, 0, 0); break; //cursed
                case 172: dustColor = new Color(0, 255, 255, 0); break; //frostburn
                case 225: dustColor = new Color(0, 255, 0, 0); break; //chlorophyte
                case 278: dustColor = new Color(255, 255, 0, 0); break; //ichor
                case 282: dustColor = new Color(128, 64, 255, 0); break; //venom
                case 474: dustColor = new Color(255, 224, 200, 0); break; //bone
                case 639: dustColor = new Color(0, 255, 150, 0); break; //luminite
                case luminiteEye: dustColor = new Color(0, 255, 150, 0); break; //luminite
                case 1006: dustColor = new Color(255, 128, 255, 0); break; //shimmer
                default: dustColor = new Color(255, 255, 255, 0); break;
            }
            if (Projectile.ai[0] == spectre)
            {
                dustColor = new Color(24, 180, 252, 0); //spectre

            }

            for (int i=0;i<20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, 0f, 0f, 150, dustColor, 0.9f);
                dust.noGravity = true;
                dust.velocity *= 3;
            }
            base.OnKill(timeLeft);

        }


        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 16);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = 1;
            int frameNum;
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
            Color renderAlpha = new Color(255,255,255,0);
            if (Projectile.ai[0] == luminiteEye)
            {
                renderAlpha = new Color(0,0,0,0);
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
                    if (useAmmoPhysics)
                    {
                        fakeVelocity.Y -= 0.1f;
                    }


                    Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), new Color(i, i, i, 0) * tempLightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
                }

            }
            position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            
            if (Projectile.ai[0] == luminiteEye)
            {
                
                Main.EntitySpriteDraw(luminiteEyeTexture.Value, position, luminiteEyeTexture.Value.Frame(1, 1, 0, 0), new Color(255, 255, 255, 0) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            return false;
        }

    }
}