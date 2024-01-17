using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using terraguardians.Companions.Alexander;

namespace terraguardians.Companions
{
    public class AlexanderBase : TerraGuardianBase
    {
        public const int SleuthStaringAnimationID = 27, SleuthAnimationID = 28, SleuthAnimationID2 = 29, SleuthBackAnimationID = 30;

        public override string Name => "Alexander";
        public override string Description => "Member of a mystery solving gang,\nuntil they disappeared, and now he looks for them.\nDoesn't miss a clue.";
        public override Sizes Size => Sizes.Large;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int Width => 28;
        public override int Height => 86;
        public override int CrouchingHeight => 62;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override float Scale => 94f / 86;
        public override int FramesInRow => 20;
        public override bool CanUseHeavyItem => true;
        public override int Age => 19;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 4);
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 375; //1200
        public override int HealthPerLifeCrystal => 35;
        public override int HealthPerLifeFruit => 15;
        public override float AccuracyPercent => .72f;
        public override float MaxFallSpeed => .45f;
        public override float MaxRunSpeed => 5.1f;
        public override float RunAcceleration => .19f;
        public override float RunDeceleration => .39f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.16f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override CompanionDialogueContainer GetDialogueContainer => new AlexanderDialogue();
        public override CompanionData CreateCompanionData => new AlexanderData();
        public override Companion GetCompanionObject => new AlexanderCompanion();
        static Dictionary<CompanionID, Action<Companion>> AlexanderStatusBoosts = new Dictionary<CompanionID, Action<Companion>>();
        public override BehaviorBase PreRecruitmentBehavior => new AlexanderPreRecruitBehavior();
        public override bool CanSpawnNpc()
        {
            return NPC.downedBoss3;
        }

        public AlexanderBase()
        {
            AlexanderDefaultStatusBoosts.SetDefaultBonuses();
        }

        public override void OnUnload()
        {
            AlexanderStatusBoosts.Clear();
            AlexanderStatusBoosts = null;
        }

        public static void AddStatusBoost(CompanionID id, Action<Companion> buff)
        {
            if (!AlexanderStatusBoosts.ContainsKey(id))
            {
                AlexanderStatusBoosts.Add(id, buff);
            }
        }

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]{ new InitialItemDefinition(ItemID.Muramasa), new InitialItemDefinition(ItemID.HealingPotion, 5) };
        }

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i <= 13; i++)
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 14; i <= 16; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(23);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 24; i <= 26; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(17);
        protected override Animation SetChairSittingFrames => new Animation(18);
        protected override Animation SetThroneSittingFrames => new Animation(20);
        protected override Animation SetBedSleepingFrames => new Animation(19);
        protected override Animation SetDownedFrames => new Animation(21);
        protected override Animation SetRevivingFrames => new Animation(22);
        protected override Animation SetBackwardStandingFrames => new Animation(31);
        protected override Animation SetBackwardReviveFrames => new Animation(32);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(17, 0);
                anim.AddFrameToReplace(18, 0);
                anim.AddFrameToReplace(27, 1);
                anim.AddFrameToReplace(28, 1);
                anim.AddFrameToReplace(29, 1);
                anim.AddFrameToReplace(30, 2);
                return anim;
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, true);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                for (short i = 17; i <= 18; i++)
                    anim.AddFramePoint2X(i, 22, 35);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(17, 14, true);
                anim.AddFramePoint2X(15, 23, 19);
                anim.AddFramePoint2X(16, 29, 26);

                anim.AddFramePoint2X(17, 25, 13);
                anim.AddFramePoint2X(18, 25, 13);

                anim.AddFramePoint2X(20, 16, 21);
                
                for (short i = 22; i <= 26; i++)
                    anim.AddFramePoint2X(i, 24, 24);
                return anim;
            }
        }

        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 14, 2);
                left.AddFramePoint2X(11, 34, 9);
                left.AddFramePoint2X(12, 38, 21);
                left.AddFramePoint2X(13, 33, 29);

                left.AddFramePoint2X(14, 34, 2);
                left.AddFramePoint2X(15, 41, 25);
                left.AddFramePoint2X(16, 38, 41);

                left.AddFramePoint2X(22, 39, 37);

                left.AddFramePoint2X(24, 39, 18);
                left.AddFramePoint2X(25, 43, 30);
                left.AddFramePoint2X(26, 35, 40);

                right.AddFramePoint2X(10, 23, 2);
                right.AddFramePoint2X(11, 38, 9);
                right.AddFramePoint2X(12, 41, 21);
                right.AddFramePoint2X(13, 36, 29);

                right.AddFramePoint2X(14, 36, 2);
                right.AddFramePoint2X(15, 43, 25);
                right.AddFramePoint2X(16, 40, 41);

                right.AddFramePoint2X(22, 44, 37);

                right.AddFramePoint2X(24, 41, 18);
                right.AddFramePoint2X(25, 45, 30);
                right.AddFramePoint2X(26, 37, 40);
                return new AnimationPositionCollection[] { left, right};
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(24, 10, true);
                anim.AddFramePoint2X(15, 30, 17);
                anim.AddFramePoint2X(16, 37, 27);

                for (short i = 17; i <= 18; i++)
                    anim.AddFramePoint2X(i, 22, 10);
                for (short i = 22; i <= 26; i++)
                    anim.AddFramePoint2X(i, 32, 23);
                for (short i = 27; i <= 30; i++)
                    anim.AddFramePoint2X(i, -1000, -1000);
                return anim;
            }
        }

        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(18, 4, -6);
                
                anim.AddFramePoint2X(20, -12, -15);
                return anim;
            }
        }
        #endregion

        public class AlexanderCompanion : TerraGuardian
        {
            List<Action<Companion>> CurrentStatusBoosts = new List<Action<Companion>>();
            int SleuthDelay = 150;

            public bool HasAlexanderSleuthedGuardian(Companion companion)
            {
                return (Data as AlexanderData).HasCompanionIdentified(companion);
            }
            
            public bool HasAlexanderSleuthedGuardian(uint ID, string ModID = "")
            {
                return (Data as AlexanderData).HasCompanionIdentified(ID, ModID);
            }

            public void AddIdentifiedCompanion(CompanionID ID)
            {
                if ((Data as AlexanderData).AddIdentifiedCompanion(ID))
                {
                    UpdateSleuthdBuffsList();
                }
            }

            public void UpdateSleuthdBuffsList()
            {
                CurrentStatusBoosts.Clear();
                AlexanderData data = Data as AlexanderData;
                foreach (CompanionID id in AlexanderStatusBoosts.Keys)
                {
                    if (data.HasCompanionIdentified(id.ID, id.ModID))
                    {
                        CurrentStatusBoosts.Add(AlexanderStatusBoosts[id]);
                    }
                }
            }

            public void SleuthSomeone(Companion target)
            {
                RunBehavior(new AlexanderSleuthBehavior(target));
            }

            protected override void PreInitialize()
            {
                UpdateSleuthdBuffsList();
            }

            public override void UpdateAttributes()
            {
                foreach (Action<Companion> Buffs in AlexanderStatusBoosts.Values)
                {
                    Buffs(this);
                }
            }

            public override void UpdateBehaviorHook()
            {
                if(velocity.X != 0 || velocity.Y != 0)
                {
                    SleuthDelay = 150;
                }
                else
                {
                    if(SleuthDelay > 0)
                    {
                        SleuthDelay--;
                    }
                    else
                    {
                        if (!IsRunningBehavior)
                        {
                            foreach (Companion c in MainMod.ActiveCompanions.Values)
                            {
                                if (c != this && c.GetGroup.IsTerraGuardian && (c.IsSleeping || c.KnockoutStates > KnockoutStates.Awake) && !HasAlexanderSleuthedGuardian(c) && (c.Center - Center).Length() < (c.SpriteWidth + SpriteWidth) * .5f + 150)
                                {
                                    SleuthSomeone(c);
                                    break;
                                }
                            }
                        }
                        SleuthDelay = 120 + Main.rand.Next(5) * 30;
                    }
                }
            }
        }

        public class AlexanderData : CompanionData
        {
            public List<CompanionID> IdenfitiedCompanions = new List<CompanionID>();
            protected override uint CustomSaveVersion => 1;

            public bool HasCompanionIdentified(Companion companion)
            {
                if (!companion.GetGroup.IsTerraGuardian) return false;
                return HasCompanionIdentified(companion.ID, companion.ModID);
            }

            public bool HasCompanionIdentified(uint ID, string ModID = "")
            {
                foreach (CompanionID id in IdenfitiedCompanions)
                {
                    if (id.IsSameID(ID, ModID)) return true;
                }
                return false;
            }

            public bool AddIdentifiedCompanion(CompanionID id)
            {
                if (id.ID != CompanionDB.Alexander || id.ModID != MainMod.GetModName)
                {
                    IdenfitiedCompanions.Add(id);
                    return true;
                }
                return false;
            }

            public override void CustomSave(TagCompound save, uint UniqueID)
            {
                save.Add("AlexanderSleuthCount_" + UniqueID, IdenfitiedCompanions.Count);
                for (int i = 0; i < IdenfitiedCompanions.Count; i++)
                {
                    save.Add("AlexanderSleuthCount_ID"+i+"_" + UniqueID, IdenfitiedCompanions[i].ID);
                    save.Add("AlexanderSleuthCount_ModID"+i+"_" + UniqueID, IdenfitiedCompanions[i].ModID);
                }
            }

            public override void CustomLoad(TagCompound tag, uint UniqueID, uint LastVersion)
            {
                if (LastVersion >= 1)
                {
                    int Count = tag.GetInt("AlexanderSleuthCount_" + UniqueID);
                    IdenfitiedCompanions.Clear();
                    for (int i = 0; i < Count; i++)
                    {
                        uint ID = tag.Get<uint>("AlexanderSleuthCount_ID"+i+"_" + UniqueID);
                        string ModID = tag.GetString("AlexanderSleuthCount_ModID"+i+"_" + UniqueID);
                        IdenfitiedCompanions.Add(new CompanionID(ID, ModID));
                    }
                }
            }
        }
    }
}