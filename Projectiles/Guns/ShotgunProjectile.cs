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
    public class ShotgunProjectile : ShotgunPelletProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AddBehaviors()
        {
            base.AddBehaviors();
            if (Projectile.ai[0] == ProjectileID.CrystalShard || Projectile.ai[0] == luminiteEyes)
            {
            }
            else
            {
                behaviors.Add(new ReduceCooldownOnHit());
            }
        }

        public override void SetAmmoDefaults()
        {
            base.SetAmmoDefaults();
        }
    }
    
    

    public class ReduceCooldownOnHit : BulletBehavior
    {
        private bool hasHit = false;
        public override void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone)
        {
            
            if (!hasHit)
            {
                hasHit = true;
                Player owner = Main.player[projectile.owner];
                if (owner.itemTime > 8 && owner.itemAnimation > 8 && owner.HeldItem.type == ItemID.Shotgun)
                {
                    if (projectile.ai[0] == ProjectileID.ChlorophyteBullet || projectile.ai[0] == ProjectileID.ExplosiveBullet)
                    {
                        owner.itemAnimation -= 4;
                        owner.itemTime -= 4;
                    }
                    else
                    {
                        owner.itemAnimation -= 6;
                        owner.itemTime -= 6;
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        Dust redGlow = Dust.NewDustDirect(projectile.position,0,0,DustID.LifeDrain);
                        redGlow.velocity *= 0.2f;
                        redGlow.velocity += Vector2.Normalize(owner.MountedCenter - redGlow.position)*Main.rand.NextFloat(0,2);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Dust redGlow = Dust.NewDustDirect(owner.Center,0,0,DustID.LifeDrain);
                        redGlow.velocity *= 1f;
                        redGlow.position -= redGlow.velocity * 32f;
                    }
                }
            }
        }
    }
}

