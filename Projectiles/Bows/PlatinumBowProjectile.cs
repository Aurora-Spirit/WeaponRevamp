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
    public class PlatinumBowProjectile:UnifiedArrowProjectile
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
        /*public override void Load() //this method AND the following one together allow this projectile to be reflected by biome mimics and the like.
        {
            On_Projectile.CanBeReflected += CanBeReflected2;
        }
        private bool CanBeReflected2(On_Projectile.orig_CanBeReflected orig, Projectile self)
        {
            bool validReflectable = Projectile.active && Projectile.friendly && !Projectile.hostile && Projectile.damage > 0;
            return orig(self) || (self.type == Projectile.type && validReflectable);
        }*/

        //Projectile.ai[0] refers to the ammo's projectile id number.
        public override void AI()
        {
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

    }
}