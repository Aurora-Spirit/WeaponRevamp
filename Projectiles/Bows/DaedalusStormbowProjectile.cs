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
    public class DaedalusStormbowProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            //Main.NewText(Projectile.ai[0]);
            useAmmoPhysics = false;

            PostSpawn(source);
        }

        

        //Projectile.ai[0] refers to the ammo's projectile id number.
        //Projectile.ai[1] refers to the X position of the mouse firing point
        //Projectile.ai[2] refers to the acceleration of the projectile
        public override void AI()
        {
            if(!useAmmoPhysics)
            {
                Projectile.velocity.Y += Projectile.ai[2];
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.X += Math.Sign(Projectile.position.X - Projectile.ai[1]) * -0.22f;
            }
            Math.Clamp(Projectile.velocity.X, -16f, 16f);
            Math.Clamp(Projectile.velocity.Y, -16f, 16f);
            if (Projectile.ai[0] != spectre)
            {
                Projectile.tileCollide = maxTimeLeft - Projectile.timeLeft > 10;
            }
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)((double)Projectile.damage * 0.5);
            if (Projectile.ai[0] == spectre)
            {
                useAmmoPhysics = true;
                Projectile.netUpdate = true;
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            useAmmoPhysics = true;
            Projectile.netUpdate = true;
            return base.OnTileCollide(oldVelocity);
        }

        /*public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }*/

    }
}