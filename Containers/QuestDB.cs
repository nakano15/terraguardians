using Terraria.ModLoader;
using System.Collections.Generic;
using terraguardians.Quests;

namespace terraguardians
{
    public class QuestDB : QuestContainer
    {
        const uint Quest_Missing = 0;
        protected override void CreateQuestDB()
        {
            //AddQuest(Quest_Missing, new BlueSeekingZackQuest());
        }
    }
}