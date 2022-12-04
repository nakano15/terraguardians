using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class RococoBase : CompanionBase
    {
        public override string Name => "Rococo";
        public override string Description => "He's a good definition of a big kid, very playful and innocent.\nLoves playing kids games, like Hide and Seek.";
        public override int Age => 15;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override bool CanCrouch => true;
        public override int Width => 28;
        public override int Height => 86;
        public override float Scale => 94f / 86;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 200;
        public override int HealthPerLifeCrystal => 40;
        public override int HealthPerLifeFruit => 10;
        //public override float Gravity => 0.5f;
        public override float MaxRunSpeed => 5.2f;
        public override float RunAcceleration => 0.18f;
        public override float RunDeceleration => 0.47f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.08f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.DD2_KoboldHurt;
        public override SoundStyle DeathSound => Terraria.ID.SoundID.DD2_KoboldDeath;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MoveInUnlock = 0, VisitUnlock = 1 };
        public override BehaviorBase PreRecruitmentBehavior => new terraguardians.Companions.Rococo.RococoRecruitmentBehavior();
        #region  Animations
        protected override Animation SetWalkingFrames {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24); //8
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames 
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 16; i <= 19; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 12; i++) anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(20);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(21, 1);
                anim.AddFrame(22, 1);
                anim.AddFrame(12, 1);
                return anim;
            }
        }
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer f = new AnimationFrameReplacer();
                f.AddFrameToReplace(23, 0);
                return f;
            }
        }
        protected override Animation SetChairSittingFrames => new Animation(23);
        protected override Animation SetSittingFrames => new Animation(23);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetThroneSittingFrames => new Animation(24);
        protected override Animation SetBedSleepingFrames => new Animation(25);
        protected override Animation SetRevivingFrames => new Animation(26);
        protected override Animation SetDownedFrames => new Animation(27);
        protected override Animation SetPetrifiedFrames => new Animation(28);
        protected override Animation SetBackwardStandingFrames => new Animation(29);
        protected override Animation SetBackwardReviveFrames => new Animation(30);
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(new Vector2(18, 31), true), 
                    new AnimationPositionCollection(new Vector2(30, 31), true)
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 8, 10);
                Hands[0].AddFramePoint2X(11, 32, 9);
                Hands[0].AddFramePoint2X(12, 44, 37);
                
                Hands[0].AddFramePoint2X(16, 15, 4);
                Hands[0].AddFramePoint2X(17, 35, 7);
                Hands[0].AddFramePoint2X(18, 40, 19);
                Hands[0].AddFramePoint2X(19, 35, 31);
                
                Hands[0].AddFramePoint2X(21, 34, 14);
                Hands[0].AddFramePoint2X(22, 44, 29);
                
                Hands[0].AddFramePoint2X(26, 34, 41);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 8, 10);
                Hands[1].AddFramePoint2X(11, 32, 9);
                Hands[1].AddFramePoint2X(12, 44, 37);
                
                Hands[1].AddFramePoint2X(16, 15, 4);
                Hands[1].AddFramePoint2X(17, 35, 7);
                Hands[1].AddFramePoint2X(18, 40, 19);
                Hands[1].AddFramePoint2X(19, 36, 31);
                
                Hands[1].AddFramePoint2X(21, 36, 16);
                Hands[1].AddFramePoint2X(22, 44, 29);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition 
        {
            get
            {
                AnimationPositionCollection animation = new AnimationPositionCollection(new Vector2(18,14), true);
                animation.AddFramePoint2X(11, 22, 20);
                animation.AddFramePoint2X(12, 30, 27);
                animation.AddFramePoint2X(20, 30, 27);
                animation.AddFramePoint2X(21, 30, 27);
                animation.AddFramePoint2X(22, 30, 27);

                animation.AddFramePoint2X(24, 16, 20);
                animation.AddFramePoint2X(25, 25, 28);
                return animation;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(new Vector2(23, 37), true);
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16);
        #endregion
        #region Dialogue
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "\"At first, the creature got surprised after seeing me, then starts laughing out of happiness.\"";
                case 1:
                    return "\"That creature waves at you while smiling, It must be friendly, I guess?\"";
                case 2:
                    return "\"For some reason, that creature got happy after seeing you, maybe It wasn't expecting another human in this world?";
                default:
                    return "\"What sort of creature is that? Is it dangerous? No, It doesn't looks like it.\"";
            }
        }

        public override string NormalMessages(Companion guardian)
        {
            bool MerchantInTheWorld = NPC.AnyNPCs(NPCID.Merchant), SteampunkerInTheWorld = NPC.AnyNPCs(NPCID.Steampunker);
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon && !Main.eclipse)
            {
                Mes.Add("*[name] is happy for seeing you.*");
                Mes.Add("*[name] asks if you brought him something to eat.*");
                Mes.Add("*[name] is asking if you want to play with him.*");
                Mes.Add("*[name] wants you to check some of his toys.*");
                Mes.Add("*[name] seems very glad to see you safe.*");
                if(!guardian.IsFollower)
                    Mes.Add("*[name] is asking you if you came to ask him to go on an adventure.*");
                if (guardian.HasBuff(Terraria.ID.BuffID.WellFed))
                {
                    Mes.Add("*[name] thanks you for the food.*");
                    Mes.Add("*[name] seems to be relaxing after eating something.*");
                }
                else
                {
                    Mes.Add("*You can hear [name]'s stomach growl.*");
                    Mes.Add("*[name] seems to be a bit hungry.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*[name] asks you what's up.*");
                    Mes.Add("*[name] is telling that is liking the weather.*");
                }
                else
                {
                    Mes.Add("*[name] seems to be watching some classic horror movie on the tv... No, wait, that's a window.*");
                    Mes.Add("*[name] is trying to hide behind you, he seems scared of the monsters.*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    if (MerchantInTheWorld)
                        Mes.Add("*As soon as [name] started talking, you hastily asked him to stop, because of the bad trash breath that comes from his mouth.*");
                    Mes.Add("*[name] is sleeping while awake.*");
                    Mes.Add("*[name] is trying hard to keep It's eyes opened.*");
                    Mes.Add("*[name] seems sleepy.*");
                }
                else
                {
                    Mes.Add("*[name] looks scared.*");
                    Mes.Add("*[name] is trembling in terror..*");
                    Mes.Add("*[name] asks if his house is safe.*");
                }
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*[name] seems to be enjoying the party.*");
            }
            if (SteampunkerInTheWorld)
                Mes.Add("*[name] is talking something about a jetpack joyride?*");
            if (MainMod.HasCompanionInWorld(1))
            {
                Mes.Add("*[name] seems to be crying, and with a purple left eye, I guess his dialogue with [gn:1] went wrong.*");
                Mes.Add("*[name] seems to be crying, and with his right cheek having a huge red paw marking, I wonder what he were talking about with [gn:1].*");
            }
            if (MainMod.HasCompanionInWorld(3))
            {
                Mes.Add("*[name] seems to have gotten kicked in his behind. Maybe he annoyed [gn:3]?*");
            }
            Player player = Main.LocalPlayer;
            if (PlayerMod.PlayerHasCompanionSummoned(player, 2) && PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("*[gn:2] is telling [name] that he's lucky that [gn:1] doesn't plays her terrible games with him. But [name] insists that he wanted to play.*");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("*[name] asked [gn:1] why she doesn't plays with him, she told him that she can't even bear seeing him.*");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3) && PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("*[name] asked [gn:3] why he doesn't plays with him, he told him that It's because he makes [gn:1] upset.*");
            }
            if (MainMod.HasCompanionInWorld(5))
            {
                Mes.Add("*[name] says that loves playing with [gn:5], but wonders why he always find him on hide and seek.*");
                Mes.Add("*[name] says that bringing [gn:5] made the town very livelly.*");
            }
            if (MainMod.HasCompanionInWorld(8))
            {
                Mes.Add("*[name] said that [gn:8] looks familiar, have they met before?*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Vladimir))
            {
                Mes.Add("*[name] hugs you. It feels a bit weird. He never hugged you without a reason.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles))
            {
                Mes.Add("*[name] says that sometimes he feels weird when [gn:" + CompanionDB.Fluffles + "] stares at him for too long.*");
                Mes.Add("*[name] is asking you if you know why [gn:" + CompanionDB.Fluffles + "] looks at him, with her paw on the chin.*");
                if (MainMod.HasCompanionInWorld(CompanionDB.Alex))
                {
                    Mes.Add("*[name] says that playing with [gn:"+CompanionDB.Alex+"] and [gn:"+CompanionDB.Fluffles+"] has been one of the most enjoyable things he has done, and asks you to join too.*");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Glenn))
            {
                Mes.Add("*[name] is telling you that [gn:" + CompanionDB.Glenn + "] is his newest friend.*");
                Mes.Add("*[name] says that loves playing with [gn:" + CompanionDB.Glenn + "].*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Cinnamon))
            {
                Mes.Add("*[name] says that after meeting [gn:" + CompanionDB.Cinnamon + "], he has been eating several tasty foods.*");
                Mes.Add("*[name] asks what is wrong with the seasonings he brings to [gn:" + CompanionDB.Cinnamon + "].*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Luna))
            {
                Mes.Add("*[name] is really happy for having [gn:"+CompanionDB.Luna+"] around. He really seems to like her.*");
                Mes.Add("*[name] seems to be expecting [gn:"+CompanionDB.Luna+"]'s visit.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*[name] seems happy that you asked, and agrees to follow you.*";
                case JoinMessageContext.FullParty:
                    return "*[name] says that is worried about the number of people in your group. He sees no way of fitting in it.*";
                case JoinMessageContext.Fail:
                    return "*[name] doesn't feel okay with joining your group right now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.Success:
                    return "*[name] gives you a farewell, and tells you that had fun on the adventure.*";
                case LeaveMessageContext.AskIfSure:
                    return "*[name] seems worried about leaving the group outside of a safe place.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*[name] tells you to be careful on your travels, and that will see you back at home.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*[name] seems relieved when you changed your mind.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*[name] happily allows. He then picked you up and put you on his shoulder.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*[name] is happy that you're willing to carry him.*";
                case MountCompanionContext.Fail:
                    return "*[name] doesn't think this is a good moment for that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*[name] refused. Maybe he doesn't entirelly trust you.*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*[name] nodded, and then placed you on the ground.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*[name] said yes, and then got off your shoulder.*";
                case DismountCompanionContext.Fail:
                    return "*[name] doesn't think this is a good moment for that.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*[name] happily accepted to live here with you.*";
                case MoveInContext.Fail:
                    return "*[name] is saddened to tell you that he can't.*";
                case MoveInContext.NotFriendsEnough:
                    return "*[name] doesn't fully trust you to stay here in this world.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*[name] begun crying as he packs his things to leave.*";
                case MoveOutContext.Fail:
                    return "*[name] tells you that now he can't leave.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*[name] told you that he wont be moving out.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }
        #endregion
    }
}