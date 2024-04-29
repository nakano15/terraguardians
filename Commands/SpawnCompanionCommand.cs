using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class SpawnCompanionCommand : ModCommand
    {
        public override string Name => "Spawn Companion";
        public override string Command => "spawncompanion";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "When in Debug Mode, adds a companion to your companion list based on id.";
        public override string Usage => "/spawncompanion [ID] \"[ModID]\"";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!MainMod.IsDebugMode)
            {
                Main.NewText("Dungeon Guardian answered your summon.", 255, 0, 0);
                NPC.SpawnOnPlayer(MainMod.GetLocalPlayer.whoAmI, 68);
                return;
            }
            uint ID;
            string ModID = "";
            if (!(args.Length >= 1 && uint.TryParse(args[0], out ID)))
            {
                Main.NewText("You must specify an id.", 255, 0, 0);
                return;
            }
            if (args.Length >= 2)
            {
                ModID = args[1];
            }
            CompanionBase b = MainMod.GetCompanionBase(ID, ModID);
            if (b.IsInvalidCompanion)
                Main.NewText("Nobody answered your summons.");
            else
            {
                if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, ID, ModID))
                {
                    Main.NewText("You already know " + b.DisplayName + ".");
                }
                else
                {
                    PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, ID, ModID);
                    Main.NewText(b.DisplayName + " has joined your companions list.");
                }
            }
        }
    }
}