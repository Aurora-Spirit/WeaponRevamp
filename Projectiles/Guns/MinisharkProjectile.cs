using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using System.CodeDom;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using WeaponRevamp.Dusts;
using WeaponRevamp.Projectiles.Guns.UnifiedBullet;

namespace WeaponRevamp.Projectiles.Guns
{
    public class MinisharkProjectile : UnifiedBulletProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AddBehaviors()
        {
            base.AddBehaviors();
            behaviors.Add(new UnderwaterHoming());
        }

        public override void SetAmmoDefaults()
        {
            base.SetAmmoDefaults();
        }
    }
    
    

    public class UnderwaterHoming : BulletBehavior
    {
        public override void AI(ref Projectile projectile)
        {
            if (projectile.penetrate != projectile.maxPenetrate)
            {
                projectile.netUpdate = true;
            }
            if (projectile.wet)
            {
                int targetIndex = projectile.FindTargetWithLineOfSight(800);
                if (targetIndex >= 0 && Main.npc[targetIndex] != null)
                {
                    NPC target = Main.npc[targetIndex];
                    Vector2 homingDirection = Vector2.Normalize(target.Center - projectile.position);
                    /*projectile.velocity += homingDirection * 2f;
                    projectile.velocity *= 0.94f;*/
                
                    //experimental - this causes the projectile to loop widely around the enemy! fun!
                    Vector2 homingStrength = homingDirection * 3f;
                    projectile.velocity += homingStrength;
                    projectile.velocity *= 0.8f;
                    if (projectile.velocity.Length() > 16f)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * 16f;
                    }
                
                }
                Dust dust = Dust.NewDustDirect(projectile.position, 0, 0,DustID.DungeonWater);
                dust.noGravity = true;
                dust.velocity *= 1f;
                dust.velocity += projectile.velocity * 0.3f;
                dust.scale = 1f;
                
                projectile.position += projectile.velocity * 0.5f;
                projectile.netUpdate = true;
            }
        }
    }
}

