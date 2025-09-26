using Terraria.ModLoader;
using System.Collections.Generic;
using terraguardians.Quests;
using nterrautils;

namespace terraguardians
{
    public class QuestDB : QuestContainer
    {
        public const uint Quest_Missing = 0,
            Stay = 1,
            GreenHealingUnlock = 2,
            MonicaSlimSkinQuest = 10,
            Mysterious_Note = 10000,
            Test_Quest = 1000000000;
        protected override void CreateQuestDB()
        {
            AddQuest(Quest_Missing, new BlueSeekingZackQuest());
            AddQuest(Stay, new BreeStayQuest());
            AddQuest(GreenHealingUnlock, new GreenHealingUnlockQuest());
            AddQuest(MonicaSlimSkinQuest, new MonicaExerciseQuest());
            AddQuest(Mysterious_Note, new MysteriousNoteQuest());
            //AddQuest(Test_Quest, new TestQuest());
        }
    }
}