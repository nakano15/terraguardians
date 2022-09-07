using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionBase
    {
        public virtual string Name { get { return ""; } }
        public virtual string Description { get { return ""; } }
        public virtual int Age { get { return 18; } }
        public virtual Genders Gender { get { return Genders.Male; } }
        public virtual CompanionTypes CompanionType { get { return CompanionTypes.TerraGuardian ;} }
        public virtual int Width { get { return 32; } }
        public virtual int Height { get { return 82; } }
        public virtual int CrouchingHeight { get { return 52; } }
        public virtual int SpriteWidth { get { return 96 ; } }
        public virtual int SpriteHeight { get { return 96 ; } }
        public virtual int FramesInRow { get { return 20; } }
        #region Base Status
        public virtual int InitialMaxHealth { get { return 100; } }
        public virtual int HealthPerLifeCrystal{ get { return 20; } }
        public virtual int HealthPerLifeFruit { get { return 5; } }
        public virtual int InitialMaxMana { get { return 20; } }
        public virtual int ManaPerManaCrystal { get { return 20; } }
        #endregion
        #region Animations
        public virtual Animation StandingFrames { get { return new Animation(0); } }
        public virtual Animation WalkingFrames { get { return new Animation(0); } }
        public virtual Animation JumpingFrames { get { return new Animation(0); } }
        public virtual Animation HeavySwingFrames { get { return new Animation(0); } }
        public virtual Animation ItemUseFrames { get { return new Animation(0); } }
        public virtual Animation CrouchingFrames { get { return new Animation(0); } }
        public virtual Animation CrouchingSwingFrames { get { return new Animation(0); } }
        public virtual Animation SittingItemUseFrames { get { return new Animation(); } }
        public virtual Animation SittingFrames { get { return new Animation(); } }
        public virtual Animation ChairSittingFrames { get { return new Animation(); } }
        public virtual Animation ThroneSittingFrames { get { return new Animation(); } }
        public virtual Animation BedSleepingFrames { get { return new Animation(); } }
        public virtual Animation RevivingFrames { get { return new Animation(); } }
        public virtual Animation DownedFrames { get { return new Animation(); } }
        public virtual Animation PetrifiedFrames { get { return new Animation(); } }
        public short PlayerMountedArmFrame = -1;
        public virtual Animation BackwardStandingFrames { get { return new Animation(); } }
        public virtual Animation BackwardReviveFrames { get { return new Animation(); } }
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

        public Companion GetCompanionObject{
            get{
                switch(CompanionType)
                {
                    case CompanionTypes.TerraGuardian:
                        return new TerraGuardian();
                    default:
                        return new Companion();
                }
            }
        }
    }
}