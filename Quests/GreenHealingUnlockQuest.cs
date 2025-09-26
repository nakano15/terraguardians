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
    public class GreenHealingUnlockQuest : QuestBase
    {
        public override string Name => "Knowing You Inside";
        public override QuestData GetQuestData => new GreenQuestData();
        bool CanStartQuest => NPC.downedBoss3;

        public override string QuestStory(QuestData data)
        {
            GreenQuestData Data = data as GreenQuestData;
            string Story = "";
            if (Data.QuestStep == 0 && Data.TimePassed > 0)
            {
                Story = "When I spoke with Green, he said that wanted to speak with me, and that couldn't treat Terrarians while that matter is not done.";
            }
            else if (Data.QuestStep > 0)
            {
                Story = "Green told me that he need to read books Terrarian anathomy, diseases and medication. so he could begin treating Terrarians aswell, with reduced chances of mistake.\nI gave the suggestion that the Dungeon might contain such books, so he recommended me to look for them there.";
            }
            if (Data.QuestStep > 1)
                Story += "\n\nI've brought him quite a number of books, which a number of them seems to be of what he wanted to know.";
            if (Data.QuestStep > 3)
                Story += " Once I brought all the books he asked for, he asked me to wait a few days until he finishes reading them all.";
            if (Data.QuestStep == 5)
            {
                Story += "\n\nAfter that time, Green told me that he managed to read all the books, and that now has better knowledge about Terrarians body, diseases and known treatments. And he also told me that can begin treating Terrarian patients wherever he goes, and for a fair fee.";
            }
            return Story;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            switch ((data as GreenQuestData).QuestStep)
            {
                case 0:
                    return "Ask Green about what he need.";
                case 1:
                case 2:
                case 3:
                    return "Bring Books from the Dungeon to Green.";
                case 4:
                    return "Wait until Green finishes reading the books.";
            }
            return base.GetQuestCurrentObjective(data);
        }

        public override bool IsQuestActive(QuestData data)
        {
            GreenQuestData Data = (GreenQuestData)data;
            return Data.QuestStep > 0 || Data.QuestStep == 0 && Data.TimePassed > 0;
        }

        public override bool IsQuestCompleted(QuestData data)
        {
            return (data as GreenQuestData).QuestStep >= 5;
        }

        public override MessageBase ImportantDialogueMessage(QuestData data, Companion companion)
        {
            if (!data.IsCompleted && companion.IsSameID(CompanionDB.Green))
            {
                GreenQuestData Data = (GreenQuestData)data;
                switch (Data.QuestStep)
                {
                    case 0:
                        if (Data.TimePassed == 0 && CanStartQuest)
                        {
                            return NotifyPlayerOfHisRequest();
                        }
                        break;
                    case 4:
                        if (Data.QuestStep <= 0)
                        {
                            return PostReadingStageDialogue();
                        }
                        break;

                }
            }
            return base.ImportantDialogueMessage(data, companion);
        }

        public override void UpdatePlayer(Player player, QuestData data)
        {
            if (data.IsCompleted) return;
            GreenQuestData Data = (GreenQuestData)data;
            if (Data.QuestStep == 4 && Data.TimePassed > 0)
            {
                Data.TimePassed -= (float)Main.dayRate;
                if (Data.TimePassed < 0) Data.TimePassed = 0;
            }
        }

        public override void AddDialogueOptions(QuestData data, bool IsTalkDialogue, Companion companion, MessageDialogue message)
        {
            GreenQuestData Data = (GreenQuestData)data;
            if (!IsTalkDialogue && companion.IsSameID(CompanionDB.Green))
            {
                if (Data.QuestStep == 0)
                {
                    if (NPC.downedBoss3)
                        message.AddOption("What do you need to begin treating Terrarians?", QuestBrief);
                }
                else if (Data.QuestStep < 4)
                {
                    message.AddOption("About the books you asked for...", OnTalkAboutBooks);
                }
                else if (Data.QuestStep == 4)
                {
                    message.AddOption("Have you finished reading the books?", ReadingStageDialogue);
                }
            }
        }

        #region Dialogues
        MessageBase NotifyPlayerOfHisRequest()
        {
            MessageDialogue md = new MessageDialogue("*Terrarian, I will need your assistance with a specific matter.*");
            md.AddOption("What is it?", QuestBrief);
            md.AddOption("Can we talk about that later?", OnPostpone);
            return md;
        }

        void OnPostpone()
        {
            (Data as GreenQuestData).TimePassed = 1f;
            Dialogue.LobbyDialogue("*Yes, we can speak about that later. I wont be able to treat Terrarians at the moment though. Right now, what do you need of me?*");
        }

        MessageBase PostReadingStageDialogue()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*[nickname], I have finished reading all those books.*", "And?");
            md.AddDialogueStep("*Beside not everything I should know about Terrarians body and their ailments is contained in them, I believe that's enough knowledge for me to try medicating Terrarians.*", "That means you can treat us?");
            md.AddDialogueStep("*Exactly. Before you arrived, I contacted some travelling merchants to bring me the supplies I need. Gladly, the first batch just arrived and I can begin right away.*", "Nice.");
            md.AddDialogueStep("*Of course, I will charge some fee out of this, but I will try making it fair.*");
            md.AddOption("Alright.", EndQuest);
            return md;
        }

        void EndQuest()
        {
            (Data as GreenQuestData).QuestStep = 5;
            Dialogue.LobbyDialogue("*That's all I had to tell you. Do you want to speak about something else?*");
        }

        void QuestBrief()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*I need books about medicine and anatomy of Terrarians, so I can know how your bodies works. If you could bring me some of them, I can check if is what I need.*", "Medicine and Anatomy Books?");
            md.AddDialogueStep("*Yes. I need to know more about how your bodies works, diseases you can get and how to treat them, so I can ensure that my tretment will be efficient.*", "I think I saw some books in the Dungeon.");
            md.AddDialogueStep("*The creepy catacombs far in this world? You think the books there will be useful? They might be... If you go there, you could bring some of those books, so I can check them.*", "Okay.");
            md.AddOption("Okay.", ReturnToMainMenuBriefStage);
            md.AddOption("I'll think about that.", ReturnToMainMenuBriefStage);
            md.RunDialogue();
        }

        void ReturnToMainMenuBriefStage()
        {
            Dialogue.LobbyDialogue("*For now, do you want to talk about something else?*");
            (Data as GreenQuestData).QuestStep = 1;
        }

        void OnTalkAboutBooks()
        {
            MessageDialogue md = new MessageDialogue("*Yes? About did you find some?*");
            if (MainMod.GetLocalPlayer.HasItem(ItemID.Book))
            {
                md.AddOption("Is this book useful?", DeliverBookDialogue);
            }
            md.AddOption("Where can I find books again..?", AskWhereToFindBooks);
            md.AddOption("Alright.", BackToLobby);
            md.RunDialogue();
        }

        void DeliverBookDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Let me check it.*");
            GetDialogueOptions(md);
            md.RunDialogue();
        }

        void GetDialogueOptions(MessageDialogue md)
        {
            md.AddOption("Hand over book.", GiveBook);
            md.AddOption("Don't hand over the book.", NevermindGiveBook);
        }

        void GiveBook()
        {
            for (int p = 0; p < 50; p++)
            {
                Item i = MainMod.GetLocalPlayer.inventory[p];
                if (i.type == ItemID.Book)
                {
                    i.stack--;
                    if (i.stack <= 0)
                        i.SetDefaults(0);
                    break;
                }
            }
            string Message = "";
            if (Main.rand.NextFloat() < 1f / 20)
            {
                GreenQuestData data = (GreenQuestData)Data;
                switch (data.QuestStep)
                {
                    case 2:
                        Message = "*Perfect! This book talks about diseases and treatments. I still need some more books.*";
                        break;
                    case 3:
                        Message = "*This is an anatomy book. Now I can know the location of your organs and more. Please look for more books.*";
                        break;
                    case 4:
                        TriggerGiveLastBookDialogue();
                        return;
                }
                data.QuestStep++;
            }
            else
            {
                switch (Main.rand.Next(5))
                {
                    case 0:
                        Message = "*Beside Terrarian story books are actually really interesting, this isn't what I'm looking for.*";
                        break;
                    case 1:
                        Message = "*This book is filled with gibberish. Who writes a book that doesn't makes sense?*";
                        break;
                    case 2:
                        Message = "*This book has several images of female Terrarians in underwear. That wont help me much related to Terrarian anathomy. Or at least for finding out internal organs placement.*";
                        break;
                    case 3:
                        Message = "*This seems like a geography book. What was it doing in a dungeon?*";
                        break;
                    case 4:
                        Message = "*There's a book talking about godly humanoid creatures. Why does this sounds so familiar?*";
                        break;
                }
            }
            MessageDialogue md = new MessageDialogue(Message);
            GetDialogueOptions(md);
            md.RunDialogue();
        }

        void NevermindGiveBook()
        {
            Dialogue.LobbyDialogue("*Wasn't you going to let me check it out? Okay.. Keep it then.*");
        }

        void TriggerGiveLastBookDialogue()
        {
            (Data as GreenQuestData).QuestStep = 4;
            (Data as GreenQuestData).TimePassed = 8 * 3600 * 24;
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*This is a medicine book. The information in it may be of help. I think that's all I need.*", "Are those books really enough?");
            md.AddDialogueStep("*I think they are. I will still need some time to read them, so check back after some days have passed.*");
            md.AddOption("So, I have to check you back in a number of days?", ReturnToLobbyAfterGivingLastBook);
            md.RunDialogue();
        }

        void ReturnToLobbyAfterGivingLastBook()
        {
            Dialogue.LobbyDialogue("*Yes. I will let you know when I finish reading the books. Until then, is there anything else you need of me?*");
        }

        void AskWhereToFindBooks()
        {
            MessageDialogue md = new MessageDialogue("*I remember you telling me that the Catacombs far in this world contained some books.*");
            md.AddOption("Oh yeah, I forgot.", BackToLobbyAfterAskingAboutBooks);
            md.RunDialogue();
        }

        void BackToLobbyAfterAskingAboutBooks()
        {
            Dialogue.LobbyDialogue("*I have really good memory, so you can cound on that.\nIs there something else you need to speak to me about?*");
        }

        void BackToLobby()
        {
            Dialogue.LobbyDialogue("*Do you need something from me?*");
        }

        void ReadingStageDialogue()
        {
            int ReadingProgress = (int)(Data as GreenQuestData).TimePassed / (24 * 3600);
            MessageDialogue md = new MessageDialogue();
            if (ReadingProgress < 2)
            {
                md.ChangeMessage("*I've finished the anatomy and diseases treatment books. Now i'm reading the medicine book you've found. I think I should have read it first, but I already know medicine... I will still read it anyways. Beside Terrarians body being quite similar to TerraGuardians, there are a few things I need to be careful about.*");
            }
            else if (ReadingProgress < 5)
            {
                md.ChangeMessage("*I've finished reading the book about Terrarian anatomy, and now I'm reading one about diseases and treatments. It looks like you Terrarians have quite a number of ailments I'll have to take note of. Maybe TerraGuardians can end up being affected by them too, so I will need better understanding about those.*");
            }
            else
            {
                md.ChangeMessage("*Not yet. I'm still reading the book about Terrarian anatomy. I seem to be getting a clearer understanding about how Terrarian bodies work, and where the organs are in the system.*");
            }
        }
        #endregion

        public class GreenQuestData : QuestData
        {
            public byte QuestStep = 0;
            public float TimePassed = 0f;
            public override ushort Version => base.Version;

            protected override void Save(TagCompound save, string QuestID)
            {
                save.Add("Step", QuestStep);
                save.Add("Time", TimePassed);
            }

            protected override void Load(TagCompound load, string QuestID, ushort LastVersion)
            {
                QuestStep = load.GetByte("Step");
                TimePassed = load.GetFloat("Time");
            }
        }
    }
}