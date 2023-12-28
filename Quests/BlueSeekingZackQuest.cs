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
            return false;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            return "Insert objective of the quest here.\n2 lines is the limit.";
        }

        public override string QuestStory(QuestData data)
        {
            return "The story of this quest must still be written.";
        }

        public class BlueSeekingZackQuestData : QuestData
        {

        }
    }
}