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
                    if(ModRequestsContainer[mid].RequestList[id].CanTakeRequest(player, companion))
                    {
                        PossibleRequests.Add(new KeyValuePair<int, string>(id, mid));
                    }
                }
            }
            KeyValuePair<int, string>[] Result = PossibleRequests.ToArray();
            PossibleRequests.Clear();
            return Result;
        }

        public static void InitializeRequests()
        {
            AddRequest(0, new HuntRequest(NPCID.BlueSlime, "Slime"));
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