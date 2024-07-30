using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace terraguardians
{
    public class WorldMod
    {
        private static CompanionTypeCount TerraGuardiansCount = new CompanionTypeCount(), 
            TerrariansCount = new CompanionTypeCount();
        private static Dictionary<string, CompanionTypeCount> CompanionCount = new Dictionary<string, CompanionTypeCount>();
        private static List<CompanionID> CompanionsMet = new List<CompanionID>();
        private static List<CompanionID> ScheduledToVisit = new List<CompanionID>();
        public static int MaxCompanionNpcsInWorld { get { return _MaxCompanionNpcsInWorld; } internal set { _MaxCompanionNpcsInWorld = value; } }
        static int _MaxCompanionNpcsInWorld = 30;
        public static List<Companion> CompanionNPCs = new List<Companion>();
        public static CompanionTownNpcState[] CompanionNPCsInWorld = new CompanionTownNpcState[30];
        public static int CompanionsMetCount { get{ return CompanionsMet.Count; } }
        public static CompanionID[] StarterCompanions = new CompanionID[0];
        public static List<BuildingInfo> HouseInfos = new List<BuildingInfo>();
        private static byte SpawnDelay = 0, LeaveCooldown = 0;
        public static int GetCompanionsCount { get { return GetTerraGuardiansCount + GetTerrariansCount; } }
        public static int GetTerrariansCount { get { return TerrariansCount.GetCount; } }
        public static int GetTerraGuardiansCount { get { return TerraGuardiansCount.GetCount; } }
        private static float DayTimeValue = 10f, LastDayTimeValue = 10f;
        public static float GetDayTime { get { return DayTimeValue; } }
        public static float GetLastDayTime { get { return LastDayTimeValue; } }

        public static bool IsCompanionScheduledToVisit(Companion c)
        {
            return IsCompanionScheduledToVisit(c.GetCompanionID);
        }

        public static bool IsCompanionScheduledToVisit(CompanionData data)
        {
            return IsCompanionScheduledToVisit(data.GetMyID);
        }

        public static bool IsCompanionScheduledToVisit(CompanionID ID)
        {
            return IsCompanionScheduledToVisit(ID.ID, ID.ModID);
        }

        public static bool IsCompanionScheduledToVisit(uint ID, string ModID = "")
        {
            foreach(CompanionID id in ScheduledToVisit)
            {
                if (id.IsSameID(ID, ModID)) return true;
            }
            return false;
        }

        public static void AddCompanionToScheduleList(Companion c)
        {
            AddCompanionToScheduleList(c.GetCompanionID);
        }

        public static void AddCompanionToScheduleList(CompanionData data)
        {
            AddCompanionToScheduleList(data.GetMyID);
        }

        public static void AddCompanionToScheduleList(CompanionID ID)
        {
            AddCompanionToScheduleList(ID.ID, ID.ModID);
        }

        public static void AddCompanionToScheduleList(uint ID, string ModID = "")
        {
            if (!IsCompanionScheduledToVisit(ID, ModID))
            {
                ScheduledToVisit.Add(new CompanionID(ID, ModID));
            }
        }

        public static void RemoveCompanionFromScheduleList(Companion c)
        {
            RemoveCompanionFromScheduleList(c.GetCompanionID);
        }

        public static void RemoveCompanionFromScheduleList(CompanionData data)
        {
            RemoveCompanionFromScheduleList(data.GetMyID);
        }

        public static void RemoveCompanionFromScheduleList(CompanionID ID)
        {
            RemoveCompanionFromScheduleList(ID.ID, ID.ModID);
        }

        public static void RemoveCompanionFromScheduleList(uint ID, string ModID = "")
        {
            for(int i = 0; i < ScheduledToVisit.Count; i++)
            {
                if (ScheduledToVisit[i].IsSameID(ID, ModID))
                {
                    ScheduledToVisit.RemoveAt(i);
                }
            }
        }

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
            CompanionCount.Clear();
            CompanionCount = null; 
            TerrariansCount = null;
            TerraGuardiansCount = null;
        }

        internal static void UpdateCompanionCount(Companion companion)
        {
            if(companion.GetTownNpcState == null) return;
            switch(companion.Base.CompanionType)
            {
                case CompanionTypes.TerraGuardian:
                    TerraGuardiansCount.Increment();
                    break;
                case CompanionTypes.Terrarian:
                    TerrariansCount.Increment();
                    break;
            }
            string GroupID = companion.GetGroup.ID;
            if (!CompanionCount.ContainsKey(GroupID))
                CompanionCount.Add(GroupID, new CompanionTypeCount());
            CompanionCount[GroupID].Increment();
        }

        internal static void RefreshCompanionInWorldCount()
        {
            TerraGuardiansCount.Refresh();
            TerrariansCount.Refresh();
            foreach(CompanionTypeCount t in CompanionCount.Values) t.Refresh();
        }

        internal static void OnInitializeWorldGen()
        {
            CompanionsMet.Clear();
            CompanionNPCs.Clear();
            CompanionNPCsInWorld = new CompanionTownNpcState[MaxCompanionNpcsInWorld];
            StarterCompanions = new CompanionID[0];
            ScheduledToVisit.Clear();
            HouseInfos.Clear();
            NpcMod.OnReloadWorld();
            Companions.CelesteBase.ResetCelestePrayerInfos();
            Companions.LiebreBase.Initialize();
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

        public static bool HasMetCompanion(Companion c)
        {
            return HasMetCompanion(c.Data);
        }

        public static bool HasMetCompanion(CompanionData data)
        {
            return HasMetCompanion(data.ID, data.ModID);
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

        public static void AddCompanionMet(Companion companion)
        {
            AddCompanionMet(companion.Data);
        }

        public static void AddCompanionMet(CompanionData data)
        {
            AddCompanionMet(data.ID, data.ModID);
        }

        public static void AddCompanionMet(CompanionID ID)
        {
            if(!HasMetCompanion(ID) && !MainMod.GetCompanionBase(ID).IsGeneric)
            {
                CompanionsMet.Add(ID);
            }
        }

        public static void RemoveCompanionMet(uint ID, string ModID = "")
        {
            RemoveCompanionMet(new CompanionID(ID, ModID));
        }

        public static void RemoveCompanionMet(Companion c)
        {
            RemoveCompanionMet(c.Data);
        }

        public static void RemoveCompanionMet(CompanionData data)
        {
            RemoveCompanionMet(data.ID, data.ModID);
        }

        public static void RemoveCompanionMet(CompanionID ID)
        {
            if(HasMetCompanion(ID))
            {
                CompanionsMet.Remove(ID);
            }
        }

        public static bool HasCompanionNPCSpawned(CompanionID ID)
        {
            return HasCompanionNPCSpawned(ID.ID, ID.ModID);
        }

        public static int GetCompanionNpcPosition(uint ID, string ModID = "")
        {
            for(int i = 0; i < CompanionNPCs.Count; i++)
            {
                if (CompanionNPCs[i].IsSameID(ID, ModID)) return i;
            }
            return -1;
        }

        public static Companion GetCompanionNpc(CompanionID ID)
        {
            return GetCompanionNpc(ID.ID, ID.ModID);
        }

        public static Companion GetCompanionNpc(uint ID, string ModID = "")
        {
            for(int i = 0; i < CompanionNPCs.Count; i++)
            {
                if (CompanionNPCs[i].IsSameID(ID, ModID)) return CompanionNPCs[i];
            }
            return null;
        }

        public static string GetCompanionNpcName(uint ID, string ModID = "", bool Colorize = true)
        {
            Companion c = null;
            for(int i = 0; i < CompanionNPCs.Count; i++)
            {
                if (CompanionNPCs[i].IsSameID(ID, ModID))
                {
                    c = CompanionNPCs[i];
                    break;
                }
            }
            if (c != null)
            {
                if(Colorize) return c.GetNameColored();
                return c.GetName;
            }
            return "Unknown";
        }

        public static bool HasCompanionNPCSpawned(uint ID, string ModID = "")
        {
            foreach(Companion c in CompanionNPCs)
            {
                if(c.IsSameID(ID, ModID)) return true;
            }
            return false;
        }

        public static void SetCompanionTownNpc(Companion c)
        {
            foreach(Companion companion in CompanionNPCs)
                if(c == companion) return;
            CompanionNPCs.Add(c);
        }

        public static Companion SpawnStarterCompanionNPC(CompanionID ID)
        {
            return SpawnCompanionNPC(Vector2.Zero, true, ID.ID, 0, ID.ModID);
        }

        public static Companion SpawnCompanionNPC(CompanionID ID)
        {
            return SpawnCompanionNPC(ID, 0);
        }

        public static Companion SpawnCompanionNPC(CompanionID ID, ushort GenericID)
        {
            return SpawnCompanionNPC(ID.ID, GenericID, ID.ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, CompanionID ID)
        {
            return SpawnCompanionNPC(SpawnPosition, 0, ID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, ushort GenericID, CompanionID ID)
        {
            return SpawnCompanionNPC(SpawnPosition, ID.ID, GenericID, ID.ModID);
        }

        public static Companion SpawnCompanionNPC(uint ID, string ModID = "")
        {
            return SpawnCompanionNPC(ID, 0, ModID);
        }

        public static Companion SpawnCompanionNPC(uint ID, ushort GenericID, string ModID = "")
        {
            return SpawnCompanionNPC(Vector2.Zero, ID, GenericID, ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, uint ID, string ModID = "")
        {
            return SpawnCompanionNPC(SpawnPosition, ID, 0, ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, uint ID, ushort GenericID, string ModID = "")
        {
            return SpawnCompanionNPC(SpawnPosition, false, ID, GenericID, ModID);
        }

        public static Companion SpawnCompanionNPC(Vector2 SpawnPosition, bool Starter, uint ID, ushort GenericID, string ModID = "")
        {
            if (MainMod.DisableModCompanions && ModID == MainMod.GetModName) return null;
            Companion c = SpawnPosition.Length() > 0 ? MainMod.SpawnCompanion(SpawnPosition, ID, ModID, GenericID: GenericID, Starter: Starter) : MainMod.SpawnCompanion(ID, ModID, GenericID: GenericID, Starter: Starter);
            if(c != null)
            {
                CompanionNPCs.Add(c);
            }
            return c;
        }

        public static Companion SpawnCompanionNPCOnPlayer(Player player, CompanionID ID)
        {
            return SpawnCompanionNPCOnPlayer(player, ID.ID, ID.ModID);
        }

        public static Companion SpawnCompanionNPCOnPlayer(Player player, uint ID, string ModID = "")
        {
            int TileSpawnX = -1, TileSpawnY = -1;
            int TileCenterX = (int)(player.Center.X * Companion.DivisionBy16),
                TileCenterY = (int)(player.Center.Y * Companion.DivisionBy16);
            int SpawnDistanceX = NPC.safeRangeX, SpawnDistanceY = NPC.safeRangeY;
            int SpawnRangeX = (int)(NPC.sWidth * Companion.DivisionBy16 * .7f) - SpawnDistanceX,
                SpawnRangeY = (int)(NPC.sHeight * Companion.DivisionBy16 * .7f) - SpawnDistanceY;
            for (int i = 0; i < 1000; i++)
            {
                int TileX = TileCenterX, TileY = TileCenterY;
                if (Main.rand.Next(2) == 0)
                {
                    TileX += SpawnDistanceX + Main.rand.Next(SpawnRangeX);
                }
                else
                {
                    TileX -= SpawnDistanceX + Main.rand.Next(SpawnRangeX);
                }
                if (Main.rand.Next(2) == 0)
                {
                    TileY += SpawnDistanceY + Main.rand.Next(SpawnRangeY);
                }
                else
                {
                    TileY -= SpawnDistanceY + Main.rand.Next(SpawnRangeY);
                }
                TileX = Math.Clamp(TileX, (int)(Main.leftWorld * Companion.DivisionBy16), (int)(Main.rightWorld * Companion.DivisionBy16));
                TileY = Math.Clamp(TileY, (int)(Main.topWorld * Companion.DivisionBy16), (int)(Main.bottomWorld * Companion.DivisionBy16));
                Tile tile = Main.tile[TileX, TileY];
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                {
                    continue;
                }
                bool FoundGround = false;
                while (TileY < Main.maxTilesY)
                {
                    for (int x = -1; x < 1; x++)
                    {
                        Tile nextTile = Main.tile[TileX + x, TileY + 1];
                        if(nextTile != null && nextTile.HasTile && Main.tileSolid[nextTile.TileType])
                        {
                            FoundGround = true;
                            tile = Main.tile[TileX, TileY];
                            FoundGround = true;
                            break;
                        }
                    }
                    if (FoundGround) break;
                    TileY++;
                }
                if (!FoundGround) continue;
                if (PathFinder.CheckForSolidBlocks(TileX, TileY))
                {
                    continue;
                }
                TileSpawnX = TileX;
                TileSpawnY = TileY;
                break;
            }
            if (TileSpawnX > -1 && TileSpawnY > -1)
            {
                return SpawnCompanionNPC(new Vector2(TileSpawnX * 16, TileSpawnY * 16), ID, ModID);
            }
            return null;
        }

        public static bool RemoveCompanionNPC(Companion companion, bool Despawn = true)
        {
            return RemoveCompanionNPC(companion.Data, Despawn);
        }

        public static bool RemoveCompanionNPC(CompanionData data, bool Despawn = true)
        {
            return RemoveCompanionNPC(data.ID, data.ModID, data.Index, Despawn);
        }

        public static bool RemoveCompanionNPC(CompanionID ID, uint Index = 0, bool Despawn = true)
        {
            return RemoveCompanionNPC(ID.ID, ID.ModID, Index, Despawn);
        }

        public static bool RemoveCompanionNPC(uint ID, string ModID = "", uint Index = 0, bool Despawn = true)
        {
            for(int c = 0; c < CompanionNPCs.Count; c++)
            {
                if (CompanionNPCs[c].IsSameID(ID, ModID) && (Index == 0 || Index == CompanionNPCs[c].Index))
                {
                    if (Despawn)
                        MainMod.DespawnCompanion(CompanionNPCs[c].GetWhoAmID);
                    CompanionNPCs.RemoveAt(c);
                    return true;
                }
            }
            return false;
        }

        public static bool AllowCompanionNPCToSpawn(Companion companion)
        {
            return AllowCompanionNPCToSpawn(companion.ID, companion.ModID);
        }

        public static bool AllowCompanionNPCToSpawn(CompanionID ID)
        {
            return AllowCompanionNPCToSpawn(ID.ID, ID.ModID);
        }

        public static bool AllowCompanionNPCToSpawn(uint ID, string ModID = "")
        {
            int Emptyslot = -1;
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                if(Emptyslot == -1 && CompanionNPCsInWorld[i] == null)
                {
                    Emptyslot = i;
                }
                if(CompanionNPCsInWorld[i] != null && CompanionNPCsInWorld[i].CharID.IsSameID(ID, ModID))
                {
                    return false;
                }
            }
            if (Emptyslot > -1)
            {
                CompanionNPCsInWorld[Emptyslot] = new CompanionTownNpcState(ID, ModID);
                foreach(Companion cs in MainMod.ActiveCompanions.Values)
                {
                    if(cs.IsSameID(ID, ModID))
                    {
                        cs.CheckIfHasNpcState();
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        public static void RemoveCompanionNPCToSpawn(CompanionData companion)
        {
            RemoveCompanionNPCToSpawn(companion.ID, companion.ModID);
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
            if(ModID == "") ModID = MainMod.GetModName;
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                if(CompanionNPCsInWorld[i] != null && CompanionNPCsInWorld[i].CharID.IsSameID(ID, ModID))
                {
                    if(CompanionNPCsInWorld[i].HouseInfo != null)
                    {
                        CompanionNPCsInWorld[i].HouseInfo.CompanionsLivingHere.Remove(CompanionNPCsInWorld[i]);
                    }
                    CompanionNPCsInWorld[i].GetCompanion.ChangeTownNpcState(null);
                    CompanionNPCsInWorld[i] = null;
                    return;
                }
            }
        }

        public static bool IsCompanionLivingHere(CompanionData companion)
        {
            return IsCompanionLivingHere(companion.ID, companion.ModID);
        }

        public static bool IsCompanionLivingHere(Companion companion)
        {
            return IsCompanionLivingHere(companion.ID, companion.ModID);
        }

        public static bool IsCompanionLivingHere(CompanionID ID)
        {
            return IsCompanionLivingHere(ID.ID, ID.ModID);
        }

        public static bool IsCompanionLivingHere(uint ID, string ModID = "")
        {
            if(ModID == "") ModID = MainMod.GetModName;
            for(int i = 0; i < MaxCompanionNpcsInWorld; i++)
            {
                if(CompanionNPCsInWorld[i] != null && CompanionNPCsInWorld[i].CharID.IsSameID(ID, ModID))
                {
                    return true;
                }
            }
            return false;
        }

        public static void PreUpdate()
        {
            if (Main.netMode != 1)
            {
                SpawnDelay++;
                if (SpawnDelay >= 20)
                {
                    LeaveCooldown++;
                    SpawnDelay = 0;
                    CheckIfCompanionNPCCanSpawn();
                    if (LeaveCooldown >= 10)
                    {
                        LeaveCooldown = 0;
                        if (!CheckIfSomeoneMustLeaveWorld())
                        {
                            CheckScheduleList();
                            CheckIfSomeoneCanVisit();
                        }
                    }
                }
            }
            LastDayTimeValue = DayTimeValue;
            if (Main.dayTime)
            {
                DayTimeValue = 4.5f + (float)(Main.time * (1f / 3600));
            }
            else
            {
                DayTimeValue = 19.5f + (float)(Main.time * (1f / 3600));
                if (DayTimeValue >= 24)
                    DayTimeValue -= 24;
            }
        }

        public static bool IsInRange(Vector2 Position1, Vector2 Position2)
        {
            return (MathF.Abs(Position1.X - Position2.X) < NPC.safeRangeX + NPC.sWidth && 
                MathF.Abs(Position1.Y - Position2.Y) < NPC.safeRangeY + NPC.sHeight);
        }

        private static void CheckScheduleList()
        {
            for (int i = 0; i < ScheduledToVisit.Count; i++)
            {
                if (MainMod.HasCompanionInWorld(ScheduledToVisit[i]))
                    ScheduledToVisit.RemoveAt(i);
            }
        }

        public static void CheckIfSomeoneCanVisit()
        {
            if ((!Main.dayTime && (Main.time < 3 * 3600 || Main.time >= 5.5f * 3600)) || (Main.dayTime && (Main.time < 2 * 3600 || Main.time >= 4.5f * 3600)))
            {
                return;
            }
            if (Main.bloodMoon || Main.eclipse || Main.invasionType > InvasionID.None || Main.pumpkinMoon || Main.snowMoon)
                return;
            float VisitRate = 1f;
            foreach (Companion c in CompanionNPCs)
            {
                if (!c.IsTownNpc) VisitRate *= 0.95f;
            }
            if ((ScheduledToVisit.Count > 0 && Main.rand.NextDouble() < 0.5) || Main.rand.NextDouble() < VisitRate * 0.025f)
            {
                List<CompanionID> PossibleIDs = new List<CompanionID>();
                List<CompanionID> CompanionsToCheck = new List<CompanionID>();
                CompanionsToCheck.AddRange(ScheduledToVisit);
                int ScheduleVisitCount = ScheduledToVisit.Count;
                CompanionsToCheck.AddRange(CompanionsMet);
                foreach(CompanionID id in CompanionsToCheck)
                {
                    ScheduleVisitCount--;
                    if (!MainMod.HasCompanionInWorld(id) && !IsCompanionLivingHere(id) && (!MainMod.DisableModCompanions || id.ModID != MainMod.GetModName))
                    {
                        CompanionBase b = MainMod.GetCompanionBase(id);
                        if (b.IsNocturnal != Main.dayTime)
                        {
                            CompanionData cd = PlayerMod.PlayerGetCompanionData(MainMod.GetLocalPlayer, id.ID, id.ModID);
                            if (ScheduleVisitCount >= 0 || (cd != null && !cd.AllowVisiting))
                            {
                                continue;
                            }
                            PossibleIDs.Add(id);
                        }
                    }
                }
                if(PossibleIDs.Count == 0) return;
                int Picked = Main.rand.Next(PossibleIDs.Count);
                CompanionID PickedOne = PossibleIDs[Picked];
                PossibleIDs.Clear();
                List<Vector2> PossibleSpawnPositions = new List<Vector2>();
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].townNPC && !Main.npc[i].homeless && Main.npc[i].type != Terraria.ID.NPCID.OldMan)
                    {
                        Vector2 NpcPos = Main.npc[i].Bottom;
                        bool PlayerNearby = false;
                        for (int p = 0; p < 255; p++)
                        {
                            if (Main.player[p].active && PlayerMod.IsPlayerCharacter(Main.player[p]) && IsInRange(Main.player[p].Center, NpcPos))
                            {
                                PlayerNearby = true;
                                break;
                            }
                        }
                        if (!PlayerNearby)
                        {
                            PossibleSpawnPositions.Add(NpcPos);
                        }
                    }
                }
                foreach (Companion c in CompanionNPCs)
                {
                    if (c.IsTownNpc)
                    {
                        Vector2 CompanionPos = c.Bottom;
                        bool PlayerNearby = false;
                        for (int p = 0; p < 255; p++)
                        {
                            if (Main.player[p].active && PlayerMod.IsPlayerCharacter(Main.player[p]))
                            {
                                if (MathF.Abs(Main.player[p].Center.X - CompanionPos.X) < NPC.safeRangeX + NPC.sWidth && 
                                    MathF.Abs(Main.player[p].Center.Y - CompanionPos.Y) < NPC.safeRangeY + NPC.sHeight)
                                {
                                    PlayerNearby = true;
                                    break;
                                }
                            }
                        }
                        if (!PlayerNearby)
                        {
                            PossibleSpawnPositions.Add(CompanionPos);
                        }
                    }
                }
                if (PossibleSpawnPositions.Count == 0)
                {
                    PossibleSpawnPositions.Add(new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16));
                }
                if (PossibleSpawnPositions.Count > 0)
                {
                    int PickedPossibleIndex = PossibleIDs.Count;
                    Vector2 SpawnPosition = PossibleSpawnPositions[Main.rand.Next(PossibleSpawnPositions.Count)];
                    PossibleSpawnPositions.Clear();
                    Companion c = SpawnCompanionNPC(SpawnPosition, PickedOne);
                    if (c != null)
                    {
                        bool IsVisiting = false;
                        for (int i = 0; i < ScheduledToVisit.Count; i++)
                        {
                            if (ScheduledToVisit[i].IsSameID(c.GetCompanionID))
                            {
                                ScheduledToVisit.RemoveAt(i);
                                IsVisiting = true;
                                break;
                            }
                        }
                        if (IsVisiting)
                        {
                            c.SaySomethingOnChat(c.GetDialogues.InviteMessages(c, InviteContext.ArrivalMessage));
                        }
                        else
                        {
                            Main.NewText(c.GetNameColored() + " came to visit.", Color.Aquamarine);
                        }
                    }
                }
            }
        }

        public static bool CheckIfSomeoneMustLeaveWorld()
        {
            if (CompanionNPCs.Count == 0) return false;
            int Pos = Main.rand.Next(CompanionNPCs.Count);
            Companion companion = CompanionNPCs[Pos];
            if (!companion.GetGoverningBehavior().AllowDespawning) return false;
            if (!companion.Base.IsNocturnal == Main.dayTime) return false;
            if (companion.IsTownNpc && !companion.GetTownNpcState.Homeless) return false;
            if (IsStarterCompanion(companion)) return false;
            bool HasPlayerNearby = false;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active && MathF.Abs(Main.player[p].Center.X - companion.Center.X) < NPC.sWidth && 
                    MathF.Abs(Main.player[p].Center.Y - companion.Center.Y) < NPC.sHeight)
                {
                    HasPlayerNearby = true;
                    break;
                }
            }
            if (!HasPlayerNearby)
            {
                if (HasMetCompanion(companion.Data))
                {
                    if (companion.Owner != null)
                    {
                        Main.NewText(companion.GetNameColored() + " is now ready to leave the world.");
                    }
                    else
                    {
                        Main.NewText(companion.GetNameColored() + " has left the world.");
                    }
                }
                RemoveCompanionNPC(companion, companion.Owner == null);
                return true;
            }
            return false;
        }

        public static void CheckIfCompanionNPCCanSpawn()
        {
            CompanionID? ToSpawn = null;
            {
                List<CompanionID> PotentialCompanions = new List<CompanionID>();
                bool HasHomelessCompanion = false;
                for (int i = 0; i < MaxCompanionNpcsInWorld; i++)
                {
                    if (CompanionNPCsInWorld[i] != null)
                    {
                        CompanionTownNpcState tns = CompanionNPCsInWorld[i];
                        if (HasCompanionNPCSpawned(tns.CharID))
                        {
                            if(tns.Homeless)
                            {
                                if (!HasHomelessCompanion)
                                    PotentialCompanions.Clear();
                                HasHomelessCompanion = true;
                                PotentialCompanions.Add(tns.CharID);
                                //Main.NewText(tns.CharID.ToString() + " is homeless.");
                            }
                        }
                        else if (!HasHomelessCompanion)
                        {
                            if (MainMod.GetCompanionBase(tns.CharID).IsNocturnal != Main.dayTime)
                                PotentialCompanions.Add(tns.CharID);
                        }
                    }
                }
                if (PotentialCompanions.Count > 0)
                {
                    ToSpawn = PotentialCompanions[Main.rand.Next(PotentialCompanions.Count)];
                }
            }
            if (!ToSpawn.HasValue) return;
            float UpdateTime = (float)WorldGen.GetWorldUpdateRate();
            float MaxCheck = (Main.maxTilesX * Main.maxTilesY) * 1.5E-05f * UpdateTime;
            for (int n = 0; n < MaxCheck; n++)
            {
                int PickedX = Main.rand.Next(10, Main.maxTilesX - 10),
                    PickedY = Main.rand.Next(10, Main.maxTilesY - 20);
                if (Main.tile[PickedX, PickedY] == null || Main.tile[PickedX, PickedY].LiquidAmount > 32 || Main.tile[PickedX, PickedY].IsActuated)
                {
                    continue;
                }
                if (Main.tile[PickedX, PickedY].WallType == 34)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        TryMovingCompanionIn(PickedX, PickedY, ToSpawn.Value, AnnounceMoveIn: true, Silent: true);
                    }
                }
                else
                {
                    TryMovingCompanionIn(PickedX, PickedY, ToSpawn.Value, AnnounceMoveIn: true, Silent: true);
                }
            }
        }

        public static bool TryMovingCompanionIn(int TileX, int TileY, CompanionID ID, bool AnnounceMoveIn = true, bool Silent = false)
        {
            return TryMovingCompanionIn(TileX, TileY, ID.ID, ID.ModID, AnnounceMoveIn, Silent);
        }

        public static bool TryMovingCompanionIn(int TileX, int TileY, uint CompanionID, string CompanionModID = "", bool AnnounceMoveIn = true, bool Silent = false)
        { //TODO - I need to check if this is working.
            if (Main.tile[TileX, TileY] == null || !Main.wallHouse[Main.tile[TileX,TileY].WallType] || !WorldGen.StartRoomCheck(TileX, TileY))
            {
                if (!Silent)
                {
                    Main.NewText("This isn't a valid house.", Color.Red);
                }
                return false;
            }
            if (CompanionModID == "") CompanionModID = MainMod.GetModName;
            CompanionBase cb = MainMod.GetCompanionBase(CompanionID, CompanionModID);
            if (!Housing_IsRoomTallEnoughForCompanion(cb))
            {
                if (!Silent)
                {
                    Main.NewText("This room is not big enough for "+cb.GetNameColored()+".", Color.Red);
                }
                return false;
            }
            bool RoomOccupied, RoomEvil;
            int RoomScore;
            int NpcWhoLivesHereID;
            Housing_GetRoomScoreForCompanionHouse(cb, out RoomOccupied, out RoomEvil, out RoomScore, out NpcWhoLivesHereID);
            string FailMessage = "";
            if (RoomScore <= 0 || !cb.CompanionRoomRequirements(RoomEvil, out FailMessage))
            {
                if(!Silent)
                {
                    if (FailMessage.Length > 0)
                        Main.NewText(FailMessage, Color.Red);
                    else
                        Main.NewText("This room is corrupted.", Color.Red);
                }
                return false;
            }
            if (Housing_IsRoomCrowded(cb))
            {
                if (!Silent)
                {
                    Main.NewText("This room is too crowded.", Color.Red);
                }
                return false;
            }
            bool IsInTheWorld = IsCompanionLivingHere(CompanionID, CompanionModID);
            int SpawnX = WorldGen.bestX, SpawnY = WorldGen.bestY;
            Housing_TryGettingPlaceForCompanionToStay(ref SpawnX, ref SpawnY);
            int HomeXBackup = SpawnX, HomeYBackup = SpawnY;
            if (!IsInTheWorld)
            {
                //Continue here
                bool NoPlayerNearby = true;
                for (int i = 0; i < 255; i++)
                {
                    if (!Main.player[i].active) continue;
                    if(MathF.Abs(Main.player[i].Center.X - SpawnX * 16 + 8) < NPC.sWidth + NPC.safeRangeX &&
                        MathF.Abs(Main.player[i].Center.Y - SpawnY * 16 + 8) < NPC.sHeight + NPC.safeRangeY)
                    {
                        NoPlayerNearby = false;
                        break;
                    }
                }
                if (!NoPlayerNearby && SpawnY <= Main.worldSurface)
                {
                    for (int dist = 1; dist < 500; dist++)
                    {
                        for (int dir = -1; dir < 2; dir += 2)
                        {
                            int PosX = SpawnX + dist * dir;
                            if (PosX > 10 && PosX < Main.maxTilesX - 10)
                            {
                                int PosY = SpawnY - dist;
                                int SurfacePos = SpawnY + dist;
                                if (PosY < 10) PosY = 10;
                                if (SurfacePos > Main.worldSurface) SurfacePos = (int)Main.worldSurface;
                                while(PosY < SurfacePos)
                                {
                                    if (!(Main.tile[PosX, PosY].IsActuated && Main.tileSolid[Main.tile[PosX, PosY].TileType]))
                                    {
                                        PosY++;
                                        continue;
                                    }
                                    if (!Collision.SolidTiles(PosX - 1, PosX + 1, PosY - 3, PosY - 1))
                                    {
                                        NoPlayerNearby = true;
                                        for (int i = 0; i < 255; i++)
                                        {
                                            if (!Main.player[i].active) continue;
                                            if(MathF.Abs(Main.player[i].Center.X - PosX * 16 + 8) < NPC.sWidth + NPC.safeRangeX &&
                                                MathF.Abs(Main.player[i].Center.Y - PosY * 16 + 8) < NPC.sHeight + NPC.safeRangeY)
                                            {
                                                NoPlayerNearby = false;
                                                break;
                                            }
                                        }
                                        if (NoPlayerNearby)
                                        {
                                            SpawnX = PosX;
                                            SpawnY = PosY;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (NoPlayerNearby)
                                break;
                        }
                            if (NoPlayerNearby)
                                break;
                    }
                }
            }
            Companion companion = null;
            if (Main.netMode == 0)
            {
                foreach (uint key in MainMod.ActiveCompanions.Keys)
                {
                    if(MainMod.ActiveCompanions[key].IsSameID(CompanionID, CompanionModID))
                    {
                        companion = MainMod.ActiveCompanions[key];
                        break;
                    }
                }
            }
            bool Spawn = companion == null;
            if(companion == null)
            {
                companion = SpawnCompanionNPC(new Vector2(SpawnX * 16 + 8, SpawnY * 16), CompanionID, CompanionModID);
                if (companion == null)
                    return false;
            }
            CompanionTownNpcState tns = companion.GetTownNpcState;
            if (tns != null)
            {
                tns.HomeX = HomeXBackup;
                tns.HomeY = HomeYBackup;
                tns.Homeless = false;
                tns.ValidateHouse();
                if (!tns.Homeless)
                {
                    if (AnnounceMoveIn)
                    {
                        string Message = companion.GetNameColored() + (Spawn ? " arrives." : " settles in.");
                        Color color = Color.Cyan;
                        if (Main.netMode == 0)
                        {
                            Main.NewText(Message, color);
                        }
                        else
                        {
                            NetMessage.SendData(25, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Message), color.R, color.G, color.B, color.A);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public static void Housing_TryGettingPlaceForCompanionToStay(ref int SpawnX, ref int SpawnY)
        {
            List<Point> ImpossiblePoints = new List<Point>();
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless)
                {
                    ImpossiblePoints.Add(new Point(Main.npc[n].homeTileX, Main.npc[n].homeTileY));
                }
            }
            for (int g = 0; g < MaxCompanionNpcsInWorld; g++)
            {
                CompanionTownNpcState tns = CompanionNPCsInWorld[g];
                if (tns != null && !tns.Homeless)
                {
                    ImpossiblePoints.Add(new Point(tns.HomeX, tns.HomeY));
                }
            }
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                int x = WorldGen.roomX[i], y = WorldGen.roomY[i];
                if (y == Main.maxTilesY - 20) continue;
                if (!ImpossiblePoints.Any(z => z.X == x && z.Y == y) && Housing_IsInRoom(x, y) && !Housing_CheckIfIsCeiling(x, y) && Main.tile[x, y].TileType == Terraria.ID.TileID.Chairs && Main.tile[x, y - 1].TileType == Terraria.ID.TileID.Chairs)
                {
                    SpawnX = x;
                    SpawnY = y;
                    break;
                }
            }
        }

        public static bool Housing_IsRoomCrowded(CompanionBase cb)
        {
            int Capacity = Housing_GetMaxNumberOfHabitants();
            int CountHouseUsers = 0;
            for (int g = 0; g < MaxCompanionNpcsInWorld; g++)
            {
                CompanionTownNpcState tns = WorldMod.CompanionNPCsInWorld[g];
                if (tns != null && !tns.Homeless)
                {
                    if (Housing_IsInRoom(tns.HomeX, tns.HomeY))
                        CountHouseUsers++;
                }
                if (CountHouseUsers >= Capacity) return true;
            }
            return false;
        }

        public static int Housing_GetMaxNumberOfHabitants()
        {
            int Chairs = 0;
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                Tile tile = Main.tile[WorldGen.roomX[i], WorldGen.roomY[i]],
                    uppertile = Main.tile[WorldGen.roomX[i], WorldGen.roomY[i] - 1];
                if (tile.TileType == TileID.Chairs && uppertile.TileType == TileID.Chairs)
                    Chairs++;
            }
            if (Chairs > 0)
            {
                return Chairs;
            }
            return 1;
        }

        public static bool Housing_CheckBasicHousingRoomNeeds(bool IsRoomEvil, out string RequirementFailMessage)
        {
            RequirementFailMessage = "";
            bool HasChair = false, HasEntrance = false, HasTable = false, HasLightSource = false;
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsChair.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsChair[i]])
                {
                    HasChair = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsDoor.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsDoor[i]])
                {
                    HasEntrance = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsTable.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsTable[i]])
                {
                    HasTable = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsTorch.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsTorch[i]])
                {
                    HasLightSource = true;
                    break;
                }
            }
            if(IsRoomEvil)
            {
                RequirementFailMessage = "The room is corrupted.";
            }
            else
            {
                if (!HasChair)
                {
                    RequirementFailMessage += "a chair";
                }
                if (!HasTable)
                {
                    if(RequirementFailMessage.Length > 0)
                        RequirementFailMessage += ", ";
                    RequirementFailMessage += "a table";
                }
                if (!HasEntrance)
                {
                    if(RequirementFailMessage.Length > 0)
                        RequirementFailMessage += ", ";
                    RequirementFailMessage += "a entrance";
                }
                if (!HasLightSource)
                {
                    if(RequirementFailMessage.Length > 0)
                        RequirementFailMessage += ", ";
                    RequirementFailMessage += "a light source";
                }
                if (RequirementFailMessage.Length > 0)
                    RequirementFailMessage = "Room lacks " + RequirementFailMessage + ".";
            }
            return HasChair && HasEntrance && HasTable && HasLightSource && !IsRoomEvil;
        }

        public static void Housing_GetRoomScoreForCompanionHouse(CompanionBase cb, out bool Occupied, out bool Evil, out int RoomScore, out int NpcWhoLivesInTheRoom)
        {
            Occupied = Evil = false;
            RoomScore = 0;
            NpcWhoLivesInTheRoom = -1;
            for (int n = 0; n < 200; n++)
            {
                if (!(Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless))
                    continue;
                for (int j = 0; j < WorldGen.numRoomTiles; j++)
                {
                    if (Main.npc[n].homeTileX == WorldGen.roomX[j] && Main.npc[n].homeTileY == WorldGen.roomY[j])
                    {
                        NpcWhoLivesInTheRoom = n;
                        break;
                    }
                }
            }
            RoomScore = 50;
            int Left,
                Right,
                Up,
                Down;
            WorldGen.Housing_GetTestedRoomBounds(out Left, out Right, out Up, out Down);
            int[] TileCount = new int[TileLoader.TileCount];
            WorldGen.CountTileTypesInArea(TileCount, Left + 1, Right - 1, Up + 1, Down - 1);
            int EvilCounter = -WorldGen.GetTileTypeCountByCategory(TileCount, Terraria.Enums.TileScanGroup.TotalGoodEvil);
            if (EvilCounter < 50) EvilCounter = 0;
            RoomScore -= EvilCounter;
            if (EvilCounter > 0)
            {
                Evil = true;
            }
            for (int x = Left + 1; x < Right; x++)
            {
                for (int y = Up + 2; y < Down + 2; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (!tile.IsActuated)
                        continue;
                    int score = RoomScore;
                    if (!(Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !Collision.SolidTiles(x - 1, x + 1, y - 3, y - 1) && Main.tile[x - 1, y].IsActuated && Main.tileSolid[Main.tile[x - 1, y].TileType] && Main.tile[x + 1, y].IsActuated && Main.tileSolid[Main.tile[x + 1, y].TileType]))
                        continue;
                    for (int x2 = x - 2; x2 < x + 3; x2++)
                    {
                        for (int y2 = y - 4; y2 < y; y2++)
                        {
                            if (!Main.tile[x2, y2].IsActuated)
                                continue;
                            if (x2 == x)
                            {
                                if (score > 0)
                                {
                                    score -= 15;
                                    if (score < 0)
                                        score = 0;
                                }
                            }
                            else if (Main.tile[x2, y2].TileType == 21)
                            {
                                if (score > 0)
                                {
                                    score -= 30;
                                    if (score < 1)
                                        score = 1;
                                }
                            }
                            else if (Main.tile[x2, y2].TileType == 10 || Main.tile[x2, y2].TileType == 11)
                            {
                                score -= 20;
                            }
                            else if (Main.tileSolid[Main.tile[x2, y2].TileType])
                            {
                                score -= 5;
                            }
                            else
                            {
                                score += 5;
                            }
                        }
                    }
                    if (score > RoomScore)
                    {
                        bool ValidPosition = Housing_IsInRoom(x, y);
                        bool[] HeightChecker = new bool[3];
                        for (int i = 1; i <= 3; i++)
                        {
                            if (!Main.tile[x, y - i].HasTile || !Main.tileSolid[Main.tile[x, y - i].TileType])
                            {
                                HeightChecker[i - 1] = true;
                            }
                            if (!Housing_IsInRoom(x, y - i))
                            {
                                HeightChecker[i - 1] = false;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            if (!HeightChecker[i])
                            {
                                ValidPosition = false;
                                break;
                            }
                        }
                        if (ValidPosition && !Housing_CheckIfIsCeiling(x, y))
                        {
                            RoomScore = score;
                            WorldGen.bestX = x;
                            WorldGen.bestY = y;
                        }
                    }
                }
            }
        }

        public static bool Housing_CheckIfIsCeiling(int x, int y)
        {
            for (int i = 0; i < WorldGen.roomCeilingsCount; i++)
            {
                if (WorldGen.roomCeilingX[i] == x && WorldGen.roomCeilingY[i] == y) return true;
            }
            return false;
        }

        public static bool Housing_IsInRoom(int x, int y)
        {
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                if (WorldGen.roomX[i] == x && WorldGen.roomY[i] == y)
                    return true;
            }
            return false;
        }

        public static bool Housing_IsRoomTallEnoughForCompanion(CompanionBase cb)
        {
            return (int)(cb.Height * cb.Scale) / 16 <= WorldGen.roomY2 - WorldGen.roomY1;
        }
        
        internal static void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new WorldGeneration.SpawnStarterCompanion());
            tasks.Add(new AlexRecruitmentScript.WorldGenAlexTombstonePlacement());
        }
        
        public static Point GetClosestBed(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null, bool TakeFurnitureInUse = true)
        {
            Point Pos = Position.ToTileCoordinates();
            Point[] Beds = GetFurnituresCloseBy(Pos, DistanceX, DistanceY, false, true, HouseLimitation, TakeFurnitureInUse);
            Point NearestPos = Point.Zero;
            float NearestDistance = float.MaxValue;
            foreach(Point p in Beds)
            {
                float Distance = (MathF.Abs(Pos.X - p.X) + MathF.Abs(Pos.Y - p.Y)) * 0.5f;
                if(Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestPos = p;
                }
            }
            return NearestPos;
        }

        public static Point GetClosestChair(Vector2 Position, int DistanceX = 12, int DistanceY = 8, BuildingInfo HouseLimitation = null, bool TakeInUseFurniture = true)
        {
            return GetClosestChair(Position, TakeInUseFurniture, DistanceX, DistanceY, HouseLimitation);
        }

        public static Point GetClosestChair(Vector2 Position, bool TryTakingFurnitureInUse, int DistanceX = 12, int DistanceY = 8, BuildingInfo HouseLimitation = null)
        {
            Point Pos = Position.ToTileCoordinates();
            Point[] Chairs = GetFurnituresCloseBy(Pos, DistanceX, DistanceY, true, false, HouseLimitation, TryTakingFurnitureInUse);
            Point NearestPos = Point.Zero;
            float NearestDistance = float.MaxValue;
            foreach(Point p in Chairs)
            {
                float Distance = (MathF.Abs(Pos.X - p.X) + MathF.Abs(Pos.Y - p.Y)) * 0.5f;
                if(Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestPos = p;
                }
            }
            return NearestPos;
        }

        public static Point[] GetBedsCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null, bool TryTakingFurnitureInUse = false)
        {
            return GetFurnituresCloseBy(Position, DistanceX, DistanceY, false, true, HouseLimitation, TryTakingFurnitureInUse);
        }

        public static Point[] GetChairsCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null, bool TryTakingFurnitureInUse = false)
        {
            return GetFurnituresCloseBy(Position, DistanceX, DistanceY, true, false, HouseLimitation, TryTakingFurnitureInUse);
        }

        public static Point[] GetFurnituresCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, bool GetChairs = true, bool GetBeds = true, BuildingInfo HouseLimitation = null, bool TryTakingFurnitureInUse = false)
        {
            int TileX = (int)(Position.X * (1f / 16));
            int TileY = (int)(Position.Y * (1f / 16));
            return GetFurnituresCloseBy(new Point(TileX, TileY), DistanceX, DistanceY, GetChairs, GetBeds, HouseLimitation, TryTakingFurnitureInUse);
        }

        public static Point[] GetFurnituresCloseBy(Point Position, int DistanceX = 8, int DistanceY = 6, bool GetChairs = true, bool GetBeds = true, BuildingInfo HouseLimitation = null, bool TryTakingFurnitureInUse = false)
        {
            List<Point> FoundFurnitures = new List<Point>();
            if (HouseLimitation != null)
            {
                foreach(FurnitureInfo f in HouseLimitation.Furnitures)
                {
                    int x = f.FurnitureX, y = f.FurnitureY;
                    Point? furniture = CheckFurniture(x, y, GetChairs, GetBeds, HouseLimitation, TryTakingFurnitureInUse);
                    if (furniture.HasValue)
                        FoundFurnitures.Add(furniture.Value);
                }
                return FoundFurnitures.ToArray();
            }
            for (int y = Position.Y - DistanceY; y <= Position.Y + DistanceY; y++)
            {
                for (int x = Position.X - DistanceX; x <= Position.X + DistanceX; x++)
                {
                    Point? furniturepos = CheckFurniture(x, y, GetChairs, GetBeds, HouseLimitation, TryTakingFurnitureInUse);
                    if (furniturepos.HasValue)
                    {
                        FoundFurnitures.Add(furniturepos.Value);
                    }
                }
            }
            return FoundFurnitures.ToArray();
        }

        private static Point? CheckFurniture(int x, int y, bool GetChairs = true, bool GetBeds = true, BuildingInfo HouseLimitation = null, bool TryTakingFurnitureInUse = false)
        {
            if(!WorldGen.InWorld(x, y)) return null;
            if(HouseLimitation != null && !HouseLimitation.BelongsToThisHousing(x, y)) return null;
            Tile tile = Main.tile[x, y];
            if (tile != null && !tile.HasTile) return null;
            bool TakeFurniture = false;
            bool IsBed = false;
            int OwnerCount = TryTakingFurnitureInUse ? 1 : 0;
            switch(tile.TileType)
            {
                case TileID.Chairs:
                    if (GetChairs && tile.TileFrameY % 40 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) <= OwnerCount)
                    {
                        TakeFurniture = true;
                    }
                    break;
                case TileID.Thrones:
                    if (GetChairs && tile.TileFrameX % 54 == 18 && tile.TileFrameY % 72 == 54 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) <= OwnerCount)
                    {
                        TakeFurniture = true;
                    }
                    break;
                case TileID.Benches:
                    if (GetChairs && tile.TileFrameX % 54 == 18 && tile.TileFrameY % 36 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) <= OwnerCount)
                    {
                        TakeFurniture = true;
                    }
                    break;
                case TileID.PicnicTable:
                    {
                        if (GetChairs)
                        {
                            int FrameX = tile.TileFrameX % 72;
                            if((FrameX == 0 || FrameX == 54) && tile.TileFrameY % 36 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) <= OwnerCount)
                            {
                                TakeFurniture = true;
                            }
                        }
                    }
                    break;
                case TileID.Beds:
                    {
                        IsBed = true;
                        bool FacingLeft = tile.TileFrameX < 72;
                        if (GetBeds && tile.TileFrameX % 72 == (FacingLeft ? 18 : 36) && tile.TileFrameY % 36 == 18 && Main.sleepingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) <= OwnerCount)
                        {
                            TakeFurniture = true;
                        }
                    }
                    break;
            }
            if (TakeFurniture)
            {
                byte FurnitureUsers = 0;
                foreach(Companion c in MainMod.ActiveCompanions.Values)
                {
                    if (c.GetFurnitureX == x && c.GetFurnitureY == y)
                    {
                        FurnitureUsers++;
                    }
                }
                if (FurnitureUsers < 1 || (IsBed && FurnitureUsers < 2))
                    return new Point(x, y);
            }
            return null;
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
            //Scheduled to visit
            Key = "CompanionsScheduleVisit_";
            tag.Add(Key + "Count", ScheduledToVisit.Count);
            for(int i = 0; i < ScheduledToVisit.Count; i++)
            {
                tag.Add(Key + i + "_ID", ScheduledToVisit[i].ID);
                tag.Add(Key + i + "_ModID", ScheduledToVisit[i].ModID);
            }
            //Companion Town Npcs
            List<Companion> CompanionsToSave = new List<Companion>();
            foreach(Companion c in CompanionNPCs)
            {
                if (HasMetCompanion(c.Data) || !c.GetGoverningBehavior().AllowDespawning || IsStarterCompanion(c))
                {
                    CompanionsToSave.Add(c);
                }
            }
            Key = "CompanionTownNpcs_";
            tag.Add(Key + "Count", CompanionsToSave.Count);
            for (int i = 0; i < CompanionsToSave.Count; i++)
            {
                tag.Add(Key + "ID_" + i, CompanionsToSave[i].ID);
                tag.Add(Key + "ModID_" + i, CompanionsToSave[i].ModID);
                tag.Add(Key + "LastFollowingSomeone_" + i, CompanionsToSave[i].Owner != null);
                if(CompanionsToSave[i].Owner == null)
                {
                    tag.Add(Key + "HP_" + i, CompanionsToSave[i].statLife == CompanionsToSave[i].statLifeMax2 ? 1f : (float)CompanionsToSave[i].statLife / CompanionsToSave[i].statLifeMax2);
                    Vector2 Position = CompanionsToSave[i].Bottom;
                    tag.Add(Key + "PX_" + i, Position.X);
                    tag.Add(Key + "PY_" + i, Position.Y);
                }
                tag.Add(Key + "Generic_" + i, CompanionsToSave[i].IsGeneric);
                if (CompanionsToSave[i].IsGeneric)
                {
                    tag.Add(Key + "GenericName_" + i, CompanionsToSave[i].Data.GetName);
                    tag.Add(Key + "GenericID_" + i, CompanionsToSave[i].Data.GetGenericID);
                    tag.Add(Key + "GenericGender_" + i, (byte)CompanionsToSave[i].Data.Gender);
                    CompanionsToSave[i].Data.Save(tag, (uint)i);
                }
            }
            Companions.CelesteBase.SaveCelestePrayerStatus(tag);
            Companions.LiebreBase.SaveBuffInfos(tag);
            SardineBountyBoard.Save(tag);
            AlexRecruitmentScript.Save(tag);
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
                    string ModID = tag.GetString(Key + "ModID" + i);
                    if (MainMod.DisableModCompanions && ModID == MainMod.GetModName) continue;
                    CompanionNPCsInWorld[i] = new CompanionTownNpcState();
                    CompanionNPCsInWorld[i].CharID.ID = tag.Get<uint>(Key + "ID" + i);
                    CompanionNPCsInWorld[i].CharID.ModID = ModID;
                    CompanionNPCsInWorld[i].Homeless = tag.GetBool(Key + "Homeless" + i);
                    CompanionNPCsInWorld[i].HomeX = tag.GetInt(Key + "HomeX" + i);
                    CompanionNPCsInWorld[i].HomeY = tag.GetInt(Key + "HomeY" + i);
                    if (MainMod.DisableModCompanions && CompanionNPCsInWorld[i].CharID.ModID == MainMod.GetModName)
                        CompanionNPCsInWorld[i] = null;
                    else
                        CompanionNPCsInWorld[i].ValidateHouse();
                }
            }
            //Scheduled to visit
            if (Version >= 25)
            {
                Key = "CompanionsScheduleVisit_";
                Count = tag.GetInt(Key + "Count");
                for(int i = 0; i < Count; i++)
                {
                    uint ID = tag.Get<uint>(Key + i + "_ID");
                    string ModID = tag.GetString(Key + i + "_ModID");
                    if (MainMod.DisableModCompanions && ModID == MainMod.GetModName) continue;
                    ScheduledToVisit.Add(new CompanionID(ID, ModID));
                }
            }
            //Companion Town Npcs
            Key = "CompanionTownNpcs_";
            Count = tag.GetInt(Key + "Count");
            List<CompanionID> AlreadySpawnedIDs = new List<CompanionID>();
            for (int i = 0; i < Count; i++)
            {
                uint ID = tag.Get<uint>(Key + "ID_" + i);
                string ModID = tag.GetString(Key + "ModID_" + i);
                if (MainMod.DisableModCompanions && ModID == MainMod.GetModName) continue;
                bool Repeated = false;
                foreach(CompanionID id in AlreadySpawnedIDs)
                {
                    if(id.IsSameID(ID, ModID))
                    {
                        Repeated = true;
                        break;
                    }
                }
                if (Repeated) continue;
                Companion c = null;
                bool WasFollowing = tag.GetBool(Key + "LastFollowingSomeone_" + i);
                bool IsGeneric = MainMod.GetCompanionBase(ID, ModID).IsGeneric && Version >= 36 && tag.GetBool(Key+"Generic_" + i);
                ushort GenericID = 0;
                if (IsGeneric && Version >= 36)
                {
                    GenericID = tag.Get<ushort>(Key+"GenericID_" + i);
                }
                if(!WasFollowing)
                {
                    float HpPercentage = tag.GetFloat(Key + "HP_" + i);
                    Vector2 Position = new Vector2(
                        tag.GetFloat(Key + "PX_" + i),
                        tag.GetFloat(Key + "PY_" + i)
                    );
                    c = SpawnCompanionNPC(Position, ID, GenericID, ModID);
                    if (c != null)
                        c.statLife = (int)(c.statLifeMax2 * HpPercentage);
                }
                else
                {
                    c = SpawnCompanionNPC(ID, GenericID, ModID);
                }
                if (c != null)
                {
                    if (IsGeneric)
                    {
                        TerrarianCompanionInfo info = new TerrarianCompanionInfo();
                        info.Load(tag, (uint)i, Version);
                        c.Data.ChangeGenericCompanionInfo(info);
                        if (Version >= 37)
                        {
                            c.Data.Gender = (Genders)tag.GetByte(Key + "GenericGender_" + i);
                        }
                        c.UpdateLookBasedOnGenericInfos();
                        c.Data.ChangeName(tag.GetString(Key + "GenericName_" + i)); //Doesn't seems to work
                        c.name = c.Data.GetName;
                    }
                    if(!MainMod.GetCompanionBase(ID, ModID).IsGeneric)
                    {
                        AlreadySpawnedIDs.Add(new CompanionID(ID, ModID));
                    }
                }
            }
            AlreadySpawnedIDs.Clear();
            if (Version >= 13)
                Companions.CelesteBase.LoadCelestePrayerStatus(tag, Version);
            if (Version >= 43)
                Companions.LiebreBase.LoadBuffInfos(tag, Version);
            SardineBountyBoard.Load(tag, Version);
            if (Version >= 27)
            {
                AlexRecruitmentScript.Load(tag, Version);
            }
        }

        private class CompanionTypeCount
        {
            private int LastCount = 0, CurrentCount = 0;

            public void Refresh()
            {
                LastCount = CurrentCount;
                CurrentCount = 0;
            }

            public int GetCount => LastCount;

            public void Increment()
            {
                CurrentCount++;
            }
        }
    }
}