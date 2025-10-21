using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class TungstenBroadsword : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.TungstenBroadsword;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.scale = 1.1f;
            entity.ArmorPenetration = 12;
            entity.damage = 12;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Tungsten);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += new Vector2(hit.HitDirection, 0) * 1f;
                dust.noGravity = true;
            }
            Dust light = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TintableDustLighted, 0, 0, 0, new Color(0.4f, 0.9f, 0.5f, 0.5f));
            light.scale = 0.5f;
            light.velocity *= 0.3f;
            light.velocity += new Vector2(hit.HitDirection, 0) * 0.5f;
            base.OnHitNPC(item, player, target, hit, damageDone);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Platinum);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += new Vector2(hurtInfo.HitDirection, 0) * 1f;
                dust.noGravity = true;
            }
            Dust light = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TintableDustLighted, 0, 0, 0, new Color(0.4f, 0.9f, 0.5f, 0.5f));
            light.scale = 0.5f;
            light.velocity *= 0.3f;
            light.velocity += new Vector2(hurtInfo.HitDirection, 0) * 0.5f;
            base.OnHitPvp(item, player, target, hurtInfo);
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 0.92f;
            
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            scale = 1.2f;
        }
        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            
            crit += 16f;

        }*/
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