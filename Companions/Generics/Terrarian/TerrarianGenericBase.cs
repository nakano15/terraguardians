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
        public override string Description => "Inhabitant of the Terra Realm.\nThey're know to travel through worlds.";
        public override bool IsGeneric => true;
        public override bool RandomGenderOnSpawn => true;
        public override bool CanChangeGenders => true;
        public override PersonalityBase GetPersonality(Companion c)
        {
            return PersonalityDB.Neutral;
        }
        public override BehaviorBase PreRecruitmentBehavior => new TerrarianGenericPreRecruitBehavior();
        protected override CompanionDialogueContainer GetDialogueContainer => new Terrarian.TerrarianGenericDialogue();
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks()
        {
            ChangeEquipmentLevelUnlock = 5
        };

        static readonly string[] MaleStartNames = 
            ["Jo", "Ro", "Da", "Ra", "Do", "Lo", "Ga", "Te", "He", "Na"];

        static readonly string[] FemaleStartNames = 
            ["Ma", "Rai", "Dia", "Lu", "Na", "Ji", "She", "Mo", "La"];


        static readonly string[] MiddleNames = 
            ["Ka", "Ge", "Der", "Bel", "Qua", "Quo", "Il", "Ni", "Di"];

        static readonly string[] EndNames = 
            ["Dan", "Gar", "Dor", "Ser", "Ro", "Do", "No", "Kor", "You"];

        public override string NameGeneratorParameters(CompanionData Data)
        {
            string FinalName = "";
            if (Data.Gender == Genders.Male || (Data.Gender == Genders.Genderless && Main.rand.Next(2) == 0))
            {
                FinalName += MaleStartNames[Main.rand.Next(MaleStartNames.Length)];
            }
            else
            {
                FinalName += FemaleStartNames[Main.rand.Next(FemaleStartNames.Length)];
            }
            if (Main.rand.NextFloat() < .4f)
                FinalName += MiddleNames[Main.rand.Next(MiddleNames.Length)].ToLower();
            FinalName += EndNames[Main.rand.Next(EndNames.Length)].ToLower();
            return FinalName;
        }
    }
}