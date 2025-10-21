using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;

namespace WeaponRevamp.Items.Ammo
{
    public class RemoveUnusedAmmoFromShop:GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if(shop.NpcType == NPCID.ArmsDealer) //candy corn was removed
            {
                if (shop.TryGetEntry(ItemID.CandyCorn, out NPCShop.Entry entry1))
                {
                    entry1.Disable();
                }
            }
            
            if(shop.NpcType == NPCID.SkeletonMerchant) //skeleton merchant always sells bone arrow and never sells wooden arrow
            {
                if (shop.TryGetEntry(ItemID.BoneArrow, out NPCShop.Entry entry1))
                {
                    entry1.Disable();
                }
                if (shop.TryGetEntry(ItemID.WoodenArrow, out NPCShop.Entry entry2))
                {
                    entry2.Disable();
                }
                shop.Add(ItemID.BoneArrow);
            }
            
        }
    }
}
