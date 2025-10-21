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

namespace WeaponRevamp.Buffs.Swords
{
    public class PalladiumBleedHeal : ModBuff
    {
        public bool palladiumBleedOwner;

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;  // Is it a debuff?
            Main.pvpBuff[Type] = false; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = false; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PalladiumBleedHealPlayer>().palladiumBleedHeal = true;
        }

    }
    public class PalladiumBleedHealPlayer : ModPlayer
    {
        public bool palladiumBleedHeal;

        public override void ResetEffects()
        {
            palladiumBleedHeal = false;
        }

        // Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
        // This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
        // The player will take damage at a rate of half the number you subtract per second
        public override void UpdateLifeRegen()
        {
            if (palladiumBleedHeal)
            {
                Player.lifeRegen += 4;
            }
        }
    }
}
