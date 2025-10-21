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

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class LeafBlowerWindProjectile : ModProjectile
    {
        private const int blowDistance = 6;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 7;
            Projectile.width = 64;
            Projectile.height = Projectile.width;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }
        /*public override void OnSpawn(IEntitySource source)
        {
            Main.NewText(Projectile.velocity.X + ", " + Projectile.velocity.Y);
            base.OnSpawn(source);
        }*/
        public override void AI()
        {
            bool dustSpawned = false;
            for(int i=0;i<Main.projectile.Length;i++)
            {
                if (Main.projectile[i] != null && Main.projectile[i].type != Projectile.type && Main.projectile[i].tileCollide && !Main.projectile[i].ignoreWater)
                {
                    Vector2 heldPosition = Projectile.position;
                    int heldWidth = Projectile.width;
                    bool projBlown = false;
                    for(int j=0; j<blowDistance;j++)
                    {
                        if(dustSpawned && projBlown)
                        {
                            break;
                        }
                        Projectile.position += Projectile.velocity * Projectile.width * 0.8f;
                        if (Projectile.Colliding( Projectile.getRect(), Main.projectile[i].getRect() ) )
                        {
                            if (Main.projectile[i].hostile)
                            {
                                Main.projectile[i].velocity += Projectile.velocity * 0.2f;
                            } else
                            {
                                if (Main.projectile[i].type == ProjectileID.Leaf)
                                {
                                    Main.projectile[i].velocity += Projectile.velocity * 1.2f;
                                    Main.projectile[i].velocity.X += Main.projectile[i].ai[0] * 0f;
                                    Main.projectile[i].velocity.Y += Main.projectile[i].ai[1] * 0f;
                                    Main.projectile[i].velocity.Y += -0.1f;
                                } else
                                {
                                    Main.projectile[i].velocity += Projectile.velocity * 0.3f;
                                }
                            }
                            projBlown = true;
                        }
                        if(!dustSpawned)
                        {
                            Dust windDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);
                            windDust.velocity *= 0.5f;
                            windDust.velocity += Projectile.velocity * (Main.rand.NextFloat() * 5f + 3);
                            windDust.scale *= (Main.rand.NextFloat() * 0.5f + 0.5f);
                        }
                        Projectile.position -= new Vector2(8,8);
                        Projectile.width = (int)(Projectile.width + 16);
                        Projectile.height = Projectile.width;
                        //Projectile.position += new Vector2(Projectile.width / 2, Projectile.height / 2);
                    }
                    dustSpawned = true;
                    Projectile.position = heldPosition;
                    Projectile.width = heldWidth;
                    Projectile.height = Projectile.width;
                }
            }
            base.AI();
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }

    }
}
