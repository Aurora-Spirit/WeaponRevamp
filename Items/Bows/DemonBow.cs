using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class DemonBow : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.DemonBow;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<Projectiles.Bows.DemonBowProjectile>(), damage, knockback, type, player.whoAmI);
            return false;
        }
        
        // BLOCK TO COPY //
        public override void SetStaticDefaults()
        {
            //{this.Name}
            Tooltip = Mod.GetLocalization($"Tooltips."+this.Name);
        }
        public static LocalizedText Tooltip { get; private set; }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.Insert(5, new(Mod, "Tooltip", Tooltip.Value));
        }
        // END BLOCK //
        

    }
}