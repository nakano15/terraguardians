using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using terraguardians;
using Terraria;
using Terraria.ID;
using Steamworks;
using terraguardians.Companions.Cotton;

namespace terraguardians.Companions;

public class CottonDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. Are you looking for a new friend? Here is one.*";
            case 1:
                return "*Hi. I'm [name]. And I don't mind having new friends in my life.*";
            case 2:
                return "*You are a Terrarian. Terrarian can have friends? I want to have you as my friend.*";
        }
        return base.GreetMessages(companion);
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        if (companion.IsUsingToilet)
        {
            Mes.Add("*I can clean myself afterwards, if that's what you came here for.*");
            Mes.Add("*Speaking with you at this moment is taking my concentration.*");
            Mes.Add("*This toilet is not going to explode, is it?*");
        }
        else if (Main.bloodMoon)
        {
            Mes.Add("*You'll protect me from those monsters, right? This night is really scary.*");
            Mes.Add("*Are you scared, [nickname]? Because I am. Very scared!*");
            Mes.Add("*Why the monsters get so scary during the red moon? And why is the moon red?*");
            Mes.Add("*I hope you don't mind if the floor here get all wet, right?*");
        }
        else
        {
            bool UsingPlushieSkin = companion.IsSkinActive(1);
            if (!UsingPlushieSkin)
            {
                Mes.Add("*You think I'm a stuffed toy? I have flesh and bones in me too.*");
                Mes.Add("*People like to call me \"Plushie Dog\", and I don't mind that.*");
                Mes.Add("*I don't squeak when my belly is pushed, but I will laugh if you tickle my belly.*");
                Mes.Add("*Why some people think that there's a Terrarian wearing me?*");
            }
            else
            {
                Mes.Add("*You think I'm a stuffed toy? I... Nevermind..*");
                Mes.Add("*People like to call me \"Plushie Dog\", that makes more sense now...*");
                Mes.Add("*I don't feel anythign when my belly is pushed, or tickled. Must be the cotton.*");
                Mes.Add("*Why some people think that there's a Terrarian wearing me? No! Don't pull the zipper!*");
            }
            Mes.Add("*Why do people like to hug me?*");

            Mes.Add("*How are you feeling, [nickname]? Everything alright?*");
            Mes.Add("*I always try to have lots of friends, so I don't end up being lonely.*");
            Mes.Add("*Do you know what people mean when they tell you they need some space? Am I asking them how they are too much?*");
            Mes.Add("*I used to have a rabbit plushie when I was a kid. I grew so used to it that standing like this ended up being a habit.*");

            if (Main.dayTime)
            {
                if (!Main.raining)
                {
                    Mes.Add("*It's a nice day, today.*");
                    Mes.Add("*I could burn some energy with this weather.*");
                }
                else
                {
                    Mes.Add("*This weather makes me a... A... A-CHO!!*");
                    Mes.Add("*Snif~ That rain is triggering my sinusitis...*");
                }
            }
            else
            {
                Mes.Add("*Yawn... I feel drowzy...*");
                Mes.Add("*You would not believe your eyes, if ten million fireflies, lit up the world as I fall asleep.*");
            }
            if (!NPC.downedBoss1)
            {
                Mes.Add("*I feel uncomfortable at night. I sense like something is watching me some times, and when I look around, I see a giant eye.*");
            }
            if (!NPC.downedBoss3)
            {
                Mes.Add("*I'm really curious about what is inside that dungeon, but the old man in front of it tells me to not enter.*");
            }
            if (NPC.AnyNPCs(NPCID.Merchant))
            {
                Mes.Add("*I like that some times [nn:" + NPCID.Merchant + "] has some bones for me. He's even nice to not charge me anything.*");
            }
            if (NPC.AnyNPCs(NPCID.Clothier))
            {
                Mes.Add("*[nn:" + NPCID.Clothier + "] asked me earlier who's my tailor... What tailor..? I got not clothing.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*It's hard for me to keep my conversations civil with [gn:" + CompanionDB.Rococo + "]. My toys are way better than the ones he has, and he knows it.*");
                Mes.Add("*Can you believe what [gn:" + CompanionDB.Rococo + "] said? How can there be anything better than squeaky toys?!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*[gn:" + CompanionDB.Brutus + "] ridicules me some times just because I don't drink. I simply preffer having some tasty juice to drink. At least I'll still be sober afterwards.*");
                Mes.Add("*Did you see [gn:" + CompanionDB.Brutus + "] passed out yesterday? Well... We had some drinks.. I feel really good about not drinking ale.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*Could you tell [gn:" + CompanionDB.Michelle + "] that I'm not a toy? Everytime she pushes my belly hurts.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Luna))
            {
                Mes.Add("*I understand that [gn:" + CompanionDB.Luna + "] is very knowledgeable about TerraGuardians. Speak with her if you have questions about us.*");
                if (NPC.AnyNPCs(NPCID.Guide))
                    Mes.Add("*I caught [gn:" + CompanionDB.Luna + "] asking [nn:" + NPCID.Guide + "] for tips on how to craft something with a item she got. Even she needs his help to figure out what to do with random junk.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*Looks like people also look for [gn:" + CompanionDB.Vladimir + "] for some hugs.*");
                Mes.Add("*[gn:" + CompanionDB.Vladimir + "] told me that at the time he appeared in the Terra Realm, people were in dire need of social contact. So he said he's happy to have tried to help.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Monica))
            {
                Mes.Add("*We got [gn:" + CompanionDB.Monica + "] here. Now where is Samson?*");
                if (!MonicaBase.IsSlimQuestCompleted)
                    Mes.Add("*[gn:" + CompanionDB.Monica + "] keeps saying that she want to be fit, but I don't see her reducing the amount of food she eats.*");
                else
                    Mes.Add("*Looks like [gn:"+CompanionDB.Monica+"] managed to actually lose weight. People do change, huh.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Zack))
            {
                Mes.Add("*Why do we have a Zombie walking among us? I avoid getting too close to him, he stinks.*");
            }
            /*if (CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("**");
            }*/
        }
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string RequestMessages(Companion companion, RequestContext context)
    {
        switch (context)
        {
            case RequestContext.NoRequest:
                return "*I don't. Maybe later I have something I could ask you.*";
            case RequestContext.HasRequest:
                return "*You're a life saver. I was in need of someone to [objective]. Would you be able to do that for me?*";
            case RequestContext.Accepted:
                return "*Thanks. I will await for your return. I will try controlling my anxiety meanwhile...*";
            case RequestContext.Rejected:
                return "*Yes. I wasn't wanting to do that either. Lets forget about it.*";
            case RequestContext.PostponeRequest:
                return "*Postpone it? Uh... Okay.*";
            case RequestContext.AskIfRequestIsCompleted:
                return "*Is my request completed? Please say yes...*";
            case RequestContext.Completed:
                return "*Is it? Woohoo! Now my anxiety levels will drop a lot. Thank you [nickname].*";
            case RequestContext.RemindObjective:
                return "*Oh, you forgot what I asked..? I... Uh... Yes, I had asked you to [objective]. There.*";
            case RequestContext.CancelRequestAskIfSure:
                return "*You can't do my request? Is it true that you want to drop it?*";
            case RequestContext.CancelRequestYes:
                return "*Aww... Now I'll feel bad all day for having you try to do that fools errand... Don't worry about the request anymore.*";
            case RequestContext.CancelRequestNo:
                return "*That was some kind of prank, right? Was a good one... My anxiety is still killing me though.*";
            case RequestContext.Failed:
                return "*Oh... You failed... Hm... Please... I'm sorry [nickname].. Don't mind me now...*";
        }
        return base.RequestMessages(companion, context);
    }

    public override string SleepingMessage(Companion companion, SleepingMessageContext context)
    {
        switch (context)
        {
            case SleepingMessageContext.WhenSleeping:
                switch (Main.rand.Next(3))
                {
                    default: return "(He's snoring gently.)";
                    case 1: return "(He looks happy. Makes me wonder what he is dreaming about.)";
                    case 2: if (Main.rand.Next(100) == 0) return "(He mentions your name in his sleep.)"; return "(He's mentioning someone's name in his sleep. Must be one of his friends?)";
                }
        }
        return base.SleepingMessage(companion, context);
    }

    public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
    {
        if (!companion.IsRunningBehavior)
        {
            dialogue.AddOption("Give me a hug.", GetHugDialogue);
        }
        else if (companion.GetGoverningBehavior() is CottonHugBehaviour)
        {
            dialogue.AddOption("Enough hug.", StopHugDialogue);
        }
    }

    void GetHugDialogue()
    {
        MessageDialogue md = new MessageDialogue("*I don't mind giving hugs. Let me know when to stop.*");
        Dialogue.Speaker.RunBehavior(new CottonHugBehaviour(MainMod.GetLocalPlayer));
        md.RunDialogue();
    }

    void StopHugDialogue()
    {
        Dialogue.Speaker.GetGoverningBehavior().Deactivate();
        MessageDialogue md = new MessageDialogue("*There. Feeling better now?*");
        md.RunDialogue();
    }
}