using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class LaserMachinegunProjectile : GlobalProjectile
    {
        /*public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.timeLeft = 600;
            entity.extraUpdates = 200;
        }*/
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.LaserMachinegun;
        }
        public override bool PreAI(Projectile projectile)
        {
            //fuck i am so sorry about all the terrible variable names i just needed to tweak like one number and I didnt want to IL edit so i just copy pasted the decompiled source code
            Player player = Main.player[projectile.owner];
            float num = (float)Math.PI / 2f;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            int num12 = 2;
            float num23 = 0f;

            projectile.ai[0] += 1f;
            int num34 = 0;
            if (projectile.ai[0] >= 40f)
            {
                num34++;
            }
            if (projectile.ai[0] >= 80f)
            {
                num34++;
            }
            if (projectile.ai[0] >= 120f)
            {
                num34++;
            }
            int num45 = 24;
            int num56 = 6;
            projectile.ai[1] += 1f;
            bool flag = false;
            if (projectile.ai[1] >= (float)(num45 - num56 * num34))
            {
                projectile.ai[1] = 0f;
                flag = true;
            }
            projectile.frameCounter += 1 + num34;
            if (projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.soundDelay <= 0)
            {
                projectile.soundDelay = num45 - num56 * num34;
                if (projectile.ai[0] != 1f)
                {
                    SoundEngine.PlaySound(in SoundID.Item91, projectile.position);
                }
            }
            if (projectile.ai[1] == 1f && projectile.ai[0] != 1f)
            {
                Vector2 spinningpoint = Vector2.UnitX * 24f;
                spinningpoint = spinningpoint.RotatedBy(projectile.rotation - (float)Math.PI / 2f);
                Vector2 vector12 = projectile.Center + spinningpoint;
                for (int i = 0; i < 2; i++)
                {
                    int num66 = Dust.NewDust(vector12 - Vector2.One * 8f, 16, 16, 135, projectile.velocity.X / 2f, projectile.velocity.Y / 2f, 100);
                    Main.dust[num66].velocity *= 0.66f;
                    Main.dust[num66].noGravity = true;
                    Main.dust[num66].scale = 1.4f;
                }
            }
            if (flag && Main.myPlayer == projectile.owner)
            {
                if (player.channel && player.CheckMana(player.inventory[player.selectedItem], -1, pay: true) && !player.noItems && !player.CCed)
                {
                    float num77 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                    Vector2 vector23 = vector;
                    Vector2 value = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector23;
                    if (player.gravDir == -1f)
                    {
                        value.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector23.Y;
                    }
                    Vector2 vector34 = Vector2.Normalize(value);
                    if (float.IsNaN(vector34.X) || float.IsNaN(vector34.Y))
                    {
                        vector34 = -Vector2.UnitY;
                    }
                    vector34 *= num77;
                    if (vector34.X != projectile.velocity.X || vector34.Y != projectile.velocity.Y)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = vector34;
                    int num79 = 440;
                    float num2 = 14f;
                    int num3 = 7;
                    float spreadMult = 3.3f - (num34*1f) + (player.manaSickReduction*8);
                    //num3 += (int)(player.manaSickReduction*5);
                    for (int j = 0; j < 2; j++)
                    {
                        vector23 = projectile.Center + new Vector2(Main.rand.Next(-num3, num3 + 1), Main.rand.Next(-num3, num3 + 1));
                        Vector2 spinningpoint3 = Vector2.Normalize(projectile.velocity) * num2;
                        spinningpoint3 = spinningpoint3.RotatedBy((Main.rand.NextDouble() * 0.19634954631328583 - 0.09817477315664291)*spreadMult);
                        if (float.IsNaN(spinningpoint3.X) || float.IsNaN(spinningpoint3.Y))
                        {
                            spinningpoint3 = -Vector2.UnitY;
                        }
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), vector23.X, vector23.Y, spinningpoint3.X, spinningpoint3.Y, num79, projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }
                else
                {
                    projectile.Kill();
                }
            }




            projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - projectile.Size / 2f;
            projectile.rotation = projectile.velocity.ToRotation() + num;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.SetDummyItemTime(num12);
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction) + num23);


            return false;


        }

        /*public override void OnKill(Projectile projectile, int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.oldPosition + projectile.oldVelocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= projectile.oldVelocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            base.OnKill(projectile, timeLeft);
        }*/
        /*public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.position + projectile.velocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= projectile.velocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            if(target.HasBuff(BuffID.OnFire3))
            {
                if(target.buffTime[target.FindBuffIndex(BuffID.OnFire3)] < 600)
                {
                    target.AddBuff(BuffID.OnFire3, target.buffTime[target.FindBuffIndex(BuffID.OnFire3)] + 20);
                }
            } else
            {
                target.AddBuff(BuffID.OnFire3, 20);
            }
            
            base.OnHitNPC(projectile, target, hit, damageDone);
        }*/
        /*public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            
            for (int i = 0; i < 40; i++)
            {
                Dust heat = Dust.NewDustDirect(projectile.oldPosition + oldVelocity * 1f, 0, 0, DustID.HeatRay);
                heat.velocity *= 2f;
                heat.velocity -= oldVelocity * 0.5f;
                heat.scale = (float)Main.rand.Next(90, 130) * 0.013f;
            }
            return true;
        }*/
        //public override bool InstancePerEntity => true;
    }
}
