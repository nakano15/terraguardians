using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public partial class Dialogue
    {
        private static MessageBase[] LobbyMessage;

        public static void Load()
        {

        }

        private static void UnloadDefaultDialogues()
        {
            LobbyMessage = null;
        }
        
        public static void LobbyDialogue()
        {
            LobbyDialogue("");
        }

        public static void LobbyDialogue(string LobbyMessage = "")
        {
            bool TryAddingCompanion = true;
            WorldMod.AddCompanionMet(Speaker.Data);
            returnToLobby:
            if(Speaker.sleeping.isSleeping && Speaker.Base.SleepsWhenOnBed && !HasBeenAwakened)
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.SleepingMessage(Speaker, SleepingMessageContext.WhenSleeping));
                md.AddOption("Wake " + Speaker.GetPronoun() + " up.", WhenWakingUpCompanion);
                md.AddOption("Let " + Speaker.GetPronoun() + " sleep.", EndDialogue);
                md.RunDialogue();
                return;
            }
            if(TryAddingCompanion && !PlayerMod.PlayerHasCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
            {
                if (PlayerMod.PlayerAddCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
                {
                    if(Speaker.Index == 0 && Main.netMode == 0)
                        Speaker.Data = PlayerMod.PlayerGetCompanionData(Main.LocalPlayer, Speaker.ID, Speaker.ModID);
                    MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.GreetMessages(Speaker));
                    md.AddOption(new DialogueOption("Hello.", LobbyDialogue));
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
                string Message;
                if(Speaker.IsComfortPointsMaxed())
                {
                    Message = Speaker.GetDialogues.TalkMessages(Speaker);
                }
                else if (LobbyMessage != "")
                {
                    Message = LobbyMessage;
                }
                else
                {
                    Message = Speaker.GetDialogues.NormalMessages(Speaker);
                }
                MessageDialogue md = new MessageDialogue(Message);
                if(!HideJoinLeaveMessage && !Speaker.IsMountedOnSomething)
                {
                    if(!Speaker.IsFollower)
                        md.AddOption("Want to join my adventure?", JoinGroupMessage);
                    else if (Speaker.Owner == Main.LocalPlayer)
                        md.AddOption("Leave group.", LeaveGroupMessage);
                }
                if(Speaker.Owner == Main.LocalPlayer)
                {
                    if(Speaker.Base.MountStyle != MountStyles.CantMount)
                    {
                        if(!Speaker.IsMountedOnSomething)
                        {
                            string MountText = "May I mount on your shoulder?";
                            switch(Speaker.Base.MountStyle)
                            {
                                case MountStyles.CompanionRidesPlayer:
                                    MountText = "Can you mount on my shoulder?";
                                    break;
                            }
                            md.AddOption(MountText, MountMessage);
                        }
                        else
                        {
                            string DismountText = "Place me on the ground.";
                            switch(Speaker.Base.MountStyle)
                            {
                                case MountStyles.CompanionRidesPlayer:
                                    DismountText = "Get off my shoulder.";
                                    break;
                            }
                            md.AddOption(DismountText, DismountMessage);
                            if(Speaker.Base.MountStyle == MountStyles.PlayerMountsOnCompanion) MountedFurnitureCheckScripts(md); //I have to fix issues where characters mounted using this have bugs when using furniture at the first time.
                        }
                    }
                }
                string RequestsMessageText = "Do you have any requests?";
                switch(Speaker.GetRequest.status)
                {
                    case RequestData.RequestStatus.Active:
                        RequestsMessageText = "About your request.";
                        break;
                }
                md.AddOption(RequestsMessageText, TalkAboutRequests);
                md.AddOption("Let's talk about something else.", TalkAboutOtherTopicsDialogue);
                Speaker.GetGoverningBehavior().ChangeLobbyDialogueOptions(md, out bool ShowCloseButton);
                if(ShowCloseButton) md.AddOption(new DialogueOption("Goodbye", EndDialogue));
                md.RunDialogue();
            }
            //TestDialogue();
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
                md.AddOption("Let's rest here.", UseNearbyChairAction);
            }
            if(HasBed && Speaker.Base.AllowSharingBedWithPlayer)
            {
                md.AddOption("Let's get some sleep.", UseNearbyBedAction);
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
            WorldMod.AllowCompanionNPCToSpawn(Speaker);
            WorldMod.SetCompanionTownNpc(Speaker);
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.AskCompanionToMoveInMessage(Speaker, MoveInContext.Success));
            md.AddOption("Welcome, neighbor.", LobbyDialogue);
            md.RunDialogue();
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
            md.AddOption("I'm sorry.", LobbyDialogue);
            md.RunDialogue();
        }

        public static void JoinGroupMessage()
        {
            HideJoinLeaveMessage = true;
            if(!PlayerMod.PlayerHasEmptyFollowerSlot(Main.LocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.FullParty));
                md.AddOption("Aww...", LobbyDialogue);
                md.RunDialogue();
            }
            else if(PlayerMod.PlayerCallCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.Success));
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.JoinGroupMessages(Speaker, JoinMessageContext.Fail));
                md.AddOption("Sorry...", LobbyDialogue);
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
            else if(PlayerMod.PlayerDismissCompanionByIndex(Main.LocalPlayer, Speaker.Index, false))
            {
                if (Speaker.active == false)
                {
                    CompanionSpeakMessage(Speaker, Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    PlayerMod.EndDialogue(Main.LocalPlayer);
                }
                else
                {
                    MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    md.AddOption("Thank you.", EndDialogue);
                    md.RunDialogue();
                }
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.LeaveGroupMessages(Speaker, LeaveMessageContext.Fail));
                md.AddOption("Oh.", LobbyDialogue);
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
            if (!Speaker.PlayerCanMountCompanion(Main.LocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.NotFriendsEnough));
                md.AddOption("Okay.", LobbyDialogue);
                md.RunDialogue();
            }
            else if(Speaker.ToggleMount(Main.LocalPlayer))
            {
                string Mes = "";
                switch(Speaker.Base.MountStyle)
                {
                    default:
                        Mes = Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.Success);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.SuccessMountedOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.MountCompanionMessage(Speaker, MountCompanionContext.Fail));
                md.AddOption("Okay.", LobbyDialogue);
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
            if(Speaker.ToggleMount(Main.LocalPlayer))
            {
                string Mes = "";
                switch(Speaker.Base.MountStyle)
                {
                    default:
                        Mes = Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMount);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMountOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.DismountCompanionMessage(Speaker, DismountCompanionContext.Fail));
                md.AddOption("Okay.", LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void TalkAboutOtherTopicsDialogue()
        {
            TalkAboutOtherTopicsDialogue(""); //Empty string means default talk messages
        }

        public static void TalkAboutOtherTopicsDialogue(string Message = "")
        {
            MessageDialogue md = new MessageDialogue(Message);
            if (Message == "")
            {
                if(Dialogue.NotFirstTalkAboutOtherMessage)
                    md.ChangeMessage(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.AfterFirstTime));
                else
                    md.ChangeMessage(Speaker.GetDialogues.TalkAboutOtherTopicsMessage(Speaker, TalkAboutOtherTopicsContext.FirstTimeInThisDialogue));
            }
            Dialogue.NotFirstTalkAboutOtherMessage = true;
            if (!HideMovingMessage)
            {
                if(!Speaker.IsTownNpc)
                {
                    md.AddOption("Would you like to live here?", AskToMoveInMessage);
                }
                else
                {
                    md.AddOption("I need you to move out.", AskToMoveOutMessage);
                }
            }
            md.AddOption("Let's review how you will act in combat.", ChangeTacticsTopicDialogue);
            if(Speaker.Base.AllowSharingChairWithPlayer)
            {
                md.AddOption(!Speaker.ShareChairWithPlayer ? (Speaker.Base.MountStyle == MountStyles.CompanionRidesPlayer ? "Mind sitting on my lap when I use a chair?" : "Mind if I sit on your lap, when you use a chair?") : "Take another chair when I sit.", ToggleSharingChair);
                md.AddOption(!Speaker.ShareBedWithPlayer ? "Mind sharing the same bed?" : "I want you to sleep on another bed.", ToggleSharingBed);
            }
            if (Speaker is TerraGuardian) md.AddOption(Speaker.PlayerSizeMode ? "Get back to your size." : "Could you be of my size?", TogglePlayerSize);
            md.AddOption("Nevermind", OnSayingNevermindOnTalkingAboutOtherTopics);
            md.RunDialogue();
        }

        public static void TogglePlayerSize()
        {
            Speaker.PlayerSizeMode = !Speaker.PlayerSizeMode;
            TalkAboutOtherTopicsDialogue();
        }

        public static void ChangeTacticsTopicDialogue()
        {
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.OnAskToChangeTactic) + "\n[Current Tactic: "+Speaker.CombatTactic.ToString()+"]");
            if(Speaker.CombatTactic != CombatTactics.CloseRange)
                md.AddOption("Attack your targets by close range.", ChangeCloseRangeTactic);
            if(Speaker.CombatTactic != CombatTactics.MidRange)
                md.AddOption("Avoid contact with your targets.", ChangeMidRangeTactic);
            if(Speaker.CombatTactic != CombatTactics.LongRange)
                md.AddOption("Attack your targets from far away.", ChangeLongRangeTactic);
            md.AddOption("Nevermind", NevermindTacticChangeDialogue);
            md.RunDialogue();
        }

        private static void ChangeCloseRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.CloseRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToCloseRange));
            md.AddOption("Okay", TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeMidRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.MidRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToMidRanged));
            md.AddOption("Okay", TalkAboutOtherTopicsDialogue);
            md.RunDialogue();
        }

        private static void ChangeLongRangeTactic()
        {
            Speaker.CombatTactic = CombatTactics.LongRange;
            MessageDialogue md = new MessageDialogue(Speaker.GetDialogues.TacticChangeMessage(Speaker, TacticsChangeContext.ChangeToLongRanged));
            md.AddOption("Okay", TalkAboutOtherTopicsDialogue);
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
                    m.AddOption("Ok", LobbyDialogue);
                    break;
                case RequestData.RequestStatus.Ready:
                    request.PickNewRequest(MainMod.GetLocalPlayer, Speaker.Data);
                    goto retry;
                case RequestData.RequestStatus.WaitingAccept:
                    m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.HasRequest).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
                    m.AddOption("I'll take it.", OnAcceptRequest);
                    m.AddOption("Maybe another time.", OnPostponeRequest);
                    m.AddOption("I refuse to do it.", OnRejectRequest);
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
                        m.AddOption("I forgot what I had to do.", OnRemindRequestObjectives);
                        m.AddOption("Nevermind.", LobbyDialogue);
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
                m.AddOption("I'll take care of my other requests...", LobbyDialogue);
            }
            else
            {
                RequestData request = Speaker.GetRequest;
                request.ChangeRequestStatus(RequestData.RequestStatus.Active);
                m.ChangeMessage(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.Accepted).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
                m.AddOption("Alright.", LobbyDialogue);
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
                m.AddOption("No problem.", LobbyDialogue);
                m.RunDialogue();
            }
            else
            {
                LobbyDialogue("(This wasn't supposed to happen.)");
            }
        }

        private static void OnRemindRequestObjectives()
        {
            RequestData request = Speaker.GetRequest;
            MessageDialogue m = new MessageDialogue(Speaker.GetDialogues.RequestMessages(Speaker, RequestContext.RemindObjective).Replace("[objective]", request.GetBase.GetBriefObjective(request)));
            m.AddOption("Thanks for the reminder.", LobbyDialogue);
            m.RunDialogue();
        }
        #endregion

        private static void TestDialogue()
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