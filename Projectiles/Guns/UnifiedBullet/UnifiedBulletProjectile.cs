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
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.Guns.UnifiedBullet
{
    public class UnifiedBulletProjectile : ModProjectile
    {
        
        private static Asset<Texture2D> shineTexture;
        private static Asset<Texture2D> shardTexture;
        private static Asset<Texture2D> redLightTexture;
        private static Asset<Texture2D> luminiteEyeTexture;
        private static SoundStyle shockSoundStyle;
        public const int tungsten = 1;
        public const int luminiteEyes = 2;
        private bool behaviorsAdded = false;

        public override string Texture => "WeaponRevamp/Projectiles/Guns/UnifiedBullet/UnifiedBulletProjectile";


        public override void Load() {
            shineTexture = ModContent.Request<Texture2D>("WeaponRevamp/Projectiles/Guns/UnifiedBullet/Bullet_Shine");
            shardTexture = ModContent.Request<Texture2D>("WeaponRevamp/Projectiles/Guns/UnifiedBullet/Crystal_Shard");
            redLightTexture = ModContent.Request<Texture2D>("WeaponRevamp/Projectiles/Guns/UnifiedBullet/Red_Light");
            luminiteEyeTexture = ModContent.Request<Texture2D>("WeaponRevamp/Projectiles/Guns/UnifiedBullet/Luminite_Eye");
            shockSoundStyle = new SoundStyle("WeaponRevamp/Projectiles/Guns/UnifiedBullet/shock")
            {
                PauseBehavior = PauseBehavior.StopWhenGamePaused,
                Volume = 0.2f,
                Type = SoundType.Sound
            };
        }
        
        public List<BulletBehavior> behaviors = new List<BulletBehavior>();
        
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
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 2;
            Projectile.alpha = 255;
            Projectile.scale = 1.3f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public virtual void AddBehaviors()
        {
            behaviors.Add(new HitTiles());
            switch (Projectile.ai[0])
            {
                case ProjectileID.Bullet:
                    behaviors.Add(new SmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(255,200,100), new Color(0.7f,0.3f,0f), shineTexture.Value));
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
                    behaviors.Add(new CursedSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(128,256,64), new Color(0.5f,1f,0f), shineTexture.Value));
                    behaviors.Add(new CursedFlameBlast(70));
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
                    behaviors.Add(new PissSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(255,128,64), new Color(0.8f,0.4f,0f), shineTexture.Value));
                    behaviors.Add(new IchorSplatter(48));
                    break;
                case ProjectileID.VenomBullet:
                    behaviors.Add(new VenomSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(200,64,255), new Color(0.8f,0f,1f), shineTexture.Value));
                    behaviors.Add(new StickIntoEnemy());
                    behaviors.Add(new AcidVenom());
                    break;
                case ProjectileID.PartyBullet:
                    behaviors.Add(new RainbowSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(255,128,255), new Color(1f,0.4f,1f), shineTexture.Value));
                    behaviors.Add(new ConfettiOnHit());
                    behaviors.Add(new ConfuseEnemies());
                    break;
                case ProjectileID.NanoBullet:
                    behaviors.Add(new NanoSmokeTrail());
                    behaviors.Add(new BulletGlow(new Color(192,220,255), new Color(0.3f,0.8f,1f), shineTexture.Value));
                    behaviors.Add(new Richochet());
                    break;
                case ProjectileID.ExplosiveBullet:
                    behaviors.Add(new ExplosiveSmokeTrail());
                    behaviors.Add(new RedLightRender(new Color(100,80,40), shineTexture.Value));
                    behaviors.Add(new ExplodeInRange(16*5));
                    break;
                case ProjectileID.GoldenBullet:
                    behaviors.Add(new GoldenSmokeTrail());
                    behaviors.Add(new SilverBulletGlow(new Color(255,200,0), new Color(1f,0.8f,0f), shineTexture.Value));
                    behaviors.Add(new MidasOnHit());
                    behaviors.Add(new ReverseDirectionOnHit());
                    break;
                case ProjectileID.MoonlordBullet:
                    behaviors.Add(new MoonlordTrail());
                    behaviors.Add(new BulletGlow(new Color(0,255,255), new Color(0f,1f,1f), shineTexture.Value));
                    behaviors.Add(new EyeSummonTrail());
                    break;
                case luminiteEyes:
                    behaviors.Add(new EyeRender(new Color(0,255,255), new Color(0f,1f,1f), luminiteEyeTexture.Value));
                    behaviors.Add(new EyeTrail());
                    behaviors.Add(new LuminiteEyeHoming());
                    break;
                case tungsten:
                    behaviors.Add(new SmokeTrail());
                    behaviors.Add(new SilverBulletGlow(new Color(128,255,164), new Color(0.5f,0.8f,0.6f), shineTexture.Value));
                    break;
                default:
                    if (Projectile.ai[0] == ModContent.ProjectileType<ElectricBulletProjectile>())
                    {
                        behaviors.Add(new SmokeTrail());
                        behaviors.Add(new ElectricTrail());
                        behaviors.Add(new BulletGlow(new Color(64,210,255), new Color(0.3f,0.9f,1f), shineTexture.Value));
                        behaviors.Add(new ElectricBurst(160,2, shockSoundStyle));
                    }
                    break;
            }
        }

        public virtual void SetAmmoDefaults()
        {
            switch (Projectile.ai[0])
            {
                case ProjectileID.Bullet:
                    break;
                case ProjectileID.MeteorShot:
                    Projectile.penetrate += 1;
                    Projectile.maxPenetrate += 1;
                    Projectile.timeLeft = (int)(Projectile.timeLeft * 1.5);
                    break;
                case ProjectileID.SilverBullet:
                    break;
                case ProjectileID.CrystalBullet:
                    break;
                case ProjectileID.CrystalShard:
                    Projectile.extraUpdates = 1;
                    Projectile.MaxUpdates = 2;
                    Projectile.timeLeft = 72;
                    break;
                case ProjectileID.CursedBullet:
                    break;
                case ProjectileID.ChlorophyteBullet:
                    ProjectileID.Sets.CultistIsResistantTo[Projectile.whoAmI] = true;
                    Projectile.localNPCHitCooldown = -1;
                    break;
                case ProjectileID.BulletHighVelocity:
                    Projectile.extraUpdates *= 3;
                    Projectile.extraUpdates += 2;
                    Projectile.MaxUpdates *= 3;
                    Projectile.penetrate += 4;
                    Projectile.maxPenetrate += 4;
                    break;
                case ProjectileID.IchorBullet:
                    break;
                case ProjectileID.VenomBullet:
                    Projectile.penetrate += 2;
                    Projectile.maxPenetrate += 2;
                    Projectile.hide = true;
                    break;
                case ProjectileID.PartyBullet:
                    break;
                case ProjectileID.NanoBullet:
                    Projectile.localNPCHitCooldown = 60;
                    Projectile.penetrate += 4;
                    Projectile.maxPenetrate += 4;
                    Projectile.timeLeft = (int)(Projectile.timeLeft * 1.5);
                    break;
                case ProjectileID.ExplosiveBullet:
                    ProjectileID.Sets.CultistIsResistantTo[Projectile.whoAmI] = true;
                    break;
                case ProjectileID.GoldenBullet:
                    Projectile.penetrate += 1;
                    Projectile.maxPenetrate += 1;
                    break;
                case ProjectileID.MoonlordBullet:
                    Projectile.penetrate = -1;
                    Projectile.maxPenetrate = -1;
                    break;
                case luminiteEyes:
                    Projectile.extraUpdates = 1;
                    Projectile.MaxUpdates = 2;
                    Projectile.timeLeft = 72;
                    ProjectileID.Sets.CultistIsResistantTo[Projectile.whoAmI] = true;
                    break;
                case tungsten:
                    break;
                default:
                    if (Projectile.ai[0] == ModContent.ProjectileType<ElectricBulletProjectile>())
                    {
                        
                    }
                    break;
            }
        }
        
        public override void AI()
        {
            if (!behaviorsAdded)
            {
                AddBehaviors();
                SetAmmoDefaults();
                behaviorsAdded = true;
            }

            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].AI(ref Main.projectile[Projectile.whoAmI]);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1]++;
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].OnHitNPC(ref Main.projectile[Projectile.whoAmI], ref target, ref hit, ref damageDone);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            bool? returnValue = base.CanHitNPC(target);
            for (int i = 0; i < behaviors.Count; i++)
            {
                bool? currentValue = behaviors[i].CanHitNPC(ref Main.projectile[Projectile.whoAmI], ref target);
                if (currentValue != null)
                {
                    returnValue = currentValue;
                }
            }
            return returnValue;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].ModifyHitNPC(ref Main.projectile[Projectile.whoAmI], ref target, ref modifiers);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool? returnValue = base.Colliding(projHitbox, targetHitbox);
            for (int i = 0; i < behaviors.Count; i++)
            {
                bool? currentValue = behaviors[i].Colliding(projHitbox, targetHitbox);
                if (currentValue != null)
                {
                    returnValue = currentValue;
                }
            }
            return returnValue;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].DrawBehind(ref Main.projectile[Projectile.whoAmI], ref index, ref behindNPCsAndTiles, ref behindNPCs, ref behindProjectiles, ref overPlayers, ref overWiresUI);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool returnValue = true;
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (returnValue)
                {
                    returnValue = behaviors[i].OnTileCollide(ref Main.projectile[Projectile.whoAmI], ref oldVelocity);
                }
            }
            return returnValue;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].OnKill(ref Main.projectile[Projectile.whoAmI], ref timeLeft);
            }
        }

        /*public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].DrawBehind(ref Main.projectile[Projectile.whoAmI], ref index, ref behindNPCsAndTiles, ref behindNPCs, ref behindProjectiles, ref overPlayers, ref overWiresUI);
            }
            if (Projectile.ai[0] == ProjectileID.VenomBullet)
            {
                behindNPCs.Append()
            }
        }*/


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
            
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 16, 0, frameNum), renderAlpha, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].Draw(ref Main.projectile[Projectile.whoAmI], ref lightColor);
            }

            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].SendExtraAI(ref writer);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].ReceiveExtraAI(ref reader);
            }
        }

        public static Projectile NewUnifiedBullet(IEntitySource spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int ammoType, int owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
        {
            switch (((EntitySource_ItemUse_WithAmmo)spawnSource).AmmoItemIdUsed)
            {
                case ItemID.MusketBall:
                case ItemID.EndlessMusketPouch: ammoType = ProjectileID.Bullet; break;
                case ItemID.MeteorShot: ammoType = ProjectileID.MeteorShot; break;
                case ItemID.SilverBullet: ammoType = ProjectileID.SilverBullet; break;
                case ItemID.CrystalBullet: ammoType = ProjectileID.CrystalBullet; break;
                case ItemID.CursedBullet: ammoType = ProjectileID.CursedBullet; break;
                case ItemID.ChlorophyteBullet: ammoType = ProjectileID.ChlorophyteBullet; break;
                case ItemID.HighVelocityBullet: ammoType = ProjectileID.BulletHighVelocity; break;
                case ItemID.IchorBullet: ammoType = ProjectileID.IchorBullet; break;
                case ItemID.VenomBullet: ammoType = ProjectileID.VenomBullet; break;
                case ItemID.PartyBullet: ammoType = ProjectileID.PartyBullet; break;
                case ItemID.NanoBullet: ammoType = ProjectileID.NanoBullet; break;
                case ItemID.ExplodingBullet: ammoType = ProjectileID.ExplosiveBullet; break;
                case ItemID.GoldenBullet: ammoType = ProjectileID.GoldenBullet; break;
                case ItemID.MoonlordBullet: ammoType = ProjectileID.MoonlordBullet; break;
                case ItemID.TungstenBullet: ammoType = tungsten; break;
                default:
                    if (((EntitySource_ItemUse_WithAmmo)spawnSource).AmmoItemIdUsed == ModContent.ItemType<Items.Ammo.ElectricBullet>()) //check for spectre arrow, since switch statements hate ModContent
                    {
                        ammoType = ModContent.ProjectileType<ElectricBulletProjectile>();
                    } else
                    {
                        type = 0; //modded ammo (or a bug) detected!
                    }
                    break; 

            }
            
            Projectile proj;
            if(type == 0) //for modded ammo, just creates a vanilla projectile
            {
                proj = Projectile.NewProjectileDirect(spawnSource, position, velocity, ammoType, damage, knockback, owner, ai0, ai1, ai2);
            } else
            {
                proj = Projectile.NewProjectileDirect(spawnSource, position, velocity, type, damage, knockback, owner, ammoType, ai1, ai2);
            }
            //((UnifiedArrowProjectile)Main.projectile[index])
            return proj;
        }
        
        
    }
    
    
    


    public abstract class BulletBehavior()
    {
        public virtual void DrawBehind(ref Projectile projectile, ref int index, ref List<int> behindNPCsAndTiles, ref List<int> behindNPCs, ref List<int> behindProjectiles, ref List<int> overPlayers, ref List<int> overWiresUI)
        {
        }
        public virtual void SendExtraAI(ref BinaryWriter writer) { }
        
        public virtual void ReceiveExtraAI(ref BinaryReader reader) { }
        
        public virtual void AI(ref Projectile projectile) { }

        public virtual void OnHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitInfo hit, ref int damageDone) { }

        public virtual bool? CanHitNPC(ref Projectile projectile, ref NPC target)
        {
            return null;
        }
        
        public virtual void ModifyHitNPC(ref Projectile projectile, ref NPC target, ref NPC.HitModifiers modifiers) { }

        public virtual bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }

        public virtual bool OnTileCollide(ref Projectile projectile, ref Vector2 oldVelocity)
        {
            return true;
        }

        public virtual void OnKill(ref Projectile projectile, ref int timeLeft) { }

        public virtual void Draw(ref Projectile projectile, ref Color lightColor) { }


    }
}

