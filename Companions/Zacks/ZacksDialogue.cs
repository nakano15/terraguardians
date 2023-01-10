using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class ZacksDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch(Main.rand.Next(3))
            {
                default:
                    return "*Just who are you? Why are you so scared? Something wrong with my face?*";
                case 1:
                    return "(The rotting corpse can't really ruin the distorted smile he gives for meeting me. Should I run away?)";
                case 2:
                    return "*Look at what I found, a Terrarian.*";
            }
            return base.GreetMessages(companion);
        }

        public override string NormalMessages(Companion companion)
        {
            TerraGuardian guardian = (TerraGuardian)companion;
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            bool BlueInTheWorld = WorldMod.HasCompanionNPCSpawned(1), SardineInTheWorld = WorldMod.HasCompanionNPCSpawned(2);
            if (!Main.bloodMoon)
            {
                if (BlueInTheWorld)
                {
                    Mes.Add("*Don't worry about your and your citizens safety, I hadan agreement with [gn:1] that I will not devour anyone in her presence. I promisse*");
                    if (player.head == 17)
                    {
                        Mes.Add("*You better be careful when wearing that hoodie, you may end up being caught by a very known bunny lover.*");
                    }
                }
                else
                {
                    Mes.Add("*I still remember the agreement we made, I wont eat your citizens, or you. I promisse.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*The sun doesn't do good to my rotten skin, so I preffer to stay on areas with shade.*");
                    Mes.Add("*I miss being alive, and being able to move my left leg.*");
                }
                else
                {
                    Mes.Add("*I preffer days like this, but with less monsters.*");
                    Mes.Add("*What kind of creatures are those?*");
                }
            }
            else
            {
                if (!NPC.downedBoss1)
                    Mes.Add("*I wonder if that Giant Eye is edible. Huh? What Giant Eye? Aren't you seeing it?*");
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss)
                    Mes.Add("*I think I'll try some vegan food. When do you plan on taking me to the Jungle?*");
                if (!Main.bloodMoon)
                {
                    Mes.Add("*I don't feel any kind of pain in my body, but I feel an unending hunger that only ceases when I eat something.*");
                    Mes.Add("*Are you going to leave me behind, too?*");
                    Mes.Add("*I only feel hunger for flesh, and when I do feel too much hunger, It doesn't end well.*");
                    Mes.Add("*Say... Could we go outside... For a walk..?*");
                }
                else
                {
                    Mes.Add("*I'm trying... To hold... Myself.. I feel like eating something whole....*");
                    Mes.Add("*Ugh.... Nghh....* (He's trying very hard to hold his hunger.");
                    Mes.Add("*Ugh... This night... Remembers me of when I died... Better... You not know of the details...* (That seems to make him very angry.)");
                }
            }
            /*switch (guardian.SkinID)
            {
                case OldBodySkinID:
                    {
                        Mes.Add("*Do I look less scary like this?*");
                        Mes.Add("*Well.. At least there aren't any more flies entering my mouth...*");
                        Mes.Add("*Some people said that I look less psychopathic like this.*");
                    }
                    break;
            }
            switch (guardian.OutfitID)
            {
                case MeatBagOutfitID:
                    Mes.Add("*\"Meat Bag\"... I hope you didn't helped [gn:"+Blue+"] pick this shirt.*");
                    Mes.Add("*It's good to have my wounds patched. It's really uncomfortable when people stares at your wounds, and shows their disgust face.*");
                    Mes.Add("*Well, I'm probably less scary now, but I still can't get rid of the smell.*");
                    break;
            }*/
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("(He got the thriller or something?)");
                Mes.Add("*What kind of dance moves are those? Seems like 90's ones?*");
                if (BlueInTheWorld)
                    Mes.Add("(He's dancing with [gn:1]. They look joyful when dancing.)");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Merchant + "] asked if I had any unused skin that I could sell. Look at me, do you think anything in this body could be of use?*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                Mes.Add("*Earlier, [nn:" + Terraria.ID.NPCID.Nurse + "] asked me if I could donate some blood, if there's some left. I asked her if she was crazy.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("*I visitted [nn:" + Terraria.ID.NPCID.Stylist + "] once and asked her to do some treatment on my fur, she then told me that can't do miracles. And recommended me a taxidermist.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(0))
            {
                Mes.Add("*If [gn:0] comes annoy him again, I'll try kicking his behind.*");
                Mes.Add("*If [gn:0] comes back with any funny jokes again, I'll kick his behind.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(1))
            {
                Mes.Add("*I feel bad when I speak with [gn:1], because she looks partially joyful and partially saddened at me.*");
                Mes.Add("*I think about [gn:1] several times, I wish to be alive and entire again just to be with her.*");
                /*if(PlayerMod.PlayerHasCompanionSummoned(player, Blue))
                {
                    switch(PlayerMod.GetPlayerGuardian(player, Blue).OutfitID)
                    {
                        case 1:
                            Mes.Add("*I would howl to [gn:" + Blue + "], if my lungs weren't badly damaged.*");
                            Mes.Add("*You look as great as when we first met, [gn:" + Blue + "].*");
                            Mes.Add("*[gn:" + Blue + "]... Looking at you now remembered me how much I like you.*");
                            break;
                        case 2:
                            Mes.Add("*Uh, [gn:" + Blue + "], where is your cloak? I didn't really liked the cloak, but couldn't imagine you not using it.*");
                            Mes.Add("*Wow! I'm really impressed at your outfit, [gn:" + Blue + "].*");
                            break;
                    }
                }*/
            }
            else
            {
                Mes.Add("*Huh? Oh, sorry... I just... Miss someone...*");
                if(Main.moonPhase == 0 && !Main.dayTime)
                    Mes.Add("(He's trying to howl at the moon, but his lungs were too damaged to be able to do that.)");
            }
            if (WorldMod.HasCompanionNPCSpawned(2))
            {
                Mes.Add("*I've been playing a game with [gn:2]. I were having fun with it, but I can't say the same about [gn:2].*");
                Mes.Add("*I've been getting really good at lasso, with the help of [gn:2]. What lasso? Well...*");
                Mes.Add("*Let me guess, you're worried that I may end up turning [gn:2] into a zombie? Don't worry about that, at least unless he dies, he'll be fine, probably.*");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 0))
            {
                Mes.Add("*I dislodged my left knee when kicking [gn:0]'s behind, can you help me put it into it's place?*");
                Mes.Add("(He seems to be paying attention to what [gn:0] is saying, but as soon as he said something stupid, the conversation ended.)");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("*Take good care of [gn:1] on your adventures, or else I'll take care of you. Understand?*");
                Mes.Add("*Both to take care on your quest, I don't want either of you turning into another Blood moon miniboss.*");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 2))
            {
                Mes.Add("(He shows a distorted smile while looking in [gn:2] direction, making him back away slowly.)");
                Mes.Add("*Hey [gn:2], let's play a game?* ([gn:2] is begging me that we should go, now.)");
            }
            if (WorldMod.HasCompanionNPCSpawned(5))
            {
                Mes.Add("*I really wanted to play with [gn:5], but my left leg doesn't really helps, so I can't do much else than dismiss him.*");
                if (WorldMod.HasCompanionNPCSpawned(2))
                {
                    Mes.Add("*Ever since [gn:5] has arrived, I barelly were able to play with [gn:2].*");
                }
                if (WorldMod.HasCompanionNPCSpawned(1))
                    Mes.Add("*Weird, [gn:1] haven't been playing with [gn:5] latelly. I wonders what happened.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(7))
            {
                Mes.Add("*Do you know why everytime [gn:7] sees me, she only shows one emotion, spooked?*");
                Mes.Add("*I tried greeting [gn:7], she ran away yelling like as if she saw a zombie or something. Wait...*");
                if (WorldMod.HasCompanionNPCSpawned(2))
                    Mes.Add("*You say that [gn:2] is [gn:7]'s husband? I guess the fear they have of me is from their family?*");
            }
            if (WorldMod.HasCompanionNPCSpawned(8))
            {
                Mes.Add("*I don't know if It's because I'm a wolf, a zombie, or if I'm male. But It gets quite hot when [gn:8] is around.*");
                Mes.Add("*[gn:8] said that heard a faint howling earlier? As if. I was practicing... Howling. I'm a wolf, after all.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(9))
            {
                Mes.Add("*I hate [gn:" + CompanionDB.Domino + "], he loves making bad jokes to people.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(10))
            {
                Mes.Add("*If you worry about [gn:" + CompanionDB.Leopold + "], don't worry, I wont eat him. But It is fun to make him panic.*");
                if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
                {
                    Mes.Add("*Sometimes I feel jealous about [gn:" + CompanionDB.Leopold + "], [gn:"+CompanionDB.Blue+"] hugs him way more than she does to me. But I don't really like the idea of spending hours around her arms too. So I guess I feel a bit of pity?*");
                }
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Vladimir) && WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
            {
                Mes.Add("*I went earlier to ask [gn:"+CompanionDB.Vladimir+"] why [gn:"+CompanionDB.Blue+"] visits him so much... I didn't knew how much pain I cause to her... And how much joy I brought to her once I returned to her side...*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
            {
                Mes.Add("*Why [gn:" + CompanionDB.Michelle + "] doesn't talk with me?*");
                Mes.Add("*I think [gn:" + CompanionDB.Michelle + "] seems to be a cool person, but she always ignores me...*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
            {
                Mes.Add("*It's quite nice having a new girl on the town. No, I'm not cheating [gn:" + CompanionDB.Blue + "] If that's what is on your mind.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Minerva))
            {
                Mes.Add("*Let me guess, [gn:" + CompanionDB.Minerva + "] told you that is scared of me, and made you come to me. Don't worry, as I said before, I wont eat any citizen. That doesn't stop me from scaring them, by the way.*");
                Mes.Add("*I don't know if [gn:" + CompanionDB.Minerva + "] charges for the food she makes, or if gives them for free. But I can only say that she cooks very good.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Glenn))
            {
                Mes.Add("*I see that you've met someone new for me to play with... Spooking [gn:" + CompanionDB.Glenn + "] will keep me entertained.*");
                Mes.Add("*I like following [gn:" + CompanionDB.Glenn + "] around when he's completelly alone. His attempts to escape from me makes me want to chase him more.*");
                Mes.Add("*I don't have anything against [gn:"+CompanionDB.Glenn+"], but I can use my current state to scare him just for fun.*");
                Mes.Add("*At night, I visit [gn:" + CompanionDB.Glenn + "]'s house to make sure he's inside, since being scared and locked inside, means not being outside and in danger.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Cinnamon))
            {
                Mes.Add("*It may sound weird, but I care for [gn:" + CompanionDB.Cinnamon + "]'s well being and safety.*");
                Mes.Add("*I hound around [gn:" + CompanionDB.Cinnamon + "]'s house during the night, since being scared and locked inside, means not being outside and in danger.*");
                Mes.Add("*Maybe if I take care of [gn:" + CompanionDB.Cinnamon + "], I'll practice to be a good parent when I have a child... If I have a child...*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Miguel))
            {
                Mes.Add("*Even [gn:" + CompanionDB.Miguel + "] thinks that I may not be able to grow a muscle, since my body is... Well.. You know... I'm quite happy that he still give me exercises for me to try.*");
                Mes.Add("*It's really complicated to visit [gn:" + CompanionDB.Miguel + "]. My ever hungry instincts make me want to devour him.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Luna))
            {
                Mes.Add("*[gn:"+CompanionDB.Luna+"]... She makes me droll... Not good...*");
                Mes.Add("*I try keeping my distance from [gn:"+CompanionDB.Luna+"], for her safety.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Green))
            {
                Mes.Add("*It seems like I wont have much need of a doctor...*");
            }

            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Yes, I can share my room with you, I can't sleep at night, anyway.*");
                Mes.Add("*If you're worried about being devoured during the night, don't worry, I wont. I know how to search for food outside.*");
                Mes.Add("*There is not much I can do during the night. Either I watch the window, or you sleep. I think I saw you putting your thumb on your mouth one night.*");
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, CompanionDB.Blue))
            {
                Mes.Add("*So, you're sharing room with [gn:"+CompanionDB.Blue+"], huh... I may wonder why she wouldn't want to share her room with me.*");
                Mes.Add("*Say.. You're sharing room with [gn:"+CompanionDB.Blue+"], right? How's she?*");
            }*/
            /*if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("*I'm sorry [nickname], I can't move. I think I pulled It up to It's limit.*");
                Mes.Add("*I'm paralized, I can't move at all.*");
                Mes.Add("*I can't move any part of my body.*");
            }*/
            if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("*I'm awaken. I can't sleep, ever since I turned into a zombie.*");
                Mes.Add("*There isn't much I can do while in the bed, so I only watch the ceiling, and expect something interesting to happen.*");
                Mes.Add("*There are all kinds of sounds that happens when people sleeps, you wouldn't believe If I told you them all.*");
                if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
                {
                    Mes.Add("*Since I stay up all night, the least I could do is make sure that [gn:" + CompanionDB.Blue + "] sleep safe and sound.*");
                    Mes.Add("*It comforts me a bit to watch [gn:" + CompanionDB.Blue + "] sleep, knowing that she's safe, and here with me.*");
                }
            }
            /*if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*Go back before It's too late! Things here are dreadful even for me!*");
                Mes.Add("*You don't know what I'm passing through here.*");
            }*/
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I don't think that just because I'm dead, I can comunicate with her. Sorry.*");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            Mes.Add("*I'm being hindered a bit by my left leg, but I'll try my best to follow you anyway.*");
            Mes.Add("*Should I consider doing fishing as a hobby. I got the bait, anyway.*");
            Mes.Add("*I keep trying to keep up to you, so you never leave me behind.*");
            Mes.Add("*Why zombies comes out from the floor? I don't know, I was never buried.*");
            Mes.Add("*Tell me [nickname], will we eventually find the Terrarian that left me to the zombies? It's not for revenge, at least a bit, but... Why?*");
            if(PlayerMod.PlayerHasCompanion(player, CompanionDB.Sardine))
                Mes.Add("*What do I make my lasso with? You will not like to know. I use my intestine to pull things to me, like [gn:2], whenever I'm chasing him.*");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3) && companion.wet)
            {
                Mes.Add("*There's water even where you wouldn't believe, I preffer not to give details.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(5) && WorldMod.HasCompanionNPCSpawned(1))
                Mes.Add("*After hearing of [gn:5]'s loss, I think I shouldn't feel so bad about my situation with [gn:1], she might have felt nearly the same...*");
            if (!PlayerMod.PlayerHasCompanionSummoned(player, 0))
            {
                Mes.Add("*Why [gn:0] is so annoying? Looks like he's not even grown up.*");
                Mes.Add("*See this [gn:0]? This is a fist, and It's reserved for the next time you come up with a funny zombie joke.*");
            }
            if(NPC.AnyNPCs(Terraria.ID.NPCID.Merchant) && !PlayerMod.PlayerHasCompanionSummoned(player, 1) && WorldMod.HasCompanionNPCSpawned(1))
                Mes.Add("*I wonder if I could give a gift to [gn:1]. Maybe that will cheer her up.*");
            if (!PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("(He started to be happy by seeying [gn:1], but once he saw the bits of despair in her face, he started to get saddened too.)");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 2))
            {
                Mes.Add("*Hey [gn:2], want to play \"The Walking Guardian\" with me? (As soon as he said that, [gn:2] started to scream and run away.)");
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                    Mes.Add("*The night I died, I were walking in the forest following a Terrarian, until a blood moon started. That Terrarian left me in the middle of the hordes of zombies to be eaten alive, after I helped him climb a corrupt cliff. I'm still furious about that.*");
                else
                    Mes.Add("*The night I died, I were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies to be eaten alive, after I helped him climb a corrupt cliff. I don't remember anything after that, other than suddenly \'waking up\' after listening to Blue's voice, and seeing yours and her face.*");
            }
            if (WorldMod.HasCompanionNPCSpawned(1))
            {
                Mes.Add("*I don't know why [gn:1] never mentions this to anybody, but she really loves bunnies. Try giving her one and thank me later.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I'm whole right now.*";
                    return "*My hunger is still on bearable levels.*";
                case RequestContext.HasRequest:
                    return "*Glad you ask, I really need something, but certainly isn't my final wish. This is it: [objective]. What do you say?*";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I'm so happy that you managed to do my request that could die again. Sorry for the bad pun.*";
                    return "(He got the things I brought him and is trying to fake his joy with a neutral look.)";
                case RequestContext.Accepted:
                    return "*You know where to find me.*";
                case RequestContext.TooManyRequests:
                    return "*You seems to have too many things to do right now. Maybe later.*";
                case RequestContext.Rejected:
                    return "*Didn't liked my request, [nickname]?*";
                case RequestContext.PostponeRequest:
                    return "*Well, It can wait, then.*";
                case RequestContext.Failed:
                    return "*I'm thinking about trying eating a succulent Terrarian now. Just kidding. I'm not that mad about this, but I wanted that request done.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you finally do it, [nickname]?*";
                case RequestContext.RemindObjective:
                    return "*Maybe something is wrong with your brain, I could analyze it if you allow me. Haha, just kidding. Anyways, I asked you to [objective]. Don't forget that again.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*You want me to live in the same world as you? Wow, Thanks. I guess I'm not scary for you anymore.*";
                case MoveInContext.Fail:
                    return "*Not right now.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I'd rather dig up a hole and sleep inside it.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*I hope isn't because you had enough of me, [nickname]. I will pack my things.*";
                case MoveOutContext.Fail:
                    return "*No. I will stay for a bit longer.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*It wasn't you who let me live here, and it will not be you who will kick me out.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I can help you on your travels, just don't leave me behind.*";
                case JoinMessageContext.Fail:
                    return "*Sorry, but I don't feel like I can follow you right now.*";
                case JoinMessageContext.FullParty:
                    return "*I think you have way too many people with you.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.Success:
                    return "*Okay. I'm going back home then.*";
                case LeaveMessageContext.Fail:
                    return "*I disagree. I'll continue limping after you for a while.*";
                case LeaveMessageContext.AskIfSure:
                    return "*Are you sure? I don't really like this place. I can't really die but... Being incapacitated isn't cool.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Then I'll try returning home. Be sure to visit me safe and sound.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Great choice, [nickname].*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*As long as you don't mind the smell, you may rest on my shoulder.*";
                case MountCompanionContext.Fail:
                    return "*I don't feel like carrying anyone for now.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*You can't walk by yourself?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*I hope wasn't the stench that made you ask me to put you on the floor.*";
                case DismountCompanionContext.Fail:
                    return "*Doesn't seems like the best moment for you to get off my shoulder.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I don't know if is a good idea. I mean, I wont eat you, but I'm a rotting corpse. I don't mind sharing my bed, I'm just saying though.*";
            return "*Fine. I'll get a bed for myself whenever I need one.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if(Share)
                return "*Right in the reach of my claws.*";
            return "*Changed your mind because I said about my claws? Hahaha.*";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Need to change my approach to foes? Very well, what do you have on mind?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*I like this plan. I can take a bite of my prey.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Hm. Fine. I hope my prey get closer to me anyways.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Fine. No snacks in combat then.*";
                case TacticsChangeContext.Nevermind:
                    return "*That's it?*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*You want to talk? Very well, what is it you want to talk about?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there anything else?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Very well. Something else before we stop talking?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }
    }
}