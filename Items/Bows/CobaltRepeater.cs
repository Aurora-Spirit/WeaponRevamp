using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows
{
    public class CobaltRepeater : GlobalItem
    {
        private bool north = false;
        private bool switchPole = true;
        private bool quickFireReady = true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.CobaltRepeater;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 48;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.GetModPlayer<CobaltRepeaterPlayer>().quickFireCounter <= 0)
            {
                quickFireReady = true;
            }
            if (north)
            {
                UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<CobaltRepeaterNorthProjectile>(), damage, knockback, type, player.whoAmI);
            }
            else
            {
                UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<CobaltRepeaterSouthProjectile>(), damage, knockback, type, player.whoAmI);
            }
            if (switchPole) { north = !north; }
            switchPole = !switchPole;
            if (quickFireReady)
            {
                player.itemTime = 10;
                player.itemAnimation = 10;
                quickFireReady = false;

                player.GetModPlayer<CobaltRepeaterPlayer>().quickFireCounter = 23;
            }
            return false;
        }

        /*public override void HoldItem(Item item, Player player)
        {
            
        }*/
        
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

        public override bool InstancePerEntity => true;

    }

    public class CobaltRepeaterPlayer : ModPlayer
    {
        public int quickFireCounter = 0;
        public override void PostUpdateMiscEffects()
        {
            if (quickFireCounter > 0) { quickFireCounter--; }
        }
    }
}