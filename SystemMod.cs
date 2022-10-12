using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System.Linq;

namespace terraguardians
{
    public class SystemMod : ModSystem
    {
        private Player[] BackedUpPlayers = new Player[Main.maxPlayers];

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
    }
}