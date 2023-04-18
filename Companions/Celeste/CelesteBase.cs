using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class CelesteBase : TerraGuardianBase
    {
        public override string Name => "Celeste";
        public override string Description => "A young priestess from the Ether Realm,\nwho spreads "+MainMod.TgGodName+"'s blessings through the land.";
        public override int Age => 19;
        public override int SpriteWidth => 112;
        public override int SpriteHeight => 108;
        public override int FramesInRow => 18;
        public override int Width => 38;
        public override int Height => 100;
        public override int CrouchingHeight => 82;
        public override float Scale => 113f / 100;
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Female;
        public override bool CanCrouch => true;
        public override int InitialMaxHealth => 160; //935
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 5;
        public override int InitialMaxMana => 90; //225
        public override int ManaPerManaCrystal => 15;
        public override float MaxRunSpeed => 4.95f;
        public override float RunAcceleration => 0.16f;
        public override float RunDeceleration => 0.4f;
        public override int JumpHeight => 18;
        public override float JumpSpeed => 7.35f;
        public override float AccuracyPercent => .58f;
        public override bool CanSpawnNpc()
        {
            return WorldMod.GetTerraGuardiansCount >= 3;
        }
        public override CombatTactics DefaultCombatTactic => CombatTactics.LongRange;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 5, MoveInUnlock = 3, MountUnlock = 6, RequestUnlock = 2 };
        protected override CompanionDialogueContainer GetDialogueContainer => new CelesteDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new Celeste.CelesteRecruitmentBehavior();
        public override void UpdateAttributes(Companion companion)
        {
            companion.manaRegen++;
            companion.lifeRegen++;
            companion.GetDamage<MagicDamageClass>() += 0.03f;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 1; i <= 8; i++)
                    a.AddFrame(i, 24);
                return a;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 12; i <= 15; i++)
                    a.AddFrame(i);
                return a;
            }
        }
        protected override Animation SetSittingFrames => new Animation(16);
        protected override Animation SetChairSittingFrames => new Animation(16);
        protected override Animation SetThroneSittingFrames => new Animation(17);
        protected override Animation SetBedSleepingFrames => new Animation(18);
        protected override Animation SetCrouchingFrames => new Animation(19);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 20; i <= 22; i++)
                    a.AddFrame(i);
                return a;
            }
        }
        protected override Animation SetRevivingFrames => new Animation(23);
        protected override Animation SetDownedFrames => new Animation(24);
        protected override Animation SetBackwardStandingFrames => new Animation(25);
        protected override Animation SetBackwardReviveFrames => new Animation(26);
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer right = new AnimationFrameReplacer(), left = new AnimationFrameReplacer();
                right.AddFrameToReplace(10, 0);
                right.AddFrameToReplace(11, 1);
                right.AddFrameToReplace(16, 2);
                right.AddFrameToReplace(17, 3);
                right.AddFrameToReplace(18, 4);
                right.AddFrameToReplace(19, 5);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer a = new AnimationFrameReplacer();
                a.AddFrameToReplace(16, 0);
                return a;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 24);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(28, 40, true);
                a.AddFramePoint2X(17, -1, 0);
                return a;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection();
                a.AddFramePoint2X(16, 2, -6);
                a.AddFramePoint2X(17, -7, -15);
                return a;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(22, 17), true);
                a.AddFramePoint2X(11, 23, 26);
                a.AddFramePoint2X(19, 22, 26);
                return a;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();

                left.AddFramePoint2X(12, 16, 4);
                left.AddFramePoint2X(13, 35, 12);
                left.AddFramePoint2X(14, 38, 23);
                left.AddFramePoint2X(15, 35, 34);

                left.AddFramePoint2X(20, 15, 15);
                left.AddFramePoint2X(21, 35, 23);
                left.AddFramePoint2X(22, 36, 37);

                right.AddFramePoint2X(12, 30, 4);
                right.AddFramePoint2X(13, 39, 12);
                right.AddFramePoint2X(14, 41, 23);
                right.AddFramePoint2X(15, 38, 33);
                
                right.AddFramePoint2X(20, 28, 15);
                right.AddFramePoint2X(21, 37, 23);
                right.AddFramePoint2X(22, 39, 37);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(new Vector2(8, -4), true);
        #endregion

        public override void UpdateBehavior(Companion companion)
        {
            if (!companion.IsRunningBehavior && Main.rand.NextBool(6))
            {
                bool AnyBoss = false;
                if (companion.Owner == null)
                {
                    for(int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && (Main.npc[n].boss || Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[Main.npc[n].type]))
                        {
                            AnyBoss = true;
                            break;
                        }
                    }
                }
                if (AnyBoss)
                {
                    companion.RunBehavior(new Celeste.CelesteBossFightPrayerBehavior());
                }
                else if ((companion.Owner == null || (companion.townNPCs > 0 && !Main.eclipse && Main.invasionType == InvasionID.None)) && Main.time >= 5.5f * 60 && !companion.HasBuff(ModContent.BuffType<Buffs.TgGodClawBlessing>()))
                {
                    companion.RunBehavior(new Celeste.CelestePrayerBehavior());
                }
            }
        }
    }
}