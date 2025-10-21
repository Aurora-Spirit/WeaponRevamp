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
    public class HelFireProjectile : GlobalProjectile
    {


        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(projectile.type == ProjectileID.HelFire)
            {
                Projectile.NewProjectile(null, target.Center, new Vector2(0, -1), ProjectileID.Volcano, projectile.damage /2, projectile.knockBack);
                
            }
           
        }
    }
}