using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics.Terrarian
{
    public class TerrarianGenericDialogue : CompanionDialogueContainer
    {
        public override string NormalMessages(Companion companion)
        {
            return "Noob.";
        }
    }
}