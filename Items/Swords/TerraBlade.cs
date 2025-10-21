using Microsoft.Xna.Framework;
using System.Security.Principal;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class TerraBlade : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.TerraBlade;
        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.shootSpeed = 8.4f;
            entity.useTime = 20;
            entity.useAnimation = 20;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.TerraBlade, 1);
            recipe.AddIngredient(ItemID.TrueNightsEdge);
            recipe.AddIngredient(ModContent.ItemType<Items.Swords.Tizona>());
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

    }
}