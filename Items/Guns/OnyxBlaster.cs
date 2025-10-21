using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Items.Guns
{
	public class OnyxBlaster : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.OnyxBlaster;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 48;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shardVel = Vector2.Normalize(velocity)*14f;
            Projectile shard = Projectile.NewProjectileDirect(source, position, shardVel, ProjectileID.BlackBolt, damage,knockback, player.whoAmI);
            shard.GetGlobalProjectile<OnyxBlasterProjectile>().source = source;
            shard.GetGlobalProjectile<OnyxBlasterProjectile>().ammoType = type;
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*72f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.rotation = velocity.ToRotation();
            flash.velocity *= 0f;
            flash.scale = 1f;
            return false;
        }
        
        
    }

    public class OnyxBlasterRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ItemID.OnyxBlaster) && recipe.HasIngredient(ItemID.Shotgun))
                {
                    recipe.RemoveIngredient(ItemID.Shotgun);
                    recipe.AddIngredient(ItemID.QuadBarrelShotgun, 1);
                }
            }
        }
    }
}