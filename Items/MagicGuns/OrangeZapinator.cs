using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.MagicGuns;

namespace WeaponRevamp.Items.MagicGuns
{
	public class OrangeZapinator : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.ZapinatorOrange;

        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shoot = ModContent.ProjectileType<ZapinatorProjectile>();
            entity.damage = 90;

        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0.5f);
            return false;
        }
    }
}