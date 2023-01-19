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