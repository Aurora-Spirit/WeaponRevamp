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
using System.IO;

namespace WeaponRevamp.Projectiles.Bows.Genesis
{
    // This Example show how to implement simple homing projectile
    // Can be tested with ExampleCustomAmmoGun
    public class GenesisGoldBowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 32; // The width of projectile hitbox
            Projectile.height = 32; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Ranged; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            //Projectile.extraUpdates = 1;
        }


        public override void OnSpawn(IEntitySource source)
        {

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((double)(Projectile.rotation));
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //float temp = ;
            Projectile.rotation = (float)(reader.ReadDouble());

            //Main.NewText(temp);
            base.ReceiveExtraAI(reader);
        }
        // Custom AI
        public override void AI()
        {
            Projectile.netUpdate = true;
            Lighting.AddLight(Projectile.position, new Vector3(1f, 1f, 0.2f));
            Projectile.velocity *= 0.96f;
            if(Projectile.shimmerWet)
            {
                Projectile.velocity.Y = -Math.Abs(Projectile.velocity.Y);
            }
            if (Projectile.ai[0]==0)
            {
                SoundEngine.PlaySound(SoundID.Item67, Projectile.position);
            }
            Projectile.ai[0] += 1;
            Vector2 aimVelocity = Projectile.velocity;
            if (Main.myPlayer == Projectile.owner)
            {
                aimVelocity = Vector2.Normalize(Main.MouseWorld - Projectile.position) * 10;
                Projectile.rotation = aimVelocity.ToRotation();

            }
            if(Main.rand.NextBool(6))
            {
                Dust light = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                light.scale = 1f;
                light.velocity *= 0.3f;
            }
            if (Projectile.ai[0] % 60 == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile arrow = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.Center.Y) /*- new Vector2(-5, -5)*/, aimVelocity, ModContent.ProjectileType<GenesisGoldArrowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
                for (int i = 0; i < 5; i++) {
                    Dust light = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                    light.scale = 1f;
                    light.velocity *= 0.5f;
                    light.velocity += aimVelocity * 0.3f;
                }
            }
            
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.netUpdate = true;
            if (Projectile.velocity.X != Projectile.oldVelocity.X * 0.96f)
            {
                Projectile.velocity.X = Projectile.oldVelocity.X * -1f;
                //Projectile.position.X += Projectile.velocity.X;
            }
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y * 0.96f)
            {
                Projectile.velocity.Y = Projectile.oldVelocity.Y * -1f;
                //Projectile.position.Y += Projectile.velocity.Y;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust light = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, 0, 0, 0, new Color(1f, 1f, 0.2f, 0.5f));
                light.scale = 1f;
                light.velocity *= 1f;
                light.velocity += Projectile.velocity;
            }
            base.OnKill(timeLeft);
        }
        

    }
}
