using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class ChlorophyteSaber : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.ChlorophyteSaber;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shootsEveryUse = true;
            entity.shootSpeed = 14f;
        }




    }
}