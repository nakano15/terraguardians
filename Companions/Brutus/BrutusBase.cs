using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class BrutusBase : TerraGuardianBase
    {
        public override string Name => "Brutus";
        public override string Description => "He was once a member of the Royal Guard\non the Ether Realm. Now is just a body guard.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 28;
        public override int Height => 92;
        public override int CrouchingHeight => 52;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int SpriteWidth => 112;
        public override int SpriteHeight => 112;
        public override float Scale => 110f / 92;
        public override int FramesInRow => 17;
        public override int Age => 28;
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 225; //1400
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 25;
        public override float AccuracyPercent => 0.63f;
        public override float MaxRunSpeed => 5.05f;
        public override float RunAcceleration => 0.22f;
        public override float RunDeceleration => 0.33f;
        public override int JumpHeight => 13;
        public override float JumpSpeed => 8.3f;
        public override bool CanCrouch => true;
        protected override CompanionDialogueContainer GetDialogueContainer => new BrutusDialogues();
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0, MoveInUnlock = 0, MountUnlock = 0 };
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.CobaltSword),
                new InitialItemDefinition(ItemID.HealingPotion, 5),
                new InitialItemDefinition(ItemID.CobaltRepeater),
                new InitialItemDefinition(ItemID.CursedArrow, 250)
            };
        }
        public override void UpdateAttributes(Companion companion)
        {
            companion.DefenseRate += 0.1f;
        }
        public override BehaviorBase DefaultFollowLeaderBehavior => new Brutus.BrutusFollowerBehavior();
        public override BehaviorBase PreRecruitmentBehavior => new Brutus.BrutusPreRecruitmentBehavior();
        public override BehaviorBase DefaultCombatBehavior => new Brutus.BrutusCombatBehavior();
        public override bool CanSpawnNpc()
        {
            int TownNpcCount = (int)(WorldMod.GetCompanionsCount * 0.5f);
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC)
                    TownNpcCount++;
            }
            return TownNpcCount >= 2;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(14);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 16; i <= 18; i++) anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i <= 13; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(19);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 20; i  <= 22; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(15);
        protected override Animation SetChairSittingFrames => new Animation(23);
        protected override Animation SetThroneSittingFrames => new Animation(24);
        protected override Animation SetBedSleepingFrames => new Animation(25);
        protected override Animation SetRevivingFrames => new Animation(26);
        protected override Animation SetDownedFrames => new Animation(27);
        protected override Animation SetPetrifiedFrames => new Animation(29);
        protected override Animation SetBackwardStandingFrames => new Animation(30);
        protected override Animation SetBackwardReviveFrames => new Animation(31);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(15, 0);
                anim.AddFrameToReplace(23, 1);
                anim.AddFrameToReplace(32, 2);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer rightarm = new AnimationFrameReplacer();
                rightarm.AddFrameToReplace(24, 0);
                return new AnimationFrameReplacer[]{ new AnimationFrameReplacer(), rightarm };
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16, false);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection LeftArm = new AnimationPositionCollection(), RightArm = new AnimationPositionCollection();
                LeftArm.AddFramePoint2X(0, 20, 35);
                LeftArm.AddFramePoint2X(1, 27, 30);
                LeftArm.AddFramePoint2X(2, 29, 29);
                LeftArm.AddFramePoint2X(3, 27, 30);
                LeftArm.AddFramePoint2X(4, 25, 31);
                LeftArm.AddFramePoint2X(5, 21, 29);
                LeftArm.AddFramePoint2X(6, 21, 29);
                LeftArm.AddFramePoint2X(7, 23, 30);
                LeftArm.AddFramePoint2X(8, 25, 31);
                LeftArm.AddFramePoint2X(9, 25, 28);
                
                LeftArm.AddFramePoint2X(10, 17, 17);
                LeftArm.AddFramePoint2X(11, 37, 12);
                LeftArm.AddFramePoint2X(12, 43, 24);
                LeftArm.AddFramePoint2X(13, 37, 36);
                
                LeftArm.AddFramePoint2X(16, 8, 21);
                LeftArm.AddFramePoint2X(17, 46, 20);
                LeftArm.AddFramePoint2X(18, 50, 45);
                
                LeftArm.AddFramePoint2X(20, 16, 12);
                LeftArm.AddFramePoint2X(21, 39, 20);
                LeftArm.AddFramePoint2X(22, 39, 37);
                
                LeftArm.AddFramePoint2X(26, 43, 48);

                RightArm.AddFramePoint2X(0, 34, 35);
                for (short i = 1; i <= 8; i++)
                    RightArm.AddFramePoint2X(i, 39, 29);
                
                RightArm.AddFramePoint2X(10, 23, 7);
                RightArm.AddFramePoint2X(11, 41, 12);
                RightArm.AddFramePoint2X(12, 46, 25);
                RightArm.AddFramePoint2X(13, 40, 36);
                
                RightArm.AddFramePoint2X(16, 36, 18);
                RightArm.AddFramePoint2X(17, 45, 32);
                RightArm.AddFramePoint2X(18, 51, 47);
                
                RightArm.AddFramePoint2X(20, 20, 12);
                RightArm.AddFramePoint2X(21, 42, 20);
                RightArm.AddFramePoint2X(22, 42, 37);
                return new AnimationPositionCollection[]{ LeftArm, RightArm };
            }
        }

        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(18, 20), true);
                anim.AddFramePoint2X(16, 15, 29);
                anim.AddFramePoint2X(17, 28, 21);
                anim.AddFramePoint2X(18, 36, 32);
                
                for (short i = 19; i <= 22; i++)
                    anim.AddFramePoint2X(i, 19, 28);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(new Vector2(27, 40), true);
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(28, 14), true);
                anim.AddFramePoint2X(16, 17, 16);
                anim.AddFramePoint2X(17, 33, 17);
                anim.AddFramePoint2X(18, 42, 28);
                
                for (short i = 19; i <= 22; i++)
                    anim.AddFramePoint2X(i, 28, 21);
                
                anim.AddFramePoint2X(24, 25, 21);

                anim.AddFramePoint2X(26, 41, 31);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(6, -4), true);
                anim.AddFramePoint2X(24, -8, -16);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(Vector2.UnitY * 2, true);
        #endregion

        public override void ModifyAnimation(Companion companion)
        {
            if (companion.sleeping.isSleeping && companion.Owner != null && companion.Owner is Player)
            {
                Player p = (companion.Owner as Player);
                if (p.sleeping.isSleeping && p.Bottom == companion.Bottom)
                {
                    companion.BodyFrameID = 
                    companion.ArmFramesID[0] = 
                    companion.ArmFramesID[1] = 32;
                }
            }
        }
    }
}