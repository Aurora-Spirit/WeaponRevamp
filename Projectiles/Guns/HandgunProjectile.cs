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
    public class HandgunProjectile : UnifiedBulletProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void SetAmmoDefaults()
        {
            base.SetAmmoDefaults();
            if (Projectile.penetrate > 0)
            {
                Projectile.maxPenetrate += 1;
                Projectile.penetrate = Projectile.maxPenetrate;
            }
        }
    }
}

