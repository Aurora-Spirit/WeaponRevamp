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

namespace WeaponRevamp.Projectiles.Bows

{
    public class MarrowProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.extraUpdates *= 3;
            Projectile.extraUpdates += 2;


            PostSpawn(source);
        }
        
        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            /*if (Projectile.ai[0]!=ProjectileID.JestersArrow)
            {
                if (Projectile.ai[0] == ProjectileID.ShimmerArrow)
                {
                    Projectile.velocity.Y -= 0.04f;
                }
                else
                {
                    Projectile.velocity.Y += 0.04f;
                }
            }*/
            if(Projectile.timeLeft >= maxTimeLeft - 35)
            {
                useAmmoPhysics = false;
                if (Projectile.shimmerWet == true && Projectile.ignoreWater == false)
                {
                    Projectile.velocity.Y -= 0.4f;
                }
            } else
            {
                useAmmoPhysics = true;

            }
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }*/
        public override void OnKill(int timeLeft)
        {
            for (int num418 = 0; num418 < 10; num418++) //death dusts
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 26);
            }
            base.OnKill(timeLeft);

        }

    }
}