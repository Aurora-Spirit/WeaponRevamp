using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace WeaponRevamp.Projectiles.Summons;

public class ImpSummonerPlayer : ModPlayer
{
    public int impOrbitTime;
    public const int maxImpOrbitTime = 240;
    
    public override void PostUpdate()
    {
        impOrbitTime++;
        if (impOrbitTime > maxImpOrbitTime)
        {
            impOrbitTime = 0;
        }
    }
}


public class ImpStaffSummon : GlobalProjectile
{
    //ai[2] represents whether the minion is already attacking, for teleportation purposes
    //ai[1] represents firing cooldown
    public override bool InstancePerEntity => true;
    private NPC currentTarget = null;
    private NPC newTarget = null;
    private int timeSinceNewTarget = 0;
    private Vector2 circlePosition = Vector2.Zero;
    
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.FlyingImp;
    }

    public override bool PreAI(Projectile projectile)
    {
        Player owner = Main.player[projectile.owner];

        
        if (owner.dead || !owner.active) {
            owner.ClearBuff(BuffID.ImpMinion);

            return false;
        }
        if (owner.HasBuff(BuffID.ImpMinion)) {
            projectile.timeLeft = 2;
        }

        NPC target;
        if ((target = FindTarget(projectile))!=null)
        {
            ChaseNPC(projectile, target);
        }
        else
        {
            FollowPlayer(projectile);
        }

        if (Main.rand.NextBool(3))
        {
            Dust fire = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
            fire.noGravity = true;
            fire.scale = 1.2f;
        }

        projectile.frameCounter++;
        if (projectile.frameCounter >= 16)
        {
            projectile.frameCounter = 0;
        }
        projectile.frame = projectile.frameCounter / 4;
        
        return false;
    }

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        binaryWriter.WriteVector2(circlePosition);
        base.SendExtraAI(projectile, bitWriter, binaryWriter);
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        circlePosition = binaryReader.ReadVector2();
        base.ReceiveExtraAI(projectile, bitReader, binaryReader);
    }


    [CanBeNull]
    private NPC FindTarget(Projectile projectile)
    {
        
        float distanceFromTarget = 700f;
        Vector2 targetCenter = projectile.position;
        bool foundTarget = false;
        NPC target = null;
        
        Player owner = Main.player[projectile.owner];
        if (owner.HasMinionAttackTargetNPC) {
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            float between = Vector2.Distance(npc.Center, projectile.Center);

            // Reasonable distance away so it doesn't target across multiple screens
            if (between < 2000f)
            {
                target = npc;
            }
        }

        if (target == null)
        {
            foreach (var npc in Main.ActiveNPCs) {
                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                    // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                    // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                    bool closeThroughWall = between < 25*8;

                    float distToPlayer = Vector2.Distance(npc.Center, owner.Center);
                    bool playerInRange = distToPlayer < 700f;

                    if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall) && playerInRange) {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                        target = npc;
                    }
                }
            }
        }

        if (target != newTarget)
        {
            newTarget = target;
            timeSinceNewTarget = 0;
        }
        else
        {
            if (timeSinceNewTarget < 15)
            {
                timeSinceNewTarget++;
            }
            else
            {
                currentTarget = newTarget;
            }
        }
        
        return currentTarget;
    }

    [CanBeNull]
    private NPC FindSecondTarget(Projectile projectile, NPC ignoredNPC)
    {
        float distanceFromTarget = 700f;
        Vector2 targetCenter = ignoredNPC.position;
        bool foundTarget = false;
        NPC target = null;
        foreach (var npc in Main.ActiveNPCs) {
            if (npc.CanBeChasedBy() && npc.whoAmI != ignoredNPC.whoAmI) {
                float between = Vector2.Distance(npc.Center, ignoredNPC.Center);
                bool closest = Vector2.Distance(ignoredNPC.Center, targetCenter) > between;
                bool inRange = between < distanceFromTarget;
                bool lineOfSight = Collision.CanHitLine(ignoredNPC.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                bool closeThroughWall = false;

                if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                    target = npc;
                }
            }
        }

        return target;
    }
    private void ChaseNPC(Projectile projectile, NPC mainTarget)
    {
        AI_GetMyGroupIndexAndFillBlackList(new List<int>(), out int index, out int totalIndexesInGroup, projectile);
        NPC secondTarget = FindSecondTarget(projectile, mainTarget);
        Player owner = Main.player[projectile.owner];
        if (Main.myPlayer == owner.whoAmI && Main.netMode == 1)
        {
            circlePosition = 15*8*Vector2.UnitX.RotatedBy(Math.PI*2 * ((float)owner.GetModPlayer<ImpSummonerPlayer>().impOrbitTime / ImpSummonerPlayer.maxImpOrbitTime) + Math.PI*2 * ((float)index/totalIndexesInGroup));
            projectile.netUpdate = true;
        }
        Vector2 targetPosition = mainTarget.Center;
        if (secondTarget == null)
        {
            targetPosition = circlePosition + mainTarget.Center;
        }
        else
        {
            Vector2 targetOffset = secondTarget.position - mainTarget.position;
            Vector2 modifiedCirclePosition = new Vector2(circlePosition.X * 0.5f, circlePosition.Y).RotatedBy(targetOffset.ToRotation())*(float)Math.Sqrt((totalIndexesInGroup-1))/4f;
            targetPosition = modifiedCirclePosition - Vector2.Normalize(targetOffset) * 15 * 8 + mainTarget.Center;
        }
        
        if (projectile.ai[2] != 1)
        {
            if (!Main.rand.NextBool(20))
            {
                FollowPlayer(projectile);
                projectile.netUpdate = true;
                return;
            }
            projectile.ai[2] = 1;
            projectile.ai[1] = 0;
            Teleport(projectile,targetPosition);
        }

        projectile.direction = Math.Sign(mainTarget.Center.X - projectile.Center.X);
        projectile.spriteDirection = -projectile.direction;
        projectile.ai[1]++;
        projectile.velocity = Vector2.Zero;
        projectile.Center = Vector2.SmoothStep(projectile.Center, targetPosition, 0.2f);
        if (projectile.ai[1] >= 120)
        {
            projectile.ai[1] = 0;
            Projectile shot = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(),projectile.Center, Vector2.Normalize(mainTarget.Center - projectile.Center)*10f, ProjectileID.ImpFireball, projectile.damage, projectile.knockBack, projectile.owner);
            SoundEngine.PlaySound(SoundID.Item20, projectile.Center);
        }


    }
    private void FollowPlayer(Projectile projectile)
    {
        AI_GetMyGroupIndexAndFillBlackList(new List<int>(), out int index, out int totalIndexesInGroup, projectile);
        AI_156_GetIdlePosition(index, totalIndexesInGroup, out Vector2 idleSpot, out int idleDirection, projectile);
        if (projectile.ai[2] != 0)
        {
            projectile.ai[2] = 0;
            Teleport(projectile,idleSpot);
        }
        
        projectile.velocity = Vector2.Zero;
        projectile.Center = Vector2.SmoothStep(projectile.Center, idleSpot, 0.2f);
        projectile.direction = idleDirection;
        projectile.spriteDirection = -projectile.direction;
    }

    private void Teleport(Projectile projectile, Vector2 target)
    {
        for (int i = 0; i < 12; i++)
        {
            Dust fire = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
            fire.noGravity = true;
            fire.scale = 1.7f;
            fire.velocity *= 4f;
        }
        projectile.position = target;
        projectile.netUpdate = true;
        SoundEngine.PlaySound(SoundID.Item8, projectile.Center);
        for (int i = 0; i < 12; i++)
        {
            Dust fire = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
            fire.noGravity = true;
            fire.scale = 1.7f;
            fire.velocity *= 4f;
        }
    }
    
    
    //copied straight from the source code lol
    private void AI_156_GetIdlePosition(int stackedIndex, int totalIndexes, out Vector2 idleSpot, out int idleDirection, Projectile projectile)
    {
        Player player = Main.player[projectile.owner];
        idleDirection = 0;
        idleSpot = Vector2.Zero;
        
        float num2 = ((float)totalIndexes - 1f) / 2f;
        idleSpot = player.Center + -Vector2.UnitY.RotatedBy(4.3982296f / (float)totalIndexes * ((float)stackedIndex - num2)) * 60f;
        idleDirection = player.direction;
    }
    private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup, Projectile me)
    {
        index = 0;
        totalIndexesInGroup = 0;
        for (int i = 0; i < 1000; i++)
        {
            Projectile projectile = Main.projectile[i];
            if (projectile.active && projectile.owner == me.owner && projectile.type == me.type && (projectile.type != 759 || projectile.frame == Main.projFrames[projectile.type] - 1))
            {
                if (me.whoAmI > i)
                {
                    index++;
                }
                totalIndexesInGroup++;
            }
        }
    }
}

public class ImpStaffSummonProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.ImpFireball;
    }

    public override void SetDefaults(Projectile entity)
    {
        entity.usesLocalNPCImmunity = true;
        entity.localNPCHitCooldown = 12;
        entity.penetrate = 4;
        entity.usesIDStaticNPCImmunity = false;
        entity.tileCollide = false;
        entity.timeLeft = 52;
        entity.width = 48;
        entity.height = 48;
    }

    public override bool PreAI(Projectile projectile)
    {
        projectile.localAI[0]+=1f;
        if (Main.rand.NextBool(1, 3))
        {
            if (projectile.localAI[0] < 40)
            {
                Dust test = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
                test.velocity *= 2f;
                test.velocity += projectile.velocity*0.3f;
                test.scale *= 1.3f;
            }
            else
            {
            
            }
            
        }
        return false;
    }

    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        DrawProj_Flamethrower(projectile);
        return false;
    }


    //copied from source code
    private static void DrawProj_Flamethrower(Projectile proj)
	{
		bool flag = proj.ai[0] == 1f;
		float num = 40f;
		float num10 = 12f;
		float fromMax = num + num10;
        
		Texture2D texture = TextureAssets.Projectile[ProjectileID.Flames].Value;
		_ = Color.Transparent;
		Color opaqueRed = new Color(255, 80, 20, 200);
		Color translucentYellow = new Color(255, 255, 20, 70);
		Color translucentOrange = Color.Lerp(new Color(255, 80, 20, 100), translucentYellow, 0.25f);
		Color translucentGray = new Color(80, 80, 80, 100);
		float num11 = 0.35f;
		float num12 = 0.7f;
		float num13 = 0.85f;
		float num14 = ((proj.localAI[0] > num - 10f) ? 0.175f : 0.2f);
		if (flag)
		{
			opaqueRed = new Color(95, 120, 255, 200);
			translucentYellow = new Color(50, 180, 255, 70);
			translucentOrange = new Color(95, 160, 255, 100);
			translucentGray = new Color(33, 125, 202, 100);
		}
		int verticalFrames = 7;
		float num15 = Utils.Remap(proj.localAI[0], num, fromMax, 1f, 0f);
		float num2 = Math.Min(proj.localAI[0], 20f);
		float num3 = Utils.Remap(proj.localAI[0], 0f, fromMax, 0f, 1f);
		float num4 = Utils.Remap(num3, 0.2f, 0.5f, 0.2f, 0.8f);
		Rectangle rectangle = ((!flag) ? texture.Frame(1, verticalFrames, 0, 3) : texture.Frame(1, verticalFrames, 0, (int)Utils.Remap(num3, 0.5f, 1f, 3f, 5f)));
		if (!(num3 < 1f))
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (float num5 = 1f; num5 >= 0f; num5 -= num14)
			{
				Color obj = ((num3 < 0.1f) ? Color.Lerp(Color.Transparent, opaqueRed, Utils.GetLerpValue(0f, 0.1f, num3, clamped: true)) : ((num3 < 0.2f) ? Color.Lerp(opaqueRed, translucentYellow, Utils.GetLerpValue(0.1f, 0.2f, num3, clamped: true)) : ((num3 < num11) ? translucentYellow : ((num3 < num12) ? Color.Lerp(translucentYellow, translucentOrange, Utils.GetLerpValue(num11, num12, num3, clamped: true)) : ((num3 < num13) ? Color.Lerp(translucentOrange, translucentGray, Utils.GetLerpValue(num12, num13, num3, clamped: true)) : ((!(num3 < 1f)) ? Color.Transparent : Color.Lerp(translucentGray, Color.Transparent, Utils.GetLerpValue(num13, 1f, num3, clamped: true))))))));
				float num6 = (1f - num5) * Utils.Remap(num3, 0f, 0.2f, 0f, 1f);
				Vector2 vector = proj.Center - Main.screenPosition + proj.velocity * (0f - num2) * num5;
				Color color5 = obj * num6;
				Color color6 = color5;
				if (!flag)
				{
					color6.G /= 2;
					color6.B /= 2;
					color6.A = (byte)Math.Min((float)(int)color5.A + 80f * num6, 255f);
					Utils.Remap(proj.localAI[0], 20f, fromMax, 0f, 1f);
				}
				float num7 = 1f / num14 * (num5 + 1f);
				float rotationOffset = proj.rotation + num5 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num7 * 2f;
				float num9 = proj.rotation - num5 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num7 * 2f;
				switch (i)
				{
				case 0:
                    Main.EntitySpriteDraw(texture, vector + proj.velocity * (0f - num2) * num14 * 0.5f, rectangle, color6 * num15 * 0.25f, rotationOffset + (float)Math.PI / 4f, rectangle.Size() / 2f, num4, SpriteEffects.None);
                    Main.EntitySpriteDraw(texture, vector, rectangle, color6 * num15, num9, rectangle.Size() / 2f, num4, SpriteEffects.None);
					break;
				case 1:
					if (!flag)
					{
                        Main.EntitySpriteDraw(texture, vector + proj.velocity * (0f - num2) * num14 * 0.2f, rectangle, color5 * num15 * 0.25f, rotationOffset + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, SpriteEffects.None);
                        Main.EntitySpriteDraw(texture, vector, rectangle, color5 * num15, num9 + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, SpriteEffects.None);
					}
					break;
				}
			}
		}
	}
}