using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class ChlorophyteShotbow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.ChlorophyteShotbow;
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int num13 = 0; num13 < 3; num13++)
            {
                float num14 = velocity.X;
                float num15 = velocity.Y;
                if (num13 > 0)
                {
                    num14 += (float)Main.rand.Next(-35, 36) * 0.04f;
                    num15 += (float)Main.rand.Next(-35, 36) * 0.04f;
                }
                if (num13 > 1)
                {
                    num14 += (float)Main.rand.Next(-35, 36) * 0.04f;
                    num15 += (float)Main.rand.Next(-35, 36) * 0.04f;
                }
                if (num13 > 2)
                {
                    num14 += (float)Main.rand.Next(-35, 36) * 0.04f;
                    num15 += (float)Main.rand.Next(-35, 36) * 0.04f;
                }
                Projectile proj = UnifiedArrowProjectile.NewUnifiedArrow(source, position, new Vector2(num14, num15), ModContent.ProjectileType<ChlorophyteShotbowProjectile>(), damage, knockback, type, player.whoAmI);
                proj.noDropItem = true;
            }
            return false;

        }
        


    }
}