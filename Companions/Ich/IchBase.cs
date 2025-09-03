using terraguardians.Companions.Ich;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions;

public class IchBase : TerraGuardianBase
{
    public override string Name => "Ich";
    public override string Description => "He's himself. Simple as that.";
    public override string FullName => "Jake Meowzer";
    public override Sizes Size => Sizes.Large;
    public override int Width => 26;
    public override int Height => 72;
    public override int SpriteWidth => 96;
    public override int SpriteHeight => 96;
    public override int FramesInRow => 20;
    public override float Scale => 64f / 72;
    public override int FavoriteFood => ItemID.CookedFish;
    public override int Age => 15;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 6);
    public override Genders Gender => Genders.Male;
    public override int InitialMaxHealth => 120;
    public override int HealthPerLifeCrystal => 15;
    public override int HealthPerLifeFruit => 6;
    public override float MaxRunSpeed => 4.2f;
    public override float JumpSpeed => 5.15f;
    public override float RunDeceleration => .4f;
    public override bool CanCrouch => false;
    public override bool CanUseHeavyItem => false;
    protected override CompanionDialogueContainer GetDialogueContainer => new IchDialogue();

    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        base.InitialInventory(out InitialInventoryItems, ref InitialEquipments);
    }

    #region Animation
    protected override Animation SetStandingFrames => new Animation(0);
    protected override Animation SetWalkingFrames
    {
        get
        {
            Animation anim = new Animation();
            for (short i = 1; i <= 8; i++)
            {
                anim.AddFrame(i, 24);
            }
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
    protected override Animation SetSittingFrames => new Animation(14);
    protected override Animation SetThroneSittingFrames => new Animation(16);
    protected override Animation SetBedSleepingFrames => new Animation(15);
    protected override Animation SetRevivingFrames => new Animation(18);
    protected override Animation SetDownedFrames => new Animation(17);
    protected override Animation SetBackwardStandingFrames => new Animation(19);
    protected override Animation SetBackwardReviveFrames => new Animation(20);
    protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer anim = new AnimationFrameReplacer();
            anim.AddFrameToReplace(14, 0);
            anim.AddFrameToReplace(15, 1);
            return anim;
        }
    }
    protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer left = new AnimationFrameReplacer(),
                right = new AnimationFrameReplacer();
            right.AddFrameToReplace(14, 0);
            right.AddFrameToReplace(16, 1);
            return [left, right];
        }
    }
    #endregion
    #region Animation Position
    protected override AnimationPositionCollection[] SetHandPositions
    {
        get
        {
            AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();
            left.AddFramePoint2X(10, 15, 11);
            left.AddFramePoint2X(11, 33, 17);
            left.AddFramePoint2X(12, 35, 25);
            left.AddFramePoint2X(13, 30, 31);
            left.AddFramePoint2X(18, 27, 36);

            right.AddFramePoint2X(10, 22, 11);
            right.AddFramePoint2X(11, 35, 17);
            right.AddFramePoint2X(12, 37, 25);
            right.AddFramePoint2X(13, 32, 31);
            right.AddFramePoint2X(18, 31, 36);
            return [left, right];
        }
    }
    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(19, 21, true);
            anim.AddFramePoint2X(16, 17, 25);
            anim.AddFramePoint2X(18, 16, 27);
            anim.AddFramePoint2X(52, 16, 27);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetSittingPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();
            anim.AddFramePoint2X(14, 22, 39);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetPlayerSittingOffset
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();
            anim.AddFramePoint2X(14, 7, -6);
            anim.AddFramePoint2X(16, -12, -16);
            return anim;
        }
    }
    /*protected override AnimationPositionCollection SetPlayerSleepingOffset
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();

            return anim;
        }
    }*/
    #endregion
}