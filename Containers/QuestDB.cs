using Terraria.ModLoader;
using System.Collections.Generic;
using terraguardians.Quests;
using nterrautils;

namespace terraguardians
{
    public class QuestDB : QuestContainer
    {
        const uint Quest_Missing = 0,
            Test_Quest = 1000000000;
        protected override void CreateQuestDB()
        {
            //AddQuest(Quest_Missing, new BlueSeekingZackQuest());
            //AddQuest(Test_Quest, new TestQuest());
        }
    }
}