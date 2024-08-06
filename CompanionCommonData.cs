using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionCommonData
    {
        private static Dictionary<string, CompanionCommonDataContainer> CompanionsDataContainer = new Dictionary<string, CompanionCommonDataContainer>();
        internal static CompanionSkillData DefaultSkillData;

        public int MaxHealth = 100;
        public int MaxMana = 20;
        public int LifeCrystalsUsed = 0, LifeFruitsUsed = 0, ManaCrystalsUsed = 0;
        public BitsByte PermanentBuffs1 = new BitsByte();
        internal Dictionary<string, CompanionSkillDataContainer> Skills = new Dictionary<string, CompanionSkillDataContainer>();
        public bool VitalCrystalUsed { get { return PermanentBuffs1[0]; } set { PermanentBuffs1[0] = value; }}
        public bool ArcaneCrystalUsed { get { return PermanentBuffs1[1]; } set { PermanentBuffs1[1] = value; }}
        public bool AegisFruitUsed { get { return PermanentBuffs1[2]; } set { PermanentBuffs1[2] = value; }}
        public bool AmbrosiaUsed { get { return PermanentBuffs1[3]; } set { PermanentBuffs1[3] = value; }}
        public bool GummyWormUsed { get { return PermanentBuffs1[4]; } set { PermanentBuffs1[4] = value; }}
        public bool GalaxyPearlUsed { get { return PermanentBuffs1[5]; } set { PermanentBuffs1[5] = value; }}

        public static string GetSaveFolder{ get { return Main.SavePath + "/Companion Infos"; } }
        public virtual uint Version => 0;
        
        public CompanionCommonData()
        {
            CompanionSkillDataContainer.CreateSkillDatasContainers(ref Skills);
        }

        internal static void OnLoad()
        {
            DefaultSkillData = new CompanionSkillData(uint.MaxValue, MainMod.GetModName);
        }

        private void PlaceSkillData(CompanionSkillData data, uint ID, string ModID)
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!Skills.ContainsKey(ModID))
                Skills.Add(ModID, new CompanionSkillDataContainer());
            CompanionSkillDataContainer container = Skills[ModID];
            container.ReOrPlaceSkill(ID, data);
        }

        public static CompanionCommonData GetCommonData(uint CompanionID, string CompanionModID = "")
        {
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            if (!CompanionsDataContainer.ContainsKey(CompanionModID))
            {
                CompanionsDataContainer.Add(CompanionModID, new CompanionCommonDataContainer());
            }
            return CompanionsDataContainer[CompanionModID].GetCompanionData(CompanionID, CompanionModID);
        }

        public static void Save(uint CompanionID, string CompanionModID = "")
        {
            if (MainMod.IsDebugMode || SystemMod.IsQuittingWorldDebugMode) return;
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            CompanionBase Base = MainMod.GetCompanionBase(CompanionID, CompanionModID);
            string SaveDirectory = GetSaveFolder + "/" + CompanionModID;
            if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);
            string SaveFile = SaveDirectory + "/" + Base.Name + ".tgf";
            if (File.Exists(SaveFile)) File.Delete(SaveFile);
            CompanionCommonData status = GetCommonData(CompanionID, CompanionModID);
            using (FileStream stream = new FileStream(SaveFile, FileMode.CreateNew))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MainMod.ModVersion);
                    writer.Write(status.LifeCrystalsUsed);
                    writer.Write(status.LifeFruitsUsed);
                    writer.Write(status.ManaCrystalsUsed);
                    writer.Write(status.PermanentBuffs1);
                    BitsByte ExtraInfo = new BitsByte();
                    writer.Write(ExtraInfo);
                    foreach(string modid in status.Skills.Keys)
                    {
                        CompanionSkillDataContainer container = status.Skills[modid];
                        foreach(uint data in container.GetSkillIDs())
                        {
                            writer.Write(true);
                            container.GetSkill(data).Save(writer);
                        }
                    }
                    writer.Write(false);
                    //
                    TagCompound save = new TagCompound();
                    status.SaveHook(save, CompanionID, CompanionModID);
                    writer.Write(status.Version);
                    TagIO.Write(save, writer);
                }
            }
        }

        public static CompanionCommonData Load(uint CompanionID, string CompanionModID = "")
        {
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            CompanionBase Base = MainMod.GetCompanionBase(CompanionID, CompanionModID);
            string SaveDirectory = GetSaveFolder + "/" + CompanionModID;
            CompanionCommonData status = Base.CreateCompanionCommonData;
            if (!Directory.Exists(SaveDirectory)) return status;
            string SaveFile = SaveDirectory + "/" + Base.Name + ".tgf";
            if (!File.Exists(SaveFile)) return status;
            using (FileStream stream = new FileStream(SaveFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    uint ModVersion = reader.ReadUInt32();
                    if (ModVersion < 28)
                    {
                        int MaxHealth = reader.ReadInt32();
                        int MaxMana = reader.ReadInt32();
                        if (MaxHealth > 400)
                        {
                            status.LifeCrystalsUsed = 15;
                            status.LifeFruitsUsed = (MaxHealth - 400) / 5;
                            if (status.LifeFruitsUsed > 20) status.LifeFruitsUsed = 20;
                        }
                        status.ManaCrystalsUsed = (MaxMana - 20) / 20;
                        if (status.ManaCrystalsUsed > 9)
                            status.ManaCrystalsUsed = 9;
                    }
                    else
                    {
                        status.LifeCrystalsUsed = reader.ReadInt32();
                        status.LifeFruitsUsed = reader.ReadInt32();
                        status.ManaCrystalsUsed = reader.ReadInt32();
                    }
                    if (ModVersion >= 41)
                    {
                        status.PermanentBuffs1 = reader.ReadByte();
                    }
                    BitsByte ExtraInfo = reader.ReadByte();
                    if (ModVersion >= 10)
                    {
                        while(reader.ReadBoolean())
                        {
                            CompanionSkillData data = new CompanionSkillData();
                            data.Load(reader, ModVersion);
                            status.PlaceSkillData(data, data.GetID, data.GetModID);
                        }
                    }
                    if (ModVersion < 22)
                    {
                        if (status.Skills.ContainsKey(MainMod.GetModName))
                        {
                            CompanionSkillData data = status.Skills[MainMod.GetModName].GetSkill(CompanionSkillContainer.AthleticsID);
                            if (data != null)
                            {
                                data.Level = (int)(data.Level * 0.1f);
                                data.Progress = 0;
                                data.UpdateMaxProgress();
                            }
                            data = status.Skills[MainMod.GetModName].GetSkill(CompanionSkillContainer.AcrobaticsID);
                            if (data != null)
                            {
                                data.Level = (int)(data.Level * 0.1f);
                                data.Progress = 0;
                                data.UpdateMaxProgress();
                            }
                        }
                    }
                    if (ModVersion >= 40)
                    {
                        uint LastVersion = reader.ReadUInt32();
                        TagCompound Load = TagIO.Read(reader);
                        status.LoadHook(Load, LastVersion, CompanionID, CompanionModID);
                    }
                }
            }
            return status;
        }
        
        protected virtual void SaveHook(TagCompound tag, uint CompanionID, string CompanionModID = "")
        {
            
        }
        
        protected virtual void LoadHook(TagCompound tag, uint LastVersion, uint CompanionID, string CompanionModID = "")
        {
            
        }

        public static void OnUnload()
        {
            DefaultSkillData = null;
            foreach(CompanionCommonDataContainer d in CompanionsDataContainer.Values)
            {
                d.Unload();
            }
            CompanionsDataContainer.Clear();
            CompanionsDataContainer = null;
        }

        public class CompanionCommonDataContainer
        {
            private Dictionary<uint, CompanionCommonData> CompanionDatas = new Dictionary<uint, CompanionCommonData>();

            public void AddCompanionData(uint ID, string ModID = "")
            {
                CompanionDatas.Add(ID, Load(ID, ModID));
            }

            public CompanionCommonData GetCompanionData(uint ID, string ModID = "")
            {
                if (!CompanionDatas.ContainsKey(ID))
                {
                    AddCompanionData(ID, ModID);
                }
                return CompanionDatas[ID];
            }

            public void Unload()
            {
                CompanionDatas.Clear();
                CompanionDatas = null;
            }
        }

        public class CompanionSkillDataContainer
        {
            private Dictionary<uint, CompanionSkillData> Skills = new Dictionary<uint, CompanionSkillData>();

            public void AddSkill(uint SkillID, CompanionSkillData Data)
            {
                Skills.Add(SkillID, Data);
            }

            public CompanionSkillData GetSkill(uint SkillID)
            {
                if (Skills.ContainsKey(SkillID))
                    return Skills[SkillID];
                return DefaultSkillData;
            }

            internal void GetSkills(List<CompanionSkillData> SkillList)
            {
                foreach(uint i in Skills.Keys)
                    SkillList.Add(Skills[i]);
            }

            public uint[] GetSkillIDs()
            {
                return Skills.Keys.ToArray();
            }

            internal void ReOrPlaceSkill(uint SkillID, CompanionSkillData Data)
            {
                if(Skills.ContainsKey(SkillID))
                    Skills[SkillID] = Data;
                else
                    Skills.Add(SkillID, Data);
            }

            internal void UpdateSkills(Companion companion)
            {
                if (!MainMod.SkillsEnabled) return;
                foreach(CompanionSkillData data in Skills.Values) data.UpdateStatus(companion);
            }

            internal static void CreateSkillDatasContainers(ref Dictionary<string, CompanionSkillDataContainer> skills)
            {
                foreach(string modid in CompanionSkillContainer.GetSkillModKeys())
                {
                    CompanionSkillDataContainer container = new CompanionSkillDataContainer();
                    skills.Add(modid, container);
                    CompanionSkillContainer.CompanionModSkillsContainer modSkillsContainer = CompanionSkillContainer.GetModSkillContainer(modid);
                    foreach(uint id in modSkillsContainer.GetSkillIDs())
                    {
                        container.AddSkill(id, new CompanionSkillData(id, modid));
                    }
                }
            }
        }
    }
}