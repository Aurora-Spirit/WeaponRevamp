using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Microsoft.CodeAnalysis.Diagnostics;
using WeaponRevamp.Buffs.Bows;
using WeaponRevamp.Dusts;
using WeaponRevamp.Gores;
using System.IO;

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisHellfireArrowProjectile : ModProjectile
    {
        bool exploded;
        bool explosionReady;
        public override void SetDefaults()
        {

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 1200; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            exploded = false;
            explosionReady = false;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(explosionReady);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            explosionReady = reader.ReadBoolean();
            base.ReceiveExtraAI(reader);
        }

        // Custom AI
        public override void AI()
        {
            if(!exploded&&explosionReady)
            {
                HellfireExplode();
                exploded = true;
            }
            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item61, Projectile.position);
            }
            if (!exploded) {
                Projectile.rotation = Projectile.velocity.ToRotation();
                //Lighting.AddLight(Projectile.position, new Vector3(Main.DiscoR / 256.0f, Main.DiscoG / 256.0f, Main.DiscoB / 256.0f));

                Projectile.ai[0]++;
                if (Projectile.ai[0] > 15)
                {
                    Projectile.velocity.Y += 0.1f;
                }
                Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y, -16, 16);
                Lighting.AddLight(Projectile.Center, 1f * 0.3f, 0.8f * 0.3f, 0.6f * 0.3f);
                int num240 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.6f);
                Main.dust[num240].noGravity = true;
                Main.dust[num240].velocity += Projectile.velocity * -0.2f;
                num240 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num240].noGravity = true;
                Main.dust[num240].velocity += Projectile.velocity * -0.2f;
                if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;
            } else
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust fire = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Torch);
                    fire.scale *= Main.rand.NextFloat() + 0.7f;
                    fire.velocity = Vector2.Zero;
                    fire.position += new Vector2(160, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                    fire.velocity += new Vector2(2, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                }
            }
            
        }

        public override void OnKill(int timeLeft)
        {


            base.OnKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);

            if (!explosionReady)
            {
                explosionReady = true;
                Projectile.netUpdate = true;
                //base.OnHitNPC(target, hit, damageDone);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.Center.X > Projectile.Center.X)
            {
                modifiers.HitDirectionOverride = 1;
            }
            else
            {
                modifiers.HitDirectionOverride = -1;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(!explosionReady)
            {
                explosionReady = true;
                Projectile.netUpdate = true;
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(!exploded)
            {
                projHitbox.X -= 16;
                projHitbox.Y -= 16;
                projHitbox.Width += 32;
                projHitbox.Height += 32;
                return base.Colliding(projHitbox, targetHitbox);
            }
            else
            {
                return (targetHitbox.Center.ToVector2() - projHitbox.Center.ToVector2()).Length() < 160;
            }
        }
        /*public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            target.AddBuff(ModContent.BuffType<DeathToxin>(), 300);
        }*/
        public override bool PreDraw(ref Color lightColor)
        {
            if (!exploded) {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, 0), new Color(255, 255, 255, 255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }


            return false;

        }

        private void HellfireExplode()
        {
            Projectile.timeLeft = 220;
            exploded = true;
            SoundEngine.PlaySound(in SoundID.DD2_ExplosiveTrapExplode, Projectile.position);

            Shockwave.NewShockwave(Projectile.Center, 160, 18, new Color(255, 255, 255, 128));
            
            for(int i=0;i<120;i++)
            {
                Dust fire = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Torch);
                fire.scale *= Main.rand.NextFloat() + 2f;
                fire.position += new Vector2(64, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                fire.velocity = new Vector2(5, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
            }

            int maxBlastDust = 80;
            for (int i = 0; i < maxBlastDust; i++)
            {
                Dust blast = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.FireworksRGB, 0, 0, 0, new Color(200, 190, 180), 1f);
                blast.velocity = new Vector2(10, 0).RotatedBy(Math.PI * 2 * i / maxBlastDust);
                blast.scale += blast.velocity.Y * 0.06f;
                blast.noGravity = true;
                blast.scale *= 0.7f;
                blast.velocity.Y *= 0.1f;
                blast.velocity *= 1.5f;
                //Main.NewText(Math.Round(Projectile.velocity.Y * 10) + ", " + (Math.Round(Projectile.oldVelocity.Y * 10) + 1));
                if (Collision.SolidCollision(Projectile.position + new Vector2(0, 16), 10, 10)) blast.velocity.Y -= 2f;
                if (Collision.SolidCollision(Projectile.position + new Vector2(0, 32), 10, 10)) blast.velocity.Y -= 1f;
                if (Collision.SolidCollision(Projectile.position + new Vector2(0, -16), 10, 10)) blast.velocity.Y += 2f;
                if (Collision.SolidCollision(Projectile.position + new Vector2(0, -32), 10, 10)) blast.velocity.Y += 1f;
            }

            /*for(int i=0;i<40;i++)
            {
                int[] goreIds = { ModContent.GoreType<BurningCloud1>(), ModContent.GoreType<BurningCloud2>(), ModContent.GoreType<BurningCloud3>() };
                Gore burningSmoke = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.NextFromList<int>(goreIds));
                burningSmoke.velocity *= 0f;
                burningSmoke.velocity += new Vector2(2, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                burningSmoke.velocity.X *= 3f;
                burningSmoke.velocity.Y -= 6f;
                burningSmoke.scale *= Main.rand.NextFloat() * 0.5f + 1f;
                burningSmoke.timeLeft = 210 + Main.rand.Next(0,60);
                burningSmoke.alpha = Main.rand.Next(64, 128);
            }*/
            //Main.NewText(Main.dust.Length);
            for (int i = 0; i < 20; i++)
            {
                
                int[] goreIds = { ModContent.GoreType<BurningCloud1>(), ModContent.GoreType<BurningCloud2>(), ModContent.GoreType<BurningCloud3>() };
                Gore burningSmoke = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position - new Vector2(8, 8), Vector2.Zero, Main.rand.NextFromList<int>(goreIds));
                burningSmoke.velocity = new Vector2(2, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                burningSmoke.velocity.X *= 2.5f;
                burningSmoke.velocity.Y -= 6f;
                burningSmoke.scale *= Main.rand.NextFloat() * 0.5f + 1f;
                burningSmoke.timeLeft = 210 + Main.rand.Next(0, 60);
                burningSmoke.alpha = Main.rand.Next(64, 128);
            }

            for (int i = 0; i < 20; i++)
            {
                int[] goreIds = { ModContent.GoreType<BurningCloud1>(), ModContent.GoreType<BurningCloud2>(), ModContent.GoreType<BurningCloud3>() };
                Gore burningSmoke = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position - new Vector2(15, 15), Vector2.Zero, Main.rand.NextFromList<int>(goreIds));
                burningSmoke.velocity = new Vector2(2, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                burningSmoke.velocity.X *= 0.5f;
                burningSmoke.velocity.Y *= 3f;
                burningSmoke.scale *= Main.rand.NextFloat() * 0.5f + 1f;
                burningSmoke.timeLeft = 210 + Main.rand.Next(0, 60);
                burningSmoke.alpha = Main.rand.Next(64, 128);
            }

            for (int i = 0; i < 20; i++)
            {
                int[] goreIds = { ModContent.GoreType<BurningCloud1>(), ModContent.GoreType<BurningCloud2>(), ModContent.GoreType<BurningCloud3>() };
                Gore burningSmoke = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position - new Vector2(15, 15), Vector2.Zero, Main.rand.NextFromList<int>(goreIds));
                burningSmoke.velocity = new Vector2(2, 0).RotatedByRandom(Math.PI * 2) * Main.rand.NextFloat();
                burningSmoke.velocity.X *= 2.5f;
                burningSmoke.velocity.Y += 6f;
                burningSmoke.scale *= Main.rand.NextFloat() * 0.5f + 1f;
                burningSmoke.timeLeft = 210 + Main.rand.Next(0, 60);
                burningSmoke.alpha = Main.rand.Next(64, 128);
            }

            Projectile.velocity = Vector2.Zero;
            Projectile.Damage();
            Projectile.knockBack = 0;
            Projectile.damage = (int)(Projectile.damage * 0.33f);
        }

        /*public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width /= 3;
            height /= 3;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }*/


    }
}
