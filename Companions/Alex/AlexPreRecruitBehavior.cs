using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Alex
{
    public class AlexPreRecruitBehavior : BehaviorBase
    {
        int AI_TYPE = 0, AI_TIME = 0;
        public override bool AllowDespawning => false;
        Player ChasedTarget = null;
        Vector2 SpawnPosition = Vector2.Zero;
        bool PlayerRecruitedAlex = false;
        bool LastOnFloor = false;

        public AlexPreRecruitBehavior()
        {
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return false;
        }

        public override void Update(Companion companion)
        {
            bool PlayerOnTheFloor = false;
            switch(AI_TYPE)
            {
                case 0: //Wait for Player
                    {
                        companion.WalkMode = true;
                        if (SpawnPosition.X == 0 && SpawnPosition.Y == 0)
                            SpawnPosition = companion.position;
                        Rectangle FoV = new Rectangle(0, -150, 250, 300);
                        if (companion.direction < 0)
                            FoV.X -= FoV.Width;
                        FoV.X += (int)companion.Center.X;
                        FoV.Y += (int)companion.Center.Y;
                        for(int i = 0; i < 255; i++)
                        {
                            if(Main.player[i] != companion && !(Main.player[i] is Companion) && Main.player[i].active && !Main.player[i].dead)
                            {
                                Player player = Main.player[i];
                                if (FoV.Intersects(player.getRect()) && companion.CanHit(player))
                                {
                                    AI_TYPE = 1;
                                    AI_TIME = 0;
                                    ChasedTarget = player;
                                    companion.WalkMode = false;
                                    return;
                                }
                            }
                        }
                        bool TeleportToSpawn = false;
                        if (ChasedTarget == null)
                        {
                            float XDiference = SpawnPosition.X - companion.position.X, YDiference = SpawnPosition.Y - companion.position.Y;
                            if (Math.Abs(XDiference) >= NPC.sWidth || Math.Abs(YDiference) >= NPC.sHeight)
                            {
                                TeleportToSpawn = true;
                            }
                            else if (Math.Abs(XDiference) >= 16)
                            {
                                if (XDiference < 0)
                                    companion.MoveLeft = true;
                                else
                                    companion.MoveRight = true;
                            }
                        }
                        if (TeleportToSpawn)
                        {
                            companion.position = SpawnPosition;
                            companion.SetFallStart();
                        }
                    }
                    break;
                case 1: //Chase target
                    {
                        if(ChasedTarget == null || !ChasedTarget.active || ChasedTarget.dead)
                        {
                            ChasedTarget = null;
                            AI_TYPE = 0;
                            AI_TIME = 0;
                        }
                        else
                        {
                            Vector2 DistanceDiference = ChasedTarget.Center - companion.Center;
                            if (Math.Abs(DistanceDiference.X) >= NPC.sWidth || Math.Abs(DistanceDiference.Y) >= NPC.sHeight)
                            {
                                ChasedTarget = null;
                                AI_TYPE = 0;
                                AI_TIME = 0;
                                return;
                            }
                            else
                            {
                                if (DistanceDiference.X < 0)
                                    companion.MoveLeft = true;
                                else
                                    companion.MoveRight = true;
                                if (companion.CanJump && Math.Abs(DistanceDiference.X) <= (companion.width + companion.Base.JumpSpeed) * 2)
                                {
                                    companion.ControlJump = true;
                                }
                                if (companion.Hitbox.Intersects(ChasedTarget.getRect()))
                                {
                                    AI_TYPE = 2;
                                    AI_TIME = 0;
                                    PlayerRecruitedAlex = PlayerMod.PlayerHasCompanion(ChasedTarget, companion.ID, companion.ModID);
                                    if (!PlayerRecruitedAlex)
                                        PlayerOnTheFloor = true;
                                    Companion mount = PlayerMod.PlayerGetMountedOnCompanion(ChasedTarget);
                                    if (mount != null)
                                    {
                                        mount.ToggleMount(ChasedTarget, true);
                                    }
                                    mount = PlayerMod.PlayerGetCompanionMountedOnMe(ChasedTarget);
                                    if (mount != null)
                                    {
                                        mount.ToggleMount(ChasedTarget, true);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2: //Speak with Player
                    {
                        if(ChasedTarget == null || !ChasedTarget.active || ChasedTarget.dead)
                        {
                            ChasedTarget = null;
                            AI_TYPE = 3;
                            AI_TIME = 0;
                        }
                        else
                        {
                            if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                            {
                                if(AI_TIME % 30 == 15)
                                {
                                    int DialogueTimer = (int)(AI_TIME * (1f / 30));
                                    if (PlayerRecruitedAlex)
                                    {
                                        switch(DialogueTimer)
                                        {
                                            case 0:
                                                companion.SaySomething("Hey buddy-buddy!");
                                                break;
                                            case 5:
                                                companion.SaySomething("It's good to see your face again.");
                                                break;
                                            case 10:
                                                companion.SaySomething("Anything you want, I'm here to protect you.");
                                                WorldMod.AddCompanionMet(companion);
                                                ChasedTarget = null;
                                                return;
                                        }
                                    }
                                    else
                                    {
                                        switch(DialogueTimer)
                                        {
                                            case 0:
                                                companion.SaySomething("Hello! Who are you?");
                                                break;
                                            case 5:
                                                companion.SaySomething("Are you my new friend? Do you want to be my friend?");
                                                break;
                                            case 10:
                                                companion.SaySomething("You're saying that I'm crushing your chest? Oh! My bad!");
                                                break;
                                            case 12:
                                                companion.velocity.X -= 5 * companion.direction;
                                                companion.velocity.Y -= companion.Base.JumpSpeed;
                                                break;
                                            case 15:
                                                companion.SaySomething("By the way, I'm " + companion.name + ". Let's go on an adventure.");
                                                PlayerMod.PlayerAddCompanion(ChasedTarget, companion);
                                                companion.IncreaseFriendshipPoint(1);
                                                WorldMod.AddCompanionMet(companion);
                                                WorldMod.AllowCompanionNPCToSpawn(companion);
                                                ChasedTarget = null;
                                                return;
                                        }
                                    }
                                }
                                AI_TIME ++;
                            }
                            if (!PlayerRecruitedAlex)
                            {
                                if (AI_TIME < 12 * 30)
                                    PlayerOnTheFloor = true;
                            }
                            else
                            {
                                float XDiference = ChasedTarget.Center.X - companion.Center.X;
                                if (Math.Abs(XDiference) > companion.width)
                                {
                                    if (XDiference > 0)
                                        companion.MoveRight = true;
                                    else
                                        companion.MoveLeft = true;
                                }
                            }
                        }
                    }
                    break;
                case 3: //Where did the player go?
                    {
                        if (AI_TIME % 30 == 15)
                        {
                            int DialogueTime = (int)(AI_TIME * (1f / 30));
                            switch(DialogueTime)
                            {
                                case 3:
                                    companion.SaySomething("Where did they go?");
                                    break;
                                case 7:
                                    companion.SaySomething("Better I go guard the tombstone then...");
                                    AI_TYPE = 0;
                                    AI_TIME = 0;
                                    return;
                            }
                        }
                        AI_TIME++;
                    }
                    break;
            }
            if (ChasedTarget != null)
            {
                if (PlayerOnTheFloor)
                {
                    Vector2 NewPosition = Vector2.Zero;
                    NewPosition.X = companion.Center.X + 26 * companion.direction;
                    NewPosition.Y = companion.position.Y + companion.height;
                    ChasedTarget.fullRotationOrigin.X = ChasedTarget.width * 0.5f;
                    ChasedTarget.fullRotationOrigin.Y = ChasedTarget.height * 0.5f;
                    ChasedTarget.velocity = Vector2.Zero;
                    if (ChasedTarget.mount.Active)
                        ChasedTarget.mount.Dismount(ChasedTarget);
                    if (companion.velocity.Y != 0)
                        ChasedTarget.fullRotation = companion.direction * MathHelper.ToRadians(45);
                    else
                        ChasedTarget.fullRotation = companion.direction * MathHelper.ToRadians(90);
                    ChasedTarget.gfxOffY = -2;
                    ChasedTarget.Center = NewPosition;
                    ChasedTarget.direction = -companion.direction;
                    ChasedTarget.immuneTime = 30;
                    ChasedTarget.immuneNoBlink = true;
                    ChasedTarget.fallStart = (int)ChasedTarget.position.Y / 16;
                    ChasedTarget.breath = ChasedTarget.breathMax;
                    ChasedTarget.statLife++;
                    if (ChasedTarget.statLife > ChasedTarget.statLifeMax2)
                        ChasedTarget.statLife = ChasedTarget.statLifeMax2;
                    DrawOrderInfo.AddDrawOrderInfo(ChasedTarget, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                }
                else if (LastOnFloor)
                {
                    ChasedTarget.fullRotation = 0;
                    ChasedTarget.velocity.Y -= 6.5f;
                    ChasedTarget.direction = -companion.direction;
                    ChasedTarget.fullRotationOrigin = Vector2.Zero;
                }
                LastOnFloor = PlayerOnTheFloor;
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (AI_TYPE == 2)
            {
                if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                {
                    if (!PlayerRecruitedAlex && AI_TIME < 12 * 30)
                    {
                        int NewFrame = (AI_TIME / 5) % 4;
                        if (NewFrame == 3)
                            NewFrame = 1;
                        companion.BodyFrameID = (short)(NewFrame + 19);
                        companion.ArmFramesID[0] = companion.BodyFrameID;
                    }
                }
            }
        }
    }
}