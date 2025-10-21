using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Terraria.GameContent;
using System.IO;
using Mono.Cecil;

namespace WeaponRevamp.Projectiles.Books
{
    public class LunarFlareMoonProjectile : ModProjectile
    {
        private const int radius = 6;

        private int textureWidth;
        private Color[] colors;
        private bool loadedTexture = false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = radius * 2 * 16;
            Projectile.height = 12 * 16;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.timeLeft = 36000; //max lifetime is 10 minutes
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;

        }

        
        




        /*private float ManaConsumptionRate
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }*/



        public override void AI()
        {
            if(!loadedTexture)
            {
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                textureWidth = texture.Width;
                texture.GetData(colors = new Color[texture.Width * texture.Height]);
                loadedTexture = true;
            }

            Player owner = Main.player[Projectile.owner];
            Projectile.Center = owner.Center + new Vector2(0, -(radius+2) * 16) + new Vector2(-2, -2);

            Projectile.netUpdate = true;
            //if its a mana usage frame, drop from this or gate without actually running CheckMana
            bool manaIsAvailable = Projectile.timeLeft%10!=0 || owner.CheckMana(owner.HeldItem.mana, true, false);
            if (!owner.channel || !manaIsAvailable)
            {
                Projectile.Kill();
            }
            
            owner.heldProj = Projectile.whoAmI;
            owner.itemAnimation = owner.itemAnimationMax;
            owner.itemTime = owner.itemTimeMax;

            if(Main.myPlayer == Projectile.owner)
            {
                owner.ChangeDir(Math.Sign(Main.MouseWorld.X-owner.position.X));
            }
            

            Projectile.damage = (int)owner.GetDamage(DamageClass.Magic).ApplyTo(owner.HeldItem.damage);


            int chargeStrength = 36000 - Projectile.timeLeft;
            chargeStrength *= 2;
            if (chargeStrength > 180)
            {
                chargeStrength = 180;
                
            }
            if (chargeStrength > 120)
            {
                if (Projectile.timeLeft % 10 == 0 && Projectile.timeLeft != 0)
                {
                    SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
                    /*owner.statMana -= owner.GetManaCost(owner.HeldItem);
                    if(owner.statMana <= 0)
                    {
                        owner.statMana = 0;
                    }*/

                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 projSpawnPoint = Projectile.Center + new Vector2(radius * 16, 0).RotatedByRandom(2 * MathHelper.Pi) * Projectile.scale;
                        Projectile firedProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), projSpawnPoint, Vector2.Normalize(projSpawnPoint - Projectile.Center) * 10f + owner.velocity, ProjectileID.LunarFlare, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Dust dust3 = Main.dust[Dust.NewDust(new Vector2(firedProj.position.X, firedProj.position.Y), firedProj.width, firedProj.height, 229)];
                        dust3.position = firedProj.Center;
                        dust3.velocity = Vector2.Zero;
                        dust3.velocity += firedProj.velocity * 0.3f;
                        dust3.noGravity = true;
                        Dust dust2 = Main.dust[Dust.NewDust(new Vector2(firedProj.position.X, firedProj.position.Y), firedProj.width, firedProj.height, 229)];
                        dust2.position = firedProj.Center;
                        dust2.velocity = Vector2.Zero;
                        dust2.velocity += firedProj.velocity * 0.2f;
                        dust2.noGravity = true;
                        Dust dust1 = Main.dust[Dust.NewDust(new Vector2(firedProj.position.X, firedProj.position.Y), firedProj.width, firedProj.height, 229)];
                        dust1.position = firedProj.Center;
                        dust1.velocity = Vector2.Zero;
                        dust1.velocity += firedProj.velocity * 0.1f;
                        dust1.noGravity = true;
                    }


                }
            }
            
            if (Projectile.timeLeft % 10 == 0 && Projectile.timeLeft != 0)
            {
                SoundStyle soundStyle = SoundID.Item20;
                soundStyle.Pitch = (float)chargeStrength/90f - 1;
                SoundEngine.PlaySound(soundStyle, Projectile.Center);
            }
            
            if(Main.moonType!= 11)
            {
                for (int i = 0; i < chargeStrength / 10f + 1; i++)
                {
                    Vector2 dustAngle = new Vector2(radius * 16, 0).RotatedByRandom(2 * MathHelper.Pi);
                    int dustID1 = Dust.NewDust(Projectile.Center + dustAngle * Projectile.scale, 0, 0, DustID.Vortex);
                    Dust dust1 = Main.dust[dustID1];
                    dust1.velocity *= 0.1f;
                    dust1.noGravity = true;
                    dust1.scale *= 1.2f;
                    dust1.position += new Vector2(-2, -2); //dust is fat so we gotta move it
                    dust1.velocity += owner.velocity;
                    dust1.velocity += /*chargeStrength*/180 * dustAngle.RotatedBy(0.5f * MathHelper.Pi) * 0.00005f;
                }
            }
            
            /*int dustID2 = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat() * 81), 0, 0, DustID.Vortex);
            Dust dust2 = Main.dust[dustID2];
            dust2.velocity *= 0.1f;
            dust2.noGravity = true;
            dust2.position += new Vector2(-2, -2);*/

            
            int index = Main.moonType;
            if (Main.snowMoon) index = 9;
            if (Main.pumpkinMoon) index = 10;
            index *= textureWidth * textureWidth;
            for (int x = 0; x < textureWidth; x++)
            {
                for (int y = 0; y < textureWidth; y++)
                {
                    //Main.NewText(colors[index] + ": " + colors[index].R);
                    if (Main.rand.NextDouble()*256*50 < colors[index].R && Main.rand.NextDouble()*180 < chargeStrength)
                    {
                        int dustID3 = Dust.NewDust(Projectile.position + new Vector2(y*2, x*2) * Projectile.scale, 0, 0, DustID.Vortex);
                        Dust dust3 = Main.dust[dustID3];
                        dust3.velocity *= 0.02f;
                        dust3.noGravity = true;
                        dust3.position += new Vector2(-2, -2);
                        dust3.velocity += owner.velocity;
                        dust3.scale *= 0.8f;
                    }
                    index++;
                }
            }



        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength = radius * 16f * Projectile.scale;

            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, 0, MathHelper.TwoPi))
            {
                return true;
            }

            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }


        public override void OnKill(int timeLeft)
        {
            int chargeStrength = 36000 - timeLeft;
            if (chargeStrength > 180)
            {
                chargeStrength = 180;
            }
            for (int i = 0; i < chargeStrength; i++)
            {
                int dustID1 = Dust.NewDust(Projectile.Center + new Vector2(radius * 16, 0).RotatedByRandom(2 * MathHelper.Pi) * Projectile.scale, 0, 0, DustID.Vortex);
                Dust dust1 = Main.dust[dustID1];
                dust1.velocity *= 3f;
                dust1.noGravity = true;
                dust1.scale *= 1.5f;
                dust1.position += new Vector2(-2, -2); //dust is fat so we gotta move it
            }
        }
    }
}
