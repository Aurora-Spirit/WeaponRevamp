using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace WeaponRevamp.Items.Swords
{
	public class ScreensaverTest : GlobalItem
	{

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.DirtBlock;

        }
        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            //the code for Main.DrawWallOfStars, modified to function in-game while a dirt block is dropped on the ground
            /*UnifiedRandom r = new UnifiedRandom(5000);
            Main.instance.LoadProjectile(99);
            Texture2D value = TextureAssets.Projectile[99].Value;
            Vector2 vector = Main.ScreenSize.ToVector2();
            for (int i = 0; i < 20000; i++)
            {
                Vector2 vector2 = r.NextVector2Square(-0.1f, 1.1f);
                vector2.X *= 0.1f;
                vector2.X -= 0.1f;
                vector2.X += Main.GlobalTimeWrappedHourly % 10f / 10f * 1.2f;
                vector2.Y -= Main.GlobalTimeWrappedHourly % 10f / 10f;
                if (vector2.Y < -0.2f)
                {
                    vector2.Y += 1.4f;
                }
                if (vector2.X > 1.1f)
                {
                    vector2.X -= 1.2f;
                }
                Vector2 position = vector2 * vector;
                spriteBatch.Draw(value, position, Color.White);
            }*/
        }

    }
}