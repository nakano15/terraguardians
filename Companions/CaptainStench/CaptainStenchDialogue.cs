using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;


namespace terraguardians.Companions.CaptainStench
{
    public class CaptainStenchDialogue : CompanionDialogueContainer
    {
        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOption("Change GP.", DebugGPsDialogue);
        }

        void DebugGPsDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Watchu testin' now mait?*");
            md.AddOption("No Guardian Powder", ChangeToNoGP);
            md.AddOption("Sleuth Amber Guardian Powder", ChangeToAmberGP);
            md.AddOption("Sleuth Emerald Guardian Powder", ChangeToEmeraldGP);
            md.AddOption("Sleuth Ruby Guardian Powder", ChangeToRubyGP);
            md.AddOption("Sleuth Sapphire Guardian Powder", ChangeToSapphireGP);
            md.AddOption("Sleuth Topaz Guardian Powder", ChangeToTopazGP);
            md.AddOption("Sleuth Amethyst Guardian Powder", ChangeToAmethystGP);
            md.AddOption("Sleuth Diamond Guardian Powder", ChangeToDiamondGP);
            md.AddOption("Nevermind.", Dialogue.EndDialogue);
            md.RunDialogue();
        }

        void ChangeToNoGP()
        {
            _ChangeStenchGP(0);
        }

        void ChangeToAmberGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Amber);
        }

        void ChangeToEmeraldGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Emerald);
        }

        void ChangeToRubyGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Ruby);
        }

        void ChangeToSapphireGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Sapphire);
        }

        void ChangeToTopazGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Topaz);
        }

        void ChangeToAmethystGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Amethyst);
        }

        void ChangeToDiamondGP()
        {
            _ChangeStenchGP(CaptainStenchBase.WeaponInfusions.Diamond);
        }

        void _ChangeStenchGP(CaptainStenchBase.WeaponInfusions ID)
        {
            (Dialogue.Speaker as CaptainStenchBase.StenchCompanion).CurrentInfusion = ID;
                MessageDialogue md = new MessageDialogue();
            if (ID != CaptainStenchBase.WeaponInfusions.None)
            {
                md.ChangeMessage("*SNORT! Snif, snif.* Done.");
            }
            else
            {
                md.ChangeMessage("*Func!* Okay.");
            }
            md.AddOption("Close", Dialogue.EndDialogue);
            md.RunDialogue();
        }
    }
}