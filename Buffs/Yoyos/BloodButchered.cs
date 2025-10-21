using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Buffs.Yoyos
{
    public class BloodButchered : GlobalBuff
    {
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            if (type == BuffID.BloodButcherer) {
                int num = npc.lifeRegenExpectedLossPerSecond;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                int num11 = 0;
                int num12 = 1;
                for (int k = 0; k < 1000; k++)
                {
                    if (Main.projectile[k].active && ( ( Main.projectile[k].type == 975 && Main.projectile[k].ai[0] == 1f && Main.projectile[k].ai[1] == (float)npc.whoAmI ) || ( Main.projectile[k].type == ProjectileID.CrimsonYoyo ) ) )
                    {
                        num11++;
                    }
                }
                npc.lifeRegen -= num11 * 2 * 4;
                if (num < num11 * 4 / num12)
                {
                    num = num11 * 4 / num12;
                }
            }
        }
        
    }
}
