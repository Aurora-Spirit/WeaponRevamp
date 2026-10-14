using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using System.CodeDom;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Projectiles.Guns
{
    public class PhoenixBlasterProjectile : UnifiedBulletProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void SetAmmoDefaults()
        {
            base.SetAmmoDefaults();
            if (Projectile.penetrate > 0)
            {
                Projectile.maxPenetrate += 1;
                Projectile.penetrate = Projectile.maxPenetrate;
            }
        }
        
        public override void AddBehaviors()
        {
            base.AddBehaviors();
            behaviors.Add(new PhoenixFlames());
        }
    }

    public class PhoenixFlames : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (projectile.ai[1] > 5)
            {
                
                /*if (Main.rand.NextBool((int)projectile.velocity.Length(), 160))
                {
                    Dust fire = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<PhoenixLavaDust>());
                    fire.velocity *= 0.2f;
                    fire.position -= projectile.velocity * 2f;
                    fire.velocity += projectile.velocity * 0.3f;
                    fire.scale = 0.8f;

                }

                if (Main.rand.NextBool((int)projectile.velocity.Length(), 160))
                {
                    Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2, -2), 0, 0, ModContent.DustType<RisingSmokeDust>());
                    smoke.velocity *= 0.5f;
                    smoke.position -= projectile.velocity * 2f;
                    smoke.velocity += projectile.velocity * 0.3f;
                    smoke.scale = 0.1f;
                }*/
                
                float dustCount2 = (float)(projectile.velocity.Length() * 0.2f);
                float i;
                for (i = 0; i < dustCount2-1; i++)
                {
                    Dust fire2 = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, DustID.Torch);
                    fire2.velocity *= 0.4f;
                    fire2.position -= projectile.velocity * 2f;
                    fire2.position += projectile.velocity * i / dustCount2;
                    fire2.velocity += projectile.velocity * 0.3f;
                    fire2.noGravity = true;
                    fire2.scale = 0.8f;
                }
                //Main.NewText(i);
                dustCount2 -= i-2;
                //Main.NewText(dustCount2);
                if (Main.rand.NextFloat() < dustCount2)
                {
                    
                    Dust fire2 = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, DustID.Torch);
                    fire2.velocity *= 0.4f;
                    fire2.position -= projectile.velocity * 2f;
                    fire2.position += projectile.velocity * 1;
                    fire2.velocity += projectile.velocity * 0.3f;
                    fire2.noGravity = true;
                    fire2.scale = 0.8f;
                }

            }
        }

        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
            int dustsToMake = 5;
            if (projectile.ai[0] == UnifiedBulletProjectile.luminiteEyes ||
                projectile.ai[0] == ProjectileID.CrystalShard)
            {
                dustsToMake = 1;
            }
            
            for (int i = 0; i < dustsToMake; i++)
            {
                Dust fire = Dust.NewDustDirect(target.Center - new Vector2(-2,-2), 0, 0, ModContent.DustType<PhoenixLavaDust>());
                fire.velocity *= 2f;
                fire.velocity.Y -= 5f;
                fire.velocity += projectile.velocity * 0.1f;
                
                Dust smoke = Dust.NewDustDirect(target.Center - new Vector2(-2, -2), 0, 0, ModContent.DustType<RisingSmokeDust>());
                smoke.velocity *= 2f;
                smoke.scale = 0.3f;
                fire.velocity += projectile.velocity * 0.1f;
            }
            
        }

        public override void OnKill(ref Projectile projectile, ref int timeLeft)
        {
            int dustsToMake = 6;
            if (projectile.ai[0] == UnifiedBulletProjectile.luminiteEyes ||
                projectile.ai[0] == ProjectileID.CrystalShard)
            {
                dustsToMake = 1;
            }
            for (int i = 0; i < dustsToMake; i++)
            {
                Dust fire = Dust.NewDustDirect(projectile.position + new Vector2(-2,-2), 0, 0, ModContent.DustType<PhoenixLavaDust>());
                fire.velocity *= 3f;
                fire.position -= projectile.velocity * 1f;
                fire.velocity += projectile.velocity * -0.3f;
                
                Dust smoke = Dust.NewDustDirect(projectile.position + new Vector2(-2, -2), 0, 0, ModContent.DustType<RisingSmokeDust>());
                smoke.velocity *= 3f;
                smoke.position -= projectile.velocity * 1f;
                smoke.velocity += projectile.velocity * -0.3f;
                smoke.scale = 0.3f;
            }
        }
    }
}

