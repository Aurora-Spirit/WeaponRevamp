using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Items.Shortswords
{
    public class PlatinumShortswordProjectile : GlobalProjectile
    {

        public override bool AppliesToEntity(Projectile proj, bool lateInstatiation)
        {
            return proj.type == ProjectileID.PlatinumShortswordStab;
        }

        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position + projectile.velocity, 0, 0, DustID.Platinum);
                dust.scale = 1.2f;
                dust.velocity *= 1f;
                dust.velocity += projectile.velocity * 1f;
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(4))
            {
                Dust light = Dust.NewDustDirect(projectile.position + projectile.velocity, 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 1f, 0.5f));
                light.scale = 0.5f;
                light.velocity *= 0.3f;
                light.velocity += projectile.velocity * 0.5f;
            }
            base.OnHitNPC(projectile, target, hit, damageDone);
        }

        /*public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            
            damage *= 2.25f;
            
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 0.67f;

        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            knockback *= 1.5f;
        }*/



    }
}