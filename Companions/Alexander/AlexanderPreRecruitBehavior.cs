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
                    if (NpcRecruitStep == IntroductingToPlayerStep)
                    {
                        DialogueDuration = 1;
                    }
                    else
                    {
                        NpcRecruitStep = IdleStep;
                        DialogueDuration = 0;
                    }
                    companion.SaySomething("*Hm.. "+(Target.Male ? "He" : "She")+" disappeared...*");
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
                                companion.SaySomething("*Better I take care of those wounds first.*");
                                DialogueDuration++;
                            }
                            Target.GetModPlayer<PlayerMod>().ChangeReviveStack(1);
                        }
                        else
                        {
                            switch(DialogueDuration)
                            {
                                case 0:
                                    companion.SaySomething("*Got you! Now don't move.*");
                                    break;
                                case 1 * 210:
                                    companion.SaySomething("*Hm...*");
                                    break;
                                case 2 * 210:
                                    companion.SaySomething("*Interesting...*");
                                    break;
                                case 3 * 210:
                                    companion.SaySomething("*Uh huh...*");
                                    break;
                                case 4 * 210:
                                    companion.SaySomething("*But you're not the one I'm looking for...*");
                                    break;
                                case 5 * 210:
                                    NpcRecruitStep = IntroductingToPlayerStep;
                                    DialogueDuration = 0;
                                    Target.fullRotation = 0;
                                    Target.fullRotationOrigin = new Vector2(40, 56) * 0.5f;
                                    Target.position = companion.Bottom;
                                    Target.position.Y -= Target.height;
                                    Target.immuneNoBlink = false;
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
                            DialogueDuration = 1; //Can't talk to him...
                            if (Target == MainMod.GetLocalPlayer)
                                Dialogue.StartDialogue(companion);
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
                                companion.SaySomething("*I didn't expected to see you here.*");
                                break;
                            case 1 * 180:
                                companion.SaySomething("*I think I may have caught that Terrarian's scent around here.*");
                                break;
                            case 2 * 180:
                                companion.SaySomething("*Or maybe I accidentally caught your scent again.*");
                                break;
                            case 3 * 180:
                                companion.SaySomething("*Anyway, If you need me, I'll be around.*");
                                WorldMod.AddCompanionMet(companion);
                                break;
                        }
                        DialogueDuration++;
                        if (Math.Abs(Target.Center.X - companion.Center.X) > 30)
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
            MessageDialogue md = new MessageDialogue("*Terrarian, " + (Target.Male ? "Male" : "Female") + ", " + (PlayerMod.PlayerGetTerraGuardianCompanionsMet(Target) > 0 ? "and you seems to have met some TerraGuardians.*" : "and It must be a shock for you to see me.*"));
            md.AddOption("Why did you jumped on me?", DialogueWhyJumpedOnPlayer);
            md.AddOption("You could have asked, instead.", DialogueCouldHaveAskedInstead);
            return md;
        }

        void DialogueWhyJumpedOnPlayer()
        {
            MessageDialogue md = new MessageDialogue("*I had to make sure if you were the person I am looking for, and I simply have you try to run away.*");
            md.AddOption("Who are you looking for?", DialogueAlexanderTellsReason);
            md.RunDialogue();
        }

        void DialogueCouldHaveAskedInstead()
        {
            MessageDialogue md = new MessageDialogue("*That's not what I wanted to know. It wouldn't be necessary to catch you to know those informations.*");
            md.AddOption("Then why you jumped on me?", DialogueAlexanderTellsReason);
            md.RunDialogue();
        }

        void DialogueAlexanderTellsReason()
        {
            MessageDialogue md = new MessageDialogue("*There is a Terrarian, who seems to be involved in the disappearance of my friends.*");
            md.AddOption("A Terrarian? Why a Terrarian?", DialogueAskWhyHeThinksIsATerrarian);
            md.RunDialogue();
        }

        void DialogueAskWhyHeThinksIsATerrarian()
        {
            MessageDialogue md = new MessageDialogue("*I know by the scent. The last place I caught my friends scent, had that unfamiliar scent too. That's the only clue I've got from my friends disappearance. My sleuthing is very accurate, which explains why I caught you, just to make sure.*");
            md.AddOption("And why you're investigating here?", DialogueAskWhyHesInvestigatingHere);
            md.RunDialogue();
        }

        void DialogueAskWhyHesInvestigatingHere()
        {
            MessageDialogue md = new MessageDialogue("*That same scent, I caught it down here too. That Terrarian must be, or have been in this world, so I needed to investigate further.*");
            md.AddOption("So, you plan on looking around this place more?", DialogueTellHimToLookAround);
            md.RunDialogue();
        }

        void DialogueTellHimToLookAround()
        {
            MessageDialogue md = new MessageDialogue("*That's what I intend to do. I do have a request for you: I need somewhere safe to gather clues and try finding out more pieces of this puzzle. Would you mind if I stay in this world for a while?*");
            md.AddOption("Yes, you may stay here.", DialogueAcceptHimMoveIn);
            md.AddOption("No, you can't stay here.", DialogueRejectHimMoveIn);
            md.RunDialogue();
        }

        void DialogueAcceptHimMoveIn()
        {
            PlayerMod.PlayerAddCompanion(Target, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
            Dialogue.LobbyDialogue("*Thank you, Terrarian. My name is " + Dialogue.Speaker.GetRealName + ". If you find a suspicious Terrarian, bring him to me.*");
        }

        void DialogueRejectHimMoveIn()
        {
            PlayerMod.PlayerAddCompanion(Target, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            Dialogue.LobbyDialogue("*I see... Anyway, my name is " + Dialogue.Speaker.GetRealName + ". If you change your mind, or find a suspicious Terrarian in your world, you just need to call me. I'll visit your world regularly looking for clues. Until another time.*");
        }

        private const byte IdleStep = 0,
            CallingOutPlayerStep = 1,
            ChasingPlayerStep = 2,
            InvestigatingPlayerStep = 3,
            IntroductingToPlayerStep = 4,
            SpeakingToAlreadyKnownPlayer = 5;
    }
}