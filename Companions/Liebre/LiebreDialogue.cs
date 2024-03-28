using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LiebreDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Don't be afraid, Terrarian. Your time haven't came yet, but the creatures you've killed must be carried to their destination.*";
                case 1:
                    return "*I have came to this place due to the many deaths happening here. It will ease my job if I stay around.*";
                case 2:
                    return "*I didn't came for your soul, if that's what you are thinking. I'm only going to be a ferry for the creatures who dies here.*";
            }
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My body consists of a kind of plasma, which I use to carry the souls I save. When It's filled enough, I deliver them to their destination.*");
            Mes.Add("*You can only see my body in dark places. If there's light, you can only see my bones.*");
            Mes.Add("*The reason why I'm missing a pelvis and my legs, is related to when I died.*");
            Mes.Add("*Depending on the amount of souls I save, they make my body look like the universe full of stars.*");

            Mes.Add("*I try to avoid being sociable with the people around, since I wouldn't feel good when taking them to their end of line.*");
            Mes.Add("*My presence here makes people scared, they may be thinking I'm after them.*");
            Mes.Add("*Due to people knowing who I am, they avoid even speaking to me. That makes It easier for me not to care about them.*");

            Mes.Add("*It may not look like It, but I like seeing children around. It makes me proud of my job.*");
            Mes.Add("*Don't worry if you see me watching over the children playing. You should be more worried about the creatures that could interrupt their fun.*");

            if(Lighting.Brightness((int)(companion.Center.X * (1f / 16)), (int)(companion.Center.Y * (1f / 16))) < 0.5f)
            {
                Mes.Add("*Don't fear me in this form, I'm still the same person you've met.*");
                Mes.Add("*My skeletal form is less spooky to people than this form, so I avoid letting people see me in the dark.*");
            }
            if (!(companion as LiebreBase.LiebreCompanion).HoldingScythe)
            {
                Mes.Add("*This scythe will not hurt anyone by It's own, don't worry.*");
                Mes.Add("*I recommend you not to stay too close to my scythe.*");
            }

            if (MainMod.GetLocalPlayer.difficulty == 2)
            {
                Mes.Add("*I'm starting to fear the end of your line, [nickname]. Taking you to your destination will end up being a pain to me.*");
            }
            else
            {
                Mes.Add("*If you take me with you on your adventures, I can hold onto your and your allies souls until you resurrect.*");
            }

            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*I shouldn't actually try saving those abominations. I think I should instead burn them, but that's against the rules.*");
                    Mes.Add("*What kind of monster created those monstrosities. Even their souls seems twisted.*");
                }
                else
                {
                    Mes.Add("*I like seeing living things around, beside you may have thought otherwise.*");
                    Mes.Add("*I used to hate being exposed to the sun for long period, but now I don't feel anything.*");
                    Mes.Add("*It's really weird to me when people stares at my bones. At those moments, I wished It was dark.*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*[nickname], there's many of them around... Be careful...*");
                    Mes.Add("*I suggest you to ensure the safety of the people around.*");
                    Mes.Add("*I can take care of some of those zombies who walks around, I'm demi immortal anyways. But I fear for the other people around.*");
                }
                Mes.Add("*I can't save the zombified people that appears during the night until their bodies aren't destroyed.*");
                Mes.Add("*You should stay safe at night, [nickname]. At this moment, dangerous creatures roams the world.*");
                Mes.Add("*I preffer when people see my plasma, but I wasn't always blue. I had white fur, and a yellow circle on my fur that covered my left eye.*");
            }
            if (Main.raining)
            {
                Mes.Add("*I'm glad I don't need an umbrella.*");
                Mes.Add("*[nickname], beware not to catch a flu. Even a simple flu can be dangerous, if you let It evolve further.*");
            }

            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("*I went earlier to [nn:" + Terraria.ID.NPCID.Merchant + "]'s shop to try buying some potions, he ended up pleading for his life.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.ArmsDealer + "] got really impressed when he discovered I was into guns. But I had to end the chatting as soon as possible.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
            {
                Mes.Add("*I asked [nn:" + Terraria.ID.NPCID.Nurse + "] if she needed some assistance. She looked upset, so I left.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
            {
                Mes.Add("*Would you mind help me? I visitted [nn:" + Terraria.ID.NPCID.Clothier + "] earlier this day, to see if he could make me a cloak, and he ran away. May you help me explain?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
            {
                Mes.Add("*Only [nn:" + Terraria.ID.NPCID.Dryad + "] seems to be the only person who's not scared of me.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*I can't really get angry at [nn:" + Terraria.ID.NPCID.Angler + "], having him lost his parents and living all alone must have changed his behavior.*");
            }

            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*I kind of sympatize with [gn:" + CompanionDB.Rococo + "], his spirit is of a child.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*Sometimes I go along on [gn:" + CompanionDB.Sardine + "]'s adventures. It helps me saving many souls.*");
                if(CanTalkAboutCompanion(CompanionDB.Bree))
                    Mes.Add("*I think [gn:" + CompanionDB.Bree + "] doesn't actually trust me, she always panics when she sees me with [gn:"+CompanionDB.Sardine+"].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("*[gn:"+CompanionDB.Alex+"] asked me if I carried his old owner to their destination. I only came to this world recently, so maybe another reaper did.*");
                Mes.Add("*I like how [gn:"+CompanionDB.Alex+"] is such a good dog. "+AlexRecruitmentScript.AlexOldPartner+" have done one fine job.*");
                Mes.Add("*This will sound grim, but [gn:"+CompanionDB.Alex+"] told me to bring him to "+AlexRecruitmentScript.AlexOldPartner+"'s resting place when he dies. I can do that but.. I would need to find out where did she went to.*");
            }
            bool HasZacks = CanTalkAboutCompanion(CompanionDB.Zacks), HasFluffles = CanTalkAboutCompanion(CompanionDB.Fluffles);
            if (HasZacks && CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*[gn:"+CompanionDB.Blue+"] confronted me earlier, wanting me not to take [gn:"+CompanionDB.Zacks+"] to their destination. I replied to her that even if I wanted, I couldn't due to his curse.*");
            }
            if(HasZacks)
            {
                Mes.Add("*I can't take [gn:"+CompanionDB.Zacks+"] to their destination due to the curse laid on him. Until that curse is lifted, I can't do anything. Beside I think he may still be willing to help you, so maybe I can open an exception.*");
                Mes.Add("*The curse [gn:"+CompanionDB.Zacks+"] carries is different from the ones the zombies in this land carries. That explains why he regained consciousness.*");
            }
            if (HasFluffles)
            {
                Mes.Add("*[gn:"+CompanionDB.Fluffles+"] soul is troubled by what happened to her old group. And she seems to have such a strong bond towards you, so I can't take her to her destination.*");
                Mes.Add("*What happened to [gn:"+CompanionDB.Fluffles+"] caused on her some kind of post traumatic stress. Maybe time will make her recover from that.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*I get so bored when [gn:"+CompanionDB.Leopold+"] asks me about the life after the life and about death.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*How many times should I say that I won't... Oh, I thought It was [gn:"+CompanionDB.Malisha+"].*");
                Mes.Add("*I sense death at [gn:"+CompanionDB.Malisha+"]'s house.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*I can't stop staring at [gn:"+CompanionDB.Mabel+"]. Wait, I have a job to do, so I can't distract myself!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*Out of everyone in this world, [gn:"+CompanionDB.Vladimir+"] is the only one who seems to want to be my friend.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*Even though I don't have a digestive system, or the need to eat, I love eating food. Gladly [gn:" + CompanionDB.Minerva + "] is such a good cook.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                bool HasBreeMet = HasCompanion(CompanionDB.Bree), HasSardineMet = HasCompanion(CompanionDB.Sardine);
                if (HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*I'm glad to see that [gn:"+CompanionDB.Glenn+"] found his parents safe and sound. It would be awful if he ended up orphan.*");
                }
                else if (!HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + CompanionDB.Glenn + "] still didn't find his mother. You should try not to think of what could have happened to her if you want to find her.*");
                }
                else if (HasBreeMet && !HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + CompanionDB.Glenn + "] still didn't find his father. You should try not to think of what could have happened to him if you want to find him.*");
                }
                else
                {
                    Mes.Add("*It seems like [gn:" + CompanionDB.Glenn + "] still didn't find any of his parents. You should try not to think of what could have happened if you want to find them.*");
                }
                Mes.Add("*That kid, [gn:"+CompanionDB.Glenn+"], is very curious and talkative. Anyways, It's not everyday one have a reaper as neighbor.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*I was watching [gn:" + CompanionDB.Cinnamon + "] earlier, and spooked her to death... Not literally, but she ran away so fast when she noticed. She should really not get scared whenever she sees me watching her.*");
                Mes.Add("*[gn:" + CompanionDB.Cinnamon + "]... *");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*Reaping also involves physical exercise. [gn:"+CompanionDB.Miguel+"]'s pondering is really stupid.*");
            }
            if (IsPlayerRoomMate())
            {
                Mes.Add("*In all my years of existence, I never had a room mate. I hope it's an interesting experience.*");
                Mes.Add("*I don't really sleep, so I guess I can instead watch for your safety during your sleep.*");
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Yes, I see that ghost. I can't remove It from you, but I can help killing what killed her to free you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Do you think the people here will eventually stop being scared of me?*");
            Mes.Add("*I don't know which moment I like more, the moment I was alive, or after death.*");
            Mes.Add("*I have been around many realms for so long, doing my job. This is the first time I actually laid down somewhere and interacted with living people.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch (context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*There is nothing I look for right now.*";
                    return "*I don't think there is something I want at this moment.*";
                case RequestContext.HasRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*There's actually one thing you can help me with, if you are interessed... I need you to [objective].*";
                    return "*I've been so busy with my job, that I forgot to take care of a thing... Gladly, you can do It instead, if you agree to. It's to [objective], what do you say?*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*You have my deepest gratitude, [nickname].*";
                    return "*You have a kind soul.*";
                case RequestContext.Accepted:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I'm glad you accepted. I will be waiting here.*";
                    return "*Thank you. Please be safe while doing my request.*";
                case RequestContext.TooManyRequests:
                    return "*Stress actually can kill a person too, take care of the other requests you have before taking mine.*";
                case RequestContext.Rejected:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*It's fine. I can try doing that later, anyways.*";
                    return "*Hm... I also thought that the request wasn't very doable.*";
                case RequestContext.PostponeRequest:
                    return "*Got some more important matters? Okay.*";
                case RequestContext.Failed:
                    return "*I really didn't liked the result of that...*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*I predict that you did my request.*";
                case RequestContext.RemindObjective:
                    return "*Your task is to [objective]. That is it.*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*The request is too much for you? If you want I can relieve you from that.*";
                case RequestContext.CancelRequestYes:
                    return "*Very well. I'll carry on my request then.*";
                case RequestContext.CancelRequestNo:
                    return "*Alright. If you change your mind, or anything change related to my request, come see me.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch (context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Curious about me? Or is it about life in general? I don't know if I can answer your questions.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Does any other question troubles your soul, [nickname]?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*No more doubts? Okay. Want to talk about something else?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.RevivingMessage:
                    if (Main.rand.Next(2) == 0)
                        return "*I shall not allow this.*";
                    return "*Your time is not now.*";
                case ReviveContext.HelpCallReceived:
                    return "*Your time didn't came yet, I'll help ensure that.*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() <= 0.5f)
                        return "You're a kind soul. Thank you.";
                    return "*I apreciate the help.*";
                case ReviveContext.RevivedByItself:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I always return.*";
                    return "*You can't get rid of me for long.*";
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*I'll intervene.*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Not now, I'll assure that.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*You're asking me if you want to be my buddy? [nickname], you do know that I'm a reaper, and a lifetime deal wont be a thing, right?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*You really want me to be your buddy? Fine. I will be your buddy, even after your life ceases.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*I really wondered you would deny after this piece of information.*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch (context)
            {
                case JoinMessageContext.Success:
                    return "*I don't mind joining your group. That will reduce the amount of stray souls in the world.*";
                case JoinMessageContext.FullParty:
                    return "*...No... It's way too crowded for me right now.*";
                case JoinMessageContext.Fail:
                    return "*Sorry, but I preffer to stay here for now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*I don't mind leaving the group here, but would be better if I leaved the group on a safe place.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I'll try returning to the village, then. Safe travels, [nickname].*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Sorry, but now is the not the best moment for that.*";
                case LeaveMessageContext.Success:
                    return "*I'll return to my house, then. Safe travels, [nickname].*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Is he asleep...? Let me try...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*This smell... What is this...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*My heart is pounding so fast... I need to calm down...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*Whew... It's done... And reeeeally scary too!*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Yikes!! So scary!*";

                case MessageIDs.LeopoldMessage1:
                    return "*We're not slaves.*";
                case MessageIDs.LeopoldMessage2:
                    return "*Oh my... The Terrarian even enslaved a Reaper! Things are more serious than I thought!*";
                case MessageIDs.LeopoldMessage3:
                    return "*I already said that we're not slaves. We're aiding the Terrarian on their quest.*";

                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I don't think even the souls in my body believe in what I see.*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}