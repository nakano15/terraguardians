using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace terraguardians.Companions.Leopold
{
    public class LeopoldRecruitmentBehavior : PreRecruitNoMonsterAggroBehavior
    {
        public SceneIDs SceneID = SceneIDs.NoScene;
        public int SceneTime = 0;
        public bool EncounterAct { get { return SceneID >= SceneIDs.LeopoldSpotsThePlayer && SceneID < SceneIDs.ThinksAboutTryingToTalk; } }
        public bool SocialAct { get { return SceneID >= SceneIDs.ThinksAboutTryingToTalk && SceneID < SceneIDs.ThinksThePlayerDidntUnderstandHim; } }
        public bool FearAct { get { return SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.AttemptsToRunAway1; } }
        public bool ScareAct { get { return SceneID >= SceneIDs.FearsAboutPlayerAttackingHim && SceneID < SceneIDs.TellsThatIsgoingToFlee; } }
        public bool PlayerHasLeopold = false;
        private Player SpottedPlayer;

        public override string CompanionNameChange(Companion companion)
        {
            return "Bunny Guardian";
        }

        public override void Update(Companion companion)
        {
            if (SceneID == SceneIDs.NoScene)
            {
                WanderAI(companion);
                SpottedPlayer = ViewRangeCheck(companion, companion.direction, 400);
                if (SpottedPlayer != null)
                {
                    PlayerHasLeopold = PlayerMod.PlayerHasCompanion(SpottedPlayer, CompanionDB.Leopold);
                    if (PlayerHasLeopold)
                    {
                        ChangeScene(SceneIDs.KnownPlayerSpottedByLeopold);
                        companion.SaySomething("*Aaahhh!!! What is that?!*");
                    }
                    else
                    {
                        ChangeScene(SceneIDs.LeopoldSpotsThePlayer);
                        companion.SaySomething("*Ack! W-what is that?!*");
                    }
                }
            }
            else
            {
                if (SpottedPlayer == null || !SpottedPlayer.active)
                {
                    SpottedPlayer = null;
                    companion.SaySomething("*Oh, they disappeared.*");
                    ChangeScene(SceneIDs.NoScene);
                    return;
                }
                if (SpottedPlayer.dead)
                {
                    SpottedPlayer = null;
                    companion.SaySomething("*I really didn't want to witness that. Poor creature.*");
                    ChangeScene(SceneIDs.NoScene);
                    return;
                }
                float PlayerDistance = SpottedPlayer.Distance(companion.Center);
                if (SceneID < SceneIDs.ThinksAboutTryingToTalk)
                {
                    if (PlayerDistance < 150)
                    {
                        companion.SaySomething("*Ah!! It approached!*");
                        ChangeScene(SceneIDs.WondersIfPlayerWillAttackHim);
                        companion.velocity.Y = -companion.Base.JumpSpeed;
                        companion.WalkMode = false;
                    }
                }
                if (SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.PlayerDoesNothing)
                {
                    if (PlayerDistance < 150)
                    {
                        companion.MoveRight = companion.MoveLeft = false;
                        companion.WalkMode = false;
                        if (SpottedPlayer.Center.X < companion.Center.X)
                        {
                            companion.MoveRight = true;
                        }
                        else
                        {
                            companion.MoveLeft = true;
                        }
                    }
                }
                if (SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.AttemptsToRunAway1)
                {
                    if (PlayerDistance < 100)
                    {
                        companion.SaySomething("*Yaah!! Please, don't kill me!*");
                        ChangeScene(SceneIDs.Crying1);
                        companion.velocity.Y = -companion.Base.JumpSpeed;
                        companion.WalkMode = false;
                    }
                }
                if (companion.velocity.X == 0)
                    companion.FaceSomething(SpottedPlayer);
                if (SceneTime <= 0)
                {
                    switch (SceneID)
                    {
                        case SceneIDs.LeopoldSpotsThePlayer:
                            companion.SaySomething("*It's a funny looking creature.*");
                            ChangeScene(SceneIDs.LeopoldSaysNeverSawAnythingLikeThat);
                            break;
                        case SceneIDs.LeopoldSaysNeverSawAnythingLikeThat:
                            companion.SaySomething("*I've never seen something like that.*");
                            ChangeScene(SceneIDs.LeopoldQuestionsHimselfAboutCreature);
                            break;
                        case SceneIDs.LeopoldQuestionsHimselfAboutCreature:
                            companion.SaySomething("*What kind of creature is it?*");
                            ChangeScene(SceneIDs.IsItTerrarian);
                            break;
                        case SceneIDs.IsItTerrarian:
                            companion.SaySomething("*Could It be a Terrarian?*");
                            ChangeScene(SceneIDs.NoticesOutfit);
                            break;
                        case SceneIDs.NoticesOutfit:
                            companion.SaySomething("*It surely has an unusual outfit...*");
                            ChangeScene(SceneIDs.QuestionsIfIsReallyATerrarian);
                            break;
                        case SceneIDs.QuestionsIfIsReallyATerrarian:
                            {
                                companion.SaySomething("*Maybe, It is said that they inhabit this world.*");
                                Companion leader = PlayerMod.GetPlayerMainGuardian(SpottedPlayer);
                                if (leader != null && leader.GetGroup.IsTerraGuardian)
                                    ChangeScene(SceneIDs.NoticesOtherGuardians);
                                else
                                    ChangeScene(SceneIDs.NoticesPlayerLooking);
                            }
                            break;
                        case SceneIDs.NoticesPlayerLooking:
                            if ((SpottedPlayer.direction == 1 && companion.Center.X > SpottedPlayer.Center.X) || 
                                (SpottedPlayer.direction == -1 && companion.Center.X < SpottedPlayer.Center.X))
                            {
                                companion.SaySomething("*It's looking at me.*");
                            }
                            else
                            {
                                companion.SaySomething("*Is it even aware that I'm here?*");
                            }
                            ChangeScene(SceneIDs.WondersPlayerReaction);
                            break;
                        case SceneIDs.WondersPlayerReaction:
                            companion.SaySomething("*What is it planning to do?*");
                            ChangeScene(SceneIDs.IsPreparingAttack);
                            break;
                        case SceneIDs.IsPreparingAttack:
                            companion.SaySomething("*Is it preparing to attack?*");
                            ChangeScene(SceneIDs.ThreatenUseSpell);
                            break;
                        case SceneIDs.ThreatenUseSpell:
                            companion.SaySomething("*If It tries to attack, I will blow It with my spells.*");
                            ChangeScene(SceneIDs.FindsWeirdTheNoReaction, true);
                            break;
                        case SceneIDs.FindsWeirdTheNoReaction:
                            companion.SaySomething("*Weird. The Terrarian isn't doing anything.*");
                            ChangeScene(SceneIDs.ThinksAboutTryingToTalk);
                            break;
                        case SceneIDs.ThinksAboutTryingToTalk:
                            companion.SaySomething("*Maybe If I try talking...*");
                            ChangeScene(SceneIDs.WondersIftheySpeak);
                            break;
                        case SceneIDs.WondersIftheySpeak:
                            companion.SaySomething("*No... Wait, I don't even know if they can speak.*");
                            ChangeScene(SceneIDs.MentionsABook);
                            break;
                        case SceneIDs.MentionsABook:
                            companion.SaySomething("*There is a book that theorized that but...*");
                            ChangeScene(SceneIDs.ThinksAboutTrying);
                            break;
                        case SceneIDs.ThinksAboutTrying:
                            companion.SaySomething("*Maybe If I try...*");
                            ChangeScene(SceneIDs.TriesTalking);
                            break;
                        case SceneIDs.TriesTalking:
                            companion.SaySomething("*H-hey... Can you hear me?*");
                            ChangeScene(SceneIDs.WondersIfIsScared, true);
                            break;
                        case SceneIDs.WondersIfIsScared:
                            companion.SaySomething("*(Maybe It's scared...)*");
                            ChangeScene(SceneIDs.SaysWontHurt);
                            break;
                        case SceneIDs.SaysWontHurt:
                            companion.SaySomething("*Come here.. I wont hurt you...*");
                            ChangeScene(SceneIDs.TriesHidingFear, true);
                            break;
                        case SceneIDs.TriesHidingFear:
                            companion.SaySomething("*(..Don't tremble... You don't want to scare it...)*");
                            ChangeScene(SceneIDs.NoticesDidntWorked);
                            break;
                        case SceneIDs.NoticesDidntWorked:
                            companion.SaySomething("*(Huh? Didn't worked? Maybe If...)*");
                            ChangeScene(SceneIDs.TriesGivingFood);
                            break;
                        case SceneIDs.TriesGivingFood:
                            companion.SaySomething("*Uh... I... Got some food.. Do you want it...?*");
                            ChangeScene(SceneIDs.WondersHowStupidHisActionWas);
                            break;
                        case SceneIDs.WondersHowStupidHisActionWas:
                            companion.SaySomething("*(Got some food?! What am I thinking?!)*");
                            ChangeScene(SceneIDs.WaitsAFewMoments);
                            break;
                        case SceneIDs.WaitsAFewMoments:
                            companion.SaySomething("*...*");
                            ChangeScene(SceneIDs.ThinksThePlayerDidntUnderstandHim);
                            break;
                        case SceneIDs.ThinksThePlayerDidntUnderstandHim:
                            companion.SaySomething("*I guess It can't understand me at all.*");
                            ChangeScene(SceneIDs.TalksAboutWalkingAway);
                            break;
                        case SceneIDs.TalksAboutWalkingAway:
                            companion.SaySomething("*Maybe If I just walk away...*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                            ////////////////////
                        case SceneIDs.GotScaredUponPlayerApproach:
                            companion.SaySomething("*Ah!! It approached.*");
                            ChangeScene(SceneIDs.WondersIfPlayerWillAttackHim);
                            break;
                        case SceneIDs.WondersIfPlayerWillAttackHim:
                            companion.SaySomething("*Is it going to attack me?!*");
                            ChangeScene(SceneIDs.FearPlayerAttack1);
                            break;
                        case SceneIDs.FearPlayerAttack1:
                            companion.SaySomething("*No!! I'm too young to die!!*");
                            ChangeScene(SceneIDs.FearPlayerAttack2);
                            break;
                        case SceneIDs.FearPlayerAttack2:
                            companion.SaySomething("*I haven't even finished discovering the mysteries of the Terra Realm.*");
                            ChangeScene(SceneIDs.FearPlayerAttack3);
                            break;
                        case SceneIDs.FearPlayerAttack3:
                            companion.SaySomething("*This... This is how I'm going to die?!*");
                            ChangeScene(SceneIDs.FearPlayerAttack4);
                            break;
                        case SceneIDs.FearPlayerAttack4:
                            companion.SaySomething("*The great Leopold... Devoured by a Terrarian...*");
                            ChangeScene(SceneIDs.FearPlayerAttack5);
                            break;
                        case SceneIDs.FearPlayerAttack5:
                            companion.SaySomething("*Oh... What a cruel world...*");
                            ChangeScene(SceneIDs.PlayerDoesNothing, true);
                            break;
                        case SceneIDs.PlayerDoesNothing:
                            companion.SaySomething("....");
                            ChangeScene(SceneIDs.PlayerDoesNothing2);
                            break;
                        case SceneIDs.PlayerDoesNothing2:
                            companion.SaySomething("*Uh...*");
                            ChangeScene(SceneIDs.WondersIfPlayerWillAttack);
                            break;
                        case SceneIDs.WondersIfPlayerWillAttack:
                            companion.SaySomething("*Is... It going to try attacking me?*");
                            ChangeScene(SceneIDs.WondersIfScaredPlayer);
                            break;
                        case SceneIDs.WondersIfScaredPlayer:
                            companion.SaySomething("*Maybe... I scared it?*");
                            ChangeScene(SceneIDs.ThreatensPlayer);
                            break;
                        case SceneIDs.ThreatensPlayer:
                            companion.SaySomething("*Yeah!! Don't you dare get near me, or I'll show you something!*");
                            ChangeScene(SceneIDs.RealizesHowStupidWhatHeSaidWas);
                            break;
                        case SceneIDs.RealizesHowStupidWhatHeSaidWas:
                            companion.SaySomething("*(Show It something?! What was I thinking?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReacts, true);
                            break;
                        case SceneIDs.SeesIfPlayerReacts:
                            companion.SaySomething("...");
                            ChangeScene(SceneIDs.WonderIfAngeredPlayer);
                            break;
                        case SceneIDs.WonderIfAngeredPlayer:
                            companion.SaySomething("*(Did I anger it?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReactsAgain, true);
                            break;
                        case SceneIDs.SeesIfPlayerReactsAgain:
                            companion.SaySomething("...");
                            ChangeScene(SceneIDs.WonderIfPlayerIsFrozenInFear);
                            break;
                        case SceneIDs.WonderIfPlayerIsFrozenInFear:
                            companion.SaySomething("*(Maybe It's frozen in fear?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReactsAgainAgain, true);
                            break;
                        case SceneIDs.SeesIfPlayerReactsAgainAgain:
                            companion.SaySomething("...");
                            ChangeScene(SceneIDs.AttemptsToRunAway1);
                            break;
                        case SceneIDs.AttemptsToRunAway1:
                            companion.SaySomething("*I... Kind of have more things to do... so...*");
                            ChangeScene(SceneIDs.AttemptsToRunAway2);
                            break;
                        case SceneIDs.AttemptsToRunAway2:
                            companion.SaySomething("*Well, I'll be going then!*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                            //////////////////////////////
                        case SceneIDs.FearsAboutPlayerAttackingHim:
                            companion.SaySomething("*Yaah!!! Please, Don't kill me...*");
                            ChangeScene(SceneIDs.Crying1);
                            break;
                        case SceneIDs.Crying1:
                            companion.SaySomething("*(Sniffle... Sob...)*");
                            ChangeScene(SceneIDs.Crying2);
                            break;
                        case SceneIDs.Crying2:
                            companion.SaySomething("*(Snif...)*");
                            ChangeScene(SceneIDs.WaitingForReaction, true);
                            break;
                        case SceneIDs.WaitingForReaction:
                            companion.SaySomething("...");
                            ChangeScene(SceneIDs.AsksIfPlayerIsGoingToAttackHim);
                            break;
                        case SceneIDs.AsksIfPlayerIsGoingToAttackHim:
                            companion.SaySomething("*You... Aren't going to attack me... Right...?*");
                            ChangeScene(SceneIDs.AsksIfPlayerUnderstandWhatHeSays);
                            break;
                        case SceneIDs.AsksIfPlayerUnderstandWhatHeSays:
                            companion.SaySomething("*Can... You even understand me...?*");
                            ChangeScene(SceneIDs.WaitingForReactionAgain, true);
                            break;
                        case SceneIDs.WaitingForReactionAgain:
                            companion.SaySomething("*...*");
                            ChangeScene(SceneIDs.TellsThatIsgoingToFlee);
                            break;
                        case SceneIDs.TellsThatIsgoingToFlee:
                            companion.SaySomething("*I'm.. Going to... Walk... Back away... *");
                            ChangeScene(SceneIDs.RunsWhileScreaming);
                            break;
                        case SceneIDs.RunsWhileScreaming:
                            companion.SaySomething("*Waaaaaaaaaahhh!!!*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                        /////////////////////////////////
                        case SceneIDs.NoticesOtherGuardians:
                            companion.SaySomething("*Wait, are those... TerraGuardians?*");
                            ChangeScene(SceneIDs.WondersWhyGuardiansFollowsPlayer);
                            break;
                        case SceneIDs.WondersWhyGuardiansFollowsPlayer:
                            companion.SaySomething("*Why are those TerraGuardians with that Terrarian?*");
                            ChangeScene(SceneIDs.ThinksPlayerIsGuardiansPet);
                            break;
                        case SceneIDs.ThinksPlayerIsGuardiansPet:
                            companion.SaySomething("*Maybe It's their little pet?*");
                            ChangeScene(SceneIDs.IgnoresTheAboveIdea);
                            break;
                        case SceneIDs.IgnoresTheAboveIdea:
                            companion.SaySomething("*It... Doesn't look like it...*");
                            ChangeScene(SceneIDs.ThinksPlayerEnslavedGuardians);
                            break;
                        case SceneIDs.ThinksPlayerEnslavedGuardians:
                            companion.SaySomething("*Maybe... Oh no... The Terrarian enslaved them!*");
                            ChangeScene(SceneIDs.YellsThatIsGoingToSaveGuardians);
                            break;
                        case SceneIDs.YellsThatIsGoingToSaveGuardians:
                            companion.SaySomething("*Don't worry friends, I will save you all!*");
                            ChangeScene(SceneIDs.WondersHowToSaveGuardians);
                            break;
                        case SceneIDs.WondersHowToSaveGuardians:
                            companion.SaySomething("*I should save them, but how...*");
                            ChangeScene(SceneIDs.PlayerMainGuardianTalksToLeopold);
                            break;
                        case SceneIDs.PlayerMainGuardianTalksToLeopold:
                            {
                                Companion Guardian = PlayerMod.GetPlayerMainGuardian(SpottedPlayer);
                                if (Guardian == null) //It's dumb, but is better than a crash.
                                {
                                    companion.SaySomething("*Ahhh!! Why did you do that?!*");
                                    ChangeScene(SceneIDs.Flee);
                                    return;
                                }
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID) //What about moving those dialogues to a separate method, so It's easier to find them.
                                    {
                                        default:
                                            Message = Guardian.GetOtherMessage(MessageIDs.LeopoldMessage1, "*"+Guardian.GetNameColored()+" is saying that you're It's friend.*");
                                            break;
                                        case CompanionDB.Rococo:
                                            Message = "*"+Guardian.GetNameColored()+" looks very confused.*";
                                            break;
                                        case CompanionDB.Blue:
                                            Message = "*Hahaha. Sorry, that's so funny.*";
                                            break;
                                        case CompanionDB.Sardine:
                                            Message = "I think the way we met isn't that weird now, right pal?";
                                            break;
                                        case CompanionDB.Zacks:
                                            Message = "*Boss, can I eat that stupid bunny?*";
                                            break;
                                        case CompanionDB.Alex:
                                            Message = "Save me? Save me from what? Who's threatening me and my friend?!";
                                            break;
                                        case CompanionDB.Brutus:
                                            Message = "*Are you missing some rivets, long-eared guy?*";
                                            break;
                                        case CompanionDB.Bree:
                                            Message = "Have you finished making yourself into a fool.";
                                            break;
                                        case CompanionDB.Mabel:
                                            Message = "*Hello, Teehee. Do you have a problem, bunny guy?*";
                                            break;
                                        case CompanionDB.Domino:
                                            Message = "*Don't look at me, I stopped selling the kind of merchandise that caused that long ago.*";
                                            break;
                                        case CompanionDB.Vladimir:
                                            Message = "*I think that guy needs a hug. Maybe It will end up fixing his head, I guess.*";
                                            break;
                                        case CompanionDB.Malisha:
                                            Message = "*And he still fears even his own shadow.*";
                                            break;
                                    }
                                }
                                Guardian.SaySomething(Message);
                            }
                            ChangeScene(SceneIDs.LeopoldAnswersTheGuardian);
                            break;
                        case SceneIDs.LeopoldAnswersTheGuardian:
                            {
                                Companion Guardian = PlayerMod.GetPlayerMainGuardian(SpottedPlayer);
                                if (Guardian == null)
                                {
                                    companion.SaySomething("*Ahhh!! Why did you do that?!*");
                                    ChangeScene(SceneIDs.Flee);
                                    return;
                                }
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID)
                                    {
                                        default:
                                            Message = Guardian.GetOtherMessage(MessageIDs.LeopoldMessage2, "*You're friends of that Terrarian?*");
                                            break;
                                        case CompanionDB.Rococo:
                                            Message = "*Uh... What is it with the look on your face?*";
                                            break;
                                        case CompanionDB.Blue:
                                            Message = "*Wait, why are you laughing?*";
                                            break;
                                        case CompanionDB.Sardine:
                                            Message = "*Wait, \"pal\"? You're that Terrarian's friend?!*";
                                            break;
                                        case CompanionDB.Zacks:
                                            Message = "*Wait, you...Yaaaaaah!! It's a zombie!!!*";
                                            break;
                                        case CompanionDB.Alex:
                                            Message = "*F..Friend?!*";
                                            break;
                                        case CompanionDB.Brutus:
                                            Message = "*I...I'm not crazy?! What are you doing with that Terrarian?*";
                                            break;
                                        case CompanionDB.Bree:
                                            Message = "*Hey! I'm not a fool!*";
                                            break;
                                        case CompanionDB.Mabel:
                                            Message = "*Ah... Uh... No... Uh... Just... I'm... Fine...*";
                                            break;
                                        case CompanionDB.Domino:
                                            Message = "*H-hey! I would never use such a thing!*";
                                            break;
                                        case CompanionDB.Vladimir:
                                            Message = "*How can you think of hugs at a moment like this?*";
                                            break;
                                        case CompanionDB.Malisha:
                                            Message = "*W-what the! What are you doing here? And who's that?*";
                                            break;
                                    }
                                }
                                companion.SaySomething(Message);
                            }
                            ChangeScene(SceneIDs.MainGuardianSaysThatPlayerHasBeenHearingAllTheTime, true);
                            break;
                        case SceneIDs.MainGuardianSaysThatPlayerHasBeenHearingAllTheTime:
                            {
                                Companion Guardian = PlayerMod.GetPlayerMainGuardian(SpottedPlayer);
                                if (Guardian == null)
                                {
                                    companion.SaySomething("*Ahhh!! Why did you do that?!*");
                                    ChangeScene(SceneIDs.Flee);
                                    return;
                                }
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID)
                                    {
                                        default:
                                            Message = Guardian.GetOtherMessage(MessageIDs.LeopoldMessage3, "*" + Guardian.GetNameColored() + " also said that you heard everything he said.*");
                                            break;
                                        case CompanionDB.Rococo:
                                            Message = "*" + Guardian.GetNameColored() + " is asking you what is his problem.*";
                                            break;
                                        case CompanionDB.Blue:
                                            Message = "*Haha.. We're not in need of rescue. That Terrarian is simply our friend, and they could hear everything you said.*";
                                            break;
                                        case CompanionDB.Sardine:
                                            Message = "Yes, and It heard everything you said as clear as day.";
                                            break;
                                        case CompanionDB.Zacks:
                                            Message = "*That guy is making me sick, my boss isn't a troglodyte, do you hear?*";
                                            break;
                                        case CompanionDB.Alex:
                                            Message = "Yes, me and my friend here were watching you talking to yourself all that time.";
                                            break;
                                        case CompanionDB.Brutus:
                                            Message = "*I was hired by it to be their bodyguard, you fool.*";
                                            break;
                                        case CompanionDB.Bree:
                                            Message = "Of course you are, how can you think that the Terrarian is a fool?";
                                            break;
                                        case CompanionDB.Mabel:
                                            Message = "*Hey friend, I can't understand that guy, can you explain to me what his problem is?*";
                                            break;
                                        case CompanionDB.Domino:
                                            Message = "*Then why were you fooling yourself a while ago? Terrarians aren't stupid.*";
                                            break;
                                        case CompanionDB.Vladimir:
                                            Message = "*What moment like this, the Terrarian is my buddy. And can understand what we are talking about.*";
                                            break;
                                        case CompanionDB.Malisha:
                                            Message = "*I moved to here for a vacation, then this Terrarian let me live here.*";
                                            break;
                                    }
                                }
                                Guardian.SaySomething(Message);
                            }
                            ChangeScene(SceneIDs.LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime);
                            break;
                        case SceneIDs.LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime:
                            companion.SaySomething("*What?! That Terrarian can understand what we are saying?!*");
                            ChangeScene(SceneIDs.LeopoldTellsToForgetEverything);
                            break;
                        case SceneIDs.LeopoldTellsToForgetEverything:
                            companion.SaySomething("*I nearly thought... Oh... Nevermind... Does It matter anyway?*");
                            ChangeScene(SceneIDs.LeopoldPresentsHimself);
                            break;
                        case SceneIDs.LeopoldPresentsHimself:
                            companion.SaySomething("*I'm Leopold, the Sage. Please disconsider what I debated with myself earlier. If you can.*");
                            ChangeScene(SceneIDs.LeopoldFreeForRecruit);
                            break;

                        case SceneIDs.KnownPlayerSpottedByLeopold:
                            companion.SaySomething("*Wait... I know that face...*");
                            ChangeScene(SceneIDs.LeopoldRecognizesTerrarian);
                            break;
                        case SceneIDs.LeopoldRecognizesTerrarian:
                            companion.SaySomething("*Oh, It is that Terrarian again.*");
                            ChangeScene(SceneIDs.LeopoldGreetsPlayer);
                            break;
                        case SceneIDs.LeopoldGreetsPlayer:
                            companion.SaySomething("*Hello, I didn't expected to see you here.*");
                            ChangeScene(SceneIDs.LeopoldTellsThatIsGoingToPlayerTown);
                            break;
                        case SceneIDs.LeopoldTellsThatIsGoingToPlayerTown:
                            companion.SaySomething("*Since you're here, I think you have some town around this world, so... See you there.*");
                            ChangeScene(SceneIDs.LeopoldTurnsToTownNPC);
                            break;
                        case SceneIDs.LeopoldTurnsToTownNPC:
                            WorldMod.AddCompanionMet(CompanionDB.Leopold);
                            PlayerMod.PlayerAddCompanion(SpottedPlayer, companion);
                            companion.IncreaseFriendshipPoint(1);
                            WorldMod.SetCompanionTownNpc(companion);
                            break;

                            //////////////////////////////////
                        case SceneIDs.Flee:
                            {
                            }
                            break;
                    }
                }
                if (SceneID == SceneIDs.Flee)
                {
                    companion.WalkMode = false;
                    if (PlayerDistance >= Main.screenWidth)
                    {
                        WorldMod.RemoveCompanionNPC(companion);
                        Main.NewText("The Bunny Guardian has escaped.", Microsoft.Xna.Framework.Color.Red);
                        Companion c = PlayerMod.GetPlayerLeaderCompanion(SpottedPlayer);
                        if (c != null)
                        {
                            string Message = c.GetOtherMessage(MessageIDs.LeopoldEscapedMessage);
                            if (Message != "")
                            {
                                c.SaySomething(Message);
                            }
                        }
                        return;
                    }
                    else if (SpottedPlayer.Center.X < companion.Center.X)
                    {
                        companion.MoveRight = true;
                    }
                    else
                    {
                        companion.MoveLeft = true;
                    }
                }
                SceneTime--;
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (companion.velocity.X != 0 || companion.velocity.Y != 0)
                return;
            switch(SceneID)
            {
                case SceneIDs.Crying1:
                case SceneIDs.Crying2:
                    {
                        const short FrameId = 27;
                        companion.BodyFrameID = FrameId;
                        for(int i = 0; i < companion.ArmFramesID.Length; i++)
                        {
                            companion.ArmFramesID[i] = FrameId;
                        }
                    }
                    return;
                case SceneIDs.WaitingForReaction:
                    {
                        const short FrameId = 28;
                        companion.BodyFrameID = FrameId;
                        for(int i = 0; i < companion.ArmFramesID.Length; i++)
                        {
                            companion.ArmFramesID[i] = FrameId;
                        }
                    }
                    return;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return EncounterAct || SocialAct || FearAct || ScareAct || SceneID == SceneIDs.LeopoldFreeForRecruit;
        }

        private MessageBase GetChatMessageBase()
        {
            WorldMod.AddCompanionMet(CompanionDB.Leopold);
            MessageDialogue md = new MessageDialogue();
            if (SceneID == SceneIDs.LeopoldFreeForRecruit)
            {
                md.ChangeMessage("*Why did you make me act like a fool in front of your TerraGuardians? That will make it harder for people to remember me as a wise sage.*");
            }
            else if (SocialAct)
            {
                md.ChangeMessage("*You can talk?! Wait, why didn't you talk to me sooner then? I nearly thought you were... No... Nevermind... I'm Leopold, the Sage.*");
            }
            else if (FearAct)
            {
                md.ChangeMessage("*Huh? You're friendly? And can talk?! Oh... Thank... No... I'm fine.. It's just... Uh... Pleased to meet you, by the way... I'm Leopold, the Sage.*");
            }
            else if (ScareAct)
            {
                md.ChangeMessage("*What?! You can talk!? Why did you made me pass through all that you idiot! Ugh... I'm Leopold, the Sage, by the way. Do you... Do you have some spare leaves with you...?*");
            }
            else
            {
                md.ChangeMessage("*You can understand what I say?! Wow! The book was right!!*");
            }
            md.AddOption("Hello.", Dialogue.LobbyDialogue);
            return md;
        }

        private void ChangeScene(SceneIDs scene, bool Extended = false)
        {
            SceneID = scene;
            SceneTime = Extended ? 600 : 180;
        }

        public enum SceneIDs
        {
            NoScene = -1,
            LeopoldSpotsThePlayer,
            LeopoldSaysNeverSawAnythingLikeThat,
            LeopoldQuestionsHimselfAboutCreature,
            IsItTerrarian,
            NoticesOutfit,
            QuestionsIfIsReallyATerrarian,
            NoticesPlayerLooking,
            WondersPlayerReaction,
            IsPreparingAttack,
            ThreatenUseSpell,
            FindsWeirdTheNoReaction,

            ThinksAboutTryingToTalk,
            WondersIftheySpeak,
            MentionsABook,
            ThinksAboutTrying,
            TriesTalking,
            WondersIfIsScared,
            SaysWontHurt,
            TriesHidingFear,
            NoticesDidntWorked,
            TriesGivingFood,
            WondersHowStupidHisActionWas,
            WaitsAFewMoments,
            ThinksThePlayerDidntUnderstandHim,
            TalksAboutWalkingAway,

            //Fear Branch
            GotScaredUponPlayerApproach,
            WondersIfPlayerWillAttackHim,
            FearPlayerAttack1,
            FearPlayerAttack2,
            FearPlayerAttack3,
            FearPlayerAttack4,
            FearPlayerAttack5,
            PlayerDoesNothing,
            PlayerDoesNothing2,
            WondersIfPlayerWillAttack,
            WondersIfScaredPlayer,
            ThreatensPlayer,
            RealizesHowStupidWhatHeSaidWas,
            SeesIfPlayerReacts,
            WonderIfAngeredPlayer,
            SeesIfPlayerReactsAgain,
            WonderIfPlayerIsFrozenInFear,
            SeesIfPlayerReactsAgainAgain,
            AttemptsToRunAway1,
            AttemptsToRunAway2,

            //Scare Branch
            FearsAboutPlayerAttackingHim,
            Crying1,
            Crying2,
            WaitingForReaction,
            AsksIfPlayerIsGoingToAttackHim,
            AsksIfPlayerUnderstandWhatHeSays,
            WaitingForReactionAgain,
            TellsThatIsgoingToFlee,
            RunsWhileScreaming,

            NoticesOtherGuardians,
            WondersWhyGuardiansFollowsPlayer,
            ThinksPlayerIsGuardiansPet,
            IgnoresTheAboveIdea,
            ThinksPlayerEnslavedGuardians,
            YellsThatIsGoingToSaveGuardians,
            WondersHowToSaveGuardians,
            PlayerMainGuardianTalksToLeopold,
            LeopoldAnswersTheGuardian,
            MainGuardianSaysThatPlayerHasBeenHearingAllTheTime,
            LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime,
            LeopoldTellsToForgetEverything,
            LeopoldPresentsHimself,
            LeopoldFreeForRecruit,

            KnownPlayerSpottedByLeopold,
            LeopoldRecognizesTerrarian,
            LeopoldGreetsPlayer,
            LeopoldTellsThatIsGoingToPlayerTown,
            LeopoldTurnsToTownNPC,

            Flee
        }
    }
}
