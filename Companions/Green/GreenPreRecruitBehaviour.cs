using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using terraguardians.NPCs.Hostiles;
using System;

namespace terraguardians.Companions.Green
{
    public class GreenPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        bool FirstFrame = true, Sleeping = true, PlayerKnowsCompanion = false;
        byte Step = 0;
        int Timer = 0;
        bool PlayerMovedAway = false;
        Vector2 SpawnPosition = Vector2.Zero;

        public override string CompanionNameChange(Companion companion)
        {
            return "Snake Guardian";
        }

        public override void Update(Companion companion)
        {
            SpawnScript(companion);
            if (!SleepingBehaviour(companion))
            {
                Player player = MainMod.GetLocalPlayer;
                if (Step > 0)
                {
                    if (Step < 5 && MathF.Abs(player.Center.X - companion.Center.X) >= 300)
                    {
                        companion.SaySomething("*Hey! I'm talking to you! Come back here!*");
                        Step = 1;
                        Timer = 0;
                        PlayerMovedAway = true;
                    }
                    else
                    {
                        if (PlayerMovedAway)
                        {
                            if (PlayerKnowsCompanion)
                            {
                                companion.SaySomething("*You can't run away from listening to me again!*");
                            }
                            else
                            {
                                companion.SaySomething("*You returned. Now you have to listen to me.*");
                            }
                            Timer = -120;
                            PlayerMovedAway = false;
                        }
                    }
                    if (companion.velocity.X == 0)
                        companion.FaceSomething(player);
                }
                switch (Step)
                {
                    case 0:
                        {
                            if (companion.velocity.Y == 0)
                            {
                                Timer++;
                                if (Timer >= 150)
                                {
                                    Timer = 0;
                                    Step++;
                                }
                            }
                        }
                        break;
                    case 1:
                        {
                            if (Timer == 50)
                            {
                                if (PlayerKnowsCompanion)
                                {
                                    companion.SaySomething("*Again you destroyed the tree I was sleeping on.*");
                                }
                                else
                                {
                                    companion.SaySomething("*What is your problem? Didn't see me there?*");
                                }
                            }
                            Timer++;
                            if (Timer >= 260)
                            {
                                Timer = 0;
                                Step++;
                            }
                        }
                        break;
                    case 2:
                        {
                            if (Timer == 0)
                            {
                                if (PlayerKnowsCompanion)
                                {
                                    companion.SaySomething("*Do you have anything against me sleeping on trees or didn't looked up again?*");
                                }
                                else
                                {
                                    companion.SaySomething("*Next time you want to get some lumber, look up-!*");
                                }
                            }
                            Timer++;
                            if (Timer >= 210)
                            {
                                Timer = 0;
                                Step++;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (Timer == 0)
                            {
                                if (PlayerKnowsCompanion)
                                {
                                    companion.SaySomething("*Ugh... Whatever...*");
                                }
                                else
                                {
                                    companion.SaySomething("*Wait a minute. Aren't you a Terrarian?*");
                                }
                            }
                            Timer++;
                            if (Timer >= 180)
                            {
                                Timer = 0;
                                Step++;
                            }
                        }
                        break;
                    case 4:
                        {
                            if (Timer == 0)
                            {
                                if (PlayerKnowsCompanion)
                                {
                                    companion.SaySomething("*I'm here if you ever need a medic... Now I need to look for a safe place for a nap.*");
                                    WorldMod.AddCompanionMet(companion);
                                    return;
                                }
                                else
                                {
                                    companion.SaySomething("*Hm... I wonder if...*");
                                }
                            }
                            Timer++;
                            if (Timer >= 180)
                            {
                                Timer = 0;
                                Step++;
                            }
                        }
                        break;
                    case 5:
                        {
                            if (Timer == 0)
                            {
                                companion.SaySomething("*If you can understand me, could you try speaking to me?*");
                                Timer++;
                            }
                        }
                        break;
                    default:
                        {
                            if (Step >= 6 && !Dialogue.IsParticipatingDialogue(companion))
                            {
                                Step = 5;
                                Timer = 1;
                                companion.SaySomething("*I still would like if we could speak some more, Terrarian.*");
                            }
                        }
                        break;
                }
            }
            if (PlayerMovedAway)
            {
                base.Update(companion);
            }
        }

        void SpawnScript(Companion companion)
        {
            if (!FirstFrame) return;
            FirstFrame = false;
            int centertilex = (int)(companion.Center.X * Companion.DivisionBy16),
                centertiley = (int)(companion.Center.Y * Companion.DivisionBy16);
            SpawnPosition = companion.position;
            for (int y = 0; y >= -4; y--)
            {
                for (int x = -2; x < 3; x++)
                {
                    int TileX = centertilex + x,
                        TileY = centertiley + y;
                    Tile tile = Main.tile[TileX, TileY];
                    if (tile.HasTile && tile.TileType == TileID.Trees)
                    {
                        int SizeCount = 0;
                        while (Main.tile[TileX, TileY].HasTile && Main.tile[TileX, TileY].TileType == TileID.Trees)
                        {
                            TileY--;
                            SizeCount++;
                        }
                        TileY += 3;
                        if (SizeCount >= 9)
                        {
                            companion.position = new Vector2(TileX * 16f - companion.width * .5f + 8f, TileY * 16f - companion.height * .5f);
                            SpawnPosition = companion.position;
                            return;
                        }
                    }
                }
            }
        }

        bool SleepingBehaviour(Companion companion)
        {
            if (!Sleeping)
                return false;
            companion.velocity = Vector2.UnitY * -companion.Base.Gravity; //He's not staying on the tree.
            companion.position = SpawnPosition;
            int TileCenterX = (int)(companion.Center.X * Companion.DivisionBy16),
                TileCenterY = (int)(companion.Center.Y * Companion.DivisionBy16);
            Tile tile = Framing.GetTileSafely(TileCenterX, TileCenterY);
            if (tile != null && !tile.HasTile)
            {
                Sleeping = false;
                companion.SaySomething("*W-what?!*");
                PlayerKnowsCompanion = PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Green);
            }
            return true;
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return Step == 5;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            return GetDialogue();
        }

        #region Dialogues
        MessageBase GetDialogue()
        {
            MessageDialogue md = new MessageDialogue("*I see, you Terrarians can also speak to us... Good.*");
            md.AddOption("Speak to us?", ProceedStep1);
            return md;
        }

        void ProceedStep1()
        {
            Step = 6;
            Timer = 0;
            MessageDialogue md = new MessageDialogue("*Yes, we TerraGuardians speaks mostly from ways out of the conventional. Gladly seems like the bond let us understand each other.*");
            AddStep1Questions(md);
            md.RunDialogue();
        }

        void AddStep1Questions(MessageDialogue md)
        {
            if (Step == 6)
            {
                if (Timer != 1)
                    md.AddOption("Who are you?", AskWhoAreYou);
                if (Timer != 2)
                    md.AddOption("What are you doing here?", AskWhatAreYouDoingHere);
            }
            else
            {
                md.AddOption("Medic? You?", AskIfHesAMedic);
                md.AddOption("Exploring this world?", AskWhyHesExploringTheWorld);
            }
        }

        void AskWhoAreYou()
        {
            MessageDialogue md = new MessageDialogue("*I'm a medic from the Ether Realm. I have treated many patients there, but right now I'm exploring this world.*");
            if (Timer > 0)
            {
                Step = 7;
                Timer = 2;
            }
            else
            {
                Timer = 1;
            }
            AddStep1Questions(md);
            md.RunDialogue();
        }

        void AskWhatAreYouDoingHere()
        {
            MessageDialogue md = new MessageDialogue("*I was exploring this world, until I was getting tired of wandering, so I went atop that tree to sleep, until you put it all down.*");
            if (Timer > 0)
            {
                Step = 7;
                Timer = 2;
            }
            else
            {
                Timer = 2;
            }
            AddStep1Questions(md);
            md.RunDialogue();
        }

        void AskIfHesAMedic()
        {
            MessageDialogue md = new MessageDialogue("*Believe it or not. Even though my face may look intimidating, I take my job very seriously.*");
            GetStep7Option(md);
            md.RunDialogue();
        }

        void AskWhyHesExploringTheWorld()
        {
            MessageDialogue md = new MessageDialogue("*I lived on the Ether Realm since I was an egg, literally. I wanted to see how this \"new world\" looks.*");
            GetStep7Option(md);
            md.RunDialogue();
        }

        void GetStep7Option(MessageDialogue md)
        {
            md.AddOption("What you're going to do now?", AskWhatHesGoingToDo);
        }

        void AskWhatHesGoingToDo()
        {
            MessageDialogue md = new MessageDialogue("*Since you demolished my slumber spot, there isn't much I can do. But meeting you has just made me curious about your kind...*");
            md.AddOption("About me?", AskAboutYourKind);
            md.RunDialogue();
        }

        void AskAboutYourKind()
        {
            MessageDialogue md = new MessageDialogue("*Not in the sense you are thinking... Hm... Say, would you mind if I stayed here, and studied your people? I may end up being helpful in case one of you end up getting injured or sick, once I discover how your bodies work.*");
            md.AddOption("Yes.", OnYesSaid);
            md.AddOption("No.", OnNoSaid);
            md.RunDialogue();
        }

        void OnYesSaid()
        {
            MessageDialogue md = new MessageDialogue("*Thank you, this is of great importance to me. My name is Jochen Green, but you can call me Dr. Green, instead.*");
            EndRecruitmentDialogue(md, true);
        }

        void OnNoSaid()
        {
            MessageDialogue md = new MessageDialogue("*Well, that's sad. Anyways, you can keep contact of me in case you change your mind. My name is Jochen Green. You can call me Dr. Green, instead.*");
            EndRecruitmentDialogue(md, false);
        }

        void EndRecruitmentDialogue(MessageDialogue md, bool MoveIn)
        {
            md.AddOption("Hello.", Dialogue.LobbyDialogue);
            md.RunDialogue();
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Ich);
            WorldMod.AddCompanionMet(CompanionDB.Ich);
            if (MoveIn)
            {
                WorldMod.AllowCompanionNPCToSpawn(CompanionDB.Ich);
            }
        }
        #endregion

        public override void UpdateStatus(Companion companion)
        {
            if (Sleeping)
                companion.GravityPower = 0;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (Sleeping)
            {
                short Frame = companion.Base.GetAnimation(AnimationTypes.BedSleepingFrames).GetFrame(0);
                companion.BodyFrameID = Frame;
                for (int i = 0; i < 2; i++)
                    companion.ArmFramesID[i] = Frame;
            }
            else if (companion.velocity.Y == 0 && Step == 0)
            {
                short Frame = companion.Base.GetAnimation(AnimationTypes.DownedFrames).GetFrame(0);
                companion.BodyFrameID = Frame;
                for (int i = 0; i < 2; i++)
                    companion.ArmFramesID[i] = Frame;
            }
        }
    }
}