using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherRanged
{
    internal class StarCannonProjectile: GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.StarCannonStar;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            projectile.position += projectile.velocity;
            Color newColor7 = Color.CornflowerBlue;
            if (Main.tenthAnniversaryWorld)
            {
                newColor7 = Color.HotPink;
                newColor7.A /= 2;
            }
            for (int num635 = 0; num635 < 7; num635++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num636 = 0f; num636 < 1f; num636 += 0.125f)
            {
                Dust.NewDustPerfect(projectile.Center, 278, Vector2.UnitY.RotatedBy(num636 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num637 = 0f; num637 < 1f; num637 += 0.25f)
            {
                Dust.NewDustPerfect(projectile.Center, 278, Vector2.UnitY.RotatedBy(num637 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector54 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector54 / 2f, vector54 + new Vector2(400f))))
            {
                for (int num638 = 0; num638 < 7; num638++)
                {
                    Gore.NewGore(projectile.GetSource_OnHit(target), projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * projectile.velocity.Length(), Utils.SelectRandom<int>(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
            projectile.position -= projectile.velocity;

            base.OnHitNPC(projectile, target, hit, damageDone);
        }

    }
}
