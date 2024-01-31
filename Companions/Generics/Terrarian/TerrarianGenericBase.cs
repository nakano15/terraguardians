using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics
{
    public class TerrarianGenericBase : TerrarianBase
    {
        public override string Name => "Terrarian";
        public override string Description => "Inhabitant of the Terra Realm. They're know to travel through worlds.";
        public override bool IsGeneric => true;
        public override bool RandomGenderOnSpawn => true;
        public override bool CanChangeGenders => true;
        public override PersonalityBase GetPersonality(Companion c)
        {
            return PersonalityDB.Neutral;
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new Terrarian.TerrarianGenericDialogue();

        public override string NameGeneratorParameters(CompanionData Data)
        {
            string FinalName = "";
            for (int i = 0; i < Main.rand.Next(8, 12); i++)
            {
                char c = (char)('a' + Main.rand.Next('z' - 'a'));
                if (i == 0)
                    c = char.ToUpper(c);
                FinalName += c;
            }
            return FinalName;
        }
    }
}