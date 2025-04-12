using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Scaleforth;

public class ScaleforthPreRecruitBehaviour : PreRecruitNoMonsterAggroBehavior
{
    bool FirstFrame = true, SpeakSecondTime = false, BedSpawn = false;
    Player SeenPlayer = null;
    
    public override void Update(Companion companion)
    {
        if (FirstFrame)
        {
            Point TilePosition = (companion.Bottom - Vector2.UnitY * 2).ToTileCoordinates();
            Tile tile = Main.tile[TilePosition.X, TilePosition.Y];
            if (tile != null && tile.TileType == Terraria.ID.TileID.Beds)
            {
                companion.UseFurniture(TilePosition.X, TilePosition.Y);
            }
            BedSpawn = companion.IsUsingBed;
            FirstFrame = false;
        }
        if (!companion.IsUsingBed)
        {
            bool LastSeenPlayer = SeenPlayer != null;
            SeenPlayer = SeekCharacterInViewRange(companion);
            if (SeenPlayer != null)
            {
                if (!BedSpawn && !LastSeenPlayer)
                {
                    companion.SaySomething("*Hello. Can we talk?*");
                }
                if (companion.velocity.X == 0)
                {
                    companion.FaceSomething(SeenPlayer);
                }
            }
            else
            {
                if (!BedSpawn && LastSeenPlayer)
                {
                    companion.SaySomething("*They're gone..*");
                }
                base.Update(companion);
            }
        }
    }

    public override MessageBase ChangeStartDialogue(Companion companion)
    {
        if (SpeakSecondTime) return StartDialogueChain2();
        if (!BedSpawn) return StartAltDialogueChain();
        return StartDialogueChain1();
    }

    #region Dialogue Chain Alternative
    MessageBase StartAltDialogueChain()
    {
        MessageDialogue md = new MessageDialogue("*Hello, do you know any village nearby?*");
        md.AddOption("I do.", Message11);
        md.AddOption("I don't actually know.", Message12);
        md.AddOption("Why do you ask?", Message13);
        return md;
    }

    void Message11()
    {
        MessageDialogue md = new MessageDialogue("*That's nice. I've been wandering for days, and my feet and wings are sore.*");
        md.AddOption("Why you've been wandering?", Message111);
        md.AddOption("Days? How many days?", Message112);
        md.RunDialogue();
    }

    void Message111()
    {
        MessageDialogue md = new MessageDialogue("*It's quite a story. I used to work as a butler, but my last masters died recently, and I went unemployed. Then I ended up finding a weird portal that popped up, and entered it, and I ended up here.*");
        md.AddOption("And you have been wandering ever since?", Message1111);
        md.RunDialogue();
    }

    void Message1111()
    {
        MessageDialogue md = new MessageDialogue("*Yes. I even tried to go back to where the portal was, but it vanished. So I've been looking for a friendly place to stay for a while.*");
        md.AddOption("You can find the village that way.", Message11111);
        md.RunDialogue();
    }

    void Message11111()
    {
        MessageDialogue md = new MessageDialogue("*Thanks. I can't wait to finally be able to rest.*");
        md.RunDialogue();
        SpeakSecondTime = true;
    }

    void Message112()
    {
        MessageDialogue md = new MessageDialogue("*Maaaaany days. I saw the sun and moon pass over me many times since I got into this world.*");
        md.AddOption("And you have been wandering ever since?", Message111);
        md.RunDialogue();
    }

    void Message12()
    {
        MessageDialogue md = new MessageDialogue("*That's bad. I was in need of a place to rest.*");
        md.AddOption("Place to rest?", Message121);
        md.RunDialogue();
    }

    void Message121()
    {
        MessageDialogue md = new MessageDialogue("*Yes. I've been wandering for days and haven't got any chance to rest my feet.*");
        md.AddOption("Good luck finding a place to rest.", Message1211);
        md.RunDialogue();
    }

    void Message1211()
    {
        MessageDialogue md = new MessageDialogue("*I will need that. I can't wait to lie down on a bed and get some sleep.*");
        md.RunDialogue();
        SpeakSecondTime = true;
    }

    void Message13()
    {
        MessageDialogue md = new MessageDialogue("*I need some place to rest. I've been walking for days.*");
        md.AddOption("I know of a village you could stay at.", Message11);
        md.AddOption("I don't know of any village nearby.", Message12);
        md.RunDialogue();
    }
    #endregion
    
    #region Dialogue Chain 1
    MessageBase StartDialogueChain1()
    {
        MessageDialogue md = new MessageDialogue("(Zzzzzz...)");
        md.AddOption("Hey. Wake up.", OnTryWakeUpOnce);
        md.AddOption("Better I not disturb...", Dialogue.EndDialogue);
        return md;
    }

    void OnTryWakeUpOnce()
    {
        MessageDialogue md = new MessageDialogue("(Om... Nom... Zzzz...)");
        md.AddOption("Wake up.", OnTryWakeUpOnce);
        md.AddOption("Better I not disturb further...", Dialogue.EndDialogue);
        md.RunDialogue();
    }

    void OnTryWakeUpTwice()
    {
        MessageDialogue md = new MessageDialogue("(Just ten more minutes...)");
        md.AddOption("WAKE UP!", OnTryWakeUpThrice);
        md.AddOption("I feel a horrible chill going down my spine..", Dialogue.EndDialogue);
        md.RunDialogue();
    }

    void OnTryWakeUpThrice()
    {
        Dialogue.Speaker.LeaveFurniture();
        MessageDialogue md = new MessageDialogue("*Yawn... Oh, sorry. Is this bed yours? I've been dead tired from travelling, and couldn't resist the sight of a bed.*");
        md.AddOption("You had to use that bed?", ThirdFirstAnswer);
        md.AddOption("You usually enter other people home to sleep in their beds?", ThirdSecondAnswer);
        md.RunDialogue();
    }

    void ThirdFirstAnswer()
    {
        MessageDialogue md = new MessageDialogue("*I don't remember seeing any other. Anyways, sorry for sleeping on it. I'll be going now.*");
        md.RunDialogue();
        SpeakSecondTime = true;
    }

    void ThirdSecondAnswer()
    {
        MessageDialogue md = new MessageDialogue("*Actually not, but now that you put it on that way, it does look really bad. I'll be going now. Sorry for sleeping on your bed.*");
        md.RunDialogue();
        SpeakSecondTime = true;
    }
    #endregion

    #region Dialogue Chain 2
    MessageBase StartDialogueChain2()
    {
        MessageDialogue md = new MessageDialogue("*Oh yes, I never asked your name. Who are you?*");
        md.AddOption("I'm [nickname].", OnSecondFirst);
        md.AddOption("Why are you asking?", OnSecondSecond);
        return md;
    }

    public void OnSecondFirst()
    {
        MessageDialogue md = new MessageDialogue("*[nickname]. Got it. I'm [name]. I used to work as a butler, but after my masters died, I'm jobless now.*");
        md.AddOption("A dragon? Butler?", OnAskSecond);
        md.RunDialogue();
    }

    public void OnSecondSecond()
    {
        MultiStepDialogue md = new MultiStepDialogue(["*It's always formality here to ask people names, right? But I don't blame you from being skeptical. TerraGuardians tend to give their name to people they make bond with.*", 
        "*But I don't mind giving mine. I am [name]. I used to be a butler, but since my masters died, I am jobless.*"]);
        md.AddOption("A dragon? Butler?", OnAskSecond);
        md.RunDialogue();
    }

    public void OnAskSecond()
    {
        MultiStepDialogue md = new MultiStepDialogue(["*Yes. You can think of Dragons as flying creatures that breath fire and stuff, but I'm also a TerraGuardian.*", 
        "*Ever since my masters died, I have been wandering aimlessly trying to look for a new job...*",
        "*Wait, why don't I work for you?*"]);
        md.AddOption("Wait, what?", OnAskThird);
        md.RunDialogue();
    }

    public void OnAskThird()
    {
        MessageDialogue md = new MessageDialogue("*Yes. If I have a new master, I have a new job. That's perfect!*");
        md.AddOption("W-Wait, but I didn't ask for a butler?", OnAskFourth1);
        md.AddOption("Me? Having a Butler? Cool!", OnAskFourth2);
        md.RunDialogue();
    }

    void OnAskFourth1()
    {
        MessageDialogue md = new MessageDialogue("*Don't worry about that. Just have me with you and I will do my job just for you. You don't even need to think about payment.*");
        SetFourthOptions(md);
        md.RunDialogue();
    }

    void OnAskFourth2()
    {
        MessageDialogue md = new MessageDialogue("*I knew you'd be happy to hear that. Just have me with you and I will do my job just for you. You don't even need to think about payment.*");
        SetFourthOptions(md);
        md.RunDialogue();
    }

    void SetFourthOptions(MessageDialogue md)
    {
        md.AddOption("Think about payment? What do you mean..", OnAskFifth1);
        md.AddOption("Having you with me?", OnAskFifth2);
    }

    public void OnAskFifth1()
    {
        MessageDialogue md = new MessageDialogue("*What I meant to say is that I'm not going to charge you for my services, as long as you don't overwork me.*");
        SetFifthOptions(md);
        md.RunDialogue();
    }

    public void OnAskFifth2()
    {
        MessageDialogue md = new MessageDialogue("*Yes. You take me on your adventures, and I supply your needs.*");
        SetFifthOptions(md);
        md.RunDialogue();
    }

    void SetFifthOptions(MessageDialogue md)
    {
        md.AddOption("I don't know...", OnAskSixth1);
        md.AddOption("That's fine by me.", OnAskSixth2);
    }

    public void OnAskSixth1()
    {
        MessageDialogue md = new MessageDialogue("*Well, you may call me any time you want for your adventures. Or at least after I get some rest.\nAnd you can ask me more about my job if you have questions.*");
        md.AddOption("Okay.", Dialogue.EndDialogue);
        md.RunDialogue();
        PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
        WorldMod.AddCompanionMet(Dialogue.Speaker);        
    }

    public void OnAskSixth2()
    {
        MessageDialogue md = new MessageDialogue("*You can call me anytime you want, and I will follow you on your adventures. Or at least after I get some rest.\nIf you have any questions about my job, I'll answer them for you.*");
        md.AddOption("Alright.", Dialogue.EndDialogue);
        md.RunDialogue();
        PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
        WorldMod.AddCompanionMet(Dialogue.Speaker);
    }
    #endregion
}