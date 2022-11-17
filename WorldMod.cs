using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Renderers;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace terraguardians
{
    public class WorldMod
    {
        private static List<CompanionID> CompanionsMet = new List<CompanionID>();
        public const int MaxCompanionNpcsInWorld = 30;
        public static List<Companion> CompanionNPCs = new List<Companion>();
        public static CompanionTownNpcState[] CompanionNPCsInWorld = new CompanionTownNpcState[MaxCompanionNpcsInWorld];
        public static int CompanionsMetCount { get{ return CompanionsMet.Count; } }
        public static CompanionID[] StarterCompanions = new CompanionID[0];
        public static List<BuildingInfo> HouseInfos = new List<BuildingInfo>();

        public static void OnUnload()
        {
            StarterCompanions = null;
            CompanionNPCsInWorld = null;
            CompanionNPCs.Clear();
            CompanionNPCs = null;
            CompanionsMet.Clear();
            CompanionsMet = null;
            HouseInfos.Clear();
            HouseInfos = null;
        }

        internal static void OnInitializeWorldGen()
        {
            CompanionsMet.Clear();
            CompanionNPCs.Clear();
            CompanionNPCsInWorld = new CompanionTownNpcState[MaxCompanionNpcsInWorld];
            StarterCompanions = new CompanionID[0];
            HouseInfos.Clear();
        }

        public static bool IsStarterCompanion(Companion companion)
        {
            foreach(CompanionID id in StarterCompanions)
            {
                if(companion.IsSameID(id)) return true;
            }
            return false;
        }

        public static bool IsStarterCompanion(CompanionData data)
        {
            foreach(CompanionID id in StarterCompanions)
            {
                if(data.IsSameID(id)) return true;
            }
            return false;
        }

        public static bool IsStarterCompanion(uint ID, string ModID = "")
        {
            foreach(CompanionID id in StarterCompanions)
            {
                if(id.IsSameID(ID, ModID)) return true;
            }
            return false;
        }

        public static bool IsStarterCompanion(CompanionID ID)
        {
            foreach(CompanionID id in StarterCompanions)
            {
                if(id.IsSameID(ID)) return true;
            }
            return false;
        }

        public static bool HasMetCompanion(CompanionID ID)
        {
            foreach(CompanionID id in CompanionsMet)
            {
                if (id.IsSameID(ID)) return true;
            }
            return false;
        }

        public static bool HasMetCompanion(uint ID, string ModID = "")
        {
            foreach(CompanionID id in CompanionsMet)
            {
                if (id.IsSameID(ID, ModID)) return true;
            }
            return false;
        }

        public static void AddCompanionMet(uint ID, string ModID = "")
        {
            AddCompanionMet(new CompanionID(ID, ModID));
        }

        public static void AddCompanionMet(CompanionID ID)
        {
            if(!HasMetCompanion(ID))
            {
                CompanionsMet.Add(ID);
            }
        }

        public static void AddCompanionMet(CompanionData data)
        {
            AddCompanionMet(data.ID, data.ModID);
        }

        public static Companion SpawnCompanionNPC(CompanionID ID)
        {
            return SpawnCompanionNPC(ID.ID, ID.ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, CompanionID ID)
        {
            return SpawnCompanionNPC(SpawnPosition, ID.ID, ID.ModID);
        }

        public static Companion SpawnCompanionNPC(uint ID, string ModID = "")
        {
            return SpawnCompanionNPC(Vector2.Zero, ID, ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, uint ID, string ModID = "")
        {
            Companion c = SpawnPosition.Length() > 0 ? MainMod.SpawnCompanion(SpawnPosition, ID, ModID) : MainMod.SpawnCompanion(ID, ModID);
            if(c != null)
            {
                CompanionNPCs.Add(c);
            }
            return c;
        }

        public static void RemoveCompanionNPC(Companion companion, bool Despawn = true)
        {
            RemoveCompanionNPC(companion.Data, Despawn);
        }

        public static void RemoveCompanionNPC(CompanionData data, bool Despawn = true)
        {
            RemoveCompanionNPC(data.ID, data.ModID, data.Index, Despawn);
        }

        public static void RemoveCompanionNPC(CompanionID ID, uint Index = 0, bool Despawn = true)
        {
            RemoveCompanionNPC(ID.ID, ID.ModID, Index, Despawn);
        }

        public static void RemoveCompanionNPC(uint ID, string ModID = "", uint Index = 0, bool Despawn = true)
        {
            for(int c = 0;c < CompanionNPCs.Count; c++)
            {
                if (CompanionNPCs[c].IsSameID(ID, ModID) && (Index == 0 || Index == CompanionNPCs[c].Index))
                {
                    if (Despawn) MainMod.DespawnCompanion(CompanionNPCs[c].GetWhoAmID);
                    CompanionNPCs.RemoveAt(c);
                    return;
                }
            }
        }

        public static void AllowCompanionNPCToSpawn(Companion companion)
        {
            AllowCompanionNPCToSpawn(companion.ID, companion.ModID);
        }

        public static void AllowCompanionNPCToSpawn(CompanionID ID)
        {
            AllowCompanionNPCToSpawn(ID.ID, ID.ModID);
        }

        public static void AllowCompanionNPCToSpawn(uint ID, string ModID = "")
        {
            int Emptyslot = -1;
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                if(Emptyslot == -1 && CompanionNPCsInWorld[i] == null)
                {
                    Emptyslot = i;
                }
                if(CompanionNPCsInWorld[i] != null && CompanionNPCsInWorld[i].CharID.IsSameID(ID, ModID))
                    return;
            }
            if (Emptyslot > -1)
            {
                CompanionNPCsInWorld[Emptyslot] = new CompanionTownNpcState(ID, ModID);
                foreach(Companion cs in MainMod.ActiveCompanions.Values)
                {
                    if(cs.IsSameID(ID, ModID))
                    {
                        cs.CheckIfHasNpcState();
                    }
                }
            }
        }

        public static void RemoveCompanionNPCToSpawn(Companion companion)
        {
            RemoveCompanionNPCToSpawn(companion.ID, companion.ModID);
        }

        public static void RemoveCompanionNPCToSpawn(CompanionID ID)
        {
            RemoveCompanionNPCToSpawn(ID.ID, ID.ModID);
        }

        public static void RemoveCompanionNPCToSpawn(uint ID, string ModID = "")
        {
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                if(CompanionNPCsInWorld[i] != null && CompanionNPCsInWorld[i].CharID.IsSameID(ID, ModID))
                {
                    CompanionNPCsInWorld[i].GetCompanion.ChangeTownNpcState(null);
                    CompanionNPCsInWorld[i] = null;
                    return;
                }
            }
        }
        
        internal static void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new WorldGeneration.SpawnStarterCompanion());
        }

        internal static void SaveWorldData(TagCompound tag)
        {
            tag.Add("ModVersion", MainMod.ModVersion);
            //Starter Companions List
            string Key = "StarterCompanion_";
            tag.Add(Key + "Count", StarterCompanions.Length);
            for(int i = 0; i < StarterCompanions.Length; i++)
            {
                tag.Add(Key + "ID_"+i, StarterCompanions[i].ID);
                tag.Add(Key + "ModID_"+i, StarterCompanions[i].ModID);
            }
            //Met Companions List
            Key = "CompanionsMet_";
            tag.Add(Key + "Count", CompanionsMet.Count);
            for(int i = 0; i < CompanionsMet.Count; i++)
            {
                tag.Add(Key + "ID_" + i, CompanionsMet[i].ID);
                tag.Add(Key + "ModID_" + i, CompanionsMet[i].ModID);
            }
            //Companions Spawn Infos
            Key = "CompanionsCanSpawn_";
            tag.Add(Key + "Count", MaxCompanionNpcsInWorld);
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                tag.Add(Key + "HasValue" + i, CompanionNPCsInWorld[i] != null);
                if(CompanionNPCsInWorld[i] != null)
                {
                    tag.Add(Key + "ID" + i, CompanionNPCsInWorld[i].CharID.ID);
                    tag.Add(Key + "ModID" + i, CompanionNPCsInWorld[i].CharID.ModID);
                    tag.Add(Key + "Homeless" + i, CompanionNPCsInWorld[i].Homeless);
                    tag.Add(Key + "HomeX" + i, CompanionNPCsInWorld[i].HomeX);
                    tag.Add(Key + "HomeY" + i, CompanionNPCsInWorld[i].HomeY);
                }
            }
            //Companion Town Npcs
            Key = "CompanionTownNpcs_";
            tag.Add(Key + "Count", CompanionNPCs.Count);
            for (int i = 0; i < CompanionNPCs.Count; i++)
            {
                tag.Add(Key + "ID_" + i, CompanionNPCs[i].ID);
                tag.Add(Key + "ModID_" + i, CompanionNPCs[i].ModID);
                tag.Add(Key + "LastFollowingSomeone_" + i, CompanionNPCs[i].Owner != null);
                if(CompanionNPCs[i].Owner == null)
                {
                    tag.Add(Key + "HP_" + i, CompanionNPCs[i].statLife == CompanionNPCs[i].statLifeMax2 ? 1f : (float)CompanionNPCs[i].statLife / CompanionNPCs[i].statLifeMax2);
                    Vector2 Position = CompanionNPCs[i].position;
                    tag.Add(Key + "PX_" + i, Position.X);
                    tag.Add(Key + "PY_" + i, Position.Y);
                }
            }
        }

        internal static void LoadWorldData(TagCompound tag)
        {
            if(!tag.ContainsKey("ModVersion")) return;
            uint Version = tag.Get<uint>("ModVersion");
            //Starter Companions List
            string Key = "StarterCompanion_";
            int Count = tag.GetInt(Key + "Count");
            StarterCompanions = new CompanionID[Count];
            for(int i = 0; i < Count; i++)
            {
                CompanionID id = new CompanionID();
                id.ID = tag.Get<uint>(Key + "ID_"+i);
                id.ModID = tag.GetString(Key + "ModID_"+i);
                StarterCompanions[i] = id;
            }
            //Met Companions List
            Key = "CompanionsMet_";
            Count = tag.GetInt(Key + "Count");
            for(int i = 0; i < Count; i++)
            {
                CompanionID id = new CompanionID();
                id.ID = tag.Get<uint>(Key + "ID_" + i);
                id.ModID = tag.GetString(Key + "ModID_" + i);
                CompanionsMet.Add(id);
            }
            //Companions Spawn Infos
            Key = "CompanionsCanSpawn_";
            Count = tag.GetInt(Key + "Count");
            for(int i = 0; i < Count; i++)
            {
                if(tag.GetBool(Key + "HasValue" + i) && i < MaxCompanionNpcsInWorld)
                {
                    CompanionNPCsInWorld[i] = new CompanionTownNpcState();
                    CompanionNPCsInWorld[i].CharID.ID = tag.Get<uint>(Key + "ID" + i);
                    CompanionNPCsInWorld[i].CharID.ModID = tag.GetString(Key + "ModID" + i);
                    CompanionNPCsInWorld[i].Homeless = tag.GetBool(Key + "Homeless" + i);
                    CompanionNPCsInWorld[i].HomeX = tag.GetInt(Key + "HomeX" + i);
                    CompanionNPCsInWorld[i].HomeY = tag.GetInt(Key + "HomeY" + i);
                }
            }
            //Companion Town Npcs
            Key = "CompanionTownNpcs_";
            Count = tag.GetInt(Key + "Count");
            for (int i = 0; i < Count; i++)
            {
                uint ID = tag.Get<uint>(Key + "ID_" + i);
                string ModID = tag.GetString(Key + "ModID_" + i);
                if(!tag.GetBool(Key + "LastFollowingSomeone_" + i))
                {
                    float HpPercentage = tag.GetFloat(Key + "HP_" + i);
                    Vector2 Position = new Vector2(
                        tag.GetFloat(Key + "PX_" + i),
                        tag.GetFloat(Key + "PY_" + i)
                    );
                    Companion c =SpawnCompanionNPC(Position, ID,ModID);
                    c.statLife = (int)(c.statLifeMax2 * HpPercentage);
                }
                else
                {
                    SpawnCompanionNPC(ID, ModID);
                }
            }
        }
    }
}