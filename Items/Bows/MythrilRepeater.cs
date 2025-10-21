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
    public class MythrilRepeater : GlobalItem
    {
        private bool magicShot = true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.MythrilRepeater;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 43;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<MythrilRepeaterProjectile>(), damage, knockback, type, player.whoAmI);
            magicShot = player.CheckMana((int)(15f * player.manaCost), true, false);
            if (magicShot)
            {
                player.manaRegenDelay = (int)player.maxRegenDelay;
                Projectile.NewProjectile(source, position, velocity*0.95f, ModContent.ProjectileType<MythrilManaArrowProjectile>(), (int)(damage*0.4*(1 - player.manaSickReduction)), knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity*0.9f, ModContent.ProjectileType<MythrilManaArrowProjectile>(), (int)(damage*0.4*(1 - player.manaSickReduction)), knockback, player.whoAmI);
            }
            return false;
        }

        public override bool InstancePerEntity => true;
        
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