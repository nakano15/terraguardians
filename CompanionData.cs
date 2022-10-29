using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

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
        public Genders Gender { get { return Base.Gender; }}
        public uint ID { get{ return MyID.ID; }}
        public string ModID  { get{ return MyID.ModID; }}
        private uint _Index = 0;
        public uint Index { get { return _Index; } internal set { _Index = value; }}
        private CompanionID MyID = new CompanionID(0);
        public int MaxHealth { get { return CommonData.MaxHealth; } set { CommonData.MaxHealth = value; } }
        public int MaxMana { get { return CommonData.MaxMana; } set { CommonData.MaxMana = value; } }
        public bool ExtraAccessorySlot { get { return CommonData.ExtraAccessorySlot; } set { CommonData.ExtraAccessorySlot = value; } }
        public Item[] Inventory = new Item[59], 
            Equipments = new Item[20],
            EquipDyes = new Item[10],
            MiscEquipment = new Item[5],
            MiscEquipDyes = new Item[5];
        public int[] BuffType = new int[22];
        public int[] BuffTime = new int[22];

        public string GetNameColored()
        {
            return Base.GetNameColored(this);
        }

        public CompanionData(uint NewID = 0, string NewModID = "", uint Index = 0)
        {
            this.Index = Index;
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
            ChangeCompanion(NewID, NewModID, true);
        }

        private void SetInitialInventory()
        {
            for(byte i = 0; i < 59; i++)
            {
                Inventory[i].SetDefaults(0);
                if(i < Equipments.Length)
                    Equipments[i].SetDefaults(0);
                if (i < MiscEquipment.Length)
                    MiscEquipment[i].SetDefaults(0);
                if (i < EquipDyes.Length)
                    EquipDyes[i].SetDefaults(0);
                if (i < MiscEquipDyes.Length)
                    MiscEquipDyes[i].SetDefaults(0);
            }
            InitialItemDefinition[] Equips = new InitialItemDefinition[10];
            for(int i = 0; i < Equips.Length; i++)
            {
                Equips[i] = new InitialItemDefinition(0);
            }
            Base.InitialInventory(out InitialItemDefinition[] Items, ref Equips);
            for(int i = 0; i < Items.Length; i++)
            {
                if(i < Inventory.Length)
                {
                    Inventory[i].SetDefaults(Items[i].ID);
                    Inventory[i].stack = Items[i].Stack;
                }
            }
            for (int i = 0; i < Equips.Length; i++)
            {
                if(i < Equipments.Length)
                {
                    Equipments[i].SetDefaults(Equips[i].ID);
                    if(Equips[i].ID == 0) continue;
                    Equipments[i].stack = Equips[i].Stack;
                }
            }
        }

        public bool IsSameID(uint ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.GetModName;
            return MyID.IsSameID(ID, ModID);
        }

        public void ChangeCommonData(CompanionCommonData NewCommonData)
        {
            CommonData = NewCommonData;
        }

        public void ChangeCompanion(uint NewID, string NewModID = "", bool ResetInventory = true)
        {
            MyID = new CompanionID(NewID, NewModID);
            _Base = null;
            if (ResetInventory) SetInitialInventory();
        }

        public void ChangeName(string NewName)
        {
            _Name = NewName;
        }
        
        public void Save(TagCompound save, uint UniqueID)
        {
            save.Add("CompanionID_" + UniqueID, MyID.ID);
            save.Add("CompanionModID_" + UniqueID, MyID.ModID);
            save.Add("CompanionHasNameSet_" + UniqueID, _Name != null);
            save.Add("CompanionName_" + UniqueID, _Name);
            //save.Add("CompanionHealth_" + UniqueID, )
            for(int i = 0; i < 59; i++)
            {
                save.Add("CompanionInventory_" + i + "_" + UniqueID, Inventory[i]);
                if(i < 20)
                    save.Add("CompanionEquipment_" + i + "_" + UniqueID, Equipments[i]);
                if (i < 10)
                    save.Add("CompanionEquipDyes_" + i + "_" + UniqueID, EquipDyes[i]);
                if(i < 5)
                {
                    save.Add("CompanionMiscEquip_" + i + "_" + UniqueID, MiscEquipment[i]);
                    save.Add("CompanionMiscEquipDyes_" + i + "_" + UniqueID, MiscEquipDyes[i]);
                }
            }
            for(int i = 0; i < 22; i++)
            {
                save.Add("CompanionBuffType_" + i + "_" + UniqueID, BuffType[i]);
                save.Add("CompanionBuffTime_" + i + "_" + UniqueID, BuffTime[i]);
            }
        }

        public void Load(TagCompound tag, uint UniqueID, uint LastVersion)
        {
            {
                uint NewID = tag.Get<uint>("CompanionID_" + UniqueID);
                string NewModID = tag.GetString("CompanionModID_" + UniqueID);
                ChangeCompanion(NewID, NewModID);
            }
            if(tag.GetBool("CompanionHasNameSet_" + UniqueID))
                _Name = tag.GetString("CompanionName_" + UniqueID);
            for(int i = 0; i < 59; i++)
            {
                Inventory[i] = tag.Get<Item>("CompanionInventory_" + i + "_" + UniqueID);
                if (i < 20)
                    Equipments[i] = tag.Get<Item>("CompanionEquipment_" + i + "_" + UniqueID);
                if (i < 10)
                    EquipDyes[i] = tag.Get<Item>("CompanionEquipDyes_" + i + "_" + UniqueID);
                if (i < 5)
                {
                    MiscEquipment[i] = tag.Get<Item>("CompanionMiscEquip_" + i + "_" + UniqueID);
                    MiscEquipDyes[i] = tag.Get<Item>("CompanionMiscEquipDyes_" + i + "_" + UniqueID);
                }
            }
            for(int i = 0; i < 22; i++)
            {
                BuffType[i] = tag.GetInt("CompanionBuffType_" + i + "_" + UniqueID);
                BuffTime[i] = tag.GetInt("CompanionBuffTime_" + i + "_" + UniqueID);
            }
        }
    }
}