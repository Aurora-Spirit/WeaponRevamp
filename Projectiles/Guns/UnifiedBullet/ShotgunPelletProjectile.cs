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

namespace WeaponRevamp.Projectiles.Guns.UnifiedBullet
{
    public class ShotgunPelletProjectile : UnifiedBulletProjectile
    {
        public override void SetDefaults()
        {
            //SetDefaultDefaults();
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 6;
            Projectile.alpha = 255;
            Projectile.scale = 1.3f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AddBehaviors()
        {
            base.AddBehaviors();
            /*if (Projectile.ai[0] == ProjectileID.MeteorShot)
            {
                behaviors.Add(new AirResistance(0.993f));
            }
            else if (Projectile.ai[0] != ProjectileID.CrystalShard)
            {
                behaviors.Add(new AirResistance(0.99f));
            }*/
            /*switch (Projectile.ai[0])
            {
                case ProjectileID.Bullet:
                    behaviors.RemoveAt(1);
                    behaviors.Add(new SmokeTrail());
                    break;
                case ProjectileID.MeteorShot:
                    behaviors.Add(new MeteorTrail());
                    behaviors.Add(new BulletGlow(new Color(170,0,255), new Color(0.8f,0f,0.2f), shineTexture.Value));
                    behaviors.Add(new BulletBounce(2));
                    break;
                case ProjectileID.SilverBullet:
                    behaviors.Add(new SmokeTrail());
                    behaviors.Add(new SilverBulletGlow(new Color(255,255,255), new Color(0.8f,0.7f,0.6f), shineTexture.Value));
                    break;
                case ProjectileID.CrystalBullet:
                    behaviors.Add(new CrystalTrail());
                    behaviors.Add(new BulletGlow(new Color(60,200,255), new Color(0.3f,0.5f,0.8f), shineTexture.Value));
                    behaviors.Add(new CrystalShatter());
                    break;
                case ProjectileID.CrystalShard:
                    behaviors.Add(new ShardRender(new Color(Projectile.ai[2],0.5f,1f), new Color(Projectile.ai[2]*0.3f,0.1f,0.3f), shardTexture.Value));
                    behaviors.Add(new BulletBounce(2));
                    behaviors.Add(new AirResistance(0.97f));
                    behaviors.Add(new ShardTrail());
                    break;
                case ProjectileID.CursedBullet:
                    break;
                case ProjectileID.ChlorophyteBullet:
                    behaviors.Add(new ChlorophyteTrail());
                    behaviors.Add(new BulletGlow(new Color(100,255,80), new Color(0.0f,0.5f,0.1f), shineTexture.Value));
                    behaviors.Add(new ChlorophyteHoming());
                    break;
                case ProjectileID.BulletHighVelocity:
                    behaviors.Add(new HighVelocityTrail());
                    behaviors.Add(new BulletGlow(new Color(255,255,255), new Color(1f,1f,0.5f), shineTexture.Value));
                    behaviors.Add(new HighVelocityHitSpark());
                    break;
                case ProjectileID.IchorBullet:
                    break;
                case ProjectileID.VenomBullet:
                    break;
                case ProjectileID.PartyBullet:
                    behaviors.Add(new RainbowSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(255,128,255), new Color(1f,0.4f,1f), shineTexture.Value));
                    behaviors.Add(new ConfettiOnHit());
                    behaviors.Add(new ConfuseEnemies());
                    break;
                default:
                    break;
            }*/
        }
    }
}

