using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Terraria.Map;

namespace terraguardians
{
    public class SystemMod : ModSystem
    {
        private Player[] BackedUpPlayers = new Player[Main.maxPlayers];
        private static CompanionMouseOverInterface CompanionMouseOverInterfaceDefinition;
        private static GroupMembersInterface GroupMembersInterfaceDefinition;
        private static CompanionInventoryInterface CompanionInventoryInterfaceDefinition;

        public override void Load()
        {
            CompanionMouseOverInterfaceDefinition = new CompanionMouseOverInterface();
            GroupMembersInterfaceDefinition = new GroupMembersInterface();
            CompanionInventoryInterfaceDefinition = new CompanionInventoryInterface();
        }

        public override void Unload()
        {
            BackedUpPlayers = null;
            CompanionMouseOverInterfaceDefinition = null;
            GroupMembersInterfaceDefinition = null;
            CompanionInventoryInterfaceDefinition = null;
        }

        private void BackupAndPlaceCompanionsOnPlayerArray()
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
                BackedUpPlayers[i] = Main.player[i];
            byte LastSlot = 254;
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                Main.player[LastSlot] = c;
                c.whoAmI = LastSlot;
                LastSlot--;
                if(LastSlot == 0) break;
            }
        }

        private void RestoreBackedUpPlayers()
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
                Main.player[i] = BackedUpPlayers[i];
            if(ProjMod.BackupMyPlayer > -1)
            {
                 Main.myPlayer = ProjMod.BackupMyPlayer;
                 ProjMod.BackupMyPlayer = -1;
            }
        }

        public override void PostUpdatePlayers()
        {
            uint[] Keys = MainMod.ActiveCompanions.Keys.ToArray();
            BackupAndPlaceCompanionsOnPlayerArray();
            foreach(uint i in Keys)
            {
                if(!MainMod.ActiveCompanions[i].active)
                {
                    MainMod.ActiveCompanions.Remove(i);
                }
                else
                {
                    MainMod.ActiveCompanions[i].UpdateCompanion();
                }
            }
            RestoreBackedUpPlayers();
        }

        public override void PreUpdateNPCs()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
        }

        public override void PostUpdateNPCs()
        {
            RestoreBackedUpPlayers();
        }

        public override void PreUpdateProjectiles()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
        }

        public override void PostUpdateProjectiles()
        {
            RestoreBackedUpPlayers();
        }

        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseInterfacePosition = -1, ResourceBarsPosition = -1, InventoryInterfacePosition = -1;
            for(int i = 0; i < layers.Count; i++)
            {
                switch(layers[i].Name)
                {
                    default: continue;
                    case "Vanilla: Mouse Text":
                        MouseInterfacePosition = i;
                        break;
                    case "Vanilla: Resource Bars":
                        ResourceBarsPosition = i;
                        break;
                    case "Vanilla: Inventory":
                        InventoryInterfacePosition = i;
                        break;
                }
            }
            if(InventoryInterfacePosition > -1) layers.Insert(InventoryInterfacePosition, CompanionInventoryInterfaceDefinition);
            if(MouseInterfacePosition > -1) layers.Insert(MouseInterfacePosition, CompanionMouseOverInterfaceDefinition);
            if(ResourceBarsPosition > -1) layers.Insert(ResourceBarsPosition, GroupMembersInterfaceDefinition);
        }
    }
}