using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Shortswords
{
    public class PlatinumShortsword : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.PlatinumShortsword;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 18;
            entity.crit = 16;
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 1.4f;
            
        }*/

        
       /* public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            
            crit += 16f;

        }*/

    }
}