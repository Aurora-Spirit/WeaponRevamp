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
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.Bows

{
    public class BloodRainBowProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            base.SetDefaults();
        }
        

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.extraUpdates += 1;
            Projectile.tileCollide = false;
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
            if (Projectile.position.Y > Projectile.ai[1])
            {
                Projectile.tileCollide = true;
            }
            
            Color dustColor;
            switch (Projectile.ai[0]) //projectile id corresponding to location in sprite
            {
                case wooden: dustColor = new Color(190, 30, 10); break; //wooden
                case fire: dustColor = new Color(255, 192, 0); break; //fire
                case 4: dustColor = new Color(100, 0, 128); break; //unholy
                case 5: dustColor = new Color(255, 255, 255); break; //jester
                case 41: dustColor = new Color(255, 128, 0); break; //hellfire
                case 91: dustColor = new Color(255, 210, 255); break; //holy
                case 103: dustColor = new Color(64, 255, 0); break; //cursed
                case 172: dustColor = new Color(64, 255, 255); break; //frostburn
                case 225: dustColor = new Color(0, 255, 0); break; //chlorophyte
                case 278: dustColor = new Color(255, 255, 64); break; //ichor
                case 282: dustColor = new Color(100, 0, 255); break; //venom
                case 474: dustColor = new Color(255, 224, 200); break; //bone
                case 639: dustColor = new Color(0, 255, 150); break; //luminite
                case 1006: dustColor = new Color(220, 192, 255); break; //shimmer
                default: dustColor = new Color(255, 255, 255); break;
            }
            if (Projectile.ai[0] == spectre)
            {
                dustColor = new Color(24, 180, 252); //spectre

            }
            
            if (Projectile.timeLeft == maxTimeLeft - 1)
            {

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<TintableBloodDust>(), 0f, 0f, 50, dustColor, Main.rand.NextFloat(0.8f, 1.1f));
                    dust.velocity *= 4f;
                    dust.velocity += Projectile.velocity * Main.rand.NextFloat(1,2);
                    dust.fadeIn = 0.04f;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (Projectile.ai[0] == spectre && spectreSpinTime == 0)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity * (i/2f), Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 50, dustColor, Main.rand.NextFloat(0.8f, 1.1f));
                    dust.velocity *= 0.02f;
                    dust.velocity += Projectile.velocity * 0.5f;
                    dust.noGravity = true;
                }
                else
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity * (i/2f), Projectile.width, Projectile.height, ModContent.DustType<TintableBloodDust>(), 0f, 0f, 50, dustColor, Main.rand.NextFloat(0.8f, 1.1f));
                    dust.velocity *= 0.02f;
                    dust.velocity += Projectile.velocity * 0.5f;
                    dust.fadeIn = 0.00f;
                    
                }
            }
            
            //Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.6f);
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
                case wooden: dustColor = new Color(190, 30, 10); break; //wooden
                case fire: dustColor = new Color(255, 192, 0); break; //fire
                case 4: dustColor = new Color(100, 0, 128); break; //unholy
                case 5: dustColor = new Color(255, 255, 255); break; //jester
                case 41: dustColor = new Color(255, 64, 0); break; //hellfire
                case 91: dustColor = new Color(255, 128, 255); break; //holy
                case 103: dustColor = new Color(64, 255, 0); break; //cursed
                case 172: dustColor = new Color(0, 255, 255); break; //frostburn
                case 225: dustColor = new Color(0, 255, 0); break; //chlorophyte
                case 278: dustColor = new Color(255, 255, 0); break; //ichor
                case 282: dustColor = new Color(100, 0, 255); break; //venom
                case 474: dustColor = new Color(255, 224, 200); break; //bone
                case 639: dustColor = new Color(0, 255, 150); break; //luminite
                case 1006: dustColor = new Color(220, 192, 255); break; //shimmer
                default: dustColor = new Color(255, 255, 255); break;
            }
            if (Projectile.ai[0] == spectre)
            {
                dustColor = new Color(24, 180, 252); //spectre

            }

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<TintableBloodDust>(), 0f, 0f, 50, dustColor, Main.rand.NextFloat(0.8f, 1.1f));
                dust.velocity *= 2f;
                dust.velocity += Projectile.velocity * -0.5f;
                dust.fadeIn = 0.05f;
            }

            /*for (int i=0;i<20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, 0f, 0f, 150, dustColor, 0.9f);
                dust.noGravity = true;
                dust.velocity *= 3;
            }*/
            base.OnKill(timeLeft);

        }


        public override bool PreDraw(ref Color lightColor)
        {
            


            return false;
        }

    }
}