using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics.Terrarian
{
    public class TerrarianGenericDialogue : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "Are you an adventurer too? I am an adventurer!";
                case 1:
                    return "Have you managed to beat any boss yet?";
                case 2:
                    if (!MainMod.DisableModCompanions)
                        return "Are you also seeing creatures popping up here, too?";
                    return "Look at what I found!";
            }
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("The secret to slaying monsters, is killing them before they kill you.");
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("I used to see a lot of those monsters at the TV, during halloween.");
                    Mes.Add("I wonder, where those monsters come from...?");
                }
                else if (Main.raining)
                {
                    Mes.Add("I'm cold and wet...");
                    Mes.Add("This weather just spoiled my adventure sense...");
                }
                else
                {
                    Mes.Add("I could use a Bunny Stew right now. Sadly I can't get the meat by simply slaying them...");
                    Mes.Add("I wonder where should I go next...");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("Ready for some grinding, [nickname]?");
                    Mes.Add("How many monsters you killed tonight, already?");
                    Mes.Add("Did you find a zombie couple out there? Me neither.");
                }
                else if (Main.raining)
                {
                    Mes.Add("I'm starting to feel drowzy...");
                    Mes.Add("All that was left was a guitar solo to improve the weather.");
                }
                else
                {
                    Mes.Add("What are the zombies that wander at night? You don't think they were adventurers... Like us... Right?");
                    Mes.Add("I wonder: what we did to get floating eye balls hunting us at night?");
                }
            }
            if (WorldGen.crimson)
            {
                Mes.Add("Why the Crimson is so red... Oh... Nevermind...");
                Mes.Add("Those Crimson creatures are so ugly, aren't they.");
            }
            else
            {
                Mes.Add("I don't like being in the Corruption. I already feel sick just for being there.");
                Mes.Add("I have once met someone who were farming for Rotten Chunks in the Corruption. Why he were doing that? I have no idea.");
            }
            if (NPC.downedBoss1)
            {
                Mes.Add("You managed to beat Eye of Cthulhu? I wonder why it came after you.");
            }
            if (NPC.downedBoss3)
            {
                Mes.Add("The dungeon is now accessible? Time for more adventure!");
            }
            if (Main.hardMode)
            {
                Mes.Add("Why do things look a bit... Harder.. Now..?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            return "What's up?";
        }
    }
}