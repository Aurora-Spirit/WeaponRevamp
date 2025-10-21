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
using ReLogic.Content;
using System.Xml.Schema;

namespace WeaponRevamp.Projectiles.Swords
{
    public class CobaltSwordSouthProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 105;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
        }


        public override void Load() //this method AND the following one together allow this projectile to be reflected by biome mimics and the like.
        {
            On_Projectile.CanBeReflected += CanBeReflected2;
        }
        private bool CanBeReflected2(On_Projectile.orig_CanBeReflected orig, Projectile self)
        {
            bool validReflectable = self.active && self.friendly && !self.hostile && self.damage > 0;
            return orig(self) || (self.type == Projectile.type && validReflectable);
        }

        public override void OnSpawn(IEntitySource source)
        {
            //Main.NewText("south");
            base.OnSpawn(source);
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].position != Projectile.position)
                {
                    Projectile.velocity += (Projectile.position - Main.projectile[i].position) / (float)Math.Pow(Vector2.Distance(Projectile.position, Main.projectile[i].position)+1, 1.5) * 5;
                    if(Main.rand.NextBool(50, (int)Vector2.Distance(Projectile.position, Main.projectile[i].position)+100))
                    {
                        Dust pushDust = Dust.NewDustDirect(Vector2.Lerp(Projectile.Center, Main.projectile[i].Center, Main.rand.NextFloat()), 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
                        pushDust.velocity *= 0.1f;
                    }
                } 
                else if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.Swords.CobaltSwordNorthProjectile>() && Main.projectile[i].position != Projectile.position)
                {
                    Projectile.velocity += (Projectile.position - Main.projectile[i].position) / (float)Math.Pow(Vector2.Distance(Projectile.position, Main.projectile[i].position)+1, 1.5) * -5;
                    if (Main.rand.NextBool(50, (int)Vector2.Distance(Projectile.position, Main.projectile[i].position) + 100))
                    {
                        Dust pushDust = Dust.NewDustDirect(Vector2.Lerp(Projectile.Center, Main.projectile[i].Center, Main.rand.NextFloat()), 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
                        pushDust.velocity *= 0.1f;
                    }
                }
            }
            Dust passiveDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, 0, 0, 0, new Color(0f, 0f, 1f));
            passiveDust.velocity *= 0.1f;
            passiveDust.velocity += (passiveDust.position - Projectile.Center).RotatedBy(Math.PI / 2) * 0.05f;
            Lighting.AddLight(Projectile.position, new Vector3(0, 0, 1));
            Projectile.rotation += 0.3f;
            if (Projectile.shimmerWet)
            {
                Projectile.velocity.Y = -Math.Abs(Projectile.velocity.Y);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.netUpdate = true;
            if (Projectile.velocity.X != Projectile.oldVelocity.X * 0.95f)
            {
                Projectile.velocity.X = Projectile.oldVelocity.X * -1f;
                //Projectile.position.X += Projectile.velocity.X;
            }
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y * 0.95f)
            {
                Projectile.velocity.Y = Projectile.oldVelocity.Y * -1f;
                //Projectile.position.Y += Projectile.velocity.Y;
            }
            return false;
        }

        


        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            int frameNum = 0;
            
            
            Color renderAlpha = new Color(255, 255, 255, 128);
            
            
            position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            


            return false;
        }
    }
}
