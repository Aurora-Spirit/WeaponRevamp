using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Items.Guns
{
	public class QuadBarrelShotgun : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.QuadBarrelShotgun;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 10;
            entity.useTime = 3;
            entity.useAnimation = 65;
            entity.useLimitPerAnimation = 4;
            entity.consumeAmmoOnFirstShotOnly = true;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(item.UseSound, player.position);
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*54f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            // new Vector2(Main.rand.NextFloat() * 6 - 3, Main.rand.NextFloat() * 6 - 3)
            //velocity = velocity.RotatedByRandom(Math.PI/6);
            flash.rotation = velocity.ToRotation();
            
            for (int i=0;i<5;i++)
            {
                Vector2 bulletVel = velocity.RotatedByRandom(Math.PI / 6);
                Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, bulletVel, ModContent.ProjectileType<ShotgunPelletProjectile>(), damage, knockback, type, player.whoAmI);
                for (int j = 0; j < 3; j++)
                {
                    Dust smoke = Dust.NewDustDirect(position+(Vector2.Normalize(velocity)*40f*item.scale),0,0,DustID.Smoke,0,0,0,new Color(0.5f,0.5f,0.5f,1f));
                    smoke.velocity *= 0.1f;
                    smoke.velocity += bulletVel*(0.5f+Main.rand.NextFloat())*2f;
                    smoke.scale = Main.rand.NextFloat() + 0.5f;
                    smoke.noGravity = true;
                }
            }
            
            for (int j = 0; j < 15; j++)
            {
                Dust smoke = Dust.NewDustDirect(position+(Vector2.Normalize(velocity)*40f*item.scale),0,0,DustID.Smoke,0,0,0,new Color(0.5f,0.5f,0.5f,1f));
                smoke.velocity *= 3;
                smoke.velocity += velocity*0.5f;
                smoke.scale = Main.rand.NextFloat() + 0.5f;
                smoke.noGravity = true;
            }
            for (int j = 0; j < 5; j++)
            {
                Dust flame = Dust.NewDustDirect(position+(Vector2.Normalize(velocity)*40f*item.scale),0,0,DustID.Torch);
                flame.velocity *= 3;
                flame.velocity += velocity*0.5f;
                flame.scale = Main.rand.NextFloat() + 0.5f;
                flame.noGravity = true;
            }
            
            return false;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
            ref int damage, ref float knockback)
        {
            if (player.itemAnimation != 55)
            {
                velocity = velocity.RotatedByRandom(Math.PI/6);
                
            }
        }
    }
}