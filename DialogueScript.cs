using System.Collections.Generic;
using Terraria;
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
        public static ReLogic.Graphics.DynamicSpriteFont GetDialogueFont => 
            FontAssets.MouseText.Value;

        public static bool InDialogue = false;
        private static BitsByte _dialogueFlags = 0;
        public static bool HideJoinLeaveMessage { get { return _dialogueFlags[0]; } set {_dialogueFlags[0] = value; } }
        public static bool HideMovingMessage {get { return _dialogueFlags[1]; } set { _dialogueFlags[1] = value; } }
        public static bool NotFirstTalkAboutOtherMessage {get { return _dialogueFlags[2]; } set { _dialogueFlags[2] = value; } }
        public static bool HasBeenAwakened {get { return _dialogueFlags[3]; } set { _dialogueFlags[3] = value; } }
        public static bool HideMountMessage {get { return _dialogueFlags[4]; } set { _dialogueFlags[4] = value; } }
        public static bool HideControlMessage {get { return _dialogueFlags[5]; } set { _dialogueFlags[5] = value; } }
        public static Companion Speaker;
        private static Companion DialogueStarterSpeaker;
        public static List<Companion> DialogueParticipants = new List<Companion>();
        public static List<TextSnippet[]> Message = null;
        public static MessageBase CurrentMessage { get; private set;}
        public static DialogueOption[] Options { get; private set; }
        private static DialogueOption[] DefaultClose = new DialogueOption[]{ new DialogueOption("Close", EndDialogue) };
        public static DialogueOption[] GetDefaultCloseDialogue { get{ return DefaultClose; } }
        private static byte ImportantUnlockMessagesToCheck = 0;
        public static float DistancingLeft = 0, DistancingRight = 0;

        public static void Unload()
        {
            Message = null;
            CurrentMessage = null;
            Options = null;
            Speaker = null;
            DialogueStarterSpeaker = null;
            DialogueParticipants.Clear();
            DialogueParticipants = null;
            DefaultClose = null;
            UnloadDefaultDialogues();
        }

        public static void Update()
        {
            DistancingLeft = DistancingRight = 0;
        }

        public static bool IsParticipatingDialogue(Companion companion)
        {
            return DialogueParticipants.Contains(companion);
        }

        public static void StartDialogue(Companion Target)
        {
            if (!PlayerMod.PlayerTalkWith(MainMod.GetLocalPlayer, Target))
                return;
            DialogueStarterSpeaker = Speaker = Target;
            DialogueParticipants.Clear();
            DialogueParticipants.Add(Target);
            ChangeDialogueMessage("");
            ChangeOptions(DefaultClose);
            Main.playerInventory = false;
            InDialogue = true;
            _dialogueFlags = 0;
            ImportantUnlockMessagesToCheck = 1;
            Speaker.GetDialogues.OnStartDialogue();
            GetInitialDialogue();
        }

        private static void GetInitialDialogue()
        {
            MessageBase message = null;
            foreach (QuestData d in nterrautils.PlayerMod.GetPlayerQuests(MainMod.GetLocalPlayer))
            {
                if (d.Base is QuestBase)
                {
                    message = (d.Base as QuestBase).ImportantDialogueMessage(d, Speaker);
                    if (message != null)
                    {
                        message.RunDialogue();
                        return;
                    }
                }
            }
            if(!Speaker.HasBeenMet && Speaker.preRecruitBehavior != null)
            {
                message = Speaker.preRecruitBehavior.ChangeStartDialogue(Speaker);
                if(message != null)
                {
                    message.RunDialogue();
                    return;
                }
            }
            message = Speaker.idleBehavior.ChangeStartDialogue(Speaker);
            if(message != null)
            {
                message.RunDialogue();
                return;
            }
            LobbyDialogue();
        }

        public static void AddParticipant(Companion Participant)
        {
            DialogueParticipants.Add(Participant);
        }

        //Always call after setting up message infos.
        public static void ChangeMessage(MessageBase NewMessage)
        {
            CurrentMessage = NewMessage;
            NewMessage.OnDialogueTrigger();
        }

        public static void ChangeDialogueMessage(string NewDialogue)
        {
            if(NewDialogue == "")
            {
                if(Message == null)
                    Message = new List<TextSnippet[]>();
                else
                    Message.Clear();
                return;
            }
            //MessageContainer.SetContents(NewDialogue, Color.White, CompanionDialogueInterface.DialogueWidth - 16);
            //Message = Utils.WordwrapString(NewDialogue, FontAssets.MouseText.Value, CompanionDialogueInterface.DialogueWidth, 10, out DialogueLines);
            List<List<TextSnippet>> Parsed = Utils.WordwrapStringSmart(ParseText(NewDialogue), Color.White, GetDialogueFont, CompanionDialogueInterface.DialogueWidth - 16, 10);
            Message = new List<TextSnippet[]>();
            foreach(List<TextSnippet> Text in Parsed)
            {
                Message.Add(Text.ToArray());
            }
        }

        public static void ChangeCurrentSpeaker(Companion NewSpeaker)
        {
            if(NewSpeaker == null)
                Speaker = DialogueStarterSpeaker;
            else
                Speaker = NewSpeaker;
        }

        public static void ChangeOptions(List<DialogueOption> NewOptions)
        {
            ChangeOptions(NewOptions.ToArray());
        }

        public static void ChangeOptions(DialogueOption[] NewOptions)
        {
            foreach(DialogueOption o in NewOptions) o.ParseText();
            Options = NewOptions;
        }

        public static void EndDialogue()
        {
            InDialogue = false;
            PlayerMod pm = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            if(pm.TalkPlayer != null)
            {
                pm.TalkPlayer.GetModPlayer<PlayerMod>().TalkPlayer = null;
            }
            pm.TalkPlayer = null;
            Message.Clear();
            Message = null;
            Options = null;
        }

        public static string ParseText(string Text)
        {
            Companion Controlled = PlayerMod.PlayerGetControlledCompanion(MainMod.GetLocalPlayer);
            string ControlledcompanionName = Controlled != null ? Controlled.GetNameColored() : "Nobody";
            Text = Text
                .Replace("[name]", Speaker.GetNameColored())
                .Replace("[nickname]", Speaker.GetPlayerNickname(MainMod.GetLocalPlayer))
                .Replace("[playername]", Main.LocalPlayer.name)
                .Replace("[controlled]", ControlledcompanionName)
                .Replace("[tggodname]", MainMod.TgGodName)
                .Replace("[pronoun]", Speaker.GetPronoun())
                .Replace("[buddy]", PlayerMod.GetPlayerBuddy(MainMod.GetLocalPlayer) != null ? PlayerMod.GetPlayerBuddy(MainMod.GetLocalPlayer).GetNameColored() : "???");
            string FinalMessage = "";
            string CommandType = "", CommandValue = "", CommandValue2 = "";
            string EntireCommand = "";
            byte GettingCommandStep = 0;
            const byte ParsingCommand = 1, GettingValue = 2, GettingValue2 = 3;
            PlayerMod pm = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            for(int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                if (c == '[')
                {
                    GettingCommandStep = ParsingCommand;
                    CommandType = "";
                    CommandValue = "";
                    CommandValue2 = "";
                    EntireCommand = c.ToString();
                }
                else if (c == ']')
                {
                    GettingCommandStep = 0;
                    switch(CommandType)
                    {
                        case "gn":
                        case "cn":
                            {
                                uint cid = uint.Parse(CommandValue);
                                string mid = CommandValue2;
                                if(pm.HasCompanion(cid, mid))
                                {
                                    FinalMessage += pm.GetCompanionData(cid, mid).GetNameColored();
                                }
                                else
                                {
                                    FinalMessage += MainMod.GetCompanionBase(cid, mid).GetNameColored();
                                }
                            }
                            break;
                        case "nn":
                            {
                                int npcid = int.Parse(CommandValue);
                                string NpcName = "Unknown";
                                if (NPC.AnyNPCs(npcid))
                                {
                                    NpcName = Main.npc[NPC.FindFirstNPC(npcid)].GivenOrTypeName;
                                }
                                else
                                {
                                    NPC n = new NPC();
                                    n.SetDefaults(npcid);
                                    NpcName = n.TypeName;
                                }
                                MainMod.SetGenderColoring(MainMod.IsNpcFemale(npcid) ? Genders.Female : Genders.Male, ref NpcName);
                                FinalMessage += NpcName;
                            }
                            break;
                        case "name":
                            {
                                FinalMessage += Speaker.name;
                            }
                            break;
                        default:
                            {
                                FinalMessage += EntireCommand + ']';
                            }
                            break;
                    }
                }
                else if (GettingCommandStep > 0)
                {
                    if (c == ':')
                    {
                        switch(GettingCommandStep)
                        {
                            case ParsingCommand:
                                GettingCommandStep = GettingValue;
                                CommandValue = "";
                                break;
                            case GettingValue:
                                GettingCommandStep = GettingValue2;
                                CommandValue2 = "";
                                break;
                        }
                    }
                    else
                    {
                        switch(GettingCommandStep)
                        {
                            case ParsingCommand:
                                CommandType += c;
                                break;
                            case GettingValue:
                                CommandValue += c;
                                break;
                            case GettingValue2:
                                CommandValue2 += c;
                                break;
                        }
                    }
                    EntireCommand += c;
                }
                else
                {
                    FinalMessage += c;
                }
            }
            return FinalMessage;
        }
    }
}