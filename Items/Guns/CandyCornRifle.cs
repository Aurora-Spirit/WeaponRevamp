using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Guns
{
	public class CandyCornRifle : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.CandyCornRifle;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 53;
            entity.useAmmo = AmmoID.None;
        }


    }
}