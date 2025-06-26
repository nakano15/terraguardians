using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using terraguardians;
using Terraria;
using Terraria.ID;

namespace terraguardians.Companions;

public class CottonDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "Hello.";
            case 1:
                return "Hi!";
            case 2:
                return "S'up?";
        }
        return base.GreetMessages(companion);
    }

    public override string NormalMessages(Companion companion)
    {
        List<string> Mes = new List<string>();
        Mes.Add("*You think I'm a stuffed toy? I have flesh and bones in me too.*");
        Mes.Add("*Why do people like to hug me?*");
        Mes.Add("*I don't squeak when pushed, unless you tickle my belly.*");
        Mes.Add("*Why some people think that there's a Terrarian wearing me?*");
        if (Terraria.Main.dayTime)
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
            Mes.Add("*I like that some times [gn:" + NPCID.Merchant + "] has some bones for me. He's even nice to not charge me anything.*");
        }
        if (NPC.AnyNPCs(NPCID.Clothier))
        {
            Mes.Add("*[gn:" + NPCID.Clothier + "] asked me earlier who's my tailor... What tailor..? I got not clothing.*");
        }
        if (CanTalkAboutCompanion(CompanionDB.Rococo))
        {
            Mes.Add("*It's hard for me to keep my conversations civil with [gn:" + CompanionDB.Rococo + "]. My toys are way better than the ones he has, and he knows it.*");
            Mes.Add("*Can you believe what [gn:" + CompanionDB.Rococo + "] said? How can there be anything better than squeaky toys?!*");
        }
        /*if (CanTalkAboutCompanion(CompanionDB.Alex))
        {
            Mes.Add("**");
        }*/
        return Mes[Main.rand.Next(Mes.Count)];
    }
}