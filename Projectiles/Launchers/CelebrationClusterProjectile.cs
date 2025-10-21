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
using log4net.Core;
using Mono.Cecil;
using System.IO;
using Terraria.ModLoader.IO;

namespace WeaponRevamp.Projectiles.Launchers
{
    public class CelebrationClusterProjectile : ModProjectile
    {
        public int fireType = 0;
        public int radius = 4;
        public bool destroyTiles = false;
        private bool exploded = false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 15;
            Projectile.width = 8;
            Projectile.height = Projectile.width;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(fireType);
            writer.Write(radius);
            writer.WriteFlags(destroyTiles, exploded);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fireType = reader.ReadInt32();
            radius = reader.ReadInt32();
            reader.ReadFlags(out destroyTiles, out exploded);
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.ai[0] = 1;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity.Y += 0.1f;
            Projectile.velocity *= 0.9f;
            if (Projectile.shimmerWet)
            {
                Projectile.velocity.Y = -Math.Abs(Projectile.velocity.Y);
            }

            int num240 = Dust.NewDust(Projectile.position - Projectile.velocity*0.5f, 0, 0, DustID.Smoke, 0f, 0f, 100, default(Color), 1.6f);
            Main.dust[num240].noGravity = true;
            Main.dust[num240].velocity += Projectile.velocity * -0.2f;

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch(fireType)
            {
                case 0:
                    target.AddBuff(BuffID.OnFire3, 120);
                    break;
                case 1:
                    target.AddBuff(BuffID.Frostburn2, 120);
                    break;
                case 2:
                    target.AddBuff(BuffID.CursedInferno, 120);
                    break;
                case 3:
                    target.AddBuff(BuffID.ShadowFlame, 120);
                    break;
            }
            if(!exploded)
            {
                Projectile.Kill();
            }
            
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (exploded)
            {
                return (targetHitbox.Center.ToVector2() - projHitbox.Center.ToVector2()).Length() < radius * 16;
                
            }
            else
            {
                return false;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item89, Projectile.position);
            exploded = true;
            Projectile.Damage();
            float r = 1.3f;
            switch(fireType)
            {
                case 0: //fire
                    for(int i=0;i<8*radius;i++)
                    {
                        
                        Vector2 velocity = new Vector2(Main.rand.NextFloat() * r, 0).RotatedByRandom(Math.PI * 2) * radius;
                        
                        for (int j=0;j<4;j++)
                        {
                            Color fireColor = Color.Lerp(new Color(200, 64, 64), new Color(150, 120, 32), Main.rand.NextFloat());
                            Dust fireDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, fireColor, 1.1f);
                            fireDust.velocity = velocity;
                            fireDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.3f;
                            fireDust.noGravity = false;
                        }
                        
                    }
                    break;
                case 1: //frost
                    float rotation = Main.rand.NextFloat() * (float)Math.PI;
                    for(int i=0;i<6;i++)
                    {
                        for (int j = 0; j < 3*radius; j++)
                        {
                            Vector2 velocity = new Vector2(Main.rand.NextFloat() * r, 0).RotatedBy(rotation).RotatedBy(Math.PI / 3 * i) * radius;
                            Color frostColor = Color.Lerp(new Color(128, 150, 200), new Color(0, 64, 180), Main.rand.NextFloat());
                            Dust frostDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, frostColor, 0.9f);
                            frostDust.velocity = velocity;
                            frostDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.3f;
                            frostDust.noGravity = true;
                        }

                        for (int j = 0; j < radius; j++)
                        {
                            Vector2 velocity = Vector2.Lerp(new Vector2(r * 0.33f, 0), new Vector2(r * 0.47f, r * 0.33f), Main.rand.NextFloat()).RotatedBy(rotation).RotatedBy(Math.PI / 3 * i) * radius;
                            Color frostColor = Color.Lerp(new Color(128, 150, 200), new Color(0, 64, 180), Main.rand.NextFloat());
                            Dust frostDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, frostColor, 0.9f);
                            frostDust.velocity = velocity;
                            frostDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.1f;
                            frostDust.noGravity = true;
                        }
                        for (int j = 0; j < radius; j++)
                        {
                            Vector2 velocity = Vector2.Lerp(new Vector2(r * 0.33f, 0), new Vector2(r * 0.47f, r * -0.33f), Main.rand.NextFloat()).RotatedBy(rotation).RotatedBy(Math.PI / 3 * i) * radius;
                            Color frostColor = Color.Lerp(new Color(128, 150, 200), new Color(0, 64, 180), Main.rand.NextFloat());
                            Dust frostDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, frostColor, 0.9f);
                            frostDust.velocity = velocity;
                            frostDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.1f;
                            frostDust.noGravity = true;
                        }

                        for (int j = 0; j < radius; j++)
                        {
                            Vector2 velocity = Vector2.Lerp(new Vector2(r*0.66f, 0), new Vector2(r * 0.8f, r * 0.33f), Main.rand.NextFloat()).RotatedBy(rotation).RotatedBy(Math.PI / 3 * i) * radius;
                            Color frostColor = Color.Lerp(new Color(128, 150, 200), new Color(0, 64, 180), Main.rand.NextFloat());
                            Dust frostDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, frostColor, 0.9f);
                            frostDust.velocity = velocity;
                            frostDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.1f;
                            frostDust.noGravity = true;
                        }
                        for (int j = 0; j < radius; j++)
                        {
                            Vector2 velocity = Vector2.Lerp(new Vector2(r * 0.66f, 0), new Vector2(r * 0.8f, r * -0.33f), Main.rand.NextFloat()).RotatedBy(rotation).RotatedBy(Math.PI / 3 * i) * radius;
                            Color frostColor = Color.Lerp(new Color(128, 150, 200), new Color(0, 64, 180), Main.rand.NextFloat());
                            Dust frostDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, frostColor, 0.9f);
                            frostDust.velocity = velocity;
                            frostDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.1f;
                            frostDust.noGravity = true;
                        }
                    }
                    break;
                case 2: //cursed
                    for(int j=1;j<=5;j++)
                    {
                        for (int i = 0; i < j*2 * radius; i++)
                        {
                            Vector2 velocity = new Vector2(r*0.2f*j, 0).RotatedByRandom(Math.PI * 2) * radius;
                            Color cursedColor = Color.Lerp(new Color(100, 255, 64), new Color(150, 200, 32), Main.rand.NextFloat());
                            Dust cursedDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, cursedColor, 0.9f);
                            cursedDust.velocity = velocity;
                            cursedDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.1f * j;
                            cursedDust.noGravity = true;
                        }
                    }
                    
                    break;
                case 3: //shadow
                    float rotation2 = Main.rand.NextFloat() * (float)Math.PI/3 - (float)Math.PI/6;
                    for (int i = 0; i < 10 * radius; i++) //head
                    {
                        float rot = (float)( Main.rand.NextFloat()*(8 * Math.PI / 6) - (4 * Math.PI / 6));
                        Vector2 velocity = new Vector2(r*0.95f, 0).RotatedBy(rot).RotatedBy(rotation2 - Math.PI / 2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 2 * radius; i++) //cheek
                    {
                        Vector2 velocity = new Vector2((Main.rand.NextFloat()*0.5f+0.4f)*r, r*0.5f).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }
                    for (int i = 0; i < 2 * radius; i++)
                    {
                        Vector2 velocity = new Vector2((Main.rand.NextFloat() * 0.5f + 0.4f) * -r, r * 0.5f).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 2 * radius; i++) //jaw
                    {
                        Vector2 velocity = new Vector2(0.4f * r, (Main.rand.NextFloat() * 0.4f + 0.5f) * r).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }
                    for (int i = 0; i < 2 * radius; i++)
                    {
                        Vector2 velocity = new Vector2(0.4f * -r, (Main.rand.NextFloat() * 0.4f + 0.5f) * r).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 2 * radius; i++) //teeth
                    {
                        Vector2 velocity = new Vector2(0.15f * r, (Main.rand.NextFloat() * 0.3f + 0.6f) * r).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }
                    for (int i = 0; i < 2 * radius; i++)
                    {
                        Vector2 velocity = new Vector2(0.15f * -r, (Main.rand.NextFloat() * 0.3f + 0.6f) * r).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 2 * radius; i++) //chin
                    {
                        Vector2 velocity = new Vector2((Main.rand.NextFloat() * 0.8f - 0.4f) * r, r * 0.9f).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 4 * radius; i++) //eyes
                    {
                        Vector2 velocity = (new Vector2(-0.4f * r, r * -0.4f) + new Vector2(0.2f*r,0).RotatedByRandom(Math.PI*2)).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }
                    for (int i = 0; i < 4 * radius; i++)
                    {
                        Vector2 velocity = (new Vector2(0.4f * r, r * -0.4f) + new Vector2(0.2f * r, 0).RotatedByRandom(Math.PI * 2)).RotatedBy(rotation2) * radius;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    for (int i = 0; i < 4 * radius; i++) //nose
                    {
                        Vector2 velocity = (new Vector2(0, r * 0.1f) + new Vector2(Main.rand.NextFloat() * 0.1f * r, 0).RotatedByRandom(Math.PI * 2)).RotatedBy(rotation2) * radius;
                        velocity.Y *= 2f;
                        Color shadowColor = Color.Lerp(new Color(32, 0, 64), new Color(150, 64, 255), Main.rand.NextFloat());
                        Dust shadowDust = Dust.NewDustDirect(Projectile.position + velocity, 0, 0, DustID.FireworksRGB, 0, 0, 0, shadowColor, 0.9f);
                        shadowDust.velocity = velocity;
                        shadowDust.velocity += new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1) * 0.5f;
                        shadowDust.noGravity = true;
                    }

                    break;
                default:

                    break;

            }
            if (destroyTiles && Projectile.owner == Main.myPlayer)
            {
                int explosionRadius = (int)((float)radius * 0.3); // Bomb: 4, Dynamite: 7, Explosives & TNT Barrel: 10
                int minTileX = (int)(Projectile.Center.X / 16f - explosionRadius);
                int maxTileX = (int)(Projectile.Center.X / 16f + explosionRadius);
                int minTileY = (int)(Projectile.Center.Y / 16f - explosionRadius);
                int maxTileY = (int)(Projectile.Center.Y / 16f + explosionRadius);

                // Ensure that all tile coordinates are within the world bounds
                Utils.ClampWithinWorld(ref minTileX, ref minTileY, ref maxTileX, ref maxTileY);

                // These 2 methods handle actually mining the tiles and walls while honoring tile explosion conditions
                bool explodeWalls = Projectile.ShouldWallExplode(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY);
                Projectile.ExplodeTiles(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY, explodeWalls);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(4, 2);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(4, 2, fireType, (destroyTiles?1:0)), new Color(255, 255, 255, 255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            



            return false;

        }

    }
}
