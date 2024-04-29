using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using nterrautils;

namespace terraguardians
{
    public partial class Dialogue
    {
        private static MessageBase[] LobbyMessage;
        const string TranslationKey = "Mods.terraguardians.Interface.Dialogue.";

        public static string GetTranslation(string Key)
        {
            return GetTranslation(Key, MainMod.GetMod);
        }

        public static string GetTranslation(string Key, Mod mod)
        {
            return Terraria.Localization.Language.GetTextValue("Mods." + mod.Name + ".Interface.Dialogue." + Key);
        }

        public static void Load()
        {

        }

        private static void UnloadDefaultDialogues()
        {
            LobbyMessage = null;
        }
        
        public static void LobbyDialogue()
        {
            if (!CheckForImportantMessages())
                LobbyDialogue("");
        }

        private static bool CheckForImportantMessages()
        {
            while (ImportantUnlockMessagesToCheck <= 128)
            {
                UnlockAlertMessageContext context = (UnlockAlertMessageContext)ImportantUnlockMessagesToCheck;
                if (!Speaker.Data.UnlockAlertsDone.HasFlag(context))
                {
                    bool Notify = false;
                    switch(context)
                    {
                        case UnlockAlertMessageContext.MoveInUnlock:
                            if (Speaker.Base.GetFriendshipUnlocks.MoveInUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.MoveInUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.ControlUnlock:
                            if (Speaker.Base.GetFriendshipUnlocks.ControlUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.ControlUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.FollowUnlock:
                            if (Speaker.Base.GetFriendshipUnlocks.FollowerUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.FollowerUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.MountUnlock:
                            if (Speaker.Base.GetFriendshipUnlocks.MountUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.MountUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.RequestsUnlock:
                            if (Speaker.Base.GetFriendshipUnlocks.RequestUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.RequestUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.BuddiesModeUnlock:
                            if (!PlayerMod.GetIsBuddiesMode(MainMod.GetLocalPlayer) && 
                                Speaker.Base.CanBeAppointedAsBuddy && 
                                Speaker.Base.GetFriendshipUnlocks.BuddyUnlock > 0 && 
                                Speaker.FriendshipLevel >= Speaker.Base.GetFriendshipUnlocks.BuddyUnlock)
                            {
                                Notify = true;
                            }
                            break;
                        case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                            if (PlayerMod.GetIsBuddiesMode(MainMod.GetLocalPlayer) && PlayerMod.GetIsPlayerBuddy(MainMod.GetLocalPlayer, Speaker))
                            {
                                Notify = true;
                            }
                            break;
                    }
                    if (Notify)
                    {
                        if (NotifyUnlock(context))
                            return true;
                    }
                }
                if (ImportantUnlockMessagesToCheck == 128)
                    ImportantUnlockMessagesToCheck = 255;
                else
                    ImportantUnlockMessagesToCheck *= 2;
            }
            return false;
        }

        private static bool NotifyUnlock(UnlockAlertMessageContext context)
        {
            Speaker.Data.UnlockAlertsDone |= context;
            string Message = Speaker.GetDialogues.UnlockAlertMessages(Speaker, context);
            if (Message == "") return false;
            MessageDialogue md = new MessageDialogue(Message);
            md.AddOption(GetTranslation("genericokay"), LobbyDialogue);
            md.RunDialogue();
            return true;
        }

        public static void LobbyDialogue(string LobbyMessage = "")
        {
            bool TryAddingCompanion = true;
            WorldMod.AddCompanionMet(Speaker.Data);
            returnToLobby:
            if(Speaker.sleeping.isSleeping && Speaker.Base.SleepsWhenOnBed && !HasBeenAwakened)
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.SleepingMessage(Speaker, SleepingMessageContext.WhenSleeping));
                string Pronoun = Speaker.GetPronoun();
                md.AddOption(GetTranslation("wakeupoption").Replace("[pronoun]", Pronoun), WhenWakingUpCompanion);
                md.AddOption(GetTranslation("letsleepoption").Replace("[pronoun]", Pronoun), EndDialogue);
                md.RunDialogue();
                return;
            }
            if(TryAddingCompanion && !PlayerMod.PlayerHasCompanion(Main.LocalPlayer, Speaker))
            {
                if (Speaker.IsGeneric)
                {
                    TryAddingCompanion = false;
                    if (LobbyMessage == "")
                        LobbyMessage = Speaker.GetDialogues.GreetMessages(Speaker);
                    goto returnToLobby;
                }
                else if (PlayerMod.PlayerAddCompanion(Main.LocalPlayer, Speaker))
                {
                    //if(Speaker.Index == 0 && Main.netMode == 0)
                    //    Speaker.Data = PlayerMod.PlayerGetCompanionData(Main.LocalPlayer, Speaker.ID, Speaker.GenericID, Speaker.ModID);
                    MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.GreetMessages(Speaker));
                    md.AddOption(new DialogueOption(GetTranslation("greet"), LobbyDialogue));
                    md.RunDialogue();
                }
                else
                {
                    TryAddingCompanion = false;
                    goto returnToLobby;
                }
            }
            else
            {
                {
                    MessageBase mb = Speaker.GetDialogues.MessageDialogueOverride(Speaker);
                    if (mb != null)
                    {
                        mb.RunDialogue();
                        return;
                    }
                }
                string Message;
                if(Speaker.IsComfortPointsMaxed())
                {
                    Message = Speaker.GetDialogues.TalkMessages(Speaker);
                }
                if (LobbyMessage != "")
                {
                    Message = LobbyMessage;
                }
                else if (Speaker.IsBeingControlledBy(MainMod.GetLocalPlayer) && (Message = Speaker.GetDialogues.ControlMessage(Speaker, ControlContext.ControlChatter)) != "")
                {

                }
                else
                {
                    Message = Speaker.GetDialogues.NormalMessages(Speaker);
                }
                MessageDialogue md = new MessageDialogue(Message);
                if (Speaker.IsBeingControlledBySomeone)
                {
                    if (Speaker.GetCharacterControllingMe == MainMod.GetLocalPlayer)
                    {
                        md.AddOption(PlayerMod.IsCompanionFreeControlEnabled(MainMod.GetLocalPlayer) ? GetTranslation("takecontroloption") : GetTranslation("givecontroloption"), ToggleFreeControl);
                        md.AddOption(GetTranslation("releasebondmergeoption"), ToggleControlScript);
                    }
                }
                else
                {
                    if(!Speaker.IsBeingControlledBySomeone && !HideJoinLeaveMessage && !Speaker.IsMountedOnSomething)
                    {
                        if(!Speaker.IsFollower)
                        {
                            if (Speaker.CanFollowPlayer()) md.AddOption(GetTranslation("joingroupoption"), JoinGroupMessage);
                        }
                        else if (Speaker.Owner == Main.LocalPlayer)
                        {
                            if (Speaker.CanStopFollowingPlayer()) md.AddOption(GetTranslation("leavegroupoption"), LeaveGroupMessage);
                        }
                    }
                    if (Speaker.IsGeneric && !PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, Speaker))
                    {
                        md.AddOption(GetTranslation("registercompanionoption"), RegisterGenericCompanionPrompt);
                    }
                    if(Speaker.Owner == Main.LocalPlayer)
                    {
                        if(!HideMountMessage && !Speaker.IsBeingControlledBySomeone)
                        {
                            if (Speaker.PlayerCanMountCompanion(MainMod.GetLocalPlayer))
                            {
                                if (Speaker.MountStyle != MountStyles.CantMount)
                                {
                                    if(!Speaker.IsMountedOnSomething)
                                    {
                                        if(Speaker.GetPlayerMod.GetMountedOnCompanion == null)
                                        {
                                            string MountText = GetTranslation("mountshoulderoption");
                                            switch(Speaker.MountStyle)
                                            {
                                                case MountStyles.CompanionRidesPlayer:
                                                    MountText = GetTranslation("reversemountshoulderoption");
                                                    break;
                                            }
                                            md.AddOption(MountText, MountMessage);
                                            if (Speaker.MountStyle == MountStyles.PlayerMountsOnCompanion)
                                                md.AddOption(GetTranslation("carrysomeoneoption"), CarrySomeoneActionLobby);
                                        }
                                    }
                                    else
                                    {
                                        if (Speaker.GetCharacterMountedOnMe == MainMod.GetLocalPlayer)
                                        {
                                            string DismountText = GetTranslation("placeongroundoption");
                                            switch(Speaker.MountStyle)
                                            {
                                                case MountStyles.CompanionRidesPlayer:
                                                    DismountText = GetTranslation("getoffshoulderoption");
                                                    break;
                                            }
                                            md.AddOption(DismountText, DismountMessage);
                                            if(Speaker.MountStyle == MountStyles.PlayerMountsOnCompanion) MountedFurnitureCheckScripts(md); //I have to fix issues where characters mounted using this have bugs when using furniture at the first time.
                                        }
                                        else
                                        {
                                            string MountedName = Speaker.GetCharacterMountedOnMe.name;
                                            md.AddOption(GetTranslation("placemountedongroundoption").Replace("[mountedname]", MountedName), DismountMessage);
                                        }
                                    }
                                }
                            }
                        }
                        if (PlayerMod.IsCompanionLeader(MainMod.GetLocalPlayer, Speaker) && !HideControlMessage && Speaker.Base.GetCompanionGroup.IsTerraGuardian && Speaker.PlayerCanControlCompanion(MainMod.GetLocalPlayer))
                            md.AddOption(GetTranslation("dobondmergeoption"), ToggleControlScript);
                        if ((!MainMod.GetLocalPlayer.sitting.isSitting && Speaker.GetCharacterMountedOnMe == MainMod.GetLocalPlayer && Speaker.MountStyle == MountStyles.PlayerMountsOnCompanion) ||
                            (MainMod.GetLocalPlayer.sitting.isSitting && Speaker.UsingFurniture && Speaker.Base.SitOnPlayerLapOnChair && !Speaker.IsUsingThroneOrBench && MainMod.GetLocalPlayer.Bottom == Speaker.Bottom))
                        {
                            md.AddOption(GetTranslation("petoption"), DoPetAction);
                        }
                    }
                    if (Speaker.CanTakeRequests(MainMod.GetLocalPlayer))
                    {
                        string RequestsMessageText = GetTranslation("askrequestoption");
                        switch(Speaker.GetRequest.status)
                        {
                            case RequestData.RequestStatus.Active:
                                RequestsMessageText = GetTranslation("aboutrequestoption");
                                break;
                        }
                        md.AddOption(RequestsMessageText, TalkAboutRequests);
                    }
                    md.AddOption(GetTranslation("doactionoption"), DoActionLobby);
                    md.AddOption(GetTranslation("justchatoption"), ChatDialogue);
                    md.AddOption(GetTranslation("misctalkoption"), TalkAboutOtherTopicsDialogue);
                }
                Speaker.GetDialogues.ManageLobbyTopicsDialogue(Speaker, md);
                Speaker.GetGoverningBehavior().ChangeLobbyDialogueOptions(md, out bool ShowCloseButton);
                if (MainMod.IsDebugMode)
                {
                    md.AddOption(GetTranslation("debugoption"), DebugLobby);
                }
                foreach (QuestData d in nterrautils.PlayerMod.GetPlayerQuests(MainMod.GetLocalPlayer))
                {
                    if (d.Base is QuestBase)
                        (d.Base as QuestBase).AddDialogueOptions(d, false, Speaker, md);
                }
                if(ShowCloseButton) md.AddOption(new DialogueOption(GetTranslation("genericgoodbye"), EndDialogue));
                md.RunDialogue();
            }
            //TestDialogue();
        }

        static void RegisterGenericCompanionPrompt()
        {
            MessageDialogue md = new MessageDialogue(GetTranslation("addcompanionaskmessage"));
            md.AddOption("Yes", RegisterGenericCompanion_Yes);
            md.AddOption("No", RegisterGenericCompanion_No);
            md.RunDialogue();
        }

        static void RegisterGenericCompanion_Yes()
        {
            if (PlayerMod.PlayerAddCompanion(Main.LocalPlayer, Speaker))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.GreetMessages(Speaker));
                md.AddOption(new DialogueOption(GetTranslation("greet"), LobbyDialogue));
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(GetTranslation("addcompanionfailmessage"));
                md.AddOption(new DialogueOption(GetTranslation("genericaww"), LobbyDialogue));
                md.RunDialogue();
            }
        }

        static void RegisterGenericCompanion_No()
        {
            LobbyDialogue();
        }

        private static void CarrySomeoneActionLobby()
        {
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.AskWhoToCarryMount));
            PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
            for(int i = 0; i < 9; i++)
            {
                if (i >= pm.GetSummonedCompanions.Length || pm.GetSummonedCompanions[i] == null)
                {
                    continue;
                }
                Companion c = pm.GetSummonedCompanions[i];
                if (c != Speaker && !c.IsMountedOnSomething && c.Base.Size < Speaker.Base.Size)
                {
                    System.Action Method = null;
                    switch(i)
                    {
                        case 0:
                            Method = Carry_0;
                            break;
                        case 1:
                            Method = Carry_1;
                            break;
                        case 2:
                            Method = Carry_2;
                            break;
                        case 3:
                            Method = Carry_3;
                            break;
                        case 4:
                            Method = Carry_4;
                            break;
                        case 5:
                            Method = Carry_5;
                            break;
                        case 6:
                            Method = Carry_6;
                            break;
                        case 7:
                            Method = Carry_7;
                            break;
                        case 8:
                            Method = Carry_8;
                            break;
                        case 9:
                            Method = Carry_9;
                            break;
                    }
                    md.AddOption(GetTranslation("carryoption").Replace("[target]", c.GetName), Method);
                }
            }
            md.AddOption(GetTranslation("nevermind"), LobbyDialogue);
            md.RunDialogue();
            HideMountMessage = true;
        }

        private static void Carry_0()
        {
            CarryCompanionAction(0);
        }

        private static void Carry_1()
        {
            CarryCompanionAction(1);
        }

        private static void Carry_2()
        {
            CarryCompanionAction(2);
        }

        private static void Carry_3()
        {
            CarryCompanionAction(3);
        }

        private static void Carry_4()
        {
            CarryCompanionAction(4);
        }

        private static void Carry_5()
        {
            CarryCompanionAction(5);
        }

        private static void Carry_6()
        {
            CarryCompanionAction(6);
        }

        private static void Carry_7()
        {
            CarryCompanionAction(7);
        }

        private static void Carry_8()
        {
            CarryCompanionAction(8);
        }

        private static void Carry_9()
        {
            CarryCompanionAction(9);
        }

        private static void CarryCompanionAction(int Index)
        {
            Companion Target = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().GetSummonedCompanions[Index];
            Speaker.ToggleMount(Target);
            LobbyDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.SuccessCompanionMount).Replace("[target]", Target.GetNameColored()));
        }

        private static void DoPetAction()
        {
            Speaker.PlayerPetCompanion(MainMod.GetLocalPlayer);
            EndDialogue();
        }

        public static void DoActionLobby()
        {
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.InteractionMessages(Speaker, InteractionMessageContext.OnAskForFavor));
            if(!Speaker.IsRunningBehavior && Speaker.Base.Size >= Sizes.Large)
            {
                md.AddOption(GetTranslation("liftupoption"), RaisePlayerAction);
            }
            md.AddOption(GetTranslation("nevermind"), ReturnToLobbyInteraction);
            md.RunDialogue();
        }

        private static void ReturnToLobbyInteraction()
        {
            LobbyDialogue(Speaker.GetDialogues.InteractionMessages(Speaker, InteractionMessageContext.Nevermind));
        }

        private static void RaisePlayerAction()
        {
            Speaker.RunBehavior(new LiftPlayerBehavior(MainMod.GetLocalPlayer, Dialogue.Speaker));
            LobbyDialogue(Speaker.GetDialogues.InteractionMessages(Speaker, InteractionMessageContext.Accepts));
        }

        private static void WhenWakingUpCompanion()
        {
            HasBeenAwakened = true;
            Speaker.LeaveFurniture();
            LobbyDialogue(Speaker.GetDialogues.SleepingMessage(Speaker, SleepingMessageContext.OnWokeUp));
        }

        private static void MountedFurnitureCheckScripts(MessageDialogue md)
        {
            if(Speaker.GoingToOrUsingFurniture) return;
            bool HasChair = false, HasBed = false;
            int CompanionBottomX = (int)(Speaker.Bottom.X * (1f / 16)), CompanionBottomY = (int)(Speaker.Bottom.Y * (1f / 16));
            for(int x = -4; x <= 4; x++)
            {
                for(int y = -3; y < 3; y++)
                {
                    Tile tile = Main.tile[CompanionBottomX + x, CompanionBottomY + y];
                    if(tile != null && tile.HasTile)
                    {
                        switch(tile.TileType)
                        {
                            case Terraria.ID.TileID.Chairs:
                            case Terraria.ID.TileID.PicnicTable:
                            case Terraria.ID.TileID.Benches:
                            case Terraria.ID.TileID.Thrones:
                                HasChair = true;
                                break;
                            case Terraria.ID.TileID.Beds:
                                HasBed = true;
                                break;
                        }
                    }
                }
            }
            if(HasChair && Speaker.Base.AllowSharingChairWithPlayer)
            {
                md.AddOption(GetTranslation("resthereoption"), UseNearbyChairAction);
            }
            if(HasBed && Speaker.Base.AllowSharingBedWithPlayer)
            {
                md.AddOption(GetTranslation("sleephereoption"), UseNearbyBedAction);
            }
        }

        public static void UseNearbyChairAction()
        {
            Point p = WorldMod.GetClosestChair(Speaker.Bottom, 4, 3);
            if(p.X > 0 && p.Y > 0)
            {
                Speaker.UseFurniture(p.X, p.Y);
            }
            LobbyDialogue();
        }

        public static void UseNearbyBedAction()
        {
            Point p = WorldMod.GetClosestBed(Speaker.Bottom, 4, 3);
            if(p.X > 0 && p.Y > 0)
            {
                Speaker.UseFurniture(p.X, p.Y);
            }
            EndDialogue();
        }

        public static void AskToMoveInMessage()
        {
            HideMovingMessage = true;
            if(Speaker.IsTownNpc)
            {
                LobbyDialogue();
                return;
            }
            bool NotFriendsEnough;
            if (Speaker.CanLiveHere(out NotFriendsEnough) && WorldMod.AllowCompanionNPCToSpawn(Speaker))
            {
                WorldMod.SetCompanionTownNpc(Speaker);
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.AskCompanionToMoveInMessage(Speaker, MoveInContext.Success));
                md.AddOption(GetTranslation("moveinsuccessansweroption"), LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.AskCompanionToMoveInMessage(Speaker, NotFriendsEnough ? MoveInContext.NotFriendsEnough : MoveInContext.Fail));
                md.AddOption(GetTranslation("moveinfailansweroption"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void AskToMoveOutMessage()
        {
            HideMovingMessage = true;
            if(!Speaker.IsTownNpc)
            {
                LobbyDialogue();
                return;
            }
            WorldMod.RemoveCompanionNPCToSpawn(Speaker);
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.AskCompanionToMoveOutMessage(Speaker, MoveOutContext.Success));
            md.AddOption(GetTranslation("moveoutsuccessoption"), LobbyDialogue);
            md.RunDialogue();
        }

        public static void JoinGroupMessage()
        {
            HideJoinLeaveMessage = true;
            if(!PlayerMod.PlayerHasEmptyFollowerSlot(Main.LocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.FullParty));
                md.AddOption(GetTranslation("genericaww"), LobbyDialogue);
                md.RunDialogue();
            }
            else if(Speaker.CanFollowPlayer() && PlayerMod.PlayerCallCompanion(Main.LocalPlayer, Speaker))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.Success));
                md.AddOption(GetTranslation("genericthanks"), LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.Fail));
                md.AddOption(GetTranslation("joinfailansweroption"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void LeaveGroupMessage()
        {
            HideJoinLeaveMessage = true;
            if(!PlayerMod.PlayerHasCompanionSummonedByIndex(Main.LocalPlayer, Speaker.Index))
            {
                LobbyDialogue();
            }
            else if(Speaker.CanStopFollowingPlayer() && PlayerMod.PlayerDismissCompanionByIndex(Main.LocalPlayer, Speaker.Index, false))
            {
                if (Speaker.active == false)
                {
                    CompanionSpeakMessage(Speaker, Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    PlayerMod.EndDialogue(Main.LocalPlayer);
                }
                else
                {
                    MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    md.AddOption(GetTranslation("leavemessagesuccessanswer"), EndDialogue);
                    md.RunDialogue();
                }
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Fail));
                md.AddOption(GetTranslation("leavemessagefailanswer"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void MountMessage()
        {
            if(Speaker.IsMountedOnSomething)
            {
                DismountMessage();
                return;
            }
            HideMountMessage = true;
            if (!Speaker.PlayerCanMountCompanion(Main.LocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.NotFriendsEnough));
                md.AddOption(GetTranslation("genericokay"), LobbyDialogue);
                md.RunDialogue();
            }
            else if(Speaker.ToggleMount(Main.LocalPlayer))
            {
                string Mes = "";
                switch(Speaker.MountStyle)
                {
                    default:
                        Mes = Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.Success);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.SuccessMountedOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption(GetTranslation("genericthanks"), LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.Fail));
                md.AddOption(GetTranslation("genericokay"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void DismountMessage()
        {
            if(!Speaker.IsMountedOnSomething)
            {
                MountMessage();
                return;
            }
            HideMountMessage = true;
            if(Speaker.ToggleMount(Speaker.GetCharacterMountedOnMe))
            {
                string Mes = "";
                switch(Speaker.MountStyle)
                {
                    default:
                        Mes = Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMount);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMountOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption(GetTranslation("genericthanks"), LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.Fail));
                md.AddOption(GetTranslation("genericokay"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void ChatDialogue()
        {
            if(Dialogue.NotFirstTalkAboutOtherMessage)
                ChatDialogue(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.AfterFirstTime));
            else
                ChatDialogue(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.FirstTimeInThisDialogue));
        }

        public static void ChatDialogue(string Message)
        {
            MessageDialogue md = new MessageDialogue(Message);
            Dialogue.NotFirstTalkAboutOtherMessage = true;
            Speaker.GetDialogues.ManageChatTopicsDialogue(Speaker, md);
            md.AddOption(GetTranslation("enoughtalkhaveatyouoption"), OnSayingNevermindOnTalkingAboutOtherTopics);
            md.RunDialogue();
        }

        public static void TalkAboutOtherTopicsDialogue()
        {
            if(Dialogue.NotFirstTalkAboutOtherMessage)
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.AfterFirstTime));
            else
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.FirstTimeInThisDialogue));
        }

        public static void TalkAboutOtherTopicsDialogue(string Message)
        {
            MessageDialogue md = new MessageDialogue(Message);
            Dialogue.NotFirstTalkAboutOtherMessage = true;
            if (!HideMovingMessage && !Speaker.IsGeneric)
            {
                if(!Speaker.IsTownNpc)
                {
                    md.AddOption(GetTranslation("asktolivehereoption"), AskToMoveInMessage);
                }
                else
                {
                    md.AddOption(GetTranslation("asktomoveoutoption"), AskToMoveOutMessage);
                }
            }
            if (Speaker.Owner == Main.LocalPlayer || Speaker.Owner == null)
                md.AddOption(GetTranslation("reviewbehavioroption"), ChangeTacticsTopicDialogue);
            if (Speaker.Owner == Main.LocalPlayer)
            {
                if(Speaker.Base.AllowSharingChairWithPlayer)
                    md.AddOption(!Speaker.ShareChairWithPlayer ? (Speaker.Base.SitOnPlayerLapOnChair ? GetTranslation("sharechairsmallcompanionoption") : GetTranslation("sharechairbigcompanionoption")) : GetTranslation("stopsharingchairoption"), ToggleSharingChair);
                if (Speaker.Base.AllowSharingBedWithPlayer)
                    md.AddOption(GetTranslation(!Speaker.ShareBedWithPlayer ? "sharebedoption" : "stopsharingbedoption"), ToggleSharingBed);
                md.AddOption(GetTranslation(!Speaker.Data.TakeLootPlayerTrashes ? "takediscardedlootoption" : "donttakediscardedlootoption"), TogglePickupTrashedItems);
                md.AddOption(GetTranslation(!Speaker.Data.AutoSellItemsWhenInventoryIsFull ? "sellitemsonfullinvoption" : "dontsellitemsonfullinvoption"), ToggleAutoSellItems);
                md.AddOption(GetTranslation(!Speaker.Data.AttackOwnerTarget ? "attackmytargetoption" : "dontattackmytargetoption"), ToggleAttackMyTarget);
                if (!PlayerMod.IsCompanionLeader(MainMod.GetLocalPlayer, Speaker))
                    md.AddOption(GetTranslation("leadgroupoption") , LeadGroupDialogue);
                    
            }
            if (Speaker is TerraGuardian && (Speaker.Owner == Main.LocalPlayer || Speaker.Owner == null)) md.AddOption(Speaker.PlayerSizeMode ? GetTranslation("backtonormalsizeoption") : GetTranslation("beofmysizeoption"), TogglePlayerSize);
            Speaker.GetDialogues.ManageOtherTopicsDialogue(Speaker, md);
            if (!PlayerMod.GetIsPlayerBuddy(MainMod.GetLocalPlayer, Speaker) && Speaker.Base.CanBeAppointedAsBuddy) md.AddOption(GetTranslation("bemybuddyoption") , BuddyProposal);
            foreach (QuestData d in nterrautils.PlayerMod.GetPlayerQuests(MainMod.GetLocalPlayer))
            {
                if (d.Base is QuestBase)
                    (d.Base as QuestBase).AddDialogueOptions(d, true, Speaker, md);
            }
            md.AddOption(GetTranslation("nevermind"), OnSayingNevermindOnTalkingAboutOtherTopics);
            md.RunDialogue();
        }

        static void ToggleAttackMyTarget()
        {
            Speaker.Data.AttackOwnerTarget = !Speaker.Data.AttackOwnerTarget;
            if (Speaker.Data.AttackOwnerTarget)
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillDo));
            }
            else
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillNotDo));
            }
        }

        static void TogglePickupTrashedItems()
        {
            Speaker.Data.TakeLootPlayerTrashes = !Speaker.Data.TakeLootPlayerTrashes;
            if (Speaker.Data.TakeLootPlayerTrashes)
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillDo));
            }
            else
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillNotDo));
            }
        }

        static void ToggleAutoSellItems()
        {
            Speaker.Data.AutoSellItemsWhenInventoryIsFull = !Speaker.Data.AutoSellItemsWhenInventoryIsFull;
            if (Speaker.Data.AutoSellItemsWhenInventoryIsFull)
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillDo));
            }
            else
            {
                TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.GenericWillNotDo));
            }
        }

        private static void LeadGroupDialogue()
        {
            if (PlayerMod.GetIsBuddiesMode(MainMod.GetLocalPlayer))
            {
                LobbyDialogue(Speaker.GetDialogues.ChangeLeaderMessage(Speaker, ChangeLeaderContext.Failed));
            }
            else
            {
                if(!PlayerMod.PlayerChangeLeaderCompanion(MainMod.GetLocalPlayer, Speaker))
                {
                    LobbyDialogue(Speaker.GetDialogues.ChangeLeaderMessage(Speaker, ChangeLeaderContext.Failed));
                }
                else
                {
                    LobbyDialogue(Speaker.GetDialogues.ChangeLeaderMessage(Speaker, ChangeLeaderContext.Success));
                }
            }
        }

        public static void BuddyProposal()
        {
            bool LackFriendship;
            if (!Speaker.CanAppointBuddy(out LackFriendship))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, LackFriendship ? BuddiesModeContext.NotFriendsEnough : BuddiesModeContext.Failed));
                md.AddOption(GetTranslation("buddyfailnotfriendsenoughoption"), EndDialogue);
                md.RunDialogue();
                return;
            }
            if (PlayerMod.GetIsBuddiesMode(MainMod.GetLocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, BuddiesModeContext.AlreadyHasBuddy));
                md.AddOption(GetTranslation("buddyfailhasonealreadyoption"), EndDialogue);
                md.RunDialogue();
                return;
            }
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, BuddiesModeContext.AskIfPlayerIsSure) + "\n[c/FF1919:(Warning: This is a for life proposal. Once picking a buddy, you can't change or remove it from your group.)]");
                md.AddOption(GetTranslation("buddymodeyesoption"), BuddyProposalAskIfSureYes);
                md.AddOption(GetTranslation("buddymodenooption"), BuddyProposalAskIfSureNo);
                md.RunDialogue(); //Test companion carry companion
            }
        }

        private static void BuddyProposalAskIfSureYes()
        {
            if (MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().SetPlayerBuddy(Speaker))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, BuddiesModeContext.PlayerSaysYes));
                md.AddOption(GetTranslation("buddymodepostaddoption"), LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, BuddiesModeContext.Failed));
                md.AddOption(GetTranslation("buddymodefailoption"), EndDialogue);
                md.RunDialogue();
            }
        }

        private static void BuddyProposalAskIfSureNo()
        {
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.BuddiesModeMessage(Speaker, BuddiesModeContext.PlayerSaysNo));
            md.AddOption(GetTranslation("buddymodepostrefusaloption"), EndDialogue);
            md.RunDialogue();
        }

        public static void TogglePlayerSize()
        {
            Speaker.PlayerSizeMode = !Speaker.PlayerSizeMode;
            TalkAboutOtherTopicsDialogue();
        }

        static string GetTacticIndex(CombatTactics tactic)
        {
            switch(tactic)
            {
                case CombatTactics.CloseRange:
                    return Language.GetTextValue("Mods.terraguardians.Tactics.closerange");
                case CombatTactics.MidRange:
                    return Language.GetTextValue("Mods.terraguardians.Tactics.midrange");
                case CombatTactics.LongRange:
                    return Language.GetTextValue("Mods.terraguardians.Tactics.longrange");
                case CombatTactics.StickClose:
                    return Language.GetTextValue("Mods.terraguardians.Tactics.stickclose");
            }
            return tactic.ToString();
        }

        public static void ChangeTacticsTopicDialogue()
        {
            string TacticsMessage = GetTranslation("tacticsmessagereviewlayour")
                .Replace("[message]", Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.OnAskToChangeTactic))
                .Replace("[tactic]", GetTacticIndex(Speaker.CombatTactic))
                .Replace("[followorder]", (Speaker.Data.FollowAhead ? Language.GetTextValue("Mods.terraguardians.Tactics.followahead") : Language.GetTextValue("Mods.terraguardians.Tactics.followbehind")))
                .Replace("[combatapproach]", (Speaker.Data.AvoidCombat ? Language.GetTextValue("Mods.terraguardians.Tactics.avoidcombat") : Language.GetTextValue("Mods.terraguardians.Tactics.partakeincombat")));
            MessageDialogue md = new MessageDialogue(TacticsMessage);
            if(Speaker.CombatTactic != CombatTactics.CloseRange)
                md.AddOption(GetTranslation("tacticcloserangeoption"), ChangeCloseRangeTactic);
            if(Speaker.CombatTactic != CombatTactics.MidRange)
                md.AddOption(GetTranslation("tacticmidrangeoption"), ChangeMidRangeTactic);
            if(Speaker.CombatTactic != CombatTactics.LongRange)
                md.AddOption(GetTranslation("tacticlongrangeoption"), ChangeLongRangeTactic);
            if(Speaker.CombatTactic != CombatTactics.StickClose)
                md.AddOption(GetTranslation("tacticstickcloseoption"), ChangeStickCloseTactic);
            md.AddOption(Speaker.Data.FollowAhead ? GetTranslation("tacticfollowbehindoption") : GetTranslation("tacticfollowaheadoption"), ToggleFollowAhead);
            md.AddOption(Speaker.Data.AvoidCombat ? GetTranslation("tacticjoinfightoption") : GetTranslation("tacticavoidcombatoption"), ToggleTakeOnCombat);
            md.AddOption(Speaker.Data.PrioritizeHelpingAlliesOverFighting ? GetTranslation("tacticfightoverhealoption") : GetTranslation("tactichealoverfightoption"), ToggleHelpingOverFightingDialogue);
            if (Speaker.Base.GetSubAttackBases.Count > 0)
            {
                md.AddOption(Speaker.Data.UnallowAutoUseSubattacks ? GetTranslation("tacticsallowsubattackuseoption") : GetTranslation("tacticsunallowsubattackuseoption"), ToggleSubattackUsage);
            }
            md.AddOption(GetTranslation("nevermind"), NevermindTacticChangeDialogue);
            md.RunDialogue();
        }

        private static void ToggleHelpingOverFightingDialogue()
        {
            Speaker.Data.PrioritizeHelpingAlliesOverFighting = !Speaker.Data.PrioritizeHelpingAlliesOverFighting;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, Speaker.Data.PrioritizeHelpingAlliesOverFighting ? TacticsChangeContext.PrioritizeHelpingOverFighting : TacticsChangeContext.PrioritizeFightingOverHelping));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ToggleSubattackUsage()
        {
            Speaker.Data.UnallowAutoUseSubattacks = !Speaker.Data.UnallowAutoUseSubattacks;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, Speaker.Data.UnallowAutoUseSubattacks ? TacticsChangeContext.UnallowSubattackUsage : TacticsChangeContext.AllowSubattackUsage));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ToggleFollowAhead()
        {
            Speaker.Data.FollowAhead = !Speaker.Data.FollowAhead;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, Speaker.Data.FollowAhead ? TacticsChangeContext.FollowAhead : TacticsChangeContext.FollowBehind));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ToggleTakeOnCombat()
        {
            Speaker.Data.AvoidCombat = !Speaker.Data.AvoidCombat;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, Speaker.Data.AvoidCombat ? TacticsChangeContext.AvoidCombat : TacticsChangeContext.PartakeInCombat));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeCloseRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.CloseRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToCloseRange));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeMidRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.MidRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToMidRanged));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeLongRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.LongRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToLongRanged));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeStickCloseTactic()
        {
            Speaker.CombatTactic = CombatTactics.StickClose;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToStickClose));
            md.AddOption(GetTranslation("genericokay"), TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void NevermindTacticChangeDialogue()
        {
            TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.Nevermind));
        }

        public static void ToggleSharingChair()
        {
            Speaker.ShareChairWithPlayer = !Speaker.ShareChairWithPlayer;
            TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.OnToggleShareChairMessage(Speaker, Speaker.ShareChairWithPlayer));
        }

        public static void ToggleSharingBed()
        {
            Speaker.ShareBedWithPlayer = !Speaker.ShareBedWithPlayer;
            TalkAboutOtherTopicsDialogue(Speaker.GetDialogues.OnToggleShareBedsMessage(Speaker, Speaker.ShareBedWithPlayer));
        }

        private static void OnSayingNevermindOnTalkingAboutOtherTopics()
        {
            LobbyDialogue(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.Nevermind));
        }

        public static void CompanionSpeakMessage(Companion companion, string Message, Color DefaultColor = default(Color))
        {
            if(DefaultColor == default(Color)) DefaultColor = Color.White;
            companion.SaySomething(Message);
            string FinalMessage = ParseText("<" + companion.GetNameColored() + "> " + Message);
            Main.NewText(FinalMessage, DefaultColor);
        }

        #region Request Related Dialogues
        public static void TalkAboutRequests()
        {
            RequestData request = Speaker.GetRequest;
            MessageDialogue m = new MessageDialogue();
            retry:
            switch(request.status)
            {
                case RequestData.RequestStatus.Cooldown:
                    m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker,RequestContext.NoRequest));
                    m.AddOption(GetTranslation("genericokay"), LobbyDialogue);
                    break;
                case RequestData.RequestStatus.Ready:
                    request.PickNewRequest(MainMod.GetLocalPlayer, Speaker.Data);
                    goto retry;
                case RequestData.RequestStatus.WaitingAccept:
                    m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.HasRequest).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
                    m.AddOption(GetTranslation("requestacceptoption"), OnAcceptRequest);
                    m.AddOption(GetTranslation("requestpostponeoption"), OnPostponeRequest);
                    m.AddOption(GetTranslation("requestrejectoption"), OnRejectRequest);
                    break;
                case RequestData.RequestStatus.Active:
                    m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.AskIfRequestIsCompleted));
                    if (request.IsRequestCompleted())
                    {
                        request.GiveAtLeast30SecondsRequestTime();
                        m.AddOption(request.GetRequestRewardInfo(0), OnCompleteRequestFirstReward);
                        m.AddOption(request.GetRequestRewardInfo(1), OnCompleteRequestSecondReward);
                        m.AddOption(request.GetRequestRewardInfo(2), OnCompleteRequestThirdReward);
                    }
                    else
                    {
                        m.AddOption(GetTranslation("requestremindobjectiveoption"), OnRemindRequestObjectives);
                        m.AddOption(GetTranslation("requestcanceloption"), OnCancelRequestPrompt);
                        m.AddOption(GetTranslation("nevermind"), LobbyDialogue);
                    }
                    break;
            }
            m.RunDialogue();
        }

        private static void OnAcceptRequest()
        {
            MessageDialogue m = new MessageDialogue();
            if(!PlayerMod.PlayerCanTakeNewRequest(MainMod.GetLocalPlayer))
            {
                m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.TooManyRequests));
                m.AddOption(GetTranslation("requesttoomanyansweroption"), LobbyDialogue);
            }
            else
            {
                RequestData request = Speaker.GetRequest;
                request.ChangeRequestStatus(RequestData.RequestStatus.Active);
                MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().UpdateActiveRequests();
                m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.Accepted).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
                m.AddOption(GetTranslation("requestacceptconfirmoption"), LobbyDialogue);
            }
            m.RunDialogue();
        }

        private static void OnRejectRequest()
        {
            Speaker.GetRequest.SetRequestOnCooldown(true);
            LobbyDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.Rejected));
        }

        private static void OnPostponeRequest()
        {
            LobbyDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.PostponeRequest));
        }

        private static void OnCompleteRequestFirstReward()
        {
            OnCompleteRequestReward(0);
        }

        private static void OnCompleteRequestSecondReward()
        {
            OnCompleteRequestReward(1);
        }

        private static void OnCompleteRequestThirdReward()
        {
            OnCompleteRequestReward(2);
        }

        private static void OnCompleteRequestReward(byte Index)
        {
            RequestData request = Speaker.GetRequest;
            if (request.CompleteRequest(MainMod.GetLocalPlayer, Speaker.Data, Index))
            {
                MessageDialogue m = new MessageDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.Completed));
                //When the possibility of picking reward is added, this should let you pick one of them.
                m.AddOption(GetTranslation("requestcompleteansweroption"), LobbyDialogue);
                m.RunDialogue();
                MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().UpdateActiveRequests();
            }
            else
            {
                LobbyDialogue(GetTranslation("requestcompleteexceptionoption"));
            }
        }

        private static void OnRemindRequestObjectives()
        {
            RequestData request = Speaker.GetRequest;
            MessageDialogue m = new MessageDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.RemindObjective).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
            m.AddOption(GetTranslation("requestremindobjectivethanksoption"), LobbyDialogue);
            m.RunDialogue();
        }

        private static void OnCancelRequestPrompt()
        {
            MessageDialogue m = new MessageDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.CancelRequestAskIfSure));
            m.AddOption(GetTranslation("requestcancelyesoption"), OnCancelRequestYes);
            m.AddOption(GetTranslation("requestcancelnooption"), OnCancelRequestNo);
            m.RunDialogue();
        }

        private static void OnCancelRequestYes()
        {
            Speaker.GetRequest.SetRequestOnCooldown();
            LobbyDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.CancelRequestYes));
            MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().UpdateActiveRequests();
        }

        private static void OnCancelRequestNo()
        {
            LobbyDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.CancelRequestNo));
        }

        private static void ToggleControlScript()
        {
            if (!Speaker.PlayerCanControlCompanion(MainMod.GetLocalPlayer) && !Speaker.IsBeingControlledBy(MainMod.GetLocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.ControlMessage(Speaker, ControlContext.NotFriendsEnough));
                md.AddOption(GetTranslation("nevermind"), LobbyDialogue);
                md.RunDialogue();
                return;
            }
            HideControlMessage = true;
            if (Speaker.TogglePlayerControl(MainMod.GetLocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.ControlMessage(Speaker, Speaker.IsBeingControlledBySomeone ? ControlContext.SuccessTakeControl : ControlContext.SuccessReleaseControl));
                md.AddOption(GetTranslation("genericclose"), EndDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.ControlMessage(Speaker, Speaker.IsBeingControlledBySomeone ? ControlContext.FailReleaseControl : ControlContext.FailTakeControl));
                md.AddOption(GetTranslation("genericoops"), LobbyDialogue);
                md.RunDialogue();
            }
        }

        private static void ToggleFreeControl()
        {
            PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
            pm.CompanionFreeControl = !pm.CompanionFreeControl;
            MessageDialogue md = new MessageDialogue();
            if(pm.CompanionFreeControl)
            {
                md.ChangeMessage(Speaker.GetDialogues.ControlMessage(Speaker, ControlContext.GiveCompanionControl));
            }
            else
            {
                md.ChangeMessage(Speaker.GetDialogues.ControlMessage(Speaker, ControlContext.TakeCompanionControl));
            }
            md.AddOption(GetTranslation("genericclose"), EndDialogue);
            md.RunDialogue();
        }
        #endregion

        private static void TestDialogue() //If it wasn't for this script, this dialogue system wouldn't have existed.
        {
            MessageDialogue md = new MessageDialogue("*Thank you " + Main.LocalPlayer.name+ ", but the other TerraGuardians are in another realm.\nAt least I got [i/s1:357] Bowl of Soup.\n[gn:0] and [gn:1] are with you.*", new DialogueOption[0]);
            md.AddOption(new DialogueOption("Ok?", EndDialogue));
            md.AddOption(new DialogueOption("Give me some too", delegate(){ 
                MessageDialogue nmd = new MessageDialogue("*Of course I will give you one. Here it goes.*", new DialogueOption[0]);
                Main.LocalPlayer.QuickSpawnItem(Speaker.GetSource_GiftOrReward(), Terraria.ID.ItemID.BowlofSoup);
                nmd.AddOption("Yay!", delegate(){ EndDialogue(); });
                nmd.RunDialogue();
            }));
            md.AddOption(new DialogueOption("Tell me a story.", delegate()
            {
                MultiStepDialogue msd = new MultiStepDialogue();
                msd.AddDialogueStep("*There was this story about a Terrarian.*");
                msd.AddDialogueStep("*That Terrarian was wielding the legendary Zenith weapon.*", "Oh, wow!");
                msd.AddDialogueStep("*No, wait. It wasn't the Zenith. It was a Copper Shortsword.*");
                msd.AddDialogueStep("*Then, a giant creature came from the sky. The Terrible Moon Lord.*", "Cool!");
                msd.AddDialogueStep("*No, no. Wasn't the Moon Lord. I think was the Eye of Cthulhu.*");
                msd.AddDialogueStep("*No, wasn't Eye of Cthulhu either. Maybe was a Blue Slime.*");
                msd.AddDialogueStep("*The Terrarian sliced the Blue Slime in half with the Copper Shortsword, and saved the entire world.\nThe End.*");
                msd.AddOption("Amazing story!", delegate(){ 
                    new MessageDialogue("*Thank you, Thank you very much.*").RunDialogue();
                });
                msd.AddOption("Boo!", delegate(){ 
                    new MessageDialogue("*You didn't liked it? :(*").RunDialogue();
                });
                msd.RunDialogue();
            }));
            md.RunDialogue();
        }
    }
}