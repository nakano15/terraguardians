using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSpawningTips
    {
        private static List<string> CurrentTips = new List<string>();
        private static ushort CurrentTipIndex = 0;

        public static void Unload()
        {
            CurrentTips.Clear();
            CurrentTips = null;
        }

        private static void PopulateTipsList()
        {
            CurrentTipIndex = 0;
            CurrentTips.Clear();
            if (!HasMetGuardian(CompanionDB.Rococo) && CanSpawnCompanionNpc(CompanionDB.Rococo))
            {
                CurrentTips.Add("*I've met a friendly TerraGuardian looking for a place to live. He may end up appearing anytime during your adventure.*");
            }
            if (!HasMetGuardian(CompanionDB.Blue) && CanSpawnCompanionNpc(CompanionDB.Blue))
            {
                CurrentTips.Add("*I've met once a TerraGuardian who liked camping. Maybe she'll stop by if there be a campfire.*");
            }
            if (!HasMetGuardian(CompanionDB.Sardine) && CanSpawnCompanionNpc(CompanionDB.Sardine))
            {
                CurrentTips.Add("*There was that cat... I don't remember his name... He said he was pursuing his highest bounty: The King Slime. I wonder If he were successfull.*");
            }
            if (!HasMetGuardian(CompanionDB.Zacks) && CanSpawnCompanionNpc(CompanionDB.Zacks))
            {
                if (HasMetGuardian(CompanionDB.Blue))
                {
                    CurrentTips.Add("*I can't forget one Blood Moon I survived. I was very far in the world, while being attacked by zombies from all the sides, until a big wolf zombie TerraGuardian appeared. Now that I'm thinking, that zombie looked a lot like " + WorldMod.GetCompanionNpcName(1) + ".*");
                }
                else
                {
                    CurrentTips.Add("*I can't forget one Blood Moon I survived. I was very far in the world, while being attacked by zombies from all the sides, until a big wolf zombie TerraGuardian appeared. I managed to dispatch it, but It was scary.*");
                }
            }
            if (!HasMetGuardian(CompanionDB.Nemesis) && CanSpawnCompanionNpc(CompanionDB.Nemesis) && Main.hardMode)
            {
                CurrentTips.Add("*The other day, I was barring my door, because there was a Possessed Armor repeatedly saying \"I'll be your shadow\". I don't know what It was talking about, but was really terrifying.*");
            }
            if (!HasMetGuardian(CompanionDB.Alex) && CanSpawnCompanionNpc(CompanionDB.Alex))
            {
                CurrentTips.Add("*In the Ether Realm, there is a popular story about a Giant Dog and a Terrarian Woman. They lived happy, went into adventures and played together everyday, until one day she died. Legends says that the Giant Dog buried his owner, and still guards her tombstone since that day. I wonder if those legends are true.*");
            }
            if (!HasMetGuardian(CompanionDB.Brutus))
            {
                int TownNpcCount = (int)(WorldMod.GetCompanionsCount * 0.5f);
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && Main.npc[n].townNPC)
                        TownNpcCount++;
                }
                if (TownNpcCount >= 2)
                {
                    CurrentTips.Add("*I've been hearing stories of a Royal Guard from Ether Realm who lost his job, and is now roaming through worlds looking to work as a bodyguard. I think there's a chance that he may be appearing here.*");
                }
                else
                {
                    CurrentTips.Add("*I've been hearing stories of a Royal Guard from Ether Realm who lost his job, and is now roaming through worlds looking to work as a bodyguard. Is said that he has higher chances of appearing in places with many people living.*");
                }
            }
            if (!HasMetGuardian(CompanionDB.Bree) && CanSpawnCompanionNpc(CompanionDB.Bree))
            {
                CurrentTips.Add("*I've bumped into a white cat earlier, who said she was looking for her husband. She said that she was travelling world by world trying to look for him, and she looked a bit worn out the last time I saw her. I tried convincing her to stay for a while, but she didn't accepted. If you find her, can you convince her to stay for a while?*");
            }
            if (!HasMetGuardian(CompanionDB.Celeste) && CanSpawnCompanionNpc(CompanionDB.Celeste))
            {
                CurrentTips.Add("*I heard rummors about a TerraGuardian priestess wandering around this world. Maybe she might appear any time soon.*");
            }
            if (!HasMetGuardian(CompanionDB.Mabel) && CanSpawnCompanionNpc(CompanionDB.Mabel))
            {
                CurrentTips.Add("*I've met a TerraGuardian who wanted to try flying like a reindeer. The problem, is that not only reindeers can't fly, but she's not a reindeer. Can you please find her before she gets hurt?*");
            }
            /*if (!HasMetGuardian(CompanionDB.Domino) && Npcs.DominoNPC.CanSpawnDomino(player))
            {
                if (HasGuardianNPC(CompanionDB.Brutus))
                {
                    CurrentTips.Add("*There is a shady TerraGuardian roaming this world. He seems to be running away from something. Maybe you should bring " + NpcMod.GetGuardianNPCName(CompanionDB.Brutus) + " with you in case you bump with him.*");
                }
                else
                {
                    CurrentTips.Add("*There is a shady TerraGuardian roaming this world. He seems to be running away from something. You can try talking to him, but I don't know if will result into anything fruitful.*");
                }
            }*/
            /*if (!HasMetGuardian(10) && Npcs.LeopoldNPC.CanSpawnLeopold)
            {
                CurrentTips.Add("*Did you hear? Leopold is visiting this world! You don't know who he is? He's a famous sage from Ether Realm. I managed to bump into him the other day when I was picking up flowers. I think you may end up finding him any time during your travels.");
            }*/
            /*if (!HasMetGuardian(11) && Npcs.VladimirNPC.CanRecruitVladimir)
            {
                CurrentTips.Add("*I heard a weird rumor from a Terrarian who said that found a \"giant bear\" when exploring the Underground Jungle. They said that the bear were saying that was hungry and that wanted them to give him a hug. I think that may be another TerraGuardian, and I recommend you to check that out, since that person seems to be in trouble, and please don't freak out like the other Terrarian.");
            }*/
            /*if (!HasMetGuardian(12) && Npcs.MalishaNPC.MalishaCanSpawn)
            {
                CurrentTips.Add("*I heard that we should be careful, since a witch seems to be taking vacation on this world. Who told me that? Well... You wont believe me, but the warning was given by a Bunny.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Wrath) && Npcs.WrathNPC.WrathCanSpawn)
            {
                CurrentTips.Add("*A person was attacked last night in the forest. They were brought unconscious to the town, and when woke up, said that a \"" +
                    (player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Companions.PigGuardianFragmentBase.AngerPigGuardianID] ? 
                    "kind of cloud in form of a red pig" :
                    "angry red pig") + "\" attacked them. You need to check that out.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Fluffles) && Npcs.GhostFoxGuardianNPC.CanGhostFoxSpawn(player))
            {
                CurrentTips.Add("*Watch out, [nickname]. I've been hearing that there's a ghost chasing people in the dark. Better not let it catch you.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Minerva))
            {
                CurrentTips.Add("*I've met a friendly TerraGuardian who seems to be travelling this world. I don't think you may end up convincing her to stay at first, but she may visit often if she finds out there's a place she can visit.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Alexander) && Npcs.AlexanderNPC.AlexanderConditionMet)
            {
                CurrentTips.Add("*I heard that there's a TerraGuardian jumping and sleuthing people who tries exploring the dungeon. Based on the what people said, every time ends up with him saying it's not who they're looking for. I wonder who that TerraGuardian is looking for.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Cinnamon))
            {
                CurrentTips.Add("*There's a cute TerraGuardian sometimes follows Travelling Merchants on their travels. I think she may end up arriving here if that's true.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Miguel) && Npcs.MiguelNPC.CanSpawnMe())
            {
                CurrentTips.Add("*There's a really buff TerraGuardian exploring this world. He also likes to insult people who don't have \"proper body building\". I know because he did that to me...*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Quentin) && NPC.downedBoss3)
            {
                CurrentTips.Add("*A person told me that they heard someone crying, when exploring the dungeon. Whoever that is, they definitelly seems to need help.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Fear) && NPC.downedBoss3)
            {
                CurrentTips.Add("*It is said that screams can be heard inside the dungeon. I don't actually screams of ghosts, wraiths or anything like that, but actually someone screaming out of terror. I think there's someone in trouble there.*");
            }*/
            /*if (!HasMetGuardian(CompanionDB.Green) && Npcs.GreenNPC.CanSpawnGreen())
            {
                CurrentTips.Add("*I heard people saying that a intimidating giant snake is roaming this world. The person said that It climbed some tree to sleep. I really can't believe that, but it doesn't hurt to look that.*");
            }*/
            /*if(!HasMetGuardian(CompanionDB.Liebre) && Npcs.LiebreNPC.CanSpawn)
            {
                switch (Npcs.LiebreNPC.EncounterTimes)
                {
                    case 0:
                        CurrentTips.Add("*Someone told me that found a grim reaper when they were exploring the forest. The person said that ran away very fast when It said that wanted to talk to them.*");
                        break;
                    case 1:
                        CurrentTips.Add("*You know that grim reaper you've found? People says they found it exploring the " + (WorldGen.crimson ? "Crimson" : "Corruption") + ".*");
                        break;
                    case 2:
                        CurrentTips.Add("*I heard that the grim reaper you met some time ago has entered the dungeon. What could it be doing there?*");
                        break;
                    case 3:
                        CurrentTips.Add("*I haven't heard about the grim reaper since you last found them in the dungeon. I wonder what could have happened.*");
                        break;
                }
            }*/
            /*if (!HasMetGuardian(CompanionDB.Cille))
            {
                if(Npcs.CilleNPC.CanSpawn())
                    CurrentTips.Add("*There is a Cheetah TerraGuardian wandering around this world. She kind of have a horrible sense of fashion, and refuses to talk to anyone. Maybe you can check out what's with her. She's never seen during New Moon or Full Moon days.*");
            }
            else
            {
                if(GuardianSpawningScripts.CilleShelterX == -1)
                {
                    CurrentTips.Add("*I didn't heard anymore about that Cheetah TerraGuardian you met some time ago.*");
                }
                else
                {
                    CurrentTips.Add("*It seems like that Cheetah TerraGuardian is living "+(GuardianSpawningScripts.CilleShelterX * 16 - Main.player[Main.myPlayer].Center.X < 0 ? "west" : "east")+" of here.*");
                }
            }*/
        }

        private static bool CanSpawnCompanionNpc(uint Id, string ModID = "")
        {
            return MainMod.GetCompanionBase(Id, ModID).CanSpawnNpc();
        }

        public static void ShowTip()
        {
            PopulateTipsList();
            if (CurrentTips.Count > 0)
                CurrentTipIndex = (ushort)Main.rand.Next(CurrentTips.Count);
            ShowNextTip();
        }

        private static void ShowNextTip()
        {
            MessageDialogue md = new MessageDialogue();
            if (CurrentTips.Count == 0)
            {
                md.ChangeMessage("*I didn't heard about anything latelly.*");
            }
            else
            {
                md.ChangeMessage(CurrentTips[CurrentTipIndex]);
                CurrentTipIndex++;
                if (CurrentTipIndex >= CurrentTips.Count)
                    CurrentTipIndex -= (ushort)CurrentTips.Count;
                if(CurrentTips.Count > 1)
                    md.AddOption("Anything else?", ShowNextTip);
            }
            md.AddOption("Thanks.", ReturnToLobby);
            md.RunDialogue();
        }

        private static void ReturnToLobby()
        {
            CurrentTips.Clear();
            Dialogue.LobbyDialogue();
        }

        private static bool HasMetGuardian(uint Id, string ModID = "")
        {
            return WorldMod.HasMetCompanion(Id, ModID);
        }
        private static bool HasGuardianNPC(uint Id, string ModID = "")
        {
            return WorldMod.HasCompanionNPCSpawned(Id, ModID);
        }
    }
}