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
            bool TryAddingCompanion = true;
            WorldMod.AddCompanionMet(Speaker.Data);
            returnToLobby:
            if(TryAddingCompanion && !PlayerMod.PlayerHasCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
            {
                if (PlayerMod.PlayerAddCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
                {
                    if(Speaker.Index == 0 && Main.netMode == 0)
                        Speaker.Data = PlayerMod.PlayerGetCompanionData(Main.LocalPlayer, Speaker.ID, Speaker.ModID);
                    MessageDialogue md = new MessageDialogue(Speaker.Base.GreetMessages(Speaker));
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
                MessageDialogue md = new MessageDialogue(Speaker.Base.NormalMessages(Speaker));
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
                        }
                    }
                }
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
                Speaker.GetGoverningBehavior().ChangeLobbyDialogueOptions(md, out bool ShowCloseButton);
                if(ShowCloseButton) md.AddOption(new DialogueOption("Goodbye", EndDialogue));
                md.RunDialogue();
            }
            //TestDialogue();
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
            MessageDialogue md = new MessageDialogue(Speaker.Base.AskCompanionToMoveInMessage(Speaker, MoveInContext.Success));
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
            MessageDialogue md = new MessageDialogue(Speaker.Base.AskCompanionToMoveOutMessage(Speaker, MoveOutContext.Success));
            md.AddOption("I'm sorry.", LobbyDialogue);
            md.RunDialogue();
        }

        public static void JoinGroupMessage()
        {
            HideJoinLeaveMessage = true;
            if(!PlayerMod.PlayerHasEmptyFollowerSlot(Main.LocalPlayer))
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.JoinGroupMessages(Speaker, JoinMessageContext.FullParty));
                md.AddOption("Aww...", LobbyDialogue);
                md.RunDialogue();
            }
            else if(PlayerMod.PlayerCallCompanion(Main.LocalPlayer, Speaker.ID, Speaker.ModID))
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.JoinGroupMessages(Speaker, JoinMessageContext.Success));
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.JoinGroupMessages(Speaker, JoinMessageContext.Fail));
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
                    CompanionSpeakMessage(Speaker, Speaker.Base.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    PlayerMod.EndDialogue(Main.LocalPlayer);
                }
                else
                {
                    MessageDialogue md = new MessageDialogue(Speaker.Base.LeaveGroupMessages(Speaker, LeaveMessageContext.Success));
                    md.AddOption("Thank you.", EndDialogue);
                    md.RunDialogue();
                }
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.LeaveGroupMessages(Speaker, LeaveMessageContext.Fail));
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
            if(Speaker.ToggleMount(Main.LocalPlayer))
            {
                string Mes = "";
                switch(Speaker.Base.MountStyle)
                {
                    default:
                        Mes = Speaker.Base.MountCompanionMessage(Speaker, MountCompanionContext.Success);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.Base.MountCompanionMessage(Speaker, MountCompanionContext.SuccessMountedOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.MountCompanionMessage(Speaker, MountCompanionContext.Fail));
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
                        Mes = Speaker.Base.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMount);
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        Mes = Speaker.Base.DismountCompanionMessage(Speaker, DismountCompanionContext.SuccessMountOnPlayer);
                        break;
                }
                MessageDialogue md = new MessageDialogue(Mes);
                md.AddOption("Thanks.", LobbyDialogue);
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue(Speaker.Base.DismountCompanionMessage(Speaker, DismountCompanionContext.Fail));
                md.AddOption("Okay.", LobbyDialogue);
                md.RunDialogue();
            }
        }

        public static void CompanionSpeakMessage(Companion companion, string Message, Color DefaultColor = default(Color))
        {
            if(DefaultColor == default(Color)) DefaultColor = Color.White;
            string FinalMessage = ParseText("<" + companion.GetNameColored() + "> " + Message);
            Main.NewText(FinalMessage, DefaultColor);
        }

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