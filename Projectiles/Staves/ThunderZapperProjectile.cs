using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponRevamp.Projectiles.Staves;

public class ThunderZapperProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[ModContent.ProjectileType<ThunderZapperProjectile>()] = 90 * 8 + 480;
    }

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 90;
        Projectile.MaxUpdates = Projectile.timeLeft;
        Projectile.stopsDealingDamageAfterPenetrateHits = true;
        Projectile.penetrate = 3;
        Projectile.alpha = 0;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.ignoreWater = true;
    }

    Vector2 lean = Vector2.Zero;
    private List<Vector2> positions = new List<Vector2>();
    private bool isDead = false;
    private const int fadeOutTime = 24;
    private const int homingRange = 8 * 16;
    
    public override void AI()
    {
        if (!isDead)
        {
            

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 8)
            {
                for (int i=0; i<2; i++)
                {
                    Dust zappy = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Electric);
                    zappy.noGravity = true;
                    zappy.velocity *= 0.8f;
                    zappy.velocity += Projectile.velocity * 0.5f;
                }
            }
            
            if (Projectile.ai[0] % 8 == 0)
            {
                positions.Add(Projectile.Center);
                if (Projectile.ai[0] > 23)
                {
                    int targetIndex = Projectile.FindTargetWithLineOfSight(homingRange);
                    if (targetIndex >= 0 && Main.npc[targetIndex] != null)
                    {
                        NPC target = Main.npc[targetIndex];
                        Vector2 homingDirection = Vector2.Normalize(target.Center - Projectile.position);
                        /*projectile.velocity += homingDirection * 2f;
                        projectile.velocity *= 0.94f;*/
                
                        //experimental - this causes the projectile to loop widely around the enemy! fun!
                        Vector2 homingStrength = homingDirection * 8f;
                        Projectile.velocity = homingStrength;
                        //Projectile.velocity *= 0.8f;
                        if (Projectile.velocity.Length() > 16f)
                        {
                            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f;
                        }
                        lean = Vector2.Zero;
                
                    }
                }
        
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * 5 + lean;
                lean -= offset;
                Projectile.velocity += offset;
                
                //homing!
                
            }

            
        }
        else
        {
            float adjustedAlpha = (256f - (float)Projectile.alpha) / 256f;
            foreach(Vector2 lightPosition in positions)
            {
                Lighting.AddLight(lightPosition, new Vector3(0, 0.6f,0.8f) * adjustedAlpha);
            }
            
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.alpha += (256/fadeOutTime);
            if (Projectile.alpha > 224)
            {
                Projectile.Kill();
            }
        }
        
        if (Projectile.damage == 0 || Projectile.timeLeft <= 2)
        {
            if (!isDead)
            {
                Die();
            }
        }
        
        
        
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.damage = (int)(Projectile.damage * 0.7);
        
        int targetIndex = Projectile.FindTargetWithLineOfSight(homingRange);
        if (targetIndex >= 0 && Main.npc[targetIndex] != null)
        {
            NPC target1 = Main.npc[targetIndex];
            Vector2 homingDirection = Vector2.Normalize(target1.Center - Projectile.position);
            /*projectile.velocity += homingDirection * 2f;
            projectile.velocity *= 0.94f;*/
                
            //experimental - this causes the projectile to loop widely around the enemy! fun!
            Vector2 homingStrength = homingDirection * 8f;
            Projectile.velocity = homingStrength;
            //Projectile.velocity *= 0.8f;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f;
            }
            lean = Vector2.Zero;
        }
        else
        {
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Die();
        return false;
    }

    private void Die()
    {
        Projectile.MaxUpdates = 1;
        Projectile.extraUpdates = 0;
        Projectile.numUpdates = 0;
        Projectile.timeLeft = fadeOutTime;
        positions.Add(Projectile.Center);
        isDead = true;
        for (int i=0; i<6; i++)
        {
            Dust zappy = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Electric);
            zappy.noGravity = true;
            zappy.velocity *= 0.8f;
        }
    }


    public override bool PreDraw(ref Color lightColor)
    {
        if (isDead)
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                Vector2 position1 = positions[i] - Main.screenPosition;
                Vector2 position2 = positions[i+1] - Main.screenPosition;
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                Rectangle sourceRectangle = texture.Frame(1, 1);
                Vector2 origin = sourceRectangle.Size() / 2f;
                origin.X = 0;
                float scale = Projectile.scale;
                //float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
                int frameNum = 0;
            
            
                Color renderColor = new Color(255, 255, 255, 0);
                float adjustedAlpha = (256f - (float)Projectile.alpha) / 256f;
                //adjustedAlpha = 1;

                float length = Vector2.Distance(position1, position2);
                Vector2 stretch = new Vector2(length / 32, 1);
                float targetRotation = (position2 - position1).ToRotation();
            
            
                //position1 = Projectile.Center - Main.screenPosition;
                Main.EntitySpriteDraw(texture, position1, texture.Frame(1, 1, 0, 0), renderColor * adjustedAlpha, targetRotation, origin, stretch, SpriteEffects.None, 0f);

            }
            
        }
        
        /*Vector2 position = Projectile.Center - Main.screenPosition;
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle sourceRectangle = texture.Frame(1, 1);
        Vector2 origin = sourceRectangle.Size() / 2f;
        float scale = Projectile.scale;
        float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
        int frameNum = 0;
            
            
        Color renderAlpha = new Color(255, 255, 255, 128);
            
            
        position = Projectile.Center - Main.screenPosition;
        Main.EntitySpriteDraw(texture, position, texture.Frame(1, 1, 0, frameNum), renderAlpha * lightingColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
        */
            


        return false;
    }
    
}