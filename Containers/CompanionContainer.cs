using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionContainer
    {
        internal static CompanionBase InvalidCompanionBase = new CompanionBase().SetInvalid();
        private Dictionary<uint, CompanionBase> Companions = new Dictionary<uint, CompanionBase>();
        private Mod ReferedMod;

        internal static void UnloadStatic()
        {
            InvalidCompanionBase = null;
        }

        internal void SetReferedMod(Mod mod)
        {
            ReferedMod = mod;
        }

        public virtual CompanionBase GetCompanionDB(uint ID)
        {
            return InvalidCompanionBase;
        }

        public CompanionBase ReturnCompanionBase(uint ID)
        {
            if(!Companions.ContainsKey(ID))
            {
                CompanionBase Base = GetCompanionDB(ID);
                Base.DefineMod(ReferedMod);
                Base.OnLoad(ID, ReferedMod.Name);
                Companions.Add(ID, Base);
            }
            return Companions[ID];
        }

        public void Unload()
        {
            foreach(uint k in Companions.Keys)
            {
                Companions[k].Unload();
            }
            Companions.Clear();
            Companions = null;
            ReferedMod = null;
        }
    }
}