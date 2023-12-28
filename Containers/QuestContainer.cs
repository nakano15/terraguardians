using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class QuestContainer
    {
        static Dictionary<string, QuestContainer> QuestsContainer = new Dictionary<string, QuestContainer>();
        Dictionary<uint, QuestBase> QuestList = new Dictionary<uint, QuestBase>();
        static QuestBase InvalidQuest = new QuestBase(true);
        string _ModID;

        public static void AddQuestContainer(Mod mod, QuestContainer container)
        {
            if (mod == null || container == null || QuestsContainer.ContainsKey(mod.Name)) return;
            QuestsContainer.Add(mod.Name, container);
            container._ModID = mod.Name;
            container.CreateQuestDB();
        }

        protected virtual void CreateQuestDB()
        {

        }

        protected void AddQuest(uint ID, QuestBase quest)
        {
            if (!QuestList.ContainsKey(ID))
            {
                QuestList.Add(ID, quest);
                quest.SetQuestModInfos(ID, _ModID);
            }
        }

        public static QuestBase GetQuest(uint ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (QuestsContainer.ContainsKey(ModID))
            {
                if (QuestsContainer[ModID].QuestList.ContainsKey(ID)) return QuestsContainer[ModID].QuestList[ID];
            }
            return InvalidQuest;
        }

        internal static void CreateQuestListToPlayer(PlayerMod player)
        {
            player.QuestDatas.Clear();
            foreach (string ModID in QuestsContainer.Keys)
            {
                QuestContainer c = QuestsContainer[ModID];
                foreach (uint id in c.QuestList.Keys)
                {
                    QuestBase quest = c.QuestList[id];
                    if (!quest.IsInvalid)
                    {
                        QuestData data = quest.GetQuestData;
                        data.ID = id;
                        data.ModID = ModID;
                        player.QuestDatas.Add(data);
                    }
                }
            }
        }

        internal static void Initialize()
        {

        }

        internal static void Unload()
        {
            foreach (string s in QuestsContainer.Keys)
            {
                QuestsContainer[s].QuestList.Clear();
            }
            QuestsContainer.Clear();
            QuestsContainer = null;
            InvalidQuest = null;
        }
    }
}