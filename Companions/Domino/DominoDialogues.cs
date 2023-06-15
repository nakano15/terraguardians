using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class DominoDialogues : CompanionDialogueContainer
    {
        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*I stayed for too long on this shop, I need to have some walk. Say, would you mind If I travelled with you?*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*I don't mind having some supportive fire. If you feel like helping, I can let you mount on my shoulder.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*If there's some place you can't go by yourself, just let me go instead.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GreetMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What? Interessed in my seeing my wares?*");
            Mes.Add("*Check out what I have to sell, I'm sure something will catch your eyes.*");
            Mes.Add("*Good, another customer. Want to look at my wares?*");
            if(HasCompanionSummoned(CompanionDB.Brutus))
                Mes.Add("*Hello "+PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Brutus).GetNameColored()+", missed me?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I have guns for sale, If you want some.*");
            Mes.Add("*None of my goods are stolen.. Probably.*");
            Mes.Add("*I've always wanted to have a shop sign. It would attract the attention of guards too, so I never even tried to. Well... I tried once. You know how It ended, though.*");
            Mes.Add("*Check this out, fresh from the Ether Realm.*");
            Mes.Add("*I like this place more than the Ether Realm, I don't need to hide, so I have more clients.*");
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*I think I have enough ammo for a day long combat.*");
                    Mes.Add("*I think I have stored away a magazine that has one of those monsters, I just need to find it.*");
                }
                else
                {
                    Mes.Add("*Ugh. Bunnies. Squirrels. Butterflies. They makes me want to grease the floor with vomit.*");
                    Mes.Add("*I'm not used to days. Can you snap your fingers to make the night come?*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*I think I have something for your female citizens. It's right over here...*");
                    Mes.Add("*I was planning on sleep this night. I guess I'll discount my frustration on the monsters outside.*");
                    Mes.Add("*Well, It could have been worse. Right?*");
                }
                else
                {
                    Mes.Add("*So peaceful... Makes me want to take a nap.*");
                    Mes.Add("*What can I sell to you on this beautiful night?*");
                }
            }
            if (Main.raining)
            {
                Mes.Add("*I wasn't planning on going outside anyway...*");
                Mes.Add("*Rwatchooo~! Ugh, I hate this weather...*");
                Mes.Add("*Yes, we can trade. Just don't stay directly in front of my snout.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*[gn:"+CompanionDB.Blue+"] asked me If I had something to help her hair grow. She said looooots of nasty things about me when I gave her chlorophyle.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*What? How dare you?! I would never sell catnip to [gn:"+CompanionDB.Sardine+"]!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Zacks))
            {
                Mes.Add("*I can't understand [gn:" + CompanionDB.Zacks + "]. He got all angry when I tried to sell bandage gauze to him. At least he would be dressed for halloween.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*I wonder what is inside [gn:" + CompanionDB.Bree + "]'s bag. Can it be sold for a good price?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*[gn:" + CompanionDB.Mabel + "] bought some orthopedic underwears earlier, but I wonder what for. I never saw her using any kind of clothing.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*[gn:" + CompanionDB.Brutus + "] has tried to arrest me several times in the Ether Realm, but I always managed to escape, because I'm smarter than him.*");
                Mes.Add("*Sometimes [gn:"+CompanionDB.Brutus+"] comes to me, threatening to arrest me for doing shady deals. I have to keep remembering that there are no laws here, neither he's a guard.*");
                Mes.Add("*[gn:"+CompanionDB.Brutus+"] said that will be keeping a closer eyes on me, waiting for me to do something wrong. I solved that by giving him some magazines.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*What?! I never got hugged by [gn:" + CompanionDB.Vladimir + "], I don't know why he would say that.*");
                Mes.Add("*I feel like I've lost some weight on my shoulder. Maybe I could try smiling? No.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*You wont believe when I say. I asked [gn:"+CompanionDB.Minerva+"] if she could give me something that would be of my taste. She gave me a bone... She's really the best at guessing what people want.*");
                Mes.Add("*What? Never saw a bone before? I like chewing them sometimes. Gladly [gn:"+CompanionDB.Minerva+"] has some stocks of them.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*Between you and [gn:" + CompanionDB.Michelle + "] around, I preffer you. At least you don't bother me all the time.*");
            }
            /*if (HasCompanionSummoned(CompanionDB.Wrath) && player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Companions.PigGuardianFragmentBase.AngerPigGuardianID])
            {
                Mes.Add("*Hey [nickname], what have you been eating latelly? Because you seems to have released a \"" + PlayerMod.GetPlayerGuardian(player, Wrath).Name + "\", hahaha. Got It? Released a \"" + PlayerMod.GetPlayerGuardian(player, Wrath).Name + "\"?*");
            }*/
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*I supply [gn:"+CompanionDB.Green+"] with things he uses to heal people. Keep that in mind if you wonder why he charges you.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.TravellingMerchant + "] has a variety of low quality goods to offer.*");
                Mes.Add("*You wait for so long for [nn:" + Terraria.ID.NPCID.TravellingMerchant + "] to arrive, but what for?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*I think [nn:" + Terraria.ID.NPCID.ArmsDealer + "] is a good business partner. He has some goods that interests me, though.*");
                Mes.Add("*I tried giving [nn:" + Terraria.ID.NPCID.ArmsDealer + "] some dating tips. I'm expert at that. But I don't understand the red hand marking on his face earlier.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.DD2Bartender))
            {
                Mes.Add("*I'm off to have a few drinks later, want to join?*");
                Mes.Add("*So.. You let [nn:"+Terraria.ID.NPCID.DD2Bartender+"] join your town because of the Old One's Army invasion? I thought It was for the drinks.*");
            }
            if (companion.IsUsingToilet)
            {
                Mes.Add("*Uh... Couldn't you wait until I'm done? I'm doing some other kind of business here.*");
                Mes.Add("*Do you know what privacy is? Because I need some right now.*");
                Mes.Add("*I don't need hygienic paper at the moment, so unless you tried bringing me some, I don't see the reason why you should enter an occupied toilet.*");
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Alright, I can share my room with you, just don't try stealing my goods while I sleep.*");
                Mes.Add("*Maybe It's a bad idea having you inside my room, because I'm known for snoring, really loud.*");
            }*/
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I think I have the right thing to solve your ghost problem, but It will cost you a lot.*");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm glad there is no kind of law about selling weapons here, It's bad for business running from guards.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                {
                    List<string> Mes = new List<string>();
                    Mes.Add("*Nope. Nothing I want, at all.*");
                    Mes.Add("*I think I have everything I need right now.*");
                    return Mes[Main.rand.Next(Mes.Count)];
                }
                
                case RequestContext.HasRequest:
                {
                    List<string> Mes = new List<string>();
                    Mes.Add("*Not everything I can get done by myself, so I need your help with this. Can you [objective]?*");
                    Mes.Add("*I have to go get some goods soon from one of my sources, so I can't do this meanwhile: [objective]. Would you be able to do this for me?*");
                    if (CanTalkAboutCompanion(CompanionDB.Brutus))
                    {
                        Mes.Add("*I need to stay here, in case \"someone\" tries to confiscate my goods. So please can you [objective] for me?*");
                    }
                    return Mes[Main.rand.Next(Mes.Count)];
                }

                case RequestContext.Completed:
                {
                    List<string> Mes = new List<string>();
                    Mes.Add("*Here, take this as reward. Also, no refund.*");
                    Mes.Add("*You don't know how much I needed that done. Here some free things I wanted to charge you for.*");
                    return Mes[Main.rand.Next(Mes.Count)];
                }

                case RequestContext.Accepted:
                    return "*I had the feeling that you would accept. I'll have something prepared for you when you come back.*";

                case RequestContext.TooManyRequests:
                    return "*I value my request, so try getting rid of those other requests before you ask for mine.*";
                    
                case RequestContext.Rejected:
                    return "*You wont get paid for that.*";

                case RequestContext.PostponeRequest:
                    return "*It's not like as if It's a one time deal, anyway.*";

                case RequestContext.Failed:
                    return "*You failed? Well, I should ask you to pay me for that, but instead I'll give you nothing.*";

                case RequestContext.AskIfRequestIsCompleted:
                    return "*You've returned, so.. Did you do my little request?*";

                case RequestContext.RemindObjective:
                    return "*Hmph.. It's for you to [objective] what I asked you to. Did you forget it already?*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    if (CanTalkAboutCompanion(CompanionDB.Brutus) && Main.rand.Next(2) == 0)
                        return "*I don't think [gn:"+CompanionDB.Brutus+"] will like the idea, and that's more than enough reasons to accept.*";
                    return "*If you don't mind having a smuggler living on your neighborhood, then it's fine.*";
                case MoveInContext.Fail:
                    return "*Here? Now? No.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I don't think so. Why should I trust you?*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*Hmph. You give me a house, and then take it from me. And I'm the mercenary.*";
                case MoveOutContext.Fail:
                    return "*No. I'll be keeping the house.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*You're not the one who can tell me that.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*I guess I could do worse, but if you say so.*";
                case ChangeLeaderContext.Failed:
                    return "*I'd rather stay behind someone else, instead.*";
            }
            return base.ChangeLeaderMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*Lets see how good you are with guns.*";
                case ControlContext.SuccessReleaseControl:
                    return "*So good to control my own body again.*";
                case ControlContext.FailTakeControl:
                    return "*It simply isn't the moment for that.*";
                case ControlContext.FailReleaseControl:
                    return "*Better you stay in the control for a while longer.*";
                case ControlContext.NotFriendsEnough:
                    return "*Maybe if I were crazy, I would do that.*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Try not to get us killed.*";
                        case 1:
                            return "*It's good to have a moment to think about future business ideas.*";
                        case 2:
                            return "*What? Need some extra thoughts?*";
                    }
            }
            return base.ControlMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*Yes, but I'll take half of the loot.*";
                case JoinMessageContext.FullParty:
                    return "*I don't think you can carry more people right now.*";
                case JoinMessageContext.Fail:
                    return "*No, I preffer to stay here and make lucre selling my merchandise.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*You're planning on leaving me here, all alone, in the wilds?*";
                case LeaveMessageContext.Success:
                    return "*I think I got enough loot from our trip, [nickname]. I'll go back to my store.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Okay... Don't you come asking for discount if I find something cool to sell.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*I thought so. Let's go to a town before you decide to remove me from the team.*";
                case LeaveMessageContext.Fail:
                    return "*You wont get rid of me so easily.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Yeah, I could use an extra gun shooting at things. Hop on my shoulder.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*Well, at least I can rest my feet.*";
                case MountCompanionContext.Fail:
                    return "*Walk on your own feet for now.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*Don't you have feet?*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*Great. More fire power.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I like that idea. Who should I carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*Fine. That should stop my ear from ringing.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Well, time to use my own feet then.*";
                case DismountCompanionContext.Fail:
                    return "*Not right now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*Depends on what you ask of me.*";
                case InteractionMessageContext.Accepts:
                    return "*Yeah. I can do that.*";
                case InteractionMessageContext.Rejects:
                    return "*That's a bit too much.*";
                case InteractionMessageContext.Nevermind:
                    return "*Okay.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*If you say so. It's because I'm a bit chubby, right?*";
            return "*I'll find a bed for myself when I need to get some rest.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Fine..*";
            return "*Alright.*";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*Is that some kind of ultimate offer you're making me, [nickname]? The price is higher than I can pay in my life time. Are you sure want to pick me as Buddy?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Alright, I can be your buddy. Doesn't mean I will offer you discounts, though.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*I see.*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I hardly even know who is asking me that. How can I bind the rest of my life with someone I don't know?*";
                case BuddiesModeContext.Failed:
                    return "*I'd rather be buddies with a rock, instead.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*Don't you have a buddy already? They're following you, right?*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*I don't see what you think is wrong with my current tactic. What do you suggest?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*I already hate that idea, but I will do it.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Alright. That seems reasonable.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*I will keep my distance from danger then.*";
                case TacticsChangeContext.Nevermind:
                    return "*Was that just to mock me?*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*So, want to know more about me? Are you expecting discount for being my friend? Haha.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there something else you need to know?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Fine. Want to talk about something else now?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "(His snores are extremelly loud.)";
                        case 1:
                            return "(He seems to be counting something.)";
                        case 2:
                            return "*A few more crates... Here the change...* (He's doing deals in his dreams)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "*Huh? What? I snore very loud? Then go away.*";
                        case 1:
                            return "*[nickname], one of the things I value is a well slept day.*";
                        case 2:
                            return "*If you want to trade, couldn't you wait until I open my shop?*";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*Yes, I'm very happy for seeing you too, so happy that I'm grinning. Is it about the request or what?*";
                        case 1:
                            return "*I hope you woke me up to say that completed my request, because I was really busy trying to sleep.*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*Ah, you're in need of a smuggler. Gladly there's one going your way now. Just you wait.*";
                case InviteContext.SuccessNotInTime:
                    return "*Alright. I will be there by the night. I hope you're not sleeping when I arrive.*";
                case InviteContext.Failed:
                    return "*I have other things to keep me busy right now.*";
                case InviteContext.CancelInvite:
                    return "*What? Fine, I'm not coming then.*";
                case InviteContext.ArrivalMessage:
                    return "*Here I am. Did you need something of me, or wanted to check my merchandises?*";
            }
            return "";
        }
    }
}