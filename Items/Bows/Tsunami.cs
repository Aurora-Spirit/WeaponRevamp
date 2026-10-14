using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Bows;

namespace WeaponRevamp.Items.Bows;

public class Tsunami : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.Tsunami;
    }

    public override void SetDefaults(Item item)
    {
        item.useTime = 4;
        item.useAnimation = 4;
        item.damage = 100;
        

    }

    public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
    {
        if (Main.rand.NextBool(4, 5))
        {
            return false;
        }
        return base.CanConsumeAmmo(weapon,ammo, player);
    }

    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        
        TsunamiPlayer pressurePlayer = player.GetModPlayer<TsunamiPlayer>();

        float useTimeMult = 1;
        float threshold = 0.6f;
        if (pressurePlayer.pressure > TsunamiPlayer.maxPressure*threshold)
        {
        }
        else
        {
            useTimeMult = (float)(TsunamiPlayer.maxPressure*threshold-pressurePlayer.pressure)/(TsunamiPlayer.maxPressure*threshold)*3 + 1;
            velocity *= pressurePlayer.pressure*1f / (TsunamiPlayer.maxPressure*threshold) + 0f;
        }
        UnifiedArrowProjectile.NewUnifiedArrow(source, position, velocity, ModContent.ProjectileType<TsunamiProjectile>(), damage, knockback, type, player.whoAmI);
        //Main.NewText(pressurePlayer.pressure);
        
        player.itemTimeMax = (int)(player.itemTimeMax * useTimeMult);
        player.itemTime = (int)(player.itemTime * useTimeMult);
        player.itemAnimationMax = (int)(player.itemAnimationMax * useTimeMult);
        player.itemAnimation = (int)(player.itemAnimation * useTimeMult);
        
        pressurePlayer.pressure -= player.itemTime*3/2;
        if (pressurePlayer.pressure < 0)
        {
            pressurePlayer.pressure = 0;
        }

        return false;
    }
}

public class TsunamiPlayer : ModPlayer
{
    public int pressure = 0;
    public static int maxPressure = 60;

    public override void PreUpdate()
    {
        if (pressure == maxPressure - 1 && Player.HeldItem.type == ItemID.Tsunami)
        {
            SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
            for(int i=0; i<10; i++)
            {
                Dust indicator = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.DungeonWater);
                indicator.noGravity = true;
                indicator.scale *= 1.3f;
            }
        }
        pressure += 1;
        if (pressure > maxPressure)
        {
            pressure = maxPressure;
        }
    }
}