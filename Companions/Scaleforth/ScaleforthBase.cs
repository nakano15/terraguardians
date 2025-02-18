using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions;

public class ScaleforthBase : TerraGuardianBase
{
    const string BackWingTextureID = "backwing";
    public override string Name => "Scaleforth";
    public override string Description => "";
    public override int Age => 56;
    public override Sizes Size => Sizes.ExtraLarge;
    public override Genders Gender => Genders.Male;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 28);
    public override int SpriteWidth => 160;
    public override int SpriteHeight => 192;
    public override int FramesInRow => 13;
    public override int Width => 66;
    public override int Height => 166;
    public override bool CanCrouch => false;
    public override float Scale => 185f / 166;
    public override bool CanUseHeavyItem => false;
    public override int InitialMaxHealth => 230; //1530
    public override int HealthPerLifeCrystal => 60;
    public override int HealthPerLifeFruit => 20;
    public override float MaxRunSpeed => 3.7f;
    public override float RunAcceleration =>  .12f
    public override float RunDeceleration => .15f;
    public override int JumpHeight => 20;
    public override float JumpSpeed => 6.85f;
    public override float AccuracyPercent => .68f;
    protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0 };
    protected override CompanionDialogueContainer GetDialogueContainer => new Scaleforth.ScaleforthDialogue();

    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        InitialInventoryItems = [
            new InitialItemDefinition(ItemID.GoldBroadsword),
            new InitialItemDefinition(ItemID.HealingPotion, 10)
        ];
    }

    public override void UpdateAttributes(Companion companion)
    {
        if (companion.wingTimeMax <= 0)
        {
            companion.wingTimeMax = 30;
        }
    }

    public override void SetupSpritesContainer(CompanionSpritesContainer container)
    {
        container.AddExtraTexture(BackWingTextureID, "wing_back");
    }

    public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
    {
        if (!IsDrawingFrontLayer)
        {
            // add wing draw script behind the right arm.
            for (int i = 0; i < DrawDatas.Count; i++)
            {
                //if (DrawDatas[i].texture == getsprite)
            }
        }
    }

    #region Animations
    protected override Animation SetStandingFrames => new Animation(0);
    protected override Animation SetWalkingFrames
    {
        get
        {
                Animation anim = new Animation();
                for(short i = 2; i <= 9; i++)
                    anim.AddFrame(i, 24);
                return anim;
        }
    }
    protected override Animation SetJumpingFrames
    {
        get{
            Animation anim = new Animation();
            anim.AddFrame(10, 7.5f);
            anim.AddFrame(11, 7.5f);
            anim.AddFrame(12, 7.5f);
            anim.AddFrame(11, 7.5f);
            return anim;
        }
    }
    protected override Animation SetItemUseFrames
    {
        get{
            Animation anim = new Animation();
            for(short i = 13; i <= 16; i++)
                anim.AddFrame(i, 1);
            return anim;
        }
    }
    protected override Animation SetSittingFrames => new Animation(19);
    protected override Animation SetChairSittingFrames => new Animation(18);
    protected override Animation SetDownedFrames => new Animation(17);
    protected override Animation SetThroneSittingFrames => new Animation(20);
    protected override Animation SetBedSleepingFrames => new Animation(21);
    protected override Animation SetRevivingFrames => new Animation(22);
    protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
    {
        get{
            AnimationFrameReplacer anim = new AnimationFrameReplacer();
            anim.AddFrameToReplace(17, 0);
            anim.AddFrameToReplace(18, 1);
            anim.AddFrameToReplace(19, 1);
            anim.AddFrameToReplace(23, 2);
            return anim;
        }
    }
    protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
    {
        get{
            AnimationFrameReplacer[] anim = [new AnimationFrameReplacer(), new AnimationFrameReplacer()];
            anim[1].AddFrameToReplace(1, 0);
            anim[1].AddFrameToReplace(18, 1);
            anim[1].AddFrameToReplace(20, 2);
            anim[1].AddFrameToReplace(21, 3);
            return anim;
        }
    }
    #endregion
    #region Animation Positions
    protected override AnimationPositionCollection[] SetHandPositions
    {
        get{
            AnimationPositionCollection[] hands = [new AnimationPositionCollection(), new AnimationPositionCollection()];
            hands[0].AddFramePoint2X(13, 20, 13);
            hands[0].AddFramePoint2X(14, 48, 22);
            hands[0].AddFramePoint2X(15, 60, 45);
            hands[0].AddFramePoint2X(16, 45, 59);
            
            hands[0].AddFramePoint2X(22, 38, 79);
            
            hands[1].AddFramePoint2X(13, 46, 13);
            hands[1].AddFramePoint2X(14, 68, 22);
            hands[1].AddFramePoint2X(15, 72, 45);
            hands[1].AddFramePoint2X(16, 69, 59);

            hands[1].AddFramePoint2X(22, 61, 79);
            return hands;
        }
    }
    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get{
            AnimationPositionCollection pos = new AnimationPositionCollection(27, 36, true);
            pos.AddFramePoint2X(10, 42, 39);
            pos.AddFramePoint2X(11, 42, 37);
            pos.AddFramePoint2X(12, 42, 36);
            
            pos.AddFramePoint2X(18, 28, 50);
            pos.AddFramePoint2X(19, 28, 50);
            pos.AddFramePoint2X(20, 29, 50);
            pos.AddFramePoint2X(22, 33, 51);
            return pos;
        }
    }
    protected override AnimationPositionCollection SetSittingPosition
    {
        get{
            AnimationPositionCollection pos = new AnimationPositionCollection(30, 87, true);
            pos.AddFramePoint2X(20, 0, 0);
            return pos;
        }
    }
    protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(0, 5, true);
    protected override AnimationPositionCollection SetPlayerSittingOffset
    {
        get{
            AnimationPositionCollection pos = new AnimationPositionCollection();

            return pos;
        }
    }
    protected override AnimationPositionCollection[] SetArmOffsetPosition
    {
        get{
            AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();
            left.AddFramePoint2X(10, 42 - 27, 43 - 39);
            left.AddFramePoint2X(11, 42 - 27, 41 - 39);
            left.AddFramePoint2X(12, 42 - 27, 40 - 39);
            
            right.AddFramePoint2X(10, 61 - 50, 43 - 39);
            right.AddFramePoint2X(11, 61 - 50, 41 - 39);
            right.AddFramePoint2X(12, 61 - 50, 40 - 39);
            return [left, right];
        }
    }
    #endregion
}