using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics
{
    public class GamerGenericBase : TerrarianBase
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
            "Dog", //Why not?
            "Skunk",
            "Smelly"
        };

        public override string Name => "Gamer";
        public override string Description => "Tired of playing singleplayer, they move to other worlds to roast other players.";
        public override bool IsGeneric => true;
        public override bool RandomGenderOnSpawn => true;
        public override bool CanChangeGenders => true;
        public override PersonalityBase GetPersonality(Companion c)
        {
            return PersonalityDB.Neutral;
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new Terrarian.GamerGenericDialogue();

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