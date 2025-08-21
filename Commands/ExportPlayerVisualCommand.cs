using Terraria;
using Terraria.ModLoader;
using System;
using System.IO;
using Terraria.ID;

namespace terraguardians
{
    public class ExportPlayerVisualCommand : ModCommand
    {
        public override string Command => "exportvisual";

        public override string Description => "Saves some informations about your character that will be useful when creating Terrarian companions.";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
                return;
            string Infos = "Name: " + caller.Player.name+ Environment.NewLine +
                "Hair Style: " + caller.Player.hair + Environment.NewLine +
                "Is Male? " + caller.Player.Male + Environment.NewLine +
                "Skin Variant: " + caller.Player.skinVariant + Environment.NewLine +
                "Hair Color: " + caller.Player.hairColor + Environment.NewLine +
                "Eyes Color: " + caller.Player.eyeColor + Environment.NewLine +
                "Skin Color: " + caller.Player.skinColor + Environment.NewLine +
                "Shirt Color: " + caller.Player.shirtColor + Environment.NewLine +
                "Undershirt Color: " + caller.Player.underShirtColor + Environment.NewLine +
                "Pants Color: " + caller.Player.pantsColor + Environment.NewLine +
                "Shoes Color: " + caller.Player.shoeColor + Environment.NewLine + 
                "Head Equip Slot: " + caller.Player.head + Environment.NewLine + 
                "Body Equip Slot: " + caller.Player.body + Environment.NewLine +
                "Legs Equip Slot: " + caller.Player.legs;
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/PlayerSkinInfo.txt";
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(Infos);
                }
            }
            Main.NewText("Your character infos are now in your desktop.");
        }
    }
}