using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Scaleforth;

public class ScaleforthDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. I'm [name]. Are you looking for a butler?*";
        }
    }
}