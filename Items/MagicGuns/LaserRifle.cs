using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Items.MagicGuns
{
	public class LaserRifle : GlobalItem
	{
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.LaserRifle;

        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.useTime = 9;
            entity.useAnimation = 9;
            entity.mana = 6;

        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //velocity = velocity.RotatedByRandom(Math.PI*(5f/360f)*2f);
            Projectile laser = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 spawnPosition = laser.position + laser.velocity * 2.5f;
            int totalRingDusts = 20;
            for (int i = 0; i < totalRingDusts; i++)
            {
                Dust ringDust = Dust.NewDustDirect(spawnPosition, 0, 0, ModContent.DustType<LaserRifleDust>());
                ringDust.rotation = (float)(Math.PI * 2)*(float)i/(float)totalRingDusts;
                ringDust.scale = 1f;
                ringDust.velocity = new Vector2(1,0).RotatedBy(ringDust.rotation);
                //Main.NewText("Before: " + ringDust.velocity.Length());
                ringDust.velocity.X *= 0.4f;
                ringDust.fadeIn = ringDust.velocity.Length() * -1.5f;
                //Main.NewText("After: " + ringDust.velocity.Length());

                ringDust.velocity = ringDust.velocity.RotatedBy(laser.velocity.ToRotation()) * 6f;
                ringDust.rotation = ringDust.velocity.ToRotation();

            }
            
            return false;
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            if (player.spaceGun)
            {
                mult = 0f;
            }
        }
    }
}