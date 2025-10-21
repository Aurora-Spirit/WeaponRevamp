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
	public class ClockworkAssaultRifle : GlobalItem
    {
        public override bool InstancePerEntity => true;
        
        public ClockworkCogProjectile[] cogs = new ClockworkCogProjectile[12];
        public int cogIndex = 0;

        public static SoundStyle clockTick1SoundStyle;
        public static SoundStyle clockTick2SoundStyle;
        public static SoundStyle bellTollSoundStyle;
        
        public override void Load() {
            clockTick1SoundStyle = new SoundStyle("WeaponRevamp/Items/Guns/clockworkone")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 0.6f,
                Type = SoundType.Sound
            };
            clockTick2SoundStyle = new SoundStyle("WeaponRevamp/Items/Guns/clockworktwo")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 0.6f,
                Type = SoundType.Sound
            };
            bellTollSoundStyle = new SoundStyle("WeaponRevamp/Items/Guns/ClockworkBell")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 0.8f,
                Type = SoundType.Sound,
                Pitch = 0f
            };
        }

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.ClockworkAssaultRifle;

        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 22;
            entity.useTime = 18;
            entity.useAnimation = 18;
            entity.reuseDelay = 0;
            entity.UseSound = clockTick1SoundStyle;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < cogs.Length; i++)
            {
                if (cogs[i] != null && !cogs[i].Projectile.active)
                {
                    cogIndex = 0;
                    cogs = new ClockworkCogProjectile[cogs.Length];
                }
            }
            Dust flash = Dust.NewDustDirect(position + (Vector2.Normalize(velocity)*70f*item.scale)+new Vector2(-4,-4),0,0,ModContent.DustType<MuzzleFlashDust>());
            flash.velocity *= 0f;
            flash.scale = 1f;
            flash.rotation = velocity.ToRotation();
            Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, position, velocity, ModContent.ProjectileType<UnifiedBulletProjectile>(), damage, knockback, type, player.whoAmI);
            Vector2 cogVelocity;
            if (cogIndex == 0)
            {
                cogVelocity = Vector2.Normalize(velocity);
            }
            else
            {
                cogVelocity = cogs[cogIndex - 1].Projectile.velocity.RotatedBy(Math.PI * 2 / cogs.Length);
            }
            Projectile cog = Projectile.NewProjectileDirect(source, position, cogVelocity, ModContent.ProjectileType<ClockworkCogProjectile>(), damage, knockback, player.whoAmI, type, cogIndex);
            cogs[cogIndex] = (ClockworkCogProjectile)cog.ModProjectile;
            cogIndex += 1;
            if (cogIndex == cogs.Length)
            {
                cogIndex = 0;
                for (int i = 0; i < cogs.Length; i++)
                {
                    cogs[i].Shoot(source);
                }
                cogs = new ClockworkCogProjectile[cogs.Length];
                SoundEngine.PlaySound(SoundID.Item40, position);
                SoundEngine.PlaySound(bellTollSoundStyle, position);
            }
            if (item.UseSound.Equals(clockTick1SoundStyle))
            {
                item.UseSound = clockTick2SoundStyle;
                //SoundEngine.PlaySound(clockTick1SoundStyle, position);
            }
            else
            {
                item.UseSound = clockTick1SoundStyle;
                //SoundEngine.PlaySound(clockTick2SoundStyle, position);
            }
            
            return false;
        }
    }
}