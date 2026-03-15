using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Ammo;

public class MusketBall : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.MusketBall;
    }

    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);
        entity.value = 5;
    }
}

public class MeteorShotRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.MeteorShot) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 250);
                recipe.ReplaceResult(ItemID.MeteorShot, 250);
            }
        }
    }
}

public class SilverBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.SilverBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 100);
                recipe.ReplaceResult(ItemID.SilverBullet, 100);
            }
        }
    }
}

public class CrystalBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.CrystalBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 400);
                recipe.ReplaceResult(ItemID.CrystalBullet, 400);
            }
        }
    }
}

public class CursedBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.CursedBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 200);
                recipe.ReplaceResult(ItemID.CursedBullet, 200);
            }
        }
    }
}

public class ChlorophyteBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.ChlorophyteBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 300);
                recipe.ReplaceResult(ItemID.ChlorophyteBullet, 300);
            }
        }
    }
}

public class HighVelocityBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.HighVelocityBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 100);
                recipe.ReplaceResult(ItemID.HighVelocityBullet, 100);
            }
        }
    }
}

public class IchorBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.IchorBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 200);
                recipe.ReplaceResult(ItemID.IchorBullet, 200);
            }
        }
    }
}

public class VenomBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.VenomBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 150);
                recipe.ReplaceResult(ItemID.VenomBullet, 150);
            }
        }
    }
}

public class PartyBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.PartyBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 50);
                recipe.ReplaceResult(ItemID.PartyBullet, 50);
            }
        }
    }
}

public class NanoBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.NanoBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 150);
                recipe.ReplaceResult(ItemID.NanoBullet, 150);
            }
        }
    }
}

public class ExplodingBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.ExplodingBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 150);
                recipe.ReplaceResult(ItemID.ExplodingBullet, 150);
            }
        }
    }
}

public class GoldenBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.GoldenBullet) && recipe.HasIngredient(ItemID.EmptyBullet))
            {
                recipe.RemoveIngredient(ItemID.EmptyBullet);
                recipe.AddIngredient(ItemID.MusketBall, 200);
                recipe.ReplaceResult(ItemID.GoldenBullet, 200);
            }
        }
    }
}

public class EndlessMusketPouchRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.EndlessMusketPouch) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 1998);
            }
        }
    }
}

public class LuminiteBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.MoonlordBullet))
            {
                recipe.ReplaceResult(ItemID.MoonlordBullet, 500);
            }
        }
    }
}

public class TungstenBulletRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.TungstenBullet) && recipe.HasIngredient(ItemID.MusketBall))
            {
                recipe.RemoveIngredient(ItemID.MusketBall);
                recipe.AddIngredient(ItemID.MusketBall, 100);
                recipe.ReplaceResult(ItemID.TungstenBullet, 100);
            }
        }
    }
}

public class UnholyArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.UnholyArrow) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 30);
                recipe.ReplaceResult(ItemID.UnholyArrow, 30);
            }
        }
    }
}

public class JestersArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.JestersArrow) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 30);
                recipe.ReplaceResult(ItemID.JestersArrow, 30);
            }
        }
    }
}

public class HellfireArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.HellfireArrow) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 150);
                recipe.ReplaceResult(ItemID.HellfireArrow, 150);
            }
        }
    }
}

public class HolyArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.HolyArrow) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 500);
                recipe.ReplaceResult(ItemID.HolyArrow, 500);
            }
        }
    }
}

public class ChlorophyteArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.ChlorophyteArrow))
            {
                recipe.ReplaceResult(ItemID.ChlorophyteArrow, 300);
            }
        }
    }
}

public class VenomArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.VenomArrow) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 100);
                recipe.ReplaceResult(ItemID.VenomArrow, 100);
            }
        }
    }
}

public class EndlessQuiverRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.EndlessQuiver) && recipe.HasIngredient(ItemID.WoodenArrow))
            {
                recipe.RemoveIngredient(ItemID.WoodenArrow);
                recipe.AddIngredient(ItemID.WoodenArrow, 1998);
            }
        }
    }
}

public class LuminiteArrowRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.MoonlordArrow))
            {
                recipe.ReplaceResult(ItemID.MoonlordArrow, 500);
            }
        }
    }
}


public class RainbowFlareRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.HasResult(ItemID.RainbowFlare) && recipe.HasIngredient(ItemID.Flare))
            {
                recipe.RemoveIngredient(ItemID.Flare);
                recipe.AddIngredient(ItemID.Flare, 100);
                recipe.ReplaceResult(ItemID.RainbowFlare, 100);
            }
        }
    }
}





