using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Shortswords
{
    public class GoldShortsword : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.GoldShortsword;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 27;
            entity.useTime = 16;
            entity.useAnimation = 16;
            entity.knockBack = 8;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 2.25f;
            
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 0.67f;

        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            knockback *= 1.5f;
        }*/



    }
}