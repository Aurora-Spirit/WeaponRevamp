using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Reflection.Emit;

namespace WeaponRevamp.Projectiles.Swords
{
    public class MeowmereProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.Meowmere;
        }
        

        /*private static void HookFixSound(ILContext il)
        {
            try
            {
                ILCursor c = new ILCursor(il);

                // Try to find where 566 is placed onto the stack
                c.GotoNext(i => i.MatchLdcR4(0.8));

                // Move the cursor after 566 and onto the ret op.
                c.Index++;
                c.Index++;
                // Push the Player instance onto the stack
                c.Emit(OpCodes.Ldc_I4_3);
                // Call a delegate using the int and Player from the stack.
                c.EmitDelegate<Func<int, SoundStyle, null>>((returnValue, player) => {
                    // Regular c# code
                    if (player.GetModPlayer<WaspNestPlayer>().strongBeesUpgrade && Main.rand.NextBool(10) && Main.ProjectileUpdateLoopIndex == -1)
                    {
                        return ProjectileID.Beenade;
                    }

                    return returnValue;
                });
            }
            catch (Exception e)
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<ExampleMod>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }*/
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            

            projectile.ai[0] += 1f;
            SoundStyle soundStyle = SoundID.Meowmere;
            soundStyle.MaxInstances = 5;
            SoundEngine.PlaySound(soundStyle, projectile.position);
            if (projectile.ai[0] >= 5f)
            {
                projectile.position += projectile.velocity;
                projectile.Kill();
            }
            else
            {
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = 0f - oldVelocity.Y;
                }
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = 0f - oldVelocity.X;
                }
            }
            Vector2 spinningpoint = new Vector2(0f, -3f - projectile.ai[0]).RotatedByRandom(3.1415927410125732);
            float num14 = 10f + projectile.ai[0] * 4f;
            Vector2 vector11 = new Vector2(1.05f, 1f);
            for (float num15 = 0f; num15 < num14; num15 += 1f)
            {
                int num16 = Dust.NewDust(projectile.Center, 0, 0, 66, 0f, 0f, 0, Color.Transparent);
                Main.dust[num16].position = projectile.Center;
                Main.dust[num16].velocity = spinningpoint.RotatedBy((float)Math.PI * 2f * num15 / num14) * vector11 * (0.8f + Main.rand.NextFloat() * 0.4f);
                Main.dust[num16].color = Main.hslToRgb(num15 / num14, 1f, 0.5f);
                Main.dust[num16].noGravity = true;
                Main.dust[num16].scale = 1f + projectile.ai[0] / 3f;
            }
            if (Main.myPlayer == projectile.owner)
            {
                int num17 = projectile.width;
                int num18 = projectile.height;
                int num19 = projectile.penetrate;
                projectile.position = projectile.Center;
                projectile.width = (projectile.height = 40 + 8 * (int)projectile.ai[0]);
                projectile.Center = projectile.position;
                projectile.penetrate = -1;
                projectile.Damage();
                projectile.penetrate = num19;
                projectile.position = projectile.Center;
                projectile.width = num17;
                projectile.height = num18;
                projectile.Center = projectile.position;
            }
            return false;
        }


        public override void AI(Projectile projectile)
        {
            if (projectile.penetrate % 2 == 0)
            {
                int targetID = projectile.FindTargetWithLineOfSight();
                //Main.NewText(targetID);
                if (targetID!=-1) {
                    NPC target = Main.npc[targetID];
                    projectile.velocity += Vector2.Normalize(target.position - projectile.position)*0.8f;
                }
            }
        }
    }
}
