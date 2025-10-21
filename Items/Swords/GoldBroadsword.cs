using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Swords
{
    public class GoldBroadsword : GlobalItem
    {

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.GoldBroadsword;
        }

        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 25;
            entity.useTime = 32;
            entity.useAnimation = 32;
            entity.scale = 1.2f;
            entity.knockBack = 11.5f;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Gold);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += new Vector2(hit.HitDirection,0) * 1f;
                dust.noGravity = true;
            }
            Dust light = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
            light.scale = 0.5f;
            light.velocity *= 0.3f;
            light.velocity += new Vector2(hit.HitDirection, 0) * 0.5f;
            base.OnHitNPC(item, player, target, hit, damageDone);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Gold);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += new Vector2(hurtInfo.HitDirection, 0) * 1f;
                dust.noGravity = true;
            }
            Dust light = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
            light.scale = 0.5f;
            light.velocity *= 0.3f;
            light.velocity += new Vector2(hurtInfo.HitDirection, 0) * 0.5f;
            base.OnHitPvp(item, player, target, hurtInfo);
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 1.6f;
            
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 0.56f;
            

        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            scale = 1.2f;
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            knockback *= 1.78f;
        }*/


    }
}