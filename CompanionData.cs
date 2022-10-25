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

        private CompanionCommonData CommonData = new CompanionCommonData();
        private string _Name = null;
        public string GetName { get { if(_Name == null) return Base.Name; return _Name; }}
        public uint ID { get{ return MyID.ID; }}
        public string ModID  { get{ return MyID.ModID; }}
        private CompanionID MyID = new CompanionID(0);
        public int MaxLife { get { return CommonData.MaxLife; } set { CommonData.MaxLife = value; } }
        public int MaxMana { get { return CommonData.MaxMana; } set { CommonData.MaxMana = value; } }
        public bool ExtraAccessorySlot { get { return CommonData.ExtraAccessorySlot; } set { CommonData.ExtraAccessorySlot = value; } }
        public Item[] Inventory = new Item[59], 
            Equipments = new Item[20],
            EquipDyes = new Item[10],
            MiscEquipment = new Item[5],
            MiscEquipDyes = new Item[5];
        public int[] BuffType = new int[22];
        public int[] BuffTime = new int[22];
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
                if (i < EquipDyes.Length)
                    EquipDyes[i] = new Item();
                if (i < MiscEquipDyes.Length)
                    MiscEquipDyes[i] = new Item();
            }
            ChangeCompanion(NewID, NewModID);
        }

        public void ChangeCommonData(CompanionCommonData NewCommonData)
        {
            CommonData = NewCommonData;
        }

        public void ChangeCompanion(uint NewID, string NewModID = "")
        {
            MyID = new CompanionID(NewID, NewModID);
            _Base = null;
        }

        public void ChangeName(string NewName)
        {
            _Name = NewName;
        }
    }
}