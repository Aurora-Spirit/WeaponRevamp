using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class GoldBow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.GoldBow;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 21;
            entity.useTime = 42;
            entity.useAnimation = 42;
            entity.knockBack = 2;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 1.85f;
            
        }*/

        /*public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 0.625f;
            
        }*/

        /*public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            knockback *= 1.6f;
        }*/

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<GoldBowProjectile>(), damage, knockback, type, player.whoAmI);
            for(int i=0;i<3;i++)
            {
                Dust dust = Dust.NewDustDirect(position + velocity, 0, 0, DustID.Gold);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += velocity * 0.4f;
                dust.noGravity = true;
            }
            
            return false;
        }

        

    }
}