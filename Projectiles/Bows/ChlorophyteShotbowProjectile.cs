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
    public class ChlorophyteShotbowProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            //Main.NewText(Projectile.ai[0]);
            maxBounces += 1;
            bouncesLeft += 1;


            PostSpawn(source);
        }

        
        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(bouncesLeft == 1)
            {
                Projectile.timeLeft = (int)((double)Projectile.timeLeft * 0.2);
            }
            if (Projectile.ai[0] == hellfire) //prevent hellfire arrows from destroying themselves
            {
                Projectile.penetrate += 1;
            }
            bool val = base.OnTileCollide(oldVelocity);
            if (Projectile.ai[0] == hellfire) //make hellfire pierce normal again
            {
                Projectile.penetrate -= 1;
            }

            return val;
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }*/

    }
}