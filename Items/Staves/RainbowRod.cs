using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Staves
{
    public class RainbowRod : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.RainbowRod;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 30;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));

            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, (float)Math.PI);

            return true;
        }

    }
}