using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class VladimirDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hello there, little buddy. Need a hug? I'll provide you as many as you need.*");
            Mes.Add("*Aren't you a Terrarian? Hello! I'm expert in hugging.*");
            Mes.Add("*Oh, You're a Terrarian. Do you need a new friend?*");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            bool IsPlayerBeingHugged = false; //guardian.DoAction.InUse && guardian.DoAction.ID == HugActionID && guardian.DoAction.IsGuardianSpecificAction;
            /*if (IsPlayerBeingHugged)
            {
                IsPlayerBeingHugged = ((Companions.Vladimir.HugAction)guardian.DoAction).Target == player;
            }*/
            List<string> Mes = new List<string>();
            Mes.Add("*I really don't mind giving hugs to anybody.*");
            Mes.Add("*Why do I like giving hugs? I used to give them to my younger brother, It always helped in various situations where he was sad.*");
            Mes.Add("*I feel that right now there are people needing me here.*");
            Mes.Add("*I don't think I can help solving people problems, but I can at least try with hugs.*");
            Mes.Add("*I most likely get confessions from the people I hug, so I mostly know of things many don't.*");
            Mes.Add("*I can try telling you some of the things I heard from the people I hug, just don't tell anyone.*");
            if (!Main.bloodMoon)
            {
                Mes.Add("*I heard from other citizens that I get really scary during blood moons... I would like to apologize for my behavior.*");
            }
            if (!PlayerMod.PlayerHasCompanionSummoned(player, CompanionDB.Vladimir))
            {
                Mes.Add("*Don't you want to take \"Teddy\" out for a walk?*");
                Mes.Add("*Let's go on an adventure!*");
                Mes.Add("*Are you going somewhere? I love adventures too!*");
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    if (!Main.raining)
                    {
                        Mes.Add("*(Takes a deep breath) Ah... I love this weather. It's perfect for an adventure.*");
                        Mes.Add("*I really enjoy days like this, I feel like doing anything.*");
                    }
                    else
                    {
                        Mes.Add("*Well, I didn't really felt like doing anything outside, anyway.*");
                    }
                }
                else
                {
                    if (IsPlayerBeingHugged)
                        Mes.Add("*Don't worry, nothing will harm you while in my arms.*");
                    Mes.Add("*Hey! I think I have watched the movie that monster is from! What monster? That one, don't you see?*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*The night smiles upon us.*");
                    Mes.Add("*Yawn~ I'm starting to get sleepy.*");
                    Mes.Add("*This night remembers me of my brother, I used to help him sleep when he was scared of monsters under his bed.*");
                }
                else
                {
                    Mes.Add("*Could you please MAKE THOSE MONSTERS BE SILENT!!! I'M TRYING TO SLEEP!!*");
                    Mes.Add("*You! Go make them stop! My head is even aching, because I CAN'T GET EVEN A LITTLE BIT OF SLEEP!*");
                    Mes.Add("*SHUT UP OUT THERE!!! I WANT TO SLEEP!*");
                }
            }
            bool HasBlue = CanTalkAboutCompanion(CompanionDB.Blue), HasZacks = CanTalkAboutCompanion(CompanionDB.Zacks);
            if (HasBlue && HasZacks)
            {
                if(!IsPlayerBeingHugged)
                    Mes.Add("*I discovered [gn:" + CompanionDB.Blue + "]'s secret. [gn:" + CompanionDB.Zacks + "] said that she really likes bunnies, I'm very sure she will like it if you give one to her to hold.*");
                Mes.Add("*I can't heal [gn:"+CompanionDB.Blue+"] and [gn:"+CompanionDB.Zacks+"] pain with hugs... It seems like they need to heal It by themselves.*");
            }
            else if (HasBlue && !HasZacks)
            {
                Mes.Add("*I saw [gn:" + CompanionDB.Blue + "] crying earlier this day, I gave her a hug, but she didn't stop crying. I wonder what makes her sad.*");
                Mes.Add("*It pains me to see [gn:" + CompanionDB.Blue + "] sad. Can we do something for her?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*It looks like [gn:" + CompanionDB.Rococo + "] thinks like me in some ways.*");
                Mes.Add("*Sometimes It feels like what makes the guardians be attracted to this place, is [gn:" + CompanionDB.Rococo + "].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                if (NPC.downedBoss3) Mes.Add("*I will never go drink with [gn:" + CompanionDB.Brutus + "] again. I don't even remember how I ended up inside the Dungeon.*");
                Mes.Add("*Whenever I try giving a hug to [gn:" + CompanionDB.Brutus + "], he tells me to back off, because he's male. What that has to do with hugging?!*");
                Mes.Add("*I wonder if [gn:"+CompanionDB.Brutus+"] has any kind of pain.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Mabel) && !IsPlayerBeingHugged)
            {
                Mes.Add("*I... I didn't want to mention this, but hugging [gn:" + CompanionDB.Mabel + "] was an error. I need to find the toilet ASAP.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("*I sometimes make company to [gn:" + CompanionDB.Alex + "], when he goes to visit " + AlexRecruitmentScript.AlexOldPartner + "'s tombstone. It's really tragic what happened to her.*");
                Mes.Add("*Everyone has loss, but I don't know if [gn:" + CompanionDB.Alex + "] is getting over his.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("*You won't believe this, but after several attempts, I managed to be able to hug [gn:" + CompanionDB.Domino + "]! But I wonder, why did he need a hug?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*For a sage, [gn:" + CompanionDB.Leopold + "] has a hundred one troubles.*");
                Mes.Add("*Generally when [gn:" + CompanionDB.Leopold + "] comes, he keeps debating himself his theories. When I wake up, he's already gone. I've never seen him angry at me for doing that.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*I like hugging [gn:" + CompanionDB.Michelle + "], she's one of the few people that want a hug for no actual reason.*");
                Mes.Add("*Sometimes I sing a lullaby at night for [gn:"+CompanionDB.Michelle+"].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I'm glad that [gn:"+CompanionDB.Malisha+"] noticed that I'm not much into chatting while hugging.*");
                Mes.Add("*How did [gn:"+CompanionDB.Malisha+"] knew of my family? I hope she doesn't tell them that I'm here. It could bring trouble to this realm.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*It's a bit hard to help [gn:" + CompanionDB.Fluffles + "], because I can't touch her, so I kind of pretend to be hugging her. It seems to be working.*");
                Mes.Add("*I question myself why [gn:" + CompanionDB.Fluffles + "] can't talk. I don't mind the silence when hugging, but It bothers me that she can't speak at all. Can you help her solve that problem?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*Since the moment we met, [gn:" + CompanionDB.Minerva + "] guessed right the kind of food I like. I always love eating her food.*");
                Mes.Add("*Whenever I'm eating the food [gn:" + CompanionDB.Minerva + "] makes, stares at me with a smile on her face. I think she's glad that I'm liking It.*");
                Mes.Add("*Sometimes [gn:" + CompanionDB.Minerva + "] asks me to hug her, and she eventually falls asleep when being hugged, and that's all fine. The problem is that she has a little flatulence problem. Sometimes I accidentally wake her up because I'm trying to blow the smell away.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                bool HasBreeMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Bree), HasSardineMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Sardine);
                if(!HasBreeMet && !HasSardineMet)
                {
                    Mes.Add("*I'm really worried about [gn:"+CompanionDB.Glenn+"], both of his parents are missing. Help him find them.*");
                }
                else if(HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*After both [gn:" + CompanionDB.Glenn + "]'s parents were found, he started to have a happy look in his face. Good job, [nickname].*");
                }
                else if (HasBreeMet)
                {
                    Mes.Add("*[gn:" + CompanionDB.Glenn + "] is happy that his mother was found, but he's still worried about his father...*");
                }
                else if (HasSardineMet)
                {
                    Mes.Add("*[gn:" + CompanionDB.Glenn + "] is happy that his father was found, but he's still worried about his mother...*");
                }
                Mes.Add("*I sometimes help [gn:"+CompanionDB.Glenn+"] with his studies, even more, whenever he needs a pacific place to study.*");
                Mes.Add("*At first, [gn:"+CompanionDB.Glenn+"] was really scared of me. After we just talked for a moment, his fear vanished.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*[gn:" + CompanionDB.Cinnamon + "] is a good girl. Sometimes I help her test the food she cooks.*");
                Mes.Add("*Sometimes [gn:" + CompanionDB.Cinnamon + "] falls asleep when we're eating some food. I place her on my bed to sleep when that happens.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*[gn:" + CompanionDB.Miguel + "] keeps making jokes about me, saying that I keep exercising my arms by carrying weight every day.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cille))
            {
                Mes.Add("*I can see the sadness on [gn:" + CompanionDB.Cille + "]'s face. I think she wants friends, but she avoids meeting people.*");
                Mes.Add("*I want to be friends of [gn:" + CompanionDB.Cille + "], but she always asks me to leave.*");
            }
            bool HasSardine = CanTalkAboutCompanion(CompanionDB.Sardine), HasBree = CanTalkAboutCompanion(CompanionDB.Bree);
            if (HasSardine)
            {
                Mes.Add("*[gn:" + CompanionDB.Sardine + "] told me that the reason why he left home was to look for good treasures, so he could make his family live well. He didn't expect to forget which world he lives.*");
                if (!HasBree)
                {
                    Mes.Add("*[gn:" + CompanionDB.Sardine + "] is very worried about his wife, he wonders how she's doing.*");
                }
                else
                {
                    Mes.Add("*[gn:" + CompanionDB.Sardine + "] wonders why [gn:"+CompanionDB.Bree+"] keeps trying to control his steps.*");
                }
            }
            if (HasBree)
            {
                Mes.Add("*[gn:"+CompanionDB.Bree+"] is worried about her son. It stayed at home when she went to look for "+(HasSardine ? "[gn:" + CompanionDB.Sardine+"]" : "her husband")+"*");
                if (!HasSardine)
                {
                    Mes.Add("*[gn:" + CompanionDB.Bree + "] keeps being worried about her husband, she sometimes even thinks he may be dead, but I always tell her not to be foolish to think of that.*");
                }
                else
                {
                    Mes.Add("*[gn:"+CompanionDB.Bree+"] feels lonely sometimes. She thinks [gn:"+CompanionDB.Sardine+"] cares more about adventure than her.*");
                }
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("*You won't believe me, but [nn:" + Terraria.ID.NPCID.Merchant + "] is really sad because nobody wants to buy his angel statues.*");
                Mes.Add("*About how much dirt sells overseas... [nn:" + Terraria.ID.NPCID.Merchant + "] was lying.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
            {
                Mes.Add("*I like hugging [nn:" + Terraria.ID.NPCID.Mechanic + "], she seems to be the only person who gets a hug because she wants one.*");
                Mes.Add("*People like [nn:" + Terraria.ID.NPCID.Mechanic + "] brighten my day.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] sometimes comes asking me for a hug, but he always falls asleep after that.*");
                Mes.Add("*Sometimes I refuse to hug [nn:" + Terraria.ID.NPCID.Angler + "], because my mouth starts filling with salivae when he gets close, so I tell him that he must take a bath before I can hug.*");
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] sometimes comes to me, complaining about how [gn:" + CompanionDB.Mabel + "] keeps telling him to eat variated food, take baths, or go to sleep. I say that she is trying to keep him well and safe.*");
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] told me earlier that likes some things [gn:" + CompanionDB.Mabel + "] does, like taking him on a trip, telling him bedtime stories, and singing.*");
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] said that whenever he's with [gn:" + CompanionDB.Mabel + "], he feels safe. He also said that didn't felt like that since... He stopped talking here, but could he be talking about...*");
                }
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse) && NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Nurse + "] sometimes comes to me to talk about [nn:" + Terraria.ID.NPCID.ArmsDealer + "], but I don't know what to answer, that's not my thing! That's when I directed her to [gn:"+CompanionDB.Blue+"].*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TaxCollector))
            {
                Mes.Add("*I discovered earlier that hugging [nn:" + Terraria.ID.NPCID.TaxCollector + "] is a good way of avoiding tax collection.*");
                Mes.Add("*I actually dislike hugging [nn:" + Terraria.ID.NPCID.TaxCollector + "], he smells like an abandoned house. I nearly sneezed last time.*");
            }

            if (companion.IsUsingToilet)
            {
                Mes.Add("*We have to talk. We need a bigger toilet, I don't think this one will handle what is coming.*");
                Mes.Add("*I haven't started yet, I'm trying to aim at the hole, but It's too small.*");
            }

            if (IsPlayerBeingHugged)
            {
                if (companion.TargettingSomething)
                {
                    if (Main.bloodMoon)
                    {
                        Mes.Add("*I WILL SILENCE YOU NOW!!!*");
                        Mes.Add("*DIE VILE CREATURE!!!!*");
                        Mes.Add("*I WILL MAKE YOUR HEAD MY TROPHY!!!!*");
                    }
                    else
                    {
                        Mes.Clear();
                        if (companion.Health >= companion.MaxHealth * 0.5f)
                        {
                            Mes.Add("*Don't worry, I'll handle this.*");
                            Mes.Add("*It will be gone in a minute, don't mind.*");
                            Mes.Add("*Ack, It's a monster! I will deal with it.*");
                            Mes.Add("*Don't mind that, I will take care of that.*");
                            Mes.Add("*I will protect you.*");
                        }
                        else
                        {
                            Mes.Add("*Gasp~! I may need some help here.*");
                            Mes.Add("*I didn't wanted to ask this, but this is being quite tough for me.*");
                            Mes.Add("*Uh... A little help, please?*");
                            Mes.Add("*I think you may need to run if this get harder.*");
                        }
                    }
                }
                else if (Main.bloodMoon)
                {
                    Mes.Clear();
                    Mes.Add("*Those monsters should Be More QUIET!! I'M WANT TO GET SOME SLEEP!*");
                    Mes.Add("*WHY ARE YOU GUYS BEING SO NOISY! I WANT TO TAKE A REST!!*");
                    Mes.Add("*IF YOU DON'T GET QUIET, I WILL RIP THE FLESH OUT OF YOUR BODY!*");
                    Mes.Add("*STOP SHIVERING ALREADY!!*");
                }
                else
                {
                    if (PlayerMod.PlayerGetControlledCompanion(player) != null)
                    {
                        Mes.Add("*I sense someone else's presence in you... Is that you, " + player.name + "?*");
                        Mes.Add("*It is you " + player.name + "? Sorry, but you can't fool me, I can sense that It's you because we've met before. The bond never fails.*");
                    }
                    if (!companion.HasBuff(Terraria.ID.BuffID.WellFed) && !companion.HasBuff(Terraria.ID.BuffID.WellFed2) && !companion.HasBuff(Terraria.ID.BuffID.WellFed3))
                    {
                        Mes.Add("*Huh? My stomach? Oh, sorry. It's because I'm getting hungry.*");
                    }
                    Mes.Add("*I always feel happiness whenever I hug someone.*");
                    Mes.Add("*Sometimes, I feel a bit depressed after listening to other people stories. That is why I prefer to hug while silent.*");
                    Mes.Add("*It's funny how from everyone in the world, you are the only one who doesn't confess anything.*");
                    Mes.Add("*I wonder if people think I have problems too... But I don't think I would openly talk about them to someone.*");
                    Mes.Add("*Say... Could you try catching some salmons for me, sometime.*");
                    if (player.ZoneSnow)
                    {
                        Mes.Add("*You wont feel cold like this.*");
                        Mes.Add("*Brr... It's cold here. Good thing that you are being warmed by me.*");
                        if (player.ZoneRain)
                        {
                            Mes.Add("*A-a-are you feeling c-c-old t-t-too?*");
                            Mes.Add("*My f-fur, It's b-barelly being able t-t-to protect m-me from c-cold.*");
                        }
                    }
                    if (player.ZoneDesert)
                    {
                        Mes.Add("*It's too hot here. Do you have a bottle of water with you?*");
                        Mes.Add("*I can't stop sweating... You're dripping. Better drink some water, or you will pass out.*");
                        Mes.Add("*We need to go someplace cool, or else we may end up passing out due to the heat.*");
                    }
                    if (player.ZoneRain)
                    {
                        Mes.Add("*It's raining cats and dogs outside, good thing that we're not out there.*");
                        Mes.Add("*It's a nice weather for a rest. Let's enjoy the sound of raindrops.*");
                        Mes.Add("*Achooo~! Sorry, I think I have caught flu.*");
                    }
                    if (!Main.dayTime)
                    {
                        Mes.Add("*This night seems perfect to sleep.*");
                        Mes.Add("*Do you mind if I hug you during my sleep?*");
                        Mes.Add("*...Zzz... Oh- ah! I'm awake!*");
                    }
                    else
                    {
                        if (!player.ZoneRain && !Main.eclipse)
                        {
                            Mes.Add("*It's a beautiful day outside!*");
                            Mes.Add("*This weather makes me happy.*");
                        }
                        else if (Main.eclipse)
                        {
                            Mes.Add("*Is this house safe against those creatures?*");
                            Mes.Add("*Yikes! Where did those things came from?*");
                            Mes.Add("*I'm not scared, It's cold here.*");
                        }
                    }
                    Mes.Add("*Do I talk during sleep? Some of the people I hug say that I do.*");
                    Mes.Add("*Some people says I talk during my sleep. I don't believe.*");
                    /*if (companion.IsPlayerRoomMate(player))
                    {
                        Mes.Add("*I'm very happy of sharing my room with you, I never feel lonelly at night again.*");
                        Mes.Add("*Having someone to hug during the night always makes the night better.*");
                    }*/
                    if (companion.IsUsingToilet)
                    {
                        Mes.Clear();
                        Mes.Add("*If the smell bother you, I can hug you another time.*");
                        Mes.Add("*Did you smell that? Sorry. I had to release.*");
                        Mes.Add("*I hope you don't mind the noises.*");
                        Mes.Add("*Ugh... My stomach... I think this is a bad time for a hug... Oww...*");
                        Mes.Add("*I don't mind hugging at this moment, It's not like as if you are watching me doing my business.*");
                        Mes.Add("*Sorry If I'm hugging too strong, but I really need to use some strength right now.*");
                        Mes.Add("*Did that noise scared you? Sorry.*");
                        Mes.Add("*Sorry for moving too much, I'm trying to aim at the toilet hole.*");
                    }
                }
            }
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*That person in your shoulder looks sad. Does she needs a hug?*");
            }*/
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I like giving hugs because I used to give them to my younger brother. I always felt great when giving him hugs whenever he needed. But then... He grew up...*");
            Mes.Add("*My father never liked the fact I used my huge paws to hug, he said I should have been a fierce warrior like the ancestors from my family.*");
            Mes.Add("*Some people look at me with weird eyes whenever they see me hugging people. What is on their mind?*");
            Mes.Add("*Remember when I mentioned my younger brother? I don't feel bad about the fact he grew up, but he rarely needs me for anything, It's like as if I got a hole in my life...*");
            Mes.Add("*Okay, Okay, I admit. I began exploring to look for places where I could help people, and fill the hole left by my brother. Good thing that there is way more to fill that hole than I expected.*");
            Mes.Add("*Whenever I hug someone, most of the times they start talking about things that trouble them. So I feel like a therapist most of the time, but one who mostly listens only.*");
            Mes.Add("*You would be impressed at the variety of troubles I listen to when hugging people.*");
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I have to tell you, [gn:"+CompanionDB.Malisha+"] scares me a bit.*");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*Nope, I don't need anything.*";
                    return "*I have everything I need with me.*";
                case RequestContext.HasRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*I feel awkward for asking that but... Wait, don't laugh, I don't mind giving hugs, but asking things isn't my kind of thing. Would you please [objective] for me?*";
                    return "*Uh... Say... Could you do something for me? I need help with this: [objective]. Can you do it for me?*";
                case RequestContext.Accepted:
                    return "*Thank you.*";
                case RequestContext.TooManyRequests:
                    return "*I'm sorry, but I can't allow that. You will get yourself stressed out if you do many things at the same time. Go do your other requests before I give you mine.*";
                case RequestContext.Rejected:
                    return "*Oh... Okay... (Did I ask something too hard?)*";
                case RequestContext.PostponeRequest:
                    return "*Sure. My request can wait.*";
                case RequestContext.Failed:
                    return "*Don't worry [nickname], you tried your best. If you want, I can give you a hug so you can feel better.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*You helped Teddy, didn't you [nickname]?*";
                case RequestContext.RemindObjective:
                    return "*My request is for you to [objective], in case you forgot.*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*You're really a great person, I like that.*";
                    return "*I think only hugging isn't enough, here are some things I had stored.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*You'd like me to live here? I can live here too. Thank you. I just wonder if you have a house big enough for me.*";
                case MoveInContext.Fail:
                    return "*This doesn't seem like the best moment to move in.*";
                case MoveInContext.NotFriendsEnough:
                    return "*You seem like a nice person, but I don't entirely trust you to move in here.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You don't need me here anymore? Okay. This is not a goodbye, since you can always talk to me, but do let me know if you change your mind.*";
                case MoveOutContext.Fail:
                    return "*I'm not moving out right now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*No. A friend of mine let me stay here, and I will stay here until they say otherwise.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*You want to pick Teddy as your buddy? Are you sure, [nickname]?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*I will be your friend forever, [nickname].*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*Aww... Forget it..*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I'd love to have a Buddy for myself, but I will only do that to someone I know.*";
                case BuddiesModeContext.Failed:
                    return "*At any other moment, I would be happy of listening to that, but right now is not the moment.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*You've already got a buddy for yourself, [nickname]. Maybe someday I'll get my own buddy too.*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*Yes, I can lead the group.*";
                case ChangeLeaderContext.Failed:
                    return "*I don't think I can do that right now...*";
            }
            return base.ChangeLeaderMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*May this be my embrace for you, with my body providing you safety and protection, while I take the backseat.*";
                case ControlContext.SuccessReleaseControl:
                    return "*I hope I managed to help you achieve your goal, [nickname].*";
                case ControlContext.FailTakeControl:
                    return "*I don't think this is a good moment for that.*";
                case ControlContext.FailReleaseControl:
                    return "*I can't release you now, [nickname]. Better find a better moment for that.*";
                case ControlContext.NotFriendsEnough:
                    return "*I can't, [nickname]. At least not now. Maybe once we're more friends.*";
                case ControlContext.ControlChatter:
                    switch (Main.rand.Next(3))
                    {
                        default: 
                            return "*Do you need to ask me something?*";
                        case 1:
                            return "*Don't worry, my body will keep you safe.*";
                        case 2:
                            return "*I'm still here, [nickname].*";
                    }
            }
            return base.ControlMessage(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*I don't mind carrying you, [nickname].*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*Thank you, [nickname].*";
                case MountCompanionContext.Fail:
                    return "*I don't think right now is a good moment for that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*Sorry, but I won't be carrying you right now.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I don't mind carrying them for you.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I can do that. Who do you need me to carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*There you go, [nickname].*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Thank you for sharing your mount.*";
                case DismountCompanionContext.Fail:
                    return "*Not at this moment, [nickname].*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch (context)
            {
                case InviteContext.Success:
                    return "*Do you need to see me? Alright. I will be showing up soon.*";
                case InviteContext.SuccessNotInTime:
                    return "*I understand that you need to see me, [nickname]. I will be showing up tomorrow.*";
                case InviteContext.Failed:
                    return "*Not a good moment to visit you now. Sorry.*";
                case InviteContext.CancelInvite:
                    return "*You want to cancel my visit? Okay.*";
                case InviteContext.ArrivalMessage:
                    return "*I'm here, [nickname].*";
            }
            return base.InviteMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*You need me to do something for you? Feel free to ask it.*";
                case InteractionMessageContext.Accepts:
                    return "*I will do that.*";
                case InteractionMessageContext.Rejects:
                    return "*Sorry, [nickname].*";
                case InteractionMessageContext.Nevermind:
                    return "*Oh, okay.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*Sure. I would love making you company during your travels.*";
                case JoinMessageContext.Fail:
                    return "*I can't right now. I have some other things to do right now. I'm sorry.*";
                case JoinMessageContext.FullParty:
                    return "*I'm sorry, but there are too many people in the group right now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Alright. I had so much fun exploring the world with you. Feel free to call me another time.*";
                case LeaveMessageContext.Fail:
                    return "*Better I not leave your group right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*But [nickname], this place is dangerous for me. Do you really want to leave me all alone in this place?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I'll try getting home then. Have a safe travel, [nickname].*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*I'm happy that you changed your mind, [nickname].*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Need me to change how I will take on combat? Sure, what should I?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Take on monsters in close range? Got it.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Attack them at mid-range? Got it.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Avoid contact as much as possible? Got it.*";
                case TacticsChangeContext.Nevermind:
                    return "*My current tactic is fine? Alright then.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch (context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*You want to chat? It's always good to hang out with a friend sometimes. What do you want to know?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Do you want to speak about something else?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*I enjoyed the chatting, let's talk more later.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I don't think we will be able to share the same bed, but I can still hold you on my paws.*";
            return "*You don't want to share a bed with me anymore? I was sleeping better when having someone to hug.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I can hold you when using a chair. I hope the chair is strong enough for both of us.*";
            return "*I'll look for a chair for me, then.*";
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MountUnlock:
                    return "*Hey buddy, I don't mind having something to hug during the travels, so if you need me to carry you, feel free to ask. At least you won't get tired.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*Hey again! I have some of my family's resistance in me, so If you need me to do something extremely dangerous, I can do it.*";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*I really like having you as my friend, and I think you are trustworthy enough to be my Buddy. I know it's an important thing, so if you think about picking me as your Buddy, I won't being your personal Teddy.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*Since we're now buddies, I will no longer mind doing many things you ask of me. Yes, if you want me to carry you, I would be happy to do so, too.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            return base.GetOtherMessage(companion, Context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            List<string> Mes = new List<string>();
            if ((companion.Data as VladimirData).CarrySomeone)
            {
                Mes.Add("(pulls closer.)");
                Mes.Add("*Zzz... Warm.... Zzz...*");
                Mes.Add("*Zzz.... Bro... Are you... Crying... Zzz...*");
                Mes.Add("*Zzz... Don't... Feel sad... I'm here... Zzz...*");
                Mes.Add("*Zzz.... You're not... Weak... Bro... Zzz...*");
                Mes.Add("*Zzz... Bro... Where are you... Going... Zzz...*");
                Mes.Add("*Zzz... Mom... Zzz...*");
                Mes.Add("*Zzz... I miss you... Mom... Zzz...*");
                Mes.Add("*Zzz... Father... Don't be mean... I don't wanna... Fight... Zzz...*");
                Mes.Add("*Zzz... I'm... Not a fighter... Zzz...*");
                Mes.Add("*Zzz... I'm not alone... Zzz...*");
                Mes.Add("*Zzz... Friends... Good... Zzz...*");
                Mes.Add("*Zzz... Where I Belong... Octave fantasy... Zzz...*");
            }
            else
            {
                Mes.Add("(Even when sleeping he seems happy)");
                Mes.Add("*Brother, don't feel sad...* (He says when sleeping)");
                Mes.Add("*Where are you going, brother...? * (He says when sleeping)");
                Mes.Add("*I want to help everybody... Hug everyone... In need..* (He says when sleeping)");
                Mes.Add("(He seems to be having nightmares)");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            VladimirData data = (VladimirData)companion.Data;
            //if (!data.CarrySomeone)
            if (!companion.IsRunningBehavior)
            {
                dialogue.AddOption("Hug me.", HugPlayerDialogue);
            }
            else
            {
                //if (data.CarriedCharacter == MainMod.GetLocalPlayer)
                if (companion.GetGoverningBehavior() is Vladimir.VladimirHugPlayerBehavior)
                {
                    dialogue.AddOption("Enough hug.", StopHuggingPlayerDialogue);
                }
            }
        }

        private void HugPlayerDialogue()
        {
            if (!Dialogue.Speaker.IsSameID(CompanionDB.Vladimir))
                return;
            TerraGuardian Vladimir = (TerraGuardian)Dialogue.Speaker;
            VladimirData data = (VladimirData)Vladimir.Data;
            VladimirBase vladbase = (VladimirBase)Vladimir.Base;
            MessageDialogue dialogue = new MessageDialogue();
            if (PlayerMod.PlayerGetMountedOnCompanion(MainMod.GetLocalPlayer) != null)
            {
                dialogue.ChangeMessage("*Get off your guardian first.*");
                dialogue.AddOption("Oh, alright.", Dialogue.LobbyDialogue);
            }
            else
            {
                /*if (data.CarrySomeone)
                {
                    vladbase.PlaceCarriedPersonOnTheFloor(Vladimir, data);
                }*/
                Vladimir.RunBehavior(new Vladimir.VladimirHugPlayerBehavior(Vladimir, MainMod.GetLocalPlayer));
                //vladbase.CarrySomeoneAction(Vladimir, data, MainMod.GetLocalPlayer, InstantPickup: true);
                dialogue.ChangeMessage("*Press Jump button or speak with me if you want me to stop.*");
                dialogue.AddOption("Okay.", Dialogue.LobbyDialogue);
            }
            dialogue.RunDialogue();
        }

        private void StopHuggingPlayerDialogue()
        {
            if (!Dialogue.Speaker.IsSameID(CompanionDB.Vladimir))
                return;
            TerraGuardian Vladimir = (TerraGuardian)Dialogue.Speaker;
            VladimirData data = (VladimirData)Vladimir.Data;
            VladimirBase vladbase = (VladimirBase)Vladimir.Base;
            //if (data.CarrySomeone)
            if (Vladimir.IsRunningBehavior && Vladimir.GetGoverningBehavior() is Vladimir.VladimirHugPlayerBehavior)
            {
                Vladimir.GetGoverningBehavior().Deactivate();
                //vladbase.PlaceCarriedPersonOnTheFloor(Vladimir, data);
                MessageDialogue md = new MessageDialogue(GetEndHugMessage(Vladimir));
                md.AddOption("Thanks.", Dialogue.LobbyDialogue);
                md.RunDialogue();
            }
        }

        public string GetEndHugMessage(TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("*GOOD.*");
                Mes.Add("*I don't need a burden now!*");
            }
            else if (guardian.IsUsingToilet)
            {
                Mes.Add("*I hope it wasn't because of the smell.*");
                Mes.Add("*I have to admit, this is quite unpleasant.*");
                Mes.Add("*The noises scared you?*");
            }
            else if (guardian.IsSleeping)
            {
                Mes.Add("*Zzz... Goodbyezzzz....*");
                Mes.Add("(Continues snoring)");
                Mes.Add("*See you... Zzzz*");
                Mes.Add("*... Sheeps... Zzz...*");
            }
            else
            {
                Mes.Add("*Alright.*");
                Mes.Add("*I hope I helped.*");
                Mes.Add("*Feeling better?*");
                Mes.Add("*That is enough? See you then.*");
                Mes.Add("*Anytime.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
