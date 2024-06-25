using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class ChangeCompanionFriendshipLevelCommand : ModCommand
    {
        public override string Name => "[Debug] Change Friendship Level";
        public override string Command => "changefriendshiplevel";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "When in Debug Mode, changes the Friendship Level of a companion of your choice.";
        public override string Usage => "/changefriendshiplevel [Level] [ID] \"[ModID]\"";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!MainMod.IsDebugMode)
            {
                Main.NewText("A wild Dungeon Guardian has appeared.", 255, 0, 0);
                NPC.SpawnOnPlayer(MainMod.GetLocalPlayer.whoAmI, 68);
                return;
            }
            uint ID;
            string ModID = "";
            int Level;
            if (!(args.Length >= 1 && int.TryParse(args[0], out Level)))
            {
                Main.NewText("You forgot to set the new friendship level and companion id.", 255, 0, 0);
                return;
            }
            if (!(args.Length >= 2 && uint.TryParse(args[1], out ID)))
            {
                Main.NewText("You must specify an id.", 255, 0, 0);
                return;
            }
            if (args.Length >= 3)
            {
                ModID = args[2];
            }
            CompanionBase b = MainMod.GetCompanionBase(ID, ModID);
            if (!PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, ID, ModID))
                Main.NewText("You don't have that companion.");
            else
            {
                PlayerMod.PlayerGetCompanionData(MainMod.GetLocalPlayer, ID, ModID).FriendshipProgress.Level = (byte)MathF.Min(255, Level);
                Main.NewText(PlayerMod.PlayerGetCompanionData(MainMod.GetLocalPlayer, ID, ModID).GetNameColored() + " friendship level changed to "+Level+".");
            }
        }
    }
}