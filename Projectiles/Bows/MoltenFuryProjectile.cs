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
    public class MoltenFuryProjectile:UnifiedArrowProjectile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            //Main.NewText(Projectile.ai[0]);


            PostSpawn(source);
        }
        
        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.75f, 0.55f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100);
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3)) 
            { 
                target.AddBuff(BuffID.OnFire, 180); 
            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnKill(int timeLeft)
        {
            for (int num418 = 0; num418 < 10; num418++) //death dusts
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100);
            }
            base.OnKill(timeLeft);

        }
            /*public override bool PreDraw(ref Color lightColor)
            {
                return base.PreDraw(ref lightColor);
            }*/

    }
}