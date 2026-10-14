using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Summons;

public class ImpStaff : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.ImpStaff;
    }

    public override void SetDefaults(Item entity)
    {
        entity.damage = 22;
    }
}