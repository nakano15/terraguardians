using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Alexander
{
    public class AlexanderPreRecruitBehavior : PreRecruitBehavior
    {
        private int NpcRecruitStep = IdleStep, DialogueDuration = 0;
        Player Target = null;

        public AlexanderPreRecruitBehavior()
        {
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }

        public override string CompanionNameChange(Companion companion)
        {
            return "Dog TerraGuardian";
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return NpcRecruitStep == IntroductingToPlayerStep;
        }

        public override void Update(Companion companion)
        {
            if (Target != null)
            {
                if (!Target.active)
                {
                    NpcRecruitStep = IdleStep;
                    companion.SaySomething("*Hm.. "+(Target.Male ? "He" : "She")+" disappeared...*");
                    Target = null;
                    return;
                }
                else if (!Target.dead)
                {
                    NpcRecruitStep = IdleStep;
                    companion.SaySomething("*Well... That's... Horrible...*");
                    Target = null;
                    return;
                }
            }
            switch (NpcRecruitStep)
            {
                case IdleStep:
                    {
                        Target = ViewRangeCheck(companion, companion.direction);
                        if (Target != null)
                        {
                            if (PlayerMod.PlayerHasCompanion(Target, companion))
                            {
                                companion.SaySomething("*Hey, "+Target.name+"! Over here!*");
                            }
                            else if (PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake)
                            {
                                companion.SaySomething("*You there! Hang on!*");
                            }
                            else
                            {
                                if (Main.rand.Next(2) == 0)
                                    companion.SaySomething("*Hey! You there! Stop!*");
                                else
                                    companion.SaySomething("*Hey! You! Don't move!*");
                            }
                            NpcRecruitStep = ChasingPlayerStep;
                            DialogueDuration = 0;
                        }
                        else
                        {
                            WanderAI(companion);
                            DialogueDuration++;
                            if (DialogueDuration >= 600)
                            {
                                DialogueDuration -= 600;
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        companion.SaySomething("*Hm... It must be around here somewhere...*");
                                        break;
                                    case 1:
                                        companion.SaySomething("*Come on... Show up...*");
                                        break;
                                    case 2:
                                        companion.SaySomething("*...It's around here... I smell It.*");
                                        break;
                                    case 3:
                                        companion.SaySomething("*Terrarian, where are you...*");
                                        break;
                                    case 4:
                                        companion.SaySomething("*Once I catch that Terrarian.... Ugh...*");
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case CallingOutPlayerStep:
                    {
                        if (DialogueDuration >= 120)
                        {
                            NpcRecruitStep = ChasingPlayerStep;
                            DialogueDuration = 0;
                        }
                        else
                        {
                            DialogueDuration++;
                        }
                        if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                        {
                            if (Target.Center.X < companion.Center.X)
                            {
                                companion.direction = -1;
                            }
                            else
                            {
                                companion.direction = 1;
                            }
                        }
                    }
                    break;
                case ChasingPlayerStep:
                    {
                        if (Target.Center.X < companion.Center.X)
                        {
                            companion.MoveLeft = true;
                        }
                        else
                        {
                            companion.MoveRight = true;
                        }
                        if (Target.velocity.Y < 0 && Target.Bottom.Y < companion.Bottom.Y && ((companion.jump == 0 && companion.velocity.Y == 0) || companion.jump > 0))
                        {
                            companion.ControlJump = true;
                        }
                        else if (Target.Hitbox.Intersects(companion.Hitbox))
                        {
                            if (PlayerMod.PlayerHasCompanion(Target, companion))
                            {
                                NpcRecruitStep = SpeakingToAlreadyKnownPlayer;
                            }
                            else
                            {
                                NpcRecruitStep = InvestigatingPlayerStep;
                            }
                            DialogueDuration = 0;
                        }
                    }
                    break;
                case InvestigatingPlayerStep:
                    {
                        if (Target.mount.Active)
                            Target.mount.Dismount(Target);
                        Companion mount = PlayerMod.PlayerGetMountedOnCompanion(Target);
                        if (mount != null)
                        {
                            mount.ToggleMount(Target, true);
                        }
                        Target.immuneTime = 90;
                        Target.immuneNoBlink = true;
                        Vector2 PlayerPosition = new Vector2(companion.Center.X + 32 * companion.direction, companion.Bottom.Y);
                        Target.fullRotationOrigin = new Vector2(Target.width, Target.height) * .5f;
                        Target.aggro = -99999;
                        Target.fullRotation = companion.direction * 1.570796f;
                        Target.direction = -companion.direction;
                        DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                        Target.gfxOffY = -2;
                        Target.AddBuff(BuffID.Cursed, 5);
                        
                    }
                    break;
            }
        }

        private const byte IdleStep = 0,
            CallingOutPlayerStep = 1,
            ChasingPlayerStep = 2,
            InvestigatingPlayerStep = 3,
            IntroductingToPlayerStep = 4,
            SpeakingToAlreadyKnownPlayer = 5;
        private const byte DialogueFirstTalk = 1,
            DialogueQuestionJump = 2,
            DialogueTellThatCouldHaveAskedInstead = 3,
            DialogueAlexanderTellsReason = 4,
            DialogueAskHowCouldBeATerrarian = 5,
            DialogueAskHowCouldBeRelatedToFriendsDisappearance = 6,
            DialogueAskWhyHeConfusedYourCharacterScentToThatTerrariansScent = 7,
            DialogueAskWhyHeCaughtYourScent = 8,
            DialogueTellHimToLookAround = 9,
            DialogueTellHimToHaveGoodLuck = 10,
            DialogueAcceptHimMovingIn = 11,
            DialogueRejectHimMovingIn = 12;
    }
}