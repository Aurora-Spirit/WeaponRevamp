using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Items.Guns
{
	public class PhoenixBlaster : GlobalItem
	{
        public static SoundStyle phoenixBlasterSoundStyle;
        public override void Load() {
            phoenixBlasterSoundStyle = new SoundStyle("WeaponRevamp/Items/Guns/PhoenixBlaster")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 1f,
                Type = SoundType.Sound
            };
        }
        
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.PhoenixBlaster;

        }

        public override void SetDefaults(Item entity)
        {
            entity.damage = 36;
            entity.UseSound = phoenixBlasterSoundStyle;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzlePosition = position + (Vector2.Normalize(velocity) * 60f * item.scale) + new Vector2(-4, -4);
            
            for (int i = 0; i < 6; i++)
            {
                Dust fire = Dust.NewDustPerfect(muzzlePosition-velocity*0.5f, ModContent.DustType<PhoenixLavaDust>(), -velocity*0.2f);
                fire.velocity += new Vector2(Main.rand.NextFloat(-1, 1),Main.rand.NextFloat(-1, 1)) * 5f;
                //fire.velocity += velocity * 0.3f;
                
                Dust smoke = Dust.NewDustDirect(muzzlePosition, 0, 0, ModContent.DustType<RisingSmokeDust>());
                smoke.velocity *= 2f;
                smoke.scale = 0.3f;
                fire.velocity += velocity * 0.5f;
            }
            
            Dust flash = Dust.NewDustDirect(muzzlePosition,0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            flash.rotation = velocity.ToRotation();
            
            
            Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, velocity, ModContent.ProjectileType<PhoenixBlasterProjectile>(), damage, knockback, type, player.whoAmI);
            
            return false;
        }
    }
    
    public class PhoenixBlasterRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ItemID.PhoenixBlaster))
                {
                    recipe.AddIngredient(ItemID.LivingFireBlock, 20);
                }
            }
        }
    }
}