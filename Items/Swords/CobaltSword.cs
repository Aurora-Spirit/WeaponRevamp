using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class CobaltSword : GlobalItem
    {
        private bool north = false;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            
            return item.type == ItemID.CobaltSword;
            
        }

        public override void SetDefaults(Item item)
        {

            base.SetDefaults(item);
            item.shoot = ModContent.ProjectileType<Projectiles.Swords.CobaltSwordNorthProjectile>();
            item.shootSpeed = 12f;
            item.useTurn = false;
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Main.NewText(north);
            SoundEngine.PlaySound(SoundID.Item20, player.position);
            if (north)
            {
                type = ModContent.ProjectileType<Projectiles.Swords.CobaltSwordNorthProjectile>();
                item.shoot = ModContent.ProjectileType<Projectiles.Swords.CobaltSwordNorthProjectile>();
            }
            else
            {
                type = ModContent.ProjectileType<Projectiles.Swords.CobaltSwordSouthProjectile>();
                item.shoot = ModContent.ProjectileType<Projectiles.Swords.CobaltSwordSouthProjectile>();
            }
            north = !north;
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
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