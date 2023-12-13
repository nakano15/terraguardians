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
        public uint ID { get { return _ID; } internal set { _ID = value; } }
        public string ModID { get { return _ModID; } internal set { _ModID = value; } }
        uint _ID = 0;
        string _ModID = "";
        
    }
}