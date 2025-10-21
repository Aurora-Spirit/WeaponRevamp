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
    public class CobaltRepeaterSouthProjectile:UnifiedArrowProjectile
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
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].position != Projectile.position)
                {
                    Projectile.velocity += (Projectile.position - Main.projectile[i].position) / (float)Math.Pow(Vector2.Distance(Projectile.position, Main.projectile[i].position) + 1, 1.5) * 5;
                    if (Main.rand.NextBool(50, (int)Vector2.Distance(Projectile.position, Main.projectile[i].position) + 100))
                    {
                        Dust pushDust = Dust.NewDustDirect(Vector2.Lerp(Projectile.Center, Main.projectile[i].Center, Main.rand.NextFloat()), 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
                        pushDust.velocity *= 0.1f;
                    }
                }
                else if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.Bows.CobaltRepeaterNorthProjectile>() && Main.projectile[i].position != Projectile.position)
                {
                    Projectile.velocity += (Projectile.position - Main.projectile[i].position) / (float)Math.Pow(Vector2.Distance(Projectile.position, Main.projectile[i].position) + 1, 1.5) * -5;
                    if (Main.rand.NextBool(50, (int)Vector2.Distance(Projectile.position, Main.projectile[i].position) + 100))
                    {
                        Dust pushDust = Dust.NewDustDirect(Vector2.Lerp(Projectile.Center, Main.projectile[i].Center, Main.rand.NextFloat()), 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
                        pushDust.velocity *= 0.1f;
                    }
                }
            }
            if (Main.rand.NextBool(2))
            {
                Dust passiveDust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
                passiveDust.velocity *= 0.1f;
            }

            Lighting.AddLight(Projectile.position, new Vector3(0f, 0f, 1f));
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