using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using WeaponRevamp.Dusts;


namespace WeaponRevamp.Projectiles.Guns.UnifiedBullet
{
    
    public class RainbowSmokeTrail : BulletBehavior
    {
        private Color rainbow = new Color(0f,0f,0f, 0.35f);
        private int rainbowCycleState = 0;
        private const int colorChangeSpeed = 24;
        
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] == 0)
            {
                rainbowCycleState = Main.rand.Next(0, 5);
            }
            switch (rainbowCycleState) //cycle the faster than DiscoRGB rgb for the trail
            {
                case 0:
                    if (rainbow.G+colorChangeSpeed >= 255)
                    {
                        rainbow.G = 255;
                        rainbowCycleState = 1;
                        break;
                    }
                    rainbow.G += colorChangeSpeed;
                    
                    break;
                case 1:
                    if (rainbow.R-colorChangeSpeed <= 0)
                    {
                        rainbow.R = 0;
                        rainbowCycleState = 2;
                        break;
                    }
                    rainbow.R -= colorChangeSpeed;
                    
                    break;
                case 2:
                    if (rainbow.B+colorChangeSpeed >= 255)
                    {
                        rainbow.B = 255;
                        rainbowCycleState = 3;
                        break;
                    }
                    rainbow.B += colorChangeSpeed;
                    
                    break;
                case 3:
                    if (rainbow.G-colorChangeSpeed <= 0)
                    {
                        rainbow.G = 0;
                        rainbowCycleState = 4;
                        break;
                    }
                    rainbow.G -= colorChangeSpeed;
                    
                    break;
                case 4:
                    if (rainbow.R+colorChangeSpeed >= 255)
                    {
                        rainbow.R = 255;
                        rainbowCycleState = 5;
                        break;
                    }
                    rainbow.R += colorChangeSpeed;
                    
                    break;
                case 5:
                    if (rainbow.B-colorChangeSpeed <= 0)
                    {
                        rainbow.B = 0;
                        rainbowCycleState = 0;
                        break;
                    }
                    rainbow.B -= colorChangeSpeed;
                    
                    break;
                    
            }
            if (projectile.ai[1] > 5)
            {
                int dustCount = (int)(projectile.velocity.Length() * 0.3f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, rainbow);
                    smoke.velocity *= 0f;
                    smoke.position -= projectile.velocity * 2f;
                    smoke.position += projectile.velocity * i / dustCount;
                    smoke.velocity += projectile.velocity * 0.1f;
                    //smoke.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
                    smoke.rotation = (projectile.ai[1]+i / dustCount) * -0.5f * Math.Sign(projectile.velocity.X) + Main.rand.NextFloat()*0.1f;
                    //smoke.fadeIn = 0.15f * Math.Abs(Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.UnitY.RotatedBy(smoke.rotation))) + 0.00f;
                    smoke.fadeIn = 0.1f + Main.rand.NextFloat()*0.05f;
                    smoke.scale = 0.6f + Main.rand.NextFloat()*0.2f;
                }
                
                
            }
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            if (projectile.ai[1] > 5)
            {
                int dustCount = (int)(projectile.velocity.Length() * 0.3f);
                for (float i = 0; i < dustCount; i++)
                {
                    Dust smoke = Dust.NewDustDirect(projectile.oldPosition + new Vector2(-2,-2), 0, 0, ModContent.DustType<TracerDust>(), 0, 0, 0, rainbow);
                    smoke.velocity *= 0f;
                    smoke.position -= projectile.oldVelocity * 1f;
                    smoke.position += projectile.oldVelocity * i / dustCount;
                    smoke.velocity += projectile.oldVelocity * 0.1f;
                    //smoke.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
                    smoke.rotation = (projectile.ai[1]+i / dustCount) * -0.5f * Math.Sign(projectile.oldVelocity.X) + Main.rand.NextFloat()*0.1f;
                    //smoke.fadeIn = 0.15f * Math.Abs(Vector2.Dot(Vector2.Normalize(projectile.velocity), Vector2.UnitY.RotatedBy(smoke.rotation))) + 0.00f;
                    smoke.fadeIn = 0.1f + Main.rand.NextFloat()*0.05f;
                    smoke.scale = 0.6f + Main.rand.NextFloat()*0.2f;
                }
                
                
            }
        }
    }

    public class ConfettiOnHit : BulletBehavior
    {
        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            for (int num666 = 0; num666 < 10; num666++)
            {
                int num667 = Main.rand.Next(139, 143);
                int num668 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, num667, (0f - projectile.velocity.X) * 0.3f, (0f - projectile.velocity.Y) * 0.3f, 0, default(Color), 1.2f);
                Main.dust[num668].velocity.X += (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.dust[num668].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.dust[num668].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.dust[num668].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.dust[num668].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                Main.dust[num668].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                Dust dust188 = Main.dust[num668];
                Dust dust334 = dust188;
                dust334.scale *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
            }
            for (int num669 = 0; num669 < 5; num669++)
            {
                int num670 = Main.rand.Next(276, 283);
                int num671 = Gore.NewGore(projectile.GetSource_FromThis(), projectile.position, -projectile.velocity * 0.3f, num670);
                Main.gore[num671].velocity.X += (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num671].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num671].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num671].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Gore gore46 = Main.gore[num671];
                Gore gore64 = gore46;
                gore64.scale *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
                Main.gore[num671].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                Main.gore[num671].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
            }
        }
    }

    public class ConfuseEnemies : BulletBehavior
    {
        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            target.AddBuff(BuffID.Confused, Main.rand.Next(60,180));
        }
    }

}

