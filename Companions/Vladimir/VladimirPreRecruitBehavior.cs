using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Companions.Vladimir
{
    public class VladimirPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        public override bool AllowDespawning => true;

        RecruitStates state = 0;
        byte FishsTaken = 0, FishsToTake = 0;
        const int FishID = Terraria.ID.ItemID.Honeyfin;
        const string FishName = "Honeyfin";
        byte ComplaintCooldown = 0;
        byte LastHoneySenseValue = 0;
        ushort MessageTime = 0;
        ushort NextMessageTime = 300;
        bool HuggingPlayer = false, HasHugCommentHappened = false;
        short HugTime = 0;
        bool PlayerHasVladimir { get { return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Vladimir); }}

        public override string CompanionNameChange(Companion companion)
        {
            return "Bear Guardian";
        }

        public override void Update(Companion companion)
        {
            bool JustSpottedPlayer = false;
            if (state < RecruitStates.SpottedPlayer)
            {
                Target = ViewRangeCheck(companion, companion.direction, 300, 150);
                if (Target != null)
                {
                    JustSpottedPlayer = true;
                    state = RecruitStates.SpottedPlayer;
                }
            }
            if (state == RecruitStates.Wandering)
            {
                WanderAI(companion);
            }
            if (state < RecruitStates.RequestTaken)
            {
                MessageTime++;
                if (state == RecruitStates.SpottedPlayer)
                {
                    if (JustSpottedPlayer)
                        companion.SaySomething("*Hey! Can you hear me? Come closer so we can talk, please.*");
                }
                else
                {
                    if (MessageTime >= NextMessageTime)
                    {
                        NextMessageTime = (ushort)(300 + 60 * (3 + Main.rand.Next(4)));
                        MessageTime = 0;
                        string Message;
                        switch(Main.rand.Next(5))
                        {
                            default:
                                Message = "*Is there really a town around here?*";
                                break;
                            case 1:
                                Message = "*I'm so hungry... I've been walking for days...*";
                                break;
                            case 2:
                                Message = "*I need to take a rest...*";
                                break;
                            case 3:
                                Message = "*Why are all the creatures here so aggressive?*";
                                break;
                            case 4:
                                Message = "*I wonder if there is someone nearby...*";
                                break;
                        }
                        companion.SaySomething(Message);
                    }
                }
                if (Target != null) companion.FaceSomething(Target);
            }
            else if (state >= RecruitStates.RequestCompleted)
            {
                if (HuggingPlayer)
                {
                    UpdateHug(companion);
                }
                else
                {
                    companion.FaceSomething(Target);
                }
            }
            else if (state == RecruitStates.RequestTaken)
            {
                float Distance = (companion.Bottom - Target.Bottom).Length();
                if (Distance > 520)
                {
                    companion.Teleport(Target);
                }
                else if (MathF.Abs(companion.Center.X - Target.Center.X) > 68)
                {
                    MoveTowards(companion, Target.Bottom);
                    companion.WalkMode = false;
                }
            }
            //UpdateHug(companion);
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (HuggingPlayer)
                UpdateHugAnimation(companion);
        }

        protected void UpdateHug(Companion companion)
        {
            DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
            Vladimir.VladimirCompanion data = (Vladimir.VladimirCompanion)companion;
            bool FaceBear = (companion.BodyFrameID != 20 && companion.BodyFrameID != 21) || companion.BodyFrameID == 25;
            Player Character = PlayerMod.PlayerGetControlledCompanion(Target);
            if (Character == null) Character = Target;
            if (Character.mount.Active)
                Character.mount.Dismount(Character);
            TerraGuardian Vladimir = (TerraGuardian)companion;
            Character.position = Vladimir.GetMountShoulderPosition;
            Character.position.X -= Character.width * 0.5f;
            Character.position.Y -= Character.height * 0.5f;
            if (data.CarrySomeone)
                Character.position.X -= 6 * companion.direction;
            Character.gfxOffY = 0;
            Character.velocity.X = 0;
            Character.velocity.Y = -Player.defaultGravity;
            Character.fallStart = (int)(Character.position.Y * Companion.DivisionBy16);
            if (Character.itemAnimation == 0)
                Character.ChangeDir(companion.direction * (FaceBear ? -1 : 1));
            Character.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
            if (PlayerMod.GetPlayerKnockoutState(Character) > KnockoutStates.Awake)
                Character.GetModPlayer<PlayerMod>().ChangeReviveStack(3);
            Character.immuneTime = 3;
            Character.immuneNoBlink = true;
            byte ActionTaken = 0;
            const byte ItemUsed = 1, JumpUsed = 2, MoveUsed = 3;
            if (Character.controlUseItem)
                ActionTaken = ItemUsed;
            else if (Character.controlJump)
                ActionTaken = JumpUsed;
            else if (Character.controlLeft || Character.controlUp || Character.controlRight || Character.controlDown)
                ActionTaken = MoveUsed;
            Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(Character);
            if (Mount != null)
            {
                Mount.ToggleMount(Character, true);
            }
            string Message = null;
            if (state >= RecruitStates.HugPassed && ActionTaken > 0)
            {
                if (ComplaintCooldown == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        default:
                            Message = "*You can just talk to me if that's enough.*";
                            break;
                        case 1:
                            Message = "*Do you want me to stop? Talk to me again.*";
                            break;
                        case 2:
                            Message = "*Had enough of this hug? Talk to me so I can stop.*";
                            break;
                    }
                }
                ComplaintCooldown = 30;
            }
            else
            {
                switch(ActionTaken)
                {
                    case ItemUsed:
                    {
                        if(ComplaintCooldown == 0)
                        {
                            HugTime -= (short)Main.rand.Next(19, 41);
                            if (Target.inventory[Target.selectedItem].damage > 0)
                            {
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        Message = ("*Be careful! That can hurt someone!*");
                                        break;
                                    case 1:
                                        Message = ("*Hey! Are you trying to hurt me?*");
                                        break;
                                    case 2:
                                        Message = ("*You're doing the complete opposite of what a hug should do!*");
                                        break;
                                    case 3:
                                        Message = ("*You'll end up hurting someone with that.*");
                                        break;
                                    case 4:
                                        Message = ("*Keep that off my skin!*");
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        Message = ("*What are you doing?*");
                                        break;
                                    case 1:
                                        Message = ("*Can you stop doing that?*");
                                        break;
                                    case 2:
                                        Message = ("*Hey! Watch out!*");
                                        break;
                                    case 3:
                                        Message = ("*Do you easily get bored?*");
                                        break;
                                    case 4:
                                        Message = ("*Why are you doing that?*");
                                        break;
                                }
                            }
                        }
                        ComplaintCooldown = 30;
                    }
                    break;

                    case JumpUsed:
                        {
                            if (ComplaintCooldown == 0)
                            {
                                HugTime -= (short)Main.rand.Next(23, 54);
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        Message = ("*Oof! Hey! My stomach!*");
                                        break;
                                    case 1:
                                        Message = ("*Ouch! Are you a cat or something?*");
                                        break;
                                    case 2:
                                        Message = ("*Urgh. Don't hit my belly!*");
                                        break;
                                    case 3:
                                        Message = ("*Hey! Why the aggression?*");
                                        break;
                                    case 4:
                                        Message = ("*Urrrf... (Breathing for a moment) Gasp! You took out all my air with that kick! Why?! Tell me why you did that?*");
                                        break;
                                }
                            }
                            ComplaintCooldown = 30;
                        }
                        break;

                    case MoveUsed:
                        {
                            if (ComplaintCooldown == 0)
                            {
                                HugTime -= (short)Main.rand.Next(14, 33);
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        Message = ("*What are you doing?*");
                                        break;
                                    case 1:
                                        Message = ("*It's hard to hug with you like doing that.*");
                                        break;
                                    case 2:
                                        Message = ("*If you want me to stop, just tell me.*");
                                        break;
                                    case 3:
                                        Message = ("*Are you tense or something? Stop that.*");
                                        break;
                                    case 4:
                                        Message = ("*You'll end up falling if you keep doing that.*");
                                        break;
                                }
                            }
                            ComplaintCooldown = 30;
                        }
                        break;
                    
                    default:
                        {
                            if (ComplaintCooldown > 0) ComplaintCooldown--;
                            if (HugTime < short.MaxValue) HugTime++;
                            if (state >= RecruitStates.HugPassed && HugTime % (60 * 10) == 0)
                            {
                                switch (Main.rand.Next(4))
                                {
                                    default:
                                        Message = "*You want some more hugs? I don't mind.*";
                                        break;
                                    case 1:
                                        Message = "*(Humming)*";
                                        break;
                                    case 2:
                                        Message = "*Talk to me If you had enough.*";
                                        break;
                                    case 3:
                                        Message = "*Warm.*";
                                        break;
                                }
                            }
                            else if (HugTime >= (PlayerMod.PlayerHasCompanion(Target, companion) ? 0.3f : (Main.expertMode ? 0.7f : 0.5f)) * 3600)
                            {
                                if (state < RecruitStates.HugPassed)
                                {
                                    state = RecruitStates.HugPassed;
                                    Message = "*Thank you, I was really needing that. I think this world could make use of someone like me. Call me Vladimir, It's my name.*";
                                    HugTime = 1;
                                }
                            }
                            else if (HugTime % 600 == 0)
                            {
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        Message = ("*You don't know how much I missed hug.*");
                                        break;
                                    case 1:
                                        Message = ("*It's good to feel another warm body after so much travel.*");
                                        break;
                                    case 2:
                                        Message = ("*Just some more.*");
                                        break;
                                    case 3:
                                        Message = ("*Thanks for the fish, by the way.*");
                                        break;
                                    case 4:
                                        Message = ("*Are there more people like you in this world?*");
                                        break;
                                }
                            }
                        }
                        break;
                }
                if (!HasHugCommentHappened && HugTime >= 90)
                {
                    foreach(Companion c in PlayerMod.PlayerGetSummonedCompanions(Target))
                    {
                        if (!c.IsBeingControlledBySomeone)
                        {
                            string Mes = c.GetDialogues.GetOtherMessage(c, MessageIDs.VladimirRecruitPlayerGetsHugged);
                            if (Mes != "")
                            {
                                c.SaySomething(Mes);
                                break;
                            }
                        }
                    }
                    HasHugCommentHappened = true;
                }
                if (HugTime < -200)
                {
                    switch (Main.rand.Next(5))
                    {
                        default:
                            Message = "*Alright, there you go. Happy?*";
                            break;
                        case 1:
                            Message = "*It was just a hug, why were you moving around like crazy?*";
                            break;
                        case 2:
                            Message = "*Do hugs makes you uncomfortable or something?*";
                            break;
                        case 3:
                            Message = "*Need to use the toilet or something?*";
                            break;
                        case 4:
                            Message = "*Is it the environment or me?*";
                            break;
                    }
                    HuggingPlayer = false;
                    Character.Bottom = companion.Bottom;
                }
            }
            if (Message != null)
            {
                companion.SaySomething(Message);
            }
            if (!Character.active)
            {
                companion.SaySomething("*Where did that person go?*");
                HuggingPlayer = false;
                Target = null;
                return;
            }
        }

        protected void UpdateHugAnimation(Companion companion)
        {
            short Frame = 1;
            switch (companion.BodyFrameID)
            {
                case 11:
                    Frame = 12;
                    break;
                case 22:
                    Frame = 23;
                    break;
                case 24:
                    Frame = 25;
                    break;
                case 27:
                    Frame = 28;
                    break;
                case 29:
                    Frame = 30;
                    break;
                case 31:
                    Frame = 32;
                    break;
            }
            if (companion.BodyFrameID == 0 || companion.BodyFrameID == 11 || companion.BodyFrameID == 22 || companion.BodyFrameID == 24)
                companion.BodyFrameID = Frame;
            if ((companion as TerraGuardian).HeldItems[1].ItemAnimation == 0)
            {
                companion.ArmFramesID[1] = Frame;
            }
            if (companion.itemAnimation == 0)
            {
                companion.ArmFramesID[0] = Frame;
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            MessageDialogue md = new MessageDialogue();
            if (HuggingPlayer)
            {
                if (state >= RecruitStates.HugPassed)
                {
                    md.ChangeMessage("*I'm feeling a lot better now, but I have a request for you. Can I move into your world? Maybe there are some other people who need my help.*");
                    md.AddOption("May you put me on the floor?", AskToBePlacedOnGroundAfterHugPassed);
                    md.AddOption("Sure, you may live here.", FinalyRecruitVladimir);
                    md.AddOption("No, you can't stay here.", FinalyRecruitVladimirButNoMoveIn);
                }
                else
                {
                    md.ChangeMessage("*I still need some more hug.*");
                    md.AddOption("Enough hug.", StopBeingHugged);
                    md.AddOption("Nevermind.", Dialogue.EndDialogue);
                }
            }
            else if (state == RecruitStates.HugPassed)
            {
                md.ChangeMessage("*Now that I placed you on the ground, can I move into your world? Maybe there are some people who need my help.*");
                md.AddOption("Sure, you may live here.", FinalyRecruitVladimir);
                md.AddOption("No, you can't stay here.", FinalyRecruitVladimirButNoMoveIn);
            }
            else if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion))
            {
                if (state < RecruitStates.RequestCompleted)
                    state = RecruitStates.RequestCompleted;
                md.ChangeMessage("*Hello Terrarian. It's me, the marsupial bear of the hugs. I was traveling around the world trying to find other places that could use my help, the only luck I had was bumping into you on the way. You know what you need to do to have me move into this world.*");
                md.AddOption("Give you a hug? Fine.", OnBeingHugged);
                md.AddOption("Not this time.", OnRejectHug);
            }
            else
            {
                switch(state)
                {
                    case RecruitStates.RequestCompleted:
                        md.ChangeMessage("*I'm all fed now, but I need to feel the warmth of another body. Can you give me a hug?*");
                        md.AddOption("Sure..", OnBeingHugged);
                        md.AddOption("A hug?", OnAskAboutHug);
                        md.AddOption("No way!", OnRejectHug);
                        break;
                    case RecruitStates.RequestTaken:
                        md.ChangeMessage("*My belly is still complaining... Do you have some more " + FishName + "?*");
                        if (MainMod.GetLocalPlayer.CountItem(FishID) > 0)
                            md.AddOption("Give " + FishName + ".", OnGiveFishs);
                        md.AddOption("Not yet.", OnNotGiveFish);
                        break;
                    default:
                        if (PlayerMod.PlayerGetControlledCompanion(MainMod.GetLocalPlayer) != null)
                        {
                            md.ChangeMessage("*You're a TerraGuardian! No, wait... You're a Terrarian bond-merged with a TerraGuardian! That Guardian must really like you to allow you to do that. Could you help me? I'm hungry and I need some fish to eat...*");
                        }
                        else
                        {
                            md.ChangeMessage("*Woah! You're a Terrarian? Amazing! Can you help me? I'm hungry, but I can't seem to be able to get some fish...*");
                        }
                        md.AddOption("I can try.", OnAcceptRequest);
                        md.AddOption("No.", OnRejectRequest);
                        break;
                }
            }
            return md;
        }

        private void FinalyRecruitVladimir()
        {
            bool LastHasMetVladimir = PlayerHasVladimir;
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, GetOwner);
            WorldMod.AddCompanionMet(GetOwner);
            WorldMod.AllowCompanionNPCToSpawn(GetOwner);
            MessageDialogue md = new MessageDialogue();
            if(HuggingPlayer)
            {
                GetOwner.RunBehavior(new VladimirHugPlayerBehavior((TerraGuardian)GetOwner, Target));
                md.ChangeMessage("*Thank you! I will try finding me an empty house to move in, but first, I will wait until you ask me to stop hugging you.*");
            }
            else
            {
                md.ChangeMessage("*Thank you! I will try looking for a house for me to live. I'll see you another time.*");
            }
            md.AddOption("Alright.", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        private void FinalyRecruitVladimirButNoMoveIn()
        {
            bool LastHasMetVladimir = PlayerHasVladimir;
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, GetOwner);
            WorldMod.AddCompanionMet(GetOwner);
            MessageDialogue md = new MessageDialogue();
            if(HuggingPlayer)
            {
                GetOwner.RunBehavior(new VladimirHugPlayerBehavior((TerraGuardian)GetOwner, Target));
                md.ChangeMessage("*Oh, that's fine. Then I guess I'll just hug you for a while, before I move away. Thanks for the fish and the hug, by the way. Feel free to call me anytime.*");
            }
            else
            {
                md.ChangeMessage("*Oh, okay then. I'll just stay around here for a while before looking for somewhere else to go. Thanks for the fish and the hug, by the way. Feel free to call me anytime.*");
            }
            md.AddOption("Alright.", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        private void AskToBePlacedOnGroundAfterHugPassed()
        {
            MessageDialogue md = new MessageDialogue("*Oh! Alright, alright. Say... Can I move in to your world? Maybe there are people who need hugs here.*");
            HuggingPlayer = false;
            Target.Bottom = GetOwner.Bottom;
            md.AddOption("Yes, you can move in.", FinalyRecruitVladimir);
            md.AddOption("Sorry, you can't stay here.", FinalyRecruitVladimirButNoMoveIn);
            md.RunDialogue();
        }

        private void StopBeingHugged()
        {
            MessageDialogue md = new MessageDialogue("*Aww... I wanted some more hug...*");
            HuggingPlayer = false;
            HasHugCommentHappened = false;
            md.RunDialogue();
        }

        private void OnBeingHugged()
        {
            HugTime = 0;
            HuggingPlayer = true;
            MessageDialogue md = new MessageDialogue();
            if (PlayerHasVladimir)
            {
                md.ChangeMessage("*Please be patient, this wont take long.*");
            }
            else
            {
                md.ChangeMessage("*It wont take too long.*");
            }
            md.RunDialogue();
        }

        private void OnAskAboutHug()
        {
            MessageDialogue md = new MessageDialogue("*It has been quite a long time since I had some contact with someone, and I really love giving hugs. Will you give me one?*");
            md.AddOption("Sure, why not?", OnBeingHugged);
            md.AddOption("No way!", OnRejectHug);
            md.RunDialogue();
        }

        private void OnRejectHug()
        {
            MessageDialogue md = new MessageDialogue();
            if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Vladimir))
            {
                switch (Main.rand.Next(5))
                {
                    default:
                        md.ChangeMessage("*We have done that before, give me a hug.*");
                        break;
                    case 1:
                        md.ChangeMessage("*Don't be like that, you know I wont hurt you.*");
                        break;
                    case 2:
                        md.ChangeMessage("*That gives me flashbacks of when we first met. Or was it another Terrarian?*");
                        break;
                    case 3:
                        md.ChangeMessage("*You will refuse my hug? I'm sad now.*");
                        break;
                    case 4:
                        md.ChangeMessage("*I've been walking for a long time, would be nice to hug someone friendly.*");
                        break;
                }
            }
            else
            {
                switch (Main.rand.Next(6))
                {
                    default:
                        md.ChangeMessage("*Come on, It's just a hug. Are you scared of me?*");
                        break;
                    case 1:
                        md.ChangeMessage("*I need that to make me feel better. It's been a long time since I last saw a person.*");
                        break;
                    case 2:
                        md.ChangeMessage("*I wont eat you or something. Come on, give me a hug.*");
                        break;
                    case 3:
                        md.ChangeMessage("*I'm not a bad person, I wont hurt you either, trust me.*");
                        break;
                    case 4:
                        md.ChangeMessage("*That saddens me, why don't you give me a hug?*");
                        break;
                    case 5:
                        md.ChangeMessage("*You won't end up like the fish, I just want a hug.*");
                        break;
                }
            }
            md.RunDialogue();
        }

        private void OnNotGiveFish()
        {
            MessageDialogue md = new MessageDialogue("*Please, gather some more "+FishName+", I'm really hungry.*");
            md.RunDialogue();
        }

        private void OnGiveFishs()
        {
            int FishAcquired = MainMod.GetLocalPlayer.CountItem(FishID);
            for (int i = 0; i < 50; i++)
            {
                if(MainMod.GetLocalPlayer.inventory[i].type == FishID)
                {
                    MainMod.GetLocalPlayer.inventory[i].SetDefaults(0);
                }
            }
            FishsTaken = (byte)(MathF.Min(FishsTaken + FishAcquired, 255));
            /*if (FishsTaken + FishAcquired > 255)
                FishsTaken = 255;
            else
                FishsTaken += (byte)FishAcquired;*/
            MessageDialogue md = new MessageDialogue();
            if (FishsTaken >= FishsToTake)
            {
                md.ChangeMessage("*I'm stuffed. Thank you friend. You helped me, and I can help you if you give me a hug.*");
                state = RecruitStates.RequestCompleted;
                md.AddOption("A hug?", OnAskAboutHug);
            }
            else
            {
                md.ChangeMessage("*Amazing! (He eats them) I'm still hungry... Could you get some more " + FishName + " for me?*");
            }
            md.RunDialogue();
        }

        private void OnAcceptRequest()
        {
            state = RecruitStates.RequestTaken;
            FishsToTake = (byte)Main.rand.Next(7, 13);
            MessageDialogue md = new MessageDialogue("*Thank you! I need some "+FishName+"s. Please be fast, I'm so hungry...*");
            md.RunDialogue();
        }

        private void OnRejectRequest()
        {
            MessageDialogue md = new MessageDialogue("*Oh... I'll get back to trying to get some fish then... (Stomach growling) Be quiet, I know I'm hungry.*");
            md.RunDialogue();
        }

        enum RecruitStates : byte
        {
            Wandering = 0,
            SpottedPlayer = 1,
            RequestTaken = 2,
            RequestCompleted = 3,
            HugPassed = 4
        }
    }
}
