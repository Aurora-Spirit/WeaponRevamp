using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using Terraria.ModLoader.IO;
using System.IO;

namespace WeaponRevamp.Projectiles.Boomerangs
{
    public class BoomerangBaseAI : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int flightTime = 0; //how long have I been alive?
        public int maxRange = 30; //lifetime before turning around
        public float turnRange = -1; //when did i turn around?
        public float turnAngle = 0; //save on some math, but this will be changed in OnHitWhatever so gotta update it across clients
        public int pierce = 1; //how many enemies can I hit before I have to turn around?
        public const int sharpTurnTime = 6; //how long to spend turning upwards right after hitting an enemy

        //i need to turn for pi * turnRange / 6 ticks, and I must turn 90 degrees, so divide 90 by this


        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return false;
        }

        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(turnRange);
            binaryWriter.Write(turnAngle);
            binaryWriter.Write(pierce);
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            turnRange = binaryReader.ReadSingle();
            turnAngle = binaryReader.ReadSingle();
            pierce = binaryReader.ReadInt32();
        }

        //ai[0] shall represent ... NOTHING!!!! I need too many numbers, i'll just make my own
        //sorry modders. your fault for trying to make a vanilla boomerang ai projectile. i have to override your code.
        public override bool PreAI(Projectile projectile)
        {
            projectile.ai[0] = 1f;
            //sounds!!!
            if (projectile.soundDelay == 0 && projectile.type != ProjectileID.Anchor)
            {
                projectile.soundDelay = 8;
                SoundEngine.PlaySound(in SoundID.Item7, projectile.position);
            }
            //increment flightTime
            flightTime++;
            //Main.NewText("Flight time: "+flightTime+", Turn range: "+turnRange);
            //cap flight range
            if(flightTime > maxRange && pierce != 0)
            {
                turnRange = flightTime;
                turnAngle = (float.Pi / 2f) / (float.Pi * turnRange / 6f) * -Math.Sign(projectile.velocity.X); //i know this can be reduced but readability
                pierce = 0;
            }
            projectile.rotation += 0.4f * projectile.direction;
            
            if(pierce != 0)
            {
                //flying outwards
                //Main.NewText("Flying Outwards");
            }
            else if(flightTime < turnRange + sharpTurnTime)
            {
                //just hit an enemy! turn sharply upwards!
                //Main.NewText("Turning Sharply");
                float sharpTurnAngle = Math.Sign(turnAngle) * float.Pi / (2f * sharpTurnTime);
                projectile.velocity = projectile.velocity.RotatedBy(sharpTurnAngle);
            }
            else if(flightTime < turnRange + (float.Pi * turnRange / 6f))
            {
                //Main.NewText("Turning Gradually");
                //finished turning sharply upwards! turn less sharply towards the player!
                projectile.velocity = projectile.velocity.RotatedBy(turnAngle);
            }
            else
            {
                //home in on the player
                //Main.NewText("Homing on Player");
                projectile.tileCollide = false;
                projectile.velocity *= 13f/16f;
                projectile.velocity += Vector2.Normalize(Main.player[projectile.owner].Center - projectile.Center) * 3f;

                //within a tile of the player? die.
                if(projectile.position.Distance(Main.player[projectile.owner].Center) < 24f)
                {
                    //Main.NewText("Age: " + flightTime);
                    projectile.Kill();
                }
            }

            //shimmer interaction, which vanilla is MISSING for some reason
            if(projectile.shimmerWet)
            {
                if (projectile.velocity.Y > 0) projectile.velocity.Y *= -1;
            }


            return false;
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, projectile.position);


            if (pierce != 0)
            {
                turnRange = flightTime;
                turnAngle = (float.Pi / 2f) / (float.Pi * turnRange / 6f) * -Math.Sign(oldVelocity.X); //i know this can be reduced but readability
                pierce = 0;
            }
            //when we hit a tile, reverse the circling direction. for funsies.
            turnAngle *= -1;

            //Main.NewText("Collided! Turn angle: " + turnAngle);


            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }

            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }

            return false;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (pierce != 0)
            {
                pierce -= 1;
                if(pierce == 0)
                {
                    turnRange = flightTime;
                    turnAngle = (float.Pi / 2f) / (float.Pi * turnRange / 6f) * -Math.Sign(projectile.velocity.X); //i know this can be reduced but readability
                }
                target.immune[projectile.owner] = 20;
            }
        }

        /*public override void SetDefaults(Projectile projectile)
        {
            base.SetDefaults(projectile);
        }*/




    }
}