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
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace WeaponRevamp.Projectiles.MagicGuns
{
    public class LeafBlowerProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            base.SetDefaults(entity);
            entity.timeLeft = 300;
        }
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation)
        {
            return projectile.type == ProjectileID.Leaf;
        }
        public override void AI(Projectile projectile)
        {
            projectile.velocity *= 0.9f;
            projectile.velocity.Y += 0.1f;
            projectile.velocity.X += projectile.ai[0] * 3f;
            projectile.velocity.Y += projectile.ai[1] * 3f;

            //projectile.localAI[1] += 1f;
        }


        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= projectile.velocity.Length() / 12f;
        }

        public override bool InstancePerEntity => true;
    }
}
