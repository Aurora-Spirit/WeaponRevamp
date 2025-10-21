using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace WeaponRevamp.Projectiles.Bows

{
    public class MoltenFuryGeyserProjectile:ModProjectile
    {
        public override void SetDefaults(){
            Projectile.width=26;
            Projectile.height=26;
            Projectile.aiStyle=0;
            Projectile.friendly=true;
            Projectile.hostile=false;
            Projectile.penetrate=-1;
            Projectile.timeLeft=6;
            Projectile.ignoreWater=true;
            Projectile.tileCollide=false;
            Projectile.extraUpdates=0;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Center = Main.player[Projectile.owner].MountedCenter;
            SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
        }

        public override void AI()
        {
            
            Projectile.alpha -= 32;
            if(Projectile.alpha<=0)
            {
                Projectile.alpha = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            for (int i = 0; i < 3; i++)
            {

                Color dustColor = Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat() * 1f);
                Vector2 dustPos = Projectile.position + new Vector2(Main.rand.NextFloat() * Projectile.width, Main.rand.NextFloat() * Projectile.height);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch, new Vector2(Main.rand.NextFloat() * 2f - 1f, Main.rand.NextFloat() * 2f - 1f), 100, dustColor, 1.3f);
                dust.velocity *= 1;
                dust.velocity += Projectile.velocity * 0.5f * (Main.rand.NextFloat()+0.1f);
                /*if(Main.rand.NextBool(2))
                {
                    dustIndex.noGravity = true;
                }*/
            }
            for (int i = 0; i < 2; i++)
            {

                Color dustColor = Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat() * 1f);
                Vector2 dustPos = Projectile.position + new Vector2(Main.rand.NextFloat() * Projectile.width, Main.rand.NextFloat() * Projectile.height);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.FireworksRGB, new Vector2(Main.rand.NextFloat() * 2 - 1, Main.rand.NextFloat() * 2 - 1), 100, dustColor, 1f);
                dust.velocity *= 1;
                dust.velocity += Projectile.velocity;
                if (Main.rand.NextBool(2))
                {
                    dust.noGravity = true;
                }
            }

            /*for (int i = 0; i < 5; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 1, 1, DustID.Smoke, 0f, 0f);
            }*/
            

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            Projectile.damage = (int)((double)Projectile.damage*0.7);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, 0), new Color(255, 255, 255, 127) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}