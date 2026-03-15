using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.MagicGuns;

namespace WeaponRevamp.Items.MagicGuns
{
	public class GrayZapinator : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.ZapinatorGray;

        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shoot = ModContent.ProjectileType<ZapinatorProjectile>();

        }
        

    }
}