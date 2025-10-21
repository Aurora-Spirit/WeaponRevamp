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
    public class SniperRifleProjectile : UnifiedBulletProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AddBehaviors()
        {
            base.AddBehaviors();
            behaviors.Add(new SniperDistanceScaling());
        }

        public override void SetAmmoDefaults()
        {
            base.SetAmmoDefaults();
            Projectile.MaxUpdates *= 3;
            Projectile.extraUpdates = Projectile.MaxUpdates-1;
            Projectile.maxPenetrate *= 3;
            Projectile.penetrate = Projectile.maxPenetrate;
            if (Projectile.ai[0] == ProjectileID.CrystalShard)
            {
                Projectile.velocity *= 1.5f;
                Projectile.timeLeft = 96;
            }
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 16);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            int stretch;
            switch (Projectile.ai[0]) //projectile id corresponding to location in sprite
            {
                case ProjectileID.Bullet: frameNum = 0; break; //musket ball
                case ProjectileID.MeteorShot: frameNum = 1; break; //meteor shot
                case ProjectileID.SilverBullet: frameNum = 2; break; //silver bullet
                case ProjectileID.CrystalBullet: frameNum = 3; break; //silver bullet
                case ProjectileID.CursedBullet: frameNum = 4; break; //silver bullet
                case ProjectileID.ChlorophyteBullet: frameNum = 5; break; //silver bullet
                case ProjectileID.BulletHighVelocity: frameNum = 6; break; //silver bullet
                case ProjectileID.IchorBullet: frameNum = 7; break; //silver bullet
                case ProjectileID.VenomBullet: frameNum = 8; break; //silver bullet
                case ProjectileID.PartyBullet: frameNum = 9; break; //silver bullet
                case ProjectileID.NanoBullet: frameNum = 10; break; //silver bullet
                case ProjectileID.ExplosiveBullet: frameNum = 11; break; //silver bullet
                case ProjectileID.GoldenBullet: frameNum = 12; break; //silver bullet
                case ProjectileID.MoonlordBullet: frameNum = 13; break; //silver bullet
                case tungsten: frameNum = 14; break; //silver bullet
                default: 
                    if (Projectile.ai[0] == ModContent.ProjectileType<ElectricBulletProjectile>())
                    {
                        frameNum = 15;
                    }
                    else
                    {
                        frameNum = 0; 
                    }
                    break;
            }
            /*if (Projectile.ai[0] == spectre) //for electric bullet later
            {
                if (spectreSpinTime > 0) { frameNum = 14; } else { frameNum = 15; }
            }*/
            Color renderAlpha;
            switch (Projectile.ai[0]) //projectile rendering alpha
            {
                case ProjectileID.Bullet:
                case ProjectileID.MeteorShot:
                case ProjectileID.SilverBullet:
                case ProjectileID.CrystalBullet:
                case ProjectileID.CursedBullet:
                case ProjectileID.ChlorophyteBullet:
                case ProjectileID.BulletHighVelocity:
                case ProjectileID.IchorBullet:
                case ProjectileID.VenomBullet:
                case ProjectileID.PartyBullet:
                case ProjectileID.NanoBullet:
                case ProjectileID.ExplosiveBullet:
                case ProjectileID.GoldenBullet:
                case ProjectileID.MoonlordBullet:
                case tungsten:
                    renderAlpha = new Color(255, 255, 255, 128); break;
                case ProjectileID.CrystalShard:
                case luminiteEyes:
                    renderAlpha = new Color(0, 0, 0, 0);
                    break;
                default:
                    renderAlpha = new Color(255, 255, 255, 128); break;
            }

            /*for (int i = 0; i < Projectile.velocity.Length() * Projectile.MaxUpdates && i < Projectile.ai[1]*sourceRectangle.Width+1; i += sourceRectangle.Width)
            {
                Main.EntitySpriteDraw(texture, position - Vector2.Normalize(Projectile.velocity)*i, texture.Frame(1, 16, 0, frameNum), renderAlpha, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }*/
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), renderAlpha, Projectile.rotation, origin, new Vector2(Projectile.velocity.Length()/15f*4+1,1)*scale, SpriteEffects.None, 0f);
            
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].Draw(ref Main.projectile[Projectile.whoAmI], ref lightColor);
            }

            return false;
        }
    }
    
    

    public class SniperDistanceScaling : BulletBehavior
    {
        public override void ModifyHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitModifiers modifiers)
        {
            float bonus = projectile.ai[1] * 0.01f;
            modifiers.SourceDamage += bonus;
        }
    }
}

