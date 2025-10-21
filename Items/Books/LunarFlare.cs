using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Books;

namespace WeaponRevamp.Items.Books
{
    public class LunarFlare : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.LunarFlareBook;
        }

        public override void SetDefaults(Item item)
        {
            item.channel = true;
            item.shoot = ModContent.ProjectileType<LunarFlareMoonProjectile>();
            item.useStyle = ItemUseStyleID.HoldUp;
            item.damage = 220;
            item.shootSpeed = 0;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<LunarFlareMoonProjectile>()] <= 0;
        }

    }
}