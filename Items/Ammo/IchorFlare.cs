using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using WeaponRevamp.Projectiles.OtherRanged.Flares;

namespace WeaponRevamp.Items.Ammo
{
    public class IchorFlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 6f;
            Item.damage = 10;
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.ammo = AmmoID.Flare;
            Item.knockBack = 1.5f;
            Item.value = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<IchorFlareProjectile>();
        }
        
        public override void AddRecipes()
        {
            CreateRecipe(33)
                .AddIngredient(ItemID.Flare, 33)
                .AddIngredient(ItemID.Ichor, 1)
                .Register();
        }
    }
}
