using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace terraguardians
{
    public class CompanionOrderInterface : GameInterfaceLayer
    {
        public static bool Active = false;
        public static int SelectedCompanion = 255;

        public CompanionOrderInterface() : 
            base("TerraGuardians: Orders UI", InterfaceScaleType.UI)
        {
            
        }

        public static void Open()
        {
            Active = true;
        }

        public static void Close()
        {
            Active = false;
        }

        protected override bool DrawSelf()
        {
            
            return base.DrawSelf();
        }

        public class CompanionOrderStep
        {
            public string Text = "";

            public virtual void OnActivate()
            {
                
            }
        }
    }
}