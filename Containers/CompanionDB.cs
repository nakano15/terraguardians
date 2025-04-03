using Terraria.ModLoader;
using System.Collections.Generic;
using terraguardians.Companions;
using terraguardians.Companions.Generics;

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
            Celeste = 33,
            Leona = 34,
            Scaleforth = 35,
            Monica = 36;
        
        public const uint GenericTerrarian = 10000,
            GamerGenericTerrarian = 10001;

        public override CompanionBase GetCompanionDB(uint ID)
        {
            switch(ID)
            {
                case Rococo: return new RococoBase();
                case Blue: return new BlueBase();
                case Sardine: return new SardineBase();
                case Zacks: return new ZackBase();
                case Nemesis: return new NemesisBase();
                case Alex: return new AlexBase();
                case Brutus: return new BrutusBase();
                case Bree: return new BreeBase();
                case Mabel: return new MabelBase();
                case Domino: return new DominoBase();
                case Leopold: return new LeopoldBase();
                case Vladimir: return new VladimirBase();
                case Malisha: return new MalishaBase();
                case Michelle: return new MichelleBase();
                case Wrath: return new WrathBase();
                case Alexander: return new AlexanderBase();
                case Fluffles: return new FlufflesBase();
                case Minerva: return new MinervaBase();
                case Daphne: return new DaphneBase();
                case Liebre: return new LiebreBase();
                case Glenn: return new GlennBase();
                case CaptainStench: return new CaptainStenchBase();
                case Cinnamon: return new CinnamonBase();
                case Quentin: return new QuentinBase();
                case Miguel: return new MiguelBase();
                case Luna: return new LunaBase();
                case Green: return new GreenBase();
                case Cille: return new CilleBase();
                case Castella: return new CastellaBase();

                case Celeste: return new CelesteBase();
                case Leona: return new LeonaBase();
                case Scaleforth: return new ScaleforthBase();
                case Monica: return new MonicaBase();
                //
                case GenericTerrarian: return new TerrarianGenericBase();
                case GamerGenericTerrarian: return new GamerGenericBase();
            }
            return base.GetCompanionDB(ID);
        }
    }
}