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
        #endregion
        #region Base Status
        public virtual int InitialMaxHealth { get { return 100; } }
        public virtual int HealthPerLifeCrystal{ get { return 20; } }
        public virtual int HealthPerLifeFruit { get { return 5; } }
        public virtual int InitialMaxMana { get { return 20; } }
        public virtual int ManaPerManaCrystal { get { return 20; } }
        public virtual void UpdateAttributes(Companion companion)
        {

        }
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
            _BackwardStandingFrames = SetBackwardStandingFrames;
            _BackwardsRevivingFrames = SetBackwardReviveFrames;
            AnimationsLoaded = true;
        }
        #endregion
        #region Animation Positions
        private AnimationPositionCollection[] _HandPositions;
        private AnimationPositionCollection _MountShoulderPosition, 
            _HeadVanityPosition, _WingPosition, _SittingPosition, 
            _SleepingOffset;

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
        #region Dialogues
        //Need to think how I'll do the dialogues...
        public virtual string GreetMessages(Companion companion)
        {
            return "*[name] liked to meet you.*";
        }
        public virtual string NormalMessages(Companion companion)
        {
            return "*[name] stares at you, waiting for you to say something.*";
        }
        public virtual string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "([name] join your adventure.)";
                case JoinMessageContext.Fail:
                    return "([name] refused.)";
                case JoinMessageContext.FullParty:
                    return "(There is no space for [name] in the group.)";
            }
            return "";
        }
        public virtual string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "([name] left your group.)";
                case LeaveMessageContext.Fail:
                    return "([name] refuses to leave your group.)";
                case LeaveMessageContext.AskIfSure:
                    return "([name] asks if you're sure you want them to leave your group.)";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "([name] stays on your group.)";
            }
            return "";
        }
        #endregion
        #region Other Hooks
        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }
        #endregion

        public Companion GetCompanionObject{
            get
            {
                switch(CompanionType)
                {
                    case CompanionTypes.TerraGuardian:
                        return new TerraGuardian();
                    default:
                        return new Companion();
                }
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
            switch(Gender)
            {
                default:
                    return Name;
                case Genders.Male:
                    return "[c/4079FF:" + Name + "]"; //004BFF
                case Genders.Female:
                    return "[c/FF4079:" + Name + "]"; //FF004B
            }
        }
    }
    
    public enum JoinMessageContext : byte
    {
        Success,
        Fail,
        FullParty
    }

    public enum LeaveMessageContext : byte
    {
        Success,
        Fail,
        AskIfSure,
        DangerousPlaceYesAnswer,
        DangerousPlaceNoAnswer
    }

    public enum AnimationPositions : byte
    {
        HandPosition,
        MountShoulderPositions,
        HeadVanityPosition,
        WingPositions,
        SittingPosition,
        SleepingOffset
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
}