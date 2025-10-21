using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using WeaponRevamp.Dusts;

namespace WeaponRevamp.Projectiles.Books
{
    public class LunarFlareProjectile : GlobalProjectile
    {

        private const int radius = 6;

        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.LunarFlare;
        }

        

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.ai[1] != -1f && projectile.position.Y > projectile.ai[1])
            {
                projectile.tileCollide = true;
            }
            if (projectile.position.HasNaNs())
            {
                projectile.Kill();
                return false;
            }
            bool num245 = WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16));
            Dust dust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229)];
            dust3.position = projectile.Center;
            dust3.velocity = Vector2.Zero;
            dust3.velocity += projectile.velocity * 0.4f;
            dust3.noGravity = true;
            dust3.scale = 1f;
            if (num245)
            {
                dust3.noLight = true;
            }
            if (projectile.ai[1] == -1f)
            {
                projectile.ai[0] += 1f;
                projectile.velocity = Vector2.Zero;
                projectile.tileCollide = false;
                projectile.penetrate = -1;
                projectile.position = projectile.Center;
                projectile.width = (projectile.height = 140);
                projectile.Center = projectile.position;
                projectile.alpha -= 10;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
                if (++projectile.frameCounter >= projectile.MaxUpdates * 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
                if (projectile.ai[0] >= (float)(Main.projFrames[projectile.type] * projectile.MaxUpdates * 3))
                {
                    projectile.Kill();
                }
                return false;
            }
            projectile.alpha = 255;
            if (projectile.numUpdates == 0)
            {
                int num117 = -1;
                float num118 = 60f;
                for (int num119 = 0; num119 < 200; num119++)
                {
                    NPC nPC2 = Main.npc[num119];
                    if (nPC2.CanBeChasedBy(this))
                    {
                        float num120 = projectile.Distance(nPC2.Center);
                        if (num120 < num118 && Collision.CanHitLine(projectile.Center, 0, 0, nPC2.Center, 0, 0))
                        {
                            num118 = num120;
                            num117 = num119;
                        }
                    }
                }
                if (num117 != -1)
                {
                    projectile.ai[0] = 0f;
                    projectile.ai[1] = -1f;
                    projectile.netUpdate = true;
                    return false;
                }
            }

            projectile.tileCollide = true;
            /*Dust dust = Dust.NewDustDirect(projectile.position, 0, 0, DustID.Vortex);
            dust.velocity *= 0.1f;
            dust.noGravity = true;
            dust.scale = 0.5f;*/

            if(Main.myPlayer == projectile.owner && projectile.ai[2] == 0 && projectile.ai[1] != -1)
            {
                
                Player player = Main.player[projectile.owner];
                Vector2 target = Main.MouseWorld;
                Vector2 offset = new Vector2(0, -(radius + 2) * 16);

                player.LimitPointToPlayerReachableArea(ref target);
                projectile.velocity *= 0.95f;
                projectile.velocity += Vector2.Normalize(target - projectile.position) * 0.5f;
                if(Vector2.Distance(projectile.Center, target) < 3 * 16 || projectile.timeLeft < 3500)
                {
                    projectile.ai[2] = 1;
                }
                if (Collision.CanHit(player.position, player.width, player.height, target, 1, 1))
                {
                    projectile.tileCollide = false;
                }
                if (projectile.velocity.Length() > 16)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= 16f;
                }
            }
            
            projectile.netUpdate = true;
            return false;
        }
    }
}
