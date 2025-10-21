using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Items.Guns
{
	public class Gatligator : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.Gatligator;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 23;
            entity.useTime = 4;
            entity.useAnimation = 4;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*62f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            velocity += new Vector2(Main.rand.NextFloat() * 6 - 3, Main.rand.NextFloat() * 6 - 3);
            flash.rotation = velocity.ToRotation();
            Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, velocity, ModContent.ProjectileType<UnifiedBulletProjectile>(), damage, knockback, type, player.whoAmI);
            
            return false;
        }
    }
}