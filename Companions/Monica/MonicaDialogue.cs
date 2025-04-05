using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions;

public class MonicaDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Hello. You're... A Terrarian..?*";
            case 1:
                return "*Oh, hi! I never met someone like you before..*";
            case 2:
                return "*Hello... Please don't make comments about my belly..*";
        }
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        if (Main.bloodMoon && !Main.dayTime)
        {
            Mes.Add("*DON'T YOU START TALKING ABOUT MY BELLY! I KNOW I'M FAT!*");
            Mes.Add("*I don't want to know!*");
            Mes.Add("*Don't... Even start...*");
        }
        else if (companion.IsUsingToilet)
        {
            Mes.Add("*Can't you leave me alone? It's really tough to do my necessities with you watching.*");
            Mes.Add("*Yes, women also have to do necessities. Curiosity cleared? Then leave me be.*");
            Mes.Add("*I need some privacy now, [nickname].*");
        }
        else
        {
            if (Main.dayTime)
            {
                if (Main.raining)
                {
                    Mes.Add("*Is the rain over already..?*");
                    Mes.Add("*The rain drops washed away my happiness, and left me moody...*");
                }
                else
                {
                    Mes.Add("*What a beautiful day.*");
                    Mes.Add("*Hello [nickname], enjoying life?*");
                }
            }
            else
            {
                if (Main.raining)
                {
                    Mes.Add("*This is the only moment rain is good... When you're about to sleep.*");
                    Mes.Add("*I wouldn't mind hibernating at the sound of rain drops. At least on the next season I would be slim.*");
                }
                else
                {
                    Mes.Add("*Are you feeling sleepy too, [nickname]?*");
                    Mes.Add("*Have you heard knocking? Who could that be?*");
                }
            }
            Mes.Add("*Oh, hello [nickname].*");
            if (companion.IsHungry)
            {
                Mes.Add("*Is it "+(Main.dayTime ? "Lunch" : "Dinner")+" time?*");
                Mes.Add("*I'm so hungry that I could eat a... Squirrel.*");
            }
            Mes.Add("*Please don't come blaming me for the bunnies showing around. If was my fault, they would be bigger.*");

            Mes.Add("*I already feel depleted... I need a rest..*");
            Mes.Add("*I don't get why people say \"You're fat.\". I know that already!*");
            Mes.Add("*I'm trying my hardest to lose my belly. Believe me.*");
            Mes.Add("*How I will lose this fat..?*");

            if (IsPlayerRoomMate())
            {
                Mes.Add("*I like sharing my bedroom with you. At least I know that I'm safe with you around.*");
                Mes.Add("*Good thing the bedroom is big enough for us, right?*");
            }

            if(CanTalkAboutCompanion(CompanionDB.Alexander))
            {
                Mes.Add("*[gn:"+CompanionDB.Alexander+"] said that I would lose my belly fast, if I were investigating monster involving cases.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*Whenever I want to take a sit on a bench, I have to check out if there's nobody using it. We nearly lost [gn:"+CompanionDB.Sardine+"] the other day...*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*I like playing games with [gn:"+CompanionDB.Rococo+"], but I really hate when I have to run after him. I never am able to catch him.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*Could you tell [gn:"+CompanionDB.Miguel+"] to stop making fun of my belly? I don't need him telling me new daily jokes related to it.*");
                Mes.Add("*I tried to get some exercises recommendations from [gn:"+CompanionDB.Miguel+"], and I think I'm doing well.*");
                Mes.Add("*Don't tell [gn:"+CompanionDB.Miguel+"] but... I actually like him.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*I tried to ask [gn:"+CompanionDB.Leopold+"] if he knew some magic to lose fat. He told me to close my mouth...*");
                Mes.Add("*I wanted to be smart like [gn:"+CompanionDB.Leopold+"]... Do you think I could be as smart as him?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Liebre))
            {
                Mes.Add("*I was talking with [gn:"+CompanionDB.Liebre+"] earlier this day. He said that if I don't watch my health, he'll take me for the ride of my life. What he means by that?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*Why [gn:"+CompanionDB.Blue+"] walks like as if she's holding something on the pelvis?*");
                Mes.Add("*How did [gn:"+CompanionDB.Blue+"] get such cool hair? I'll ask her some time.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*I think that the only one who can understand me is [gn:"+CompanionDB.Vladimir+"]...*");
                Mes.Add("*I love spending some time talking with [gn:"+CompanionDB.Vladimir+"], even more when we are sharing a bowl of chips.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*Have you eaten the food [gn:"+CompanionDB.Minerva+"] makes? It's divine... D.I.V.I.N.E.*");
                Mes.Add("*I could eat [gn:"+CompanionDB.Minerva+"]'s food all day...*");
            }
        }
        //Need to work on her dialogues...
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string GetFeedRelatedMessage(Companion companion, FeedRelatedContext context)
    {
        switch (context)
        {
            case FeedRelatedContext.WhenFed:
                return "*That will hit the spot. Thank you!*";
            case FeedRelatedContext.WhenFedFavoriteFood:
                return "*Is... Is that a... [item]?! How did you knew I love that?! Thank you!*";
            case FeedRelatedContext.PlanningOnOfferingFood:
                return "*You'll give me some food? My belly was rumbling, so I wont deny it.*";
            case FeedRelatedContext.Nevermind:
                return "*Hey! I'm not fasting!*";
        }
        return base.GetFeedRelatedMessage(companion, context);
    }

    public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
    {
        switch (context)
        {
            case JoinMessageContext.Success:
                return "*Sure, I can. Maybe I can burn some fat while travelling.*";
        }
        return base.JoinGroupMessages(companion, context);
    }
}