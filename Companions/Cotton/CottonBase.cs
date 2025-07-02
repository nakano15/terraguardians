using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace terraguardians.Companions;

public class CottonBase : TerraGuardianBase
{
    public override string Name => "Cotton"; //Need to port to TerraGuardians mod.
    public override string[] PossibleNames => new[] { "Patchy", "Cotton", "Stitches", "Waffles" };
    public override string FullName => "Pelucio Friendgarden";
    public override string Description => "Everybody mistakes him for a oversized plushie doll.\nIn reality, he want to be everyone's friend.";
    public override int Age => 19;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Winter, 17);
    public override Sizes Size => Sizes.Large;
    public override int SpriteWidth => 100;
    public override int SpriteHeight => 96;
    public override int FramesInRow => 20;
    public override int Width => 46;
    public override int Height => 90;
    public override float Scale => 98f / 90;
    public override int FavoriteFood => Terraria.ID.ItemID.Bacon;
    public override bool CanUseHeavyItem => true;
    public override int InitialMaxHealth => 280; //1330
    public override int HealthPerLifeCrystal => 50;
    public override int HealthPerLifeFruit => 15;
    public override float MaxRunSpeed => 4.9f;
    public override float RunAcceleration => .12f;
    public override float RunDeceleration => .25f;
    public override int JumpHeight => 12;
    public override float JumpSpeed => 6.34f;
    public override float AccuracyPercent => .65f;
    public override PersonalityBase GetPersonality(Companion c)
    {
        return PersonalityDB.Friendly;
    }
    protected override CompanionDialogueContainer GetDialogueContainer => new CottonDialogue();
    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        InitialInventoryItems = new InitialItemDefinition[]
        {
            new InitialItemDefinition(Terraria.ID.ItemID.PalmWoodSword),
            new InitialItemDefinition(Terraria.ID.ItemID.Shuriken, 250),
            new InitialItemDefinition(Terraria.ID.ItemID.HealingPotion, 5)
        };
    }

    #region Animations
    protected override Animation SetStandingFrames => new Animation(0);
    protected override Animation SetIdleFrames => new Animation(25);
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
            {
                anim.AddFrame(i);
            }
            return anim;
        }
    }
    protected override Animation SetChairSittingFrames => new Animation(17);
    protected override Animation SetSittingFrames => new Animation(18);
    protected override Animation SetThroneSittingFrames => new Animation(20);
    protected override Animation SetBedSleepingFrames => new Animation(19);
    protected override Animation SetRevivingFrames => new Animation(21);
    protected override Animation SetDownedFrames => new Animation(22);
    protected override Animation SetBackwardStandingFrames => new Animation(23);
    protected override Animation SetBackwardReviveFrames => new Animation(24);

    protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer anim = new AnimationFrameReplacer();
            anim.AddFrameToReplace(17, 0);
            anim.AddFrameToReplace(18, 0);
            return anim;
        }
    }

    protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
    {
        get
        {
            AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
            left.AddFrameToReplace(20, 0);

            right.AddFrameToReplace(17, 0);
            right.AddFrameToReplace(20, 1);
            right.AddFrameToReplace(25, 2);
            return new AnimationFrameReplacer[] { left, right };
        }
    }
    #endregion

    #region Animation Positions
    protected override AnimationPositionCollection[] SetHandPositions
    {
        get
        {
            AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();
            left.AddFramePoint2X(10, 14, 6);
            left.AddFramePoint2X(11, 35, 14);
            left.AddFramePoint2X(12, 37, 20);
            left.AddFramePoint2X(13, 31, 29);

            left.AddFramePoint2X(14, 12, 5);
            left.AddFramePoint2X(15, 38, 7);
            left.AddFramePoint2X(16, 44, 30);

            left.AddFramePoint2X(21, 30, 40);

            right.AddFramePoint2X(10, 26, 6);
            right.AddFramePoint2X(11, 38, 14);
            right.AddFramePoint2X(12, 39, 20);
            right.AddFramePoint2X(13, 38, 29);

            right.AddFramePoint2X(14, 16, 5);
            right.AddFramePoint2X(15, 40, 7);
            right.AddFramePoint2X(16, 46, 30);

            return new AnimationPositionCollection[]
            {
                left, right
            };
        }
    }

    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(19, 17, true);
            anim.AddFramePoint2X(14, 18, 16);
            anim.AddFramePoint2X(15, 22, 16);
            anim.AddFramePoint2X(16, 25, 24);

            anim.AddFramePoint2X(17, 20, 21);
            anim.AddFramePoint2X(18, 20, 21);

            anim.AddFramePoint2X(21, 23, 26);
            anim.AddFramePoint2X(24, 23, 26);
            return anim;
        }
    }

    protected override AnimationPositionCollection SetSittingPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(23, 39, true);
            anim.AddFramePoint2X(20, 0, 0);
            return anim;
        }
    }

    protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(6, 0, true);

    protected override AnimationPositionCollection SetPlayerSittingOffset
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(7, 6 - 12, true);
            anim.AddFramePoint2X(20, -10, -16);
            return anim;
        }
    }
    protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(8, -14, true);
    protected override AnimationPositionCollection SetHeadVanityPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(25, 12, true);
            anim.AddFramePoint2X(14, 22, 12);
            anim.AddFramePoint2X(15, 28, 11);
            anim.AddFramePoint2X(16, 33, 19);
            anim.AddFramePoint2X(17, 25, 16);
            anim.AddFramePoint2X(18, 25, 16);
            anim.AddFramePoint2X(21, 30, 22);
            anim.AddFramePoint2X(22, 31, 30);
            anim.AddFramePoint2X(24, 30, 22);
            return anim;
        }
    }
    #endregion
}