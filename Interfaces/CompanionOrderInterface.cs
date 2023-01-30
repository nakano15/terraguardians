using System.Collections.Generic;
using Terraria;

namespace terraguardians
{
    public class CompanionOrderInterface
    {
        public static bool Active = false;
        public static int SelectedCompanion = 255;

        public static void Open()
        {
            Active = true;
        }

        public static void Close()
        {
            Active = false;
        }
    }
}