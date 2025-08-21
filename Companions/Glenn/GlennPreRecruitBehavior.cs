using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using terraguardians.NPCs.Hostiles;

namespace terraguardians.Companions.Glenn
{
    public class GlennPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        public bool HasSeenHisParents = false, PlayerHasMetGlenn = false;
        public byte DialogueStep = 0;
        public int Delay = 0;
        public Player TargetPlayer = null;

        public override string CompanionNameChange(Companion companion)
        {
            return "Black and White Cat";
        }

        public override void Update(Companion companion)
        {
            bool Idle = true;
            if (DialogueStep == 0)
            {
                TargetPlayer = ViewRangeCheck(companion, companion.direction);
                if (TargetPlayer != null)
                {
                    DialogueStep = 1;
                    PlayerHasMetGlenn = PlayerMod.PlayerHasCompanion(TargetPlayer, CompanionDB.Glenn);
                }
            }
            else
            {
                if (!TargetPlayer.active)
                {
                    DialogueStep = 0;
                    TargetPlayer = null;
                    companion.SaySomething((TargetPlayer.Male ? "He's" : "She's") + " gone! How?");
                }
                else if (TargetPlayer.Distance(companion.Center) > 800f)
                {
                    DialogueStep = 0;
                    TargetPlayer = null;
                    companion.SaySomething("...");
                }
                else if (TargetPlayer.dead)
                {
                    DialogueStep = 0;
                    TargetPlayer = null;
                    companion.SaySomething("I really didn't wanted to witness that.");
                }
                else
                {
                    Idle = false;
                    Companion Bree = PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Bree),
                        Sardine = PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Sardine),
                        Zack = (Sardine == null && Bree == null) ? PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Zack) : null;
                    if (Sardine != null)
                        Sardine.FaceSomething(companion);
                    else if (Bree != null)
                        Bree.FaceSomething(companion);
                    if (Delay > 0)
                        Delay--;
                    else
                    {
                        bool HasBree = Bree != null || PlayerMod.PlayerHasCompanion(TargetPlayer, CompanionDB.Bree),
                            HasSardine = Sardine != null || PlayerMod.PlayerHasCompanion(TargetPlayer, CompanionDB.Sardine);
                        switch (DialogueStep)
                        {
                            case 1:
                                {
                                    if (PlayerHasMetGlenn)
                                    {
                                        companion.FaceSomething(TargetPlayer);
                                        Delay = companion.SaySomething("Hey Terrarian, over here!");
                                    }
                                    else if (Bree != null && Sardine != null)
                                    {
                                        if (Main.rand.NextBool(2))
                                        {
                                            companion.FaceSomething(Bree);
                                        }
                                        else
                                        {
                                            companion.FaceSomething(Sardine);
                                        }
                                        Delay = companion.SaySomething("Mom! Dad! I'm glad to see you again.");
                                    }
                                    else if (Bree != null)
                                    {
                                        companion.FaceSomething(Bree);
                                        Delay = companion.SaySomething("Mom! I'm glad to see you. Have you found dad?");
                                    }
                                    else if (Sardine != null)
                                    {
                                        companion.FaceSomething(Sardine);
                                        Delay = companion.SaySomething("Dad! I'm glad you're alright? Have mom found you?");
                                    }
                                    else
                                    {
                                        companion.FaceSomething(TargetPlayer);
                                        Delay = companion.SaySomething("Hey, Terrarian. Can... Can we talk?");
                                    }
                                    DialogueStep = 2;
                                }
                                break;
                        }
                    }
                }
            }
            if (Idle)
            {
                WanderAI(companion);
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            return GetDialogue();
        }

        MultiStepDialogue GetDialogue()
        {
            MultiStepDialogue dialogue = new MultiStepDialogue();
            Companion Bree = PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Bree),
                Sardine = PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Sardine),
                Zack = (Sardine == null && Bree == null) ? PlayerMod.PlayerGetSummonedCompanion(TargetPlayer, CompanionDB.Zack) : null;
            bool HasBree = Bree != null || PlayerMod.PlayerHasCompanion(TargetPlayer, CompanionDB.Bree),
                HasSardine = Sardine != null || PlayerMod.PlayerHasCompanion(TargetPlayer, CompanionDB.Sardine);
            if (PlayerHasMetGlenn)
            {
                dialogue.AddDialogueStep("I was looking for the world my house is at, but I wasn't expecting to see you here.");
                dialogue.AddDialogueStep("So far, I didn't had any luck with that...");
                dialogue.AddDialogueStep("Anyways, I'm here if you need me.");
                dialogue.AddOption("Okay.", PostRecruitResult);
            }
            else if (Bree != null && Sardine != null)
            {
                dialogue.AddDialogueStep("Glenn! What are you doing here?", Speaker: Sardine);
                dialogue.AddDialogueStep("I came looking for you two. I'm glad to see you alright.");
                dialogue.AddDialogueStep("But It's too dangerous for you to be travelling alone!", Speaker: Bree);
                dialogue.AddDialogueStep("I'm sorry...");
                dialogue.AddDialogueStep("At least you managed to get here safe and sound, that's what's important now.", Speaker: Sardine);
                dialogue.AddDialogueStep("Now we can finally go back home. Do you remember which world our house is at?", Speaker: Bree);
                dialogue.AddDialogueStep("I... Forgot...");
                dialogue.AddDialogueStep("...", Speaker: Bree);
                dialogue.AddDialogueStep("It seems like we'll have to live here for a little longer... Or at least until we remember where our home is...", Speaker: Bree);
                dialogue.AddDialogueStep("Oh yeah, I never got to introduce myself. I'm " + Dialogue.Speaker.GetNameColored() + ". I hope you don't mind if I stay here with my parents.");
                dialogue.AddOption("I'm " + MainMod.GetLocalPlayer.name + ".", PostRecruitResult);
            }
            else if (Bree != null)
            {
                dialogue.AddDialogueStep("Glenn!! What are you doing here? I told you to stay at home!", Speaker: Bree);
                dialogue.AddDialogueStep("Yes, but you two were gone for quite long, and I got worried...");
                if (HasSardine)
                {
                    dialogue.AddDialogueStep("Don't worry about your dad, we found him and he's safe.", Speaker: Bree);
                    dialogue.AddDialogueStep("Yay! That's nice, Mom!");
                }
                dialogue.AddDialogueStep("But It's too dangerous for you to be travelling alone!", Speaker: Bree);
                dialogue.AddDialogueStep("I'm sorry...");
                dialogue.AddDialogueStep("Nevermind... At least you're unharmed. Now little boy, time to go back home. Do you know which world our house is at?", Speaker: Bree);
                dialogue.AddDialogueStep("I... Forgot...");
                dialogue.AddDialogueStep("...", Speaker: Bree);
                dialogue.AddDialogueStep("Looks like we'll have to stay here for longer...", Speaker: Bree);
                dialogue.AddDialogueStep("Alright mom. Oh yeah, I don't think we introduced ourselves. My name's "+Dialogue.Speaker.GetNameColored()+".\nSeems like I'll be living here for a while too.");
                dialogue.AddOption("I'm " + MainMod.GetLocalPlayer.name + ".", PostRecruitResult);
            }
            else if (Sardine != null)
            {
                dialogue.AddDialogueStep("Glenn! What are you doing here?", Speaker: Sardine);
                dialogue.AddDialogueStep("I came looking for you and Mom. She left few months ago to look for you and haven't returned.\nI got worried.");
                dialogue.AddDialogueStep("You should have stayed at home. The world is too dangerous for a little kitty to be walking around.", Speaker: Sardine);
                dialogue.AddDialogueStep("I'm sorry...");
                if(HasBree)
                {
                    dialogue.AddDialogueStep("At least it's good to see that you and your Mother are safe..", Speaker: Sardine);
                    dialogue.AddDialogueStep("You found my mom? She's safe?");
                    dialogue.AddDialogueStep("Yes, she's safe. I doubt she will be happy for finding out you were exploring by yourself, though.", Speaker: Sardine);
                    dialogue.AddDialogueStep("I'll be grounded for that?");
                    dialogue.AddDialogueStep("I don't know, maybe. She'll probably be more busy trying to see if you're hurt than thinking of the punishment", Speaker: Sardine);
                    dialogue.AddDialogueStep("Haha... Ha...");
                    dialogue.AddDialogueStep("Your mom is itching to go back home, so time for us to go back. Do you remember the way home?", Speaker: Sardine);
                }
                else
                {
                    dialogue.AddDialogueStep("At least I'm glad you're safe, but I'm now worried about your mother.", Speaker: Sardine);
                    dialogue.AddDialogueStep("Lets go look for her.");
                    dialogue.AddDialogueStep("You will actually stay at home. I'll go look for her. You took heavy risks trying to look for us.", Speaker: Sardine);
                    dialogue.AddDialogueStep("Oh... Okay dad...");
                    dialogue.AddDialogueStep("I'll begin that once I get you back home. Do you remember the way home?", Speaker: Sardine);
                }
                dialogue.AddDialogueStep("I... Forgot...");
                dialogue.AddDialogueStep("...I guess you'll have to stay with us here for a while...", Speaker: Sardine);
                dialogue.AddDialogueStep("Alright! Oh, I didn't introduced myself to you. I'm "+Dialogue.Speaker.GetNameColored()+". What's your name?");
                dialogue.AddOption("I'm " + MainMod.GetLocalPlayer.name + ".", PostRecruitResult);
            }
            else
            {
                dialogue.AddDialogueStep("Hello. Have you seen a Black Cat, or a White Cat that look like me, but taller?");
                if (HasSardine || HasBree)
                {
                    dialogue.AddOption("Yes, I have seen them.", OnPlayerHasHisParents);
                }
                dialogue.AddOption("No, I haven't.", OnPlayerDoesntHasHisParents);
            }
            return dialogue;
        }

        void OnPlayerHasHisParents()
        {
            MessageDialogue md = new MessageDialogue("Can you please call them here? My feet are really tired...");
            md.RunDialogue();
        }

        void OnPlayerDoesntHasHisParents()
        {
            MessageDialogue md = new MessageDialogue("If you see them, please let me know.");
            md.RunDialogue();
        }

        void PostRecruitResult()
        {
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
            Dialogue.LobbyDialogue();
            DialogueStep = 0;
            Delay = 0;
            TargetPlayer = null;
        }
    }
}