using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;
using nterrautils;
using System.Security.AccessControl;
using Microsoft.Extensions.Options;
using Mono.Cecil;
using terraguardians.Companions;

namespace terraguardians.Quests
{
    public class BreeStayQuest : QuestBase
    {
        public override string Name => "Stay";
        public override QuestData GetQuestData => new BreeStayData();
        public const short FishID = ItemID.VariegatedLardfish;
        public const byte FishCount = 15;
        public const string FishName = "Variegated Lardfish";

        public override string QuestStory(QuestData rawdata)
        {
            BreeStayData data = (BreeStayData)rawdata;
            string Story = "Bree told me that many people in this world are asking her to stay.\n"+
            "She seems to be really annoyed by that, and said that if I want her to stay in my world, I should bring her " + FishCount + " " + FishName + ".";
            if (data.QuestStep >= 2)
            {
                Story += "\n\n";
                Story += "I brought her the fish she asked for, and she looked really happy for that.\n";
                switch (data.ConclusionState)
                {
                    case 0:
                        Story += "She was sad knowing that she would eat them alone, because her husband and son are nowhere to be found.";
                        break;
                    case 1:
                        Story += "She will cook the fish to dine alongside her husband, but she was partially unhappy that her son will not be there to enjoy the diner too.\nShe then begun wondering about her son's well being at home...";
                        break;
                    case 2:
                        Story += "She will cook the fish to dine alongside her son, but the worry about the whereabouts of her husband, will keep both of them from enjoying their dinner.";
                        break;
                    case 3:
                        Story += "She will cook the fish to dine alongside her husband and son, and she'll feel happy that everyone will enjoy their dinner together again.";
                        break;
                }
            }
            if (data.QuestStep == 3)
            {
                Story += "\n\nLater that day, Bree untied the bag off her neck, and stored all her belongs on the house, which she now call hers.";
                Story += "\n\nTHE END";
            }
            return Story;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            BreeStayData Data = (BreeStayData)data;
            if (Data.QuestStep == 2)
                return "Bree must return home to think.";
            int FishsToGive = FishCount - Data.FishGiven;
            return "Give " + FishsToGive + " " + FishName + " to Bree,\nso you can convince her to stay.";
        }

        public override bool IsQuestActive(QuestData data)
        {
            BreeStayData Data = (BreeStayData)data;
            return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Bree) && Data.QuestStep > 0;
        }

        public override bool IsQuestCompleted(QuestData data)
        {
            return (data as BreeStayData).QuestStep == 3;
        }

        public override void UpdatePlayer(Player player, QuestData data)
        {
            BreeStayData Data = (BreeStayData)data;
            if (Data.QuestStep == 2)
            {
                if (!Main.dayTime && Main.time >= 1800 && Main.rand.Next(20) == 0 && WorldMod.HasCompanionNPCSpawned(CompanionDB.Bree) && !PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Bree))
                {
                    if (WorldMod.GetCompanionNpc(CompanionDB.Bree).IsAtHome)
                    {
                        Data.QuestStep = 3;
                        Data.ShowQuestCompletedNotification();
                    }
                }
            }
        }

        public override MessageBase ImportantDialogueMessage(QuestData data, Companion companion)
        {
            if (companion.IsSameID(CompanionDB.Bree) && companion.FriendshipLevel >= 5 && (data as BreeStayData).QuestStep == 0)
            {
                return BreeDialogueQuestStart();
            }
            return base.ImportantDialogueMessage(data, companion);
        }

        MultiStepDialogue BreeDialogueQuestStart()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("We have to talk now, [nickname], and I don't care if you're busy or not.");
            md.AddDialogueStep("Everyone keeps nagging me to stay here. I already said that I will go back home once I find what world my house is situated.");
            md.AddDialogueStep("If you really want me to stay too, maybe I could consider that, if you bring me " + FishCount + " " + FishName + ".");
            md.AddDialogueStep("Otherwise, don't come nagging me with that anymore.");
            md.AddOption("Got it.", OnTakeQuest);
            return md;
        }

        void OnTakeQuest()
        {
            (Data as BreeStayData).QuestStep = 1;
            Data.ShowQuestStartedNotification();
            Dialogue.LobbyDialogue("Now that that's out of the way, is there something else you want to talk about?");
        }

        public override void AddDialogueOptions(QuestData data, bool IsTalkDialogue, Companion companion, MessageDialogue message)
        {
            if (companion.IsSameID(CompanionDB.Bree))
            {
                BreeStayData d = data as BreeStayData;
                if (d.QuestStep == 1)
                {
                    if (Main.LocalPlayer.HasItem(FishID))
                    {
                        message.AddOption("I've got the fish.", OnGiveFishsToBree);
                    }
                    else
                    {
                        message.AddOption("What kind of fish you want?", OnAskWhichFishSheLooksFor);
                    }
                }
            }
        }

        void OnGiveFishsToBree()
        {
            MessageDialogue md = new MessageDialogue("You got the fish? Let me see them.");
            md.AddOption("Continue", PostGiveFishs);
            md.RunDialogue();
        }

        void PostGiveFishs()
        {
            BreeStayData data = Data as BreeStayData;
            byte Count = 0;
            Player player = MainMod.GetLocalPlayer;
            for (int i = 0; i < 50; i++)
            {
                if (player.inventory[i].type == FishID)
                {
                    if (player.inventory[i].stack + Count > byte.MaxValue)
                    {
                        Count = 255;
                        break;
                    }
                    else
                    {
                        Count += (byte)player.inventory[i].stack;
                        if (Count >= FishCount) break;
                    }
                }
            }
            MultiStepDialogue md = new MultiStepDialogue();
            if (Count <= FishCount)
            {
                md.AddDialogueStep("Ah, so you've got " + Count + " of them.");
            }
            else
            {
                md.AddDialogueStep("Wow, you've got quite a lot of them.");
            }
            byte Needed = (byte)(FishCount - data.FishGiven);
            if (Needed < Count)
            {
                md.AddDialogueStep("I only need " + Needed + " of them, you can keep the rest, lets not get greedy.");
                Count = Needed;
            }
            else if (Needed > Count)
            {
                md.AddDialogueStep("It's not enough. Still need "+(FishCount - data.FishGiven - Count)+" more fish. But you can always bring me more.");
            }
            else
            {
                md.AddDialogueStep("Ah, that's the necessary amount I was needing.");
            }
            data.FishGiven += Count;
            for (int i = 0; i < 50; i++)
            {
                if (player.inventory[i].type == FishID)
                {
                    int StackToRemove = player.inventory[i].stack;
                    if (StackToRemove > Count)
                        StackToRemove = Count;
                    player.inventory[i].stack -= StackToRemove;
                    Count -= (byte)StackToRemove;
                    if (player.inventory[i].stack == 0)
                        player.inventory[i].SetDefaults(0);
                    if (Count <= 0) break;
                }
            }
            if (data.FishGiven >= FishCount)
            {
                data.QuestStep = 2;
                bool HasSardine = PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Sardine);
                bool HasGlenn = PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Glenn);
                if (HasSardine && HasGlenn)
                {
                    data.ConclusionState = 3;
                    md.AddDialogueStep("My family and I will enjoy eating those as dinner for about two weeks.");
                }
                else if (HasGlenn)
                {
                    data.ConclusionState = 2;
                    md.AddDialogueStep("My son and I will enjoy eating those during dinner for two weeks, but I really wanted my husband to be here too.");
                }
                else if (HasSardine)
                {
                    data.ConclusionState = 1;
                    md.AddDialogueStep("My husband and I will enjoy eating those during dinner for two weeks, but my son should also be here dinning with us...");
                }
                else
                {
                    data.ConclusionState = 0;
                    md.AddDialogueStep("This will give me a nice dinner for 2 weeks, but I really wish my family were here for dinner too...");
                }
                md.AddDialogueStep("Yes, I know that I promissed to stay here if you brought me all those fish. You don't need to remind me of that.");
                md.AddDialogueStep("I'll store away the things on my bag at my house when I get back home.");
                md.AddDialogueStep("At least, I will be able to relieve my back from this heavy bag.");
            }
            else
            {
                md.AddDialogueStep("Bring me the fish that are left if you want me to stay.");
            }
            md.AddOption("Ok.", EndPostGiveDialogue);
            md.RunDialogue();
        }

        void EndPostGiveDialogue()
        {
            Dialogue.LobbyDialogue("Is there anything else you want to talk to me?");
        }

        void OnAskWhichFishSheLooksFor()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("You forgot already? I asked you to bring me "+FishCount+" "+FishName+".");
            BreeStayData data = (BreeStayData)Data;
            if (data.FishGiven > 0)
            {
                int NewFishCount = FishCount - data.FishGiven;
                md.AddDialogueStep("Actually, since you've already gave me some, you only need to bring me " + NewFishCount + " of them.");
            }
            md.AddDialogueStep("I really hope you don't forget, because I don't like repeating myself.");
            md.AddOption("Understud.", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        public class BreeStayData : QuestData
        {
            public byte QuestStep = 0, FishGiven = 0;
            public byte ConclusionState = 0;
            public override ushort Version => base.Version;

            protected override void Save(TagCompound save, string QuestID)
            {
                save.Add("qstep_"+QuestID, QuestStep);
                save.Add("qfish_"+QuestID, FishGiven);
                save.Add("qconc_"+QuestID, ConclusionState);
            }

            protected override void Load(TagCompound load, string QuestID, ushort LastVersion)
            {
                QuestStep = load.GetByte("qstep_"+QuestID);
                FishGiven = load.GetByte("qfish_"+QuestID);
                ConclusionState = load.GetByte("qconc_"+QuestID);
            }
        }
    }
}