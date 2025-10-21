using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Dusts
{
    public class ShockwaveDust : ModDust
    {
        public override void Load()
        {
            /*if (Main.netMode != NetmodeID.Server)
            {
                Asset<Effect> shockwaveShader = Mod.Assets.Request<Effect>("Effects/Shockwave");

                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(shockwaveShader, "Shockwave"), EffectPriority.Medium);
            }*/
        }

        /*public override void OnSpawn(Dust dust)
        {

        
            


        }*/
        public override bool Update(Dust dust)
        {
            
            if (dust.fadeIn == 0f) 
            {
                //Main.NewText(Main.GameZoomTarget);
                dust.scale /= (float)Math.Pow(Main.GameZoomTarget, 2);
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave1"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave1", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);
                    dust.rotation = 1;

                }
                else if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave2"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave2", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);
                    dust.rotation = 2;

                }
                else if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave3"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave3", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);
                    dust.rotation = 3;

                }
                else if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave4"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave4", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);
                    dust.rotation = 4;

                }
                else if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave5"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave5", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);
                    dust.rotation = 5;

                }
                else if (Main.netMode != NetmodeID.Server)
                {
                    //dust.rotation = 5;
                    dust.active = false;
                    return false;
                    /*for (int i = 0; i < Main.dust.Length; i++)
                    {
                        if (Main.dust[i] != null && Main.dust[i].active && Main.dust[i].type == dust.type && i != dust.dustIndex)
                        {
                            Main.dust[i].active = false;
                        }
                    }
                    Filters.Scene["Shockwave5"].Deactivate();
                    Filters.Scene.Activate("Shockwave5", dust.position).GetShader().UseColor(2, dust.scale, 15).UseTargetPosition(dust.position);*/
                }
            }

            String ShockwaveID = "Shockwave" + (int)dust.rotation;
            //Main.NewText("Shockwave ID: " + ShockwaveID + ", Size: " + dust.scale + ", Progress: " + dust.fadeIn);


            if (Main.netMode != NetmodeID.Server && Filters.Scene[ShockwaveID].IsActive())
            {
                float progress = dust.fadeIn; //progress goes from 0.5f to 1.5f assuming dust scale is 1
                Filters.Scene[ShockwaveID].GetShader().UseProgress(progress+0f).UseOpacity(12000/Main.GameZoomTarget * (1f - progress));
                // / (float)Math.Pow(dust.scale, 2)) * 1000 * 1000
            }


            dust.fadeIn += 0.12f;
            if (dust.fadeIn > 1f)
            {
                dust.active = false;
                if (Main.netMode != NetmodeID.Server && Filters.Scene[ShockwaveID].IsActive())
                {
                    //Filters.Scene[ShockwaveID].GetShader().UseProgress(0f).UseOpacity(12000/Main.GameZoomTarget);
                    Filters.Scene[ShockwaveID].Deactivate();
                }
            }
            return false;
        }

        public override bool PreDraw(Dust dust)
        {
            return false;
        }

    }
}
