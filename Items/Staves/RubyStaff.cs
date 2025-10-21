using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Staves
{
    public class RubyStaff : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.RubyStaff;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 34;
            entity.useTime = 47;
            entity.useAnimation = 47;
            entity.mana = 12;
            entity.knockBack = 8f;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 1.66f;
            
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 0.6f;
            
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            mult = 1.66f;
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            knockback *= 1.66f;
        }*/


    }
}