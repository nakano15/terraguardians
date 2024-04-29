using Terraria;
using Terraria.ModLoader;
using System;
using System.IO;

namespace terraguardians
{
    public class SetDebugModeCharacterCommand : ModCommand
    {
        public override string Command => "setasdebugmodecharacter";

        public override CommandType Type => CommandType.World;
        public override string Name => "Set as Debug Mode Character";
        public override string Description => "Flags this character as a Debug Mode character.\nWarning! It can't be undone once done so.\nYou must type \"Yes\" without the \" as command argument to work.";
        public override string Usage => "/setasdebugmodecharacter Yes";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "yes")
            {
                PlayerMod pm = caller.Player.GetModPlayer<PlayerMod>();
                if (pm.IsDebugModeCharacter)
                {
                    Main.NewText("This is already a debug mode character.");
                }
                else
                {
                    pm.SetDebugModeCharacter();
                    Main.NewText("Character set as Debug Mode character. This can't be undone anymore.");
                }
            }
        }
    }
}