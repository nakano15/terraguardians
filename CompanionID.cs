using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public struct CompanionID
    {
        public uint ID;
        public string ModID;

        public CompanionID(uint ID, string ModID = "")
        {
            this.ID = ID;
            if(ModID == "")
                this.ModID = terraguardians.MainMod.GetModName;
            else
                this.ModID = ModID;
        }

        public bool IsSameID(uint ID, string ModID = "")
        {
            if(ModID == "") ModID = MainMod.GetModName;
            return this.ID == ID && this.ModID == ModID;
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if(obj is CompanionID)
            {
                CompanionID cid = (CompanionID)obj;
                return IsSameID(cid.ID, cid.ModID);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int newHash = (int)ID;
            foreach(char c in ModID) newHash += c;
            return newHash;
        }
    }
}