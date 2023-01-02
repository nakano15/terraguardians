using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class CompanionBase
    {
        #region Companion Infos
        private bool InvalidCompanion = false, AnimationsLoaded = false, AnimationPositionsLoaded = false;
        internal CompanionBase SetInvalid() { InvalidCompanion = true; return this; }
        public bool IsInvalidCompanion { get{ return InvalidCompanion; }}
        public virtual string Name { get { return ""; } }
        public virtual string Description { get { return ""; } }
        public virtual string CompanionContentFolderName { get { return Name; } }
        public virtual int Age { get { return 18; } }
        public virtual Genders Gender { get { return Genders.Male; } }
        public virtual CompanionTypes CompanionType { get { return CompanionTypes.TerraGuardian ;} }
        public virtual MountStyles MountStyle { get { return MountStyles.PlayerMountsOnCompanion; } }
        public virtual bool CanCrouch { get { return false; } }
        public virtual int Width { get { return 32; } }
        public virtual int Height { get { return 82; } }
        public virtual float Scale { get { return 1f; } }
        public virtual int CrouchingHeight { get { return 52; } }
        public virtual int SpriteWidth { get { return 96 ; } }
        public virtual int SpriteHeight { get { return 96 ; } }
        public virtual int FramesInRow { get { return 20; } }
        public virtual bool CanUseHeavyItem { get { return false; } }
        public virtual Rectangle GetHeadDrawFrame(Texture2D HeadTexture)
        {
            return new Rectangle(0, 0, HeadTexture.Width, HeadTexture.Height);
        }
        public virtual SoundStyle HurtSound {get { return Terraria.ID.SoundID.NPCHit1; }}
        public virtual SoundStyle DeathSound{ get{ return Terraria.ID.SoundID.NPCDeath1; }}
        public virtual void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[] { new InitialItemDefinition(ItemID.WoodenSword), new InitialItemDefinition(ItemID.LesserHealingPotion, 5) };
        }
        public virtual bool IsNocturnal { get { return false; } }
        public virtual bool SleepsWhenOnBed { get { return true; } }
        protected virtual FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks();
        private FriendshipLevelUnlocks? _unlocks = null;
        public FriendshipLevelUnlocks GetFriendshipUnlocks
        {
            get
            {
                if (!_unlocks.HasValue)
                    _unlocks = SetFriendshipUnlocks;
                return _unlocks.Value;
            }
        }
        private TerrarianCompanionInfo terrariancompanioninfo = null;
        public TerrarianCompanionInfo GetTerrarianCompanionInfo
        {
            get
            {
                if(terrariancompanioninfo == null)
                    terrariancompanioninfo = SetTerrarianCompanionInfo;
                return terrariancompanioninfo;
            }
        }
        public virtual CombatTactics DefaultCombatTactic { get { return CombatTactics.MidRange; } }
        public virtual CompanionGroup GetCompanionGroup { get { return MainMod.GetTerrariansGroup; } }
        protected virtual TerrarianCompanionInfo SetTerrarianCompanionInfo { get { return new TerrarianCompanionInfo(); } }
        #endregion
        #region Base Status
        public virtual int InitialMaxHealth { get { return 100; } }
        public virtual int HealthPerLifeCrystal{ get { return 20; } }
        public virtual int HealthPerLifeFruit { get { return 5; } }
        public virtual int InitialMaxMana { get { return 20; } }
        public virtual int ManaPerManaCrystal { get { return 20; } }
        public float HealthScale { get { return (float)(InitialMaxHealth + HealthPerLifeCrystal * 15 + HealthPerLifeFruit * 20) / 500;}}
        public float ManaScale { get { return (float)(InitialMaxMana + ManaPerManaCrystal * 9) / 200; } }
        public virtual void UpdateAttributes(Companion companion)
        {

        }
        public virtual float AccuracyPercent { get { return .5f; } }
        public virtual float AgilityPercent { get { return .5f; } }
        public virtual byte TriggerPercent { get { return 50; } }
        #endregion
        #region Default Permissions
        public virtual bool AllowSharingChairWithPlayer { get { return true; } }
        public virtual bool AllowSharingBedWithPlayer { get { return true; } }
        #endregion
        #region Mobility Status
        public virtual float MaxFallSpeed { get { return 10f; }}
        public virtual float Gravity { get { return 0.4f; }}
        public virtual int JumpHeight { get{ return 15;}}
        public virtual float JumpSpeed { get{ return 5.01f; }}
        public virtual float MaxRunSpeed { get{ return 3f; } }
        public virtual float RunAcceleration { get { return 0.08f; }}
        public virtual float RunDeceleration { get{ return 0.2f; }}
        public virtual int FallHeightTolerance { get { return 25; }}
        #endregion
        #region Behavior Scripts
        public virtual BehaviorBase DefaultIdleBehavior { get { return new IdleBehavior(); } }
        public virtual BehaviorBase DefaultCombatBehavior { get { return new CombatBehavior(); } }
        public virtual BehaviorBase DefaultFollowLeaderBehavior { get { return new FollowLeaderBehavior(); } }
        public virtual BehaviorBase PreRecruitmentBehavior { get { return null; } }
        #endregion
        #region Animations
        private Animation _StandingFrame, _WalkingFrames, _JumpingFrames, 
        _HeavySwingFrames, _ItemUseFrames, _CrouchingFrames, _CrouchingSwingFrames,
        _SittingItemUseFrames, _SittingFrames, _ChairSittingFrames, _ThroneSittingFrames,
        _BedSleepingFrames, _RevivingFrames, _DownedFrames, _PetrifiedFrames, _PlayerMountedArmFrame,
        _BackwardStandingFrames, _BackwardsRevivingFrames;
        private AnimationFrameReplacer _BodyFrontFrameReplacers;
        public Animation GetAnimation(AnimationTypes anim)
        {
            if (!AnimationsLoaded) InitializeAnimations();
            switch(anim)
            {
                case AnimationTypes.StandingFrame:
                    return _StandingFrame;
                case AnimationTypes.WalkingFrames:
                    return _WalkingFrames;
                case AnimationTypes.JumpingFrames:
                    return _JumpingFrames;
                case AnimationTypes.HeavySwingFrames:
                    return _HeavySwingFrames;
                case AnimationTypes.ItemUseFrames:
                    return _ItemUseFrames;
                case AnimationTypes.CrouchingFrames:
                    return _CrouchingFrames;
                case AnimationTypes.CrouchingSwingFrames:
                    return _CrouchingSwingFrames;
                case AnimationTypes.SittingItemUseFrames:
                    return _SittingItemUseFrames;
                case AnimationTypes.SittingFrames:
                    return _SittingFrames;
                case AnimationTypes.ChairSittingFrames:
                    return _ChairSittingFrames;
                case AnimationTypes.ThroneSittingFrames:
                    return _ThroneSittingFrames;
                case AnimationTypes.BedSleepingFrames:
                    return _BedSleepingFrames;
                case AnimationTypes.RevivingFrames:
                    return _RevivingFrames;
                case AnimationTypes.DownedFrames:
                    return _DownedFrames;
                case AnimationTypes.PetrifiedFrames:
                    return _PetrifiedFrames;
                case AnimationTypes.PlayerMountedArmFrame:
                    return _PlayerMountedArmFrame;
                case AnimationTypes.BackwardStandingFrames:
                    return _BackwardStandingFrames;
                case AnimationTypes.BackwardsRevivingFrames:
                    return _BackwardsRevivingFrames;
            }
            return null;
        }
        public AnimationFrameReplacer GetAnimationFrameReplacer(AnimationFrameReplacerTypes anim)
        {
            if (!AnimationsLoaded) InitializeAnimations();
            switch(anim)
            {
                case AnimationFrameReplacerTypes.BodyFront:
                    return _BodyFrontFrameReplacers;
            }
            return null;
        }
        protected virtual Animation SetStandingFrames { get { return new Animation(0); } }
        protected virtual Animation SetWalkingFrames { get { return new Animation(0); } }
        protected virtual Animation SetJumpingFrames { get { return new Animation(0); } }
        protected virtual Animation SetHeavySwingFrames { get { return new Animation(0); } }
        protected virtual Animation SetItemUseFrames { get { return new Animation(0); } }
        protected virtual Animation SetCrouchingFrames { get { return new Animation(0); } }
        protected virtual Animation SetCrouchingSwingFrames { get { return new Animation(0); } }
        protected virtual Animation SetSittingItemUseFrames { get { return new Animation(); } }
        protected virtual Animation SetSittingFrames { get { return new Animation(); } }
        protected virtual Animation SetChairSittingFrames { get { return new Animation(); } }
        protected virtual Animation SetThroneSittingFrames { get { return new Animation(); } }
        protected virtual Animation SetBedSleepingFrames { get { return new Animation(); } }
        protected virtual Animation SetRevivingFrames { get { return new Animation(); } }
        protected virtual Animation SetDownedFrames { get { return new Animation(); } }
        protected virtual Animation SetPetrifiedFrames { get { return new Animation(); } }
        protected virtual Animation SetPlayerMountedArmFrame { get { return new Animation(); } }
        protected virtual Animation SetBackwardStandingFrames { get { return new Animation(); } }
        protected virtual Animation SetBackwardReviveFrames { get { return new Animation(); } }
        protected virtual AnimationFrameReplacer SetBodyFrontFrameReplacers { get { return new AnimationFrameReplacer(); } }
        internal void InitializeAnimations()
        {
            _StandingFrame = SetStandingFrames;
            _WalkingFrames = SetWalkingFrames;
            _JumpingFrames = SetJumpingFrames; 
            _HeavySwingFrames = SetHeavySwingFrames;
            _ItemUseFrames = SetItemUseFrames;
            _CrouchingFrames = SetCrouchingFrames;
            _CrouchingSwingFrames = SetCrouchingSwingFrames;
            _SittingItemUseFrames = SetItemUseFrames;
            _SittingFrames = SetSittingFrames;
            _ChairSittingFrames = SetChairSittingFrames;
            _ThroneSittingFrames = SetThroneSittingFrames;
            _BedSleepingFrames = SetBedSleepingFrames;
            _RevivingFrames = SetRevivingFrames;
            _DownedFrames = SetDownedFrames;
            _PetrifiedFrames = SetPetrifiedFrames;
            _PlayerMountedArmFrame = SetPlayerMountedArmFrame;
            _BackwardStandingFrames = SetBackwardStandingFrames;
            _BackwardsRevivingFrames = SetBackwardReviveFrames;
            //
            _BodyFrontFrameReplacers = SetBodyFrontFrameReplacers;
            //
            AnimationsLoaded = true;
        }
        #endregion
        #region Animation Positions
        private AnimationPositionCollection[] _HandPositions;
        private AnimationPositionCollection _MountShoulderPosition, 
            _HeadVanityPosition, _WingPosition, _SittingPosition, 
            _SleepingOffset, _PlayerSittingOffset, _PlayerSleepingOffset;

        public int GetHands
        {
            get
            { 
                if(!AnimationPositionsLoaded)
                {
                    InitializeAnimationPositions();
                }
                return _HandPositions.Length;
            }
        }

        internal void InitializeAnimationPositions()
        {
            _HandPositions = SetHandPositions;
            if(_HandPositions.Length == 0)
            {
                _HandPositions = new AnimationPositionCollection[]{ new AnimationPositionCollection() };
            }
            _MountShoulderPosition = SetMountShoulderPosition;
            _HeadVanityPosition = SetHeadVanityPosition;
            _WingPosition = SetWingPosition;
            _SittingPosition = SetSittingPosition;
            _SleepingOffset = SetSleepingOffset;
            _PlayerSittingOffset = SetPlayerSittingOffset;
            _PlayerSleepingOffset = SetPlayerSleepingOffset;
            AnimationPositionsLoaded = true;
        }
        protected virtual AnimationPositionCollection[] SetHandPositions { get { return new AnimationPositionCollection[]{
            new AnimationPositionCollection(),
            new AnimationPositionCollection()
        }; } }
        protected virtual AnimationPositionCollection SetMountShoulderPosition { get { return new AnimationPositionCollection(); }}
        protected virtual AnimationPositionCollection SetHeadVanityPosition { get { return new AnimationPositionCollection(); }}
        protected virtual AnimationPositionCollection SetWingPosition { get { return new AnimationPositionCollection(); }}
        protected virtual AnimationPositionCollection SetSittingPosition { get { return new AnimationPositionCollection(); }}
        protected virtual AnimationPositionCollection SetSleepingOffset { get { return new AnimationPositionCollection(); }}
        protected virtual AnimationPositionCollection SetPlayerSittingOffset { get { return new AnimationPositionCollection(); } }
        protected virtual AnimationPositionCollection SetPlayerSleepingOffset { get { return new AnimationPositionCollection(); } }
        public AnimationPositionCollection GetAnimationPosition(AnimationPositions Position, byte MultipleAnimationsIndex = 0)
        {
            if(!AnimationPositionsLoaded)
            {
                InitializeAnimationPositions();
            }
            switch(Position)
            {
                case AnimationPositions.HandPosition:
                    if(MultipleAnimationsIndex < _HandPositions.Length)
                        return _HandPositions[MultipleAnimationsIndex];
                    return _HandPositions[0];
                case AnimationPositions.MountShoulderPositions:
                    return _MountShoulderPosition;
                case AnimationPositions.HeadVanityPosition:
                    return _HeadVanityPosition;
                case AnimationPositions.SittingPosition:
                    return _SittingPosition;
                case AnimationPositions.SleepingOffset:
                    return _SleepingOffset;
                case AnimationPositions.WingPositions:
                    return _WingPosition;
                case AnimationPositions.PlayerSittingOffset:
                    return _PlayerSittingOffset;
                case AnimationPositions.PlayerSleepingOffset:
                    return _PlayerSleepingOffset;
            }
            return null;
        }
        #endregion
        #region Spritesheet Loading Trick
        public CompanionSpritesContainer GetSpriteContainer { get{
            if (_spritecontainer == null)
            {
                _spritecontainer = new CompanionSpritesContainer(this, ReferedMod);
                _spritecontainer.LoadContent();
            }
            return _spritecontainer;
        } }
        private CompanionSpritesContainer _spritecontainer;
        private Mod ReferedMod;
        internal void DefineMod(Mod mod)
        {
            ReferedMod = mod;
        }
        #endregion
        #region Dialogue Container
        public CompanionDialogueContainer GetDialogues
        {
            get
            {
                LoadDialogue();
                return _dialogueContainer;
            }
        }
        private CompanionDialogueContainer _dialogueContainer = null;
        protected virtual CompanionDialogueContainer GetDialogueContainer { get { return new CompanionDialogueContainer(); } }
        private void LoadDialogue()
        {
            if (_dialogueContainer == null)
            {
                _dialogueContainer = GetDialogueContainer;
                _dialogueContainer.SetOwnerCompanion(this);
            }
        }
        #endregion
        #region Other Hooks
        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        public virtual bool CompanionRoomRequirements(bool IsRoomEvil, out string RequirementFailMessage)
        {
            return WorldMod.Housing_CheckBasicHousingRoomNeeds(IsRoomEvil, out RequirementFailMessage);
        }
        #endregion

        public virtual Companion GetCompanionObject{
            get
            {
                return new Companion();
            }
        }

        internal void Unload()
        {
            if(_spritecontainer != null)
            {
                _spritecontainer.Unload();
                _spritecontainer = null;
            }
            ReferedMod = null;
        }

        public string GetNameColored(CompanionData data = null)
        {
            string Name = data != null ? data.GetName : this.Name;
            MainMod.SetGenderColoring(Gender, ref Name);
            return Name;
        }
    }

    public enum AnimationPositions : byte
    {
        HandPosition,
        MountShoulderPositions,
        HeadVanityPosition,
        WingPositions,
        SittingPosition,
        SleepingOffset,
        PlayerSittingOffset,
        PlayerSleepingOffset
    }

    public enum AnimationTypes : byte
    {
        StandingFrame, 
        WalkingFrames, 
        JumpingFrames, 
        HeavySwingFrames, 
        ItemUseFrames, 
        CrouchingFrames, 
        CrouchingSwingFrames,
        SittingItemUseFrames, 
        SittingFrames, 
        ChairSittingFrames, 
        ThroneSittingFrames,
        BedSleepingFrames, 
        RevivingFrames, 
        DownedFrames, 
        PetrifiedFrames, 
        PlayerMountedArmFrame,
        BackwardStandingFrames, 
        BackwardsRevivingFrames
    }

    public enum AnimationFrameReplacerTypes : byte
    {
        BodyFront
    }

    public enum MountStyles : byte
    {
        CantMount = 0,
        PlayerMountsOnCompanion = 1,
        CompanionRidesPlayer = 2
    }

    public enum MountCompanionContext : byte
    {
        Success = 0,
        SuccessMountedOnPlayer = 1,
        Fail = 2,
        NotFriendsEnough = 3
    }

    public enum DismountCompanionContext : byte
    {
        SuccessMount = 0,
        SuccessMountOnPlayer = 1,
        Fail = 2
    }

    public struct InitialItemDefinition
    {
        public int ID;
        public int Stack;

        public InitialItemDefinition(int ItemID, int ItemStack = 1)
        {
            ID = ItemID;
            Stack = ItemStack;
        }
    }

    public struct FriendshipLevelUnlocks
    {
        public byte FollowerUnlock;
        public byte VisitUnlock;
        public byte MountUnlock;
        public byte MoveInUnlock;

        public FriendshipLevelUnlocks()
        {
            FollowerUnlock = 0;
            VisitUnlock = 1;
            MoveInUnlock = 3;
            MountUnlock = 5;
        }
    }
}