using System;
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
	public class Shotgun : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.Shotgun;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.useTime = 50;
            entity.useAnimation = 50;
        }

        private static int bulletCount = 4;
        
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * velocity.Length();
                //player.itemRotation = (float)( ( (velocity.ToRotation()+Math.PI) % Math.PI ) - Math.PI);
                player.itemRotation = (float)( velocity.ToRotation() );
                if (player.direction == -1)
                {
                    player.itemRotation -= (float)Math.PI*Math.Sign(player.itemRotation);
                }
            }
            SoundEngine.PlaySound(item.UseSound, player.position);
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*52f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            // new Vector2(Main.rand.NextFloat() * 6 - 3, Main.rand.NextFloat() * 6 - 3)
            //velocity = velocity.RotatedByRandom(Math.PI/6);
            flash.rotation = velocity.ToRotation();
            
            for (int i=0;i<bulletCount;i++)
            {
                Vector2 bulletVel = velocity.RotatedByRandom(Math.PI / 8);
                Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, bulletVel, ModContent.ProjectileType<ShotgunProjectile>(), damage, knockback, type, player.whoAmI);
                for (int j = 0; j < 5; j++)
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
                smoke.velocity *= 2.5f;
                smoke.velocity += velocity*0.4f;
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