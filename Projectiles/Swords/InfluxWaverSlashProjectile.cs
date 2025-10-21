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

namespace WeaponRevamp.Projectiles.Swords

{
    public class InfluxWaverSlashProjectile:ModProjectile
    {
        private const int minAlpha = 0;

        public override void SetDefaults(){
            Projectile.width=10;
            Projectile.height=10;
            Projectile.aiStyle=0;
            Projectile.friendly=true;
            Projectile.hostile=false;
            Projectile.penetrate=-1;
            Projectile.timeLeft=60;
            Projectile.light = 0.2f;
            Projectile.ignoreWater=false;
            Projectile.tileCollide=false;
            Projectile.extraUpdates=0;
            Projectile.localNPCHitCooldown = 0;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = -3f * Vector2.Normalize(Projectile.velocity).X; //acceleration X
            Projectile.ai[1] = -3f * Vector2.Normalize(Projectile.velocity).Y; //acceleration Y
            Projectile.ai[2] = 0; //number of enemies hit

            Projectile.netUpdate = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 60)
            {
                Projectile.velocity *= 1.5f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.alpha = 255;
            }
            //vanilla influx waver dust code
            /*int dustType = Utils.SelectRandom<int>(Main.rand, 226, 229);
            Vector2 center19 = Projectile.Center;
            Vector2 spinningpoint4 = new Vector2(-16f, 16f);
            float num716 = 1f;
            spinningpoint4 += new Vector2(-16f, 16f);
            spinningpoint4 = spinningpoint4.RotatedBy(Projectile.rotation);
            int num717 = 4;
            int num718 = Dust.NewDust(center19, num717 * 2, num717 * 2, dustType, 0f, 0f, 100, default(Color), num716);
            Dust dust97 = Main.dust[num718];
            Dust dust212 = dust97;
            dust212.velocity *= 0.1f;
            if (Main.rand.Next(6) != 0)
            {
                Main.dust[num718].noGravity = true;
            }*/

            //Main.NewText("alpha: " + Projectile.alpha + " hits: " + Projectile.ai[2] + " Xvel: " + Projectile.velocity.X);

            //(int)(Vector2.Distance(Projectile.velocity, Vector2.Zero))+1
            if (Main.rand.Next(5) == 0)
            {
                int dustID = Dust.NewDust(Projectile.Center, 1, 1, DustID.Electric, 0f, 0f);
                Dust dust = Main.dust[dustID];
                dust.velocity *= 1f;
                dust.velocity += Projectile.velocity;
                dust.velocity *= 0.1f;
                dust.noGravity = true;
            }

            Projectile.velocity += new Vector2(Projectile.ai[0], Projectile.ai[1]);
            if(Vector2.Distance(Projectile.velocity, Vector2.Zero) >= 36f) //cap speed
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 36f;
            }
            if (Projectile.alpha > minAlpha && Projectile.ai[2] == 0)
            {
                Projectile.alpha -= (256-minAlpha)/ 12;

            }
            else if (Projectile.alpha < 255 && Projectile.ai[2] > 0)
            {
                Projectile.alpha += (256 - minAlpha) / 12;
            }
            else if (Projectile.alpha >= 255)
            {
                //Main.NewText("killed");
                Projectile.Kill();
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.alpha <= minAlpha && Projectile.ai[2] == 0 && !target.friendly;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[2] += 1;
            for (int i = 0; i <= 16; i++)
            {
                int dustID = Dust.NewDust(Projectile.Center, 1, 1, DustID.Electric, 0f, 0f);
                Dust dust = Main.dust[dustID];
                dust.velocity *= 2;
                dust.velocity += Projectile.velocity;
                dust.velocity *= -1f + (float)i * 0.2f;
                dust.velocity *= 0.4f;
                if (Main.rand.Next(3) != 0)
                {
                    dust.noGravity = true;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Color.White;
                if(Projectile.alpha>255)
                {
                    Projectile.alpha = 255;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                //(float)(Projectile.alpha / 256)
                color = new Color(255, 255, 255, 200) * ((255f - (float)Projectile.alpha) / 255f);
                Main.spriteBatch.Draw ( texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}