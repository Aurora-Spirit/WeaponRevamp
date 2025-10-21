using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using WeaponRevamp.Projectiles.Swords;

namespace WeaponRevamp.Items.Swords
{
    public class Meowmere : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            
            return item.type == ItemID.Meowmere;
            
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 160;
            entity.shootsEveryUse = true;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(item); // Get the melee scale of the player and item.
            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<MeowmereSlashProjectile>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.
            damage = (int)(damage * 0.5f);
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            return false;
        }
        public override bool CanHitPvp(Item item, Player player, Player target)
        {
            return false;
        }
        



    }
}