using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using WeaponRevamp.Buffs.Yoyos;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using WeaponRevamp.Projectiles.Yoyos;
using Microsoft.Xna.Framework;

namespace WeaponRevamp.Projectiles.Yoyos
{
    public class KrakenProjectile : GlobalProjectile
    {

        int counter = 0;
        public override void PostAI(Projectile projectile)
        {
            if(projectile.type == ProjectileID.Kraken)
            {
                counter += 1;
                if (counter>=5)
                {
                    Vector2 realVelocity = Vector2.Subtract(projectile.position, projectile.oldPosition);
                    Projectile.NewProjectile(null, projectile.Center, realVelocity, ModContent.ProjectileType<KrakenWaterStream>(), projectile.damage / 5, projectile.knockBack, projectile.owner);
                    counter = 0;
                }
            }
           
        }

        public override bool InstancePerEntity => true;
    }
}