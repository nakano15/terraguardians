using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionBase
    {
        #region Companion Infos
        private bool InvalidCompanion = false;
        internal CompanionBase SetInvalid() { InvalidCompanion = true; return this; }
        public bool IsInvalidCompanion { get{ return InvalidCompanion; }}
        public virtual string Name { get { return ""; } }
        public virtual string Description { get { return ""; } }
        public virtual string CompanionContentFolderName { get { return Name; } }
        public virtual int Age { get { return 18; } }
        public virtual Genders Gender { get { return Genders.Male; } }
        public virtual CompanionTypes CompanionType { get { return CompanionTypes.TerraGuardian ;} }
        public virtual int Width { get { return 32; } }
        public virtual int Height { get { return 82; } }
        public virtual float Scale { get { return 1f; } }
        public virtual int CrouchingHeight { get { return 52; } }
        public virtual int SpriteWidth { get { return 96 ; } }
        public virtual int SpriteHeight { get { return 96 ; } }
        public virtual int FramesInRow { get { return 20; } }
        #endregion
        #region Base Status
        public virtual int InitialMaxHealth { get { return 100; } }
        public virtual int HealthPerLifeCrystal{ get { return 20; } }
        public virtual int HealthPerLifeFruit { get { return 5; } }
        public virtual int InitialMaxMana { get { return 20; } }
        public virtual int ManaPerManaCrystal { get { return 20; } }
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
        #region Animations
        private Animation _StandingFrame, _WalkingFrames, _JumpingFrames, 
        _HeavySwingFrames, _ItemUseFrames, _CrouchingFrames, _CrouchingSwingFrames,
        _SittingItemUseFrames, _SittingFrames, _ChairSittingFrames, _ThroneSittingFrames,
        _BedSleepingFrames, _RevivingFrames, _DownedFrames, _PetrifiedFrames, 
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
        protected virtual Animation PlayerMountedArmFrame { get { return new Animation(); } }
        protected virtual Animation SetBackwardStandingFrames { get { return new Animation(); } }
        protected virtual Animation SetBackwardReviveFrames { get { return new Animation(); } }
        private bool AnimationsLoaded = false;
        internal void InitializeAnimations()
        {
            if(AnimationsLoaded) return;
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
        #region AnimationPositions
        public virtual AnimationPositionCollection[] HandPositions { get { return new AnimationPositionCollection[]{
            new AnimationPositionCollection(),
            new AnimationPositionCollection()
        }; } }
        public virtual AnimationPositionCollection MountShoulderPosition { get { return new AnimationPositionCollection(); }}
        public virtual AnimationPositionCollection HeadVanityPosition { get { return new AnimationPositionCollection(); }}
        public virtual AnimationPositionCollection WingPosition { get { return new AnimationPositionCollection(); }}
        public virtual AnimationPositionCollection SittingPosition { get { return new AnimationPositionCollection(); }}
        public virtual AnimationPositionCollection SleepingOffset { get { return new AnimationPositionCollection(); }}
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
            BackwardStandingFrames, 
            BackwardsRevivingFrames
        }
    }
}