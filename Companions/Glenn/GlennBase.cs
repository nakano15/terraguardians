using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;
using terraguardians.Companions.Glenn;

namespace terraguardians.Companions
{
    public class GlennBase : TerraGuardianBase
    {
        public override string Name => "Glenn";
        public override string FullName => "Glenn Alexander";
        public override string Description => "Interested in literature and games.\nCan stay up all night because of that.";
        public override Sizes Size => Sizes.Small;
        public override TalkStyles TalkStyle => TalkStyles.Normal;
        public override bool HelpAlliesOverFighting => true;
        public override int Width => 14;
        public override int Height => 38;
        public override int SpriteWidth => 64;
        public override int SpriteHeight => 56;
        public override float Scale => 32f / 38;
        public override int FramesInRow => 25;
        public override int FavoriteFood => ItemID.PotatoChips;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int Age => 11;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Winter, 25);
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 70; //280
        public override int HealthPerLifeCrystal => 10;
        public override int HealthPerLifeFruit => 3;
        public override float AccuracyPercent => 0.88f;
        public override float MaxRunSpeed => 4.80f;
        public override float RunAcceleration => 0.18f;
        public override float RunDeceleration => 0.9f;
        public override int JumpHeight => 12;
        public override float JumpSpeed => 9.52f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer;
        public override CompanionGroup GetCompanionGroup => MainMod.GetCaitSithGroup;
        public override bool CanUseHeavyItem => false;
        public override bool AllowSharingBedWithPlayer => false;
        public override SoundStyle HurtSound => SoundID.NPCHit51;
        public override SoundStyle DeathSound => SoundID.NPCDeath54;
        public override bool CanSpawnNpc()
        {
            return NPC.downedGoblins;
        }
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks()
        {
            FollowerUnlock = 3,
            MoveInUnlock = 0
        };
        protected override CompanionDialogueContainer GetDialogueContainer => new Glenn.GlennDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new GlennPreRecruitBehavior();

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = [
                new InitialItemDefinition(ItemID.SilverBroadsword),
                new InitialItemDefinition(ItemID.Shuriken),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            ];
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++) anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 13; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(15);
        protected override Animation SetPlayerMountedArmFrame => new Animation(15);
        protected override Animation SetChairSittingFrames => new Animation(14);
        protected override Animation SetThroneSittingFrames => new Animation(16);
        protected override Animation SetBedSleepingFrames => new Animation(17);
        protected override Animation SetRevivingFrames => new Animation(18);
        protected override Animation SetDownedFrames => new Animation(19);
        protected override Animation SetBackwardStandingFrames => new Animation(20);
        protected override Animation SetBackwardReviveFrames => new Animation(21);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                anim.AddFrameToReplace(15, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),right = new AnimationPositionCollection();

                left.AddFramePoint2X(10, 14, 8);
                left.AddFramePoint2X(11, 20, 13);
                left.AddFramePoint2X(12, 22, 17);
                left.AddFramePoint2X(13, 20, 22);
                
                left.AddFramePoint2X(15, 18, 16);
                
                left.AddFramePoint2X(18, 17, 22);

                right.AddFramePoint2X(10, 18, 8);
                right.AddFramePoint2X(11, 22, 13);
                right.AddFramePoint2X(12, 24, 17);
                right.AddFramePoint2X(13, 22, 22);

                right.AddFramePoint2X(15, 21, 16);
                
                right.AddFramePoint2X(18, 20, 22);

                return [ left , right ];
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(14, 17, 21);
                anim.AddFramePoint2X(15, 17, 21);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(16, 14, true);
                anim.AddFramePoint2X(14, 16, 10 + 2);
                anim.AddFramePoint2X(15, 16, 10 + 2);
                anim.AddFramePoint2X(16, -1000, -1000);
                anim.AddFramePoint2X(18, 16, 14 + 2);
                anim.AddFramePoint2X(19, 22, 23 + 2);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(15, 8, 0);
                anim.AddFramePoint2X(15, 8, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(17, 16, -16);
                return anim;
            }
        }
        #endregion
    }
}