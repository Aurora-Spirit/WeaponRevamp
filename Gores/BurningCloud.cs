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
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace WeaponRevamp.Gores
{
    public class BurningCloud : ModGore
    {

        public override bool Update(Gore gore)
        {
            Lighting.AddLight(gore.position, new Vector3(0.9f,0.3f,0));
            gore.timeLeft -= 2;
            gore.position += gore.velocity;
            gore.velocity *= 0.95f;
            gore.rotation += ((gore.timeLeft%2)-0.5f) * 0.05f / gore.scale;
            gore.sticky = true;
            if(gore.timeLeft < 60)
            {
                gore.scale -= 0.03f;
                gore.alpha += 6;
            }
            if(Collision.SolidCollision(gore.position + new Vector2(16, 16 + gore.velocity.Y) , 1, 1))
            {
                gore.velocity.Y *= -0.2f;
                gore.velocity.X *= 1.1f;
            }
            if (Collision.SolidCollision(gore.position + new Vector2(16 + gore.velocity.X, 16), 1, 1))
            {
                gore.velocity.X *= -0.2f;
                gore.velocity.Y *= 1.1f;
            }

            if (gore.timeLeft <= 0)
            {
                gore.active = false;
            }
            return false;
        }

    }
    public class BurningCloud1 : BurningCloud
    {

    }
    public class BurningCloud2 : BurningCloud
    {

    }
    public class BurningCloud3 : BurningCloud
    {

    }

}
