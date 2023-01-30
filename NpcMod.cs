using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class NpcMod : GlobalNPC
    {
        private const int PlaceCatOnKingSlimeValue = -50;
        private static int TrappedCatKingSlime = -1;

        public static void OnReloadWorld()
        {
            TrappedCatKingSlime = -1;
        }

        public override void SetDefaults(NPC npc)
        {
            switch(npc.type)
            {
                case NPCID.KingSlime:
                    if (!WorldMod.HasMetCompanion(CompanionDB.Sardine) && !MainMod.HasCompanionInWorld(CompanionDB.Sardine) && Main.rand.NextFloat() < 0.4f)
                    {
                        TrappedCatKingSlime = PlaceCatOnKingSlimeValue;
                    }
                    break;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.KingSlime && TrappedCatKingSlime == PlaceCatOnKingSlimeValue)
            {
                TrappedCatKingSlime = npc.whoAmI;
            }
            else if (TrappedCatKingSlime == npc.whoAmI && npc.type != NPCID.KingSlime)
            {
                TrappedCatKingSlime = -1;
            }
            return base.PreAI(npc);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(npc.whoAmI == TrappedCatKingSlime)
            {
                TextureAssets.Ninja = MainMod.TrappedCatTexture;
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(npc.whoAmI == TrappedCatKingSlime)
            {
                TextureAssets.Ninja = MainMod.NinjaTextureBackup;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.whoAmI == TrappedCatKingSlime)
            {
                TrappedCatKingSlime = -1;
                if (!MainMod.HasCompanionInWorld(CompanionDB.Sardine))
                {
                    Companion Sardine = WorldMod.SpawnCompanionNPC(npc.Center, CompanionDB.Sardine);
                    if (Sardine != null)
                        Sardine.AddBuff(BuffID.Slimed, 10 * 60);
                }
            }
            if (npc.type == Terraria.ID.NPCID.PossessedArmor && !WorldMod.HasMetCompanion(CompanionDB.Nemesis) && !WorldMod.HasCompanionNPCSpawned(CompanionDB.Nemesis) && Main.rand.Next(100) == 0)
            {
                WorldMod.SpawnCompanionNPC(npc.Bottom, CompanionDB.Nemesis);
                Main.NewText("The wraith stayed after you broke its armor.", MainMod.MysteryCloseColor);
            }
            if (npc.AnyInteractions())PlayerMod.UpdatePlayerMobKill(MainMod.GetLocalPlayer, npc);
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (Main.rand.NextDouble() >= 0.25)
                return;
            List<string> PossibleMessages = new List<string>();
            switch (npc.type)
            {
                case Terraria.ID.NPCID.Guide:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                        PossibleMessages.Add("Have you seen " + WorldMod.GetCompanionNpcName(0) + "? I think he just stole my Guide book to prank on me.");
                    if (WorldMod.HasCompanionNPCSpawned(2))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(2) + " comes frequently to me, asking if there is any tough creature he can take down.");
                    if (WorldMod.HasCompanionNPCSpawned(5))
                    {
                        //if (Main.rand.Next(2) == 0) //Alex isn't implemented yet
                        //    PossibleMessages.Add("You want to know about " + AlexRecruitScripts.AlexOldPartner + "? Sorry, I don't know that person.");
                        //else
                            PossibleMessages.Add(WorldMod.GetCompanionNpcName(5) + " has been a positive addition to the town. But I wonder who cleans up his mess.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(8))
                    {
                        PossibleMessages.Add("I keep dismissing " + WorldMod.GetCompanionNpcName(8) + ", she distracts me.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Vladimir))
                    {
                        PossibleMessages.Add("I've been hearing good things about " + WorldMod.GetCompanionNpcName(CompanionDB.Vladimir) + ", It seems like he's been helping several people with their problems.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add("Since you've found " + WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + ", I wonder If there are other people going to join your travels.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
                    {
                        PossibleMessages.Add("D-d-did you s-see " + WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + " with a doll that looks like me?");
                    }
                    break;
                case Terraria.ID.NPCID.Nurse:
                    {
                        PossibleMessages.Add("I can heal try healing your companions too, but I will charge more for that.");
                        if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Vladimir) && WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue) && NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                        {
                            PossibleMessages.Add("I've got some good tips of things I could do on my date with " + NPC.GetFirstNPCNameOrNull(Terraria.ID.NPCID.ArmsDealer) + " from " + WorldMod.GetCompanionNpcName(CompanionDB.Blue) + ". She said that her boyfriend fell for her after she did that, so what could go wrong?! The only weird thing is the method she used.");
                        }
                    }
                    break;
                case Terraria.ID.NPCID.ArmsDealer:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                        PossibleMessages.Add("I tried to teach " + WorldMod.GetCompanionNpcName(0) + " how to use a gun, he nearly shot my head off. Never. Again.");
                    if (WorldMod.HasCompanionNPCSpawned(2))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(2) + " comes at me frequently with very absurdly overpowered weapon requests, from rocket launchers to sniper rifles. Seriously, where would I get those?");
                    if (WorldMod.HasCompanionNPCSpawned(6))
                        PossibleMessages.Add("What is " + WorldMod.GetCompanionNpcName(6) + "'s skin made of? He asked me to shot him for training and he just stud still, like as If nothing happened.");
                    if (WorldMod.HasCompanionNPCSpawned(7))
                        PossibleMessages.Add("Can you believe that " + WorldMod.GetCompanionNpcName(7) + " had the audacity of coming to MY STORE, and saying that my weapons are DATED?");
                    if (WorldMod.HasCompanionNPCSpawned(8))
                    {
                        PossibleMessages.Add("Everyone keeps staring at " + WorldMod.GetCompanionNpcName(8) + ", but something on me wants to shoot her, instead.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(9))
                    {
                        PossibleMessages.Add("Finally, a worthy TerraGuardian. " + WorldMod.GetCompanionNpcName(9) + " can be quite of a business partner for me.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Vladimir) && WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue) && NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                    {
                        PossibleMessages.Add("Ack!! You scared me! Can you tell me what is wrong with "+NPC.GetFirstNPCNameOrNull(Terraria.ID.NPCID.Nurse)+"? We were going on a date, until she pounced on me on the table and tried to bite my arm! I ran as fast as I could after that!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " came here earlier looking for a gun. I asked If she wanted a pistol or a machinegun. She said that wanted a rocket launcher.");
                    }
                    break;
                case Terraria.ID.NPCID.Truffle:
                    if (WorldMod.HasCompanionNPCSpawned(1))
                        PossibleMessages.Add("Seeing " + WorldMod.GetCompanionNpcName(1) + " makes me feel Blue. That is the logic.");
                    if (WorldMod.HasCompanionNPCSpawned(2))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(2) + " said that talking to me was making him feel like being under catnip effect.");
                    //if (WorldMod.HasCompanionNPCSpawned(5)) //Alex isn't implemented yet
                    //    PossibleMessages.Add("A long time ago I've met " + WorldMod.GetCompanionNpcName(5) + " and " + AlexRecruitScripts.AlexOldPartner + ". They were exploring the caverns when they found the town I lived. People was overjoyed when they discovered that they didn't went there to eat them.");
                    break;
                case Terraria.ID.NPCID.Stylist:
                    if (WorldMod.HasCompanionNPCSpawned(1))
                        PossibleMessages.Add("If you want me to do your hair, you will have to wait a day or two for my arms to rest, because " + WorldMod.GetCompanionNpcName(1) + " wanted me to do her hair, but do you know how much hair she has?");
                    if (WorldMod.HasCompanionNPCSpawned(7))
                        PossibleMessages.Add("I feel pitty of " + WorldMod.GetCompanionNpcName(7) + ", she asks me to do her hair, but she nearly has any, so I pretend that I'm doing something.");
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
                    {
                        PossibleMessages.Add("I think I could do some hair work on " + WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + ".");
                    }
                    break;
                case Terraria.ID.NPCID.Mechanic:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                    {
                        PossibleMessages.Add("If you manage to see " + WorldMod.GetCompanionNpcName(0) + " again, tell him to stop pressing my switches?");
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(0) + " asked me if I could make him a giant robot, but ever since that dungeon incident, I say: No more.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(2))
                        PossibleMessages.Add("I always love it when " + WorldMod.GetCompanionNpcName(2) + " comes to visit me. I can't wait for the next time.");
                    if (WorldMod.HasCompanionNPCSpawned(11))
                    {
                        PossibleMessages.Add("(She's humming. Something must have brightened her day.)");
                        PossibleMessages.Add("I love having "+WorldMod.GetCompanionNpcName(11) + " around, he always gives me a happiness boost. I want to be his friend forever.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add("I think " + WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " must have some kind of trigger. She can't see any kind of switch, she wants to flip them.");
                    }
                    break;
                case Terraria.ID.NPCID.GoblinTinkerer:
                    if (WorldMod.HasCompanionNPCSpawned(2) && NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
                    {
                        PossibleMessages.Add("That creepy cat keeps visiting " + NPC.GetFirstNPCNameOrNull(Terraria.ID.NPCID.Mechanic) + " from time to time, If I could fake out an accident to stop him from...");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(11))
                    {
                        if (WorldMod.HasCompanionNPCSpawned(2))
                            PossibleMessages.Add("Oh boy... First that creepy cat appeared, now a giant bear?! That will kill my chances with " + NPC.GetFirstNPCNameOrNull(Terraria.ID.NPCID.Mechanic) + "...");
                        PossibleMessages.Add("Why " + NPC.GetFirstNPCNameOrNull(Terraria.ID.NPCID.Mechanic) + " always comes radiating happiness from " + WorldMod.GetCompanionNpcName(CompanionDB.Vladimir) + "?");
                    }
                    break;
                case Terraria.ID.NPCID.Steampunker:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                        PossibleMessages.Add("I asked " + WorldMod.GetCompanionNpcName(0) + " the other day to test my newest jetpack. He flew with it, then lost control while in the air and then fell in a lake. Now he wants to know when's the next test.");
                    break;
                case Terraria.ID.NPCID.WitchDoctor:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(0) + " asked me how to use a blowpipe, then I tried teaching him. He nearly died out of suffocation because inhaled the seed.");
                    if (WorldMod.HasCompanionNPCSpawned(1))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(1) + " seems to have some interest in poisons, just watch out if she gives you a drink. Who knows?");
                    if (WorldMod.HasCompanionNPCSpawned(8))
                        PossibleMessages.Add("Red paint? I didn't painted my mask red... Uh oh... Forget what you saw.");
                    break;
                case Terraria.ID.NPCID.DD2Bartender:
                    PossibleMessages.Add("Bringing a TerraGuardian to the defense of the crystal is a bit of a cheat, but It is very helpful when defending it alone.");
                    if (WorldMod.HasCompanionNPCSpawned(6))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(6) + " is my best client, he always drinks about 10~15 mugs of Ale before returning to his post. He still looks fine afterwards, I guess.");
                    if (WorldMod.HasCompanionNPCSpawned(9))
                        PossibleMessages.Add("I always have to watch over " + WorldMod.GetCompanionNpcName(9) + ", because he doesn't knows when to stop drinking.");
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " got angry when I asked her age when she asked for a drink.");
                    }
                    break;
                case Terraria.ID.NPCID.Merchant:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                    {
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(0) + " keeps coming to me asking if I have Giant Healing Potions, but I have no idea of where I could find such a thing.");
                        PossibleMessages.Add("Have you found someone to take care of the trash? Every morning, I check my trash can and It's clean.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(1))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(1) + " is one of my best clients, she buys a new Shampoo flask every day.");
                    //if (WorldMod.HasCompanionNPCSpawned(5)) //Alex isn't implemented yet
                    //    PossibleMessages.Add(AlexRecruitScripts.AlexOldPartner + " you say? She used to buy pet food for " + WorldMod.GetCompanionNpcName(5) + " from me. She really wasn't into talking with people, most of the time you saw her with " + WorldMod.GetCompanionNpcName(5) + ".");
                    if (WorldMod.HasCompanionNPCSpawned(7))
                        PossibleMessages.Add("What " + WorldMod.GetCompanionNpcName(7) + " expects of my store? My products have quality and I get them in high stack.");
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Vladimir))
                        PossibleMessages.Add("What are you saying?! I never complained to anyone about my sales!");
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add("Everytime " + WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " returns from her travels, she buys several stacks of potions. I think she's not very good at adventuring.");
                    }
                    break;
                case Terraria.ID.NPCID.TravellingMerchant:
                    if (WorldMod.HasCompanionNPCSpawned(7))
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(1) + " really loves when I bring products from the land that produces the Sake. She says that reminds her of home.");
                    if (WorldMod.HasCompanionNPCSpawned(8))
                        PossibleMessages.Add("I've got a huge increase in tissues sale latelly, It seems like there's an incidence of nose bleeding around this place...? I hope isn't contagious.");
                    if (WorldMod.HasCompanionNPCSpawned(9))
                        PossibleMessages.Add("That " + WorldMod.GetCompanionNpcName(9) + " is such a jester, he thinks my products are trash.");
                    if (!WorldMod.HasCompanionNPCSpawned(CompanionDB.Cinnamon))
                    {
                        PossibleMessages.Add("There is a TerraGuardian that follows me into my travels, she keeps me company when moving from a world to another. She may pop up any time soon.");
                    }
                    else
                    {
                        if (WorldMod.IsCompanionLivingHere(CompanionDB.Cinnamon))
                        {
                            PossibleMessages.Add("I quite miss that TerraGuardian company on my travels, now they look a bit more boring. What is her name, by the way?");
                        }
                        else
                        {
                            PossibleMessages.Add("You know that TerraGuardian that arrived with me? She follows me during my travels. At least travelling isn't so lonely with her around.");
                        }
                    }
                    break;
                case Terraria.ID.NPCID.Dryad:
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            PossibleMessages.Add("I forgot to mention about the TerraGuardians, they are mythical creatures that live in another realm. They may be willing to help you on your adventure, if you manage to find them.");
                            break;
                        case 1:
                            PossibleMessages.Add("The TerraGuardians have no problems with humans, at least most of them don't. But If you manage to be a good friend to them, they will retribute the favor.");
                            break;
                        case 2:
                            PossibleMessages.Add("It's weird the fact that the TerraGuardians started to appear right now... That may mean that something big is about to happen...");
                            break;
                        case 3:
                            PossibleMessages.Add("Try befriending as many TerraGuardians as possible, no one knows when we may end up needing their help.");
                            break;
                        case 4:
                            PossibleMessages.Add("Do you ask yourself why you can understand what some guardians says without saying a word? It's easy. Once they meet someone, they create a bond with it, so they not only can express themselves, and also understand what the other wants.");
                            break;
                        case 5:
                            PossibleMessages.Add("There are two realms in this world, the Terra realm, and the Ether realm. You live in the Terra realm, the TerraGuardians lives on the Ether realm, but It is really weird to see them here in Terra.");
                            break;
                        case 6:
                            PossibleMessages.Add("The TerraGuardians grows stronger when they travel with you, they may get stronger on many of their characteristics depending on what they do during your travels.");
                            break;
                    }
                    if (PlayerMod.PlayerHasCompanionSummoned(Main.player[Main.myPlayer], 3))
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                PossibleMessages.Add("You said that when [gn:1] spoke to [gn:3] and then he remember who he was? It seems like at that moment of the battle where he was weakened, the bond with her has strengthened, making him recover his senses.");
                                break;
                            case 1:
                                PossibleMessages.Add("Actually, I guess I remember vaguelly that Terrarian who [gn:3] was following, It seems he had the same goal as you. But he wanted to try exploring the powers of the evil to save the world. I wonder what happened to him?");
                                break;
                        }
                    }
                    /*if (PlayerMod.PlayerHasCompanionSummoned(Main.player[Main.myPlayer], 5)) //Alex isn't implemented yet
                    {
                        PossibleMessages.Add("Oh, you're the TerraGuardian of the tale? You says that her name was " + AlexRecruitScripts.AlexOldPartner + "? I think I remember her, I actually aided her on the beggining of her adventure, years ago, but I didn't heard about her after that.");
                    }*/
                    if (PlayerMod.PlayerHasCompanionSummoned(Main.player[Main.myPlayer], 6))
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                PossibleMessages.Add("The TerraGuardians are leaving the Ether Realm? That is really strange. That didn't happened in a long time. I didn't even existed at the time they used to live here.");
                                break;
                            case 1:
                                PossibleMessages.Add("If you really like the TerraGuardians, then the recent events will certainly be good for you. The only problem is that the fact they are moving from the Ether Realm isn't a good sign.");
                                break;
                            case 2:
                                PossibleMessages.Add("No matter what happens, try recruiting as many TerraGuardians as possible. We will need their help in the future, if my guess is right.");
                                break;
                        }
                    }
                    if (PlayerMod.PlayerHasCompanionSummoned(Main.player[Main.myPlayer], 10))
                    {
                        PossibleMessages.Add("Wait... You're... AAAAAAAAAHHHH!!! PLEASE!! GIVE ME YOUR AUTOGRAPH!!! AAAAAAAAAAAAAHHHHH!!!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Leopold) && WorldMod.HasCompanionNPCSpawned(CompanionDB.Bree))
                    {
                        PossibleMessages.Add("Even though " + WorldMod.GetCompanionNpcName(CompanionDB.Leopold) + " has said something stupid during the Popularity Contest, I'm still her number one fan! " + WorldMod.GetCompanionNpcName(CompanionDB.Leopold)+ "! I love you!!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(11))
                    {
                        PossibleMessages.Add("I don't know the reason, but " + WorldMod.GetCompanionNpcName(11) + " gets extremelly aggressive during Bloodmoons. I recommend you to avoid contact with him during those events.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
                    {
                        PossibleMessages.Add("I know trouble when I see one, and that one is named " + WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + ".");
                    }
                    /*if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().HasGhostFoxHauntDebuff) //Fluffles isn't implemented yet
                    {
                        PossibleMessages.Add("You seems to be haunted by a ghost, gladly that one only needs your help. If you ask for It to show you visions of what is haunting It, you can find clues on how to lift the haunt.");
                        PossibleMessages.Add("I don't know if you know, but there is a giant guardian ghost on your back. That one seems to be wanting your help. If you ask about what's haunting It, you will know how to lift the haunt.");
                    }*/
                    break;
                case Terraria.ID.NPCID.Angler:
                    if (WorldMod.HasCompanionNPCSpawned(0) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("If I see " + WorldMod.GetCompanionNpcName(0) + " again, I will skin him alive, because he keeps stealing all my fish!");
                    if (WorldMod.HasCompanionNPCSpawned(2) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("Did " + WorldMod.GetCompanionNpcName(2) + " told you what is his trick for catching more than one fish at a time?");
                    if (WorldMod.HasCompanionNPCSpawned(8) && Main.rand.NextDouble() < 0.5)
                    {
                        switch(Main.rand.Next(5)){
                            case 0:
                                PossibleMessages.Add(WorldMod.GetCompanionNpcName(8) + " keeps trying to give me vegetables to eat, I don't need vegetables, I have fish!");
                                break;
                            case 1:
                                PossibleMessages.Add("Can't " + WorldMod.GetCompanionNpcName(8) + " just stop telling me to clean my room? She isn't my mom, neither I do need one!");
                                break;
                            case 2:
                                PossibleMessages.Add("Eugh, I'm clean. " + WorldMod.GetCompanionNpcName(8) + " gave me a bath, and removed my fish stench. I'm even hurting in some parts of my body, because the smell wasn't going away.");
                                break;
                            case 3:
                                PossibleMessages.Add(WorldMod.GetCompanionNpcName(8) + " surelly don't want to let me in peace. She always asks how's my day, or asks how I'm feeling.");
                                break;
                            case 4:
                                PossibleMessages.Add("Every night, " + WorldMod.GetCompanionNpcName(8) + " tells me bedtime stories for me to sleep.");
                                break;
                        }
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add("If you don't watch out, " + WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " may take your place as my minion. She's probably waiting for that Sextant.");
                    }
                    break;
                case Terraria.ID.NPCID.TaxCollector:
                    if (WorldMod.HasCompanionNPCSpawned(0))
                        PossibleMessages.Add("I tried to collect the rent from " + WorldMod.GetCompanionNpcName(0) + ", he gave me a pile of trash. What is that supposed to mean?");
                    if (WorldMod.HasCompanionNPCSpawned(1))
                        PossibleMessages.Add("I tried to collect the rent from " + WorldMod.GetCompanionNpcName(1) + ", and she tried to stab me with her sword! Do something about your tenants.");
                    if (WorldMod.HasCompanionNPCSpawned(2))
                        PossibleMessages.Add("Hey! You, look at this. Do you see this wound? That happened when I tried to collect " + WorldMod.GetCompanionNpcName(2) + "'s rent. Now you have to pay for this.");
                    if (WorldMod.HasCompanionNPCSpawned(3))
                        PossibleMessages.Add("I don't think I and " + WorldMod.GetCompanionNpcName(3) + " are speaking in the same language, I asked him for GOLDS, and he VOMITED in my shoes, and you don't want to know the kinds of things that were in it. I had to throw my shoes away and brush VERY hard my feet.");
                    if (WorldMod.HasCompanionNPCSpawned(4))
                        PossibleMessages.Add("I have a job for you, go talk to " + WorldMod.GetCompanionNpcName(4) + " and collect from him my rent. What? I collect the rent? Are you mad?");
                    if (WorldMod.HasCompanionNPCSpawned(5))
                        PossibleMessages.Add("You owe me a new cane. I tried to collect the rent from " + WorldMod.GetCompanionNpcName(5) + " and he tried to bite me! Good thing that my cane was in the way, but now It's nearly breaking.");
                    if (WorldMod.HasCompanionNPCSpawned(6))
                        PossibleMessages.Add("Ow-ow ow ow. That brute lion you hired has hit my head with his huge paw when I asked him for the rent. Didn't he noticed that I'm a fragile old man?");
                    if (WorldMod.HasCompanionNPCSpawned(7))
                        PossibleMessages.Add("What is the problem with your Ether Realm tenants? That psichotic white cat you found tried hit me with a rolling pin when I tried to collect her rent. She even chased me about 50 meters while swinging that thing!");
                    if (WorldMod.HasCompanionNPCSpawned(8))
                    {
                        PossibleMessages.Add("Ah? Nothing, nothing! Uh... Don't tell anyone that you saw me leaving " + WorldMod.GetCompanionNpcName(8) + "'s room, and DARE YOU to comment about the blood coming from my nose!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(9))
                        PossibleMessages.Add("Where were you?! " + WorldMod.GetCompanionNpcName(9) + " nearly sent me back to hell just because I wanted to collect his rent!");
                    if (WorldMod.HasCompanionNPCSpawned(10))
                        PossibleMessages.Add("You're saying you didn't have seen me in a long time? Of course! That crazy bunny " + WorldMod.GetCompanionNpcName(10) + " turned me into a frog! All I did was use my methods of asking for rent!");
                    if (WorldMod.HasCompanionNPCSpawned(11))
                        PossibleMessages.Add("Do you want to talk? Just stay away from me! I need as little physical contact as possible. I tried collecting " + WorldMod.GetCompanionNpcName(11) + "'s rent earlier, and I ended up being hugged for several hours. I nearly wet my pants because of that.");
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Michelle))
                    {
                        PossibleMessages.Add("That girl is the devil! " + WorldMod.GetCompanionNpcName(CompanionDB.Michelle) + " nearly dropped me into a lava pit because I tried to collect her rent!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
                    {
                        PossibleMessages.Add("Missed me? It's because " + WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + " turned me into a frog, and has placed me inside a cage for hours! I had only flies to eat meanwhile, FLIES!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Wrath))
                    {
                        PossibleMessages.Add("The next time I need to collect the rent from " + WorldMod.GetCompanionNpcName(CompanionDB.Wrath) + ", YOU DO THAT! I think he even broke some of my bones!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Alexander))
                    {
                        PossibleMessages.Add("Don't bother me, my back is aching right now! Everytime I visit " + WorldMod.GetCompanionNpcName(CompanionDB.Alexander) + ", he jumps on me and drops me on my back on the floor!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Fluffles))
                    {
                        PossibleMessages.Add("I tried collecting " + WorldMod.GetCompanionNpcName(CompanionDB.Fluffles) + " rent earlier. I didn't found her, until someone saw her on my shoulder. What is her problem?!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Minerva))
                    {
                        PossibleMessages.Add("I charge less rent from " + WorldMod.GetCompanionNpcName(CompanionDB.Minerva) + ". She's the only person who treats me right, and also cooks something whenever I visit.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Liebre))
                    {
                        PossibleMessages.Add("What? Are you nuts? Trying to collect rent from " + WorldMod.GetCompanionNpcName(CompanionDB.Liebre) + " is like asking to be sent to afterlife!");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Glenn))
                    {
                        PossibleMessages.Add("Kid or not, " + WorldMod.GetCompanionNpcName(CompanionDB.Glenn) + " has also to pay for the rent.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.CaptainStench))
                    {
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(CompanionDB.CaptainStench) + " nearly splitted me in half when I tried collecting her rent.");
                    }
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Cinnamon))
                    {
                        PossibleMessages.Add("Can you tell "+WorldMod.GetCompanionNpcName(CompanionDB.Cinnamon) + " to stop tossing pies on my face when I try collecting her rent?");
                    }
                    break;
                case Terraria.ID.NPCID.PartyGirl:
                    if (WorldMod.HasCompanionNPCSpawned(6) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("Looks like " + WorldMod.GetCompanionNpcName(6) + " flourishes when It's his birthday party. He enjoys the most of the special day, I say.");
                    break;
                case Terraria.ID.NPCID.Wizard:
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Malisha))
                    {
                        PossibleMessages.Add("I'm really glad of meeting some one as enthusiast of magic as me, I would have " + WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + " as my apprentice If I had met her earlier.");
                        PossibleMessages.Add(WorldMod.GetCompanionNpcName(CompanionDB.Malisha) + "s researches have quite some interesting results, but some of them are extremelly volatile.");
                    }
                    break;
                case Terraria.ID.NPCID.BestiaryGirl:
                    if (!npc.ShouldBestiaryGirlBeLycantrope())
                    {
                        if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
                        {
                            PossibleMessages.Add(WorldMod.GetCompanionNpcName(CompanionDB.Blue) + " doesn't believe me when I tell her that I'm a Lycantrope.");
                        }
                        if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Alex))
                        {
                            PossibleMessages.Add("What a cute doggie " + WorldMod.GetCompanionNpcName(CompanionDB.Alex) + " is. Who's his owner?");
                        }
                        if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Leopold))
                        {
                            PossibleMessages.Add("It seems like " + WorldMod.GetCompanionNpcName(CompanionDB.Leopold) + " can help me find a cure to my lycantropy. Or at least he's been reading many books regarding that.");
                        }
                    }
                    else
                    {
                        if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
                        {
                            PossibleMessages.Add("*She's growling at " + WorldMod.GetCompanionNpcName(CompanionDB.Blue) + ". Better we keep distance.*");
                        }
                    }
                    break;
            }
            if (PossibleMessages.Count > 0)
            {
                chat = PossibleMessages[Main.rand.Next(PossibleMessages.Count)];
            }
        }
    }
}