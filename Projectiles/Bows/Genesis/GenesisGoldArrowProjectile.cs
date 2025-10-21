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

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisGoldArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        // Custom AI
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.position, new Vector3(1f, 1f, 0.2f));
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 15)
            {
                Projectile.velocity.Y += 0.07f;
            }
            if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;
            NPC target = Projectile.FindTargetWithinRange(240);
            if(target != null) Projectile.velocity += Vector2.Normalize(target.Center - Projectile.Center) * 0.3f;
            if (Main.rand.NextBool(12))
            {
                Dust light = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                light.scale = 1f;
                light.velocity *= 0.3f;
                light.velocity += Projectile.velocity * 0.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i=0;i<10;i++)
            {
                Dust light = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                light.scale = 1f;
                light.velocity *= 1f;
                light.velocity += Projectile.velocity * 0.5f;
            }
            base.OnKill(timeLeft);
        }
        

    }
}
