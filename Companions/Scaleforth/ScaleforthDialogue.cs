using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;

namespace terraguardians.Companions.Scaleforth;

public class ScaleforthDialogue : CompanionDialogueContainer
{
    int RequestIndex = -1;

    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. I'm [name]. Are you looking for a butler?*";
            case 1:
                return "*Hi. I'm [name], a butler. I'm currently unemployed, if you're interested.*";
            case 2:
                return "*A new face. I'm [name]. Butler. If you need my services, let meknow.*";
        }
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        if (companion.IsUsingToilet)
        {
            Mes.Add("*I can't do much while I'm busy here. Sorry.*");
            Mes.Add("*This toilet is too small for what is coming to it..*");
            Mes.Add("*I believe you might want to ask me to do something for you now, but I'm a bit busy with 'depositing'.*");
        }
        else
        {
            Mes.Add("*The scenery of this place feels like a fairy tale. All that's left now is a princess and an abandoned castle."+(NPC.AnyNPCs(NPCID.Princess) ? "\nOh, yes. We got a princess here." : "")+"*");
            Mes.Add("*My old master ended up dying of old age. Since his death, I've been roaming through the Ether Realm, until I found a portal, and suddenly got here after passing through it. Funny enough, my clothes were destroyed when I passed through.*");
            Mes.Add("*If you need my services, you can have me accompany you on your journeys. I can take care of feeding your group and also reporting requests.*");
            Mes.Add("*You've never seen a TerraGuardian like me before? I haven't either.*");

            bool RoomMate = IsPlayerRoomMate();

            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*I wish it was a bright day, not a dark eclipse.*");
                    Mes.Add("*I saw some of those creatures in the books my old master used to asked me to read.*");
                }
                else if (!Main.raining)
                {
                    Mes.Add("*It's a fine day, isn't it?*");
                    Mes.Add("*Did you also came to admire the clouds, [nickname]?*");
                }
                else
                {
                    Mes.Add("*Well... At least I can still look at the clouds...*");
                    Mes.Add("*Say... Does my horns turn me into a giant lightning rod..?*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*I don't know what is more hostile tonight: The monsters outside or the women inside. Geez, I just asked if they were fine.*");
                    Mes.Add("*Why the moon sometimes turns red? And why monsters get more hostile when that happens? You don't know either..?*");
                }
                else
                {
                    if (RoomMate)
                    {
                        Mes.Add("*I already cleaned our bedroom, so you should sleep well tonight.*");
                    }
                    if (!Main.raining)
                    {
                        Mes.Add("*Did you see that? Only I did? Hm...*");
                        Mes.Add("*The night always reminds me that this day is nearly over, and that tomorrow will be another day. Are you ready for it?*");
                    }
                    else
                    {
                        Mes.Add("*Need me to prepare a hot chocolate for you to drink?*");
                        Mes.Add("*I can tend your bedroom, so you can enjoy the night of sleep with fresh bed sheet and pillows.*");
                    }
                }
                if (RoomMate)
                {
                    Mes.Add("*Never in all my time working as a butler, I had the one I serve share their bedroom with me.*");
                    Mes.Add("*One of the masters I worked for, used to lock her bedroom when she went sleep. Before firing me, she revealed that feared I would invade her bedroom and devour her whole. What she thinks I am? Some savage creature? Good thing that you're not like her.*");
                    Mes.Add("*Am I snoring during sleep? I get quite worried that I might be disturbing your slumber.*");
                }
                if (IsPlayerBuddy())
                {
                    Mes.Add("*Being your Buddy means I'm your butler until the rest of our lives, right?*");
                    Mes.Add("*I feel a bit awkward about being picked as a Buddy, but I try to keep my composure.*");
                }
                if(HasCompanion(CompanionDB.Alex))
                {
                    Mes.Add("*[gn:"+CompanionDB.Alex+"] is such a nice dog. Is he yours?*");
                }
                if (HasCompanion(CompanionDB.Brutus))
                {
                    Mes.Add("*[gn:"+CompanionDB.Brutus+"] keep picking on me just because I'm a butler.*");
                    Mes.Add("*Not so loud... My head hurts... I shouldn't drink too much with [gn:"+CompanionDB.Brutus+"]..*");
                }
                if (HasCompanion(CompanionDB.Bree))
                {
                    Mes.Add("*I think [gn:"+CompanionDB.Bree+"] want to hire me, but she seems reluctant to ask.*");
                    if (HasCompanion(CompanionDB.Sardine))
                    {
                        Mes.Add("*I haven't seen such a noisy couple like [gn:"+CompanionDB.Sardine+"] and [gn:"+CompanionDB.Bree+"] since the second house I worked on...*");
                    }
                }
                if (HasCompanion(CompanionDB.Cinnamon))
                {
                    Mes.Add("*Everytime [gn:"+CompanionDB.Cinnamon+"] ask me to taste food she makes, I have to drink a entire gallon of water afterwards.*");
                    Mes.Add("*I think [gn:"+CompanionDB.Cinnamon+"] has the potential to be a good cook when she grows up. She only needs to master taking it easy on the seasoning.*");
                }
                if (HasCompanion(CompanionDB.Domino))
                {
                    Mes.Add("*Isn't what [gn:"+CompanionDB.Domino+"] sells supposed to be illegal? At least on the Ether Realm..?*");
                    Mes.Add("*No, I haven't been bartering with [gn:"+CompanionDB.Domino+"], I really haven- Oh. You weren't going to ask about... Nevermind.*");
                }
                if (HasCompanion(CompanionDB.Wrath))
                {
                    Mes.Add("*I'd love to be able to not see [gn:"+CompanionDB.Wrath+"]'s face. He's extremely rude and hostile to anyone.*");
                }
                if (HasCompanion(CompanionDB.Fluffles))
                {
                    Mes.Add("*Why this world is haunted by such a nice ghost?*");
                    Mes.Add("*Do you know why [gn:"+CompanionDB.Fluffles+"] can't speak? I'm just curious about that.*");
                }
                if (HasCompanion(CompanionDB.Green))
                {
                    Mes.Add("*I'm pretty sure [gn:"+CompanionDB.Green+"] wont be very impressed independing on what ailment or injury you go see him with.*");
                }
                if (HasCompanion(CompanionDB.Leona))
                {
                    Mes.Add("*I were having [gn:"+CompanionDB.Leona+"] visit me on my lunch times. She seems interested in what I have to say.*");
                }
                if (HasCompanion(CompanionDB.Leopold))
                {
                    Mes.Add("*Why does seeing [gn:"+CompanionDB.Leopold+"] makes me want to eat a nice bowl of bunny stew..?*");
                }
                if (HasCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("*Having [gn:"+CompanionDB.Mabel+"] around distracts me. I ended up doing a few mistakes earlier because of that.*");
                    Mes.Add("*I can't stop dreaming with roasted venison...*");
                }
                if (HasCompanion(CompanionDB.Malisha))
                {
                    Mes.Add("*The subtle grins [gn:"+CompanionDB.Malisha+"] delivers to me makes me want to keep distance from her.*");
                }
                if (HasCompanion(CompanionDB.Miguel))
                {
                    Mes.Add("*[gn:"+CompanionDB.Miguel+"] want to know how I'm fit. Doesn't butlers get fit from doing their jobs daily?*");
                }
                if (HasCompanion(CompanionDB.Vladimir))
                {
                    Mes.Add("*[gn:"+CompanionDB.Vladimir+"] is one weird person. The only thing he thinks of is delivering hugs.*");
                    Mes.Add("*I was speaking earlier about [gn:"+CompanionDB.Vladimir+"]'s little brother. He wonders how's he now adays.*");
                }
            }
        }
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string RequestMessages(Companion companion, RequestContext context)
    {
        switch (context)
        {
            case RequestContext.NoRequest:
                return "*Hm? No, I don't need anything right now.*";
            case RequestContext.HasRequest:
                return "*Now that you mentioned, I needed something done.. [objective] is it. Could you do that for me instead?*";
            case RequestContext.Accepted:
                return "*Great. I'll wait for your return.*";
            case RequestContext.Rejected:
                return "*I see. It was a silly request anyways.*";
            case RequestContext.PostponeRequest:
                return "*Yes, I can leave it on hold or now.*";
            case RequestContext.CancelRequestAskIfSure:
                return "*You want to cancel my request, then?*";
            case RequestContext.CancelRequestYes:
                return "*Alright. You don't need to bother about that anymore.*";
            case RequestContext.CancelRequestNo:
                return "*I have no idea why you brought that up, but okay.*";
            case RequestContext.AskIfRequestIsCompleted:
                return "*You've got news about my request?*";
            case RequestContext.Completed:
                return "*Thank you. You helped me a lot with that.*";
        }
        return base.RequestMessages(companion, context);
    }

    public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
    {
        switch (context)
        {
            case BuddiesModeContext.AskIfPlayerIsSure:
                return "*Uh? Sorry, I thought you asked me if I wanted to be your Buddy. I mean... Why would you want to be a Buddy of a butler? Right..? Are.. Are you serious..?*";
            case BuddiesModeContext.PlayerSaysYes:
                return "*Y-you are?! I... Yes, I accept. I shall be your Butler for the rest of our lives! I.. Sorry, I'm not used to this.... Thanks.*";
            case BuddiesModeContext.PlayerSaysNo:
                return "*Yes, I knew it. That was a good one..*";
            case BuddiesModeContext.NotFriendsEnough:
                return "*That's such a random thing you asked me for, [nickname]. Beside I hardly even know you yet for such a thing.*";
            case BuddiesModeContext.Failed:
                return "*Not now, [nickname]...*";
            case BuddiesModeContext.AlreadyHasBuddy:
                return "*But [nickname], you've got a buddy already. I can even see the connecting bond on you.*";
        }
        return base.BuddiesModeMessage(companion, context);
    }

    public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
    {
        dialogue.AddOption("How a butler could help and adventurer?", AskHowHeCanHelp);
    }

    void AskHowHeCanHelp()
    {
        MessageDialogue md = new MessageDialogue("*I can help in many ways. If I have food on my inventory while travelling with you, I can serve it to your party whenever you get hungry.\nI can also help reporting completed requests to their requesters, so you don't need to go talk with them.*");
        md.AddOption("That's handy.", Dialogue.LobbyDialogue);
        md.RunDialogue();
    }

    public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
    {
        dialogue.AddOption("Report a request for me.", OnAskToReportRequest);
    }

    void OnAskToReportRequest()
    {
        MessageDialogue md = new MessageDialogue("*Which request should I report?*");
        PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
        RequestData[] rds = pm.GetActiveRequests;
        for (int i = 0; i < rds.Length; i++)
        {
            int index = i;
            if (rds[i] != null && rds[i].IsRequestCompleted())
            {
                md.AddOption(rds[i].GetRequestGiver.GetNameColored() + "'s request.", delegate() { DoReportRequest(index); });
            }
        }
        md.AddOption("Nevermind", CancelReportRequest);
        md.RunDialogue();
    }

    void CancelReportRequest()
    {
        Dialogue.LobbyDialogue("*Let me know if you got any request to report.*");
    }

    void DoReportRequest(int RequestIndex)
    {
        PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
        RequestData request = pm.GetActiveRequests[RequestIndex];
        if (request == null)
        {
            OnRequestTimedOut();
        }
        else
        {
            this.RequestIndex = RequestIndex;
            MessageDialogue md = new MessageDialogue("*Yes, looks like you managed to complete "+request.GetRequestGiver.GetNameColored()+" request. Pick one of those rewards.*");
            request.GiveAtLeast30SecondsRequestTime();
            for (byte i = 0; i < 3; i++)
            {
                byte index = i;
                md.AddOption(request.GetRequestRewardInfo(i), delegate() { DoCompleteReportedRequest(index); });
            }
            md.RunDialogue();
        }
    }

    void DoCompleteReportedRequest(byte RewardIndex)
    {
        PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
        RequestData request = pm.GetActiveRequests[RequestIndex];
        if (request == null)
        {
            OnRequestTimedOut();
        }
        else
        {
            if (request.CompleteRequest(pm.Player, request.GetRequestGiver, RewardIndex))
            {
                MessageDialogue md = new MessageDialogue("*The requester is happy that you completed their request.\nI hope you like your reward.*");
                md.AddOption("Let me report another request.", OnAskToReportRequest);
                md.AddOption("That was all.", Dialogue.LobbyDialogue);
                md.RunDialogue();
                pm.UpdateActiveRequests();
            }
        }
    }

    void OnRequestTimedOut()
    {
        MessageDialogue md = new MessageDialogue("*Looks like this request time is up. Sorry.*");
        md.AddOption("Oh...", CancelReportRequest);
        md.RunDialogue();
    }
}