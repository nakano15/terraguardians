using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace terraguardians.Companions.Mabel
{
    public class MabelRecruitmentBehavior : PreRecruitNoMonsterAggroBehavior
    {
        public override bool AllowDespawning => true;
        private Companion companion;

        bool Spawn = true;
        byte FallState = 0;
        float FallAnimationFrame = 0;
        ushort SpeechTime = 0;

        public override void Update(Companion companion)
        {
            this.companion = companion;
            switch(FallState)
            {
                case 0:
                    {
                        if (Spawn)
                        {
                            companion.velocity.Y = .1f;
                            Spawn = false;
                            companion.direction = Main.rand.Next(2) == 0 ? -1 : 1;
                        }
                        companion.gfxOffY = 0;
                        companion.SetFallStart();
                        if (companion.velocity.Y == 0)
                        {
                            FallState = 1;
                            companion.position.Y += 48;
                            SoundEngine.PlaySound(SoundID.Item11, companion.Center);
                        }
                        companion.behindBackWall = true;
                    }
                    break;
                case 1:
                    {
                        companion.gfxOffY = 0;
                        int CenterX = (int)(companion.Center.X * (1f / 16) - 1);
                        int CenterY = (int)(companion.Center.Y * (1f / 16));
                        bool FaceOnFloor = false;
                        for(int x = 0; x < 2; x++)
                        {
                            if (Main.tile[CenterX + x, CenterY].HasTile && Main.tileSolid[Main.tile[CenterX + x, CenterY].TileType])
                            {
                                FaceOnFloor = true;
                                break;
                            }
                        }
                        if(!FaceOnFloor)
                        {
                            companion.position.Y += 16;
                        }
                        SpeechTime ++;
                        if (SpeechTime >= 300)
                        {
                            SpeechTime -= 300;
                            companion.SaySomething("*Someone help me!*");
                        }
                        companion.behindBackWall = true;
                    }
                    break;
                case 2:
                    {
                        companion.behindBackWall = false;
                        WanderAI(companion);
                    }
                    break;
            }
        }

        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            if (FallState < 2) drawSet.playerEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically | Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            switch(FallState)
            {
                case 0:
                    companion.BodyFrameID = companion.ArmFramesID[0] = companion.ArmFramesID[1] = companion.Base.GetAnimation(AnimationTypes.JumpingFrames).GetFrame(0);
                    break;
                case 1:
                    companion.ArmFramesID[0] = companion.ArmFramesID[1] = companion.Base.GetAnimation(AnimationTypes.JumpingFrames).GetFrame(0);
                    companion.BodyFrameID = companion.Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(4, ref FallAnimationFrame);
                    break;
            }
        }

        private bool IsntXmasClose { get { return DateTime.Now.Month != 12 || DateTime.Now.Day > 25; }}
        private bool PlayerMetMabel { get { return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion); }}

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            if (!PlayerMetMabel)
            {
                MessageDialogue md = new MessageDialogue("*Thanks for helping me.*");
                md.AddOption("Did you just fell from the sky?", Dialogue0);
                return md;
            }
            else
            {
                MessageDialogue md = new MessageDialogue("*Oh, hello. It's you again.*");
                md.AddOption("Still trying to fly?", DialoguePR0);
                return md;
            }
        }

        public override void ChangeDrawMoment(Companion companion, ref CompanionDrawMomentTypes DrawMomentType)
        {
            if (FallState < 2) DrawMomentType = CompanionDrawMomentTypes.BehindTiles;
        }

        #region Pre Recruit
        private void Dialogue0()
        {
            MessageDialogue md = new MessageDialogue("*Yes, I tried to see If I could fly like a Reindeer. It didn't worked well...*");
            md.AddOption("You doesn't look like a reindeer.", Dialogue1);
            md.RunDialogue();
        }

        private void Dialogue1()
        {
            MessageDialogue md = new MessageDialogue("*And I'm not. My name is " + companion.GetNameColored() + ", by the way*");
            md.AddOption("I'm "+MainMod.GetLocalPlayer.name+".", Dialogue2);
            md.RunDialogue();
        }

        private void Dialogue2()
        {
            MessageDialogue md = new MessageDialogue("*Hello. I have to practice for Miss North Pole, It's going to happen soon, close to when the Xmas you people celebrates happen.*");
            if (IsntXmasClose)
                md.AddOption("But the the holiday has passed.", Dialogue3_xmas);
            else
                md.AddOption("The holiday will happen in some days.", Dialogue3);
            md.RunDialogue();
        }

        private void Dialogue3_xmas()
        {
            MessageDialogue md = new MessageDialogue("*What?! It has already passed? Nooooooooo!!! Well... I guess I can practice for next year?*");
            md.AddOption("At least you'll have more time to practice.", Dialogue4);
            md.RunDialogue();
        }

        private void Dialogue3()
        {
            MessageDialogue md = new MessageDialogue("*It's close to that day?! Oh my... I have so much to practice.*");
            md.AddOption("Yup.", Dialogue4);
            md.RunDialogue();
        }

        private void Dialogue4()
        {
            MessageDialogue md = new MessageDialogue("*Do you mind If I stay on your world for a while, while I practice?*");
            md.AddOption("Yes, you can stay.", LetStayDialogue);
            md.AddOption("This world? No..", NotLetStayDialogue);
            md.RunDialogue();
        }

        private void LetStayDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Thank you! I wonder if the people here are nice like you, too.*");
            companion.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            WorldMod.AllowCompanionNPCToSpawn(companion);
            md.AddOption("Enjoy your stay.", Dialogue.EndDialogue);
            md.RunDialogue();
        }

        private void NotLetStayDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Aww... By the way, If you change your mind, feel free to call me. Bye.*");
            companion.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            md.AddOption("Alright.", Dialogue.EndDialogue);
            md.RunDialogue();
        }
        #endregion

        #region Recruit Known Player
        private void DialoguePR0()
        {
            MessageDialogue md = new MessageDialogue("*Yes, and it's not going well...*");
            md.AddOption("Why don't you stop doing that?", DialoguePR1);
            md.RunDialogue();
        }
        
        private void DialoguePR1()
        {
            MessageDialogue md = new MessageDialogue("*I just can't give up being a model.*");
            md.AddOption("You can try being a model without flying.", DialoguePR2);
            md.RunDialogue();
        }
        
        private void DialoguePR2()
        {
            MessageDialogue md = new MessageDialogue("*I can try... Since I'm here, I guess I'll hang around here.*");
            md.AddOption("You're welcome.", Dialogue.EndDialogue);
            md.RunDialogue();
            WorldMod.AddCompanionMet(companion);
        }
        #endregion
    }
}