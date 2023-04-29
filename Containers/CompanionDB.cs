using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionDB : CompanionContainer
    {
        public const uint Rococo = 0,
            Blue = 1,
            Sardine = 2,
            Zacks = 3,
            Zack = 3,
            Nemesis = 4,
            Alex = 5,
            Brutus = 6,
            Bree = 7,
            Mabel = 8,
            Domino = 9,
            Leopold = 10,
            Vladimir = 11,
            Malisha = 12,
            Michelle = 13,
            Wrath = 14,
            Alexander = 15,
            Fluffles = 16,
            Minerva = 17,
            Daphne = 18,
            Liebre = 19,
            Bapha = 20,
            Glenn = 21,
            CaptainStench = 22,
            Cinnamon = 23,
            Quentin = 24,
            Miguel = 25,
            Luna = 26,
            Fear = 27,
            Sadness = 28,
            Joy = 29,
            Green = 30,
            Cille = 31,
            Castella = 32,
            Celeste = 33;

        public override CompanionBase GetCompanionDB(uint ID)
        {
            switch(ID)
            {
                case Rococo: return new Companions.RococoBase();
                case Blue: return new Companions.BlueBase();
                case Sardine: return new Companions.SardineBase();
                case Zacks: return new Companions.ZackBase();
                case Nemesis: return new Companions.NemesisBase();
                case Alex: return new Companions.AlexBase();
                case Brutus: return new Companions.BrutusBase();
                case Bree: return new Companions.BreeBase();
                case Mabel: return new Companions.MabelBase();

                case Michelle: return new Companions.MichelleBase();

                case Luna: return new Companions.LunaBase();

                case Celeste: return new Companions.CelesteBase();
            }
            return base.GetCompanionDB(ID);
        }
    }
}