using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class DaedalusStormbow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.DaedalusStormbow;
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newPosition;
            Vector2 newVelocity = velocity;
            float acceleration;
            bool leftSide = true;
            //left side
            for(int i=0; i<4; i++)
            {
                if(leftSide)
                {
                    newPosition = player.position + new Vector2(-62 * 16, 0);
                } else
                {
                    newPosition = player.position + new Vector2(62 * 16, 0);
                }
                leftSide = !leftSide;
                
                if (Main.myPlayer == player.whoAmI)
                {
                    newVelocity = Vector2.Normalize(Main.MouseWorld - newPosition) * velocity.Length();
                }
                acceleration = Math.Sign(Main.rand.Next(0, 2) * 2 - 1) * (Main.rand.NextFloat() + 1f) * 0.05f;
                newVelocity.Y -= acceleration * Math.Abs(Main.MouseWorld.X - newPosition.X) * 0.03f;
                newVelocity.X += Main.rand.NextFloat() * 2 + 1;
                UnifiedArrowProjectile.NewUnifiedArrow(source, newPosition, newVelocity, ModContent.ProjectileType<DaedalusStormbowProjectile>(), damage, knockback, type, player.whoAmI, 0, Main.MouseWorld.X, acceleration);

            }
            return false;

        }
        


    }
}