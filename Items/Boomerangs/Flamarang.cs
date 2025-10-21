using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Bows
{
	public class Flamarang : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.Flamarang;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 42;
        }

    }
}