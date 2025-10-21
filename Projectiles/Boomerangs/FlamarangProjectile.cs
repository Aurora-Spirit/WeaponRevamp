using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using Microsoft.Xna.Framework;

namespace WeaponRevamp.Projectiles.Boomerangs
{
    public class FlamarangProjectile : BoomerangBaseAI
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.Flamarang;
        }

        public override bool PreAI(Projectile projectile)
        {
            for (int num300 = 0; num300 < 2; num300++)
            {
                int num311 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                Main.dust[num311].noGravity = true;
                Main.dust[num311].velocity.X *= 0.3f;
                Main.dust[num311].velocity.Y *= 0.3f;
            }
            return base.PreAI(projectile);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(projectile.GetSource_FromThis(), target.Center, new Vector2(0,-1), ProjectileID.Volcano, projectile.damage/2, projectile.knockBack*0.5f);
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
    }
}