using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionContainer
    {
        private Dictionary<uint, CompanionBase> Companions = new Dictionary<uint, CompanionBase>();

        public virtual CompanionBase GetCompanionDB(uint ID)
        {
            return new CompanionBase();
        }

        public CompanionBase ReturnCompanionBase(uint ID)
        {
            if(!Companions.ContainsKey(ID))
                Companions.Add(ID, GetCompanionDB(ID));
            return Companions[ID];
        }

        public void Unload()
        {
            Companions.Clear();
        }
    }
}