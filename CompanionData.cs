using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionData
    {
        public CompanionBase Base 
        {
            get
            { 
                if(_Base == null) 
                    _Base = MainMod.GetCompanionBase(ID, ModID); 
                return _Base; 
            } 
        }
        private CompanionBase _Base = null;

        private string _Name = null;
        public string GetName{get{ if(_Name == null) return Base.Name; return _Name; }}
        public uint ID = 0;
        public string ModID = "";
        public int MaxLife = 100;
        public int MaxMana = 20;
        public bool ExtraAccessorySlot = false;
        public Item[] Inventory = new Item[59], 
            Equipments = new Item[20],
            MiscEquipment = new Item[5];
        private bool _Initialized = false;
        public bool IsInitialized { get { return _Initialized; } set { _Initialized = value; } }

        public CompanionData(uint NewID = 0, string NewModID = "")
        {
            for(byte i = 0; i < 59; i++)
            {
                Inventory[i] = new Item();
                if(i < Equipments.Length)
                    Equipments[i] = new Item();
                if (i < MiscEquipment.Length)
                    MiscEquipment[i] = new Item();
            }
            ChangeCompanion(NewID, NewModID);
        }

        public void ChangeCompanion(uint NewID, string NewModID = "")
        {
            if(NewModID == "") NewModID = MainMod.GetModName;
            ID = NewID;
            ModID = NewModID;
            _Base = null;
        }

        public void ChangeName(string NewName)
        {
            _Name = NewName;
        }
    }
}