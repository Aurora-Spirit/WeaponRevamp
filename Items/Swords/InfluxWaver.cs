using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WeaponRevamp.Items.Swords
{
    public class InfluxWaver : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            
            return item.type == ItemID.InfluxWaver;
            
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 80;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            item.shoot = ModContent.ProjectileType<Projectiles.Swords.InfluxWaverLaserProjectile>();
            Projectile.NewProjectileDirect(source, player.Center, velocity*2, item.shoot, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool? UseItem(Item item, Player player)
        {
            SoundEngine.PlaySound(SoundID.Item15);
            return null;
        }




    }
}