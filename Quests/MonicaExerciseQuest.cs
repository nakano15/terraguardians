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

namespace terraguardians.Quests;

public class MonicaExerciseQuest : QuestBase
{
    public override string Name => "Losing the Fat";
    public override QuestData GetQuestData => new MonicaExerciseQuestData();
    MonicaExerciseQuestData GetData => Data as MonicaExerciseQuestData;

    public override string GetQuestCurrentObjective(QuestData data)
    {
        MonicaExerciseQuestData Data = data as MonicaExerciseQuestData;
        if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.Postponed)
            return "Speak to Monica to see what she wants.";
        if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.AgreedToHelp)
        {
            if (Data.Fat <= 0)
            {
                return "Speak with Monica.";
            }
            return "Take Monica on your adventures, and help her lose weight.";
        }
        return base.GetQuestCurrentObjective(data);
    }

    public override string QuestStory(QuestData data)
    {
        MonicaExerciseQuestData Data = data as MonicaExerciseQuestData;
        string Story = "";
        if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.Postponed)
        {
            Story = "Monica wanted to speak about something important, but I told her that I can't speak about that right now.\nMaybe when possible I could ask her what she wants.";
        }
        else
        {
            Story = "Monica spoke with me about wanting help losing weight, and asked me if I could help while taking her on my travels.";
            if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.Denied)
            {
                Story += "\nI refused to help her, leaving her disappointed.\n\nMaybe I can bring that up again if I change my mind.";
            }
            else
            {
                Story += "\nI agreed to help her.\n\nShe said that taking her on my adventures would help her exercising.\nBeside also getting some help with tips on how she could do that could be handy..";
            }
            if (Data.SpokenWithMiguel)
            {
                Story += "\n\nI spoke with Miguel, who also gave some ideas of how she could lose weight.\nHe said that if I do his daily exercises while having Monica following me, will help her lose weight.\nHe also said that she should reduce the amount of food she eats daily, since she's accumulating more fat than burning.";
            }
            if (Data.Fat <= 0f)
            {
                Story += "\n\nAfter a long period of time doing exercises and exploring, Monica finally lost weight, just as she wanted.";
                if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.SpokenAfterLosingWeight)
                    Story += "\nShe Thanked me for that, and said that will try avoiding gaining weight again.";
                if (Data.SpokenWithMiguel && Data.SpokenWithMiguelAfterWeightLoss)
                {
                    Story += "\n\nAfter bringing Monica to Miguel, he congratulated her for her achievement, beside wasn't very happy about her declining trying to get some muscles in her body.";
                }
            }
            if (Data.IsCompleted)
            {
                Story += "\n\nTHE END";
            }
        }
        return Story;
    }

    public override bool IsQuestActive(QuestData data)
    {
        return GetData.SpokenState >= MonicaExerciseQuestData.SpokenStep.Postponed;
    }

    public override bool IsQuestCompleted(QuestData data)
    {
        return GetData.SpokenState == MonicaExerciseQuestData.SpokenStep.SpokenAfterLosingWeight;
    }

    public override void UpdatePlayer(Player player, QuestData data)
    {
        MonicaExerciseQuestData Data = GetData;
        if (Data.SpokenState == MonicaExerciseQuestData.SpokenStep.AgreedToHelp)
        {
            Companion Monica = PlayerMod.PlayerGetSummonedCompanion(player, CompanionDB.Monica);
            if (Monica != null)
            {
                if (Monica.velocity.Y != 0)
                {
                    if (Monica.justJumped)
                    {
                        Data.ModifyMonicaFatValue(Monica, -.05f);
                    }
                }
                else if (Monica.velocity.X != 0)
                {
                    Data.ModifyMonicaFatValue(Monica, -MathF.Abs(Monica.velocity.X * .025f));
                }
            }
        }
    }

    public override MessageBase ImportantDialogueMessage(QuestData data, Companion companion)
    {
        if (companion.IsSameID(CompanionDB.Monica) && companion.FriendshipLevel >= 5)
        {
            switch (GetData.SpokenState)
            {
                case MonicaExerciseQuestData.SpokenStep.NotSpoken:
                    return GetHerRequestMessage();
                case MonicaExerciseQuestData.SpokenStep.AgreedToHelp:
                    if (companion.IsSkinActive(MonicaBase.MonicaFitSkinID))
                    {
                        return OnSpeakWithMonicaAfterLosingWeight();
                    }
                    break;
            }
        }
        return base.ImportantDialogueMessage(data, companion);
    }

    public override void AddDialogueOptions(QuestData data, bool IsTalkDialogue, Companion companion, MessageDialogue message)
    {
        if (!IsTalkDialogue)
        {
            if (companion.IsSameID(CompanionDB.Monica))
            {
                switch (GetData.SpokenState)
                {
                    case MonicaExerciseQuestData.SpokenStep.Postponed:
                        message.AddOption("What did you wanted to speak with me about?", HearRequest1);
                        break;
                    case MonicaExerciseQuestData.SpokenStep.Denied:
                        message.AddOption("You still need help with exercises?", RetryToOfferHelp);
                        break;
                    case MonicaExerciseQuestData.SpokenStep.AgreedToHelp:
                        message.AddOption("How do you feel right now?", OnAskMonicaAboutFatState);
                        break;
                }
            }
            else if (companion.IsSameID(CompanionDB.Miguel))
            {
                if (GetData.SpokenState >= MonicaExerciseQuestData.SpokenStep.AgreedToHelp)
                {
                    if (!GetData.SpokenWithMiguel)
                        message.AddOption("Could you help [gn:" + CompanionDB.Monica + "] lose weight?", OnAskMiguelForHelp);
                    else if (!GetData.SpokenWithMiguelAfterWeightLoss && PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Monica) && PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Monica).IsSkinActive(MonicaBase.MonicaFitSkinID))
                    {
                        message.AddOption("Look who lost weight.", OnSpeakWithMiguelAfterLosingWeight);
                    }
                }
            }
        }
    }

    #region Dialogues Area
    public MessageBase GetHerRequestMessage()
    {
        MessageDialogue md = new MessageDialogue("*[nickname], can I speak with you about something?*");
        md.AddOption("Sure, what is it?", HearRequest1);
        md.AddOption("Not right now.", PostponeRequest);
        return md;
    }

    void HearRequest1()
    {
        MultiStepDialogue md = new MultiStepDialogue();
        md.AddDialogueStep("*You see... I'm quite overweight, and I want to lose my weight.*");
        md.AddDialogueStep("*I heard that doing exercises will help me with that, so I thought you could help me with that.*", "How could I help?");
        md.AddDialogueStep("*You know, when you're adventuring you're always running and jumping around. That seems like a good exercise.*", "I think so..?");
        md.AddDialogueStep("*So I wanted to ask if you want to help me lose weight while taking me on your adventures.*");
        md.AddOption("Yes, I can help with that.", AgreeToHelp);
        md.AddOption("I don't want to.", DenyToHelp);
        md.RunDialogue();
    }

    void AgreeToHelp()
    {
        GetData.SpokenState = MonicaExerciseQuestData.SpokenStep.AgreedToHelp;
        MultiStepDialogue md = new MultiStepDialogue();
        md.AddDialogueStep("*Oh, Thank You! Following you on your travels should help me lose weight, but maybe there's other ways of doing that.*");
        md.AddDialogueStep("*Would also be nice to know someone who could help me with that...*");
        md.AddDialogueStep("*I can hardly wait to get myself doing some exercises.*");
        md.RunDialogue();
    }

    void DenyToHelp()
    {
        GetData.SpokenState = MonicaExerciseQuestData.SpokenStep.Denied;
        MessageDialogue md = new MessageDialogue("*Oh... Uh.. Sorry I asked...\nThat was just... Something I wanted...\nNevermind I asked... (She looks sad)*");
        md.AddOption("Close", Dialogue.EndDialogue);
        md.RunDialogue();
    }

    void PostponeRequest()
    {
        MessageDialogue md = new MessageDialogue("*Oh... Okay... Speak with me again when possible.\nIt's quite important to me..*");
        md.AddOption("Okay.", Dialogue.LobbyDialogue);
        md.RunDialogue();
        GetData.SpokenState = MonicaExerciseQuestData.SpokenStep.Postponed;
    }

    void RetryToOfferHelp()
    {
        MultiStepDialogue md = new MultiStepDialogue();
        md.AddDialogueStep("*Yes. I can't seem to be able to do that alone.*");
        md.AddDialogueStep("*And the more I try, the more I end up eating, and the more my belly grows...*");
        md.AddDialogueStep("*You will help me exercise?*");
        md.AddOption("I will.", AgreeToHelp);
        md.AddOption("Nope.", OnDenyOnceAgain);
        md.RunDialogue();
    }

    void OnDenyOnceAgain()
    {
        MessageDialogue md = new MessageDialogue("*Oh...*");
        md.AddOption("Close", Dialogue.EndDialogue);
    }

    void OnAskMiguelForHelp()
    {
        GetData.SpokenWithMiguel = true;
        MultiStepDialogue md = new MultiStepDialogue();
        Companion Monica = PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Monica);
        md.AddDialogueStep("*If I can? Of course I can.*");
        if (Monica != null)
        {
            md.AddDialogueStep("*You can? Then how can I lose weight?*", Speaker: Monica);
            md.AddDialogueStep("*Obviously, I recommend doing some exercises, that way you can burn your fat.*");
            md.AddDialogueStep("*I offer daily exercises for [nickname] here, just join in and should help.*");
            md.AddDialogueStep("*Got it. What else?*", Speaker: Monica);
            md.AddDialogueStep("*Of course you also need to review your diet. With the fat you've accumulated, you're not burning enough of what you eat.*");
            md.AddDialogueStep("*It will do you good if you lower the amount of food you eat.*");
            md.AddDialogueStep("*I can try doing that, but the meals are so delicious...*", Speaker: Monica);
            md.AddDialogueStep("*Do you want to lose weight?*");
            md.AddDialogueStep("*Yes, I want.*", Speaker: Monica);
            md.AddDialogueStep("*So don't eat more than you can burn daily.*");
            md.AddDialogueStep("*I can't recommend what you could eat, since I'm no nutritionist. I'm more into gains.*");
            md.AddDialogueStep("*Beautiful gains...*");
            md.AddDialogueStep("*That's all I can tell you on how to lose weight.*");
            md.AddDialogueStep("*Thank you, that was useful.*", Speaker: Monica);
            md.AddDialogueStep("*Be sure to come see me once you lose that fat. I want to see that.*");
            md.AddDialogueStep("*Uh... Sure.*", Speaker: Monica);
        }
        else
        {
            md.AddDialogueStep("*I have seen her case. So I have an idea of what she should do.*");
            md.AddDialogueStep("*Obviously, she will need to do exercises to burn that fat.*");
            md.AddDialogueStep("*I offer daily exercises that even you can do. Have her join in when you do them. It should do her good.*");
            md.AddDialogueStep("*She will need to review her died. She is accumulating more fat than is burning it, and that is a problem.*");
            md.AddDialogueStep("*She should not over eat food. Lowering the food quantity would help.*");
            md.AddDialogueStep("*Not eating more than the body can burn per day is a good idea.*");
            md.AddDialogueStep("*I can't recommend what she should eat, since I'm not a nutritionist. I'm more into gains.*");
            md.AddDialogueStep("*Beautiful gains...*");
            md.AddDialogueStep("*That's all I can tell about her to lose weight.*");
            md.AddDialogueStep("*If you manage to make her lose weight, bring her to me. I want to see that.*");
        }
        md.RunDialogue();
    }

    void OnSpeakWithMiguelAfterLosingWeight()
    {
        GetData.SpokenWithMiguelAfterWeightLoss = true;
        MultiStepDialogue md = new MultiStepDialogue();
        Companion Monica = PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Monica);
        md.AddDialogueStep("*Look at that. You managed to lose that bodywork.*");
        md.AddDialogueStep("*Hey! That's not funny!*", Speaker: Monica);
        md.AddDialogueStep("*Hahaha. Anyways, congratulations on losing your weight.*");
        md.AddDialogueStep("*Thanks, beside [nickname] and you helped too.*", Speaker: Monica);
        md.AddDialogueStep("*Indeed. Anyways, aren't you interested in making some muscles show up on your body?*");
        md.AddDialogueStep("*No way. I'm happy just being slim like this.*", Speaker: Monica);
        md.AddDialogueStep("*Hehe, your loss then. Would be nice to see you getting some abs.*");
        md.AddDialogueStep("*Anyways, congratulations on losing weight. Try not to get weight again.*");
        md.AddDialogueStep("*I will.*", Speaker: Monica);
        md.RunDialogue();
    }

    MessageBase OnSpeakWithMonicaAfterLosingWeight()
    {
        GetData.SpokenState = MonicaExerciseQuestData.SpokenStep.SpokenAfterLosingWeight;
        MultiStepDialogue md = new MultiStepDialogue();
        md.AddDialogueStep("*I can't believe it happened. I lost weight.*");
        md.AddDialogueStep("*I lost weight, [nickname]! I lost weight! Hahaha.*");
        md.AddDialogueStep("*I'm so happy that happened. Thank you so much [nickname] for your help.*");
        md.AddDialogueStep("*I will try my best not to get overweight again.*");
        return md;
    }

    void OnAskMonicaAboutFatState()
    {
        MessageDialogue md = new MessageDialogue();
        float Percentage = GetData.Fat * 100f / MonicaExerciseQuestData.DefaultFatLevel;
        if (Percentage >= 90f)
            md.ChangeMessage("*I feel no different right now. Still pretty fat...*");
        else if (Percentage >= 75f)
            md.ChangeMessage("*I feel something different. Like my legs are more able to carry my body.*");
        else if (Percentage >= 50f)
            md.ChangeMessage("*I think that now I can sit decently on a chair, without pushing the table with my belly.*");
        else if (Percentage >= 30f)
            md.ChangeMessage("*It seems to be working. I can see my feet when I look down.*");
        else if (Percentage >= 15f)
            md.ChangeMessage("*I'm feeling a lot more active lately. That feels good.*");
        else
            md.ChangeMessage("*I feel like I'm really close to being slim. I feel like I can do it.*");
        md.AddOption("Okay.", Dialogue.LobbyDialogue);
        md.RunDialogue();
    }

    void SetFatLevelsToLow()
    {
        GetData.Fat = 500;
    }
    #endregion

    public class MonicaExerciseQuestData : QuestData
    {
        public override ushort Version => 0;

        public SpokenStep SpokenState = 0;
        public float Fat = DefaultFatLevel;
        BitsByte internalflags = new BitsByte();
        public bool SpokenWithMiguel
        {
            get
            {
                return internalflags[0];
            }
            set
            {
                internalflags[0] = value;
            }
        }
        public bool SpokenWithMiguelAfterWeightLoss
        {
            get
            {
                return internalflags[1];
            }
            set
            {
                internalflags[1] = value;
            }
        }
        public const float DefaultFatLevel = 22000f;

        public void ModifyMonicaFatValue(Companion Monica, float Change)
        {
            if (Fat <= 0) return;
            Fat += Change;
            if (Fat <= 0f && !Monica.IsSkinActive(MonicaBase.MonicaFitSkinID))
            {
                Monica.ChangeSkin(MonicaBase.MonicaFitSkinID);
                Monica.SaySomething("*I did it!*");
            }
        }

        public enum SpokenStep : byte
        {
            NotSpoken = 0,
            Postponed = 1,
            Denied = 2,
            AgreedToHelp = 3,
            SpokenAfterLosingWeight = 4
        }

        protected override void Save(TagCompound save, string QuestID)
        {
            save.Add(QuestID + "SpokenState", (byte)SpokenState);
            save.Add(QuestID + "Fat", Fat);
            save.Add(QuestID + "Flags", (byte)internalflags);
        }

        protected override void Load(TagCompound load, string QuestID, ushort LastVersion)
        {
            SpokenState = (SpokenStep)load.GetByte(QuestID + "SpokenState");
            Fat = load.GetFloat(QuestID + "Fat");
            internalflags = load.GetByte(QuestID + "Flags");
        }
    }
}