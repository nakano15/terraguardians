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
        byte NpcRecruitStep = IdleStep;
        int DialogueDuration = 0;
        new Player Target = null;

        public AlexanderPreRecruitBehavior()
        {
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }

        public override string CompanionNameChange(Companion companion)
        {
            return companion.GetTranslation("recruitalias");
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
                    if (NpcRecruitStep == IntroductingToPlayerStep)
                    {
                        DialogueDuration = 1;
                    }
                    else
                    {
                        NpcRecruitStep = IdleStep;
                        DialogueDuration = 0;
                    }
                    companion.SaySomething(companion.GetTranslation("recruitdisappear"));
                    Target = null;
                    return;
                }
                else if (Target.dead)
                {
                    if (NpcRecruitStep == IntroductingToPlayerStep)
                    {
                        DialogueDuration = 1;
                    }
                    else
                    {
                        NpcRecruitStep = IdleStep;
                        DialogueDuration = 0;
                    }
                    companion.SaySomething(companion.GetTranslation("recruitdead"));
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
                                companion.SaySomething(companion.GetTranslation("recruitfamiliarface").Replace("[name]", Target.name));
                            }
                            else if (PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake)
                            {
                                companion.SaySomething(companion.GetTranslation("recruitresfriend"));
                            }
                            else
                            {
                                companion.SaySomething(companion.GetTranslation("recruitwarn" + Main.rand.Next(1, 3)));
                            }
                            NpcRecruitStep = CallingOutPlayerStep;
                            DialogueDuration = 0;
                        }
                        else
                        {
                            WanderAI(companion);
                            DialogueDuration++;
                            if (DialogueDuration >= 600)
                            {
                                DialogueDuration -= 600;
                                companion.SaySomething(companion.GetTranslation("recruitidle" + Main.rand.Next(1, 6)));
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
                        companion.WalkMode = false;
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
                        Vector2 PlayerPosition = new Vector2(companion.Center.X + 32 * companion.direction, companion.Bottom.Y - 20);
                        Target.fullRotationOrigin = new Vector2(Target.width, Target.height) * .5f;
                        Target.aggro = -99999;
                        Target.fullRotation = companion.direction * 1.570796f;
                        Target.direction = -companion.direction;
                        Target.position = PlayerPosition;
                        DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                        Target.gfxOffY = -2;
                        Target.AddBuff(BuffID.Cursed, 5);
                        if (PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake)
                        {
                            if (DialogueDuration == 0)
                            {
                                companion.SaySomething(companion.GetTranslation("recruitressay"));
                                DialogueDuration++;
                            }
                            Target.GetModPlayer<PlayerMod>().ChangeReviveStack(1);
                        }
                        else
                        {
                            switch(DialogueDuration)
                            {
                                case 0:
                                    companion.SaySomething(companion.GetTranslation("recruitcatch1"));
                                    break;
                                case 1 * 210:
                                    companion.SaySomething(companion.GetTranslation("recruitcatch2"));
                                    break;
                                case 2 * 210:
                                    companion.SaySomething(companion.GetTranslation("recruitcatch3"));
                                    break;
                                case 3 * 210:
                                    companion.SaySomething(companion.GetTranslation("recruitcatch4"));
                                    break;
                                case 4 * 210:
                                    companion.SaySomething(companion.GetTranslation("recruitcatch5"));
                                    break;
                                case 5 * 210:
                                    NpcRecruitStep = IntroductingToPlayerStep;
                                    DialogueDuration = 0;
                                    Target.fullRotation = 0;
                                    Target.fullRotationOrigin = new Vector2(20, 28);
                                    Target.position = companion.Bottom;
                                    Target.position.Y -= Target.height;
                                    Target.immuneNoBlink = true;
                                    Target.immuneTime = 180;
                                    return;
                            }
                            DialogueDuration++;
                        }
                    }
                    break;
                case IntroductingToPlayerStep:
                    {
                        if (DialogueDuration == 0)
                        {
                            DialogueDuration = 1;
                            if (Target == MainMod.GetLocalPlayer)
                                Dialogue.StartDialogue(companion); //Can't talk to him like this...
                        }
                        WanderAI(companion);
                        if (DialogueDuration > 1 && (!Dialogue.InDialogue || !Dialogue.IsParticipatingDialogue(companion)))
                        {
                            DialogueDuration = 1;
                        }
                    }
                    break;
                case SpeakingToAlreadyKnownPlayer:
                    {
                        switch(DialogueDuration)
                        {
                            case 0:
                                companion.SaySomething(companion.GetTranslation("recruitfamiliar1"));
                                break;
                            case 1 * 180:
                                companion.SaySomething(companion.GetTranslation("recruitfamiliar2"));
                                break;
                            case 2 * 180:
                                companion.SaySomething(companion.GetTranslation("recruitfamiliar3"));
                                break;
                            case 3 * 180:
                                companion.SaySomething(companion.GetTranslation("recruitfamiliar4"));
                                WorldMod.AddCompanionMet(companion);
                                return;
                        }
                        DialogueDuration++;
                        if (Math.Abs(Target.Center.X - companion.Center.X) > 60)
                        {
                            companion.WalkMode = true;
                            if(Target.Center.X < companion.Center.X)
                            {
                                companion.MoveLeft = true;
                            }
                            else
                            {
                                companion.MoveRight = true;
                            }
                        }
                        else
                        {
                            if(Target.Center.X < companion.Center.X)
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
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (NpcRecruitStep == InvestigatingPlayerStep)
            {
                short Frame = 0;
                if (PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake)
                {
                    Frame = 22;
                }
                else
                {
                    int a = DialogueDuration % 210;
                    if (DialogueDuration < 4 * 210 && a >= 70 && a < 180)
                    {
                        if ((a >= 70 && a < 80) ||
                            (a >= 100 && a < 110) ||
                            (a >= 130 && a < 140))
                        {
                            Frame = 29;
                        }
                        else
                        {
                            Frame = 28;
                        }
                    }
                    else
                    {
                        Frame = 27;
                    }
                }
                companion.BodyFrameID = companion.ArmFramesID[0] = companion.ArmFramesID[1] = Frame;
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            //[gender] = (Target.Male ? "Male" : "Female")
            //[companions] = (PlayerMod.PlayerGetTerraGuardianCompanionsMet(Target) > 0 ? "and you seems to have met some TerraGuardians.*" : "and It must be a shock for you to see me.*")
            MessageDialogue md = new MessageDialogue(companion.GetTranslation("recruittalk1")
                .Replace("[gender]", Localization.GetTranslation(Target.Male ? Localization.LocalizationKeys.male : Localization.LocalizationKeys.female))
                .Replace("[companions]", companion.GetTranslation(PlayerMod.PlayerGetTerraGuardianCompanionsMet(Target) > 0 ? "recruittalk1hascompanions" : "recruittalk1nocompanions")));
            md.AddOption(companion.GetTranslation("recruitanswer1-1"), DialogueWhyJumpedOnPlayer);
            md.AddOption(companion.GetTranslation("recruitanswer1-2"), DialogueCouldHaveAskedInstead);
            return md;
        }

        void DialogueWhyJumpedOnPlayer()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk2"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer2"), DialogueAlexanderTellsReason);
            md.RunDialogue();
        }

        void DialogueCouldHaveAskedInstead()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk3"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer3"), DialogueAlexanderTellsReason);
            md.RunDialogue();
        }

        void DialogueAlexanderTellsReason()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk4"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer4"), DialogueAskWhyHeThinksIsATerrarian);
            md.RunDialogue();
        }

        void DialogueAskWhyHeThinksIsATerrarian()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk5"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer5"), DialogueAskWhyHesInvestigatingHere);
            md.RunDialogue();
        }

        void DialogueAskWhyHesInvestigatingHere()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk6"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer6"), DialogueTellHimToLookAround);
            md.RunDialogue();
        }

        void DialogueTellHimToLookAround()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.Speaker.GetTranslation("recruittalk7"));
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer7-1"), DialogueAcceptHimMoveIn);
            md.AddOption(Dialogue.Speaker.GetTranslation("recruitanswer7-2"), DialogueRejectHimMoveIn);
            md.RunDialogue();
        }

        void DialogueAcceptHimMoveIn()
        {
            Dialogue.LobbyDialogue(Dialogue.GetTranslation("recruittalk8").Replace("[name]", Dialogue.Speaker.GetRealName));
            PlayerMod.PlayerAddCompanion(Target, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
        }

        void DialogueRejectHimMoveIn()
        {
            Dialogue.LobbyDialogue(Dialogue.Speaker.GetTranslation("recruittalk9").Replace("[name]", Dialogue.Speaker.GetRealName));
            PlayerMod.PlayerAddCompanion(Target, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
        }

        private const byte IdleStep = 0,
            CallingOutPlayerStep = 1,
            ChasingPlayerStep = 2,
            InvestigatingPlayerStep = 3,
            IntroductingToPlayerStep = 4,
            SpeakingToAlreadyKnownPlayer = 5;
    }
}