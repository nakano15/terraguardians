using Terraria;
using Terraria.ModLoader;
using System.IO;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionCommonData
    {
        private static Dictionary<string, CompanionCommonDataContainer> CompanionsDataContainer = new Dictionary<string, CompanionCommonDataContainer>();

        public int MaxHealth = 100;
        public int MaxMana = 20;
        public bool ExtraAccessorySlot = false;

        public static string GetSaveFolder{ get { return Main.SavePath + "/Companion Infos"; } }
        
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
                    uint ModID = reader.ReadUInt32();
                    status.MaxHealth = reader.ReadInt32();
                    status.MaxMana = reader.ReadInt32();
                    BitsByte ExtraInfo = reader.ReadByte();
                    status.ExtraAccessorySlot = ExtraInfo[0];
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
    }
}