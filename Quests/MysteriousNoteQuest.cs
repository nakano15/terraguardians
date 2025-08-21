using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;
using nterrautils;

namespace terraguardians.Quests
{
    public class MysteriousNoteQuest : QuestBase
    {
        public override string Name => "The Mysterious Note";
        public override QuestData GetQuestData => new MysteriousNoteQuestData();

        public override bool IsQuestActive(QuestData data)
        {
            return (data as MysteriousNoteQuestData).QuestStage >= MysteriousNoteQuestData.Stages.Started;
        }

        public override bool IsQuestCompleted(QuestData data)
        {
            return (data as MysteriousNoteQuestData).QuestStage >= MysteriousNoteQuestData.Stages.Finished;
        }

        public override string QuestStory(QuestData rawdata)
        {
            string Text = "";
            MysteriousNoteQuestData.Stages Stage = (rawdata as MysteriousNoteQuestData).QuestStage;
            if (Stage == MysteriousNoteQuestData.Stages.FinishedNoStart)
            {
                return "While saying \"Daphne\" in the open, a white dog with a halo on the head appeared, approached me, and then barked while wagging her tail.\n\nI think I might have just made a new friend.\n\nTHE END";
            }
            else
            {
                switch(Stage)
                {
                    case MysteriousNoteQuestData.Stages.FinishedNoStart:
                        return "While saying \"Daphne\" in the open, a white dog with a halo on the head appeared, approached me, and then barked while wagging her tail.\nHer leash said \"Daphne\", so I believe I might have ended up calling her.\n\nSeems like I just ended up befriending someone new.\n\nTHE END";
                    default:
                        Text = "When I tried to speak with Rococo, he told me that found a mysterious note, and then gave it to me.\n\nUpon inspecting the note, it said the following: \n\"There's a guardian angel called 'Daphne'.\nShe will show up if you invoke her name.\"";
                        if (Stage == MysteriousNoteQuestData.Stages.Finished)
                            Text += "\n\nTo my surprise, when I said that name, I felt like I have summoned something. As I looked, there was that white dog with a halo on the head, coming towards me.\nShe came close, barked at me while wagging her tail.\nIt seems like I've just made a new friend.\n\nTHE END";
                        break;
                }
            }
            return Text;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            return "Follow the clue on the Mysterious Note.";
        }

        public override MessageBase ImportantDialogueMessage(QuestData rawdata, Companion companion)
        {
            if (companion.IsSameID(CompanionDB.Rococo) && companion.FriendshipLevel >= 5)
            {
                MysteriousNoteQuestData data = rawdata as MysteriousNoteQuestData;
                if (data.QuestStage == MysteriousNoteQuestData.Stages.NotStarted)
                {
                    return GetRococosMessage();
                }
            }
            return base.ImportantDialogueMessage(rawdata, companion);
        }

        MessageDialogue GetRococosMessage()
        {
            MessageDialogue md = new MessageDialogue();
            bool InventoryIsFull = true;
            for (int i = 0; i < 50; i++)
            {
                if (MainMod.GetLocalPlayer.inventory[i].type == ItemID.None)
                {
                    InventoryIsFull = false;
                    break;
                }
            }
            if (InventoryIsFull)
            {
                md.ChangeMessage("*[name] tells you that he wanted to give you something, but can't do that while your inventory is full.*");
                md.AddOption("Okay.", Dialogue.LobbyDialogue);
            }
            else
            {
                md.ChangeMessage("*[name] gave you some odd note he found.\nHe thought it might interest you.*");
                md.AddOption("Thanks.", OnTakeNoteDialogue);
            }
            return md;
        }

        void OnTakeNoteDialogue()
        {
            Item.NewItem(Item.GetSource_NaturalSpawn(), MainMod.GetLocalPlayer.Center, Vector2.One, Terraria.ModLoader.ModContent.ItemType<Items.Misc.Note>(), noBroadcast: true, noGrabDelay: true);
            MysteriousNoteQuestData data = (MysteriousNoteQuestData)Data;
            data.QuestStage = MysteriousNoteQuestData.Stages.Started;
            data.ShowQuestStartedNotification();
            Dialogue.LobbyDialogue();
        }

        internal static void CompleteOnDaphneArrive()
        {
            MysteriousNoteQuestData data = (MysteriousNoteQuestData)nterrautils.PlayerMod.GetPlayerQuestData(MainMod.GetLocalPlayer, QuestDB.Mysterious_Note, MainMod.GetModName);
            if (data.QuestStage < MysteriousNoteQuestData.Stages.Finished)
                data.ShowQuestCompletedNotification();
            if (data.QuestStage == MysteriousNoteQuestData.Stages.Started)
                data.QuestStage = MysteriousNoteQuestData.Stages.Finished;
            else if (data.QuestStage == MysteriousNoteQuestData.Stages.NotStarted)
                data.QuestStage = MysteriousNoteQuestData.Stages.FinishedNoStart;
        }
        
        public class MysteriousNoteQuestData : QuestData
        {
            public override ushort Version => 1;
            public Stages QuestStage = 0;

            protected override void Save(TagCompound save, string QuestID)
            {
                save.Add(QuestID + "_Stage", (byte)QuestStage);
            }

            protected override void Load(TagCompound load, string QuestID, ushort LastVersion)
            {
                if (LastVersion > 0)
                {
                    QuestStage = (Stages)load.GetByte(QuestID + "_Stage");
                }
            }

            public enum Stages : byte
            {
                NotStarted = 0,
                Started = 1,
                Finished = 2,
                FinishedNoStart = 3
            }
        }
    }
}