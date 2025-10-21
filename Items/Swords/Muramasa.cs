using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class Muramasa : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            
            return item.type == ItemID.Muramasa;
            
        }

        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
            item.shoot = ModContent.ProjectileType<Projectiles.Utility.NothingProjectile>();
            item.useTurn = false;
            item.useTime = 16;
            item.useAnimation = 16;
            item.scale = 1.3f;
            
        }



    }
}