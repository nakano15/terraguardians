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
            Mes.Add("*Some people get confused when they hear my nickname. They say I'm talking about myself twice.*");
            Mes.Add("*How do you like my nickname? I heard some people saying it, and I thought it would be funny.*");
            Mes.Add("*I love seeing so many birds around. They make a good game.*");

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
                    Mes.Add("*Fuel low. Need zzz charge.*");
                }
            }

            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                
            }
        }
        return Mes[Main.rand.Next(Mes.Count)];
    }
}