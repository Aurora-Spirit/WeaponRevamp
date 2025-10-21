using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Staves
{
    public class DiamondStaff : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.DiamondStaff;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 20;
            entity.shootSpeed = 14.25f;
            entity.crit = 16;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 0.9f;
            
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            
            velocity = Vector2.Multiply(velocity, 1.5f);
            
        }
        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            
            crit += 16f;

        }*/

    }
}