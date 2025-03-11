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
                    Mes.Add("**");
                }
            }
        }
        return Mes[Main.rand.Next(Mes.Count)];
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