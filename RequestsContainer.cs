using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians
{
    public class RequestContainer
    {
        private static Dictionary<string, ModRequests> ModRequestsContainer = new Dictionary<string, ModRequests>();
        private static RequestBase defaultRequest = new RequestBase();

        internal static void InitializeRequests()
        {
            //Hunt
            //Normal
            AddRequest(11000, new HuntRequest(NPCID.BlueSlime, "Slime"));
            AddRequest(11001, new HuntRequest(NPCID.MotherSlime, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 350));
            AddRequest(11002, new HuntRequest(NPCID.Skeleton));
            AddRequest(11003, new HuntRequest(NPCID.CaveBat));
            AddRequest(11004, new HuntRequest(NPCID.GiantBat) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11005, new HuntRequest(NPCID.PossessedArmor) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11006, new HuntRequest(NPCID.Wraith) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11007, new HuntRequest(NPCID.WanderingEye) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11008, new HuntRequest(NPCID.Werewolf) { CanTakeRequest = HandyMethods.IsFullMoonHardmode });
            AddRequest(11009, new HuntRequest(NPCID.ArmoredSkeleton) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11010, new HuntRequest(NPCID.RockGolem, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 500) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11011, new HuntRequest(NPCID.Tim, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 150) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            AddRequest(11012, new HuntRequest(NPCID.RuneWizard, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 650) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11013, new HuntRequest(NPCID.Nymph, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 1000) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            //Corruption
            AddRequest(11100, new HuntRequest(NPCID.EaterofSouls, InitialCount: 7){ CanTakeRequest = HandyMethods.IsEoCDownCorruption });
            AddRequest(11101, new HuntRequest(NPCID.DevourerHead, InitialCount: 3, FriendshipLevelExtraCount: 0.2f){ CanTakeRequest = HandyMethods.IsEoCDownCorruption });
            AddRequest(11102, new HuntRequest(NPCID.Corruptor, InitialCount: 7){ CanTakeRequest = HandyMethods.WorldIsCorruptAndHardmode });
            AddRequest(11103, new HuntRequest(NPCID.Clinger, InitialCount: 7){ CanTakeRequest = HandyMethods.WorldIsCorruptAndHardmode });
            AddRequest(11104, new HuntRequest(NPCID.SeekerHead, InitialCount: 5){ CanTakeRequest = HandyMethods.WorldIsCorruptAndHardmode });
            AddRequest(11105, new HuntRequest(NPCID.CursedHammer, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 350){ CanTakeRequest = HandyMethods.WorldIsCorruptAndHardmode });
            //Crimson            
            AddRequest(11200, new HuntRequest(NPCID.Crimera, InitialCount: 7){ CanTakeRequest = HandyMethods.IsEoCDownCrimson });
            AddRequest(11201, new HuntRequest(NPCID.FaceMonster, InitialCount: 7){ CanTakeRequest = HandyMethods.IsEoCDownCrimson });
            AddRequest(11202, new HuntRequest(NPCID.Herpling, InitialCount: 7){ CanTakeRequest = HandyMethods.WorldIsCrimsonAndHardmode });
            AddRequest(11203, new HuntRequest(NPCID.FloatyGross, InitialCount: 7){ CanTakeRequest = HandyMethods.WorldIsCrimsonAndHardmode });
            AddRequest(11204, new HuntRequest(NPCID.IchorSticker, InitialCount: 7){ CanTakeRequest = HandyMethods.WorldIsCrimsonAndHardmode });
            AddRequest(11205, new HuntRequest(NPCID.CrimsonAxe, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 350){ CanTakeRequest = HandyMethods.WorldIsCrimsonAndHardmode });
            //Hallow
            AddRequest(11300, new HuntRequest(NPCID.Pixie, InitialCount: 7){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11301, new HuntRequest(NPCID.Unicorn){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11302, new HuntRequest(NPCID.Gastropod, InitialCount: 7){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11303, new HuntRequest(NPCID.IlluminantBat, InitialCount: 7){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11304, new HuntRequest(NPCID.IlluminantSlime, InitialCount: 7){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11305, new HuntRequest(NPCID.ChaosElemental, InitialCount: 3, RewardValue: 200){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11306, new HuntRequest(NPCID.EnchantedSword, InitialCount: 1, FriendshipLevelExtraCount: 0, RewardValue: 350){ CanTakeRequest = HandyMethods.IsHardmode });
            //Snow Biome
            AddRequest(11400, new HuntRequest(NPCID.IceSlime));
            AddRequest(11401, new HuntRequest(NPCID.ZombieEskimo));
            AddRequest(11402, new HuntRequest(NPCID.IceBat, InitialCount: 7));
            AddRequest(11403, new HuntRequest(NPCID.SnowFlinx, InitialCount: 3));
            AddRequest(11404, new HuntRequest(NPCID.SpikedIceSlime));
            AddRequest(11405, new HuntRequest(NPCID.UndeadViking, InitialCount: 3));
            AddRequest(11406, new HuntRequest(NPCID.IceElemental){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11407, new HuntRequest(NPCID.Wolf){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11408, new HuntRequest(NPCID.ArmoredViking, InitialCount: 3){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11409, new HuntRequest(NPCID.IceTortoise, InitialCount: 3){ CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11410, new HuntRequest(NPCID.IcyMerman){ CanTakeRequest = HandyMethods.IsHardmode });
            //Desert
            AddRequest(11500, new HuntRequest(NPCID.Vulture) { CanTakeRequest = HandyMethods.IsEoCDown });
            AddRequest(11501, new HuntRequest(NPCID.Antlion, InitialCount: 3) { CanTakeRequest = HandyMethods.IsEoCDown });
            AddRequest(11502, new HuntRequest(NPCID.WalkingAntlion) { CanTakeRequest = HandyMethods.IsEoCDown });
            AddRequest(11503, new HuntRequest(NPCID.FlyingAntlion) { CanTakeRequest = HandyMethods.IsEoCDown });
            AddRequest(11504, new HuntRequest(NPCID.TombCrawlerHead) { CanTakeRequest = HandyMethods.IsEoCDown });
            AddRequest(11505, new HuntRequest(NPCID.DesertBeast) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(11506, new HuntRequest(NPCID.DuneSplicerHead) { CanTakeRequest = HandyMethods.IsHardmode });
            //Ocean
            AddRequest(11600, new HuntRequest(NPCID.Shark, InitialCount: 1) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            AddRequest(11601, new HuntRequest(NPCID.Crab) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            //Jungle
            AddRequest(11700, new HuntRequest(NPCID.JungleBat, InitialCount: 7) { CanTakeRequest = HandyMethods.IsEvilBossDown });
            AddRequest(11701, new HuntRequest(NPCID.ManEater) { CanTakeRequest = HandyMethods.IsEvilBossDown });
            AddRequest(11702, new HuntRequest(NPCID.Derpling) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            AddRequest(11703, new HuntRequest(NPCID.GiantTortoise, InitialCount: 3) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            AddRequest(11704, new HuntRequest(NPCID.GiantFlyingFox) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            AddRequest(11705, new HuntRequest(NPCID.AngryTrapper) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            AddRequest(11706, new HuntRequest(NPCID.Arapaima) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            AddRequest(11707, new HuntRequest(NPCID.MossHornet) { CanTakeRequest = HandyMethods.IsAllMechBossesDead });
            //Dungeon
            AddRequest(11800, new HuntRequest(NPCID.DarkCaster, InitialCount: 5) { CanTakeRequest = HandyMethods.IsSkeletronDown });
            AddRequest(11801, new HuntRequest(NPCID.CursedSkull) { CanTakeRequest = HandyMethods.IsSkeletronDown, AliasIDs = new int[]{ NPCID.GiantCursedSkull } });
            //Underworld
            AddRequest(11900, new HuntRequest(NPCID.Demon, InitialCount: 7) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown, AliasIDs = new int[] { NPCID.VoodooDemon, NPCID.RedDevil } });
            AddRequest(11901, new HuntRequest(NPCID.Hellbat, InitialCount: 7) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown, AliasIDs = new int[]{NPCID.Lavabat} });
            AddRequest(11902, new HuntRequest(NPCID.FireImp) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            AddRequest(11903, new HuntRequest(NPCID.RedDevil) { CanTakeRequest = HandyMethods.IsAnyMechBossDown });
            AddRequest(11904, new HuntRequest(NPCID.Lavabat) { CanTakeRequest = HandyMethods.IsAnyMechBossDown });
            AddRequest(11905, new HuntRequest(NPCID.LavaSlime) { CanTakeRequest = HandyMethods.IsRemixWorld });
            
            //Invasion Requests
            AddRequest(20000, new InvasionRequest(NPCID.Demon, 7) { CanTakeRequest = HandyMethods.IsEvilBossDown });
            AddRequest(20001, new InvasionRequest(NPCID.Werewolf, 3, MaxSpawnCount: 1) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
            AddRequest(20002, new InvasionRequest(NPCID.EaterofSouls, 10) { CanTakeRequest = HandyMethods.IsEoCDownCorruption });
            AddRequest(20003, new InvasionRequest(NPCID.Crimera, 10) { CanTakeRequest = HandyMethods.IsEoCDownCrimson });
            AddRequest(20004, new InvasionRequest(NPCID.CorruptBunny, 10) { CanTakeRequest = HandyMethods.WorldIsCorruptAndBossKilled });
            AddRequest(20005, new InvasionRequest(NPCID.CrimsonGoldfish, 10) { CanTakeRequest = HandyMethods.WorldIsCrimsonAndBossKilled });
            AddRequest(20006, new InvasionRequest(NPCID.DeadlySphere, 1) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(20007, new InvasionRequest(NPCID.CreatureFromTheDeep, 5, MaxSpawnCount: 1) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(20008, new InvasionRequest(NPCID.Psycho, 1) { CanTakeRequest = HandyMethods.IsAnyMechBossDown });
            AddRequest(20009, new InvasionRequest(NPCID.EnchantedSword, 5, MaxSpawnCount: 1) { CanTakeRequest = HandyMethods.IsHardmode });
            AddRequest(20010, new InvasionRequest(NPCID.CursedHammer, 5, MaxSpawnCount: 1) { CanTakeRequest = HandyMethods.WorldIsCorruptAndHardmode });
            AddRequest(20011, new InvasionRequest(NPCID.CrimsonAxe, 5, MaxSpawnCount: 1) { CanTakeRequest = HandyMethods.WorldIsCrimsonAndHardmode });
            AddRequest(20012, new InvasionRequest(NPCID.Ghost, 20, MaxSpawnCount: 5));
            AddRequest(20013, new InvasionRequest(NPCID.HoppinJack, 7, MaxSpawnCount: 3) { CanTakeRequest = HandyMethods.IsHalloween });
            AddRequest(20014, new InvasionRequest(NPCID.Tim, 1));
            AddRequest(20015, new InvasionRequest(NPCID.WaterSphere, 20) { CanTakeRequest = HandyMethods.IsAnyFirstThreeBossesDown });
        }

        public static bool AddRequest(int ID, Mod Mod, RequestBase Request)
        {
            if(!ModRequestsContainer.ContainsKey(Mod.Name))
                ModRequestsContainer.Add(Mod.Name, new ModRequests());
            ModRequests reqs = ModRequestsContainer[Mod.Name];
            if(!reqs.RequestList.ContainsKey(ID))
            {
                reqs.RequestList.Add(ID, Request);
                return true;
            }
            return false;
        }

        internal static bool AddRequest(int ID, RequestBase Request)
        {
            return AddRequest(ID, MainMod.GetMod, Request);
        }

        public static RequestBase GetRequest(int ID, string ModID, out bool Success)
        {
            Success = false;
            if(ModRequestsContainer.ContainsKey(ModID) && ModRequestsContainer[ModID].RequestList.ContainsKey(ID))
            {
                Success = true;
                return ModRequestsContainer[ModID].RequestList[ID];
            }
            return defaultRequest;
        }

        public static bool GetAnyPossibleRequest(Player player, CompanionData companion, out int ID, out string ModID)
        {
            ID = 0;
            ModID = "";
            KeyValuePair<int, string>[] PossibleRequests = GetPossibleRequests(player, companion);
            if(PossibleRequests.Length > 0)
            {
                int Picked = Main.rand.Next(0, PossibleRequests.Length);
                ID = PossibleRequests[Picked].Key;
                ModID = PossibleRequests[Picked].Value;
                PossibleRequests = null;
                return true;
            }
            return false;
        }

        public static KeyValuePair<int, string>[] GetPossibleRequests(Player player, CompanionData companion)
        {
            List<KeyValuePair<int, string>> PossibleRequests = new List<KeyValuePair<int, string>>();
            foreach(string mid in ModRequestsContainer.Keys)
            {
                ModRequests container = ModRequestsContainer[mid];
                foreach(int id in container.RequestList.Keys)
                {
                    if(ModRequestsContainer[mid].RequestList[id].AllowTakingRequest && ModRequestsContainer[mid].RequestList[id].CanTakeRequest(player, companion))
                    {
                        PossibleRequests.Add(new KeyValuePair<int, string>(id, mid));
                    }
                }
            }
            KeyValuePair<int, string>[] Result = PossibleRequests.ToArray();
            PossibleRequests.Clear();
            return Result;
        }

        public static void Unload()
        {
            foreach(string r in ModRequestsContainer.Keys)
            {
                ModRequestsContainer[r].Unload();
                ModRequestsContainer[r] = null;
            }
            defaultRequest = null;
        }

        public class ModRequests
        {
            public Dictionary<int, RequestBase> RequestList = new Dictionary<int, RequestBase>();

            public void Unload()
            {
                foreach(int i in RequestList.Keys)
                    RequestList[i] = null;
                RequestList.Clear();
                RequestList = null;
            }
        }
    }
}