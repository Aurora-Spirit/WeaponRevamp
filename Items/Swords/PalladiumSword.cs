using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Buffs.Swords;

namespace WeaponRevamp.Items.Swords
{
    public class PalladiumSword : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.PalladiumSword;
        }



        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(item, player, target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<PalladiumBleed>(), 150);
            target.GetGlobalNPC<PalladiumBleedNPC>().palladiumBleedOwner = player.whoAmI;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Send the ModPacket if used by a player in multiplayer so that other players can receive the change, too.
                // The ModPacket is handled in ExampleMod.Networking.cs
                ModPacket packet = ModContent.GetInstance<WeaponRevamp>().GetPacket();
                packet.Write((byte)WeaponRevamp.MessageType.PalladiumBleed);
                packet.Write((byte)target.whoAmI);
                packet.Write((byte)player.whoAmI);
                packet.Send();
                //Main.NewText("Packet Sent");

            }
            Vector2 slashAngle = new Vector2(1,2 * Math.Sign(target.position.X - player.position.X)).RotatedByRandom(0.2) * hit.HitDirection;
            slashAngle *= 0.05f;
            for(int i=0;i < 10; i++)
            {
                Dust slash = Dust.NewDustDirect(new Vector2(target.Center.X - (target.width*hit.HitDirection), player.position.Y - 28 ), 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(230, 140, 40));
                slash.velocity = slashAngle;
                slash.velocity *= i - 2;
                slash.scale += (i / 10);
                slash.scale *= 0.3f;
            }
        }

        // BLOCK TO COPY //
        public override void SetStaticDefaults()
        {
            //{this.Name}
            Tooltip = Mod.GetLocalization($"Tooltips."+this.Name);
        }
        public static LocalizedText Tooltip { get; private set; }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.Insert(5, new(Mod, "Tooltip", Tooltip.Value));
        }
        // END BLOCK //


    }
}