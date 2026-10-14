using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;
using WeaponRevamp.Projectiles.Staves;

namespace WeaponRevamp.Items.Staves
{
	public class ThunderZapper : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.ThunderStaff;

        }
        
        public static SoundStyle thunderZapperSoundStyle;
        public override void Load() {
            thunderZapperSoundStyle = new SoundStyle("WeaponRevamp/Items/Staves/ThunderZapper")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 1f,
                Type = SoundType.Sound
            };
        }

        public override void SetDefaults(Item entity)
        {
            entity.shoot = ModContent.ProjectileType<ThunderZapperProjectile>();
            entity.shootSpeed = 8;
            entity.useTime = 8;
            entity.useAnimation = 8;
            entity.damage = 10;
            entity.knockBack = 1f;
            entity.mana = 3;
            entity.UseSound = thunderZapperSoundStyle;
        }

        
    }
}