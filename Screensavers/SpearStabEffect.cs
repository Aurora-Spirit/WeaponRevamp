using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static Humanizer.In;
using ReLogic.Content;
using Terraria.Enums;
using Terraria.GameContent.Achievements;

namespace WeaponRevamp.Screensavers
{
    public class SpearStabEffect : GlobalProjectile
    {
        
       /* public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.aiStyle == 19;
        }

       

        
        public override void PostDraw(Projectile proj, Color lightColor)
        {
            
            Color projectileColor = Lighting.GetColor((int)((double)proj.position.X + (double)proj.width * 0.5) / 16, (int)(((double)proj.position.Y + (double)proj.height * 0.5) / 16.0));
            SpriteEffects dir = SpriteEffects.None;
            float num = (float)Math.Atan2(proj.velocity.Y, proj.velocity.X) + 2.355f;
            Asset<Texture2D> asset = TextureAssets.Projectile[proj.type];
            Player player = Main.player[proj.owner];
            Rectangle value = asset.Frame();
            Rectangle rect = proj.getRect();
            Vector2 vector = Vector2.Zero;
            if (player.direction > 0)
            {
                dir = SpriteEffects.FlipHorizontally;
                vector.X = asset.Width();
                num -= (float)Math.PI / 2f;
            }
            if (player.gravDir == -1f)
            {
                if (proj.direction == 1)
                {
                    dir = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    vector = new Vector2(asset.Width(), asset.Height());
                    num -= (float)Math.PI / 2f;
                }
                else if (proj.direction == -1)
                {
                    dir = SpriteEffects.FlipVertically;
                    vector = new Vector2(0f, asset.Height());
                    num += (float)Math.PI / 2f;
                }
            }
            Vector2.Lerp(vector, value.Center.ToVector2(), 0.25f);
            float num2 = 0f;

            AI_019_Spears_GetExtensionHitbox(out Rectangle extensionBox, proj);
            Vector2 vector2 = proj.Center + new Vector2(0f, proj.gfxOffY);
            //Player player = Main.player[proj.owner];
            Vector2 value2 = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            float num3 = extensionBox.Size().Length() / proj.Hitbox.Size().Length();
            _ = new Color(255, 255, 255, 0) * 1f;
            float num4 = Utils.Remap(player.itemAnimation, player.itemAnimationMax, (float)player.itemAnimationMax / 3f, 0f, 1f);
            float num5 = Utils.Remap(num4, 0f, 0.3f, 0f, 1f) * Utils.Remap(num4, 0.3f, 1f, 1f, 0f);
            num5 = 1f - (1f - num5) * (1f - num5);
            Vector2 vector3 = extensionBox.Center.ToVector2() + new Vector2(0f, proj.gfxOffY);
            Vector2.Lerp(value2, vector3, 1.1f);
            Texture2D value3 = TextureAssets.Extra[98].Value;
            Vector2 origin = value3.Size() / 2f;
            Color color = new Color(255, 255, 255, 0) * 0.5f;

            switch (proj.type)
            {
                case 105:
                    color = new Color(255, 220, 80, 0);
                    break;
                case 46:
                    color = new Color(180, 80, 255, 0);
                    break;
                case 342:
                    color = new Color(80, 140, 255, 0);
                    break;
                case 153:
                    color = new Color(255, 50, 30, 15);
                    break;

            }

            float num6 = num - (float)Math.PI / 4f * (float)proj.spriteDirection;
            if (player.gravDir < 0f)
            {
                num6 -= (float)Math.PI / 2f * (float)proj.spriteDirection;
            }
            Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 0.5f) - Main.screenPosition, null, color * num5, num6, origin, new Vector2(num5 * num3, num3) * proj.scale * num3, dir);
            Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 1f) - Main.screenPosition, null, color * num5, num6, origin, new Vector2(num5 * num3, num3 * 1.5f) * proj.scale * num3, dir);
            Main.EntitySpriteDraw(value3, Vector2.Lerp(value2, vector2, num4 * 1.5f - 0.5f) - Main.screenPosition + new Vector2(0f, 2f), null, color * num5, num6, origin, new Vector2(num5 * num3 * 1f * num5, num3 * 2f * num5) * proj.scale * num3, dir);

            *//*Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 0.5f) - Main.screenPosition, null, color1 * num5, num6, origin, new Vector2(num5 * num3, num3) * proj.scale * num3, dir);
            Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 1f) - Main.screenPosition, null, color1 * num5, num6, origin, new Vector2(num5 * num3, num3 * 1.5f) * proj.scale * num3, dir);
            Main.EntitySpriteDraw(value3, Vector2.Lerp(value2, vector2, num4 * 1.5f - 0.5f) - Main.screenPosition + new Vector2(0f, 2f), null, color1 * num5, num6, origin, new Vector2(num5 * num3 * 1f * num5, num3 * 2f * num5) * proj.scale * num3, dir);*//*

            for (float num7 = 0.4f; num7 <= 1f; num7 += 0.1f)
            {
                Vector2 vector4 = Vector2.Lerp(value2, vector3, num7 + 0.2f);
                Main.EntitySpriteDraw(value3, vector4 - Main.screenPosition + new Vector2(0f, 2f), null, color * num5 * 0.75f * num7, num6, origin, new Vector2(num5 * num3 * 1f * num5, num3 * 2f * num5) * proj.scale * num3, dir);
            }
            extensionBox.Offset((int)(0f - Main.screenPosition.X), (int)(0f - Main.screenPosition.Y));

            Main.EntitySpriteDraw(asset.Value, vector2 - Main.screenPosition, value, proj.GetAlpha(projectileColor), num, vector, proj.scale, dir);
            rect.Offset((int)(0f - Main.screenPosition.X), (int)(0f - Main.screenPosition.Y));
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.White * num2);
            
        }

        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 center = projectile.Center;
            AI_019_Spears_GetExtensionHitbox(out var extensionBox, projectile);
            Vector2 vector2 = extensionBox.Center.ToVector2();
            float num9 = Vector2.Distance(vector2, center);
            Vector2 size = extensionBox.Size();
            float num10 = MathHelper.Max(extensionBox.Width, extensionBox.Height);
            if (num10 < 12f)
            {
                num10 = 12f;
            }
            //this section is super broken and I don't know why but it doesn't seem to be necessary so I have removed it
            //Main.NewText("SpearRect:" + extensionBox.TopLeft().X + " " + extensionBox.TopLeft().Y + ", " + extensionBox.BottomRight().X + " " + extensionBox.BottomRight().Y + "\nTargetRect:" + targetHitbox.TopLeft().X + " " + targetHitbox.TopLeft().Y + ", " + targetHitbox.BottomRight().X + " " + targetHitbox.BottomRight().Y);
            *//*for (float num11 = num10; num11 < num9; num11 += num10)
            {
                Vector2 funnyVector = Vector2.Lerp(center, vector2, num11 / num9);
                for (int i = 0; i < 6; i++) Dust.NewDust(new Vector2(funnyVector.X - (size.X / 2), funnyVector.Y - (size.Y / 2)), (int)size.X, (int)size.Y, DustID.TintableDustLighted, 0, 0, 0, new Color(255-num11*3, 255, num11*3), 0.8f);
                if (Utils.CenteredRectangle(Vector2.Lerp(center, vector2, num11 / num9), size).Intersects(targetHitbox))
                {
                    Main.NewText("Funny coords: " + Vector2.Lerp(center, vector2, num11 / num9).X + " " + Vector2.Lerp(center, vector2, num11 / num9).Y);
                    if (size.X < 0 || size.Y<0) Main.NewText("broken size: " + size.X + " " + size.Y);
                    Main.NewText("hit with weird thing");
                    return true;
                }
            }*//*
            if (extensionBox.Intersects(targetHitbox))
            {
                //Main.NewText("hit with spear extension");
                return true;
            }

            return base.Colliding(projectile, projHitbox, targetHitbox);
        }

        public bool AI_019_Spears_GetExtensionHitbox(out Rectangle extensionBox, Projectile proj)
        {
            extensionBox = default(Rectangle);
            Player player = Main.player[proj.owner];
            if (player.itemAnimation < player.itemAnimationMax / 3)
            {
                return false;
            }
            int itemAnimationMax = player.itemAnimationMax;
            int itemAnimation = player.itemAnimation;
            int num = player.itemAnimationMax / 3;
            float num2 = Utils.Remap(itemAnimation, itemAnimationMax, num, 0f, 1f);
            float num3 = 10f;
            float num4 = 30f;
            float num5 = 10f;
            float num6 = 10f;
            switch (proj.type)
            {
                case 105:
                    num4 = 50f;
                    num6 = 20f;
                    break;
                case 46:
                    num4 = 50f;
                    num6 = 15f;
                    break;
                case 153:
                    num4 = 40f;
                    num6 = 10f;
                    break;
            }
            num4 *= player.GetTotalAttackSpeed(DamageClass.Melee); ;
            float num7 = num3 + num4 * num2;
            float num8 = num5 + num6 * num2;
            float f = proj.velocity.ToRotation();
            Vector2 center = proj.Center + f.ToRotationVector2() * num7;
            extensionBox = Utils.CenteredRectangle(center, new Vector2(num8, num8));
            return true;
        }
        public override void CutTiles(Projectile projectile)
        {
            base.CutTiles(projectile);
            AI_019_Spears_GetExtensionHitbox(out Rectangle extensionBox, projectile);
            CutTilesAt(extensionBox.TopLeft(), extensionBox.Width, extensionBox.Height, projectile);
            AchievementsHelper.CurrentlyMining = false;
        }

        private void CutTilesAt(Vector2 boxPosition, int boxWidth, int boxHeight, Projectile proj)
        {
            //Main.NewText("Method called");
            int num = (int)(boxPosition.X / 16f);
            int num2 = (int)((boxPosition.X + (float)boxWidth) / 16f) + 1;
            int num3 = (int)(boxPosition.Y / 16f);
            int num4 = (int)((boxPosition.Y + (float)boxHeight) / 16f) + 1;
            if (num < 0)
            {
                num = 0;
            }
            if (num2 > Main.maxTilesX)
            {
                num2 = Main.maxTilesX;
            }
            if (num3 < 0)
            {
                num3 = 0;
            }
            if (num4 > Main.maxTilesY)
            {
                num4 = Main.maxTilesY;
            }
            bool[] tileCutIgnorance = Main.player[proj.owner].GetTileCutIgnorance(allowRegrowth: false, proj.trap);
            //Main.NewText("Got tile cut ignorance");
            for (int i = num; i < num2; i++)
            {
                for (int j = num3; j < num4; j++)
                {
                    if (Main.tile[i, j] != null && Main.tileCut[Main.tile[i, j].TileType] && !tileCutIgnorance[Main.tile[i, j].TileType] && WorldGen.CanCutTile(i, j, TileCuttingContext.AttackProjectile))
                    {
                        //Main.NewText("Hit a tile");
                        WorldGen.KillTile(i, j);
                        //Main.NewText("Killed tile");
                        if (Main.netMode != 0)
                        {
                            //Main.NewText("In multiplayer");
                            NetMessage.SendData(17, -1, -1, null, 0, i, j);
                            //Main.NewText("Netmessage sent");
                        }
                    }
                }
            }
        }*/

    }
}
