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
        //https://whatifgaming.com/best-xbox-gamertag-ideas/
        static readonly string[] Tags = 
        {
            "Nexus",
            "Fury",
            "Quantum",
            "Strike",
            "Cyber",
            "Vortex",
            "Thunder",
            "Skull",
            "Phoenix",
            "Blaze",
            "Mystic",
            "Shade",
            "Aero",
            "Blitz",
            "Falcon",
            "Shadow",
            "Hunter",
            "Viper",
            "Frenzy",
            "Serenity",
            "Blade",
            "Echo",
            "Nemesis",
            "Pixel",
            "Dark",
            "Nova",
            "Spark",
            "Luna",
            "Marauder",
            "Pulse",
            "Apex",
            "Mango",
            "Hyper",
            "Raven",
            "Velocity",
            "Obsidian",
            "Edge",
            "Dream",
            "Weaver",
            "Atomic",
            "Specter",
            "Titan",
            "Wraith",
            "Crimson",
            "Dusk",
            "Dawn", //Why not?
            "Celestial",
            "Solar",
            "Storm",
            "Serpent",
            "Aurora",
            "Aegis",
            "Glide",
            "Ether",
            "Terra", //Why not?
            "Dragon",
            "Infinite",
            "Champion",
            "Star",
            "Neon",
            "Soul",
            "Byte",
            "Sniper",
            "Wolf", //Why not?
            "Cat", //Why not?
            "Dog" //Why not?
        };

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
            int Prefix = Main.rand.Next(Tags.Length), Suffix = Main.rand.Next(Tags.Length);
            while (Suffix == Prefix)
            {
                Suffix = Main.rand.Next(Tags.Length);
            }
            FinalName = Tags[Prefix]+Tags[Suffix];
            if (Main.rand.NextFloat() < 0.4f)
            {
                FinalName += Main.rand.Next(16, 30);
            }
            return FinalName;
        }
    }
}