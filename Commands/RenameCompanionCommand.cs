using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class RenameCompanionCommand : ModCommand
    {
        public override string Name => "Rename Companion";

        public override string Command => "renamecompanion";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.dedServ) return;
            if (args.Length >= 3)
            {
                uint ID;
                if (!uint.TryParse(args[0], out ID))
                {
                    Main.NewText("ID must be a number!", 255, 0,0);
                    return;
                }
                int Index = 1;
                string ModID = args[1];
                if (ModID.Contains('"'))
                {
                    string NewModID = ModID;
                    int NextIndex = Index;
                    bool ClosedBrackets = false;
                    while(++NextIndex < args.Length)
                    {
                        NewModID += " " + args[NextIndex];
                        if (args[NextIndex].Contains('"'))
                        {
                            ClosedBrackets = true;
                            break;
                        }
                    }
                    if (ClosedBrackets)
                    {
                        ModID = NewModID;
                        Index = NextIndex;
                    }
                }
                ModID = ModID.Replace("\"", "");
                if (!PlayerMod.PlayerHasCompanion(caller.Player, ID, ModID))
                {
                    Main.NewText("You can't rename a companion you don't have.", 255, 0, 0);
                    return;
                }
                Index++;
                string NewName = "";
                for(int a = Index; a < args.Length; a++)
                {
                    if (NewName.Length > 0)
                        NewName += " ";
                    NewName += args[a];
                }
                if (NewName.Length < 2 || NewName.Length > 16)
                {
                    Main.NewText("Name is either too short or too long.", 255, 0, 0);
                    return;
                }
                string TrimmedName = NewName.Trim();
                if (NewName.Length < 2)
                {
                    Main.NewText("Nice try.");
                    return;
                }
                CompanionData c = PlayerMod.PlayerGetCompanionData(caller.Player, ID, ModID);
                c.ChangeName(NewName);
                Main.NewText(c.Base.Name + " has been nicknamed "+c.GetName+".");
                if (PlayerMod.PlayerHasCompanionSummoned(caller.Player, ID, ModID))
                {
                    Companion c2 = PlayerMod.PlayerGetSummonedCompanion(caller.Player, ID, ModID);
                    c2.name = c2.Data.GetName;
                }
            }
            else
            {
                Main.NewText("Command is too short.", 255, 0, 0);
            }
        }

        public override string Description => "Allows you to rename a companion.";
        public override string Usage => "/renamecommand <ID> <ModID> <Name>. Use Selection Interface for this.";
    }
}