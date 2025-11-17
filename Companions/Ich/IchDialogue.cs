using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions.Ich;

public class IchDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. I am Ich. Ich I am.*";
            case 1:
                return "*You speak to Ich. Ich is me. Yes.*";
            case 2:
                return "*Ich I am? Ich I am!*";
        }
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        if (companion.IsUsingToilet)
        {
            Mes.Add("*I thought the smell would make you go away. Even I am unable to bear it for too long before flushing.*");
            Mes.Add("*No. I'm not telling you how many rows I've flushed.*");
        }
        else
        {
            Mes.Add("*Some people get confused when they hear my nickname.*");
            Mes.Add("*Why some people say I'm talking about myself twice?*");
            Mes.Add("*How do you like my nickname? I heard some people saying it, and I thought it would be funny.*");
            Mes.Add("*I love seeing so many birds around. They make a good game.*");
            Mes.Add("*There are two things I love the most: Fish and Birds. Even more when they're roasted.*");
            Mes.Add("*What nickname you think is cooler? [name] or [nickname]? I find [name] cooler.*");

            Mes.Add("*I wonder why some people ask why I speak about myself in third person?*");
            Mes.Add("*You're speaking to [name]. [name] is me.*");

            if (Main.raining)
            {
                Mes.Add("*Lovely... It had to rain... Ugh..*");
                Mes.Add("*Don't you have some machine to make rain go away? I'm just asking. Ok, not just asking.*");
            }

            if (!Main.dayTime)
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*Ah! It's good to see you. I have something important to tell you: HEEEEEELP!!*");
                    Mes.Add("*I thought those houses were safe! Aaahhh!!*");
                    Mes.Add("*Why there are more dead Terrarians around than alive?!*");
                }
                else
                {
                    Mes.Add("*I can't wait to find some window to lie on...*");
                    Mes.Add("*Fuel low... Need zzz charge...*");
                }
            }

            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*I have quite a partnership going on with [gn:" + CompanionDB.Rococo + "]. He fishes while I help him eat them.*");
                Mes.Add("*It's odd how [gn:" + CompanionDB.Rococo + "] doesn't speak like most TerraGuardians do. Is he made different?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("*Could you tell [gn:" + CompanionDB.Alex + "] to stop barking at me?*");
                Mes.Add("*I'm pretty sure that [gn:" + CompanionDB.Alex + "] doesn't like me.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*[gn:" + CompanionDB.Sardine + "]? What kind of name is [gn:" + CompanionDB.Sardine + "]?! Why he's named that?*");
                Mes.Add("*So... Tell me again how you found [gn:" + CompanionDB.Sardine + "]? It gets funnier each time I hear it.*");
                Mes.Add("*You're asking me why I'm bigger than [gn:" + CompanionDB.Sardine + "]? I ask you why [gn:" + CompanionDB.Sardine + "] is so small? He's a TerraGuardian too, right?*");
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    Mes.Add("*How is it possible that [gn:" + CompanionDB.Sardine + "] has a partner? While I don't have one! No! I'm not jealous!*");
                    Mes.Add("*What does [gn:" + CompanionDB.Bree + "] sees on [gn:" + CompanionDB.Sardine + "]? Like, there's probably better guys she could be with, like me. Right? Beside she's too small for my taste.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*Looks like [gn:" + CompanionDB.Bree + "] is so charismatic... At least looks like she didn't wanted to be here.*");
                Mes.Add("*Why [gn:" + CompanionDB.Bree + "] has about the same size as my knee?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("*It's nice to have [gn:" + CompanionDB.Domino + "] here. There are some things he sells that I can't buy in the Ether Realm.*");
                Mes.Add("*Can I tell you something, Pal? Never ask for refund to [gn:" + CompanionDB.Domino + "].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*Seeing [gn:" + CompanionDB.Mabel + "] makes me very... Uncomfortable... It even makes me unable to say 'Hi'.*");
                Mes.Add("*How did you met [gn:" + CompanionDB.Mabel + "]? She fell from the sky? Come on, seriously. How did you met her?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*If you see [gn:" + CompanionDB.Cinnamon + "], please tell her that you didn't saw me. The last time helped her taste one of her meals left me for hours sitting in a throne.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leona))
            {
                Mes.Add("*I keep trying not to stay directly in front of [gn:" + CompanionDB.Leona + "]. You never know when she will slice you with that big sword.*");
                Mes.Add("*Doesn't [gn:" + CompanionDB.Leona + "] get arm cramp from holding that sword all the time? Also, why don't she sheathe it? Actually, why she has no sheath for that sword?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*If you're coming to see me because [gn:" + CompanionDB.Malisha + "] asked to, then tell her that I still don't agree with \"disassembling\" me.*");
            }
        }
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
    {
        switch (context)
        {
            case JoinMessageContext.Success:
                return "*Sure I can join you. You have [name] with you now.*";
            case JoinMessageContext.Fail:
                return "*Not now, [nickname].*";
            case JoinMessageContext.FullParty:
                return "*There is no space for [name] there. Even if I try to squeeze into the group.*";
        }
        return base.JoinGroupMessages(companion, context);
    }

    public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
    {
        switch (context)
        {
            case LeaveMessageContext.Success:
                return "*Whew... My feet were getting sore. I don't mind leaving now.*";
            case LeaveMessageContext.Fail:
                return "*You're not getting rid of me that easily. Better find a better moment to drop me from the group.*";
            case LeaveMessageContext.AskIfSure:
                return "*You want me to leave the group right now? You'd do that to [name]? Better do that in a safer place, no?*";
            case LeaveMessageContext.DangerousPlaceYesAnswer:
                return "*Well... Better [name] run and scream all the way back home then...*";
            case LeaveMessageContext.DangerousPlaceNoAnswer:
                return "*[name] thought so. It's not cool to leave your friends in the wilderness. Right, friend?*";
        }
        return base.LeaveGroupMessages(companion, context);
    }

    public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
    {
        switch (context)
        {
            case BuddiesModeContext.AskIfPlayerIsSure:
                return "*Hahaha, very funny [nickname]. You asked [name] to be your buddy. That's some joke, right? Wait... Is that serious? Like, that's a thing that can't be undone so... Must be a joke, right?*";
            case BuddiesModeContext.PlayerSaysYes:
                return "*Wait, did [nickname] say yes? Haha... Haha... Ah... I... Can't contain the happiness. [name] never thought he would be buddy of someone. I... Think I might end up passing out.*";
            case BuddiesModeContext.PlayerSaysNo:
                return "*Yeah, good one [nickname]. Like seriously, who would like having [name] as their buddy? It must be just part of one big joke.*";
            case BuddiesModeContext.NotFriendsEnough:
                return "*Hahaha, very funny [nickname]. I never had heard that one before, but surely was funny.*";
            case BuddiesModeContext.Failed:
                return "*This is not the best moment for that, even if was a joke.*";
            case BuddiesModeContext.AlreadyHasBuddy:
                return "*You want me as your buddy? What about that one? I can see how strong the bond line between your hearts is.*";
        }
        return base.BuddiesModeMessage(companion, context);
    }
}