using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions;

public class MonicaDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. You're... A Terrarian..?*";
            case 1:
                return "*Oh, hi! I never met someone like you before..*";
            case 2:
                return "*Hello... Please don't make comments about my belly..*";
        }
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        //Need to work on her dialogues...
        return base.NormalMessages(companion);
    }
}