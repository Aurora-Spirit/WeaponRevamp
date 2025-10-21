using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Bows;
using WeaponRevamp.Projectiles.Bows.Genesis;

namespace WeaponRevamp.Items.Bows
{
    public class Genesis : ModItem
    {
        public override void SetDefaults()
        {

            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 50;
            Item.height = 18;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.damage = 510;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.value = 1000000;
            Item.DamageType = DamageClass.Ranged;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 2f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            switch (source.AmmoItemIdUsed)
            {

                case ItemID.WoodenArrow:
                    type = ModContent.ProjectileType<GenesisGoldBowProjectile>();
                    position.X += 8;
                    //SoundEngine.PlaySound(SoundID.Item67, position); //moved to projectile
                    for (int i = 0; i < 10; i++)
                    {
                        Dust light = Dust.NewDustDirect(position, 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                        light.scale = 1f;
                        light.velocity *= 1f;
                        light.velocity += velocity * 0.5f;
                    }
                    break;

                case ItemID.EndlessQuiver:
                    damage = (int)(damage * 0.3f);
                    type = ProjectileID.WoodenArrowFriendly;
                    velocity += new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() - 0.5f) * 6;
                    position += velocity + new Vector2(Main.rand.NextFloat()-0.5f, Main.rand.NextFloat()-0.5f) * 30;
                    player.itemTime = 1;
                    //SoundEngine.PlaySound(SoundID.Dig, position);
                    for (int i = 0; i < 2; i++)
                    {
                        Dust wood = Dust.NewDustDirect(position, 0, 0, DustID.WoodFurniture, 0, 0, 0);
                        wood.scale = 1f;
                        wood.velocity *= 1f;
                        wood.velocity += velocity * 0.5f;
                    }
                    break;

                case ItemID.FlamingArrow: type = ProjectileID.FireArrow; break;

                case ItemID.UnholyArrow: type = ProjectileID.UnholyArrow; break;

                case ItemID.JestersArrow:
                    damage = (int)(damage * 2.5f);
                    knockback *= 5;
                    velocity *= 0.5f;
                    type = ModContent.ProjectileType<GenesisJesterArrowProjectile>();
                    //SoundEngine.PlaySound(SoundID.Item9, position); //moved to projectile
                    for (int i = 0; i < 10; i++)
                    {
                        int dustType = Main.rand.Next(3);
                        Dust starDust = Dust.NewDustDirect(position, 0, 0, dustType switch
                        {
                            0 => 15,
                            1 => 57,
                            _ => 58,
                        });
                        starDust.scale += Main.rand.NextFloat();
                        starDust.velocity += velocity * 0.6f;

                        Dust traildust = Dust.NewDustDirect(position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                        traildust.scale += Main.rand.NextFloat();
                        traildust.velocity *= 2f;
                        traildust.velocity += velocity * 1f;

                        Dust burstdust = Dust.NewDustDirect(position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(220, 220, 220));
                        burstdust.scale += Main.rand.NextFloat();
                        burstdust.velocity *= 2f;
                        burstdust.velocity += velocity * 1f;
                        burstdust.noGravity = true;

                    }
                    break;

                case ItemID.HellfireArrow:
                    type = ModContent.ProjectileType<GenesisHellfireArrowProjectile>();

                    break;

                case ItemID.HolyArrow: type = ProjectileID.HolyArrow; break;

                case ItemID.CursedArrow:
                    type = ModContent.ProjectileType<GenesisCursedArrowProjectile>();
                    //SoundEngine.PlaySound(SoundID.Item67, position); //moved to projectile
                    for (int i = 0; i < 30; i++)
                    {
                        Dust light = Dust.NewDustDirect(position, 0, 0, DustID.CursedTorch);
                        light.scale = 1.5f;
                        light.velocity = velocity.RotatedByRandom(Math.PI / 3);
                        light.velocity *= Main.rand.NextFloat();
                        light.velocity *= 0.5f;
                    }
                    break;

                case ItemID.FrostburnArrow: type = ProjectileID.FrostburnArrow; break;

                case ItemID.ChlorophyteArrow: type = ProjectileID.ChlorophyteArrow; break;

                case ItemID.IchorArrow: type = ProjectileID.IchorArrow; break;

                case ItemID.VenomArrow: type = ProjectileID.VenomArrow;
                    type = ModContent.ProjectileType<GenesisVenomArrowProjectile>();
                    for(int i=0;i<10;i++)
                    {
                        Dust dust13 = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<TintableOpaqueDustLighted>(), 0, 0, 0, Color.Lerp(new Color(64, 0, 192), new Color(0, 192, 0), Main.rand.NextFloat()), 1.3f + Main.rand.NextFloat() * 0.5f);
                        dust13.noGravity = true;
                        dust13.velocity *= 0.3f;
                        dust13.velocity += velocity * i * 0.05f;
                        //dust13.fadeIn = dust13.scale + 0.05f;
                        Dust dust14 = Dust.CloneDust(dust13);
                        //dust14.type = DustID.TintableDust;
                        dust14.color = Color.Lerp(dust14.color, Color.Black, 0.7f);
                        dust14.scale -= 0.3f;
                    }
                    break;

                case ItemID.BoneArrow: type = ProjectileID.BoneArrowFromMerchant;
                    type = ModContent.ProjectileType<GenesisBoneArrowProjectile>();
                    //position.X += 8;
                    //SoundEngine.PlaySound(SoundID.Dig, position);
                    for (int i = 0; i < 20; i++)
                    {
                        Dust bone = Dust.NewDustDirect(position, 0, 0, DustID.Bone, 0, 0, 0);
                        bone.scale = 1f;
                        bone.velocity *= 2f;
                        bone.velocity += velocity * 0.3f;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Dust dust13 = Dust.NewDustDirect(position, 0, 0, DustID.BoneTorch);
                        dust13.velocity *= 2f;
                        dust13.velocity += velocity * 0.3f;
                        dust13.noGravity = true;
                        dust13.scale = 1.5f;
                    }
                    break;

                case ItemID.MoonlordArrow: type = ProjectileID.MoonlordArrow; break;

                case ItemID.ShimmerArrow:
                    type = ModContent.ProjectileType<GenesisShimmerArrowProjectile>();
                    //SoundEngine.PlaySound(SoundID.Item92, position);
                    //Vector2 altVelocity = velocity.RotatedByRandom(Math.PI / 3) * 0.7f;
                    velocity = velocity.RotatedByRandom(Math.PI/3);
                    velocity *= 0.7f;
                    /*for (int i = 0; i < 10; i++)
                    {
                        Dust light = Dust.NewDustDirect(position, 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                        light.scale = 1f;
                        light.velocity *= 1f;
                        light.velocity += velocity * 0.5f;
                    }*/
                    //Projectile.NewProjectileDirect(source, position, altVelocity, type, damage, knockback, player.whoAmI);
                    break;

                default:
                    if (((EntitySource_ItemUse_WithAmmo)source).AmmoItemIdUsed == ModContent.ItemType<Items.Ammo.SpectreArrow>()) //check for spectre arrow, since switch statements hate ModContent
                    {
                        type = ModContent.ProjectileType<SpectreArrowProjectile>();
                    }
                    else
                    {
                        //type = 0; //modded ammo (or a bug) detected!
                    }
                    break;

            }

            Projectile proj;
            
            proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            
            //((UnifiedArrowProjectile)Main.projectile[index])
            return false;
        }
        /*public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
            Vector2 origin = itemFrame.Size() / 2f;
            Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
            //Texture2D texture = TextureAssets.Projectile[Type].Value;
            spriteBatch.Draw(itemTexture, drawPosition, itemFrame, Color.White, rotation, origin, scale, SpriteEffects.None, 0);

            return false;
        }*/

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        /*public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            
        }*/


        /*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            
            Vector2 position = Item.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture,position,sourceRectangle,alphaColor,rotation,origin,scale,SpriteEffects.None);
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }*/
        /* public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
         {
             base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
         }*/


    }
}
