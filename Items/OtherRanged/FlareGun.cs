using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Items.Ammo;

namespace WeaponRevamp.Items.OtherRanged;

public class FlareGun : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstatiation)
    {

        return item.type == ItemID.FlareGun;

    }
    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);
        entity.damage = 3;
        entity.DamageType = DamageClass.Ranged;
    }

    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (source.AmmoItemIdUsed == ModContent.ItemType<ClusterFlare>())
        {
            for (int i = 0; i < 3; i++)
            {
                float spread = 5f;
                Vector2 adjustedSpeed = new Vector2(Main.rand.NextFloat(-spread, spread), Main.rand.NextFloat(-spread, spread));
                Projectile.NewProjectileDirect(source, position, velocity + adjustedSpeed, type, damage, knockback, player.whoAmI);
                
            }
        }
        return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
    }

    public override void HoldItem(Item item, Player player)
    {
        if (item.holdStyle == 1 && !player.pulley)
        {
            
        }
        else
        {
            return;
        }

        if (!player.CanVisuallyHoldItem(item))
        {
            return;
        }
        if (Main.dedServ)
        {
            return;
        }

        if (player.itemTime > 0)
        {
            return;
        }
        
        float x = player.position.X + (float)(player.width / 2) + (float)(38 * player.direction);
        if (player.direction == 1)
        {
            x -= 10f;
        }
        float y = player.MountedCenter.Y - 4f * player.gravDir;
        if (player.gravDir == -1f)
        {
            y -= 8f;
        }
        player.RotateRelativePoint(ref x, ref y);
        int num3 = 0;
        for (int i = 54; i < 58; i++)
        {
            if (player.inventory[i].stack > 0 && player.inventory[i].ammo == 931)
            {
                num3 = player.inventory[i].type;
                break;
            }
        }
        if (num3 == 0)
        {
            for (int j = 0; j < 54; j++)
            {
                if (player.inventory[j].stack > 0 && player.inventory[j].ammo == 931)
                {
                    num3 = player.inventory[j].type;
                    break;
                }
            }
        }
        if (num3 == ModContent.ItemType<IchorFlare>())
        {
            int num4 = Dust.NewDust(new Vector2(x, y + player.gfxOffY), 6, 6, DustID.Ichor, 0f, 0f, 100, default(Color), 1f);
            Main.dust[num4].velocity *= 0.5f;
            Main.dust[num4].noGravity = true;
            Main.dust[num4].velocity.Y -= 4f * player.gravDir;
        }
        else if (num3 == ModContent.ItemType<ClusterFlare>())
        {
            int num4 = Dust.NewDust(new Vector2(x, y + player.gfxOffY), 6, 6, DustID.Flare, 0f, 0f, 100, default(Color), 2.2f);
            Main.dust[num4].velocity *= 1.5f;
            Main.dust[num4].noGravity = true;
            Main.dust[num4].velocity.Y -= 4f * player.gravDir;
        }
        else
        {
            
        }
    }
}