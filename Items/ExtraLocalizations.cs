using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WeaponRevamp.Items;

//this file is for those items that don't need the item itself to be reworked, but still need a localization. to avoid clutter.

public class LaserMachineGun : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.LaserMachinegun;
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
        tooltips.Insert(7, new(Mod, "Tooltip", Tooltip.Value));
    }
    // END BLOCK //
}

public class ShadowbeamStaff : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.ShadowbeamStaff;
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