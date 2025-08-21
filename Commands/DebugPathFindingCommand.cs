using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace terraguardians
{
    public class DebugPathFindingCommand : ModCommand
    {
        public override string Name => "Debug Companions Path Finding";

        public override string Command => "debugpathfinding";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode > NetmodeID.SinglePlayer)
            {
                Main.NewText("Nice try.");
                return;
            }
            MainMod.DebugPathFinding = !MainMod.DebugPathFinding;
            Main.NewText("Pathfinding Debug is turned "+(MainMod.DebugPathFinding ? "ON" : "OFF")+".");
        }

        public override string Description => "When enabled, disables companions path finding seeking, and shows winning nodes.";
        public override string Usage => "/debugpathfinding";
    }
}