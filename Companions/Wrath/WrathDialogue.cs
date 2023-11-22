using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class WrathDialogue : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm furious, why I'm furious? I don't know! THAT PISSES ME OFF!!!*");
            Mes.Add("*Agh!GRRRRR!! UUGGGHHHHH!*");
            Mes.Add("*Who are you?! What?! Something's funny with my face?! Want to taste my fist?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*UGHHH! what is there to talk about?!*");
            Mes.Add("*Stay away! I'm not in the mood! I never am!*");
            Mes.Add("*No talking, only smashing!*");
            Mes.Add("*What do you want?!!!*");
            Mes.Add("*Just looking at things aggravates me, I want to demolish!*");
            bool CloudForm = (companion.Data as PigGuardianFragmentPiece.PigGuardianFragmentData).IsCloudForm;
            if (CloudForm)
            {
                Mes.Add("*Don't joke about my this form!*");
                Mes.Add("*Being intangible weakens me! I need a more solid form to pound people harder!*");
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Perfect! more faces to pound!*");
                    Mes.Add("*BRING IT ON! MY HANDS ARE RATED E FOR EVERYONE!!!*");
                }
                else
                {
                    Mes.Add("*That lush green grass..... and the bird chirping sounds..... IS DRIVING ME NUTS!!!*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*TONIGHTS MENU?! UNDEAD BUTT CHEEKS!!!*");
                    Mes.Add("*More undead skulls to bash!*");
                }
                else
                {
                    Mes.Add("*Urgh! All those \"Grahs\" during the night are infuriating me! I'm about to go outside and kick their undead asses!*");
                }
            }
            if (companion.IsUsingToilet)
            {
                Mes.Add("*Don't you know privacy! Go away! Im taking a dump here!*");
                Mes.Add("*Want me to put your flush your head in the toilet?! GO AWAY!*");
            }
            if (Main.raining)
            {
                Mes.Add("*Great!, It couldn't get worse could it?!, now I have to be annoyed by rain drops!*");
                Mes.Add("*THE SPLASH NOISES ARE INFURIATING!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Alex))
                Mes.Add("*GO AWAY!. Pftt, I thought It was [gn:"+CompanionDB.Alex+"] wanting to play.*");
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
                Mes.Add("Looking at [gn:"+CompanionDB.Brutus+"] try to be tuff makes me want to beat him to a pulp!");
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("How many times do I have to tell [gn:"+CompanionDB.Malisha+"] that.. I'M NOT GOING TO PARTICIPATE IN ANY OF YOUR GOD DANM EXPERIMENTS!!!.");
            }
            /*if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
                    Mes.Add("*The bunny may know something about me that I dont?! Thats maddening!*");
                else
                    Mes.Add("*Tell the white bunny to help me change forms now!*");
            }*/
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*[gn:" + CompanionDB.Sardine + "] is the only reason I haven't pounded anyone here since he keeps me busy with fighting monsters.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("*If I ever see [gn:" + CompanionDB.Domino + "] making another joke about me, I'll turn his other eye into a sunny side up egg!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*I dont care how big [gn:"+CompanionDB.Vladimir+"] is, I'll pummel his fat ass until he becomes a malnourished bear!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                if (CloudForm)
                    Mes.Add("*I dont care if I look like a ghost! just dont compare me to [gn:" + CompanionDB.Fluffles + "]!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fear))
            {
                Mes.Add("*[gn:" + CompanionDB.Fear + "] knows what to do when I'm around, to just stay out of my way! Because Its so annoying when he screams like a little bitch!*");
            }
            if (IsPlayerRoomMate())
            {
                Mes.Add("*WHY DO I HAVE A FUCKING ROOM MATE!!! Don't try anything unless you want your ASS BEAT!*");
                if(CloudForm)
                    Mes.Add("*Just because im intagible does not mean we can share beds.... MORON!*");
            }
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Does It look like I'm the same as her?! I'm actually alive!*");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I tend to rage a lot. Stay out of my way before you become a victim!*");
            Mes.Add("*I still don't remember anything from before I woke up....uugghhh!!!.  who was I before??!! it really pisses me off NOT KNOWING!!!! GRRRRRR!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default: return "(It growls while sleeping, as if was going to bite someone.)";
                        case 1: return "(It doesn't seems to be having a very peaceful sleep, because of the constant movements it does.)";
                        case 2: return "(It looks like they are fighting against someone in their sleep.)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "*You got the nerve of waking me up in the middle of the night! Do you want your shit kicked in?!!!*";
                        case 1:
                            return "*You really woke me up!? are you trying to get your ass kicked!*";
                        case 2:
                            return "*Grrr... I already have trouble sleeping and you decided to wake me up?!*";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*You did the request correct?! TELL ME!.... because If not im going on a RAMPAGE!*";
                        case 1:
                            return "*You better have completed my request, because im very close to smashing stuff.*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*You want to take me on a adventure?! Who cares! just let me destroy things!*";
                case JoinMessageContext.FullParty:
                    return "*There's too many people! I hate mobs! This makes me aggravated!!!*";
                case JoinMessageContext.Fail:
                    return "*Not now!*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*You're crazy?! You plan on leaving me here?!*";
                case LeaveMessageContext.Success:
                    return "*Grr... I dont care!*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Don't ask me to join back!...*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Yeah I thought so!*";
                case LeaveMessageContext.Fail:
                    return "*I'm not leaving now!*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.HasRequest:
                    if(Main.rand.Next(2) == 0)
                        return "*GGGRRRRRR! I'm too mad for this! You should do it instead! Just [objective]!*";
                    return "*Grrr!! There is something I should do that is making me angry, but I can't do it myself! Do this [objective]!*";
                case RequestContext.NoRequest:
                    if(Main.rand.Next(2) == 0)
                        return "*NO!*";
                    return "*I don't! There isn't anything!*";
                case RequestContext.Completed:
                    if(Main.rand.Next(2) == 0)
                        return "*I will direct my animosity else where for now.*";
                    return "*I wont rage you for a few hours, is that a good enough reward?!*";
                case RequestContext.Accepted:
                    return "*Hurry up!*";
                case RequestContext.TooManyRequests:
                    return "*No more. Go deal with your other requests first!*";
                case RequestContext.Rejected:
                    return "*Fine! I'll do It myself as always!*";
                case RequestContext.PostponeRequest:
                    return "*What?! But I wanted It now!*";
                case RequestContext.Failed:
                    return "*WHAT? SOME BODY'S ASS IS ABOUT TO GET KICKED! *";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*What?! You did my request?!*";
                case RequestContext.RemindObjective:
                    return "*You what?! Fine! Here it goes! [objective]! That is it! Need me to nail that in your head?!*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You TWAT! WHY DID YOU DECLINE?!*";
                case RequestContext.CancelRequestYes:
                    return "*Urgh... Whatever then... I'll do it myself. No thanks to you, of course!*";
                case RequestContext.CancelRequestNo:
                    return "*So you just wanted to irritate me?!*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*WHAT ELSE DO YOU WANT?! Grr...*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Grrr... whatever. Stop annoying me!*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Finally! Move on!*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*FINE! I'M COMING! STOP WHINING!!*";
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*What!? Your too weak to last in battle!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Wake up now! before I put you to sleep permenanetly!*";
                case ReviveContext.RevivingMessage:
                {
                    List<string> Mes = new List<string>();
                    if (target is not Companion && target == companion.Owner)
                    {
                        Mes.Add("*Get up now!*");
                        Mes.Add("*I didn't agree to baby sit you!*");
                        Mes.Add("*Rise up now or I'll give you a worse fate then death!*");
                    }
                    else
                    {
                        Mes.Add("*Hey, get up before I bash your head in more!*");
                        Mes.Add("*I hope you aren't acting like this on purpose, because if you are your as good as dead.*");
                        Mes.Add("*This is already making me mad!*");
                    }
                    return Mes[Main.rand.Next(Mes.Count)];
                }
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*OK IM FINE! GET OFF ME!*";
                    return "*IM STILL FURIOUS!*";
                case ReviveContext.RevivedByItself:
                    return "*I HATE YOU ALL! I didn't need your help anyway!*";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
