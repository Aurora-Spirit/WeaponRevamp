using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Staves
{
    public class SpectreStaff : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.SpectreStaff;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shootSpeed *= 2;
            entity.damage = 81;
        }



    }
}