using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics.Terrarian
{
    public class GamerGenericDialogue : CompanionDialogueContainer
    {
        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Noob.");
            Mes.Add("Too bad that I left the Dynamites on my other world.");
            Mes.Add("What is a milk drinker doing here?");
            if (!Main.hardMode)
                Mes.Add("I've killed the Wall of Flesh already. Why haven't you?");
            else if (!NPC.downedPlantBoss)
                Mes.Add("I've killed Plantera already. Why haven't you?");
            else if (!NPC.downedGolemBoss)
                Mes.Add("I've killed The Golem already. Why haven't you?");
            else if (!NPC.downedMoonlord)
                Mes.Add("I've killed Moonlord and saved the world already. Why haven't you?");
            else
                Mes.Add("I've killed Dungeon Guardian with a Copper Shortsword already. Why haven't you?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            return "You're still a noob.";
        }
    }
}