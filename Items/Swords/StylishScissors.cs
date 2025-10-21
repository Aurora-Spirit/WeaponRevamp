using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class StylishScissors : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return false; //disabled because unfinished
            return item.type == ItemID.StylistKilLaKillScissorsIWish;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            switch (player.hairDye)
            {
                case 3:
                    damage *= 1f + (float)(0.2f * ((float)player.position.Y / Main.bottomWorld));
                    break;
                case 6:

                    break;
            }

            }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            switch (player.hairDye)
            {
                case 3:
                    return 1f + (float)(0.2f * ((float)(Main.bottomWorld-player.position.Y) / Main.bottomWorld));
                    break;


            }
            return 1f;

        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {

            if(player.hairDye == 5)
            {
                double totalDayLength = Main.dayLength + Main.nightLength;
                double currentTime = Main.time;
                if (!Main.dayTime)
                {
                    currentTime += Main.dayLength;
                }
                //adjust time to start at 0 at noon
                currentTime -= Main.dayLength / 2;
                if (currentTime < 0)
                {
                    currentTime += totalDayLength;
                }
                //bounce 0 off of midnight ig lmao
                if (currentTime > totalDayLength / 2)
                {
                    currentTime = totalDayLength - currentTime;
                }
                //now we can modify scale
                crit += (float)(20 * (currentTime) / (totalDayLength / 2));
            }

        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            switch (player.hairDye)
            {
                case 5:
                    double totalDayLength = Main.dayLength + Main.nightLength;
                    double currentTime = Main.time;
                    if(!Main.dayTime)
                    {
                        currentTime += Main.dayLength;
                    }
                    //adjust time to start at 0 at noon
                    currentTime -= Main.dayLength / 2;
                    if(currentTime < 0)
                    {
                        currentTime += totalDayLength;
                    }
                    //bounce 0 off of midnight ig lmao
                    if(currentTime > totalDayLength/2)
                    {
                        currentTime = totalDayLength - currentTime;
                    }
                    //now we can modify scale
                    scale *= 1f + (float)(0.2 * ((totalDayLength / 2) - currentTime) / (totalDayLength/2));
                    break;
                case 9:
                    scale *= 1f + (float)(0.2 * ((float)Main.DiscoB / 255f));
                    break;
            }
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            if (player.hairDye == 7)
            {
                //desert hair dye
            }
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            switch (player.hairDye)
            {
                case 1: //life
                    if(target.type != NPCID.TargetDummy)
                    {
                        player.Heal(1);
                    }
                    break;

                case 2: //mana
                    if(player.statMana + 20 > player.statManaMax2)
                    {
                        player.statMana = player.statManaMax2;
                    } 
                    else
                    {
                        player.statMana += 20;
                    }
                    break;
                case 3: //depth
                    break;
                case 4: //money
                    target.AddBuff(BuffID.Midas, 120);

                    break;
                case 5: //time
                    break;
                case 6: //team
                    break;
                case 7: //biome
                    //SOMETHING WILL NEED TO BE ADDED HERE LATER
                    break;
                case 8: //party
                    for (int num955 = 0; num955 < 8; num955++)
                    {
                        int num956 = Dust.NewDust(target.Center, 1, 1, 219 + Main.rand.Next(5));
                        Dust dust106 = Main.dust[num956];
                        Dust dust334 = dust106;
                        dust334.velocity *= 1.4f;
                        Main.dust[num956].fadeIn = 1f;
                        Main.dust[num956].noGravity = true;
                    }
                    for (int num957 = 0; num957 < 15; num957++)
                    {
                        int num958 = Dust.NewDust(target.Center, 1, 1, 139 + Main.rand.Next(4), 0f, 0f, 0, default(Color), 1.6f);
                        Main.dust[num958].noGravity = true;
                        Dust dust107 = Main.dust[num958];
                        Dust dust334 = dust107;
                        dust334.velocity *= 5f;
                        num958 = Dust.NewDust(target.Center, 1, 1, 139 + Main.rand.Next(4), 0f, 0f, 0, default(Color), 1.9f);
                        dust107 = Main.dust[num958];
                        dust334 = dust107;
                        dust334.velocity *= 3f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int num962 = Gore.NewGore(null, target.Center, default(Vector2), Main.rand.Next(276, 283));
                        Gore gore28 = Main.gore[num962];
                        Gore gore64 = gore28;
                        gore64.velocity *= 0.4f;
                        Main.gore[num962].velocity.X += 1f;
                        Main.gore[num962].velocity.Y += 1f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int num961 = Gore.NewGore(null, target.Center, default(Vector2), Main.rand.Next(276, 283));
                        Gore gore27 = Main.gore[num961];
                        Gore gore64 = gore27;
                        gore64.velocity *= 0.4f;
                        Main.gore[num961].velocity.X -= 1f;
                        Main.gore[num961].velocity.Y += 1f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int num960 = Gore.NewGore(null, target.Center, default(Vector2), Main.rand.Next(276, 283));
                        Gore gore26 = Main.gore[num960];
                        Gore gore64 = gore26;
                        gore64.velocity *= 0.4f;
                        Main.gore[num960].velocity.X += 1f;
                        Main.gore[num960].velocity.Y -= 1f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int num959 = Gore.NewGore(null, target.Center, default(Vector2), Main.rand.Next(276, 283));
                        Gore gore25 = Main.gore[num959];
                        Gore gore64 = gore25;
                        gore64.velocity *= 0.4f;
                        Main.gore[num959].velocity.X -= 1f;
                        Main.gore[num959].velocity.Y -= 1f;
                    }
                    break;
                case 9: //rainbow
                    break;
                case 10: //speed
                    break;
                case 11: //martian
                    target.AddBuff(BuffID.Electrified, 600);
                    player.AddBuff(BuffID.Electrified, 60);
                    break;
                case 12: //twilight
                    float x = target.position.X + (float)Main.rand.Next(-400, 400);
                    float y = target.position.Y - (float)Main.rand.Next(600, 900);
                    Vector2 vector53 = new Vector2(x, y);
                    float num575 = target.position.X + (float)(target.width / 2) - vector53.X;
                    float num576 = target.position.Y + (float)(target.height / 2) - vector53.Y;
                    int num577 = 22;
                    float num578 = (float)Math.Sqrt(num575 * num575 + num576 * num576);
                    num578 = (float)num577 / num578;
                    num575 *= num578;
                    num576 *= num578;
                    int num579 = damageDone;
                    //num579 /= 3;
                    int num580 = Projectile.NewProjectile(item.GetSource_FromThis(), x, y, num575, num576, 92, num579/2, hit.Knockback);
                    Main.projectile[num580].ai[1] = target.position.Y;
                    Main.projectile[num580].ai[0] = 1f;
                    
                    break;

            }
        }

        


    }
}