using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Spears
{
    public class MythrilHalberd : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.MythrilHalberd;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 55;
        }

        // BLOCK TO COPY //
        public override void SetStaticDefaults()
        {
            //{this.Name}
            Tooltip = Mod.GetLocalization($"Tooltips."+this.Name);
        }
        public static LocalizedText Tooltip { get; private set; }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.Insert(5, new(Mod, "Tooltip", Tooltip.Value));
        }
        // END BLOCK //
    }
}