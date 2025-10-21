using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.MagicGuns;

namespace WeaponRevamp.Items.MagicGuns
{
	public class LeafBlower : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.LeafBlower;

        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 68;
            entity.shootSpeed = 12f;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Normalize(velocity), ModContent.ProjectileType<LeafBlowerWindProjectile>(), 0, 0);
            //Main.NewText(velocity.ToRotation());
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}