using Terraria;
using Terraria.ModLoader;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionCommonData
    {
        private static Dictionary<string, CompanionCommonDataContainer> CompanionsDataContainer = new Dictionary<string, CompanionCommonDataContainer>();
        private static CompanionSkillData DefaultSkillData = new CompanionSkillData(uint.MaxValue, MainMod.GetModName);

        public int MaxHealth = 100;
        public int MaxMana = 20;
        public bool ExtraAccessorySlot = false;
        private Dictionary<string, CompanionCommonSkillContainer> Skills = new Dictionary<string, CompanionCommonSkillContainer>();

        public static string GetSaveFolder{ get { return Main.SavePath + "/Companion Infos"; } }
        
        public CompanionCommonData()
        {
            foreach(string modid in CompanionSkillContainer.GetSkillModKeys())
            {
                CompanionCommonSkillContainer container = new CompanionCommonSkillContainer();
                Skills.Add(modid, container);
                CompanionSkillContainer.CompanionModSkillsContainer modSkillsContainer = CompanionSkillContainer.GetModSkillContainer(modid);
                foreach(uint id in modSkillsContainer.GetSkillIDs())
                {
                    container.AddSkill(id, new CompanionSkillData(id, modid));
                }
            }
        }

        public CompanionSkillData[] GetSkillDatas()
        {
            List<CompanionSkillData> skills = new List<CompanionSkillData>();
            foreach(string modid in Skills.Keys)
            {
                Skills[modid].GetSkills(skills);
            }
            return skills.ToArray();
        }

        public CompanionSkillData GetSkillData(uint ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!Skills.ContainsKey(ModID)) return DefaultSkillData;
            return Skills[ModID].GetSkill(ID);
        }

        private void PlaceSkillData(CompanionSkillData data, uint ID, string ModID)
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!Skills.ContainsKey(ModID))
                Skills.Add(ModID, new CompanionCommonSkillContainer());
            CompanionCommonSkillContainer container = Skills[ModID];
            container.ReOrPlaceSkill(ID, data);
        }

        public void UpdateSkills(Companion companion)
        {
            foreach(string modid in Skills.Keys)
            {
                Skills[modid].UpdateSkills(companion);
            }
        }

        public void IncreaseSkillProgress(float Progress, uint ID, string ModID = "")
        {
            GetSkillData(ID, ModID).AddProgress(Progress);
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
            if (MainMod.DebugMode) return;
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
                    writer.Write(status.MaxHealth);
                    writer.Write(status.MaxMana);
                    BitsByte ExtraInfo = new BitsByte();
                    ExtraInfo[0] = status.ExtraAccessorySlot;
                    writer.Write(ExtraInfo);
                    foreach(string modid in status.Skills.Keys)
                    {
                        CompanionCommonSkillContainer container = status.Skills[modid];
                        foreach(uint data in container.GetSkillIDs())
                        {
                            writer.Write(true);
                            container.GetSkill(data).Save(writer);
                        }
                    }
                    writer.Write(false);
                }
            }
        }

        public static CompanionCommonData Load(uint CompanionID, string CompanionModID = "")
        {
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            CompanionBase Base = MainMod.GetCompanionBase(CompanionID, CompanionModID);
            string SaveDirectory = GetSaveFolder + "/" + CompanionModID;
            if (!Directory.Exists(SaveDirectory)) return new CompanionCommonData();
            string SaveFile = SaveDirectory + "/" + Base.Name + ".tgf";
            if (!File.Exists(SaveFile)) return new CompanionCommonData();
            CompanionCommonData status = new CompanionCommonData();
            using (FileStream stream = new FileStream(SaveFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    uint ModVersion = reader.ReadUInt32();
                    status.MaxHealth = reader.ReadInt32();
                    status.MaxMana = reader.ReadInt32();
                    BitsByte ExtraInfo = reader.ReadByte();
                    status.ExtraAccessorySlot = ExtraInfo[0];
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
                }
            }
            return status;
        }

        public static void OnUnload()
        {
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

        public class CompanionCommonSkillContainer
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
                foreach(CompanionSkillData data in Skills.Values) data.UpdateStatus(companion);
            }
        }
    }
}