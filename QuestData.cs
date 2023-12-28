using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace terraguardians
{
    public class QuestData
    {
        QuestBase _Base;
        public QuestBase Base { get 
        {
            if (_Base == null)
                _Base = QuestContainer.GetQuest(ID, ModID);
            return _Base;
        }}
        public string Name { get { return Base.Name; } }
        public uint ID { get { return _ID; } internal set { _ID = value; } }
        public string ModID { get { return _ModID; } internal set { _ModID = value; } }
        uint _ID = 0;
        string _ModID = "";
        public bool IsActive { get { return Base.IsQuestActive(this); } }
        public bool IsCompleted { get { return Base.IsQuestFinished(this); } }
        public string GetObjective { get { return Base.GetQuestCurrentObjective(this); } }
        public string GetStory { get { return Base.QuestStory(this); } }        
    }
}