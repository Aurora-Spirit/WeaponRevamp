using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;
using WeaponRevamp.Projectiles.Swords;
using static Humanizer.In;

namespace WeaponRevamp.Items.Swords
{
    public class MythrilSword : GlobalItem
    {
        private bool magicSwing = true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            
            return item.type == ItemID.MythrilSword;
            
        }

        public override void SetDefaults(Item item)
        {
            item.shoot = ModContent.ProjectileType<MythrilSwordSlashProjectile>();
            item.useAnimation = 20;
            item.useTime = 20;
            item.damage = 55;
            base.SetDefaults(item);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            magicSwing = player.CheckMana((int)(15f * player.manaCost), true, false);
            if (magicSwing)
            {
                player.manaRegenDelay = (int)player.maxRegenDelay;
                float adjustedItemScale = player.GetAdjustedItemScale(item); // Get the melee scale of the player and item.
                Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, (int)(damage * (1-player.manaSickReduction)), knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            if (magicSwing)
                return false;
            else
                return base.CanHitNPC(item, player, target);
        }
        public override bool CanHitPvp(Item item, Player player, Player target)
        {
            if(magicSwing)
                return false;
            else
                return base.CanHitPvp(item, player, target);
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