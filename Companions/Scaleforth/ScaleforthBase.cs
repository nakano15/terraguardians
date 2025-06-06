using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using terraguardians.Companions.Scaleforth.Actions;
using terraguardians.Companions.Scaleforth.SubAttacks;
using terraguardians.Companions.Scaleforth;

namespace terraguardians.Companions;

public class ScaleforthBase : TerraGuardianBase
{
    const string BackWingTextureID = "backwing";
    public override string Name => "Scaleforth";
    public override string FullName => "Scaleforth Emeraldwing";
    public override string Description => "A unusual TerraGuardian that used to be a butler.";
    public override int Age => 56;
    public override Sizes Size => Sizes.ExtraLarge;
    public override Genders Gender => Genders.Male;
    public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 28);
    public override int FavoriteFood => ItemID.ChickenNugget;
    public override int SpriteWidth => 160;
    public override int SpriteHeight => 192;
    public override int FramesInRow => 13;
    public override int Width => 66;
    public override int Height => 166;
    public override int CrouchingHeight => 114;
    public override bool CanCrouch => true;
    public override float Scale => 185f / 166;
    public override bool CanUseHeavyItem => false;
    public override int InitialMaxHealth => 230; //1530
    public override int HealthPerLifeCrystal => 60;
    public override int HealthPerLifeFruit => 20;
    public override float MaxRunSpeed => 3.7f;
    public override float RunAcceleration =>  .12f;
    public override float RunDeceleration => .15f;
    public override int JumpHeight => 20;
    public override float JumpSpeed => 6.85f;
    public override float AccuracyPercent => .68f;
    public override Companion GetCompanionObject => new ScaleforthCompanion();
    public override BehaviorBase PreRecruitmentBehavior => new ScaleforthPreRecruitBehaviour();
    protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0 };
    protected override CompanionDialogueContainer GetDialogueContainer => new Scaleforth.ScaleforthDialogue();
    protected override SubAttackBase[] GetDefaultSubAttacks()
    {
        return [new FireBreathSubAttack()];
    }

    public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
    {
        InitialInventoryItems = [
            new InitialItemDefinition(ItemID.GoldBroadsword),
            new InitialItemDefinition(ItemID.HealingPotion, 10)
        ];
    }

    public override void UpdateAttributes(Companion companion)
    {
        if (companion.wingsLogic <= 0)
        {
            companion.wingsLogic = 1;
            companion.wingTimeMax = 60;
        }
        companion.noFallDmg = true;
        if (companion.velocity.Y != 0)
        {
            companion.runAcceleration += .4f;
            companion.moveSpeed *= 1.8f;
        }
    }

    public override void SetupSpritesContainer(CompanionSpritesContainer container)
    {
        container.AddExtraTexture(BackWingTextureID, "wing_back");
    }

    public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
    {
        if (!IsDrawingFrontLayer && Holder.GetCompanion.BodyFrameID != 21)
        {
            DrawData dd = new DrawData(GetSpriteContainer.GetExtraTexture(BackWingTextureID), Holder.DrawPosition, Holder.BodyFrame, Holder.DrawColor, drawSet.rotation, drawSet.rotationOrigin, Holder.GetCompanion.Scale, drawSet.playerEffect, 0);
            dd.shader = Holder.BodyShader;
            DrawDatas.Insert(0, dd);
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
    protected override Animation SetCrouchingFrames => new Animation(24);
    protected override Animation SetCrouchingSwingFrames => new Animation();
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
            anim.AddFrameToReplace(21, 2);
            anim.AddFrameToReplace(23, 3);
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
            anim[1].AddFrameToReplace(24, 4);
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
            
            for (short i = 25; i < 28; i++)
                hands[1].AddFramePoint2X(i, 59, 43);
            return hands;
        }
    }
    protected override AnimationPositionCollection SetMountShoulderPosition
    {
        get{
            AnimationPositionCollection pos = new AnimationPositionCollection(27, 36, true);
            pos.AddFramePoint2X(10, 42, 37);
            pos.AddFramePoint2X(11, 42, 37);
            pos.AddFramePoint2X(12, 42, 37);
            
            pos.AddFramePoint2X(18, 28, 50);
            pos.AddFramePoint2X(19, 28, 50);
            pos.AddFramePoint2X(20, 29, 50);
            pos.AddFramePoint2X(22, 33, 51);
			
            pos.AddFramePoint2X(24, 40, 52);
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
    protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(0, 3, true);
    protected override AnimationPositionCollection SetPlayerSittingOffset
    {
        get{
            AnimationPositionCollection pos = new AnimationPositionCollection(14f, -14, true);
            pos.AddFramePoint2X(20, -18, -22);
            return pos;
        }
    }
    protected override AnimationPositionCollection[] SetArmRelocationPosition
    {
        get{
            AnimationPositionCollection left = new AnimationPositionCollection(27, 39, true),
                right = new AnimationPositionCollection(50, 39, true);
            left.AddFramePoint2X(10, 42, 43);
            left.AddFramePoint2X(11, 42, 41);
            left.AddFramePoint2X(12, 42, 40);
            left.AddFramePoint2X(24, 33, 61);
            
            //right.AddFramePoint2X(1, 59, 43);
            right.AddFramePoint2X(10, 61, 43);
            right.AddFramePoint2X(11, 61, 41);
            right.AddFramePoint2X(12, 61, 40);
            right.AddFramePoint2X(24, 53, 61);
            return [left, right];
        }
    }
	
    protected override AnimationPositionCollection SetHeadVanityPosition
    {
        get
        {
            AnimationPositionCollection anim = new AnimationPositionCollection(-1000, -1000, true);
            return anim;
        }
    }
    #endregion

    public class ScaleforthCompanion : TerraGuardian
    {
        byte MealCheckDelay = 0;
        const byte MaxMealCheckDelay = 15;

        public override void UpdateBehaviorHook()
        {
            CheckIfCanServeMeal();
        }

        void CheckIfCanServeMeal()
        {
            MealCheckDelay++;
            if (MealCheckDelay >= MaxMealCheckDelay)
            {
                MealCheckDelay -= MaxMealCheckDelay;
                if (Owner != null && !IsRunningBehavior)
                {
                    for (int b = 0; b < Owner.buffType.Length; b++)
                    {
                        if (Owner.buffType[b] > 0 && Owner.buffTime[b] > 0 && BuffID.Sets.IsFedState[Owner.buffType[b]]) //Player is not hungry, so no feeding.
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < 50; i++)
                    {
                        if (inventory[i].type > 0 && inventory[i].buffType > -1 && BuffID.Sets.IsFedState[inventory[i].buffType])
                        {
                            RunBehavior(new ServeDinnerAction(i));
                            return;
                        }
                    }
                }
            }
        }
    }
}