using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.IO;

namespace terraguardians
{
    public class GenericCompanionInfos
    {
        static string GetSaveFolder => CompanionCommonData.GetSaveFolder;
        static string GetSaveFile => "GenericCompanionInfos.sav";
        internal static Dictionary<ushort, GenericCompanionInfos> CompanionInfos = new Dictionary<ushort, GenericCompanionInfos>();
		internal static int MaxGenericCompanionsOnMemory = 100;

        internal static GenericCompanionInfos GetCompanionInfo(ushort ID)
        {
            if (!CompanionInfos.ContainsKey(ID))
            {
                GenericCompanionInfos NewInfo = new GenericCompanionInfos();
                CompanionInfos.Add(ID, NewInfo);
            }
            return CompanionInfos[ID];
        }

        internal static ushort GetRandomGenericOfType(uint ID, string ModID)
        {
            ushort GenericID = 0;
            foreach (ushort k in CompanionInfos.Keys)
            {
                GenericCompanionInfos info = CompanionInfos[k];
                if (info.ID == ID && info.ModID == ModID)
                {
                    if (!MainMod.HasCompanionInWorld(ID, k, ModID) && (GenericID == 0 || Main.rand.Next(0, 2) == 0))
                    {
                        GenericID = k;
                    }
                }
            }
            return GenericID;
        }

        internal static void DepleteLifeTime()
        {
            ushort[] Keys = CompanionInfos.Keys.ToArray();
            foreach (ushort k in Keys)
            {
                GenericCompanionInfos info = CompanionInfos[k];
                if (info.LifeTime > 0) info.LifeTime--;
                if (info.LifeTime <= 0 && !MainMod.HasCompanionInWorld(info.ID, k, info.ModID))
                {
                    CompanionInfos.Remove(k);
                }
            }
        }

        internal static bool CanCreateNewGenericEntry()
        {
            return CompanionInfos.Count < MaxGenericCompanionsOnMemory;
        }

        internal static ushort GetRandomID()
        {
            while (true)
            {
                ushort ID = (ushort)Main.rand.Next(1, ushort.MaxValue);
                if (!CompanionInfos.ContainsKey(ID))
                    return ID;
            }
        }

        internal static void EraseCompanionInfo(ushort GenericID)
        {
            if (CompanionInfos.ContainsKey(GenericID))
                CompanionInfos.Remove(GenericID);
        }

        internal static void SaveGenericInfos()
        {
            string SaveDirectory = GetSaveFolder;
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);
            string SaveFile = SaveDirectory + "/" + GetSaveFile;
            using (FileStream stream = new FileStream(SaveFile, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MainMod.ModVersion);
                    writer.Write(CompanionInfos.Count);
                    foreach (ushort k in CompanionInfos.Keys)
                    {
                        GenericCompanionInfos info = CompanionInfos[k];
                        writer.Write(k);
                        using (MemoryStream SaveStream = new MemoryStream())
                        {
                            TagCompound save = new TagCompound();
                            info.Save(save);
                            TagIO.ToStream(save, SaveStream);
                            byte[] SaveBytes = SaveStream.ToArray();
                            writer.Write(SaveBytes.Length);
                            writer.Write(SaveBytes);
                        }
                    }
                }
            }
        }

        internal static void LoadGenericInfos()
        {
            string SaveDirectory = GetSaveFolder;
            if (!Directory.Exists(SaveDirectory))
                return;
            string SaveFile = SaveDirectory + "/" + GetSaveFile;
            if (!File.Exists(SaveFile))
                return;
            CompanionInfos.Clear();
            using (FileStream stream = new FileStream(SaveFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    uint Version = reader.ReadUInt32();
                    int IdsToRead = reader.ReadInt32();
                    while (IdsToRead-- > 0)
                    {
                        ushort GID = reader.ReadUInt16();
                        GenericCompanionInfos info = new GenericCompanionInfos();
                        int Length = reader.ReadInt32();
                        byte[] Array = reader.ReadBytes(Length);
                        using (MemoryStream LoadStream = new MemoryStream(Array))
                        {
                            TagCompound load = TagIO.FromStream(LoadStream);
                            info.Load(load, Version);
                        }
                        CompanionInfos.Add(GID, info);
                    }
                }
            }
        }

        internal static void ClearTemporaryCompanionInfos()
        {
            ushort[] Keys = CompanionInfos.Keys.ToArray();
            foreach (ushort k in Keys)
            {
                if (CompanionInfos[k].IsTemporary)
                {
                    CompanionInfos.Remove(k);
                }
            }
        }

        internal static void Unload()
        {
            CompanionInfos.Clear();
            CompanionInfos = null;
        }
        
        #region Infos
        uint ID = 0;
        string ModID = "";
        uint GetID => ID;
        string GetModID => ModID;
        internal int LifeTime = 0;
        bool Temporary = true;
        public bool IsTemporary 
        {
            get
            {
                return LifeTime <= 0 && Temporary;
            }
            set
            {
                Temporary = value;
            }
        }
        public string Name = "";
        public Genders Gender = Genders.Male;
        public int HairStyle = 1;
        public int SkinVariant = 1;
        public Color HairColor = new Color(),
            EyeColor = new Color(),
            SkinColor = new Color(),
            ShirtColor = new Color(),
            UndershirtColor = new Color(),
            PantsColor = new Color(),
            ShoesColor = new Color();
        #endregion

        internal void SetIDs(uint ID, string ModID)
        {
            if (ModID == "") ModID = MainMod.GetModName;
            this.ID = ID;
            this.ModID = ModID;
        }

        #region Data Saving and Loading
        public void Save(TagCompound save)
        {
            save.Add("Name", Name);
            save.Add("ID", ID);
            save.Add("ModID", ModID);
            save.Add("Hair", HairStyle);
            save.Add("Skin", SkinVariant);
            save.Add("HairColor", HairColor);
            save.Add("EyeColor", EyeColor);
            save.Add("SkinColor", SkinColor);
            save.Add("ShirtColor", ShirtColor);
            save.Add("UShirtColor", UndershirtColor);
            save.Add("PantsColor", PantsColor);
            save.Add("ShoesColor", ShoesColor);
            save.Add("LifeTime", LifeTime);
        }
        
        public void Load(TagCompound tag, uint LastVersion)
        {
            Name = tag.GetString("Name");
            if (LastVersion >= 51)
            {
                ID = tag.Get<uint>("ID");
                ModID = tag.GetString("ModID");
            }
            HairStyle = tag.GetInt("Hair");
            SkinVariant = tag.GetInt("Skin");
            HairColor = tag.Get<Color>("HairColor");
            EyeColor = tag.Get<Color>("EyeColor");
            SkinColor = tag.Get<Color>("SkinColor");
            ShirtColor = tag.Get<Color>("ShirtColor");
            UndershirtColor = tag.Get<Color>("UShirtColor");
            PantsColor = tag.Get<Color>("PantsColor");
            ShoesColor = tag.Get<Color>("ShoesColor");
            LifeTime = tag.Get<int>("LifeTime");
        }
        #endregion
    }
}