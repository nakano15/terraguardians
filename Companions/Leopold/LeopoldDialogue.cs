using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LeopoldDialogues : CompanionDialogueContainer
    {
        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*I've read so many books about adventures. You're an adventurer, right? I'd like to see with my own eyes how is it.*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*Can you hop? You can move faster that way. Wait, you can't? Then sit on my shoulder and see how I do that.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*I'd like to have my own adventure, but I don't really have the initiative to do so, what if you help me?*";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*Do you have a moment to listen to me, [nickname]? After all that time we spent talking with each other, I think you might be perfect for helping me with my Buddy research. If you're looking to pick someone as your Buddy, you can count on me.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*You are aware of how much of an honor it is for me to be picked as a Buddy, right? That means I will not question many things you ask of me, including control. Does that interest you?*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GreetMessages(Companion companion)
        {
            bool AnyTgFollower = false;
            foreach (Companion c in PlayerMod.PlayerGetSummonedCompanions(MainMod.GetLocalPlayer))
            {
                if (c != null && c.GetGroup.IsTerraGuardian)
                {
                    AnyTgFollower = true;
                    break;
                }
            }
            List<string> Mes = new List<string>();
            Mes.Add("*Huh? Ah!! Oh... You're friendly. Sorry, I came here because I noticed several TerraGuardians were coming here.*");
            Mes.Add("*Are you... A Terrarian...? I read about your kind in the books in my house. "+(AnyTgFollower ? "Hey, those are TerraGuardians! What are you doing here?" : "Say, have you seen any TerraGuardians around?") +"*");
            Mes.Add("*Yikes!! You scared me! I've never seen a Terrarian before. Wait, you can hear me? Then It's true... Oh... I'm thinking out loud again. Do you have someplace I can stay?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            /*if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
            {
                Mes.Add("*I can try changing the forms of the pig emotions between Cloud and Solid, but I will need all emotions to merge them together.*");
                Mes.Add("*There is not exactly a penalty for them being in cloud form, but they may not like It.*");
            }*/
            if (companion.GetGoverningBehavior() is Leopold.HeldByBlueBehavior)
            {
                Mes.Add("*Help! She wont let me go!*");
                Mes.Add("*[gn:"+CompanionDB.Blue+"]! I need to pee!!*");
                Mes.Add("*Can you convince [gn:"+CompanionDB.Blue+"] to let me go?*");
            }
            Mes.Add("*So, this is where the TerraGuardians have been moving to? I can see why.*");
            Mes.Add("*How many times do I have to say that my tail is not made of cotton! Stop trying to touch it!*");
            Mes.Add("*I spend most of my time reading books, so I know many things.*");
            if (!player.Male)
                Mes.Add("*Hey! Get your hand off my tail! It's not made of cotton!*");
            if (!NPC.downedBoss3)
                Mes.Add("*The dungeon in your world contains a great amount of readable material I could make use of. But the Old Man won't allow me in.*");
            else
            {
                Mes.Add("*So much knowledge found inside the dungeon. From dark secrets to sci-fi literature.*");
            }
            if (companion.FriendshipLevel < 2)
                Mes.Add("*Don't call me bunny, I don't know you yet.*");
            if (CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*What did I have on my mind during the popularity contest, when I agreed with Bree when she was clearly wrong?*");
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*Too bright! I think I stayed inside home for too long.*");
                }
                else
                {
                    Mes.Add("*Want some popcorn?*");
                    Mes.Add("*I think I read about them in a book. Had something to do with the creators.*");
                }
            }
            else
            {
                Mes.Add("*What? It was day a while ago!*");
                if (Main.bloodMoon)
                {
                    Mes.Add("*Interesting, so whenever the red moon rises, monsters gets aggressive and smarter? And what is wrong with the female citizens?*");
                    Mes.Add("*I'm not scared! And no, the screams didn't come from here!*");
                }
                else
                {
                    Mes.Add("*This seems like a good night for some reading.*");
                    Mes.Add("*I tried staring at the moon, but then I got bored. I prefer my books.*");
                }
            }
            if (NPC.TowerActiveNebula || NPC.TowerActiveSolar || NPC.TowerActiveStardust || NPC.TowerActiveVortex)
            {
                Mes.Add("*I feel... Like something bad is about to happen...*");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*This place is being too noisy, I can't concentrate on my books.*");
                Mes.Add("*Could you guys PLEASE SHUT UP!! Oh! I didn't see you there. Need something?*");
                Mes.Add("*Ugh... The headache comes... Couldn't they party somewhere else?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*I keep avoiding crossing [gn:" + CompanionDB.Blue + "]'s path, because every time she sees me, I spend the next 1~2 hours trying to get off her arms.*");
                Mes.Add("*What is [gn:" + CompanionDB.Blue + "]'s problem? Whenever she sees a bunny she wants to hug it.*");
                Mes.Add("*You won't believe me, but [gn:" + CompanionDB.Blue + "] has really strong arms. I have to struggle for hours to get off them.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Zacks))
            {
                Mes.Add("*I try my best to avoid crossing [gn:" + CompanionDB.Zacks + "]'s path, for my own safety.*");
                Mes.Add("*Creepy! [gn:" + CompanionDB.Zacks + "]'s mouth starts salivating when he sees me!*");
                if (HasCompanionSummoned(CompanionDB.Brutus))
                {
                    Mes.Add("*Would you mind If I borrow [gn:"+CompanionDB.Brutus+"] for a while? I'm still not sure If I'm safe with [gn:"+CompanionDB.Zacks+"] around.*");
                }
            }
            if (HasCompanionSummoned(CompanionDB.Blue))
            {
                Mes.Add("*Uh!? No way! Don't hug me again!*");
                Mes.Add("*You again?! Keep your arms off me!* (He exclaimed after seeing [gn:"+CompanionDB.Blue+"].)");
            }
            if (HasCompanionSummoned(CompanionDB.Zacks))
            {
                Mes.Add("Great, you came, I wanted to talk to you about some reseaAAAAAAAAHHHHH Zombie!! \n....\nUh... Do you have some more leaves..?");
            }
            if (HasCompanionSummoned(CompanionDB.Mabel))
            {
                Mes.Add("*A...Aa.... Uh.... Could... You please... Go away... With.. Her...*");
                Mes.Add("*Ah... Uh... I.... Have to... Go... To the toilet... Yes. The Toilet...*");
            }
            else if (CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*I get reactionless when [gn:" + CompanionDB.Mabel + "] is nearby.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*D-d-did y-you l-let a g-g-ghost l-live here? A-are you out of your mind?*");
                Mes.Add("*[nickname], one of the things that mostly scares me are ghosts. Why did you let one live here?*");
                Mes.Add("*I think [gn:" + CompanionDB.Fluffles + "] knows I'm scared of her. She always catches me off guard to spook me out.*");
                Mes.Add("*I look distracted? I'm checking out if [gn:" + CompanionDB.Fluffles + "] wont surge from somewhere to give me a scare.*");
                //Mes.Add("*You need to speak with [gn:" + CompanionDB.Fluffles + "]. The other day she made me faint out of a scare, when I woke up she was over me. I've never been so scared in my life!*"); //That would give a bad impression of what happened.
            }
            if (CanTalkAboutCompanion(CompanionDB.Luna))
            {
                Mes.Add("*Ah... It's good to have [gn:"+CompanionDB.Luna+"] around. She lessens the influx of questions upon me.*");
            }
            if (!Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("*Looks like I'll have troubles sleeping this night...*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*I discovered a way of dealing with [gn:" + CompanionDB.Michelle + "], I just need to talk about my research.*");
                Mes.Add("*That girl [gn:"+CompanionDB.Michelle+"] surely is curious. I wonder, what's wrong with her?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*Of everyone you could have let move into this world, you had to let [gn:"+CompanionDB.Malisha+"] live here? She's a menace to us all!*");
                Mes.Add("*During my life of mentoring, I never regretted teaching someone, except for [gn:"+CompanionDB.Malisha+"]. She's not careless, or cares about the wellbeing of the people she does experiments with.*");
                Mes.Add("*Sometimes I think [gn:" + CompanionDB.Malisha + "] is trying to eat me. If I suddenly disappear, try looking inside her mouth.*");
                Mes.Add("*Huh? Sorry, I'm trying to make my house \"[gn:"+CompanionDB.Malisha+"] proof\".*");
                Mes.Add("*My greatest misfortunes in life begins when [gn:"+CompanionDB.Malisha+"] says that has a new experiment to test.*");
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("*I wouldn't be surprised if what happened to [gn:"+CompanionDB.Zacks+"] wasn't [gn:"+CompanionDB.Malisha+"]s doing.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*No no no! As I said to [gn:"+CompanionDB.Miguel+"], I'd rather do anything else than boring exercises. And tell him to stop sending people to persuade me.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*I had to help [gn:" + CompanionDB.Green + "] by borrowing some terrarian anatomy books to him.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cille))
            {
                Mes.Add("*[gn:" + CompanionDB.Cille + "] seems to avoid contact with anyone. I wonder, why?*");
                Mes.Add("*I think I saw [gn:" + CompanionDB.Cille + "] around my house during a New Moon Night. Well, at least I think It was [gn:"+CompanionDB.Cille+"] since I only saw a shadow like her.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leona))
            {
                Mes.Add("*How is [gn:"+CompanionDB.Leona+"] able to hold that big sword for so long? My arms can hardly hold a large books for 10 seconds! Wait. What if I ask her to carry some books for me? I'm a genius!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Scaleforth))
            {
                Mes.Add("*You better be careful when near [gn:"+CompanionDB.Scaleforth+"]. I think he contracted rabbies, since he keeps drooling when I look at him.*");
            }
            if (companion.IsUsingToilet)
            {
                Mes.Add("*What's with you Terrarians? Don't you know that this is a moment of privacy?*");
                if (!player.Male)
                    Mes.Add("*No! Don't try touching my tail now!*");
                Mes.Add("*You're interrupting my reflection moment! Go away!*");
            }
            if (IsPlayerRoomMate())
            {
                Mes.Add("*Yes, I don't mind sharing my room with you, just place your bed somewhere.*");
                Mes.Add("*I hope you don't snore at night, because I need sleep to process my researches.*");
                Mes.Add("*You're not planning on throwing parties every night, right? I've had enough of those during the Magic University.*");
            }
            if (IsPlayerRoomMate(new CompanionID(CompanionDB.Malisha)))
            {
                Mes.Add("*You must either be courageous, or very stupid, for sharing your room with [gn:"+CompanionDB.Malisha+"]. Who knows what she does to you while you sleep?*");
                Mes.Add("*Stop sharing room with [gn:"+CompanionDB.Malisha+"], you may wake up tied in a bed while she does wacky experiments on you.*");
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Yaaaaaaaaaaaah- Oh no, now I gotta wipe myself.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(He's theorizing to himself, while sleeping.)");
            Mes.Add("(Is he saying elements of the periodic table?)");
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*No! Go away! Don't come closer! No! NOOOOOOOOOO!!!! (I think [gn:" + CompanionDB.Malisha + "] appeared on his dream)*");
                Mes.Add("*Wait! What is in that flask! No! I wont drink it! NO! NOOOO!! (I think [gn:" + CompanionDB.Malisha + "] appeared on his dream)*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*This place doesn't have enough spoony bards.*");
            Mes.Add("*I see far away places blurry, I think I read too much.*");
            Mes.Add("*Why do people keep blaming me when they see several bunnies around?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextBool(2))
                        return "*What? I don't need anything.*";
                    return "*Oh no, I don't need your help right now.*";
                case RequestContext.HasRequest:
                    if (Main.rand.NextBool(2))
                        return "*I'm filled with things to research, so I barely have the time to [objective]. Can you help me with it?*";
                    return "*There is something that needs to be done for one of my research projects, which is [objective]. Would you please do it?*";
                case RequestContext.Accepted:
                    return "*Great. Now, where was I... Oh... Yeah.*";
                case RequestContext.TooManyRequests:
                    return "*I'm used to multi-tasking, I don't think you are though.*";
                case RequestContext.Rejected:
                    return "*Maybe I need to do that myself, then.*";
                case RequestContext.PostponeRequest:
                    return "*Huh? Oh.. Fine.*";
                case RequestContext.Failed:
                    return "*I'll... Try adding that to my research notes. Thanks for the cooperation.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Please, tell me you completed my request.*";
                case RequestContext.RemindObjective:
                    return "*This is what I asked you to do: [objective].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You can't drop my request, the future of the Terra Realms research is in your hands. Are you really going to cancel my request?*";
                case RequestContext.CancelRequestYes:
                    return "*Sigh.. Okay. But be sure not to drop my requests in the future.*";
                case RequestContext.CancelRequestNo:
                    return "*I knew I could count on you, [nickname].*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*Stay here? Sure. That would help me with conducting research on this world.*";
                case MoveInContext.Fail:
                    return "*I'd rather not think about moving in right now.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I have research to attend, and I will be able to do them better at my tower.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You're kicking me out? Fine. I'll go back to my tower then.*";
                case MoveOutContext.Fail:
                    return "*I have research to finish, so I can't leave right now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*No. This house was given to me by someone else. You can't kick me out.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch (context)
            {
                case JoinMessageContext.Success:
                    return "*It seems like a great idea, I wonder how many new things I may end up meeting during your trip.*";
                case JoinMessageContext.Fail:
                    return "*I still have some books to read.*";
                case JoinMessageContext.FullParty:
                    return "*There are too many people around you.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Okay. I have to research in my books about the creatures we found on the way.*";
                case LeaveMessageContext.Fail:
                    return "*I'm not leaving your group right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*Are you crazy?! There are very dangerous creatures on the way back home!*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*You are kidding, right? You aren't?! Looks like I'll have to f-fight my way back home then...*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Whew... You got me worried for a while.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Sure. Hop on my shoulder. Be careful not to fall, and don't pull my ears.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*Thank you. I accept the ride.*";
                case MountCompanionContext.Fail:
                    return "*Not a good moment for that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*Better use your own feet instead.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I can carry them. No worries.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*Yes, I can, but who?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch (context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*Fine, I'll place you on the ground, just don't pull my eaAAAAAHHH!! That hurt!*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*I'll be back to hopping then.*";
                case DismountCompanionContext.Fail:
                    return "*Not the moment for that.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*M-Me?! Leading the group? O-okay..*";
                case ChangeLeaderContext.Failed:
                    return "*B-better not.*";
            }
            return base.ChangeLeaderMessage(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*You say you want to pick me as your buddy?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Hm... Fine. I can test if friendship is really magic.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*What was the point of asking me then?*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I don't know you yet, so I refuse.*";
                case BuddiesModeContext.Failed:
                    return "*I can't think about that right now.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*Huh? I thought you had one already. I can even see the bonding line between you and your Buddy.*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*My spells are yours to command, as long as you don't get us killed.*";
                case ControlContext.SuccessReleaseControl:
                    if (Main.rand.NextBool(2))
                        return "*That experience will be good information for my research.*";
                    return "*So good to have control over myself again.*";
                case ControlContext.FailTakeControl:
                    return "*I can't be doing that now!*";
                case ControlContext.FailReleaseControl:
                    return "*Sorry. I can't do that now.*";
                case ControlContext.NotFriendsEnough:
                    return "*What? No!*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Why are my senses tingling? It's like we're in danger.*";
                        case 1:
                            return "*Go on with your business, I can only observe to then take notes.*";
                        case 2:
                            return "*It feels so odd to be like someone else's consciousness. Oh, I should write that in the book.*";
                    }
                case ControlContext.GiveCompanionControl:
                    return "*Wha- Oh. Well, at least I can experience having control during a Bond-Merge.*";
                case ControlContext.TakeCompanionControl:
                    return "*The experience was amazing while it lasted.*";
            }
            return base.ControlMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*If it's quick, I don't mind. I have research to attend to.*";
                case InteractionMessageContext.Accepts:
                    return "*Doing it.*";
                case InteractionMessageContext.Rejects:
                    return "*I refuse.*";
                case InteractionMessageContext.Nevermind:
                    return "*Changed your mind? Fine.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*I just finished writing my research. I'll be there soon.*";
                case InviteContext.SuccessNotInTime:
                    return "*I'm still flooded with notes to write, and need to have a nap too. Tomorrow I will be showing up.*";
                case InviteContext.Failed:
                    return "*I have research to attend.*";
                case InviteContext.CancelInvite:
                    return "*Don't want me there anymore? Fine, I'm returning home.*";
                case InviteContext.ArrivalMessage:
                    return "*I'm here, [nickname].*";
            }
            return base.InviteMessages(companion, context);
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == CompanionDB.Malisha)
                {
                    Weight = 1.5f;
                    return "*You're going to stay here?! No! No! NO!!!*";
                }
            }
            Weight = 1f;
            return "*Interesting having someone new here.*";
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Green:
                        Weight = 1.5f;
                        return "*A medic. Perfect! We will need one.*";
                    case CompanionDB.Malisha:
                        Weight = 1.5f;
                        return "*Oh no, this expedition will be a trip in hell...*";
                    case CompanionDB.Fluffles:
                        Weight = 1.5f;
                        return "*Yikes! She has to come too?!*";
                    case CompanionDB.Brutus:
                        Weight = 1.5f;
                        return "*Can he defend me, instead? I'm alergic to pain.*";
                    case CompanionDB.Miguel:
                        Weight = 1.2f;
                        return "*Is that some way you're telling me to do exercises?*";
                }
            }
            Weight = 1f;
            return "*Great, another person for the expedition.*";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Is this the time for that? Fine...*";
                case MessageIDs.RPSAskToPlaySuccess:
                    return "*What?! I was expecting something else that would make use of my intellect! Oh well... Lets play that then..*";
                case MessageIDs.RPSAskToPlayFail:
                    return "*Not right now.*";
                case MessageIDs.RPSCompanionWinMessage:
                    return "*You didn't counted on my cleverness.*";
                case MessageIDs.RPSCompanionLoseMessage:
                    return "*What? How could you outsmart me?!*";
                case MessageIDs.RPSCompanionTieMessage:
                    return "*This was just a coincidence. I assure you.*";
                case MessageIDs.RPSPlayAgainMessage:
                    return "*Another one? Yes, I wasn't very happy with previous result anyways.*";
                case MessageIDs.RPSEndGameMessage:
                    return "*That's it then? Alright.*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I can share my bed, but... That's odd.*";
            return "*Whew. I'll have a bed for myself then.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I don't see why, but yes, I can share a chair. But I question myself, how.*";
            return "*Good, because I was feeling odd about sharing it.*";
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch (context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Do you need to know about something? I might be able to help, depending on what it is.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there something else you want to talk about?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Anything else you need before we cease talking?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch (context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*What is wrong with my combat behavior? You have ideas on how I can improve it?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*ARE YOU CRAZY? ME? ATTACKING AT CLOSE RANGE?! I hope you gave me something to make that less dangerous.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Hm.. That actually seems fine for me.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Yes. I love the idea of staying away from monsters.*";
                case TacticsChangeContext.Nevermind:
                    return "*Oh. Right.*";
                case TacticsChangeContext.FollowAhead:
                    return "*W-what? O-okay!*";
                case TacticsChangeContext.FollowBehind:
                    return "*Yes! Yes!! I'll follow you! Not go in front of you!*";
                case TacticsChangeContext.AvoidCombat:
                    return "*I'm actually not against that. I think I can even dig a hole and hide inside if danger comes.*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*Oh, okay. I'll be using my magic again, then.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*I heard you, I'm coming. This is perfect for trying some healing spells.*";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("*I know some healing magic, this will help you.*");
                        Mes.Add("*I've read several medicine books. Don't worry, I know what I'm doing.*");
                        Mes.Add("*I hope I don't need to open you up to try and fix your problems.*");
                        bool IsPlayer = !(target is Companion);
                        if (!IsPlayer && (target as Companion).ModID == companion.ModID)
                        {
                            Companion ReviveTarget = target as Companion;
                            if (ReviveTarget.ID == CompanionDB.Blue)
                            {
                                Mes.Add("*I should help her carefully away, I don't want to be stuck in her arms for hours again.*");
                            }
                            if (ReviveTarget.ID == CompanionDB.Vladimir)
                            {
                                Mes.Add("*There is still a lot you can help me with.*");
                            }
                            if (ReviveTarget.ID == CompanionDB.Zacks)
                            {
                                Mes.Add("*Come on "+companion.GetNameColored()+"... Control your intestine. You need to help him, no time for... Leaf, please.*");
                            }
                            if (ReviveTarget.ID == CompanionDB.Mabel)
                            {
                                Mes.Add("*Oh no... My nose... Does someone have a leaf I could use?*");
                            }
                            if (ReviveTarget.ID == CompanionDB.Malisha)
                            {
                                Mes.Add("*Ugh... I really don't want to... But... I'll help...*");
                                Mes.Add("*I hope she stops tormenting me after this.*");
                            }
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*Oh no! I'm coming!!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Okay, now we need distance from danger.*";
                case ReviveContext.RevivedByItself:
                    return "*Ow, okay... I think I'm fine now.*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Ow ow ow... Be careful, I'm still a bit hurt...*";
                    return "*Thanks to you, my geniality still lives.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            if (companion.Owner == MainMod.GetLocalPlayer)
            {
                Companion Controlled = PlayerMod.PlayerGetControlledCompanion(MainMod.GetLocalPlayer);
                if (Controlled != null && Controlled.IsSameID(CompanionDB.Blue))
                {
                    BehaviorBase behavior = companion.GetGoverningBehavior();
                    if (behavior is Leopold.HeldByBlueBehavior)
                    {
                        dialogue.AddOption("Place [gn:"+CompanionDB.Leopold+"] on the ground.", BluePlaceLeopoldOnTheGround);
                    }
                    else
                    {
                        dialogue.AddOption("Carry [gn:"+CompanionDB.Leopold+"].", BlueCarryLeopold);
                    }
                }
            }
        }

        public override void ManageChatTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOption("I have questions.", OtherDialoguesLobby);
        }

        private void BluePlaceLeopoldOnTheGround()
        {
            BehaviorBase behavior = Dialogue.Speaker.GetGoverningBehavior();
            if (behavior is Leopold.HeldByBlueBehavior)
            {
                behavior.Deactivate();
                MessageDialogue md = new MessageDialogue("*Finally. It's over..*");
                md.RunDialogue();
            }
        }

        private void BlueCarryLeopold()
        {
            TerraGuardian Blue = (TerraGuardian)PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Blue);
            if (Blue != null)
            {
                Dialogue.Speaker.RunBehavior(new Leopold.HeldByBlueBehavior(Blue));
                MessageDialogue md = new MessageDialogue("*Ack! No!!*");
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue("*What? What are you talking about?*");
                md.RunDialogue();
            }
        }

        #region Other Dialogues
        private void OtherDialoguesLobby()
        {
            OtherDialoguesLobby(true);
        }
        private void OtherDialoguesRepeated()
        {
            OtherDialoguesLobby(false);
        }
        private void OtherDialoguesLobby(bool First = false)
        {
            MessageDialogue md = new MessageDialogue(First ? "*You have questions? Alright, I can try answering them.*" : "*That was all I have to tell about that. Is there something else you want to know?*");
            md.AddOption("About the Ether Realm.", AskAboutEtherRealm);
            md.AddOption("About the Terra Realm.", AskAboutTerraRealm);
            md.AddOption("That was all my questions.", Dialogue.ChatDialogue);
            md.RunDialogue();
        }

        private void AskAboutEtherRealm()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*The Ether Realm is a realm where TerraGuardians come from.*");
            md.AddDialogueStep("*We have kingdoms and towns there, and several places too, including unknown ones.*");
            md.AddDialogueStep("*But from what I've read in a book, the Ether Realm is a dangerous place for Terra Realm people like you.*");
            md.AddDialogueStep("*It seems like there's something in there, that weakens Terra Realm creatures. It's not specified what it is.*");
            md.AddDialogueStep("*TerraGuardians will not have many problems there, so if you plan on visiting it some time, I may recommend you to take TerraGuardians with you.*");
            md.AddOption("Return.", OtherDialoguesRepeated);
            md.RunDialogue();
        }

        private void AskAboutTerraRealm()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*The Terra Realm is where we are now.*");
            md.AddDialogueStep("*You probably know a lot more about it than me.*");
            md.AddDialogueStep("*It is said that a long time ago, TerraGuardians used to live alongside Terrarians, and help protect the Terra Realm.*");
            md.AddDialogueStep("*Maybe that explains why we're called TerraGuardians. I don't know why we left to the Ether Realm.*");
            md.AddDialogueStep("*One way we will end up finding out why, or at least I am reading many Terra Realms books to find out why.*");
            md.AddOption("Return.", OtherDialoguesRepeated);
            md.RunDialogue();
        }
        #endregion
    }
}
