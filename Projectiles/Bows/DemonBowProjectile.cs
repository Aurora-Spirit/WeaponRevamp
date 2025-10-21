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
    public class DemonBowProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            //Main.NewText(Projectile.ai[0]);


            PostSpawn(source);
        }

        

        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            int homeRange = 300;
            Vector2 vel = Projectile.velocity;
            Vector2 pos = Projectile.position;
            int target = Projectile.FindTargetWithLineOfSight(homeRange);
            if (target != -1)
            {

                NPC targetNPC = Main.npc[target];
                if(targetNPC.position.X > pos.X && vel.X > 0)
                {
                    if(targetNPC.position.Y < pos.Y && Projectile.ai[0] == shimmer)
                    {
                        float a = -0.05f;
                        float b = vel.Y;
                        float c = pos.Y - targetNPC.position.Y; //(b * b) / (2 * -a)
                        double intercept = vel.X * (-b - Math.Sqrt( (b * b) - (4 * a * c) ) ) / (2 * a);
                        intercept += pos.X;
                        //Main.NewText("a:"+a+" b:"+b+" c:"+c);
                        //Main.NewText("NPC X:" + targetNPC.position.X + " intercept X:" + intercept);
                        //Dust.NewDust(new Vector2((float)intercept, targetNPC.position.Y), 1, 1, DustID.Torch);
                        float targetX = targetNPC.position.X + targetNPC.velocity.X * (targetNPC.position.X - pos.X) / vel.X;
                        if (targetX < intercept)
                        {
                            Projectile.velocity.X -= ((float)intercept - targetX) * 0.00003f * vel.X * (float)Math.Sqrt(homeRange - Projectile.Distance(targetNPC.Center));
                            Projectile.velocity.Y -= 0.05f;
                        }
                    }
                    else if(targetNPC.position.Y > pos.Y && Projectile.ai[0] != shimmer)
                    {
                        float a;
                        switch(Projectile.ai[0])
                        {
                            case (holy):
                                a = 0.035f;
                                break;
                            case (frostburn):
                                a = 0.0425f;
                                break;
                            default:
                                a = 0.05f;
                                break;
                        }
                        float b = vel.Y;
                        float c = pos.Y - targetNPC.position.Y; //(b * b) / (2 * -a)
                        double intercept = vel.X * ( -b + Math.Sqrt( (b * b) - (4 * a * c) ) ) / ( 2 * a );
                        intercept += pos.X;
                        //Main.NewText("a:"+a+" b:"+b+" c:"+c);
                        //Main.NewText("NPC X:" + targetNPC.position.X + " intercept X:" + intercept);
                        //Dust.NewDust(new Vector2((float)intercept, targetNPC.position.Y), 1, 1, DustID.Torch);
                        float targetX = targetNPC.position.X + targetNPC.velocity.X * (targetNPC.position.X - pos.X) / vel.X;
                        if (targetX < intercept)
                        {
                            Projectile.velocity.X -= ((float)intercept - targetX) * 0.00003f * vel.X * (float)Math.Sqrt(homeRange - Projectile.Distance(targetNPC.Center));
                            Projectile.velocity.Y += 0.05f;
                        }
                    }
                }

                /////////////////////////////////
                //////////////////////////////


                if (targetNPC.position.X < pos.X && vel.X < 0)
                {
                    if (targetNPC.position.Y < pos.Y && Projectile.ai[0] == shimmer)
                    {
                        float a = -0.05f;
                        float b = vel.Y;
                        float c = pos.Y - targetNPC.position.Y; //(b * b) / (2 * -a)
                        double intercept = vel.X * (-b - Math.Sqrt( (b * b) - (4 * a * c) ) ) / (2 * a);
                        intercept += pos.X;
                        //Main.NewText("a:"+a+" b:"+b+" c:"+c);
                        //Main.NewText("NPC X:" + targetNPC.position.X + " intercept X:" + intercept);
                        //Dust.NewDust(new Vector2((float)intercept, targetNPC.position.Y), 1, 1, DustID.Torch);
                        float targetX = targetNPC.position.X + targetNPC.velocity.X * (targetNPC.position.X - pos.X) / vel.X;
                        if (targetX > intercept)
                        {
                            Projectile.velocity.X += ((float)intercept - targetX) * 0.00003f * vel.X * (float)Math.Sqrt(homeRange - Projectile.Distance(targetNPC.Center));
                            Projectile.velocity.Y -= 0.05f;
                        }
                    }
                    else if (targetNPC.position.Y > pos.Y && Projectile.ai[0] != shimmer)
                    {
                        float a;
                        switch (Projectile.ai[0])
                        {
                            case (holy):
                                a = 0.035f;
                                break;
                            case (frostburn):
                                a = 0.0425f;
                                break;
                            default:
                                a = 0.05f;
                                break;
                        }
                        float b = vel.Y;
                        float c = pos.Y - targetNPC.position.Y; //(b * b) / (2 * -a)
                        double intercept = vel.X * (-b + Math.Sqrt( (b * b) - (4 * a * c) ) ) / (2 * a);
                        intercept += pos.X;
                        //Main.NewText("a:"+a+" b:"+b+" c:"+c);
                        //Main.NewText("NPC X:" + targetNPC.position.X + " intercept X:" + intercept);
                        //Dust.NewDust(new Vector2((float)intercept, targetNPC.position.Y), 1, 1, DustID.Torch);
                        float targetX = targetNPC.position.X + targetNPC.velocity.X * (targetNPC.position.X - pos.X) / vel.X;
                        if (targetX > intercept)
                        {
                            Projectile.velocity.X += ((float)intercept - targetX) * 0.00003f * vel.X * (float)Math.Sqrt(homeRange - Projectile.Distance(targetNPC.Center));
                            Projectile.velocity.Y += 0.05f;
                        }
                    }
                }

            }
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

    }
}