using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class PlatinumBow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.PlatinumBow;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 10;
            entity.shootSpeed = 7.9f;
            entity.crit = 16;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 0.9f;
            
        }*/

        /*public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            
            velocity = Vector2.Multiply(velocity, 1.2f);
            
        }*/
        /*public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            
            crit += 16f;

        }*/

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<PlatinumBowProjectile>(), damage, knockback, type, player.whoAmI);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(position + velocity, 0, 0, DustID.Platinum);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += velocity * 0.4f;
                dust.noGravity = true;
            }
            return false;
        }

    }
}