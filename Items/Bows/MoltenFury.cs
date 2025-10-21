using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class MoltenFury : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.MoltenFury;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 20;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*if(source.AmmoItemIdUsed == ItemID.WoodenArrow|| source.AmmoItemIdUsed == ItemID.EndlessQuiver)
            {
                type = 1;
            }*/
            UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<MoltenFuryProjectile>(), damage, knockback, type, player.whoAmI);
            Projectile.NewProjectile(source, position, (Vector2.Normalize(velocity) * 20f), ModContent.ProjectileType<MoltenFuryGeyserProjectile>(), (int)(damage * 0.8f), knockback * 0.5f, player.whoAmI);
            return false;
        }
        




    }
}