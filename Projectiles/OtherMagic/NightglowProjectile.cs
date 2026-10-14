using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.OtherMagic;

public class NightglowProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public bool targetAcquired = false;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.FairyQueenMagicItemShot;
    }

    public override void SetDefaults(Projectile projectile)
    {
        projectile.scale = 1.3f;
        projectile.localNPCHitCooldown = -1;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.GetGlobalNPC<NightglowProjectileTarget>().nightglowTargeted = false;
        projectile.ai[0] = projectile.FindTargetWithLineOfSight();
    }

    public override void PostAI(Projectile projectile)
    {
        try
        {
            if (!targetAcquired && projectile.ai[0] != -1)
            {
                int newTarget = FindNewTargetWithLineOfSight(projectile);
                if (newTarget != -1)
                {
                    projectile.ai[0] = newTarget;
                }

                targetAcquired = true;
            }

        }
        catch (Exception e)
        {
            //Main.NewText(e);
            targetAcquired = true;
        }
    }


    public int FindNewTargetWithLineOfSight(Projectile projectile, float maxRange = 800f)
    {
        float num = maxRange;
        int result = -1;
        for (int i = 0; i < 200; i++) {
            NPC nPC = Main.npc[i];
            bool flag = nPC.CanBeChasedBy(this);
            if (projectile.localNPCImmunity[i] != 0)
                flag = false;
            if (flag && nPC.GetGlobalNPC<NightglowProjectileTarget>().nightglowTargeted)
                flag = false;

            if (flag) {
                float num2 = projectile.Distance(Main.npc[i].Center);
                if (num2 < num && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height)) {
                    num = num2;
                    result = i;
                }
            }
        }
        
        //claim our target
        if (result != -1)
        {
            Main.npc[result].GetGlobalNPC<NightglowProjectileTarget>().nightglowTargeted = true;
        }

        return result;
    }
}

public class NightglowProjectileTarget : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public bool nightglowTargeted = false;

}