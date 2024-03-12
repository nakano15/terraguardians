using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.IO;

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
        public CompanionCommonData GetCommonData { get { return CommonData; } }
        private string _Name = null;
        public string GetName { get {
             if(_Name == null)
             {
                if (Base.IsInvalidCompanion)
                {
                    return "?" + ID + ":" + ModID;
                }
                string[] NameArray = Base.PossibleNames;
                if (NameArray != null && NameArray.Length > 0 && NameIndex < NameArray.Length)
                    return NameArray[NameIndex];
                return Base.DisplayName;
            } return _Name;
          }
        }
        byte NameIndex = 0;
        public string GetRealName
        {
            get
            {
                string[] NameArray = Base.PossibleNames;
                if (NameArray != null && NameArray.Length > 0 && NameIndex < NameArray.Length)
                    return NameArray[NameIndex];
                return Base.DisplayName;
            }
        }

        public string GetNameWithNickname { get {
             string NameReturn = GetName;
             if (_Name != null)
                NameReturn += " (" + GetRealName +")";
            return NameReturn;
          }
         }
        public Genders Gender { get { if (Base.CanChangeGenders) return _Gender; return Base.Gender; } set { _Gender = value; }}
        private Genders _Gender = Genders.Male;
        public uint ID { get{ return MyID.ID; }}
        public string ModID  { get{ return MyID.ModID; }}
        private uint _Index = uint.MaxValue;
        public uint Index { get { return _Index; } internal set { _Index = value; }}
        private CompanionID MyID = new CompanionID(0);
        public CompanionID GetMyID { get { return MyID; } }
        public int LifeCrystalsUsed { get { return CommonData.LifeCrystalsUsed; } set { CommonData.LifeCrystalsUsed = value; } }
        public int LifeFruitsUsed { get { return CommonData.LifeFruitsUsed; } set { CommonData.LifeFruitsUsed = value; } }
        public int ManaCrystalsUsed { get { return CommonData.ManaCrystalsUsed; } set { CommonData.ManaCrystalsUsed = value; } }
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
        public string OutfitModID = "", SkinModID = "";
        public bool IsStarter = false;
        public CombatTactics CombatTactic = CombatTactics.MidRange;
        public byte FriendshipLevel { get { return FriendshipProgress.Level; } }
        public sbyte FriendshipExp { get { return FriendshipProgress.Progress; } }
        public byte FriendshipMaxExp { get { return FriendshipProgress.MaxProgress; } }
        public BitsByte _furnitureusageflags = 0;
        public bool ShareChairWithPlayer { get { return _furnitureusageflags[0]; } set { _furnitureusageflags[0] = value; }}
        public bool ShareBedWithPlayer { get { return _furnitureusageflags[1]; } set { _furnitureusageflags[1] = value; }}
        public bool PlayerSizeMode = false;
        public BitsByte _behaviorflags = 0;
        public bool FollowAhead { get { return _behaviorflags[0]; } set { _behaviorflags[0] = value; } }
        public bool AvoidCombat { get { return _behaviorflags[1]; } set { _behaviorflags[1] = value; } }
        public bool UnallowAutoUseSubattacks { get { return _behaviorflags[2]; } set { _behaviorflags[2] = value; } }
        public bool PrioritizeHelpingAlliesOverFighting { get { return _behaviorflags[3]; } set { _behaviorflags[3] = value; } }
        public bool TakeLootPlayerTrashes { get { return _behaviorflags[4]; } set { _behaviorflags[4] = value; } }
        public bool AutoSellItemsWhenInventoryIsFull { get { return _behaviorflags[5]; } set { _behaviorflags[5] = value; } }
        public bool AttackOwnerTarget { get { return _behaviorflags[6]; } set { _behaviorflags[6] = value; } }
        private RequestData request;
        public RequestData GetRequest { get { return request; } }
        public UnlockAlertMessageContext UnlockAlertsDone = 0;
        internal PlayerFileData FileData = null;
        protected virtual uint CustomSaveVersion { get{ return 0; } }
        TerrarianCompanionInfo GenericCompanionInfo = null;
        public bool IsGeneric { get { return Base.IsGeneric; }}
        public TerrarianCompanionInfo GetGenericCompanionInfo { get { return GenericCompanionInfo; }}
        ushort GenericID = 0;
        public ushort GetGenericID { get { return GenericID; } }
        public const int MaxSubAttackSlots = 4;
        byte[] _SubAttackIndexes = new byte[] { 0, 1, 2, 3 };
        public byte[] GetSubAttackIndexes { get { return _SubAttackIndexes; } internal set { _SubAttackIndexes = value; } }

        internal void ChangeGenericCompanionInfo(TerrarianCompanionInfo New)
        {
            GenericCompanionInfo = New;
        }

        public string GetPlayerNickname(Player player)
        {
            if(player is Player && _PlayerNickname != null) return _PlayerNickname;
            return player.name;
        }

        public void ChangePlayerNickname(string NewNickname)
        {
            _PlayerNickname = NewNickname;
        }

        public string GetNameColored()
        {
            return Base.GetNameColored(this);
        }

        public CompanionData()
        {
            request = new RequestData(this);
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
            _SubAttackIndexes = new byte[MaxSubAttackSlots];
            for (int i = 0; i < MaxSubAttackSlots; i++)
            {
                _SubAttackIndexes[i] = (byte)i;
            }
            ShareChairWithPlayer = Base.AllowSharingChairWithPlayer;
            ShareBedWithPlayer = Base.AllowSharingBedWithPlayer;
            CombatTactic = Base.DefaultCombatTactic;
        }

        public void SetSaveData(Player owner)
        {
            PlayerFileData fileData = null;
            foreach(PlayerFileData d in Main.PlayerList)
            {
                if (d.Player == owner)
                {
                    fileData = d;
                    break;
                }
            }
            if (fileData == null) return;
            string CompanionSavePath = fileData.Path.Remove(fileData.Path.Length - 4) + "\\Companions";
            Main.NewText(CompanionSavePath);
            if (!Directory.Exists(CompanionSavePath))
                Directory.CreateDirectory(CompanionSavePath);
            FileData = new PlayerFileData(CompanionSavePath + "\\" + ID + ":" + ModID + ".sav", false);
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
            InitialItemDefinition[] Equips = new InitialItemDefinition[20];
            for(int i = 0; i < Equips.Length; i++)
            {
                Equips[i] = new InitialItemDefinition(0);
            }
            if (MainMod.MrPlagueRacesInstalled && Base.CompanionType == CompanionTypes.Terrarian)
            {
                Equips[11] = new InitialItemDefinition(Terraria.ID.ItemID.FamiliarShirt);
                Equips[12] = new InitialItemDefinition(Terraria.ID.ItemID.FamiliarPants);
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

        public void Update(Player owner)
        {
            request.UpdateRequest(owner, this);
            FriendshipProgress.UpdateFriendship();
            CustomUpdate(owner);
        }

        protected virtual void CustomUpdate(Player owner)
        {

        }

        public bool IsSameID(uint ID, string ModID = "")
        {
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
            Gender = Base.Gender;
            if(Base.CanChangeGenders && Base.RandomGenderOnSpawn)
            {
                Gender = Main.rand.NextFloat() < 0.5f ? Genders.Male : Genders.Female;
            }
            _Name = null;
            string[] PossibleNames = Base.PossibleNames;
            if (PossibleNames != null && PossibleNames.Length > 0)
                NameIndex = (byte)(Main.rand.Next(0, (int)System.Math.Min(PossibleNames.Length, 255)));
            else
                NameIndex = 0;
            PrioritizeHelpingAlliesOverFighting = Base.HelpAlliesOverFighting;
            if (Base.IsGeneric)
            {
                GenericCompanionInfo = new TerrarianCompanionInfo();
                GenericCompanionRandomizer.RandomizeCompanion(this);
                AssignGenericID();
            }
            else
            {
                GenericCompanionInfo = null;
                GenericID = 0;
            }
            CombatTactic = Base.DefaultCombatTactic;
        }

        internal void AssignGenericID()
        {
            if (Main.gameMenu) return;
            retry:
            GenericID = (ushort)Main.rand.Next(1, ushort.MaxValue);
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (c.IsGeneric && c.GenericID == GenericID) goto retry;
            }
            foreach(uint d in MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().GetCompanionDataKeys)
            {
                CompanionData cd = PlayerMod.PlayerGetCompanionDataByIndex(MainMod.GetLocalPlayer, d);
                if (cd.IsGeneric && cd.GenericID == GenericID) goto retry;
            }
        }

        internal void AssignGenericID(ushort SpecificID)
        {
            GenericID = SpecificID;
        }

        public void ChangeName(string NewName)
        {
            _Name = NewName;
        }
        
        public void Save(TagCompound save, uint UniqueID)
        {
            save.Add("CompanionHasNameSet_" + UniqueID, _Name != null);
            if(_Name != null) save.Add("CompanionName_" + UniqueID, _Name);
            save.Add("CompanionNameIndex_" + UniqueID, NameIndex);
            save.Add("CompanionGender_" + UniqueID, (byte)_Gender);
            save.Add("CompanionHasNicknameSet_" + UniqueID, _PlayerNickname != null);
            if (_PlayerNickname == null)
                save.Add("CompanionPlayerNickname_" + UniqueID, _PlayerNickname);
            FriendshipProgress.Save(save, UniqueID);
            save.Add("CompanionIsGeneric_"+UniqueID, IsGeneric);
            if (IsGeneric)
            {
                GenericCompanionInfo.Save(save, UniqueID);
                save.Add("CompanionGenericID_" + UniqueID, GenericID);
            }
            save.Add("CompanionSkinID_" + UniqueID, SkinID);
            save.Add("CompanionSkinModID_" + UniqueID, SkinModID);
            save.Add("CompanionOutfitID_" + UniqueID, OutfitID);
            save.Add("CompanionOutfitModID_" + UniqueID, OutfitModID);
            save.Add("CompanionFurnitureUsageFlags_" + UniqueID, (byte)_furnitureusageflags);
            save.Add("CompanionBehaviorFlags_" + UniqueID, (byte)_behaviorflags);
            save.Add("CompanionCombatTactic_" + UniqueID, (byte)CombatTactic);
            save.Add("CompanionPlayerSize_" + UniqueID, PlayerSizeMode);
            save.Add("CompanionSubAtkSlotsCount_" + UniqueID, MaxSubAttackSlots);
            for (int i = 0; i < _SubAttackIndexes.Length; i++)
            {
                save.Add("CompanionSubAtkSlot_"+i+"_" + UniqueID, _SubAttackIndexes[i]);
            }
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
                ModBuff buff = ModContent.GetModBuff(BuffType[i]);
                save.Add("CompanionIsModBuff_" + i + "_" + UniqueID, buff != null);
                if(buff != null)
                {
                    save.Add("CompanionBuffType_" + i + "_" + UniqueID, buff.Name);
                    save.Add("CompanionBuffTypeMod_" + i + "_" + UniqueID, buff.Mod.Name);
                }
                else
                {
                    save.Add("CompanionBuffType_" + i + "_" + UniqueID, BuffType[i]);
                }
                save.Add("CompanionBuffTime_" + i + "_" + UniqueID, BuffTime[i]);
            }
            save.Add("UnlockNotifications" + UniqueID, (byte)UnlockAlertsDone);
            request.Save(UniqueID, save);
            save.Add("LastCustomSaveVersion_" + UniqueID, CustomSaveVersion);
            CustomSave(save, UniqueID);
            CompanionCommonData.Save(ID, ModID);
        }

        public void Load(TagCompound tag, uint UniqueID, uint LastVersion)
        {
            if(tag.GetBool("CompanionHasNameSet_" + UniqueID))
                _Name = tag.GetString("CompanionName_" + UniqueID);
            if (LastVersion >= 32)
            {
                NameIndex = tag.GetByte("CompanionNameIndex_" + UniqueID);
            }
            if (LastVersion >= 11)
                _Gender = (Genders)tag.GetByte("CompanionGender_" + UniqueID);
            if (LastVersion >= 42 && tag.GetBool("CompanionHasNicknameSet_" + UniqueID))
            {
                _PlayerNickname = tag.GetString("CompanionPlayerNickname_" + UniqueID);
            }
            if(LastVersion > 1)
                FriendshipProgress.Load(tag, UniqueID, LastVersion);
            if (LastVersion >= 33)
            {
                if (tag.GetBool("CompanionIsGeneric_"+UniqueID))
                {
                    if (GenericCompanionInfo == null)
                    {
                        GenericCompanionInfo = new TerrarianCompanionInfo();
                    }
                    GenericCompanionInfo.Load(tag, UniqueID, LastVersion);
                    if (LastVersion >= 36)
                    {
                        GenericID = tag.Get<ushort>("CompanionGenericID_" + UniqueID);
                    }
                }
            }
            if (LastVersion >= 31)
            {
                SkinID = tag.GetByte("CompanionSkinID_" + UniqueID);
                SkinModID = tag.GetString("CompanionSkinModID_" + UniqueID);
                OutfitID = tag.GetByte("CompanionOutfitID_" + UniqueID);
                OutfitModID = tag.GetString("CompanionOutfitModID_" + UniqueID);
            }
            if (LastVersion > 3)
                _furnitureusageflags = tag.GetByte("CompanionFurnitureUsageFlags_" + UniqueID);
            if (LastVersion > 29)
                _behaviorflags = tag.GetByte("CompanionBehaviorFlags_" + UniqueID);
            if (LastVersion > 4)
                CombatTactic = (CombatTactics)tag.GetByte("CompanionCombatTactic_" + UniqueID);
            if (LastVersion > 5)
                PlayerSizeMode = tag.GetBool("CompanionPlayerSize_" + UniqueID);
            if (LastVersion >= 38)
            {
                int MaxSlots = tag.GetInt("CompanionSubAtkSlotsCount_" + UniqueID);
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (i >= MaxSubAttackSlots) break;
                    _SubAttackIndexes[i] = tag.GetByte("CompanionSubAtkSlot_"+i+"_" + UniqueID);
                }
            }
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
                if (LastVersion < 20)
                {
                    BuffType[i] = System.Math.Min(tag.GetInt("CompanionBuffType_" + i + "_" + UniqueID), Terraria.ID.BuffID.Count);
                }
                else
                {
                    bool IsModBuff = tag.GetBool("CompanionIsModBuff_" + i + "_" + UniqueID);
                    if(IsModBuff)
                    {
                        string ItemName = tag.GetString("CompanionBuffType_" + i + "_" + UniqueID);
                        string ModName = tag.GetString("CompanionBuffTypeMod_" + i + "_" + UniqueID);
                        if (ModLoader.HasMod(ModName)) //Not working...
                        {
                            Mod mod = ModLoader.GetMod(ModName);
                            ModBuff m;
                            if(mod.TryFind<ModBuff>(ItemName, out m))
                                BuffType[i] = m.Type;
                            else
                                BuffType[i] = 0;
                        }
                        else
                        {
                            BuffType[i] = 0;
                        }
                        //BuffType[i] = 0;
                    }
                    else
                    {
                        BuffType[i] = tag.GetInt("CompanionBuffType_" + i + "_" + UniqueID);
                    }
                }
                BuffTime[i] = tag.GetInt("CompanionBuffTime_" + i + "_" + UniqueID);
                if (BuffType[i] < 0)
                {
                    BuffType[i] = 0;
                    BuffTime[i] = 0;
                }
            }
            if(LastVersion >= 12)
                UnlockAlertsDone = (UnlockAlertMessageContext)tag.GetByte("UnlockNotifications" + UniqueID);
            if (LastVersion >= 8)
                request.Load(UniqueID, LastVersion, tag);
            if (LastVersion >= 29)
            {
                uint LastCustomSaveVersion = tag.Get<uint>("LastCustomSaveVersion_"+ UniqueID);
                CustomLoad(tag, UniqueID, LastCustomSaveVersion);
            }
        }

        public virtual void CustomSave(TagCompound save, uint UniqueID)
        {

        }

        public virtual void CustomLoad(TagCompound tag, uint UniqueID, uint LastVersion)
        {

        }
    }
}