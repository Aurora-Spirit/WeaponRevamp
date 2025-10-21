using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using WeaponRevamp.Items.Ammo;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Projectiles.Guns
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class ElectricBulletProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 2;
            Projectile.alpha = 255;
            Projectile.scale = 1.3f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }
        // Custom AI
        public override void AI()
        {
            if (Projectile.TryGetOwner(out Player player))
            {
                EntitySource_ItemUse_WithAmmo source =
                    new EntitySource_ItemUse_WithAmmo(player, null, ModContent.ItemType<ElectricBullet>());
                UnifiedBulletProjectile.NewUnifiedBullet(source, Projectile.position,
                    Projectile.velocity, ModContent.ProjectileType<UnifiedBulletProjectile>(), Projectile.damage,
                    Projectile.knockBack, Projectile.type, Projectile.owner);
                Projectile.Kill();
            }
            

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            
            return false;

        }

    }
}
