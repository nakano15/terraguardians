using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions;

public class GreenBase : TerraGuardianBase
{
    public override string Name => "Green";
    public override string DisplayName => "Green";
    public override string FullName => "Jochen Green";
    public override string Description => "Treated many TerraGuardians in the Ether Realm,\nhis newest challenge is now in the Terra Realm.";
    public override Sizes Size => Sizes.Large;
    public override int Width => 24;
    public override int Height => 86;
    public override int SpriteWidth => 96;
    public override int SpriteHeight => 96;
    public override float Scale => 89f / 86;
    public override CombatTactics DefaultCombatTactic => CombatTactics.LongRange;
    public override int Age => 31;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 4);
    public override Genders Gender => Genders.Male;
    public override int InitialMaxHealth => 170; //895
    public override int HealthPerLifeCrystal => 15;
    public override int HealthPerLifeFruit => 25;
    public override float AccuracyPercent => .6f;
    public override float MaxFallSpeed => .36f;
    public override float MaxRunSpeed => 5.15f;
    public override float RunAcceleration => .21f;
    public override float RunDeceleration => .39f;
    public override bool CanCrouch => false;
    public override bool CanUseHeavyItem => false;
    protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks()
    {
        FollowerUnlock = 3,
        MountUnlock = 6
    };
    protected override CompanionDialogueContainer GetDialogueContainer => new Green.GreenDialogue();

    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        InitialInventoryItems = [
            new InitialItemDefinition(ItemID.FlintlockPistol),
            new InitialItemDefinition(ItemID.HealingPotion, 5),
            new InitialItemDefinition(ItemID.MeteorShot, 250)
        ];
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
                anim.AddFrame(i, 24f);
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
    protected override Animation SetSittingFrames => new Animation(15);
    protected override Animation SetChairSittingFrames => new Animation(14);
    protected override Animation SetThroneSittingFrames => new Animation(17);
    protected override Animation SetBedSleepingFrames => new Animation(16);
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
    protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer left = new AnimationFrameReplacer(), 
                right = new AnimationFrameReplacer();
            right.AddFrameToReplace(14, 0);
            right.AddFrameToReplace(15, 1);
            return [left, right];
        }
    }
    #endregion

    #region Animation Positions
    protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, false);
    protected override AnimationPositionCollection SetSittingPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();
            anim.AddFramePoint2X(14, 21, 40);
            anim.AddFramePoint2X(15, 21, 40);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(18, 15, true);
            anim.AddFramePoint2X(14, 17, 20);
            anim.AddFramePoint2X(17, 17, 15);
            anim.AddFramePoint2X(18, 29, 21);
            anim.AddFramePoint2X(21, 29, 21);
            return anim;
        }
    }
    protected override AnimationPositionCollection[] SetHandPositions
    {
        get
        {
            AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();
            left.AddFramePoint2X(10, 12, 3);
            left.AddFramePoint2X(11, 33, 11);
            left.AddFramePoint2X(12, 37, 21);
            left.AddFramePoint2X(13, 30, 29);
            
            left.AddFramePoint2X(15, 29, 31);
            
            left.AddFramePoint2X(18, 41, 41);
            
            right.AddFramePoint2X(10, 25, 3);
            right.AddFramePoint2X(11, 37, 11);
            right.AddFramePoint2X(12, 39, 21);
            right.AddFramePoint2X(13, 33, 29);
            
            right.AddFramePoint2X(18, 37, 41);
            return [left, right];
        }
    }
    protected override AnimationPositionCollection SetHeadVanityPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(23, 12, true);
            anim.AddFramePoint2X(14, 23, 17);
            anim.AddFramePoint2X(15, 23, 17);
            anim.AddFramePoint2X(18, 36, 30);
            anim.AddFramePoint2X(21, 36, 30);
            return anim;
        }
    }
    #endregion
}