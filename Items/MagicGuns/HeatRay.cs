using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.MagicGuns
{
	public class HeatRay : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.HeatRay;

        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shootSpeed = 6f;
            entity.useTime = 5;
            entity.useAnimation = 5;
            entity.mana = 4;
            entity.damage = 63;

        }
        

    }
}