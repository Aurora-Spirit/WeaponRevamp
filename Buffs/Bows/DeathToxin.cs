using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using WeaponRevamp.Dusts;
using Terraria.GameContent.ItemDropRules;

namespace WeaponRevamp.Buffs.Bows
{
    public class DeathToxin : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<DeathToxinNPC>().deathToxin = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<DeathToxinPlayer>().deathToxin = true;
        }

    }
    public class DeathToxinPlayer : ModPlayer
    {
        public bool deathToxin;

        public override void ResetEffects()
        {
            deathToxin = false;
        }

        // Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
        // This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
        // The player will take damage at a rate of half the number you subtract per second
        public override void UpdateBadLifeRegen()
        {
            if (deathToxin)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                // Player.lifeRegenTime used to increase the speed at which the player reaches its maximum natural life regeneration
                // So we set it to 0, and while this debuff is active, it never reaches it
                Player.lifeRegenTime = 0;
                // lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second
                Player.lifeRegen -= 6000;
            }
        }
    }
    public class DeathToxinNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool deathToxin = false;

        public override void ResetEffects(NPC npc)
        {
            deathToxin = false;
            base.ResetEffects(npc);
        }
        public override void PostAI(NPC npc)
        {
            if(deathToxin && Main.GameUpdateCount%2==0)
            {
                Dust dust13 = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<TintableOpaqueDustLighted>(), 0, 0, 0, Color.Lerp(new Color(64, 0, 192), new Color(0, 192, 0), Main.rand.NextFloat()), 1.3f + Main.rand.NextFloat() * 0.5f);
                dust13.noGravity = true;
                dust13.velocity *= 1f;
                //dust13.fadeIn = dust13.scale + 0.05f;
                Dust dust14 = Dust.CloneDust(dust13);
                //dust14.type = DustID.TintableDust;
                dust14.color = Color.Lerp(dust14.color, Color.Black, 0.7f);
                dust14.scale -= 0.3f;
                //npc.color = new Color(64, 0, 128);
            }
            base.PostAI(npc);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (deathToxin)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= 8000;
                damage = 200;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            // This simple color effect indicates that the buff is active
            if (deathToxin)
            {
                Color debuffColor = Color.Lerp(new Color(0f, 0.2f, 0.1f), new Color(0.6f, 0f, 1.2f), (float)Math.Cbrt(Math.Abs(((Main.GameUpdateCount%60f)/30f) - 1f)));
                //Main.NewText("R:"+debuffColor.R+" G:"+debuffColor.G+" B:"+debuffColor.B);
                drawColor = Main.buffColor(drawColor, debuffColor.R / 256f, debuffColor.G / 256f, debuffColor.B / 256f, 1f);
            }
        }
    }
}
