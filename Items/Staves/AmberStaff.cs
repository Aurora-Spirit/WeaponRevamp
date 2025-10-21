using Microsoft.Xna.Framework;
using System.Security.Policy;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Staves
{
    public class AmberStaff : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.AmberStaff;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.knockBack = 10f;
            entity.shootSpeed = 4.5f;
        }

        /*public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            
            velocity = Vector2.Multiply(velocity, 0.5f);
            
        }
        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            
            knockback *= 2f;

        }*/

    }
}