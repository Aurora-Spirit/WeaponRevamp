using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;
using WeaponRevamp.Projectiles.Launchers;

namespace WeaponRevamp.Items.Launchers
{
    public class Celebration : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.FireworksLauncher;
        }

        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
            //entity.useTime = 28;
            //entity.useAnimation = 28;
            item.useAnimation = 18;
            item.useTime = 6; // one third of useAnimation
            item.reuseDelay = 20;
            item.consumeAmmoOnLastShotOnly = true;
            item.shootSpeed = 15;
            item.UseSound = SoundID.Item42;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int ammoType = 0;
            switch (source.AmmoItemIdUsed)
            {
                case ItemID.RocketI: ammoType = 0; break;
                case ItemID.RocketII: ammoType = 1; break;
                case ItemID.RocketIII: ammoType = 2; break;
                case ItemID.RocketIV: ammoType = 3; break;
                case ItemID.ClusterRocketI: ammoType = 4; break;
                case ItemID.ClusterRocketII: ammoType = 5; break;
                case ItemID.WetRocket: ammoType = 6; break;
                case ItemID.LavaRocket: ammoType = 7; break;
                case ItemID.HoneyRocket: ammoType = 8; break;
                case ItemID.MiniNukeI: ammoType = 9; break;
                case ItemID.MiniNukeII: ammoType = 10; break;
                case ItemID.DryRocket: ammoType = 11; break;

            }




            position.Y -= 6;
            for(int i=0;i<1;i++)
            {
                Vector2 newVel = velocity + new Vector2((Main.rand.NextFloat()-0.5f)*6, (Main.rand.NextFloat() - 0.5f)*6);
                Projectile proj = (Projectile.NewProjectileDirect(source, position + velocity, newVel, ModContent.ProjectileType<CelebrationProjectile>(), damage, knockback, player.whoAmI));

                if (proj.ModProjectile is CelebrationProjectile rocket)
                {
                    rocket.fireType = Main.rand.Next(0,4);

                    rocket.cluster = false;
                    rocket.destroyTiles = false;
                    rocket.liquidType = 0;
                    switch (ammoType)
                    {
                        case 0:
                        case 1:
                            rocket.radius = 10;
                            break;
                        case 2:
                        case 3:
                            rocket.radius = 12;
                            break;
                        case 4:
                        case 5:
                            rocket.radius = 8;
                            rocket.cluster = true;
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 11:
                            rocket.radius = 5;
                            break;
                        case 9:
                        case 10:
                            rocket.radius = 14;
                            break;

                    }
                    switch(ammoType)
                    {
                        case 1:
                        case 3:
                        case 5:
                        case 10:
                            rocket.destroyTiles = true;
                            break;
                        case 6:
                            rocket.liquidType = 1;
                            break;
                        case 7:
                            rocket.liquidType = 2;
                            break;
                        case 8:
                            rocket.liquidType = 3;
                            break;
                        case 11:
                            rocket.liquidType = 4;
                            break;

                    }
                    rocket.ammoSprite = ammoType;
                }


            }
            return false;
        }



    }
}