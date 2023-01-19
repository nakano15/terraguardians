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
        public CompanionID GetMyID { get { return MyID; } }
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
        public FriendshipSystem FriendshipProgress = new FriendshipSystem();
        private string _PlayerNickname = null;
        public byte OutfitID = 0, SkinID = 0;
        public bool IsStarter = false;
        public CombatTactics CombatTactic = CombatTactics.MidRange;
        public byte FriendshipLevel { get { return FriendshipProgress.Level; } }
        public sbyte FriendshipExp { get { return FriendshipProgress.Progress; } }
        public byte FriendshipMaxExp { get { return FriendshipProgress.MaxProgress; } }
        public BitsByte _furnitureusageflags = 0;
        public bool ShareChairWithPlayer { get { return _furnitureusageflags[0]; } set { _furnitureusageflags[0] = value; }}
        public bool ShareBedWithPlayer { get { return _furnitureusageflags[1]; } set { _furnitureusageflags[1] = value; }}
        public bool PlayerSizeMode = false;
        public RequestData request = new RequestData();

        public string GetPlayerNickname(Player player)
        {
            if(player is Player && _PlayerNickname != null) return _PlayerNickname;
            return player.name;
        }

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
            ShareChairWithPlayer = Base.AllowSharingChairWithPlayer;
            ShareBedWithPlayer = Base.AllowSharingBedWithPlayer;
            CombatTactic = Base.DefaultCombatTactic;
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

        public bool IsSameID(CompanionID ID)
        {
            return MyID.IsSameID(ID);
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
            CommonData = CompanionCommonData.GetCommonData(NewID, NewModID);
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
            if(_Name != null) save.Add("CompanionName_" + UniqueID, _Name);
            FriendshipProgress.Save(save, UniqueID);
            save.Add("CompanionFurnitureUsageFlags_" + UniqueID, (byte)_furnitureusageflags);
            save.Add("CompanionCombatTactic_" + UniqueID, (byte)CombatTactic);
            save.Add("CompanionPlayerSize_" + UniqueID, PlayerSizeMode);
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
            save.Add("CompanionMaxBuffs_"+UniqueID, BuffType.Length);
            for(int i = 0; i < BuffType.Length; i++)
            {
                save.Add("CompanionBuffType_" + i + "_" + UniqueID, BuffType[i]);
                save.Add("CompanionBuffTime_" + i + "_" + UniqueID, BuffTime[i]);
            }
            CompanionCommonData.Save(ID, ModID);
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
            if(LastVersion > 1)
                FriendshipProgress.Load(tag, UniqueID, LastVersion);
            if (LastVersion > 3)
                _furnitureusageflags = tag.GetByte("CompanionFurnitureUsageFlags_" + UniqueID);
            if (LastVersion > 4)
                CombatTactic = (CombatTactics)tag.GetByte("CompanionCombatTactic_" + UniqueID);
            if (LastVersion > 5)
                PlayerSizeMode = tag.GetBool("CompanionPlayerSize_" + UniqueID);
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
            int MaxBuffs = 22;
            if(LastVersion >= 7)
            {
                MaxBuffs = tag.GetInt("CompanionMaxBuffs_"+UniqueID);
                BuffType = new int[MaxBuffs];
                BuffTime = new int[MaxBuffs];
            }
            for(int i = 0; i < MaxBuffs; i++)
            {
                BuffType[i] = tag.GetInt("CompanionBuffType_" + i + "_" + UniqueID);
                BuffTime[i] = tag.GetInt("CompanionBuffTime_" + i + "_" + UniqueID);
            }
        }
    }
}