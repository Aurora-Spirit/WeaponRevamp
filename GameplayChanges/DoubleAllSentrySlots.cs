using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WeaponRevamp.GameplayChanges;

public class DoubleSentrySlotsPlayer : ModPlayer
{
    public override void PostUpdateEquips()
    {
        Player.maxTurrets *= 2;
    }
}

public class DoubleSentrySlotsItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.sentry;
    }

    // BLOCK TO COPY //
    public override void SetStaticDefaults()
    {
        //{this.Name}
        Tooltip = Mod.GetLocalization($"Tooltips.DoubleSentrySlots");
    }

    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);
        entity.useTime /= 2;
        entity.useAnimation /= 2;
    }

    public static LocalizedText Tooltip { get; private set; }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        tooltips.Insert(5, new(Mod, "Tooltip", Tooltip.Value));
    }
    // END BLOCK //
}