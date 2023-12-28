using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Quests
{
    public class BlueSeekingZackQuest : QuestBase
    {
        public override string Name => "Missing";
        public override QuestData GetQuestData => new BlueSeekingZackQuestData();

        public override bool IsQuestActive(QuestData data)
        {
            return (data as BlueSeekingZackQuestData).BlueDialogueStep > 0;
        }

        public override bool IsQuestCompleted(QuestData data)
        {
            return (data as BlueSeekingZackQuestData).SpokeToBluePosQuest;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            BlueSeekingZackQuestData Data = data as BlueSeekingZackQuestData;
            if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Zack))
            {
                switch(Data.BlueDialogueStep)
                {
                    case 1:
                        return "Speak to Blue about the Zombie Guardian.";
                    case 2:
                        return "Bring Zack to Blue.";
                }
            }
            else if (Data.SpottedZackOnce > 0)
            {
                switch(Data.SpottedZackOnce)
                {
                    case 1:
                        return "Tell Blue about the Zombie you saw.";
                    case 2:
                        return "Take Blue with you during a Bloodmoon, on the same place you found the Zombie Guardian.";
                    case 3:
                        return "Find a way of speaking to the Zombie, with Blue's help.";
                }
            }
            else
            {
                switch (Data.BlueDialogueStep)
                {
                    case 0:
                        return "Blue may eventually mention this upon speaking to her.";
                    case 1:
                        return "Listen to what Blue has to say if she brings up the topic again.";
                    case 2:
                        return "Find the missing person Blue seeks.";
                }
            }
            return "Insert objective of the quest here.\n2 lines is the limit.";
        }

        public override string QuestStory(QuestData data)
        {
            string Story = "";
            if (data.IsActive)
            {
                BlueSeekingZackQuestData Data = (BlueSeekingZackQuestData)data;
                switch(Data.BlueDialogueStep)
                {
                    case 1:
                        Story = "Blue asked to speak to me about something, but then I denied, since I was too busy to listen to what she had to say.";
                        break;
                    case 2:
                        Story = "Blue asked to speak to me about something, and I agreed. She told me that she did not come to my world for camping.\n\nShe was searching for another of her kind, a Wolf TerraGuardian, whose last whereabouts was travelling with a Terrarian named Brandon.\n\nIf I find that TerraGuadian, I should let her know.";
                        break;
                }
                if (Data.SpottedZackOnce > 0)
                {
                    if (Story.Length > 0)
                    {
                        Story += "\n\n";
                    }
                    Story += "A Bloodmoon happened in my world, and in the middle of the hordes of horrible monsters, a zombie Wolf TerraGuardian has appeared.";
                    switch (Data.SpottedZackOnce)
                    {
                        case 1:
                            Story += "It tried to devour me, so I had to defend myself.\nI should tell Blue about this. It might be relevant for her.";
                            break;
                        case 2:
                            Story += "It tried to devour me, so I had to defend myself.\n\nI managed to speak with Blue about this. She feared that the one she's looking for may be the zombie, and asked me to take her with me the next time a Bloodmoon happens. She wants to see with her own eyes that, and so we should return to the place I found the Zombie.";
                            break;
                        case 3:
                            Story += "\nBlue seems to have recognized the Zombie, she said that his name was Zack. We still had to defend ourselves from that zombie attack.";
                            break;
                    }
                }
                if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Zack))
                {
                    if (Data.SpokeToBluePosQuest)
                    {
                        if (Story.Length > 0)
                        {
                            Story += "\n\n";
                        }
                        if (Data.BlueDialogueStep == 0)
                        {
                            Story += "After speaking with Blue, I discovered that she was actually looking for the Zombie TerraGuardian I met during my travels. He presented himself to me as Zack. Now Blue and Zack can be together again.\n\n";
                        }
                        else if (Data.SpottedZackOnce == 0)
                        {
                            Story += "I managed to find the TerraGuardian Blue was looking for. I found him living on some world.\nNow Blue and Zack can be together again.\n\n";
                        }
                        else
                        {
                            Story += "We managed to be able to speak to the Zombie Guardian, somehow. After It was weakened, ";
                            if (Data.BlueWasPresent)
                            {
                                Story += " It seems like the sound of Blue's voice, and the strength of their bond, made him snap out of the zombie instincts, recognize Blue, and cease his attack.";
                            }
                            else
                            {
                                Story += " It sleuthed a Hairpin Blue gave you, which made him snap out of his zombie instincts, and cease his attack.";
                            }
                            Story += "\nAfterwards, he thanked me for making him be able to think rationally again, and told me that his name is Zack.\n\n";
                        }
                        Story += "The Zombie Guardian now lives with Blue, and is fighting off against his unending hunger, with her help.";
                        if (true/*!PlayerMod.IsQuestCompleted(Main.LocalPlayer, TgQuestContainer.ZacksMeatbagOutfitQuest)*/) //If Meatbag Quest isn't completed
                        {
                            Story += "\nAt the same time they are happy for being together, they are saddened due to each other's worries and thoughts.\n\nTHE END?";
                        }
                        else
                        {
                            Story += "\nAfter Blue and I prepared a gift for Zacks, they both managed to forget for a while each other's worries, and managed to spend the " +
                                "sunset together.\n\nTHE END";
                        }
                    }
                    else
                    {
                        if(Data.BlueDialogueStep == 2)
                        {
                            if (Story.Length > 0)
                            {
                                Story += "\n\n";
                            }
                            Story += "Zacks seems to meet the description that Blue gave you. You should bring him to her.";
                        }
                    }
                }
            }
            return Story;
        }

        public override MessageBase ImportantDialogueMessage(QuestData data, Companion companion)
        {
            if (!data.IsCompleted && companion.IsSameID(CompanionDB.Blue))
            {
                if (/*companion.FriendshipLevel >= 5 && MainMod.GetLocalPlayer.statLifeMax > 180 && */(data as BlueSeekingZackQuestData).BlueDialogueStep < 2)
                {
                    return BlueTalksToPlayerAboutZackSearch();
                }
            }
            return base.ImportantDialogueMessage(data, companion);
        }

        #region Blue Talking About Zack Dialogue
        MessageBase BlueTalksToPlayerAboutZackSearch()
        {
            MessageDialogue md = new MessageDialogue();
            BlueSeekingZackQuestData data = (BlueSeekingZackQuestData)Data;
            if (data.BlueDialogueStep == 0)
                md.ChangeMessage("*[nickname], I have something I have to talk about... Can we talk about it now?*");
            else
                md.ChangeMessage("*I must tell you something, can we talk about it right now?*");
            md.AddOption("Yes", BlueTalkAboutZackPlayerSaysYes);
            md.AddOption("No", BlueTalkAboutZackPlayerSaysNo);
            return md;
        }

        void BlueTalkAboutZackPlayerSaysYes()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*I'll be sincere with you, [nickname]. The reason why I came here, wasn't for camping. I'm actually looking for someone.*");
            md.AddDialogueStep("*The reason why I took so long to tell you this, is because I think I can trust you on this matter..*");
            md.AddDialogueStep("*Anyways, do you know who Brandon is?*");
            if(MainMod.GetLocalPlayer.name.ToLower().Contains("brandon"))
                md.AddOption("Yes. That's me.", BlueTalkAboutZackPlayerBranch_1);
            md.AddOption("No, I don't.", BlueTalkAboutZackPlayerBranch_2);
            md.AddOption("Who?", BlueTalkAboutZackPlayerBranch_3);
            md.RunDialogue();
        }

        void BlueTalkAboutZackPlayerBranch_1()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*No, not you. The one i'm mentioning is another Brandon. Beside he's also a Terrarian but... It's not you.*");
            md.AddDialogueStep("*I think that by that answer, you don't know who he is.*");
            BlueTalkAboutZackPlayerBackOnRails(md);
        }

        void BlueTalkAboutZackPlayerBranch_2()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*Hm.. Maybe they didn't got here yet, or you didn't arrived this world at the time they were exploring it. But It has been a really long time... It doesn't makes sense them not arriving here yet.*");
            BlueTalkAboutZackPlayerBackOnRails(md);
        }

        void BlueTalkAboutZackPlayerBranch_3()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*I think that by that answer, you don't know who he is.*");
            BlueTalkAboutZackPlayerBackOnRails(md);
        }

        void BlueTalkAboutZackPlayerBackOnRails(MultiStepDialogue md)
        {
            md.AddDialogueStep("*I really need to find him..*", ProceedText:"Why are you looking for that person?");
            md.AddDialogueStep("*I'm not exactly looking for that person, but for the TerraGuardian that accompanied him.*");
            md.AddDialogueStep("*Last time I saw them, they were off to do a mission on some Terra Realm world, and then I never heard of them again.*", ProceedText:"What can you tell me about the TerraGuardian you're looking for?");
            md.AddDialogueStep("*Well, he's a Wolf Guardian, just like me. He's also taller, and likes pulling pranks on people, really easy to find out.*");
            md.AddDialogueStep("*I am getting a bit desperated trying to look for him, so if you find him, please tell me.*");
            md.AddOption("Okay.", BlueTalkAboutZackPlayerThankAndBackToLobby);
            md.RunDialogue();
        }

        void BlueTalkAboutZackPlayerThankAndBackToLobby()
        {
            (Data as BlueSeekingZackQuestData).BlueDialogueStep = 2;
            Data.ShowQuestStartedNotification();
            Dialogue.LobbyDialogue("*Thank you, [nickname]...\nBy the way, want to speak about something else? Or do you want more details?*");
        }

        void BlueTalkAboutZackPlayerSaysNo()
        {
            BlueSeekingZackQuestData data = (BlueSeekingZackQuestData)Data;
            data.BlueDialogueStep = 1;
            Dialogue.LobbyDialogue("*Not now? We can speak about this later, then.\n" +
                    "Want to talk about something else?*");
        }
        #endregion

        public class BlueSeekingZackQuestData : QuestData
        {
            public byte BlueDialogueStep = 0;
            public bool BlueWasPresent = false, SpokeToBluePosQuest = false;
            public byte SpottedZackOnce = 0;
            public const byte ZACKSPOTTED_BLUENOTPRESENT = 1, ZACKSPOTTED_BLUENOTPRESENTBUTASKEDPLAYERTOGOWITH = 2, ZACKSPOTTED_BLUEPRESENT = 3;
        }
    }
}