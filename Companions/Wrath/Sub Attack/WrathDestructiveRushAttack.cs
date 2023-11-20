using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Wrath
{
    internal class WrathDestructiveRushAttack : SubAttackBase
    {
        public override string Name => "Destructive Rush";
        public override string Description => "Rushes in a direction on front of the character, hitting any foe in the way.";
        public override bool AllowItemUsage => false;
        public override float Cooldown => 32;
        public override SubAttackData GetSubAttackData => new WrathDestructiveRushAttackData();

        public override void OnBeginUse(Companion User, SubAttackData RawData)
        {
            WrathDestructiveRushAttackData Data = (WrathDestructiveRushAttackData)RawData;
            Data.Damage = GetDamage(User); 
            for (int i = 0; i < 200; i++)
                Data.HitNpc[i] = false;           
        }

        public override void Update(Companion User, SubAttackData RawData)
        {
            WrathDestructiveRushAttackData Data = (WrathDestructiveRushAttackData)RawData;
            User.MoveLeft = User.MoveRight = User.ControlJump = User.MoveDown = false;
            const float MoveDistance = 192f; //48f
            if (Data.GetTime == 10)
            {
                bool MovingLeft = User.direction < 0;
                float MaxMoveX = User.GetAimedPosition.X;
                if (!User.IsBeingControlledBy(MainMod.GetLocalPlayer) && User.Target != null)
                {
                    if (User.Target != null)
                        MaxMoveX = User.Target.Center.X;
                    MovingLeft = User.Center.X > User.Target.Center.X;
                }
                //DO the destructive rush of death.
                Vector2 EndPosition = User.position;
                if (MovingLeft)
                    MaxMoveX -= MoveDistance;
                else
                    MaxMoveX += MoveDistance;
                while (true)
                {
                    if (!CanRushThrough(ref EndPosition, MovingLeft))
                        break;
                    if ((MovingLeft && EndPosition.X < MaxMoveX) || (!MovingLeft && EndPosition.X > MaxMoveX))
                    {
                        break;
                    }
                    for(int i = 0; i < 25; i++)
                        Dust.NewDust(EndPosition, 32, 48, Terraria.ID.DustID.SomethingRed, 1f - ((float)Main.rand.NextDouble() * 2), 1f - ((float)Main.rand.NextDouble() * 2));
                }
                Data.ChargeDashDestination = EndPosition;
            }
            else if (Data.GetTime == 55)
            {
                bool MovingLeft = User.direction < 0;
                float MaxMoveX = Data.ChargeDashDestination.X + 20 * User.direction;
                Vector2 EndPosition = User.position;
                while (true)
                {
                    if (!CanRushThrough(ref EndPosition, MovingLeft))
                        break;
                    if ((MovingLeft && EndPosition.X < MaxMoveX) || (!MovingLeft && EndPosition.X > MaxMoveX))
                    {
                        break;
                    }
                    for(int i = 0; i < 25; i++)
                        Dust.NewDust(EndPosition, 32, 48, Terraria.ID.DustID.SomethingRed, 1f - ((float)Main.rand.NextDouble() * 2), 1f - ((float)Main.rand.NextDouble() * 2));
                    Rectangle rect = new Rectangle((int)(EndPosition.X + 16 - User.width * 0.5f), (int)(EndPosition.Y + 48 - User.height), User.width, User.height);
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead && Main.player[i] != User && User.IsHostileTo(Main.player[i]) && rect.Intersects(Main.player[i].Hitbox))
                        {
                            Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " couldn't endure the Destructive Rush."), Data.Damage, User.direction);
                        }
                        if (i < 200 && !Data.HitNpc[i] && Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Hitbox.Intersects(rect))
                        {
                            Main.npc[i].StrikeNPC(new NPC.HitInfo(){ Damage = Data.Damage, DamageType = DamageClass.Melee });
                            Data.HitNpc[i] = true;
                        }
                    }
                }
                User.position = EndPosition;
                User.SetFallStart();
                User.velocity.X = 8f * (MovingLeft ? -1f : 1f);
            }
            else if(Data.GetTime >= 100)
            {
                Data.EndUse();
            }
        }

        public int GetDamage(Companion companion)
        {
            if (NPC.downedGolemBoss)
            {
                return 150;
            }
            else if (NPC.downedMechBossAny)
            {
                return 90;
            }
            else if (Main.hardMode)
            {
                return 60;
            }
            else if (NPC.downedBoss3)
            {
                return 40;
            }
            return 20;
        }

        public static bool CanRushThrough(ref Vector2 Position, bool MovingLeft)
        {
            int tx = (int)Position.X / 16 + (MovingLeft ? -1 : 1), ty = (int)Position.Y / 16;
            bool FailedOnce = false;
            byte VerticalMoveCount = 0;
            for (int Attempt = 0; Attempt < 2; Attempt++)
            {
                bool Failed = false;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        if (PathFinder.CheckForSolidBlocks(tx + x, ty + y, 1, true))
                        {
                            if (y <= 1 || FailedOnce)
                                return false;
                            else
                                ty--;
                            FailedOnce = true;
                            Failed = true;
                        }
                    }
                }
                if (!Failed)
                {
                    bool HasSolidGround = false;
                    for (int x = 0; x < 2; x++)
                    {
                        if (PathFinder.CheckForSolidBlocks(tx + x, ty + 3, 1))
                        {
                            HasSolidGround = true;
                            break;
                        }
                    }
                    if (!HasSolidGround)
                    {
                        if (FailedOnce)
                            return false;
                        if (VerticalMoveCount >= 3)
                            return false;
                        ty++;
                        VerticalMoveCount++;
                    }
                }
            }
            Position.X = tx * 16;
            Position.Y = ty * 16;
            return true;
        }

        public override void UpdateStatus(Companion User, SubAttackData Data)
        {
            User.noKnockback = true;
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            int Time = Data.GetTime;
            if (Time > 10 && Time < 55)
            {
                User.ArmFramesID[0] = User.ArmFramesID[1] = (short)((Time - 10) * 0.5f % 9);
            }
            else
            {
                if (User.velocity.Y != 0)
                {
                    User.ArmFramesID[0] = User.ArmFramesID[1] = 9;
                }
                else
                {
                    User.ArmFramesID[0] = User.ArmFramesID[1] = 19;
                }
            }
            if ((User.Data as PigGuardianFragmentPiece.PigGuardianFragmentData).IsCloudForm)
                User.BodyFrameID = 19;
        }

        public class WrathDestructiveRushAttackData : SubAttackData
        {
            public Vector2 ChargeDashDestination = Vector2.Zero;
            public int Damage = 0;
            public bool[] HitNpc = new bool[200];
        }
    }
}