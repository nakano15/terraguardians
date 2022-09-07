using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians.Containers
{
    public class CompanionDB : CompanionContainer
    {
        public const uint Rococo = 0;

        public override CompanionBase GetCompanionDB(uint ID)
        {
            switch(ID)
            {
                case Rococo: return new Companions.RococoBase();
            }
            return base.GetCompanionDB(ID);
        }
    }
}