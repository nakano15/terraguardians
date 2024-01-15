using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace terraguardians.Quests
{
    public class TestQuest: QuestBase
    {
        public override string Name => "Test Quest";
        public override QuestData GetQuestData => new TestQuestData();
        public override string GetQuestCurrentObjective(QuestData Data)
        {
            TestQuestData data = Data as TestQuestData;
            if (!data.KilledEoc)
                return "You have to kill Eye of Cthulhu.";
            if (!data.KilledEow)
                return "You have to kill Eater of Worlds.";
            if (!data.KilledSkele)
                return "You have to kill Skeletron.";
            return base.GetQuestCurrentObjective(Data);
        }

        public override bool IsQuestActive(QuestData data)
        {
            return true;
        }

        public override bool IsQuestCompleted(QuestData data)
        {
            return (data as TestQuestData).AllKilled;
        }

        public override string QuestStory(QuestData Data)
        {
            TestQuestData data = Data as TestQuestData;
            string Story = "To be considered a true Terrarian, I have to beat some bosses.";
            if (data.KilledEoc)
            {
                Story += "\n\nI have managed to defeat Eye of Cthulhu.";
            }
            if (data.KilledEow)
            {
                Story += "\n\nI have managed to defeat Eater of Worlds.";
            }
            if (data.KilledSkele)
            {
                Story += "\n\nI have managed to defeat Skeletron.";
            }
            if (data.AllKilled)
            {
                if (data.KilledSlimeKing)
                    Story += "\n\nSeems like I took care of almost Vanilla Terraria bosses, including King Slime.\nBut still, this is a different version of the game, so there's lots more of bosses to kill.\n\nTHE END";
                else
                    Story += "\n\nSeems like I took care of almost Vanilla Terraria bosses, except King Slime.\nBut still, this is a different version of the game, so there's lots more of bosses to kill.\n\nTHE END";
            }
            return Story;
        }

        public override void OnMobKill(NPC killedNpc, QuestData Data)
        {
            TestQuestData data = Data as TestQuestData;
            switch(killedNpc.type)
            {
                case NPCID.EyeofCthulhu:
                    if (!data.KilledEoc)
                        Main.NewText("The Eye of Cthulhu didn't stand a chance.");
                    data.KilledEoc = true;
                    break;
                case NPCID.EaterofWorldsHead:
                    if (!NPC.AnyNPCs(NPCID.EaterofWorldsBody))
                    {
                        if (!data.KilledEow)
                            Main.NewText("The Eater of Worlds turned into instant noodles.");
                        data.KilledEow = true;
                    }
                    break;
                case NPCID.SkeletronHead:
                    if (!data.KilledSkele)
                        Main.NewText("The Skeletron is now bone.");
                    data.KilledSkele = true;
                    break;
                case NPCID.KingSlime:
                    if (!data.KilledSkele)
                        Main.NewText("The King Slime didn't stand a chance.");
                    data.KilledSlimeKing = true;
                    break;
            }
        }

        public class TestQuestData : QuestData
        {
            public bool KilledEoc = false, KilledEow = false, KilledSkele = false, KilledSlimeKing = false;
            public override ushort Version => 0;
            public bool AllKilled => KilledEoc && KilledEow && KilledSkele;

            protected override void Save(TagCompound save, string QuestID)
            {
                save.Add(QuestID + "_eoc", KilledEoc);
                save.Add(QuestID + "_eow", KilledEow);
                save.Add(QuestID + "_ske", KilledSkele);
                save.Add(QuestID + "_king", KilledSlimeKing);
            }

            protected override void Load(TagCompound load, string QuestID, ushort LastVersion)
            {
                KilledEoc = load.GetBool(QuestID + "_eoc");
                KilledEow = load.GetBool(QuestID + "_eow");
                KilledSkele = load.GetBool(QuestID + "_ske");
                KilledSlimeKing = load.GetBool(QuestID + "_king");
            }
        }
    }
}