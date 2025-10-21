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

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisVenomArrowProjectile : ModProjectile
    {

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
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        // Custom AI
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Lighting.AddLight(Projectile.position, new Vector3(Main.DiscoR / 256.0f, Main.DiscoG / 256.0f, Main.DiscoB / 256.0f));
            
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 35)
            {
                Projectile.velocity.Y += 0.07f;
            }
            NPC target = Projectile.FindTargetWithinRange(90);
            if (target != null) { Projectile.velocity += Vector2.Normalize(target.Center - Projectile.Center) * 4f; Projectile.velocity *= 0.99f; }
            if (Projectile.velocity.Length() > 16f) Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f;
            if (((int)Projectile.ai[0]) % 1 == 0)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<TintableOpaqueDustLighted>(), 0, 0, 0, Color.Lerp(new Color(64, 0, 192), new Color(0, 192, 0), Main.rand.NextFloat()), 1.3f + Main.rand.NextFloat() * 0.5f);
                dust13.noGravity = true;
                dust13.velocity *= 0.3f;
                dust13.velocity += Projectile.velocity * 0.5f;
                //dust13.fadeIn = dust13.scale + 0.05f;
                Dust dust14 = Dust.CloneDust(dust13);
                //dust14.type = DustID.TintableDust;
                dust14.color = Color.Lerp(dust14.color, Color.Black, 0.7f);
                dust14.scale -= 0.3f;
            }
            if (Projectile.shimmerWet) Projectile.velocity.Y -= 0.4f;
            
            
        }

        public override void OnKill(int timeLeft)
        {
            for(int i=0;i<20f;i++)
            {
                Dust dust13 = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<TintableOpaqueDustLighted>(), 0, 0, 0, Color.Lerp(new Color(64, 0, 192), new Color(0, 192, 0), Main.rand.NextFloat()), 1.3f + Main.rand.NextFloat() * 0.5f);
                dust13.noGravity = true;
                dust13.velocity *= 1.5f;
                dust13.velocity += Projectile.oldVelocity * 0.2f;
                //dust13.fadeIn = dust13.scale + 0.05f;
                Dust dust14 = Dust.CloneDust(dust13);
                //dust14.type = DustID.TintableDust;
                dust14.color = Color.Lerp(dust14.color, Color.Black, 0.7f);
                dust14.scale -= 0.3f;
            }

            base.OnKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<DeathToxin>(), 300);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            target.AddBuff(ModContent.BuffType<DeathToxin>(), 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale;
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            //position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, 0), new Color(255, 255, 255, 255) * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            


            return false;

        }

        /*public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width /= 3;
            height /= 3;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }*/


    }
}
