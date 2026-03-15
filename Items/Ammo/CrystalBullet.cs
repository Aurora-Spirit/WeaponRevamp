using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Prefixes;
using WeaponRevamp.Projectiles.OtherRanged.Flares;

namespace WeaponRevamp.Items.Ammo
{
    public class CrystalBullet : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CrystalBullet;
        }

        public override void Load()
        {
            TextureAssets.Item[ItemID.CrystalBullet] = ModContent.Request<Texture2D>("WeaponRevamp/Items/Ammo/CrystalBullet");
        }

        public override void Unload()
        {
            TextureAssets.Item[ItemID.CrystalBullet] = ModContent.Request<Texture2D>("WeaponRevamp/Items/Ammo/CrystalBullet");
        }
    }
}
