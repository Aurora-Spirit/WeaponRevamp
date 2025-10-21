using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace WeaponRevamp.Projectiles.Yoyos
{
    public class KrakenWaterStream : ModProjectile
    {


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 8; // The width of projectile hitbox
            Projectile.height = 8; // The height of projectile hitbox
            Projectile.aiStyle = 1; // The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.penetrate = 1; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?

            Projectile.DamageType = DamageClass.Melee;

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.Kill();

            return false;

        }
        public override void PostAI()
        {
            int dustIndex = Dust.NewDust(Projectile.Center, 1, 1, DustID.DungeonWater, 0f, 0f);
            Main.dust[dustIndex].velocity *= 0.15f;
            Main.dust[dustIndex].velocity += Projectile.velocity * 0.5f;

            base.PostAI();
        }

    }
}
