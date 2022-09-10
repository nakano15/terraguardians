using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionData
    {
        public CompanionBase Base 
        {
            get
            { 
                if(_Base == null) 
                    _Base = MainMod.GetCompanionBase(ID, ModID); 
                return _Base; 
            } 
        }
        private CompanionBase _Base = null;

        public uint ID = 0;
        public string ModID = "";

        public void ChangeCompanion(uint NewID, string NewModID = "")
        {
            if(NewModID == "") NewModID = MainMod.GetModName;
            ID = NewID;
            ModID = NewModID;
            _Base = null;
        }
    }
}