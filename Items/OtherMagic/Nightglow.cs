using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.OtherMagic;

public class Nightglow : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstatiation)
    {
        return item.type == ItemID.FairyQueenMagicItem;
    }

    public override void SetDefaults(Item item)
    {
        item.scale = 0.9f;
        item.damage = 90;
    }
    
}

public class NightglowHoldingPlayer : ModPlayer
{
    
    private static Asset<Texture2D> glowTexture;
    public override void Load() {
        glowTexture = ModContent.Request<Texture2D>("WeaponRevamp/Items/OtherMagic/Nightglow_glow");
    }
    
    
    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        
        if (drawInfo.shadow == 0f && drawInfo.heldItem.type == ItemID.FairyQueenMagicItem)
        {
            Texture2D texture = glowTexture.Value;
            Rectangle frame = texture.Frame();
            Vector2 pivot = frame.Center();
            Vector2 itemOffset = new Vector2(drawInfo.heldItem.width/3f*drawInfo.drawPlayer.direction, -drawInfo.heldItem.height/2f)*drawInfo.heldItem.scale;
            Vector2 drawPosition = drawInfo.ItemLocation - Main.screenPosition + itemOffset;
            Projectile temporaryPoS = new Projectile();
            temporaryPoS.ai[1] = drawInfo.drawPlayer.miscCounterNormalized + 0.66f % 1f;
            Main.EntitySpriteDraw(texture, drawPosition, frame, temporaryPoS.GetFairyQueenWeaponsColor(0, 0.2f) * 0.2f, 0, pivot, 2, SpriteEffects.None);
            
        }
    }
}