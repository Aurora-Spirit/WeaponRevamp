using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using System.CodeDom;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Projectiles.Guns
{
    public class ClockworkCogProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 7;
            Projectile.width = 10;
            Projectile.height = Projectile.width;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }
        /*public override void OnSpawn(IEntitySource source)
        {
            Main.NewText(Projectile.velocity.X + ", " + Projectile.velocity.Y);
            base.OnSpawn(source);
        }*/
        public override void AI()
        {
            //Main.NewText("X: "+Projectile.position.X+" Y: "+Projectile.position.Y);
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.position += Projectile.velocity * 32f;
            Projectile.velocity = Projectile.velocity.RotatedBy(0.1f);
            Projectile.timeLeft += 1;
            Projectile.rotation -= 0.15f;
            if (Main.player[Projectile.owner].dead)
            {
                Projectile.Kill();
            }
        }

        public void Shoot(EntitySource_ItemUse_WithAmmo source)
        {
            Vector2 vel = Vector2.Normalize(Main.MouseWorld - Projectile.position)*11;
            Projectile bullet = UnifiedBulletProjectile.NewUnifiedBullet(source, Projectile.position, vel, ModContent.ProjectileType<UnifiedBulletProjectile>(), Projectile.damage, Projectile.knockBack, (int)Projectile.ai[0], Projectile.owner);
            Projectile.Kill();
        }
        
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }

    }
}
