using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions;

public class MonicaBase : TerraGuardianBase
{
    public override string Name => "Monica";
    public override string FullName => "Monica Chubbycheeks";
    public override string Description => "";
    public override int Age => 16;
    public override Sizes Size => Sizes.Medium;
    public override int Width => 32;
    public override int Height => 72;
    public override int SpriteWidth => 76;
    public override int SpriteHeight => 76;
    public override float Scale => 72f / 80;
    public override int FavoriteFood => ItemID.Bacon;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Autumn, 17);
    public override Genders Gender => Genders.Female;
    public override int InitialMaxHealth => 180; //720
    public override int HealthPerLifeCrystal => 20;
    public override int HealthPerLifeFruit => 12;
    public override float AccuracyPercent => .43f;
    public override float MaxRunSpeed => 4.1f;
    public override float RunAcceleration => .13f;
    public override float RunDeceleration => .17f;
    public override int JumpHeight => 17;
    public override float JumpSpeed => 7.15f;
    public override bool CanCrouch => false;
    public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
    protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 4, MountUnlock = 7 };
    protected override CompanionDialogueContainer GetDialogueContainer => new MonicaDialogue();
    public override bool AllowSharingBedWithPlayer => false;

    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        InitialInventoryItems = [
            new InitialItemDefinition(ItemID.Katana),
            new InitialItemDefinition(ItemID.HealingPotion, 5)
        ];
    }
    #region Animations
    protected override Animation SetStandingFrames => new Animation(0);
    protected override Animation SetWalkingFrames
    {
        get
        {
            Animation anim = new Animation();
            for (short i = 2; i <= 9; i++)
            {
                anim.AddFrame(i, 24);
            }
            return anim;
        }
    }
    protected override Animation SetJumpingFrames => new Animation(10);
    protected override Animation SetPlayerMountedArmFrame => new Animation(10);
    protected override Animation SetItemUseFrames
    {
        get
        {
            Animation anim = new Animation();
            for (short i = 11; i <= 14; i++)
            {
                anim.AddFrame(i);
            }
            return anim;
        }
    }
    protected override Animation SetChairSittingFrames => new Animation(15);
    protected override Animation SetSittingFrames => new Animation(16);
    protected override Animation SetThroneSittingFrames => new Animation(17);
    protected override Animation SetBedSleepingFrames => new Animation(18);
    protected override Animation SetRevivingFrames => new Animation(19);
    protected override Animation SetDownedFrames => new Animation(20);
    protected override Animation SetBackwardStandingFrames => new Animation(21);
    protected override Animation SetBackwardReviveFrames => new Animation(22);
    protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer anim = new AnimationFrameReplacer();
            anim.AddFrameToReplace(15, 0);
            anim.AddFrameToReplace(16, 0);
            anim.AddFrameToReplace(17, 1);
            return anim;
        }
    }
    #endregion

    #region Animation Positions
    protected override AnimationPositionCollection SetSittingPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();
            for (short i = 15; i <= 16; i++)
            {
                anim.AddFramePoint2X(i, 18, 30);
            }
            return anim;
        }
    }
    protected override AnimationPositionCollection[] SetHandPositions
    {
        get
        {
            AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
            left.AddFramePoint2X(11, 13, 3);
            left.AddFramePoint2X(12, 27, 8);
            left.AddFramePoint2X(13, 30, 16);
            left.AddFramePoint2X(14, 23, 24);
            left.AddFramePoint2X(19, 20, 24);
            
            right.AddFramePoint2X(11, 24, 3);
            right.AddFramePoint2X(12, 31, 8);
            right.AddFramePoint2X(13, 34, 16);
            right.AddFramePoint2X(14, 30, 24);
            right.AddFramePoint2X(19, 30, 24);

            return [left, right];
        }
    }
    protected override AnimationPositionCollection SetHeadVanityPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(20, 9, true);
            anim.AddFramePoint2X(1, 21, 10);
            anim.AddFramePoint2X(19, 20, 13);
            anim.AddFramePoint2X(22, 20, 13);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(14, 13, true);

            return anim;
        }
    }
    protected override AnimationPositionCollection SetPlayerSittingOffset
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection();
            anim.AddFramePoint2X(15, 8, -4);
            anim.AddFramePoint2X(17, -10, -14);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetPlayerSleepingOffset
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(4, -8, true);
            return anim;
        }
    }
    #endregion
}