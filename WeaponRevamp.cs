using Mono.Cecil.Cil;
using MonoMod.Cil;
using static Mono.Cecil.Cil.OpCodes;
/*using Mono;
using MonoMod;
using System;*/
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using ReLogic.Content;
/*using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.GameContent.Skies;
using Terraria.GameContent;
using Terraria.GameContent.Animations;*/

namespace WeaponRevamp
{
	public partial class WeaponRevamp : Mod
	{

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Asset<Effect> screenRef = Assets.Request<Effect>("Effects/ShockwaveEffect"); // The path to the compiled shader file.
                Filters.Scene["Shockwave1"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave1"].Load();
                Filters.Scene["Shockwave2"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave2"].Load();
                Filters.Scene["Shockwave3"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave3"].Load();
                Filters.Scene["Shockwave4"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave4"].Load();
                Filters.Scene["Shockwave5"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave5"].Load();
            }
        }

        /*public override void Load()
        {
            IL_CreditsRollComposer.FillSegments += HookFillSegments;
            base.Load();
        }
        private static void HookFillSegments(ILContext il)
        {


            try
            {
                // Start the Cursor at the start
                var c = new ILCursor(il);
                // Try to find where 566 is placed onto the stack
                c.Goto(150);

                // Move the cursor after 566 and onto the ret op.
                var label = il.DefineLabel(); // Make a label that will point to the instruction pushing 566 to the stack

                c.Emit(Ldloc_0);
                c.Emit(Ldarg_0);
                c.Emit(Ldloc_0);
                c.Emit(OpCodes.Call, typeof(CreditsRollComposer).GetMethod("PlaySegment_GuideEmotingAtRainbowPanel", new Type[] { typeof(Terraria.GameContent.Skies.CreditsRoll.CreditsRollComposer), typeof(int) }));
                c.Emit(Ldfld, typeof(SegmentInforReport).GetField(nameof(SegmentInforReport.totalTime)));
                c.Emit(Add);
                c.Emit(Stloc_0);
                c.Emit(Ldloc_0);
                c.Emit(Ldloc_2);
                c.Emit(Add);
                c.Emit(Stloc_0);

                c.MarkLabel(label); // The cursor is still pointing to the ldc.i4 566 instruction, this label gives the branch instructions a destination

                
            }
            catch (Exception e)
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<WeaponRevamp>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }*/

    }
}