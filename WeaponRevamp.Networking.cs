using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WeaponRevamp.Buffs.Swords;
using Microsoft.Xna.Framework;

namespace WeaponRevamp
{
    // This is a partial class, meaning some of its parts were split into other files. See ExampleMod.*.cs for other portions.
    // The class is partial to organize similar code together to clarify what is related.
    // This class extends from the Mod class as seen in ExampleMod.cs. Make sure to extend from the mod class, ": Mod", in your own code if using this file as a template for you mods Mod class.
    partial class WeaponRevamp
    {
        internal enum MessageType : byte
        {
            PalladiumBleed
        }

        // Override this method to handle network packets sent for this mod.
        //TODO: Introduce OOP packets into tML, to avoid this god-class level hardcode.
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Packet Recieved"), Color.White);
            switch (msgType)
            {
                
                case MessageType.PalladiumBleed:
                    //send the message type, then the npc id, then the player id, in that order.
                    NPC bleedNPC = Main.npc[reader.ReadByte()];
                    int player = reader.ReadByte();
                    if ( bleedNPC.active )
                    {
                        bleedNPC.GetGlobalNPC<PalladiumBleedNPC>().palladiumBleedOwner = player;
                        bleedNPC.netUpdate = true;
                    }
                    //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("NPC: " + bleedNPC.whoAmI + "Player: " + player), Color.White);

                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}