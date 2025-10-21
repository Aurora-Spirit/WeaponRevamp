using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Items.Guns
{
	public class Minishark : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.Minishark;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.ArmorPenetration += 5;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*62f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            velocity += new Vector2(Main.rand.NextFloat() * 1 - 0.5f, Main.rand.NextFloat() * 1 - 0.5f);
            flash.rotation = velocity.ToRotation();
            Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, velocity, ModContent.ProjectileType<MinisharkProjectile>(), damage, knockback, type, player.whoAmI);
            
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
            tooltips.Insert(7, new(Mod, "Tooltip", Tooltip.Value));
        }
        // END BLOCK //
    }
}