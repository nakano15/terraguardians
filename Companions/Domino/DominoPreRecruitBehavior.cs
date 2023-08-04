using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Domino
{
    public class DominoRecruitmentBehavior : PreRecruitBehavior
    {
        public TerraGuardian Brutus = null;
        private byte SceneIndex = 255;
        private ushort SceneTime = 0;
        private const ushort DialogueStepTime = 300;
        private bool PlayerHasDomino = false;

        public DominoRecruitmentBehavior() : base()
        {
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            AllowSeekingTargets = false;
        }

        public override bool AllowDespawning => true;

        public override string CompanionNameChange(Companion companion)
        {
            return "Shady Dog Guardian";
        }

        public override void Update(Companion companion)
        {
            if (Brutus == null)
            {
                WanderAI(companion);
                if (Target != null)
                {
                    if (SceneIndex == 255 && PlayerMod.PlayerHasCompanionSummoned(Target, CompanionDB.Brutus))
                    {
                        Brutus = (TerraGuardian)PlayerMod.PlayerGetSummonedCompanion(Target, CompanionDB.Brutus);
                        Brutus.FaceSomething(companion);
                        Brutus.SaySomething("*Halt! You there!*");
                        SceneIndex = 0;
                        SceneTime = 0;
                    }
                    PlayerHasDomino = PlayerMod.PlayerHasCompanion(Target, CompanionDB.Domino);
                }
            }
            else if (!Dialogue.InDialogue || Dialogue.Speaker != companion)
            {
                bool FaceEachOther = false;
                byte LastSceneIndex = SceneIndex;
                if (!PlayerHasDomino)
                {
                    switch (SceneIndex)
                    {
                        case 0:
                            FaceEachOther = true;
                            if (SceneTime == 60)
                                companion.SaySomething("*Uh oh.*");
                            if (SceneTime == 90)
                                ChangeScene(1);
                            break;
                        case 1:
                            if (SceneTime == 30)
                                Brutus.SaySomething("*After him! Don't let him escape!*");
                            if(Target.Center.X < companion.Center.X)
                                companion.MoveRight = true;
                            else
                                companion.MoveLeft = true;
                            if (Math.Abs(Target.Center.X - companion.Center.X) >= NPC.sWidth ||
                                Math.Abs(Target.Center.Y - companion.Center.Y) >= NPC.sHeight)
                            {
                                Brutus.SaySomething("*Blast it! He got away! He'll return, for sure.*");
                                WorldMod.RemoveCompanionNPC(companion);
                                return;
                            }
                            if (Target.Hitbox.Intersects(companion.Hitbox))
                            {
                                ChangeScene(2);
                                Brutus.SaySomething("*I got you now! You won't run away anymore!*");
                            }
                            break;
                        case 2:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Ah, then it was you who was chasing me.*");
                                ChangeScene(3);
                            }
                            break;
                        case 3:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Didn't you lose your place as a Royal Guard in the Ether Realm?*");
                                ChangeScene(4);
                            }
                            break;
                        case 4:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*If it wasn't for you, I wouldn't have lost my job. Beside, I was already sick of it.*");
                                ChangeScene(5);
                            }
                            break;
                        case 5:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Pft. Whatever the reason was, I doubt I caused that.*");
                                ChangeScene(6);
                            }
                            break;
                        case 6:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*I'm going to have you locked behind bars forever for smuggling!*");
                                ChangeScene(7);
                            }
                            break;
                        case 7:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Arrest me, how? You're no longer a guard or anything.*");
                                ChangeScene(8);
                            }
                            break;
                        case 8:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*Ugh... Uh...*");
                                ChangeScene(10);
                            }
                            break;
                        case 10:
                            FaceEachOther = true;
                            if (SceneTime >= 90)
                            {
                                companion.SaySomething("*See? You can't arrest me. And the laws of the Ether Realm aren't valid here.*");
                                ChangeScene(11);
                            }
                            break;
                        case 11:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Now, if you don't mind, I'd like to talk with that Terrarian you follow.*");
                                ChangeScene(12);
                            }
                            break;
                        case 12:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*Terrarian, come talk to me.*");
                                ChangeScene(13);
                            }
                            break;
                        case 13:
                            companion.FaceSomething(Target);
                            break;
                        case 15:
                            FaceEachOther = true;
                            if (SceneTime >= 90)
                            {
                                companion.SaySomething("*Looks like I will be staying here.*");
                                ChangeScene(16);
                            }
                            break;
                        case 16:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*Oh great. Now I have to share this place with one of my most hated persons.*");
                                ChangeScene(17);
                            }
                            break;
                        case 17:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime + 60)
                            {
                                companion.SaySomething("*Yeah, I love having you as my neighbor too. Now if you don't mind, I have things to move in here.*");
                                ChangeScene(18);
                            }
                            break;
                        case 18:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime + 60)
                            {
                                Brutus.SaySomething("*Mark my words. If you do something bad to the citizens here, I will take care of you personally.*");
                                ChangeScene(19);
                            }
                            break;
                        case 19:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime + 90)
                            {
                                companion.SaySomething("*Yeah, yeah. Whatever. Call me Domino.*");
                                PlayerMod.PlayerAddCompanion(Target, companion);
                                WorldMod.AddCompanionMet(companion);
                                WorldMod.AllowCompanionNPCToSpawn(companion);
                                return;
                            }
                            break;

                        case 25:
                            FaceEachOther = true;
                            if (SceneTime >= 90)
                            {
                                companion.SaySomething("*Well, I guess I should find another place to stay then..*");
                                ChangeScene(26);
                            }
                            break;
                        case 26:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*Good riddance, "+Target.name+", we don't need to have a criminal living here.*");
                                ChangeScene(27);
                            }
                            break;
                        case 27:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                companion.SaySomething("*I'll still let the Terrarian contact me anytime, should they change their mind.*");
                                ChangeScene(28);
                            }
                            break;
                        case 28:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*I sure hope not..*");
                                ChangeScene(29);
                            }
                            break;
                        case 29:
                            FaceEachOther = true;
                            if (SceneTime >= DialogueStepTime)
                            {
                                Brutus.SaySomething("*That's up to the Terrarian. You can call me Domino.*");
                                ChangeScene(30);
                                PlayerMod.PlayerAddCompanion(Target, companion);
                                WorldMod.AddCompanionMet(companion);
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    switch (SceneIndex)
                    {
                        case 0:
                            if (SceneTime == 60)
                            {
                                companion.SaySomething("*This again?!*");
                                ChangeScene(1);
                            }
                            break;
                        case 1:
                            if (SceneTime == 120)
                            {
                                if (Brutus != null)
                                {
                                    Brutus.SaySomething("*You have the audacity of showing up again.*");
                                }
                                else
                                {
                                    companion.SaySomething("*Ah, you're not with him... Good.*");
                                }
                                ChangeScene(2);
                            }
                            break;
                        case 2:
                            if (SceneTime == DialogueStepTime)
                            {
                                if (Brutus != null)
                                {
                                    companion.SaySomething("*Yeah right, like as if I'm happy for seeing you again.*");
                                }
                                else
                                {
                                    companion.SaySomething("*Since you're here too, I don't think you'll mind if I open a business here too.*");
                                }
                                ChangeScene(3);
                            }
                            break;
                        case 3:
                            if (SceneTime == DialogueStepTime)
                            {
                                if (Brutus != null)
                                {
                                    companion.SaySomething("*Terrarian, you know the drill. Need me for anything, I will be here.*");
                                }
                                else
                                {
                                    companion.SaySomething("*Or, if you need my help for anything, you know where to find me.*");
                                }
                                ChangeScene(4);
                            }
                            break;
                        case 4:
                            if (SceneTime == DialogueStepTime)
                            {
                                if (Brutus != null)
                                {
                                    Brutus.SaySomething("*Ugh...*");
                                }
                                ChangeScene(3);
                                WorldMod.AddCompanionMet(companion);
                            }
                            break;
                    }
                }
                if (FaceEachOther)
                {
                    if (companion.itemAnimation == 0) companion.FaceSomething(Brutus);
                    if (Brutus.itemAnimation == 0) Brutus.FaceSomething(companion);
                }
                if (LastSceneIndex == SceneIndex && SceneTime < ushort.MaxValue)
                    SceneTime ++;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return SceneIndex == 255 || (!PlayerHasDomino && (SceneIndex == 12 || SceneIndex == 13));
        }

        private void ChangeScene(byte NewScene)
        {
            SceneIndex = NewScene;
            SceneTime = 0;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            PlayerHasDomino = PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion);
            Brutus = (TerraGuardian)PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Brutus);
            if (SceneIndex == 255) //No scene playing.
            {
                //A quest asking the player to find Illegal Gun Parts, if the player doesn't has Brutus.
                //If the player has Brutus, should spook him instead, and jump to the scene past catching him.
                if (PlayerHasDomino)
                {
                    return GetPlayerKnowDominoDialogueLobby();
                }
                if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Brutus))
                {
                    return GetBrutusCatchesDominoInLobby();
                }
                return GetDominoMakesBusinessWithPlayerLobby();
            }
            return GetDominoAskPlayerToMoveInLobby();
        }

        private MessageDialogue GetPlayerKnowDominoDialogueLobby()
        {
            MessageDialogue md = new MessageDialogue("*Hello again. Mind if I extend my shop to this world too?*");
            md.AddOption("Sure.", PlayerAllowsKnownDominoToMoveIn);
            md.AddOption("Nope.", PlayerDoesntAllowsKnownDominoToMoveIn);
            return md;
        }

        private void PlayerAllowsKnownDominoToMoveIn()
        {
            WorldMod.AddCompanionMet(CompanionDB.Domino);
            WorldMod.AllowCompanionNPCToSpawn(CompanionDB.Domino);
            Dialogue.LobbyDialogue("*Thanks mate.*");
        }

        private void PlayerDoesntAllowsKnownDominoToMoveIn()
        {
            WorldMod.AddCompanionMet(CompanionDB.Domino);
            Dialogue.LobbyDialogue("*Huh, fine. If you change your mind, do let me know.*");
        }

        private MessageDialogue GetDominoAskPlayerToMoveInLobby()
        {
            MessageDialogue md = new MessageDialogue("*I have some goods I can sell, If you don't mind, I would like to open a shop here. What do you say?*");
            md.AddOption("Feel free to.", PlayerLetsDominoMoveIn);
            md.AddOption("No way! Go away!", PlayerDoesntLetsDominoMoveIn);
            return md;
        }

        private void PlayerLetsDominoMoveIn()
        {
            MessageDialogue md = new MessageDialogue("*Wise choice, Terrarian.*");
            md.RunDialogue();
            ChangeScene(15);
        }

        private void PlayerDoesntLetsDominoMoveIn()
        {
            MessageDialogue md = new MessageDialogue("*Eh... Alright... Go Team Brutus... Happy now?*");
            md.RunDialogue();
            ChangeScene(25);
        }

        private MessageDialogue GetBrutusCatchesDominoInLobby()
        {
            MessageDialogue md = new MessageDialogue("*W-What? Brutus? Haha, you finally caught me, huh?*");
            ChangeScene(3);
            return md;
        }

        private MessageDialogue GetDominoMakesBusinessWithPlayerLobby()
        {
            MessageDialogue md = new MessageDialogue("*Hello there, I'm... A travelling merchant, and I'm looking for an special item...*");
            md.AddOption("What item?", PlayerAsksWhatItem);
            md.AddOption("I don't really care.", Dialogue.EndDialogue);
            return md;
        }

        private void PlayerDoesntCare()
        {
            MessageDialogue md = new MessageDialogue("*Not a very interested person, huh? You doesn't seems interesting either. Bye.*");
            md.RunDialogue();
        }

        private void PlayerAsksWhatItem()
        {
            MessageDialogue md = new MessageDialogue("*The item seems to be called \"Illegal Gun Parts\", said to be banned in many Terrarian kingdoms. Do you know if it can be found here?*");
            md.AddOption("I have it.", PlayerTriesToGiveGunParts);
            md.AddOption("The Arms Dealer might have it.", PlayerTalksAboutArmsDealer);
            md.AddOption("I'm not helping you with that.", PlayerRefuses);
            md.RunDialogue();
        }

        private void PlayerTriesToGiveGunParts()
        {
            MessageDialogue md = new MessageDialogue();
            if (MainMod.GetLocalPlayer.HasItem(Terraria.ID.ItemID.IllegalGunParts))
            {
                md.ChangeMessage("*Hm, that seems to match the description. Would you mind selling me it for 15 Gold Coins?*");
                md.AddOption("Alright.", SellFor15G);
                md.AddOption("No way.", RefuseSellFor15G);
            }
            else
            {
                md.ChangeMessage("*You have? Then where is it? Seems like you got a nothing sandwich instead.*");
            }
            md.RunDialogue();
        }

        private void SellFor15G()
        {
            MessageDialogue md = new MessageDialogue("*Perfect. Exactly what I needed. And this give me some security too. Would you mind if I opened a shop here?*");
            ExchangeForGunparts(15);
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Domino);
            WorldMod.AddCompanionMet(CompanionDB.Domino);
            md.AddOption("Sure.", PlayerLetsDominoMoveIn);
            md.AddOption("No.", PlayerDoesntLetsDominoOpenShop);
            md.RunDialogue();
        }

        private void RefuseSellFor15G()
        {
            MessageDialogue md = new MessageDialogue("*Hm, that offer doesn't please you at all? What about selling for 25 Golds instead?*");
            md.AddOption("Alright.", SellFor25G);
            md.AddOption("No way.", StillRefuseToSell);
            md.RunDialogue();
        }

        private void SellFor25G()
        {
            MessageDialogue md = new MessageDialogue("*Perfect. Exactly what I needed. And this give me some security too. Would you mind if I opened a shop here?*");
            ExchangeForGunparts(25);
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Domino);
            WorldMod.AddCompanionMet(CompanionDB.Domino);
            md.AddOption("Sure.", PlayerLetsDominoMoveIn);
            md.AddOption("No.", PlayerDoesntLetsDominoOpenShop);
            md.RunDialogue();
        }

        private void StillRefuseToSell()
        {
            MessageDialogue md = new MessageDialogue("*Then there's no higher offer I can do for you.*");
            md.RunDialogue();
        }

        private void ExchangeForGunparts(int Golds)
        {
            for(int i = 0; i < 50; i++)
            {
                if (MainMod.GetLocalPlayer.inventory[i].type == Terraria.ID.ItemID.IllegalGunParts)
                {
                    MainMod.GetLocalPlayer.inventory[i].stack--;
                    if (MainMod.GetLocalPlayer.inventory[i].stack <= 0)
                        MainMod.GetLocalPlayer.inventory[i].TurnToAir();
                    break;
                }
            }
            Item.NewItem(new Terraria.DataStructures.EntitySource_Misc(""), MainMod.GetLocalPlayer.Center, Microsoft.Xna.Framework.Vector2.One, Terraria.ID.ItemID.GoldCoin, Golds);
        }

        private void PlayerTalksAboutArmsDealer()
        {
            MessageDialogue md = new MessageDialogue("*Arms Dealer, huh? Just like that? I don't trust that at all.*");
            md.RunDialogue();
        }

        private void PlayerRefuses()
        {
            MessageDialogue md = new MessageDialogue("*Fine. I'll look into it myself then.*");
            md.RunDialogue();
        }

        private void PlayerLetsDominoOpenShop()
        {
            WorldMod.AllowCompanionNPCToSpawn(CompanionDB.Domino);
            Dialogue.LobbyDialogue("*Perfect. I will begin moving my things. You can call me Domino, that's what people who I do business with call me.*");
        }

        private void PlayerDoesntLetsDominoOpenShop()
        {
            Dialogue.LobbyDialogue("*I see. Well, should you change your mind, you have my contact. You can call me Domino.*");
        }
    }
}
