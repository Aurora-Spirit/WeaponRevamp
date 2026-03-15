using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class BloodRainBow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.BloodRainBow;
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //size of the area that a rainbow instance can spawn in
            int rainbowBlockX = 800;
            int rainbowBlockY = 256;
            
            Vector2 newVelocity = velocity;
            float passThroughDestination;
            Vector2 target = Main.MouseWorld;
            Vector2 newPosition = new Vector2(Vector2.Lerp(target, position, 0.2f).X, position.Y + -16*45);
            Vector2 rainbowPos = new Vector2(Vector2.Lerp(target, position, 0.1f).X, position.Y + -16*20);
            Vector2 preciseRainbowPos = rainbowPos;
            Random seededRNG = new Random((int)((float)Math.Round(rainbowPos.X/rainbowBlockX)*2*(float)Math.Round(rainbowPos.Y/rainbowBlockY)*3)%int.MaxValue);
            rainbowPos = new Vector2((float)Math.Round(rainbowPos.X/rainbowBlockX)*rainbowBlockX-(rainbowBlockX/2f),(float)Math.Round(rainbowPos.Y/rainbowBlockY)*rainbowBlockY-(rainbowBlockY/2f));
            rainbowPos += new Vector2((float)seededRNG.NextDouble()*rainbowBlockX/2f + rainbowBlockX/4f, (float)seededRNG.NextDouble()*rainbowBlockY);
            rainbowPos += new Vector2(Main.rand.NextFloat(0,5), Main.rand.NextFloat(0,5));
            float distanceToRainbow = -rainbowPos.X + preciseRainbowPos.X;
            if (Collision.CanHit(player.position, player.width, player.height, target, 1, 1))
            {
                passThroughDestination = target.Y;
            }
            else
            {
                passThroughDestination = newPosition.Y;
            }
            for(int i=0; i<3; i++)
            {
                
                if (Main.myPlayer == player.whoAmI)
                {
                    newVelocity = Vector2.Normalize(target - newPosition) * velocity.Length();
                }
                
                newVelocity.X += Main.rand.NextFloat(-1.5f,1.5f);
                newPosition.X += Main.rand.NextFloat(-48,48);
                UnifiedArrowProjectile.NewUnifiedArrow(source, newPosition, newVelocity, ModContent.ProjectileType<BloodRainBowProjectile>(), damage, knockback, type, player.whoAmI, 0, passThroughDestination);

            }

            Projectile.NewProjectileDirect(source, rainbowPos, new Vector2(0, 0), ModContent.ProjectileType<BloodRainBowRainbow>(), 0, 0, player.whoAmI, 0, distanceToRainbow);
            return false;

        }
        


    }
}