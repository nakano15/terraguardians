using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace terraguardians
{
    public class TerraGuardian : Companion
    {
        private static bool SharingFurniture = false;
        public Rectangle BodyFrame = new Rectangle();
        public Rectangle BodyFrontFrame = new Rectangle();
        public Rectangle[] ArmFrame = new Rectangle[0], ArmFrontFrame = new Rectangle[0];
        public float BodyFrameTime = 0;
        private AnimationStates PreviousAnimationState = AnimationStates.Standing;
        public override bool DropFromPlatform { get { return MoveDown && ControlJump; } }
        public bool IsCrouching { get{ return MoveDown && velocity.Y == 0 && Base.CanCrouch; } }
        public Vector2 DeadBodyPosition = Vector2.Zero, DeadBodyVelocity = Vector2.Zero;
        private bool InitializedAnimationFrames = false;
        public Vector2 GetMountShoulderPosition
        {
            get
            {
                return GetAnimationPosition(AnimationPositions.MountShoulderPositions, BodyFrameID, 0) + BodyOffset;
            }
        }
        public HeldItemSetting[] HeldItems = new HeldItemSetting[0];
        internal HatCompatibilityLogger LastHatCompatibility = new HatCompatibilityLogger();

        public void OnInitializeTgAnimationFrames()
        {
            int MaxArms = Base.GetHands;
            ArmFrame = new Rectangle[MaxArms];
            ArmFrontFrame = new Rectangle[MaxArms];
            ArmFramesID = new short[MaxArms];
            ArmFrontFramesID = new short[MaxArms];
            HeldItems = new HeldItemSetting[MaxArms];
            ArmOffset = new Vector2[MaxArms];
            for (int i = 0; i < MaxArms; i++)
            {
                ArmFrame[i] = new Rectangle();
                ArmFrontFrame[i] = new Rectangle();
                HeldItems[i] = new HeldItemSetting(this) { IsActionHand = i == 0 };
                ArmOffset[i] = Vector2.Zero;
            }
            HeldItems[0].SetSettings(this);
            InitializedAnimationFrames = true;
        }

        protected override void UpdateFurniturePositioning()
        {
            Tile tile = Main.tile[furniturex, furniturey];
            SharingFurniture = false;
            if(sitting.isSitting) //Vertical issue persists. The workaround of 32 + 32 * (scale - 1) certainly wasn't a solution.
            {
                if (tile.TileType != TileID.Thrones && tile.TileType != TileID.Benches)
                {
                    Vector2 SittingPos = GetAnimationPosition(AnimationPositions.SittingPosition, BodyFrameID, AlsoTakePosition: false, ConvertToCharacterPosition: false, DiscountDirections: false);
                    //SittingPos.X = (width - SpriteWidth) * .5f + SittingPos.X;
                    SittingPos.X = SpriteWidth * .5f - SittingPos.X;
                    //if (direction > 0)
                    //    SittingPos.X += (width - SpriteWidth) * .5f;
                    SittingPos.Y = SpriteHeight - SittingPos.Y - 16; //;
                    if (Base.SitOnPlayerLapOnChair)
                    {
                        bool PlayerSittingHere = false;
                        if (GetCharacterMountedOnMe != null && GetCharacterMountedOnMe.Bottom == Bottom)
                        {
                            direction = GetCharacterMountedOnMe.direction;
                            PlayerSittingHere = true;
                        }
                        if (!PlayerSittingHere)
                        {
                            for(int p = 0; p < 255; p++)
                            {
                                if(Main.player[p].active && Main.player[p] != this && Main.player[p].sitting.isSitting && Main.player[p].Bottom == Bottom)
                                {
                                    PlayerSittingHere = true;
                                    direction = Main.player[p].direction;
                                    break;
                                }
                            }
                        }
                        if (PlayerSittingHere)
                        {
                            SharingFurniture = true;
                            Vector2 Offset = GetAnimationPosition(AnimationPositions.PlayerSittingOffset, BodyFrameID, 0, AlsoTakePosition: false, DiscountCharacterDimension: false, DiscountDirections: false, ConvertToCharacterPosition: false);
                            //Offset.X *= -1;
                            Offset.Y *= gravDir;
                            SittingPos += Offset;
                            //SittingPos.X -= width * 0.25f * direction;
                        }
                    }
                    else
                    {
                        int Index = Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(furniturex, furniturey)) - 1;
                        sitting.offsetForSeat.X -= Index * 4;
                        sitting.offsetForSeat.Y += Index * 4;
                    }
                    sitting.offsetForSeat += SittingPos;
                }
                else
                {
                    sitting.offsetForSeat.Y += (16f) * (Scale - 1);
                    int Index = Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(furniturex, furniturey)) - 1;
                    sitting.offsetForSeat.X -= Index * 4;
                    sitting.offsetForSeat.Y += Index * 4;
                }
            }
            else if(sleeping.isSleeping)
            {
                bool PlayerSleepingHere = MountStyle == MountStyles.CompanionRidesPlayer && GetCharacterMountedOnMe != null && GetCharacterMountedOnMe.Bottom == Bottom;
                if (!PlayerSleepingHere)
                {
                    for(int p = 0; p < 255; p++)
                    {
                        if(Main.player[p].active && Main.player[p] != this && Main.player[p].sleeping.isSleeping && Main.player[p].Bottom == Bottom)
                        {
                            if (Main.player[p] is not Companion || !(Main.player[p] as Companion).Base.SitOnPlayerLapOnChair)
                                PlayerSleepingHere = true;
                            break;
                        }
                    }
                }
                Vector2 SleepingPos = GetAnimationPosition(AnimationPositions.SleepingOffset, BodyFrameID, AlsoTakePosition: false, DiscountDirections: false, DiscountCharacterDimension: false, ConvertToCharacterPosition: false);
                if(PlayerSleepingHere)
                {
                    Vector2 Offset = GetAnimationPosition(AnimationPositions.PlayerSleepingCompanionOffset, BodyFrameID, 0, AlsoTakePosition: false, ConvertToCharacterPosition: false, DiscountDirections: false);
                    Offset.X *= direction;
                    Offset.Y *= gravDir;
                    SleepingPos += Offset;
                    SharingFurniture = true;
                }
                SleepingPos.X -= SleepingPos.X * (1f - Scale) * .5f;
                //SleepingPos.X *= -direction; //Need fixing
                //SleepingPos.X += -SpriteWidth * .5f + 16 * direction;
                //SleepingPos.X += (width - SpriteWidth) * .5f;
                //SleepingPos.X = SpriteWidth * .5f - SleepingPos.X * .5f + 16;// + 16;
                //if (direction > 0)
                //    SleepingPos.X += (width - SpriteWidth) * .5f;
                SleepingPos.Y += sleeping.sleepingIndex * 6;
                //SleepingPos.X = (SpriteWidth - SleepingPos.X) * direction - 16;
                //SleepingPos.Y += SpriteHeight + sleeping.sleepingIndex * 6;
                sleeping.visualOffsetOfBedBase += SleepingPos;
                fullRotation = 0;
            }
        }

        public bool MovingToOpositeDirection { get{ return (velocity.X < 0 && direction == 1) || (velocity.X > 0 && direction == -1); }}

        void TransformationFrameReverter()
        {
            switch (head)
            {
                case Terraria.ID.ArmorIDs.Head.Werewolf:
                case Terraria.ID.ArmorIDs.Head.Merman:
                    {
                        head = armor[0].headSlot;
                        if (armor[10].headSlot >= 0)
                            head = armor[10].headSlot;
                    }
                    break;
            }
        }

        public override void UpdateAnimations()
        {
            PlayerFrame();
            TransformationFrameReverter();
            int AltHat = FestiveHatSetup();
            if (head == -1)
                head = AltHat;
            if (LastHatCompatibility.LastHatID != head)
            {
                LastHatCompatibility = new HatCompatibilityLogger(head, AltHat);
            }
            AnimationStates NewState = AnimationStates.Standing;
            if (KnockoutStates > KnockoutStates.Awake && velocity.Y == 0) NewState = AnimationStates.Defeated;
            if (sitting.isSitting) NewState = AnimationStates.Sitting;
            else if (sleeping.isSleeping) NewState = AnimationStates.Sleeping;
            else if (swimTime > 0) NewState = AnimationStates.Swiming;
            else if (velocity.Y != 0 || dead) NewState = AnimationStates.InAir;
            else if (mount.Active || GetPlayerMod.GetMountedOnCompanion != null) NewState = AnimationStates.RidingMount;
            else if (sliding) NewState = AnimationStates.WallSliding;
            else if (IsCrouching) NewState = AnimationStates.Crouching;
            else if (velocity.X != 0 && (slippy || slippy2 || windPushed) && !controlLeft && !controlRight) NewState = AnimationStates.IceSliding;
            else if (velocity.X != 0) NewState = AnimationStates.Moving;
            else if (Target == null && itemAnimation == 0 && !IsRunningBehavior) NewState = AnimationStates.Idle;
            if (NewState != PreviousAnimationState)
                BodyFrameTime = 0;
            if(!InitializedAnimationFrames) OnInitializeTgAnimationFrames();
            PreviousAnimationState = NewState;
            BodyFrameID = 0;
            bool AllowMountedArmSprite = true;
            if (mount.Active || GetPlayerMod.GetMountedOnCompanion != null)
            {
                Animation anim = Base.GetAnimation(AnimationTypes.SittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.ChairSittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.StandingFrame);
                BodyFrameID = anim.UpdateTimeAndGetFrame(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse is missing, but...
            {
                if (NewState == AnimationStates.Defeated)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.DownedFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if (NewState == AnimationStates.Sitting)
                {
                    //Add a script to check if it's using a throne or sofa.
                    Point TileAtfeet = (Bottom - Vector2.UnitY * 2).ToTileCoordinates();
                    Tile tile = Main.tile[TileAtfeet.X, TileAtfeet.Y];
                        AllowMountedArmSprite = false;
                    if ((!Base.SitOnPlayerLapOnChair || !SharingFurniture) && (tile.TileType == TileID.Thrones || tile.TileType == TileID.Benches))
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.ThroneSittingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                    else
                    {
                        if (Base.GetAnimation(AnimationTypes.ChairSittingFrames).HasFrames)
                            BodyFrameID = Base.GetAnimation(AnimationTypes.ChairSittingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                        else
                            BodyFrameID = Base.GetAnimation(AnimationTypes.SittingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                }
                else if (NewState == AnimationStates.Sleeping)
                {
                    AllowMountedArmSprite = false;
                    BodyFrameID = Base.GetAnimation(AnimationTypes.BedSleepingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(itemAnimation > 0 && Items.GuardianItemPrefab.GetItemType(HeldItem) == Items.GuardianItemPrefab.ItemType.Heavy && HeldItem.useStyle == ItemUseStyleID.Swing)
                {
                    float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                    AnimationPercentage = 1f - AnimationPercentage * AnimationPercentage;
                    Animation animation = Base.GetAnimation(AnimationTypes.HeavySwingFrames);
                    BodyFrameID = animation.GetFrameFromPercentage(AnimationPercentage);
                }
                else if (swimTime > 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(2 * 1.3f, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.JumpingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(Base.CanCrouch && IsCrouching)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.CrouchingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(carpetFrame >= 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if (velocity.X != 0)
                {
                    if((slippy || slippy2 || windPushed) && !controlLeft && !controlRight)
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                    else
                    {
                        float AnimationDirection = MovingToOpositeDirection ? -1 : 1;
                        float AnimationTime = 1.3f;
                        if(WalkMode) AnimationTime *= 2;
                        BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(System.Math.Abs(velocity.X) * AnimationTime * AnimationDirection, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.GetAnimation(NewState == AnimationStates.Idle ? AnimationTypes.IdleFrames : AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
            }
            if(BodyFrameID == -1) BodyFrameID = 0;
            for(int a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFramesID[a] = BodyFrameID;
            }
            if(AllowMountedArmSprite && GetCharacterMountedOnMe != null)
            {
                switch(MountStyle)
                {
                    case MountStyles.PlayerMountsOnCompanion:
                        if(!IsCrouching && ArmFramesID.Length > 0)
                        {
                            short Frame = Base.GetAnimation(AnimationTypes.PlayerMountedArmFrame).GetFrame(0);
                            if (Frame > -1) ArmFramesID[0] = Frame;
                        }
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        {
                            if(!GoingToOrUsingFurniture)
                            {
                                Animation anim = Base.GetAnimation(AnimationTypes.SittingFrames);
                                short Frame = anim.GetFrame(0);
                                if (Frame > -1)
                                {
                                    BodyFrameID = Frame;
                                    for(int i = 0; i < ArmFramesID.Length; i++)
                                        ArmFramesID[i] = Frame;
                                }
                            }
                        }
                        break;
                }
            }
            //Item attack animations here
            {
                int heldItem = selectedItem;
                int AnimTimeBackup = itemAnimation, 
                    AnimMaxTimeBackup = itemAnimationMax;
                float ItemRotation = itemRotation;
                for(byte i = 0; i < ArmFramesID.Length; i++)
                {
                    byte Arm = i;
                    /*if(i < 2 && GetCharacterMountedOnMe != null && MountStyle == MountStyles.PlayerMountsOnCompanion && ArmFramesID.Length > 1)
                    {
                        if(i == 0)
                            Arm = 1;
                        else
                            Arm = 0;
                    }*/
                    if (i > 0)
                    {
                        itemAnimation = HeldItems[i].ItemAnimation;
                        itemAnimationMax = HeldItems[i].ItemAnimationMax;
                        selectedItem = HeldItems[i].SelectedItem;
                        itemRotation = HeldItems[i].ItemRotation;
                    }
                    bool CanVisuallyHoldItem = this.CanVisuallyHoldItem(HeldItem);
                    bool HeldItemTypeIsnt4952 = HeldItem.type != ItemID.FairyQueenMagicItem;
                    if(sandStorm)
                    {

                    }
                    else if (itemAnimation > 0 && HeldItem.useStyle != ItemUseStyleID.HiddenAnimation && HeldItemTypeIsnt4952)
                    {
                        if (!dead)
                        {
                            ArmFramesID[Arm] = GetItemUseArmFrame();
                        }
                    }
                    else
                    {
                        if (!dead)
                        {
                            short Frame = GetItemHoldArmFrame();
                            if (Frame > -1)
                            {
                                ArmFramesID[Arm] = Frame;
                            }
                        }
                    }
                }
                itemAnimation = AnimTimeBackup;
                itemAnimationMax = AnimMaxTimeBackup;
                selectedItem = heldItem;
                itemRotation = ItemRotation;
                HeldItems[0].ApplyToTg(this);
            }
            //
            reviveBehavior.UpdateAnimationFrame(this);
            GetGoverningBehavior().UpdateAnimationFrame(this);
            ModifyAnimation();
            Base.ModifyAnimation(this);
            foreach(BehaviorBase.AffectedByBehaviorInfo affected in BehaviorBase.AffectedList)
            {
                if (affected.Contains(this))
                {
                    affected.TheBehavior.UpdateAffectedCompanionAnimationFrame(this);
                }
            }
            if (SubAttackInUse < 255)
            {
                GetSubAttackActive.UpdateAnimation(this);
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            BodyFrontFrameID = Base.GetAnimationFrameReplacer(AnimationFrameReplacerTypes.BodyFront).GetFrameID(BodyFrameID);
            if (BodyFrontFrameID > -1) BodyFrontFrame = GetAnimationFrame(BodyFrontFrameID);
            else BodyFrontFrame = Rectangle.Empty;
            BodyOffset = GetAnimationPosition(AnimationPositions.BodyPositionOffset, BodyFrameID, 0, false, false, false, false, false) * Scale;
            BodyOffset.X *= direction;
            for(byte a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFrame[a] = GetAnimationFrame(ArmFramesID[a]);
                ArmFrontFramesID[a] = Base.GetArmFrontAnimationFrame(a).GetFrameID(ArmFramesID[a]);
                if (ArmFrontFramesID[a] > -1)
                    ArmFrontFrame[a] = GetAnimationFrame(ArmFrontFramesID[a]);
                else ArmFrontFrame[a] = Rectangle.Empty;
                ArmOffset[a] = GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, a, false, false, false, false, false) + 
                    (GetAnimationPosition(AnimationPositions.ArmRelocationPosition, BodyFrameID, a, false, false, false, false, false) - GetAnimationPosition(AnimationPositions.ArmRelocationPosition, ArmFramesID[a], a, false, false, false, false, false)) * Scale;

                ArmOffset[a].X *= direction;
                ArmOffset[a] += BodyOffset;
            }
            PostUpdateAnimation();
            Base.PostUpdateAnimation(this);
        }

        public short GetItemHoldArmFrame()
        {
            AnimationTypes ItemUseAnimation = AnimationTypes.ItemUseFrames;
            if(Base.CanCrouch && IsCrouching && Base.GetAnimation(AnimationTypes.CrouchingSwingFrames).HasFrames) ItemUseAnimation = AnimationTypes.CrouchingSwingFrames;
            short Frame = -1;
            switch(HeldItem.holdStyle)
            {
                case 3:
                    Frame = Base.GetAnimation(ItemUseAnimation).GetFrameFromPercentage(0.7f);
                    break;
            }
            return Frame;
        }

        public short GetItemUseArmFrame()
        {
            AnimationTypes ItemUseAnimation = AnimationTypes.ItemUseFrames;
            if(Base.CanCrouch && IsCrouching && Base.GetAnimation(AnimationTypes.CrouchingSwingFrames).HasFrames) ItemUseAnimation = AnimationTypes.CrouchingSwingFrames;
            short Frame = 0;
            Items.GuardianItemPrefab.ItemType itemType = Items.GuardianItemPrefab.GetItemType(HeldItem);
            if (GetPlayerMod.GoldenShowerStance)
            {
                Frame = Base.GetAnimation(ItemUseAnimation).GetFrameFromPercentage(1);
            }
            else if(itemType == Items.GuardianItemPrefab.ItemType.Heavy && HeldItem.useStyle == ItemUseStyleID.Swing)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                AnimationPercentage = 1f - AnimationPercentage * AnimationPercentage;
                Animation animation = Base.GetAnimation(AnimationTypes.HeavySwingFrames);
                Frame = animation.GetFrameFromPercentage(AnimationPercentage);
            }
            else if(HeldItem.useStyle == ItemUseStyleID.Swing || HeldItem.useStyle == ItemUseStyleID.MowTheLawn || HeldItem.type == ItemID.None)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrameFromPercentage(AnimationPercentage);
            }
            else if(HeldItem.useStyle == ItemUseStyleID.DrinkOld)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.67f + 0.33f) * animation.GetTotalAnimationDuration);
            }
            /*else if(HeldItem.useStyle == 2)
            {
                float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                float FramePercentage = 1;
                if (AttackPercentage < 0.1f)
                {
                    FramePercentage = AttackPercentage * 10;
                }
                else if(AttackPercentage > 0.9f)
                {
                    FramePercentage = (0.1f - (AttackPercentage - 0.9f)) * 10;
                }
                //itemRotation = (1f - (AttackPercentage * 6)) * direction * 2 - 1.4f * direction;
                Frame = Base.GetAnimation(ItemUseAnimation).GetFrameFromPercentage(0.4f + FramePercentage * 0.333f);
                //itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame);
            }*/
            else if(HeldItem.useStyle == ItemUseStyleID.DrinkLong ||HeldItem.useStyle == ItemUseStyleID.DrinkLiquid || HeldItem.useStyle == ItemUseStyleID.GolfPlay || HeldItem.useStyle == ItemUseStyleID.EatFood)
            {
                float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                Frame = Base.GetAnimation(ItemUseAnimation).GetFrameFromPercentage(System.Math.Clamp(AttackPercentage, 0, 0.6f));
            }
            else if(HeldItem.useStyle == ItemUseStyleID.DrinkLong)
            {
                float AnimationPercentage = System.Math.Min(1, (1f - (float)itemAnimation / itemAnimationMax) * 6);
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.5f + 0.5f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == ItemUseStyleID.Thrust || HeldItem.useStyle == ItemUseStyleID.Guitar)
            {
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == ItemUseStyleID.HoldUp)
            {
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrameFromPercentage(0.3f);
            }
            /*else if(HeldItem.useStyle == 13)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }*/
            else if(HeldItem.useStyle == ItemUseStyleID.Shoot || HeldItem.useStyle == ItemUseStyleID.Rapier)
            {
                Animation animation = Base.GetAnimation(ItemUseAnimation);
                if (Item.staff[HeldItem.type])
                {
                    float Percentage = (itemRotation * direction + 1) * 0.5f;
                    Frame = animation.GetFrameFromPercentage(Percentage);
                }
                else
                {
                    float RotationValue = Math.Clamp(((float)(System.Math.PI * 0.5f) + itemRotation * direction) * (float)(1f / System.Math.PI), 0, 0.999f);
                    //if(gravDir == -1)
                    //    AnimationPercentage = 1f - AnimationPercentage;
                    Frame = animation.GetFrame((short)(1 + RotationValue * (animation.GetFrameCount - 1)));
                }
            }
            return Frame;
        }

        public void OnDeath()
        {
            DeadBodyPosition = Vector2.Zero;
            DeadBodyVelocity = -Vector2.UnitY * 6;
        }

        public void UpdateDeadAnimation()
        {
            DeadBodyVelocity.Y += 0.3f;
            DeadBodyPosition += DeadBodyVelocity;
        }

        protected override void UpdateItemScript()
        {
            //OldUpdateItemScript();
            
            //ItemCheck_Inner(0);
            HeldItems[0].SetSettings(this);
            bool ItemUsePressed = controlUseItem, ItemUseReleased = releaseUseItem;
            //Not only same slot is set, but also the animation plays at the same time.
            for (byte i = 0; i < ArmFramesID.Length; i++)
            {
                byte Arm = i;
                if(Arm < 2 && GetCharacterMountedOnMe != null && MountStyle == MountStyles.PlayerMountsOnCompanion && ArmFramesID.Length > 1)
                {
                    if (Arm == 0)
                        Arm = 1;
                    else
                        Arm = 0;
                }
                using(new ItemMask(this, Arm))
                {
                    if (PlayerLoader.PreItemCheck(this))
                    {
                        controlUseItem = HeldItems[Arm].IsActionHand && ItemUsePressed;
                        releaseUseItem = HeldItems[Arm].releaseUseItem;
                        ItemCheck_Inner(Arm);
                    }
                    PlayerLoader.PostItemCheck(this);
                }
            }
            HeldItems[0].ApplyToTg(this);
            controlUseItem = ItemUsePressed;
            releaseUseItem = ItemUseReleased;
        }

        private void OldUpdateItemScript()
        {
            //using (new ItemMask(this, 0))
            {
                //HeldItems[0].SetSettings(this);
                if (PlayerLoader.PreItemCheck(this))
                {
                    ItemCheck_Inner(0);
                }
                PlayerLoader.PostItemCheck(this);
                //HeldItems[0].ApplyToTg(this);
            }
        }

        #region Item Use Scripts
        private void ItemCheck_Inner(byte Arm)
        {
            if(CCed)
            {
                channel = false;
                itemAnimation = itemAnimationMax = 0;
                return;
            }
            float HeightOffsetHitboxCenter = this.HeightOffsetHitboxCenter;
            Item item = HeldItem;
            if ((IsPlayerCharacter || IsBeingControlledBy(MainMod.GetLocalPlayer)) && PlayerInput.ShouldFastUseItem)
            {
                controlUseItem = true;
            }
            Item lastItem = itemAnimation > 0 ? lastVisualizedSelectedItem : item; //item
            Rectangle drawHitbox = Item.GetDrawHitbox(lastItem.type, this);
            ItemCheckContext context = default(ItemCheckContext);
            if(itemAnimation > 0)
            {
                ItemCheck_ApplyManaRegenDelay(item);
                if (Main.dedServ)
                {
                    itemHeight = item.height;
                    itemWidth = item.width;
                }
                else
                {
                    itemHeight = drawHitbox.Height;
                    itemWidth = drawHitbox.Width;
                }
                itemAnimation--;
                if (itemAnimation <= 0)
                {
                    itemLocation = Vector2.Zero;
                }
            }
            if (itemTime > 0)
            {
                itemTime--;
                if (ItemTimeIsZero && IsPlayerCharacter)
                {
                    if (!JustDroppedAnItem)
                    {
                        int type = item.type;
                        switch (type)
                        {
                            case 65:
                            case 676:
                            case 723:
                            case 724:
                            case 989:
                            case 1226:
                            case 1227:
                                EmitMaxManaEffect();
                                break;
                        }
                    }
                    PlayerInput.TryEndingFastUse();
                }
            }
            ItemCheck_HandleMount();
            int damage = GetWeaponDamage(item);
            ItemCheck_HandleMPItemAnimation(item);
            ItemCheck_HackHoldStyles(item);
            if (itemAnimation < 0) itemAnimation = 0;
            if (itemTime < 0) itemTime = 0;
            if (itemAnimation == 0 && reuseDelay > 0)
                ApplyReuseDelay();
            if (IsPlayerCharacter && itemAnimation == 0 && TileObjectData.CustomPlace(item.createTile, item.placeStyle))
            {
                int HackCreateTile = item.createTile;
                int HackPlaceStyle = item.placeStyle;
                if (HackCreateTile == 20)
                {
                    Tile soil = Main.tile[tileRangeX, tileRangeY + 1];
                    if (soil.HasTile)
                    {
                        TileLoader.SaplingGrowthType(soil.TileFrameY, ref HackCreateTile, ref HackPlaceStyle);
                    }
                }
                TileObject.CanPlace(tileTargetX, tileTargetY, HackCreateTile, HackPlaceStyle, direction, out var _, true);
            }
            if (itemAnimation == 0 && altFunctionUse == 2) altFunctionUse = 0;
            bool AllowUsage = true;
            if (gravDir == -1 && GolfHelper.IsPlayerHoldingClub(this))
            {
                AllowUsage = false;
            }
            if(AllowUsage && controlUseItem && releaseUseItem && itemAnimation == 0 && item.useStyle != ItemUseStyleID.None)
            {
                if (altFunctionUse == 1) altFunctionUse = 2;
                if (item.shoot == ProjectileID.None) itemRotation = 0;
                bool CanUse = ItemCheck_CheckCanUse(item);
                if (item.potion && CanUse) ApplyPotionDelay(item);
                if (item.mana > 0 && CanUse && (IsLocalCompanion || IsPlayerCharacter) && item.buffType != 0 && item.buffTime != 0)
                {
                    AddBuff(item.buffType, item.buffTime);
                }
                if (item.shoot <= ProjectileID.None || !ProjectileID.Sets.MinionTargettingFeature[item.shoot] || altFunctionUse != 2)
                {
                    ItemCheck_ApplyPetBuffs(item);
                }
                if ((IsLocalCompanion || IsPlayerCharacter) && gravDir == 1 && item.mountType != -1 && mount.CanMount(item.mountType, this))
                {
                    mount.SetMount(item.mountType, this);
                }
                if ((item.shoot <= ProjectileID.None || !ProjectileID.Sets.MinionTargettingFeature[item.shoot] || altFunctionUse != 2) && CanUse && IsLocalCompanion && item.shoot >= ProjectileID.None && (ProjectileID.Sets.LightPet[item.shoot] || Main.projPet[item.shoot]))
                {
                    FreeUpPetsAndMinions(item);
                }
                if(CanUse)
                    ItemCheck_StartActualUse(item);
            }
            if (!controlUseItem) channel = false;
            ItemLoader.HoldItem(item, this);
            if(itemAnimation > 0)
            {
                //ApplyUseStyle script.
                ItemCheck_TerraGuardiansApplyUseStyle(HeightOffsetHitboxCenter, lastItem, drawHitbox, Arm);
                ItemLoader.UseStyle(lastItem, this, drawHitbox);
            }
            else
            {
                ItemCheck_ApplyHoldStyle(HeightOffsetHitboxCenter, lastItem, drawHitbox, Arm);
                ItemLoader.HoldStyle(lastItem, this, drawHitbox);
                //ApplyHoldStyle script.
            }
            releaseUseItem = !controlUseItem;
            if (!JustDroppedAnItem)
            {
                //Effects
                bool CanShoot = true;
                int type = item.type;
                if(!ItemAnimationJustStarted)
                {
                    switch(type)
                    {
                        case 65:
                        case 676:
                        case 723:
                        case 724:
                        case 757:
                        case 674:
                        case 675:
                        case 989:
                        case 1226:
                        case 1227:
                            CanShoot = false;
                            break;
                    }
                    if(type == 3852 && altFunctionUse == 2)
                        CanShoot = false;
                    if (item.useLimitPerAnimation.HasValue && ItemUsesThisAnimation >= item.useLimitPerAnimation.Value)
                        CanShoot = false;
                }
                else
                {
                    /*switch(type)
                    {
                        case 5097:
                            //Can't set batbat to heal
                            break;
                        case 5094:
                            //Can't set tentacle spikes to spawn
                            break;
                    }*/
                }
                ItemCheck_TurretAltFeatureUse(item, CanShoot);
                ItemCheck_MinionAltFeatureUse(item, CanShoot);
                if (item.shoot > ProjectileID.None && itemAnimation > 0 && ItemTimeIsZero && CanShoot)
                {
                    //SystemMod.BackupMousePosition();
                    //ApplyCompanionMousePosition();
                    ItemCheck_Shoot(item, damage, Arm);
                    //SystemMod.RevertMousePosition();
                }
                if (IsPlayerCharacter || IsLocalCompanion)
                {
                    if (!channel)
                        toolTime = itemTime;
                    else
                    {
                        toolTime --;
                        if (toolTime < 0) toolTime = item.useTime;
                    }
                    ItemCheck_ItemUsageEffects(item);
                    PlaceThing(ref context);
                }
                if (((item.damage >= 0 && item.type > ItemID.None && !item.noMelee) || item.type == ItemID.BubbleWand || ItemID.Sets.CatchingTool[item.type] || item.type == ItemID.NebulaBlaze || item.type == ItemID.SpiritFlame) && itemAnimation > 0)
                {
                    ItemCheck_GetMeleeHitbox(item, drawHitbox, out bool CantHit, out Rectangle Hitbox);
                    if (!CantHit)
                    {
                        //ItemCheck_EmitUseVisuals()
                        //ItemCheck_CatchCritters
                        //ItemCheck_CutTiles
                        if ((IsLocalCompanion || IsPlayerCharacter) && item.damage > 0)
                        {
                            UpdateMeleeHitCooldowns();
                            int MeleeDamage = damage;
                            float kb = GetWeaponKnockback(item, item.knockBack);
                            //CutTiles Ignore list.
                            ItemCheck_MeleeHit(item, Hitbox, MeleeDamage, kb);
                        }
                    }
                }
                if (ItemTimeIsZero && itemAnimation > 0)
                {
                    if (ItemLoader.UseItem(item, this) == true)
                    {
                        ApplyItemTime(item, 1f, false);
                    }
                    if (item.healLife > 0)
                    {
                        int Heal = GetHealLife(item);
                        statLife += Heal;
                        ApplyItemTime(item);
                        if (Heal > 0 && (IsLocalCompanion || IsPlayerCharacter))
                            HealEffect(Heal);
                    }
                    if (item.healMana > 0)
                    {
                        int Heal = GetHealMana(item);
                        statMana += Heal;
                        ApplyItemTime(item);
                        if (Heal > 0 && (IsLocalCompanion || IsPlayerCharacter))
                        {
                            AddBuff(94, manaSickTime);
                            ManaEffect(Heal);
                        }
                    }
                    if (item.buffType > 0)
                    {
                        if ((IsLocalCompanion || IsPlayerCharacter) && item.buffType != 90 && item.buffType != 27)
                        {
                            AddBuff(item.buffType, item.buffTime);
                        }
                        ApplyItemTime(item);
                    }
                    if (item.type == ItemID.RedPotion && (IsLocalCompanion || IsPlayerCharacter))
                    {
                        ApplyItemTime(item);
                        if(Main.getGoodWorld)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int b = 0;
                                const int Time = 108000;
                                switch(Main.rand.Next(18)){
                                    default:
                                        b = 16;
                                        break;
                                    case 1:
                                        b = 111;
                                        break;
                                    case 2:
                                        b = 114;
                                        break;
                                    case 3:
                                        b = 8;
                                        break;
                                    case 4:
                                        b = 105;
                                        break;
                                    case 5:
                                        b = 17;
                                        break;
                                    case 6:
                                        b = 116;
                                        break;
                                    case 7:
                                        b = 5;
                                        break;
                                    case 8:
                                        b = 113;
                                        break;
                                    case 9:
                                        b = 7;
                                        break;
                                    case 10:
                                        b = 6;
                                        break;
                                    case 11:
                                        b = 104;
                                        break;
                                    case 12:
                                        b = 115;
                                        break;
                                    case 13:
                                        b = 2;
                                        break;
                                    case 14:
                                        b = 9;
                                        break;
                                    case 15:
                                        b = 3;
                                        break;
                                    case 16:
                                        b = 117;
                                        break;
                                    case 17:
                                        b = 1;
                                        break;
                                }
                                AddBuff(b, Time);
                            }
                        }
                        else
                        {
                            AddBuff(20, 216000);
                            AddBuff(22, 216000);
                            AddBuff(23, 216000);
                            AddBuff(24, 216000);
                            AddBuff(30, 216000);
                            AddBuff(31, 216000);
                            AddBuff(32, 216000);
                            AddBuff(33, 216000);
                            AddBuff(35, 216000);
                            AddBuff(36, 216000);
                            AddBuff(68, 216000);
                        }
                    }
                }
                if ((item.type == ItemID.MagicMirror || item.type == ItemID.CellPhone || item.type == ItemID.IceMirror) && itemAnimation > 0)
                {
                    if(Main.rand.NextBool(2))
                        Dust.NewDust(position, width, height, DustID.MagicMirror, 0,0, 150, Scale: 1.1f);
                    if (ItemTimeIsZero) ApplyItemTime(item);
                    else if (itemTime == (int)(itemTimeMax * 0.5f))
                    {
                        for (int i = 0; i < 70; i++)
                        {
                            Dust.NewDust(position, width, height, DustID.MagicMirror, velocity.X * 0.5f, velocity.Y * 0.5f, 150, Scale: 1.5f);
                        }
                        RemoveAllGrapplingHooks();
                        Spawn(PlayerSpawnContext.RecallFromItem);
                        for (int i = 0; i < 70; i++)
                        {
                            Dust.NewDust(position, width, height, DustID.MagicMirror, 0, 0, 150, Scale: 1.5f);
                        }
                    }
                }
                if (item.type == ItemID.MagicConch && itemAnimation > 0)
                {
                    //Effect
                    if (ItemTimeIsZero) ApplyItemTime(item);
                    else if (itemTime == 2)
                    {
                        //Should do something else on multiplayer, it seems?
                        MagicConch();
                    }
                }
                if (item.type == ItemID.DemonConch && itemAnimation > 0)
                {
                    //Effect
                    if (ItemTimeIsZero) ApplyItemTime(item);
                    else if (itemTime == 2)
                    {
                        //Should do something else on multiplayer, it seems?
                        DemonConch();
                    }
                }
                if (item.type == ItemID.PotionOfReturn && itemAnimation > 0)
                {
                    if (ItemTimeIsZero)
                    {
                        ApplyItemTime(item);
                        SoundEngine.PlaySound(SoundID.Item3, position);
                        for(byte i = 0; i < 10; i++)
                        {
                            Main.dust[Dust.NewDust(position, width, height, DustID.MagicMirror, velocity.X * 0.2f ,velocity.Y * 0.2f, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                        }
                    }
                    else if (itemTime == 20)
                    {
                        SoundEngine.PlaySound(HeldItem.UseSound, position);
                        for (int i = 0; i < 70; i++)
                        {
                            Main.dust[Dust.NewDust(position, width, height, DustID.MagicMirror, velocity.X * 0.5f, velocity.Y * 0.5f, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                        }
                        RemoveAllGrapplingHooks();
                        bool WasImmune = immune;
                        int LastImmuneTime = immuneTime;
                        Spawn(PlayerSpawnContext.RecallFromItem);
                        immune = WasImmune;
                        immuneTime = LastImmuneTime;
                        for (int i = 0; i < 70; i++)
                        {
                            Main.dust[Dust.NewDust(position, width, height, DustID.MagicMirror, 0, 0, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                        }
                        if (ItemLoader.ConsumeItem(item, this) && item.stack > 0)
                            item.stack --;
                    }
                }
                //Recall Potion
                //Teleportation Potion
                //Gender Swap Potion
                if(IsLocalCompanion || IsPlayerCharacter)
                {
                    if ((itemTimeMax != 0 && itemTime == itemTimeMax) || (!item.IsAir && item.IsNotSameTypePrefixAndStack(lastVisualizedSelectedItem)))
                        lastVisualizedSelectedItem = item.Clone();
                }
                else
                {
                    lastVisualizedSelectedItem = item.Clone();
                }
                //Tile wand and coin placement
                if(IsLocalCompanion || IsPlayerCharacter)
                {
                    if (itemTimeMax != 0 && itemTime == itemTimeMax && item.consumable)
                    {
                        bool ConsumeAmmo = true;
                        if (!item.IsACoin && item.DamageType.CountsAsClass(DamageClass.Ranged))
                        {
                            if (huntressAmmoCost90 && Main.rand.NextBool(10))
                                ConsumeAmmo = false;
                            else if (chloroAmmoCost80 && Main.rand.NextBool(5))
                                ConsumeAmmo = false;
                            else if (ammoCost80 && Main.rand.NextBool(5))
                                ConsumeAmmo = false;
                            else if (ammoCost75 && Main.rand.NextBool(4))
                                ConsumeAmmo = false;
                        }
                        bool? ForceConsumption = ItemID.Sets.ForceConsumption[item.type];
                        if(ForceConsumption.HasValue) ConsumeAmmo = ForceConsumption.Value;
                        if(ConsumeAmmo && ItemLoader.ConsumeItem(item, this))
                        {
                            if (item.stack > 0) item.stack--;
                            if (item.stack <= 0) 
                            {
                                itemTime = itemAnimation;
                                if(IsPlayerCharacter)
                                    Main.blockMouse = true;
                            }
                        }
                    }
                    if (item.stack <= 0 && itemAnimation == 0) inventory[selectedItem] = new Item();
                    if (selectedItem == 58 && itemAnimation != 0 && IsPlayerCharacter) Main.mouseItem = item.Clone();
                }
            }
            if (itemAnimation == 0)
            {
                JustDroppedAnItem = false;
                lastVisualizedSelectedItem = HeldItem.Clone();
            }
        }

        private void ItemCheck_ItemUsageEffects(Item item)
        {
            if(itemAnimation == 0 || !ItemTimeIsZero) return;
            switch(item.type)
            {
                case 29:
                    {
                        if(ConsumedLifeCrystals < 15)
                        {
                            ApplyItemTime(item);
                            UseHealthMaxIncreasingItem(20);
                            ConsumedLifeCrystals++;
                            /*statLifeMax += 20;
                            int HealthChange = Base.HealthPerLifeCrystal;
                            statLifeMax2 += HealthChange;
                            statLife += HealthChange;
                            if(IsPlayerCharacter || IsLocalCompanion)
                            {
                                HealEffect(HealthChange);
                            }*/
                        }
                    }
                    break;
                case 1291:
                    {
                        if(ConsumedLifeCrystals == 15 && ConsumedLifeFruit < 20)
                        {
                            ApplyItemTime(item);
                            UseHealthMaxIncreasingItem(5);
                            ConsumedLifeFruit++;
                            /*statLifeMax += 5;
                            int HealthChange = Base.HealthPerLifeFruit;
                            statLifeMax2 += HealthChange;
                            statLife += HealthChange;
                            if(IsPlayerCharacter || IsLocalCompanion)
                            {
                                HealEffect(HealthChange);
                            }*/
                        }
                    }
                    break;
                case 109:
                    {
                        if(ConsumedManaCrystals < 9)
                        {
                            ApplyItemTime(item);
                            UseManaMaxIncreasingItem(20);
                            ConsumedManaCrystals++;
                        }
                    }
                    break;
                case ItemID.AegisCrystal:
                    if (!usedAegisCrystal)
                    {
                        usedAegisCrystal = true;
                    }
                    break;
                case ItemID.AegisFruit:
                    if (!usedAegisFruit)
                    {
                        usedAegisFruit = true;
                    }
                    break;
                case ItemID.ArcaneCrystal:
                    if (!usedArcaneCrystal)
                    {
                        usedArcaneCrystal = true;
                    }
                    break;
                case ItemID.Ambrosia:
                    if (!usedAmbrosia)
                    {
                        usedAmbrosia = true;
                    }
                    break;
                case ItemID.GummyWorm:
                    if (!usedGummyWorm)
                    {
                        usedGummyWorm = true;
                    }
                    break;
                case ItemID.GalaxyPearl:
                    if (!usedGalaxyPearl)
                    {
                        usedGalaxyPearl = true;
                    }
                    break;
            }
        }

        private void ItemCheck_MeleeHit(Item item, Rectangle Hitbox, int Damage, float Knockback)
        {
            for(int i = 0; i < 255; i++) //I am Nakman
            {
                if (i < 200)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.immune[whoAmI] > 0 || !CanHitNPCWithMeleeHit(i) || attackCD > 0)
                    {
                        continue;
                    }
                    npc.position += npc.netOffset;
                    ProcessHitAgainstNPC(item, Hitbox, Damage, Knockback, i);
                    npc.position -= npc.netOffset;
                }
                Player player = Main.player[i];
                if (!(player is Companion))MeleeHitPlayer(player, item, Hitbox, Damage, Knockback);
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                MeleeHitPlayer(c, item, Hitbox, Damage, Knockback);
            }
        }

        private void ProcessHitAgainstNPC(Item sItem, Rectangle itemRect, int originalDamage, float knockBack, int npcIndex)
        {
            NPC npc = Main.npc[npcIndex];
            if (npc.dontTakeDamage || !CanNPCBeHitByPlayerOrPlayerProjectile(npc))
            {
                if (NPCID.Sets.ZappingJellyfish[npc.type] && itemRect.Intersects(npc.Hitbox) && (npc.noTileCollide || CanHit(npc)))
                {
                    TakeDamageFromJellyfish(npcIndex);
                }
                return;
            }
            bool? modCanHit = CombinedHooks.CanPlayerHitNPCWithItem(this, sItem, npc);
            if (!(modCanHit ?? true) || (!(modCanHit ?? false) && npc.friendly && (npc.type != NPCID.Guide || !killGuide) && (npc.type != NPCID.Clothier || !killClothier) && (!npc.isLikeATownNPC || sItem.type != 5129)))
            {
                return;
            }
            Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
            bool intersects = itemRect.Intersects(npcRect);
            /*if (sItem.type == 121) //Line 44171
            {
                GetPointOnSwungItemPath(70, 70, 0, GetAdjustedItemScale(sItem), out var location, out var outwardDirection);
                GetPointOnSwungItemPath(70, 70, 0.9f, GetAdjustedItemScale(sItem), out var location2, out outwardDirection);
                bool hitl = Utils.LineRectangleDistance(npcRect, location, location2) <= 16f;
                //intersects = () //_spawnVolcanoExplosion is private
            }*/
            bool? modCanCollide = CombinedHooks.CanPlayerMeleeAttackCollideWithNPC(this, sItem, itemRect, npc);
            if (modCanCollide == false) return;
            if (modCanCollide == true) intersects = true;
            if (!intersects || (!npc.noTileCollide && !CanHit(npc)))
                return;
            NPC.HitModifiers modifiers = npc.GetIncomingStrikeModifiers(sItem.DamageType, direction);
            float num = 1000f;
            bool crit = Main.rand.Next(1, 101) <= GetWeaponCrit(sItem);
            ApplyBannerOffenseBuff(npc, ref modifiers);
            if (parryDamageBuff && sItem.DamageType is MeleeDamageClass)
            {
                modifiers.ScalingBonusDamage += 4;
                parryDamageBuff = false;
                ClearBuff(198);
            }
            if (sItem.type == ItemID.BreakerBlade && npc.life >= npc.life * 0.9f)
                num *= 2.5f;
            if (sItem.type == ItemID.HamBat)
            {
                int stack = 0;
                if (FindBuffIndex(26) != -1) stack = 1;
                if (FindBuffIndex(206) != -1) stack = 2;
                if (FindBuffIndex(207) != -1) stack = 3;
                num = (int)(num * (1f + 0.05f * stack));
            }
            if (sItem.type == ItemID.Keybrand)
            {
                float perc = (float)npc.life / npc.lifeMax;
                float lerpValue = Utils.GetLerpValue(1f, 0.1f, perc, true);
                num = (int)(num * (1f + lerpValue));
                Vector2 point = itemRect.Center.ToVector2();
                Vector2 positionInWorld = npc.Hitbox.ClosestPointInRect(point);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
                {
                    PositionInWorld = positionInWorld
                }, whoAmI);
            }
            modifiers.SourceDamage *= num / 1000f;
            float apPercentage = 0f;
            if (sItem.type == 5129 && npc.isLikeATownNPC)
            {
                apPercentage = 1f;
                if (npc.type == NPCID.Nurse)
                    modifiers.TargetDamageMultiplier *= 2;
            }
            if (sItem.type == ItemID.SlapHand)
            {
                ParticleOrchestraSettings particleOrchestraSettings3 = default(ParticleOrchestraSettings);
                particleOrchestraSettings3.PositionInWorld = npc.Center;
                ParticleOrchestraSettings settings = particleOrchestraSettings3;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.SlapHand, settings, whoAmI);
            }
            if (sItem.type == 5382)
            {
                ParticleOrchestraSettings particleOrchestraSettings2 = default(ParticleOrchestraSettings);
                particleOrchestraSettings2.PositionInWorld = npc.Center;
                ParticleOrchestraSettings settings2 = particleOrchestraSettings2;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.WaffleIron, settings2, whoAmI);
            }
            if (sItem.type == 5129)
            {
                ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
                particleOrchestraSettings.PositionInWorld = npc.Center;
                ParticleOrchestraSettings settings3 = particleOrchestraSettings;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlyMeal, settings3, whoAmI);
            }
            StatusToNPC(sItem.type, npcIndex);
            if (npc.life > 5)
            {
                OnHit(npc.Center.X, npc.Center.Y, npc);
            }
            modifiers.ArmorPenetration += (float)GetWeaponArmorPenetration(sItem);
            modifiers.ScalingArmorPenetration += apPercentage;
            CombinedHooks.ModifyPlayerHitNPCWithItem(this, sItem, npc, ref modifiers);
            NPC.HitInfo strike = modifiers.ToHitInfo(originalDamage, crit, knockBack, true, luck);
            NPCKillAttempt attempt = new NPCKillAttempt(npc);
            int dmgDone = npc.StrikeNPC(strike);
            CombinedHooks.OnPlayerHitNPCWithItem(this, sItem, npc, in strike, dmgDone);
            //44285
            ApplyNPCOnHitEffects(sItem, itemRect, strike.SourceDamage, strike.Knockback, npcIndex, strike.SourceDamage, dmgDone);
            int bannerid = Item.NPCtoBanner(npc.BannerID());
            if (bannerid >= 0)
            {
                lastCreatureHit = bannerid;
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendStrikeNPC(npc, in strike);
            }
            if (accDreamCatcher && !npc.HideStrikeDamage)
            {
                addDPS(dmgDone);
            }
            SetMeleeHitCooldown(npcIndex, itemAnimation);
            if (attempt.DidNPCDie())
                OnKillNPC(ref attempt,sItem);
            ApplyAttackCooldown();
        }

        private void ApplyNPCOnHitEffects(Item sItem, Rectangle itemRectangle, int damage, float knockBack, int npcIndex, int dmgRandomized, int dmgDone)
        {
            NPC npc = Main.npc[npcIndex];
            bool mortal = !npc.immortal;
            if (sItem.type == ItemID.Bladetongue)
            {
                Vector2 vel = new Vector2(direction * 100 + Main.rand.Next(-25, 26), Main.rand.Next(-75, 76));
                vel.Normalize();
                vel *= Main.rand.Next(30, 41) * 0.1f;
                Vector2 pos = new Vector2(itemRectangle.X + Main.rand.Next(itemRectangle.Width), itemRectangle.Y + Main.rand.Next(itemRectangle.Height));
                pos = (pos + npc.Center * 2) / 3f;
			    Projectile.NewProjectile(GetProjectileSource_Item(sItem), pos.X, pos.Y, vel.X, vel.Y, 524, (int)((double)damage * 0.5), knockBack * 0.7f, whoAmI);
            }
            if (beetleOffense && mortal)
            {
                beetleCounter += dmgDone;
                beetleCountdown = 0;
            }
            if (meleeEnchant == 7)
            {
                Projectile.NewProjectile(GetSource_Misc("WeaponEnchantment_Confetti"), Main.npc[npcIndex].Center.X, Main.npc[npcIndex].Center.Y, Main.npc[npcIndex].velocity.X, Main.npc[npcIndex].velocity.Y, 289, 0, 0f, whoAmI);
            }
            if (sItem.type == ItemID.PsychoKnife)
            {
                stealth = 1f;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    //NetMessage.SendData(84, -1, -1, null, whoAmI);
                }
            }
            if (sItem.type == ItemID.TentacleSpike)
            {
                //TentacleSpike_TrySpiking(Main.npc[npcIndex], sItem, damage, knockBack);
            }
            if (sItem.type == ItemID.BloodButcherer)
            {
                //BloodButcherer_TryButchering(Main.npc[npcIndex], sItem, damage, knockBack);
            }
            if (sItem.type == ItemID.FieryGreatsword)
            {
                //Volcano_TrySpawningVolcano(Main.npc[npcIndex], sItem, (int)((float)damage * 0.75f), knockBack, itemRectangle);
            }
            if (sItem.type == ItemID.BatBat)
            {
                //BatBat_TryLifeLeeching(Main.npc[npcIndex]);
            }
            if (sItem.type == ItemID.BeeKeeper && mortal)
            {
                int num = Main.rand.Next(1, 4);
                if (strongBees && Main.rand.NextBool(3))
                {
                    num++;
                }
                for (int i = 0; i < num; i++)
                {
                    float num4 = (float)(direction * 2) + (float)Main.rand.Next(-35, 36) * 0.02f;
                    float num5 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    num4 *= 0.2f;
                    num5 *= 0.2f;
                    int num6 = Projectile.NewProjectile(GetProjectileSource_Item(sItem), itemRectangle.X + itemRectangle.Width / 2, itemRectangle.Y + itemRectangle.Height / 2, num4, num5, beeType(), beeDamage(dmgRandomized / 3), beeKB(0f), whoAmI);
                    Main.projectile[num6].DamageType = new MeleeDamageClass();
                }
            }
            //_spawnMuramasa is private
            /*if (sItem.type == 155 && mortal && _spawnMuramasaCut)
            {
                _spawnMuramasaCut = false;
                int num7 = Main.rand.Next(1, 4);
                num7 = 1;
                for (int j = 0; j < num7; j++)
                {
                    NPC nPC = Main.npc[npcIndex];
                    Rectangle hitbox = nPC.Hitbox;
                    ((Rectangle)(ref hitbox)).Inflate(30, 16);
                    hitbox.Y -= 8;
                    Vector2 vector3 = Main.rand.NextVector2FromRectangle(hitbox);
                    Vector2 val = ((Rectangle)(ref hitbox)).Center.ToVector2();
                    Vector2 spinningpoint = (val - vector3).SafeNormalize(new Vector2((float)direction, gravDir)) * 8f;
                    Main.rand.NextFloat();
                    float num8 = (float)(Main.rand.Next(2) * 2 - 1) * ((float)Math.PI / 5f + (float)Math.PI * 4f / 5f * Main.rand.NextFloat());
                    num8 *= 0.5f;
                    spinningpoint = spinningpoint.RotatedBy(0.7853981852531433);
                    int num9 = 3;
                    int num10 = 10 * num9;
                    int num11 = 5;
                    int num2 = num11 * num9;
                    vector3 = val;
                    for (int k = 0; k < num2; k++)
                    {
                        vector3 -= spinningpoint;
                        spinningpoint = spinningpoint.RotatedBy((0f - num8) / (float)num10);
                    }
                    vector3 += nPC.velocity * (float)num11;
                    Projectile.NewProjectile(GetProjectileSource_Item(sItem), vector3, spinningpoint, 977, (int)((float)dmgRandomized * 0.5f), 0f, whoAmI, num8);
                }
            }*/
            if (Main.npc[npcIndex].value > 0f && hasLuckyCoin && Main.rand.NextBool(5))
            {
                int type = 71;
                if (Main.rand.NextBool(10))
                {
                    type = 72;
                }
                if (Main.rand.NextBool(100))
                {
                    type = 73;
                }
                int num3 = Item.NewItem(GetSource_OnHit(Main.npc[npcIndex], "LuckyCoin"), (int)Main.npc[npcIndex].position.X, (int)Main.npc[npcIndex].position.Y, Main.npc[npcIndex].width, Main.npc[npcIndex].height, type);
                Main.item[num3].stack = Main.rand.Next(1, 11);
                Main.item[num3].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                Main.item[num3].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)direction;
                Main.item[num3].timeLeftInWhichTheItemCannotBeTakenByEnemies = 60;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(148, -1, -1, null, num3);
                }
            }
        }

        internal IEntitySource GetProjectileSource_Buff(int buffIndex)
        {
            int buffId = buffType[buffIndex];
            return new EntitySource_Buff(this, buffId, buffIndex);
        }

        internal IEntitySource GetProjectileSource_Item(Item item)
        {
            return GetSource_ItemUse(item);
        }

        internal IEntitySource GetItemSource_OpenItem(int itemType)
        {
            return GetSource_OpenItem(itemType);
        }

        internal IEntitySource GetItemSource_Death()
        {
            return GetSource_Death();
        }

        internal IEntitySource GetProjectileSource_Item_WithPotentialAmmo(Item item, int ammoItemId)
        {
            return GetSource_ItemUse_WithPotentialAmmo(item, ammoItemId);
        }

        internal IEntitySource GetProjectileSource_Accessory(Item item)
        {
            return GetSource_Accessory(item);
        }

        internal IEntitySource GetProjectileSource_TileInteraction(int tileCoordsX, int tileCoordsY)
        {
            return GetSource_TileInteraction(tileCoordsX, tileCoordsY);
        }

        internal IEntitySource GetItemSource_TileInteraction(int tileCoordsX, int tileCoordsY)
        {
            return GetSource_TileInteraction(tileCoordsX, tileCoordsY);
        }

        internal IEntitySource GetNPCSource_TileInteraction(int tileCoordsX, int tileCoordsY)
        {
            return GetSource_TileInteraction(tileCoordsX, tileCoordsY);
        }

        private void GetPointOnSwungItemPath(float spriteWidth, float spriteHeight, float normalizedPointOnPath, float itemScale, out Vector2 location, out Vector2 outwardDirection)
        {
            float num = (float)Math.Sqrt(spriteWidth * spriteWidth + spriteHeight * spriteHeight);
            float num2 = (float)(direction == 1).ToInt() * ((float)Math.PI / 2f);
            if (gravDir == -1f)
            {
                num2 += (float)Math.PI / 2f * (float)direction;
            }
            outwardDirection = itemRotation.ToRotationVector2().RotatedBy(3.926991f + num2);
            location = RotatedRelativePoint(itemLocation + outwardDirection * num * normalizedPointOnPath * itemScale);
        }

        private void MeleeHitPlayer(Player player, Item item, Rectangle Hitbox, int Damage, float Knockback)
        {
            if (player.active)
            {
            }
            if(player == this || !player.active || !player.hostile || player.immune || player.dead || !PlayerMod.IsEnemy(this, player) || !Hitbox.Intersects(player.Hitbox) || !CanHit(player) || !ItemLoader.CanHitPvp(item, this, player) || !PlayerLoader.CanHitPvp(this, item, player))
                return;
            Player playerBackup = Main.player[player.whoAmI];
            Main.player[player.whoAmI] = player;
            //bool Critical = Main.rand.Next(1, 101) <= 10;
            int NewDamage = Main.DamageVar(Damage, luck);
            StatusToPlayerPvP(item.type, player.whoAmI);
            OnHit(player.Center.X, player.Center.Y, player);
            PlayerDeathReason deathReason = PlayerDeathReason.ByPlayerItem(whoAmI, item);
            int FinalDamage = (int)player.Hurt(deathReason, NewDamage, direction, true, false);
            if (item.type == ItemID.Bladetongue)
            {
                Vector2 ProjSpawnDirection = new Vector2(direction * 100 * Main.rand.Next(-25, 26), Main.rand.Next(-75, 76));
                ProjSpawnDirection.Normalize();
                ProjSpawnDirection *= Main.rand.Next(30, 41) * 0.1f;
                Vector2 ProjSpawnPos = (ProjSpawnDirection + player.Center * 2) * (1f / 3f);
                Projectile.NewProjectile(GetSource_ItemUse(item), ProjSpawnPos.X, ProjSpawnPos.Y, ProjSpawnDirection.X, ProjSpawnDirection.Y, 524, (int)(Damage * 0.7f), Knockback * 0.7f, whoAmI);
            }
            //BatBat leech health, if the method and variable somehow goes unprivate.
			if (item.type == ItemID.BatBat)
			{
				//BatBat_TryLifeLeeching(player);
			}
            if(beetleOffense)
            {
                beetleCountdown += FinalDamage;
                beetleCountdown = 0;
            }
            if (meleeEnchant == 7)
            {
                Projectile.NewProjectile(GetSource_Misc("WeaponEnchantment_Confetti"), player.Center.X, player.Center.Y, player.velocity.X, player.velocity.Y, 289, 0, 0f, whoAmI);
            }
            if (item.type == ItemID.BeeKeeper) //The bees!
            {
                int bees = Main.rand.Next(1, 4);
                if (strongBees && Main.rand.NextBool(3))
                {
                    bees++;
                }
                for (int j = 0; j < bees; j++)
                {
                    float num4 = (float)(direction * 2) + (float)Main.rand.Next(-35, 36) * 0.02f;
                    float num5 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    num4 *= 0.2f;
                    num5 *= 0.2f;
                    int p = Projectile.NewProjectile(GetSource_ItemUse(HeldItem), Hitbox.X + (Hitbox.Width * 0.5f), Hitbox.Y + (int)(Hitbox.Height * 0.5f), num4, num5, beeType(), beeDamage(NewDamage / 3), beeKB(0f), whoAmI);
                    Main.projectile[p].DamageType = new MeleeDamageClass();
                }
            }
            if (item.type == ItemID.PsychoKnife)
            {
                stealth = 1f;
            }
            //How to send player hurt of a companion?
            /*if(Main.netMode != 0)
            {
                NetMessage.SendPlayerHurt(player.whoAmI, deathReason, NewDamage, direction, Critical, true, -1);
            }*/
            ApplyAttackCooldown();
            Main.player[player.whoAmI] = playerBackup;
        }

        private void ItemCheck_GetMeleeHitbox(Item item, Rectangle itemframe, out bool CantHit, out Rectangle Hitbox)
        {
            CantHit = false;
            Hitbox = new Rectangle((int)itemLocation.X, (int)itemLocation.Y, 32, 32);
            if (!Main.dedServ)
            {
                int w = itemframe.Width, h = itemframe.Height;
                switch(item.type)
                {
                    case 5094:
                    case 5095:
                        w -= 10;
                        h -= 10;
                        break;
                    case 5096:
                        w -= 12;
                        h -= 12;
                        break;
                    case 5097:
                        w -= 8;
                        h -= 8;
                        break;
                }
                Hitbox.Width = w;
                Hitbox.Height = h;
            }
            float ItemScale = GetAdjustedItemScale(item);
            Hitbox.Width = (int)(Hitbox.Width * ItemScale);
            Hitbox.Height = (int)(Hitbox.Height * ItemScale);
            if (direction == -1) Hitbox.X -= Hitbox.Width;
            if (gravDir == 1) Hitbox.Y -= Hitbox.Height;
            switch (item.useStyle)
            {
                case 1:
                    {
                        float itemAnimation = this.itemAnimation;
                        if(Items.GuardianItemPrefab.GetItemType(item) == Items.GuardianItemPrefab.ItemType.Heavy)
                        {
                            itemAnimation *= itemAnimation; // / itemAnimationMax;
                        }
                        if (itemAnimation < itemAnimationMax * 0.333f)
                        {
                            if (direction == -1)
                                Hitbox.X -= (int)(Hitbox.Width * 1.4f - Hitbox.Width);
                            Hitbox.Width = (int)(Hitbox.Width * 1.4f);
                            Hitbox.Y += (int)(Hitbox.Height * 0.5f * gravDir);
                        }
                        else if (itemAnimation < itemAnimationMax * 0.666f)
                        {
                            if (direction == -1)
                                Hitbox.X -= (int)(Hitbox.Width * 1.2f);
                            Hitbox.Width *= 2;
                            Hitbox.Y -= (int)((Hitbox.Height * 1.4f - Hitbox.Height) * gravDir);
                            Hitbox.Height = (int)(Hitbox.Height * 1.4f);
                        }
                    }
                    break;
                case 3:
                    if (itemAnimation > itemAnimationMax * 0.666f)
                    {
                        CantHit = true;
                    }
                    else
                    {
                        if (direction == -1)
                            Hitbox.X -= (int)(Hitbox.Width * 1.4f - Hitbox.Width);
                        Hitbox.Width = (int)(Hitbox.Width * 1.4f);
                        Hitbox.Y += (int)(Hitbox.Height * 0.6f);
                        Hitbox.Height = (int)(Hitbox.Height * 0.6f);
                        if (item.type == ItemID.Umbrella || item.type == ItemID.TragicUmbrella)
                        {
                            Hitbox.Height += 14;
                            Hitbox.Width -= 10;
                            if (direction == -1) Hitbox.X += 10;
                        }
                    }
                    break;
            }
            ItemLoader.UseItemHitbox(item, this, ref Hitbox, ref CantHit);
            //Item 1450 effect
            if (item.type == ItemID.NebulaBlaze) CantHit = true;
            if (item.type == ItemID.SpiritFlame)
            {
                CantHit = true;
                //Its effect script
            }
        }

        private void ItemCheck_Shoot(Item item, int Damage, byte Hand)
        {
            if (!CombinedHooks.CanShoot(this, item)) return;
            int ProjToShoot = item.shoot;
            float ProjSpeed = item.shootSpeed;
            int ProjDamage = Damage;
            float Knockback = item.knockBack;
            if (item.noMelee && ProjToShoot != 699 && ProjToShoot != 707 && ProjToShoot > 879) //It was actually (uint)(ProjToShoot - 877) > 2u, so I thought this would be a good variant.
            {
                ProjSpeed /= 1f / GetTotalAttackSpeed(DamageClass.Melee);
            }
            if (item.CountsAsClass(DamageClass.Throwing) && ProjSpeed < 16)
            {
                ProjSpeed *= ThrownVelocity;
                if (ProjSpeed > 16) ProjSpeed = 16;
            }
            if (IsLocalCompanion && (ProjToShoot == 13 || ProjToShoot == 32 || ProjToShoot == 315 || (ProjToShoot >= 230 && ProjToShoot <= 235) || ProjToShoot == 331))
            {
                grappling[0] = -1;
                grapCount = 0;
                for(int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this))
                    {
                        if(Main.projectile[i].type >= 230 && Main.projectile[i].type <= 235)
                        {
                            Main.projectile[i].Kill();
                        }
                        else
                        {
                            switch(Main.projectile[i].type)
                            {
                                case 13:
                                case 331:
                                case 315:
                                    Main.projectile[i].Kill();
                                    break;
                            }
                        }
                    }
                }
            }
            bool CanShoot = true;
            int UsedAmmoItemId = 0;
            if (item.useAmmo > 0)
            {
                CanShoot = PickAmmo(item, out ProjToShoot, out ProjSpeed, out ProjDamage, out Knockback, out UsedAmmoItemId, ItemID.Sets.gunProj[item.type]);
            }
            if (ItemID.Sets.gunProj[item.type])
            {
                Knockback = item.knockBack;
                ProjDamage = Damage;
                ProjSpeed = item.shootSpeed;
            }
            if (item.IsACoin) CanShoot = false;
            if (ProjToShoot == 14)
            {
                if(item.type == ItemID.SniperRifle || item.type == ItemID.VenusMagnum || item.type == ItemID.Uzi)
                    ProjToShoot = 242;
            }
            if (item.type == ItemID.NebulaBlaze)
            {
                if (Main.rand.Next(100) < 20)
                {
                    ProjToShoot++;
                    Damage *= 3;
                }
                else
                {
                    ProjSpeed --;
                }
            }
            if (item.type == ItemID.ChristmasTreeSword)
                Damage = (int)(Damage * 0.75f);
            if (ProjToShoot == 73)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == i)
                    {
                        if (Main.projectile[i].type == 73) ProjToShoot = 74;
                        if (ProjToShoot == 74 && Main.projectile[i].type == 74) CanShoot = false;
                    }
                }
            }
            if (CanShoot)
            {
                if (!IsLocalCompanion && !IsPlayerCharacter)
                {
                    ApplyItemTime(item);
                    return;
                }
                Knockback = GetWeaponKnockback(item, Knockback);
                IEntitySource projSource = GetSource_ItemUse_WithPotentialAmmo(item, UsedAmmoItemId);
                if(ProjToShoot == 228) Knockback = 0;
                if (ProjToShoot == 1 && item.type == ItemID.MoltenFury) ProjToShoot = 2;
                switch(item.type)
                {
                    case 682:
                        ProjToShoot = 117;
                        break;
                    case 725:
                        ProjToShoot = 120;
                        break;
                    case 2796:
                        ProjToShoot = 442;
                        break;
                    case 2223:
                        ProjToShoot = 357;
                        break;
                    case 5117:
                        ProjToShoot = 968;
                        break;
                }
                ApplyItemTime(item);
                Vector2 AimDestination = GetAimDestinationPosition(GetAimedPosition);
                direction = Center.X < AimDestination.X ? 1 : -1;
                Vector2 FiringPosition = Center;
                if (item.useStyle == ItemUseStyleID.Shoot)
                {
                    Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                    Vector2 AimDirection = AimDestination - MountedCenter;
                    AimDirection.Normalize();
                    float ArmFramePosition = (float)System.Math.Atan2(AimDirection.Y * direction, AimDirection.X * direction);
                    ArmFramePosition = Math.Clamp((((float)System.Math.PI * 0.5f) + ArmFramePosition * direction) * (float)(1f / System.Math.PI), 0, 0.999f);
                    FiringPosition = GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrame((short)(1 + ArmFramePosition * (anim.GetTotalAnimationDuration - 1))), Hand) +
                        GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                }
                else if (item.useStyle == ItemUseStyleID.Swing)
                {
                    FiringPosition = GetAnimationPosition(AnimationPositions.HandPosition, Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrameFromPercentage(.5f), Hand);
                }
                else
                {
                    FiringPosition = Center /*GetAnimationPosition(AnimationPositions.HandPosition, GetItemUseArmFrame(), Hand)*/;
                }
                if(item.type == ItemID.ChainGun || item.type == ItemID.Gatligator)
                {
                    AimDestination.X += Main.rand.Next(-50, 51) * 0.03f;
                    AimDestination.Y += Main.rand.Next(-50, 51) * 0.03f;
                }
                Vector2 FireDirection = AimDestination - FiringPosition;
                FireDirection.Normalize();
                switch (item.useStyle)
                {
                    case 5:
                        {
                            if(item.type == ItemID.DaedalusStormbow)
                            {

                            }
                            else if (item.type == ItemID.BloodRainBow)
                            {
                                
                            }
                            else if (item.type == ItemID.SpiritFlame)
                            {
                                itemRotation = 0;
                            }
                            else
                            {
                                itemRotation = (float)Math.Atan2(FireDirection.Y * direction, FireDirection.X * direction) - fullRotation;
                            }
                        }
                        break;
                }
                if (ProjToShoot == 9)
                {
                    Damage *= 2;
                    Knockback = 0;
                }
                switch(item.type)
                {
                    case 757:
                    case 675:
                        Damage = (int)(Damage * 1.5f);
                        break;
                    case 986:
                    case 281:
                        FiringPosition.X += 6 * direction;
                        FiringPosition.Y -= 6 * gravDir;
                        break;
                    case 3007:
                        FiringPosition.X -= 4 * direction;
                        FiringPosition.Y -= 2f * gravDir;
                        break;
                }
                if (ProjToShoot == 250)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && (Main.projectile[i].type == 250 || Main.projectile[i].type == 251))
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                }
                FireDirection *= ProjSpeed;
                CombinedHooks.ModifyShootStats(this, item, ref FiringPosition, ref FireDirection, ref ProjToShoot, ref Damage, ref Knockback);
                if(item.useStyle == ItemUseStyleID.Shoot)
                {
                    switch (item.type)
                    {
                        case 3029:
                        case 4381:
                            FireDirection.X = GetAimedPosition.X - FiringPosition.X;
                            FireDirection.Y = GetAimedPosition.Y - FiringPosition.Y - 1000f;
                            itemRotation = (float)Math.Atan2(FireDirection.Y * direction, FireDirection.X * direction);
                            break;
                        case 3779:
                            itemRotation = 0;
                            break;
                        default:
                            itemRotation = (float)Math.Atan2(FireDirection.Y * direction, FireDirection.X * direction);
                            break;
                    }
                }
                if (item.useStyle == ItemUseStyleID.Rapier)
                {
                    itemRotation = (float)Math.Atan2(FireDirection.Y * direction, FireDirection.X * direction) - fullRotation;
                }
                //ProjSpeed = FireDirection.Length();
                //FireDirection.Normalize();
                if (!CombinedHooks.Shoot(this, item, (EntitySource_ItemUse_WithAmmo)projSource, FiringPosition, FireDirection, ProjToShoot, Damage, Knockback)) return;
                switch(item.type)
                {
                    default:
                        if(ProjectileID.Sets.IsAGolfBall[ProjToShoot])
                        {

                        }
                        else if (ProjToShoot == 76) //Leaves a floating harp afterwards
                        {
                            ProjToShoot += Main.rand.Next(3);
                            float WhateverThisDivisionIs = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
                            WhateverThisDivisionIs = Math.Min(1f, velocity.Length() / (WhateverThisDivisionIs * 0.5f));
                            if(WhateverThisDivisionIs > 1) WhateverThisDivisionIs = 1;
                            velocity.X += Main.rand.Next(-40, 41) * 0.01f;
                            velocity.Y += Main.rand.Next(-40, 41) * 0.01f;
                            velocity *= WhateverThisDivisionIs + 0.25f;
                            int p = Projectile.NewProjectile(projSource, FiringPosition.X, FiringPosition.Y, FireDirection.X, FireDirection.Y, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            Main.projectile[p].ai[1] = 1;
                            WhateverThisDivisionIs = (float)Math.Round(Math.Clamp(WhateverThisDivisionIs * 2 - 1, -1f, 1f) * (float)musicNotes);
                            WhateverThisDivisionIs /= (float)musicNotes;
                            Main.projectile[p].ai[0] = WhateverThisDivisionIs;
                        }
                        else if(item.shoot > ProjectileID.None && (Main.projPet[item.shoot] || item.shoot == ProjectileID.BlueFairy || item.shoot == ProjectileID.ShadowOrb || item.shoot == ProjectileID.CrimsonHeart || item.shoot == ProjectileID.SuspiciousTentacle) && !item.DamageType.CountsAsClass<SummonDamageClass>())
                        {
                            for(int i = 0; i < 1000; i++)
                            {
                                if (!Main.projectile[i].active || !ProjMod.IsThisCompanionProjectile(i, this)) continue;
                                if (item.shoot == ProjectileID.BlueFairy)
                                {
                                    if (Main.projectile[i].type == 72 || Main.projectile[i].type == 86 || Main.projectile[i].type == 87)
                                        Main.projectile[i].Kill();
                                }
                                else if (item.shoot == Main.projectile[i].type)
                                    Main.projectile[i].Kill();
                            }
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, 0, 0, whoAmI);
                        }
                        else
                        {
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 3029:
                        {
                            byte Arrows = 3;
                            switch(ProjToShoot)
                            {
                                case 91:
                                case 4:
                                case 5:
                                case 41:
                                    Arrows--;
                                    break;
                                default:
                                    if(Main.rand.NextBool(3))
                                        Arrows++;
                                    break;
                            }
                            for(byte i = 0; i < Arrows; i++)
                            {
                                Vector2 ShotSpawnPos = new Vector2(position.X + width * 0.5f + Main.rand.Next(201) * -direction + (GetAimedPosition.X - position.X), Center.Y - 600f);
                                ShotSpawnPos.X = (ShotSpawnPos.X * 10 + Center.X) / 11f + Main.rand.Next(-100, 101);
                                ShotSpawnPos.Y -= 150 * i;
                                FireDirection = GetAimedPosition - ShotSpawnPos;
                                if(FireDirection.Y < 0)
                                    FireDirection *= -1f;
                                if (FireDirection.Y < 20) FireDirection.Y = 20;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed;
                                FireDirection.X += Main.rand.Next(-40, 41) * 0.03f;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.03f;
                                FireDirection.X *= Main.rand.Next(75, 150) * 0.01f;
                                ShotSpawnPos.X += Main.rand.Next(-50, 51);
                                int p = Projectile.NewProjectile(projSource, ShotSpawnPos, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].noDropItem = true;
                            }
                        }
                        break;
                    case 4381:
                        {
                            byte Arrows = (byte)Main.rand.Next(1, 3);
                            if (Main.rand.NextBool(3)) Arrows++;
                            for(byte i = 0; i < Arrows; i++)
                            {
                                Vector2 ShotSpawnPos = new Vector2(position.X + width * 0.5f + Main.rand.Next(61) * -direction + (GetAimedPosition.X - position.X), Center.Y - 600f);
                                ShotSpawnPos.X = (ShotSpawnPos.X * 10 + Center.X) / 11f + Main.rand.Next(-30, 31);
                                ShotSpawnPos.Y -= 150 * Main.rand.NextFloat();
                                FireDirection = GetAimedPosition - ShotSpawnPos;
                                if(FireDirection.Y < 0)
                                    FireDirection *= -1f;
                                if (FireDirection.Y < 20)
                                    FireDirection.Y = 20;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed;
                                FireDirection.X += Main.rand.Next(-20, 21) * 0.03f;
                                FireDirection.Y += Main.rand.Next(-20, 21) * 0.03f;
                                FireDirection.X *= Main.rand.Next(55, 80) * 0.01f;
                                ShotSpawnPos.X += Main.rand.Next(-50, 51);
                                int p = Projectile.NewProjectile(projSource, ShotSpawnPos, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].noDropItem = true;
                            }
                        }
                        break;
                    case 98:
                    case 533:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.01f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.01f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 1319:
                    case 3107:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.02f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 3053:
                        {
                            FireDirection.Normalize();
                            Vector2 RandomDirection = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                            RandomDirection.Normalize();
                            FireDirection = FireDirection * 4 + RandomDirection;
                            FireDirection.Normalize();
                            FireDirection *= item.shootSpeed;
                            float AI0 = Main.rand.Next(10, 80) * 0.001f;
                            if(Main.rand.NextBool(2)) AI0 *= -1f;
                            float AI1 = Main.rand.Next(10, 80) * 0.001f;
                            if(Main.rand.NextBool(2)) AI1 *= -1f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, AI0, AI1);
                        }
                        break;
                    case 3019:
                        {
                            Vector2 EndPoint = new Vector2(FireDirection.X, FireDirection.Y);
                            EndPoint.X += Main.rand.Next(-100, 101) * 0.01f * ProjSpeed * 0.15f;
                            EndPoint.Y += Main.rand.Next(-100, 101) * 0.01f * ProjSpeed * 0.15f;
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.03f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.03f;
                            EndPoint.Normalize();
                            EndPoint *= ProjSpeed;
                            FireDirection.X *= Main.rand.Next(50, 150) * 0.01f;
                            FireDirection.Y *= Main.rand.Next(50, 150) * 0.01f;
                            FireDirection *= ProjSpeed;
                            FireDirection.X += Main.rand.Next(-100, 101) * 0.025f;
                            FireDirection.Y += Main.rand.Next(-100, 101) * 0.025f;
                            FireDirection.Normalize();
                            FireDirection *= ProjSpeed;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, EndPoint.X, EndPoint.Y);
                        }
                        break;
                    case 2797: //Projectile that generates from it is following mouse cursor, and doesn't affect foes.
                        {
                            Vector2 SpawnPos = Vector2.Normalize(FireDirection) * 40f * item.scale;
                            if (Collision.CanHit(FiringPosition, 0, 0, FiringPosition + SpawnPos, 0, 0))
                            {
                                FiringPosition += SpawnPos;
                            }
                            float ai = Utils.ToRotation(FireDirection);
                            const float RadiusFactor = (float)Math.PI * 2f / 3f;
                            byte Bullets = 4;
                            if (Main.rand.NextBool(4)) Bullets++;
                            for(byte b = 0; b < Bullets; b++)
                            {
                                float Scale = Main.rand.NextFloat() * 0.2f + 0.05f;
                                double Radians = RadiusFactor * Main.rand.NextDouble() - RadiusFactor / 2f;
                                Vector2 Direction = Utils.RotatedBy(FireDirection, Radians) * Scale;
                                int p = Projectile.NewProjectile(projSource, FiringPosition, Direction, 444, Damage, Knockback, whoAmI, ai);
                                Main.projectile[p].localAI[0] = ProjToShoot;
                                Main.projectile[p].localAI[1] = ProjSpeed;
                            }
                        }
                        break;
                    case 2270:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.05f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.05f;
                            if (Main.rand.NextBool(3))
                            {
                                FireDirection.X *= 1f + Main.rand.Next(-30, 31) * 0.02f;
                                FireDirection.Y *= 1f + Main.rand.Next(-30, 31) * 0.02f;
                            }
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 5117:
                        {
                            FireDirection.X += Main.rand.Next(-15, 16) * 0.075f;
                            FireDirection.Y += Main.rand.Next(-15, 16) * 0.075f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, 0, Main.rand.Next(Main.projFrames[item.shoot]));
                        }
                        break;
                    case 1930:
                        {
                            byte Projs = (byte)(2 + Main.rand.Next(3));
                            for(byte i = 0; i < Projs; i++)
                            {
                                Vector2 ShotDirection = new Vector2(FireDirection.X, FireDirection.Y);
                                float Power = 0.025f * i;
                                ShotDirection.X += Main.rand.Next(-35, 36) * Power;
                                ShotDirection.Y += Main.rand.Next(-35, 36) * Power;
                                ShotDirection.Normalize();
                                ShotDirection *= ProjSpeed;
                                Vector2 FirePosition = new Vector2(FiringPosition.X, FiringPosition.Y);
                                FiringPosition.X += ShotDirection.X * (Projs - i) * 1.75f;
                                FiringPosition.Y += ShotDirection.Y * (Projs - i) * 1.75f;
                                Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, Main.rand.Next(0, 10 * (i + 1)));
                            }
                        }
                        break;
                    case 1931:
                        {
                            for(byte i = 0; i < 2; i++)
                            {
                                FiringPosition.X = position.X + width * 0.5f + Main.rand.Next(201) * direction + GetAimedPosition.X - Center.X;
                                FiringPosition.Y = MountedCenter.Y - 600;
                                FiringPosition.X = (FiringPosition.X + Center.X) * 0.5f + Main.rand.Next(-200, 201);
                                FiringPosition.Y -= 100 * i;
                                FireDirection.X = GetAimedPosition.X - FiringPosition.X;
                                FireDirection.Y = GetAimedPosition.Y - FiringPosition.Y;
                                if (gravDir == -1)
                                {
                                    FireDirection.Y += -AimDirection.Y * 2;
                                }
                                if(FireDirection.Y < 0) FireDirection *= -1;
                                if (FireDirection.Y < 20) FireDirection.Y = 20;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed;
                                FireDirection.X += Main.rand.Next(-40, 41) * 0.02f;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, 0, Main.rand.Next(5));
                            }
                        }
                        break;
                    case 2750:
                        {
                            byte Shots = 1;
                            for (byte i = 0; i < Shots; i++)
                            {
                                FiringPosition.X = position.X + width * 0.5f + Main.rand.Next(201) * direction + GetAimedPosition.X - Center.X;
                                FiringPosition.Y = MountedCenter.Y - 600;
                                FiringPosition.X = (FiringPosition.X + Center.X) * 0.5f + Main.rand.Next(-200, 201);
                                FiringPosition.Y -= 100 * i;
                                FireDirection.X = GetAimedPosition.X - FiringPosition.X + Main.rand.Next(-40, 41) * 0.03f;
                                FireDirection.Y = GetAimedPosition.Y - FiringPosition.Y;
                                if (gravDir == -1)
                                {
                                    FireDirection.Y += -AimDirection.Y * 2;
                                }
                                if (FireDirection.Y < 0)
                                    FireDirection.Y *= -1f;
                                if (FireDirection.Y < 20)
                                    FireDirection.Y = 20;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + Main.rand.Next(3), ProjDamage, Knockback, whoAmI, 0, 0.5f + Main.rand.NextFloat() * 0.3f);
                            }
                        }
                        break;
                    case 3570:
                        {
                            byte Shots = 3;
                            for (byte i = 0; i < Shots; i++)
                            {
                                FiringPosition.X = position.X + width * 0.5f + Main.rand.Next(201) * direction + GetAimedPosition.X - Center.X;
                                FiringPosition.Y = MountedCenter.Y - 600;
                                FiringPosition.X = (FiringPosition.X + Center.X) * 0.5f + Main.rand.Next(-200, 201);
                                FiringPosition.Y -= 100 * i;
                                FireDirection.X = GetAimedPosition.X - FiringPosition.X;
                                FireDirection.Y = GetAimedPosition.Y - FiringPosition.Y;
                                float ai2 = FireDirection.Y + FiringPosition.Y;
                                if (FireDirection.Y < 0)
                                    FireDirection.Y *= -1f;
                                if (FireDirection.Y < 20)
                                    FireDirection.Y = 20;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed * 0.5f;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, 0, ai2);
                            }
                        }
                        break;
                    case 5065:
                        {
                            Vector2 FarthestSpawnPosition = GetFarthestSpawnPositionOnLine(FiringPosition, FireDirection.X, FireDirection.Y);
                            Projectile.NewProjectile(projSource, FarthestSpawnPosition, Vector2.Zero, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 3065:
                        {
                            float NecessaryYPosition = GetAimedPosition.Y;
                            if(NecessaryYPosition > Center.Y - 200)
                                NecessaryYPosition = Center.Y - 200;
                            for (byte i = 0; i < 3; i++)
                            {
                                FiringPosition = Center + new Vector2(Main.rand.Next(0, 401) * direction, -600f);
                                FiringPosition.Y -= 100 * i;
                                FireDirection = GetAimedPosition - FiringPosition;
                                if(FireDirection.Y < 0) FireDirection.Y *= -1;
                                if (FireDirection.Y < 20f) FireDirection.Y = 20f;
                                FireDirection.Normalize();
                                FireDirection *= ProjSpeed;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage * 2, Knockback, whoAmI, 0f, NecessaryYPosition);
                            }
                        }
                        break;
                    case 2624:
                        {
                            const float ATenthofPi = (float)Math.PI / 10f;
                            FireDirection.Normalize();
                            FireDirection *= 40f;
                            int MaxShots = 5;
                            bool CanHit = Collision.CanHit(FiringPosition, 0, 0, FiringPosition + FireDirection, 0, 0);
                            for (int i = 0; i < MaxShots; i++)
                            {
                                float AimRadian = ATenthofPi * ((float)i - ((float)MaxShots - 1) / 2);
                                Vector2 SpinningPoint = FireDirection;
                                Vector2 ArrowFiringPosition = SpinningPoint.RotatedBy(AimRadian, default(Vector2));
                                if (!CanHit)
                                {
                                    ArrowFiringPosition -= FireDirection;
                                }
                                int p = Projectile.NewProjectile(projSource, FiringPosition + ArrowFiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].noDropItem = true;
                            }
                        }
                        break;
                    case 1929:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.03f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.03f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 1553:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.005f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.005f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 518:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.04f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.04f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 1265:
                        {
                            FireDirection.X += Main.rand.Next(-30, 31) * 0.03f;
                            FireDirection.Y += Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 4262: //Snake Charmers Flute
                        {

                        }
                        break;
                    case 4952:
                        {
                            Vector2 Velocity = Main.rand.NextVector2Circular(1, 1) + Main.rand.NextVector2CircularEdge(3, 3);
                            if (Velocity.Y > 0) Velocity.Y *= -1f;
                            float SomeFactor = (float)itemAnimation / itemAnimationMax * 0.66f + miscCounterNormalized;
                            Point point = FiringPosition.ToTileCoordinates();
                            Tile tile = Main.tile[point.X, point.Y];
                            if (tile != null && tile.HasTile && !tile.IsActuated && !Main.tileSolid[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
                            {
                                FiringPosition = MountedCenter;
                            }
                            Projectile.NewProjectile(projSource, FiringPosition, Velocity, ProjToShoot, ProjDamage, Knockback, whoAmI, -1f, SomeFactor % 1f);
                        }
                        break;
                    case 4953: //I tried guessing what each variable do
                        {
                            const float ATenthOfPi = (float)Math.PI / 10f;
                            const float TotalBlades = 5;
                            Vector2 ShotDirection = FireDirection + Vector2.Zero;
                            ShotDirection.Normalize();
                            ShotDirection *= 40f;
                            bool CanReach = Collision.CanHit(FiringPosition, 0, 0, FiringPosition + FireDirection, 0, 0);
                            int HalfAttackProgress = (itemAnimationMax - itemAnimation) / 2;
                            int FiringOrder = HalfAttackProgress;
                            if (direction == 1) FiringOrder = 4 - HalfAttackProgress;
                            float SomeFactor = (float)FiringOrder - (TotalBlades - 1f) / 2;
                            Vector2 SpinningPoint = FireDirection;
                            double Radians = ATenthOfPi * SomeFactor;
                            Vector2 ShotSpawnOffset = SpinningPoint.RotatedBy(Radians);
                            if (!CanReach)
                                ShotSpawnOffset -= FireDirection;
                            Vector2 ShotSpawnPosition = FiringPosition + ShotSpawnOffset;
                            Vector2 NormalizedDirectionTarget = ShotSpawnPosition.DirectionTo(GetAimedPosition).SafeNormalize(-Vector2.UnitY);
                            Vector2 NormalizedDirectionFromCaster = Center.DirectionTo(Center + FireDirection).SafeNormalize(-Vector2.UnitY);
                            float LerpValue = Utils.GetLerpValue(100, 40, GetAimedPosition.Distance(Center), true);
                            if (LerpValue > 0)
                                NormalizedDirectionTarget = Vector2.Lerp(NormalizedDirectionTarget, NormalizedDirectionFromCaster, LerpValue).SafeNormalize(Utils.SafeNormalize(FireDirection, -Vector2.UnitY));
                            Vector2 FinalSpeed = NormalizedDirectionTarget * ProjSpeed;
                            if (HalfAttackProgress == 2)
                            {
                                ProjToShoot = 932;
                                ProjDamage *= 2;
                            }
                            if(ProjToShoot == 932)
                            {
                                float ai3 = miscCounterNormalized * 12f % 1f;
                                FinalSpeed = FinalSpeed.SafeNormalize(Vector2.Zero) * (ProjSpeed * 2);
                                Projectile.NewProjectile(projSource, ShotSpawnPosition, NormalizedDirectionFromCaster, ProjToShoot, Damage, Knockback, whoAmI, 0, ai3);
                            }
                            else
                            {
                                int proj = Projectile.NewProjectile(projSource, ShotSpawnPosition, NormalizedDirectionFromCaster, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[proj].noDropItem = true;
                            }
                        }
                        break;
                    case 534:
                        {
                            int Bullets = Main.rand.Next(4, 6);
                            for (int i = 0; i < Bullets; i++)
                            {
                                Vector2 BulletSpeed = FireDirection + new Vector2(Main.rand.Next(-40, 41) * 0.05f, Main.rand.Next(-40, 41) * 0.05f);
                                Projectile.NewProjectile(projSource, FiringPosition, BulletSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 4703:
                        {
                            const float HalfOfPi = (float)Math.PI / 2f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            for (int i = 0; i < 7; i++)
                            {
                                Vector2 BulletSpeed = FireDirection;
                                float ScaleFactor = BulletSpeed.Length();
                                Vector2 SpinningPoint = BulletSpeed.SafeNormalize(Vector2.Zero);
                                double Radians = HalfOfPi * Main.rand.NextFloat();
                                BulletSpeed = BulletSpeed + SpinningPoint.RotatedBy(Radians) * Main.rand.NextFloatDirection() * 6f;
                                BulletSpeed = BulletSpeed.SafeNormalize(Vector2.Zero) * ScaleFactor;
                                BulletSpeed.X += Main.rand.Next(-40, 41) * 0.05f;
                                BulletSpeed.Y += Main.rand.Next(-40, 41) * 0.05f;
                                Projectile.NewProjectile(projSource, FiringPosition, BulletSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 4270:
                        {
                            Vector2 AimedPosition = GetAimedPosition + Main.rand.NextVector2Circular(8f, 8f);
                            LimitPointToPlayerReachableArea(ref AimedPosition);
                            Vector2 ThornPosition = Vector2.Zero;
                            {
                                Point point = AimedPosition.ToTileCoordinates();
                                Vector2 Center = this.Center;
                                int SamplesToTake = 3;
                                float SamplingWidth = 4f;
                                Collision.AimingLaserScan(Center, AimedPosition, SamplingWidth, SamplesToTake, out Vector2 VectorTowardsTarget, out float[] Samples);
                                float num = float.PositiveInfinity;
                                for (int i = 0; i < Samples.Length; i++)
                                {
                                    if (Samples[i] < num)
                                    {
                                        num = Samples[i];
                                    }
                                }
                                AimedPosition = Center + VectorTowardsTarget.SafeNormalize(Vector2.Zero) * num;
                                Rectangle rect1 = new Rectangle(point.X, point.Y, 1, 1);
                                rect1.Inflate(6, 16);
                                Rectangle rect2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
                                rect2.Inflate(-40, -40);
                                rect1 = Rectangle.Intersect(rect1, rect2);
                                List<Point> list = new List<Point>(), list2 = new List<Point>();
                                for (int x = rect1.Left; x <= rect1.Right; x++)
                                {
                                    for (int y = rect1.Top; y <= rect1.Bottom; y++)
                                    {
                                        if (!WorldGen.SolidTile(x, y)) continue;
                                        Vector2 TilePosition = new Vector2(x * 16 + 8, y * 16 + 8);
                                        if (Vector2.Distance(AimedPosition, TilePosition) <= 200)
                                        {
                                            bool FoundOpening = false;
                                            if (x > point.X && !WorldGen.SolidTile(x - 1, y))
                                                FoundOpening = true;
                                            else if (x < point.X && !WorldGen.SolidTile(x + 1, y))
                                                FoundOpening = true;
                                            else if (y > point.Y && !WorldGen.SolidTile(x, y - 1))
                                                FoundOpening = true;
                                            else if (y < point.Y && !WorldGen.SolidTile(x, y + 1))
                                                FoundOpening = true;
                                            if (FoundOpening)
                                            {
                                                list.Add(new Point(x, y));
                                            }
                                            else
                                            {
                                                list2.Add(new Point(x, y));
                                            }
                                        }
                                    }
                                }
                                if (list.Count == 0 && list2.Count == 0)
                                {
                                    list.Add((Center.ToTileCoordinates().ToVector2() + Main.rand.NextVector2Square(-2f, 2f)).ToPoint());
                                }
                                if (list.Count > 0)
                                {
                                    ThornPosition = list[Main.rand.Next(list.Count)].ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
                                }
                                else
                                {
                                    ThornPosition = list2[Main.rand.Next(list2.Count)].ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
                                }
                            }
                            Vector2 Speed = (AimedPosition - ThornPosition).SafeNormalize(-Vector2.UnitY) * 16;
                            Projectile.NewProjectile(projSource, ThornPosition, Speed, ProjToShoot, ProjDamage, Knockback, whoAmI, 0, Main.rand.NextFloat() * 0.5f + 0.5f);
                        }
                        break;
                    case 4715: //It doesn't work as intended.
                        {
                            Vector2 AimedPosition = GetAimedPosition;
                            List<NPC> ValidTargets = new List<NPC>();
                            {
                                Rectangle rect = Utils.CenteredRectangle(Center, new Vector2(1000, 800));
                                for (int i = 0; i < 200; i++)
                                {
                                    NPC npc = Main.npc[i];
                                    if (npc.CanBeChasedBy(this))
                                    {
                                        if (npc.Hitbox.Intersects(rect))
                                            ValidTargets.Add(npc);
                                    }
                                }
                            }
                            bool GotTargets = ValidTargets.Count > 0;
                            if (GotTargets)
                            {
                                NPC victim = ValidTargets[Main.rand.Next(ValidTargets.Count)];
                                AimedPosition = victim.Center + victim.velocity * 20f;
                            }
                            Vector2 Distance = AimedPosition - Center;
                            Vector2 ShotDirection = Main.rand.NextVector2CircularEdge(1, 1);
                            const int Shots = 1;
                            for (int i = 0; i < Shots; i++)
                            {
                                if (!GotTargets)
                                {
                                    AimedPosition += Main.rand.NextVector2Circular(24, 24);
                                    if (Distance.Length() > 700)
                                    {
                                        Distance *= 700f / Distance.Length();
                                        AimedPosition = Center + Distance;
                                    }
                                    float Lerp = Utils.GetLerpValue(0f, 6f, velocity.Length(), true) * 0.8f;
                                    ShotDirection *= 1f - Lerp;
                                    ShotDirection += velocity * Lerp;
                                    ShotDirection = ShotDirection.SafeNormalize(Vector2.UnitX);
                                }
                                const float BaseSpeedIGuess = 60f;
                                float FiringDirection = Main.rand.NextFloatDirection() * (float)Math.PI * (1f / BaseSpeedIGuess) * 0.5f;
                                const float HalfBaseSpeedIGuessToo = BaseSpeedIGuess / 2f;
                                float ScaleFactor = 12f + Main.rand.NextFloat() * 2f;
                                Vector2 ShotDirectionMultipliedByFactor = ShotDirection * ScaleFactor;
                                Vector2 DerivativeOfShotDirMultByFactor = ShotDirectionMultipliedByFactor;
                                Vector2 AnotherStackedVector = Vector2.Zero;
                                for (int j = 0; j < HalfBaseSpeedIGuessToo; j++)
                                {
                                    AnotherStackedVector += DerivativeOfShotDirMultByFactor;
                                    DerivativeOfShotDirMultByFactor = DerivativeOfShotDirMultByFactor.RotatedBy(FiringDirection);
                                }
                                Vector2 ShotPosition = AimedPosition - AnotherStackedVector;
                                float LerpValue = Utils.GetLerpValue(itemAnimationMax, 0, itemAnimation, true);
                                Projectile.NewProjectile(projSource, ShotPosition, ShotDirectionMultipliedByFactor, ProjToShoot, ProjDamage, Knockback, whoAmI, FiringDirection, LerpValue);
                            }
                        }
                        break;
                    case 4607: //Summon Invoking
                    case 5069:
                    case 5114:
                        {
                            int ProjID = ProjToShoot;
                            float kb = Knockback;
                            SpawnMinionOnCursor(projSource, whoAmI, ProjID, Damage, kb);
                        }
                        break;
                    case 2188:
                        {
                            int Stings = 4;
                            if (Main.rand.NextBool(3)) Stings++;
                            if (Main.rand.NextBool(4)) Stings++;
                            if (Main.rand.NextBool(5)) Stings++;
                            for (int i = 0; i < Stings; i++)
                            {
                                Vector2 ProjectileSpeed = FireDirection + Vector2.Zero;
                                float SpeedMod = 0.05f * ((byte)i);
                                ProjectileSpeed.X += Main.rand.Next(-35, 36) * SpeedMod;
                                ProjectileSpeed.Y += Main.rand.Next(-35, 36) * SpeedMod;
                                ProjectileSpeed.Normalize();
                                ProjectileSpeed *= ProjSpeed;
                                Projectile.NewProjectile(projSource, FiringPosition, ProjectileSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 1308:
                        {
                            int Stings = 3;
                            if (Main.rand.NextBool(3)) Stings++;
                            for (int i = 0; i < Stings; i++)
                            {
                                Vector2 ProjectileSpeed = FireDirection + Vector2.Zero;
                                float SpeedMod = 0.05f * ((byte)i);
                                ProjectileSpeed.X += Main.rand.Next(-35, 36) * SpeedMod;
                                ProjectileSpeed.Y += Main.rand.Next(-35, 36) * SpeedMod;
                                ProjectileSpeed.Normalize();
                                ProjectileSpeed *= ProjSpeed;
                                Projectile.NewProjectile(projSource, FiringPosition, ProjectileSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 1258:
                        {
                            FireDirection.X += Main.rand.Next(-40, 41) * 0.01f;
                            FireDirection.Y += Main.rand.Next(-40, 41) * 0.01f;
                            FiringPosition.X += Main.rand.Next(-40, 41) * 0.05f;
                            FiringPosition.Y += Main.rand.Next(-45, 36) * 0.05f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 964:
                        {
                            int Bullets = Main.rand.Next(3, 5);
                            for (int i = 0; i < Bullets; i++)
                            {
                                Vector2 NewBulletSpeed = FireDirection + new Vector2(Main.rand.Next(-35, 36) * 0.04f, Main.rand.Next(-35, 36) * 0.04f);
                                Projectile.NewProjectile(projSource, FiringPosition, NewBulletSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 3788:
                        {
                            float AFourthOfPi = (float)Math.PI / 4f;
                            for (int i = 0; i < 2; i++)
                            {
                                Vector2 NewBulletSpeed = FireDirection.SafeNormalize(Vector2.Zero);
                                float Radians = AFourthOfPi * (Main.rand.NextFloat() * .5f + .5f);
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection + NewBulletSpeed.RotatedBy(Radians, Vector2.Zero) * Main.rand.NextFloatDirection() * 2, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Radians = -AFourthOfPi * (Main.rand.NextFloat() * .5f + .5f);
                                Projectile.NewProjectile(projSource, FiringPosition, FireDirection + NewBulletSpeed.RotatedBy(Radians, Vector2.Zero) * Main.rand.NextFloatDirection() * 2, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection.SafeNormalize(Vector2.UnitX * direction) * (ProjSpeed * 1.3f), 661, ProjDamage * 2, Knockback, whoAmI);
                        }
                        break;
                    case 1569:
                        {
                            int Knives = 4;
                            if (Main.rand.NextBool(2))
                                Knives++;
                            if (Main.rand.NextBool(4))
                                Knives++;
                            if (Main.rand.NextBool(8))
                                Knives++;
                            if (Main.rand.NextBool(16))
                                Knives++;
                            for (int i = 0; i < Knives; i++)
                            {
                                float SpeedMod = 0.05f * i;
                                Vector2 NewSpeed = FireDirection + new Vector2(Main.rand.Next(-35, 36) * SpeedMod, Main.rand.Next(-35, 36) * SpeedMod);
                                NewSpeed.Normalize();
                                NewSpeed *= ProjSpeed;
                                Projectile.NewProjectile(projSource, FiringPosition, NewSpeed, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 1572:
                    case 2366:
                    case 3571:
                    case 3569:
                    case 5119:
                        {
                            bool Staves = item.type == ItemID.RainbowCrystalStaff || item.type == ItemID.MoonlordTurretStaff;
                            Point MousePosition = GetAimedPosition.ToTileCoordinates();
                            if (gravDir == -1)
                            {

                            }
                            if (!Staves)
                            {
                                while(MousePosition.Y < Main.maxTilesY - 10 && Main.tile[MousePosition.X, MousePosition.Y] != null && !WorldGen.SolidTile2(MousePosition.X, MousePosition.Y) && Main.tile[MousePosition.X - 1, MousePosition.Y] != null && !WorldGen.SolidTile2(MousePosition.X - 1, MousePosition.Y) && Main.tile[MousePosition.X + 1, MousePosition.Y] != null && !WorldGen.SolidTile2(MousePosition.X + 1, MousePosition.Y))
                                    MousePosition.Y++;
                                MousePosition.Y--;
                            }
                            Vector2 ResultPosition = new Vector2(GetAimedPosition.X, MousePosition.Y * 16 - 24);
                            int proj = Projectile.NewProjectile(projSource, ResultPosition, Vector2.UnitY * 15, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            Main.projectile[proj].originalDamage = Damage;
                            UpdateMaxTurrets();
                        }
                        break;
                    case 1244:
                    case 1256:
                        {
                            int p = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                            Main.projectile[p].ai[0] = GetAimedPosition.X;
                            Main.projectile[p].ai[1] = GetAimedPosition.X;
                        }
                        break;
                    case 1229:
                        {
                            int Bolts = 2;
                            if (Main.rand.NextBool(3)) Bolts++;
                            for (int i = 0; i < Bolts; i++)
                            {
                                Vector2 ShotDirection = FireDirection + Vector2.Zero;
                                for(int j = 0; j < i; j++)
                                {
                                    ShotDirection.X += Main.rand.Next(-35, 36) * 0.04f;
                                    ShotDirection.Y += Main.rand.Next(-35, 36) * 0.04f;
                                }
                                int Proj = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[Proj].noDropItem = true;
                            }
                        }
                        break;
                    case 1121:
                        {
                            int Bees = Main.rand.Next(1, 4);
                            if (Main.rand.NextBool(6))
                                Bees++;
                            if (Main.rand.NextBool(6))
                                Bees++;
                            if (strongBees && Main.rand.NextBool(3))
                                Bees++;
                            for(int i = 0; i < Bees; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-35, 36) * 0.02f, Main.rand.Next(-35, 36) * 0.02f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, beeType(), beeDamage(ProjDamage), beeKB(Knockback), whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 1155:
                        {
                            int Bees = Main.rand.Next(2, 5);
                            for(int i = 0; i < Bees; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-35, 36) * 0.02f, Main.rand.Next(-35, 36) * 0.02f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 1801:
                        {
                            int Bees = Main.rand.Next(2, 4);
                            for(int i = 0; i < Bees; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-35, 36) * 0.05f, Main.rand.Next(-35, 36) * 0.05f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 679:
                        {
                            for(int i = 0; i < 6; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-40, 41) * 0.05f, Main.rand.Next(-40, 41) * 0.05f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 1156:
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-40, 41) * 0.05f, Main.rand.Next(-40, 41) * 0.05f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 4682:
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-20, 21) * 0.01f, Main.rand.Next(-20, 21) * 0.01f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 2623:
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                Vector2 ShotDirection = FireDirection + new Vector2(Main.rand.Next(-40, 41) * 0.01f, Main.rand.Next(-40, 41) * 0.01f);
                                int p = Projectile.NewProjectile(projSource, FiringPosition, ShotDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Magic;
                            }
                        }
                        break;
                    case 3210:
                        {
                            FireDirection.X += Main.rand.Next(-30, 31) * 0.04f;
                            FireDirection.Y += Main.rand.Next(-30, 31) * 0.03f;
                            FireDirection.Normalize();
                            FireDirection *= Main.rand.Next(70, 91) * 0.1f;
                            FireDirection.X += Main.rand.Next(-30, 31) * 0.04f;
                            FireDirection.Y += Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI, Main.rand.Next(20));
                        }
                        break;
                    case 434:
                        {
                            if (itemAnimation < 5)
                            {
                                FireDirection.X += Main.rand.Next(-40, 41) * 0.01f;
                                FireDirection.Y += Main.rand.Next(-40, 41) * 0.01f;
                                FireDirection *= 1.1f;
                            }
                            else if (itemAnimation < 10)
                            {
                                FireDirection.X += Main.rand.Next(-20, 21) * 0.01f;
                                FireDirection.Y += Main.rand.Next(-20, 21) * 0.01f;
                                FireDirection *= 1.05f;
                            }
                            Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, ProjDamage, Knockback, whoAmI);
                        }
                        break;
                    case 1157:
                        {
                            ProjToShoot = Main.rand.Next(191, 195);
                            int proj = SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot, Damage, Knockback);
                            Main.projectile[proj].localAI[0] = 30f;
                        }
                        break;
                    case 1802:
                    case 2364:
                    case 2365:
                    case 2749:
                    case 3249:
                    case 3474:
                    case 4273:
                    case 4281:
                    case 1309:
                    case 4758:
                    case 4269:
                    case 5005:
                        {
                            int proj = SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot, Damage, Knockback);
                        }
                        break;
                    case 2535:
                        {
                            FireDirection = Vector2.Zero;
                            Vector2 SpinningPoint = FireDirection;
                            SpinningPoint = SpinningPoint.RotatedBy(Math.PI / 2);
                            SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot, Damage, Knockback, SpinningPoint, SpinningPoint);
                            SpinningPoint = Vector2.Zero;
                            SpinningPoint = SpinningPoint.RotatedBy(-Math.PI);
                            SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot + 1, Damage, Knockback, SpinningPoint, SpinningPoint);
                        }
                        break;
                    case 2551:
                        {
                            ProjToShoot += nextCycledSpiderMinionType;
                            SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot, Damage, Knockback);
                            nextCycledSpiderMinionType ++;
                            nextCycledSpiderMinionType %= 3;
                        }
                        break;
                    case 2584:
                        {
                            ProjToShoot += Main.rand.Next(3);
                            SpawnMinionOnCursor(projSource, whoAmI, ProjToShoot, Damage, Knockback);
                            nextCycledSpiderMinionType ++;
                            nextCycledSpiderMinionType %= 3;
                        }
                        break;
                    case 3531: //Stardust Dragon
                        {
                            int head = -1, tail = -1;
                            for (int i = 0; i < 1000; i++)
                            {
                                Projectile proj = Main.projectile[i];
                                if (proj.active && ProjMod.IsThisCompanionProjectile(i, this))
                                {
                                    if (head == -1 && proj.type == 625)
                                        head = i;
                                    if (tail == -1 && proj.type == 628)
                                        tail = i;
                                    if (head != -1 && tail != -1) break;
                                }
                            }
                            if (head == -1 && tail == -1)
                            {
                                FireDirection = Vector2.Zero;
                                FiringPosition = GetAimedPosition;
                                int first = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot, Damage, Knockback, whoAmI);
                                int second = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + 1, Damage, Knockback, whoAmI, first);
                                int third = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + 2, Damage, Knockback, whoAmI, second);
                                int fourth = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + 3, Damage, Knockback, whoAmI, third);
                                Main.projectile[second].localAI[1] = third;
                                Main.projectile[third].localAI[1] = fourth;
                                Main.projectile[first].originalDamage = Damage;
                                Main.projectile[second].originalDamage = Damage;
                                Main.projectile[third].originalDamage = Damage;
                                Main.projectile[fourth].originalDamage = Damage;
                            }
                            else if (head != -1 && tail != -1)
                            {
                                int tailattachedto = (int)Main.projectile[tail].ai[0];
                                int NewBodyPart = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + 1, Damage, Knockback, whoAmI, tailattachedto);
                                int NewBodyPart2 = Projectile.NewProjectile(projSource, FiringPosition, FireDirection, ProjToShoot + 2, Damage, Knockback, whoAmI, NewBodyPart);
                                Main.projectile[NewBodyPart].localAI[1] = NewBodyPart2;
                                Main.projectile[NewBodyPart].netUpdate = true;
                                Main.projectile[NewBodyPart].ai[1] = 1f;
                                Main.projectile[NewBodyPart2].localAI[1] = tail;
                                Main.projectile[NewBodyPart2].netUpdate = true;
                                Main.projectile[NewBodyPart2].ai[1] = 1f;
                                Main.projectile[tail].localAI[1] = NewBodyPart;
                                Main.projectile[tail].netUpdate = true;
                                Main.projectile[tail].ai[1] = 1f;
                                Main.projectile[NewBodyPart].originalDamage = Damage;
                                Main.projectile[NewBodyPart2].originalDamage = Damage;
                                Main.projectile[tail].originalDamage = Damage;
                            }
                        }
                        break;
                    /*case 3006:
                        {

                        }
                        break;*/
                    case 273:
                    {
                        float adjustedItemScale = GetAdjustedItemScale(item);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale);
                        Projectile.NewProjectile(projSource, MountedCenter, FireDirection, ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir * 0.1f, 30f, adjustedItemScale);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                    }
                    break;
                    case 3859:
                        {
                            ProjToShoot = 710;
                            FireDirection *= .8f;
                            Vector2 ExtraShotRange = FireDirection.SafeNormalize(-Vector2.UnitY);
                            float Orientation = (float)MathF.PI / 180f * -direction;
                            for (float f = -2.5f; f < 3f; f += 1f)
                            {
                                Vector2 ThisFireDirection = FireDirection + ExtraShotRange * f * .5f;
                                double radians = f * Orientation;
                                Projectile.NewProjectile(projSource, FiringPosition, ThisFireDirection.RotatedBy(radians, Vector2.Zero), ProjToShoot, Damage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 3870:
                        {
                            Vector2 ProjSpawnOffset = Vector2.Normalize(FireDirection) * 40f * item.scale;
                            if (Collision.CanHit(FiringPosition, 0, 0, ProjSpawnOffset + FiringPosition, 0, 0))
                            {
                                FiringPosition += ProjSpawnOffset;
                            }
                            Vector2 ShootSpeed = FireDirection * .8f;
                            Vector2 ShootDirection = ShootSpeed.SafeNormalize(-Vector2.UnitY);
                            float Orientation = (float)MathF.PI / 180f * -direction;
                            for (int i = 0; i <= 2; i++)
                            {
                                Vector2 ThisFireDirection = ShootSpeed + ShootDirection * i;
                                double radians = i * Orientation;
                                Projectile.NewProjectile(projSource, FiringPosition, ThisFireDirection.RotatedBy(radians, Vector2.Zero), ProjToShoot, Damage, Knockback, whoAmI);
                            }
                        }
                        break;
                    case 368:
                    {
                        float adjustedItemScale2 = GetAdjustedItemScale(item);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale2);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                    }
                    break;
                    case 1826:
                    {
                        float adjustedItemScale3 = GetAdjustedItemScale(item);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale3);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                    }
                    break;
                    case 675:
                    {
                        float adjustedItemScale4 = GetAdjustedItemScale(item);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), 972, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale4);
                        Projectile.NewProjectile(projSource, MountedCenter, FireDirection, ProjToShoot, Damage / 2, Knockback, whoAmI, (float)direction * gravDir, 32f, adjustedItemScale4);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                    }
                    break;
                    case 674:
                    {
                        float adjustedItemScale5 = GetAdjustedItemScale(item);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale5);
                        Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), 982, 0, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale5);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                    }
                    break;
                    case 757:
                        {
                            float adjustedItemScale6 = GetAdjustedItemScale(item);
                            Projectile.NewProjectile(projSource, MountedCenter, new Vector2((float)direction, 0f), 984, Damage, Knockback, whoAmI, (float)direction * gravDir, itemAnimationMax, adjustedItemScale6);
                            Projectile.NewProjectile(projSource, MountedCenter, FireDirection * 5f, ProjToShoot, Damage, Knockback, whoAmI, (float)direction * gravDir, 18f, adjustedItemScale6);
                            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, whoAmI);
                        }
                        break;
                }
            }
            else if ((item.useStyle == ItemUseStyleID.Shoot || item.useStyle == ItemUseStyleID.Rapier) && (IsLocalCompanion || IsPlayerCharacter))
            {
                itemRotation = 0;
            }
        }

        private void ItemCheck_MinionAltFeatureUse(Item item, bool Shoot)
        {
            if (item.shoot > ProjectileID.None && ProjectileID.Sets.MinionTargettingFeature[item.shoot] && altFunctionUse == 2 && Shoot && ItemTimeIsZero)
            {
                ApplyItemTime(item);
                if(IsLocalCompanion || IsPlayerCharacter)
                    MinionNPCTargetAim(false);
            }
        }

        private void ItemCheck_TurretAltFeatureUse(Item item, bool Shoot)
        {
            if (item.shoot <= ProjectileID.None || !ProjectileID.Sets.TurretFeature[item.shoot] || altFunctionUse != 2 || !Shoot || !ItemTimeIsZero)
                return;
            ApplyItemTime(item);
            if (!IsLocalCompanion && !IsPlayerCharacter) return;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && ProjMod.IsThisCompanionProjectile(i, this) && ProjectileID.Sets.TurretFeature[proj.type])
                    proj.Kill();
            }
        }

        private void ItemCheck_ApplyHoldStyle(float MountOffset, Item item, Rectangle HeldItemFrame, byte Arm)
        { //TODO
            //Petting script. Why is this here?
            if (!CanVisuallyHoldItem(item)) return;
            switch (item.holdStyle)
            {
                case 1:
                    if(pulley) break;
                    
                    break;
                case 3:
                    {
                        if(pulley) break;
                        AnimationTypes ItemUseType = (Base.CanCrouch && IsCrouching && Base.GetAnimation(AnimationTypes.CrouchingSwingFrames).HasFrames) ? AnimationTypes.CrouchingSwingFrames : AnimationTypes.ItemUseFrames;
                        short Frame = Base.GetAnimation(ItemUseType).GetFrameFromPercentage(0.7f);
                        Vector2 HeldPosition = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Arm) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, Frame, Arm, false, false, false, false, false);
                        HeldPosition.X -= HeldItemFrame.Width * 0.5f + 12 * direction;
                        HeldPosition.Y -= HeldItemFrame.Height * 0.5f;
                        itemRotation = 0;
                        itemLocation = HeldPosition;
                    }
                    break;
            }
        }

        private void ItemCheck_TerraGuardiansApplyUseStyle(float MountOffset, Item item, Rectangle HeldItemFrame, byte Hand)
        {
            if(Main.dedServ) return;
            AnimationTypes ItemUseType = (Base.CanCrouch && IsCrouching && Base.GetAnimation(AnimationTypes.CrouchingSwingFrames).HasFrames) ? AnimationTypes.CrouchingSwingFrames : AnimationTypes.ItemUseFrames;
            switch(item.useStyle)
            {
                case 1:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        if(Items.GuardianItemPrefab.GetItemType(item) == Items.GuardianItemPrefab.ItemType.Heavy)
                        {
                            AttackPercentage = 1f - AttackPercentage * AttackPercentage;
                            itemRotation = MathHelper.ToRadians(-158 + 292.75f * AttackPercentage) * direction;
                            Animation anim = Base.GetAnimation(AnimationTypes.HeavySwingFrames);
                            short Frame = anim.GetFrameFromPercentage(AttackPercentage);
                            float Scale = GetAdjustedItemScale(item);
                            Vector2 ItemOrigin = (item.ModItem as Items.GuardianItemPrefab).ItemOrigin;
                            itemLocation = GetBetweenAnimationPosition(AnimationPositions.HandPosition, Frame, BottomCentered: true) + GetBetweenAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, false, false); //origin issues...
                            Dust.NewDust(itemLocation, 1, 1, DustID.Blood);
                            float rotation = itemRotation * direction; // + 1.570796f * direction;
                            Vector2 ItemOffset = new Vector2(
                                (float)((HeldItemFrame.Height - ItemOrigin.Y) * Math.Sin(rotation) + (HeldItemFrame.Width - ItemOrigin.X) * Math.Cos(rotation)),
                                (float)((HeldItemFrame.Height - ItemOrigin.Y) * Math.Cos(rotation) + (HeldItemFrame.Width - ItemOrigin.X) * Math.Sin(rotation))
                            );
                            if(direction < 0)
                                ItemOffset.X = width * 0.5f - ItemOffset.X; //(HeldItemFrame.Width * 0.5f)
                            itemLocation.X -= ItemOffset.X * direction * Scale;
                            itemLocation.Y -= ItemOffset.Y * gravDir * Scale;
                        }
                        else
                        {
                            Animation anim = Base.GetAnimation(ItemUseType);
                            short Frame = anim.GetFrameFromPercentage(1f - AttackPercentage);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false) - itemRotation.ToRotationVector2() * 2;
                            itemRotation = (AttackPercentage - 0.5f) * -direction * 3.5f - direction * 0.3f;
                        }
                    }
                    break;
                /*case 2: //Fix this
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        Animation anim = Base.GetAnimation(ItemUseType);
                        float FramePercentage = 1;
                        if (AttackPercentage < 0.1f)
                        {
                            FramePercentage = AttackPercentage * 10;
                        }
                        else if(AttackPercentage > 0.9f)
                        {
                            FramePercentage = (0.1f - (AttackPercentage - 0.9f)) * 10;
                        }
                        short Frame = anim.GetFrameFromPercentage(0.4f - FramePercentage * 0.333f);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                        //itemRotation = 0;
                        itemRotation = MathHelper.ToRadians(15 - 150f * FramePercentage) * direction;//(1f - (AttackPercentage * 6)) * direction * 2 - 1.4f * direction;
                        itemLocation.X -= HeldItemFrame.Width * 0.5f * direction;
                        itemLocation.Y -= HeldItemFrame.Height * 0.5f * gravDir;
                    }
                    break;*/
                case 3:
                    {
                        Animation anim = Base.GetAnimation(ItemUseType);
                        short Frame = anim.GetFrameFromPercentage(0.7f);
                        if(itemAnimation > itemAnimationMax * 0.666f)
                        {
                            itemLocation.X = itemLocation.Y = -1000f;
                            itemRotation = -1.3f * direction;
                        }
                        else
                        {
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                            float MovementDirection = (float)itemAnimation / itemAnimationMax * HeldItemFrame.Width * direction * GetAdjustedItemScale(item) * 1.2f - 10f * direction;
                            if (MovementDirection * direction > 4f) MovementDirection = 8 * direction;
                            itemLocation.X -= MovementDirection;
                            itemRotation = 0.8f * direction;
                            if(item.type == ItemID.Umbrella || item.type == ItemID.TragicUmbrella) itemLocation.X -= 6 * direction;
                        }
                    }
                    break;
                case 4:
                    {
                        Animation anim = Base.GetAnimation(ItemUseType);
                        short Frame = anim.GetFrameFromPercentage(0.3f);
                        float OffsetX = 0, OffsetY = 0;
                        if (item.type == ItemID.CelestialSigil) OffsetX = 10;
                        else if (item.type == ItemID.AbigailsFlower)
                        {
                            OffsetX = 10;
                            OffsetY = -1;
                        }
                        else if (item.type == ItemID.DeerThing) OffsetX = 10;
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                        itemLocation.X += OffsetX - HeldItemFrame.Width * 0.5f * direction;
                        itemLocation.Y += OffsetY + HeldItemFrame.Height * 0.5f * gravDir;
                    }
                    break;
                case 5:
                    {
                        Animation anim = Base.GetAnimation(ItemUseType);
                        if(item.type == ItemID.SpiritFlame)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                        }
                        else if (item.type == ItemID.MysticCoilSnake)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                            if (Main.rand.NextBool(20))
                            {
                                //Snake flute effect
                            }
                        }
                        else if (Item.staff[item.type])
                        {
                            float ScaleFactor = 6f;
                            if (item.type == ItemID.NebulaArcanum) ScaleFactor = 14f;
                            float Percentage = (itemRotation * direction + 1) * 0.5f;
                            //short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                            short Frame = anim.GetFrameFromPercentage(Percentage);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) +
                             GetAnimationPosition(AnimationPositions.ArmPositionOffset, Frame, Hand, false, false, false, false, false) +
                             (itemRotation.ToRotationVector2() * ScaleFactor/* * direction*/).Floor();
                        }
                        else
                        {
                            float Percentage = Math.Clamp(((float)(System.Math.PI * 0.5f) + itemRotation * direction) * (float)(1f / System.Math.PI), 0, 0.999f); //Still need to fix positioning issues
                            short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrame(Frame), Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false) - new Vector2(direction * 2, HeldItemFrame.Height * 0.5f) - itemRotation.ToRotationVector2() * 12f * Scale * direction; //Item is positioned incorrectly. Why?
                        }
                        //Item 5065 effect script.
                    }
                    break;
                case 7: //Unused, it seems
                    {
                        //float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        //itemRotation = AttackPercentage * direction * 2 + -1.4f * direction;
                    }
                    break;
                case 8: //Golf. Todo
                    {

                    }
                    break;
                case 2:
                case 6:
                case 9:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        float t = Utils.GetLerpValue(0.3f, 1, 1f - AttackPercentage, true);
                        itemRotation = t * -direction * 2 + 0.7f + direction;
                        Animation anim = Base.GetAnimation(ItemUseType);
                        short Frame = anim.GetFrameFromPercentage(System.Math.Clamp(AttackPercentage, 0f, 0.6f));
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                        if(HeldItem.useStyle == ItemUseStyleID.EatFood)
                        {
                            itemLocation.X -= HeldItemFrame.Width * direction;
                            itemLocation.Y += HeldItemFrame.Height * gravDir;
                        }
                    }
                    break;
                case 11:
                    {
                        Animation anim = Base.GetAnimation(ItemUseType);
                        short Frame = anim.GetFrameFromPercentage(1f);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                    }
                    break;
                case 12: //Guitar TODO
                    {

                    }
                    break;
                case 13:
                    {
                        Animation anim = Base.GetAnimation(ItemUseType);
                        float Percentage = (itemRotation * direction + 1) * 0.5f;
                        short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, Hand, false, false, false, false, false);
                    }
                    break;
                case 14: //Lamp
                    {

                    }
                    break;
            }
            if (gravDir == -1) itemRotation = -itemRotation;
        }

        private void ItemCheck_StartActualUse(Item item)
        {
            bool IsGravediggerShovel = item.type == ItemID.GravediggerShovel;
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0 || IsGravediggerShovel)
                toolTime = 1;
            if (grappling[0] > -1)
            {
                pulley = false;
                pulleyDir = 1;
                if(controlRight) direction = 1;
                else if(controlLeft) direction = -1;
            }
            channel = item.channel;
            attackCD = 0;
            ResetMeleeHitCooldowns();
            ApplyItemAnimation(item);
            bool SkipInitialSound = ItemID.Sets.SkipsInitialUseSound[item.type];
            if (item.UseSound.HasValue && !SkipInitialSound)
                SoundEngine.PlaySound(item.UseSound, Center);
        }

        private void FreeUpPetsAndMinions(Item item)
        {
            if (ProjectileID.Sets.MinionSacrificable[item.shoot])
            {
                List<int> list = new List<int>();
                float MinionSlotsSum = 0;
                for(int i = 0; i < 1000; i++)
                {
                    if(!Main.projectile[i].active || !ProjMod.IsThisCompanionProjectile(i, this) || !Main.projectile[i].minion)
                        continue;
                    int n;
                    for(n = 0; n < list.Count; n++)
                    {
                        if(Main.projectile[list[n]].minionSlots > Main.projectile[i].minionSlots)
                        {
                            list.Insert(n, i);
                            break;
                        }
                    }
                    if(n == list.Count) list.Add(i);
                    MinionSlotsSum += Main.projectile[i].minionSlots;
                }
                float SlotsReq = ItemID.Sets.StaffMinionSlotsRequired[item.type];
                float AltMinionSlots = 0;
                int SummonID = 388;
                int VariantPosition = -1;
                for(int i = 0; i < list.Count; i++)
                {
                    int type = Main.projectile[list[i]].type;
                    if(type == 626) list.RemoveAt(i--);
                    if(type == 627)
                    {
                        if(Main.projectile[(int)Main.projectile[list[i]].localAI[1]].type == 628)
                            VariantPosition = i;
                        list.RemoveAt(i--);
                    }
                }
                if(VariantPosition != -1)
                {
                    list.Add(VariantPosition);
                    list.Add(Projectile.GetByUUID(Main.projectile[VariantPosition].owner, Main.projectile[VariantPosition].ai[0]));
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if(!(MinionSlotsSum - AltMinionSlots > maxMinions - SlotsReq))
                        break;
                    int type = Main.projectile[list[i]].type;
                    if(type == SummonID || type == 625 || type == 628 || type == 623) continue;
                    if(type == 388 && SummonID == 387) SummonID = 388;
                    if(type == 387 && SummonID == 388) SummonID = 388;
                    AltMinionSlots += Main.projectile[list[i]].minionSlots;
                    if(type == 626 || type == 627)
                    {
                        Projectile proj = Main.projectile[list[i]];
                        int byUUID = Projectile.GetByUUID(proj.owner, proj.ai[0]);
                        if(Main.projectile.IndexInRange(byUUID))
                        {
                            Projectile proj2 = Main.projectile[byUUID];
                            if(proj2.type != 625)
                                proj2.localAI[1] = proj.localAI[1];
                            proj2 = Main.projectile[(int)proj.localAI[1]];
                            proj2.ai[0] = proj.ai[0];
                            proj2.ai[1] = 1;
                            proj2.netUpdate = true;
                        }
                    }
                    Main.projectile[list[i]].Kill();
                }
                list.Clear();
                /*if(IsPlayerCharacter && MinionSlotsSum + SlotsReq >= 9)
                {
                    //9 or more minions achievement
                }*/
                return;
            }
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this))
                {
                    if(Main.projectile[i].type == item.shoot)
                        Main.projectile[i].Kill();
                    if(item.shoot == ProjectileID.BlueFairy && (Main.projectile[i].type == 86 || Main.projectile[i].type == 87))
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
        }

        private void ItemCheck_ApplyPetBuffs(Item item)
        {
            if(!(IsLocalCompanion || IsPlayerCharacter))
                return;
            bool GetPet = false;
            switch(item.type)
            {
                case 603:
                    if(Main.runningCollectorsEdition)
                        GetPet = true;
                    break;
                case 669:
                case 115:
                case 3060:
                case 3628:
                case 3062:
                case 3577:
                case 753:
                case 994:
                case 1169:
                case 1170:
                case 1171:
                case 1172:
                case 1180:
                case 1181:
                case 1182:
                case 1183:
                case 1242:
                case 1157:
                case 1309:
                case 1311:
                case 1837:
                case 1312:
                case 1798:
                case 1799:
                case 1802:
                case 1810:
                case 1927:
                case 1959:
                case 2364:
                case 2365:
                case 3043:
                case 2420:
                case 2535:
                case 2551:
                case 2584:
                case 2587:
                case 2621:
                case 2749:
                case 3249:
                case 3474:
                case 3531:
                case 4269:
                case 4273:
                case 4281:
                case 4607:
                case 4758:
                case 5005:
                case 5069:
                case 5114:
                case 3855:
                case 3856:
                case 3857:
                case 4365:
                case 4366:
                case 4425:
                case 4550:
                case 4551:
                case 4603:
                case 4604:
                case 4605:
                case 4701:
                case 4735:
                case 4736:
                case 4737:
                case 4777:
                case 4960:
                case 5088:
                case 5089:
                case 5090:
                case 5091:
                case 5098:
                    GetPet = true;
                    break;
                case 425:
                    {
                        int PickedPet = Main.rand.Next(3);
                        switch(PickedPet)
                        {
                            default:
                                PickedPet = 27;
                                break;
                            case 1:
                                PickedPet = 101;
                                break;
                            case 2:
                                PickedPet = 102;
                                break;
                        }
                        for(int i = 0; i < MaxBuffs; i++)
                        {
                            if (buffType[i] == 27 || buffType[i] == 101 || buffType[i] == 102)
                                DelBuff(i--);
                        }
                        AddBuff(PickedPet, 3600);
                    }
                    return;
            }
            if(item.type >= ItemID.KingSlimePetItem && item.type <= ItemID.DD2BetsyPetItem)
                GetPet = true;
            if (GetPet) AddBuff(item.buffType, 3600);
        }

        private void ApplyPotionDelay(Item item)
        {
            if (item.type == ItemID.RestorationPotion) AddBuff(21, potionDelay = restorationDelayTime);
            else if (item.type == ItemID.Mushroom) AddBuff(21, potionDelay = mushroomDelayTime);
            else AddBuff(21, potionDelay = potionDelayTime);
        }

        private bool ItemCheck_CheckCanUse(Item item)
        {
            if (item.IsAir || !CombinedHooks.CanUseItem(this, item)) return false;
            bool Can = true;
            int MouseX = (int)((GetAimedPosition.X) * DivisionBy16);
            int MouseY = gravDir == -1 ? 
                (int)((Main.screenHeight - GetAimedPosition.Y)) : 
                (int)((GetAimedPosition.Y) * DivisionBy16);
            if (item.type == ItemID.DemonHeart && (extraAccessory || !Main.expertMode))
                Can = false;
            if (pulley)
            {
                if (item.fishingPole > 0)
                    Can = false;
                if (ItemID.Sets.IsAKite[item.type])
                    Can = false;
            }
            if (item.type == ItemID.WireKite && (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode.Red | WiresUI.Settings.MultiToolMode.Green | WiresUI.Settings.MultiToolMode.Blue | WiresUI.Settings.MultiToolMode.Yellow | WiresUI.Settings.MultiToolMode.Yellow)) == 0)
                Can = false;
            if ((item.type == ItemID.WireKite || item.type == ItemID.MulticolorWrench) && wireOperationsCooldown > 0)
                Can = false;
            if (!CheckDD2CrystalPaymentLock(item))
                Can = false;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && !downedDD2EventAnyDifficulty && !DD2Event.Ongoing)
                Can = false;
            int PushYUp;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && (IsPlayerCharacter || IsLocalCompanion))
            {
                FindSentryRestingSpot(item.shoot, out int WorldX, out int WorldY, out PushYUp);
                if (DD2Event.Ongoing)
                {
                    if (WouldSpotOverlapWithSentry(WorldX, WorldY, item.shoot == ProjectileID.DD2LightningAuraT1 || item.shoot == ProjectileID.DD2LightningAuraT2 || item.shoot == ProjectileID.DD2LightningAuraT3))
                        Can = false;
                }
                if (Can)
                {
                    WorldX = (int)(WorldX * DivisionBy16);
                    WorldY = (int)(WorldY * DivisionBy16);
                    WorldY--;
                    if (item.shoot == ProjectileID.DD2LightningAuraT1 || item.shoot == ProjectileID.DD2LightningAuraT2 || item.shoot == ProjectileID.DD2LightningAuraT3)
                    {
                        if (Collision.SolidTiles(WorldX, WorldX, WorldY - 2, WorldY)) Can = false;
                    }
                    else if (WorldGen.SolidTile(WorldX,WorldY)) Can = false;
                }
            }
            if (wet && (item.shoot == ProjectileID.Flames || item.shoot == ProjectileID.BallofFire || item.shoot == ProjectileID.Flamelash))
                Can = false;
            if (item.makeNPC > 0 && !NPC.CanReleaseNPCs(whoAmI))
                Can = false;
            if((IsLocalCompanion || IsPlayerCharacter) && item.type == ItemID.Carrot && !Main.runningCollectorsEdition)
                Can = false;
            if (item.type == ItemID.Paintbrush || item.type == ItemID.PaintRoller)
            {
                bool HasPaint = false;
                for(int i = 0; i < 58; i++)
                {
                    if (inventory[i].paint > 0)
                    {
                        HasPaint = true;
                        break;
                    }
                }
                if (!HasPaint) Can = false;
            }
            if (noItems)
            {
                Can = false;
            }
            if(item.tileWand > 0)
            {
                Can = false;
                for (int i = 0; i < 58; i++)
                {
                    if (item.tileWand == inventory[i].type && inventory[i].stack > 0)
                    {
                        Can = true;
                        break;
                    }
                }
            }
            if (item.shoot == ProjectileID.EnchantedBoomerang || item.shoot == ProjectileID.Flamarang || item.shoot == ProjectileID.ThornChakram || item.shoot == ProjectileID.WoodenBoomerang || item.shoot == ProjectileID.IceBoomerang || item.shoot == ProjectileID.BloodyMachete || item.shoot == ProjectileID.FruitcakeChakram || item.shoot == ProjectileID.Anchor || item.shoot == ProjectileID.FlyingKnife || item.shoot == ProjectileID.Shroomerang || item.shoot == ProjectileID.CombatWrench || item.shoot == ProjectileID.BouncingShield)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                    {
                        Can = false;
                        break;
                    }
                }
            }
            if (item.shoot == ProjectileID.LightDisc || item.shoot == ProjectileID.Bananarang)
            {
                int Discs = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                    {
                        Discs++;
                    }
                }
                if(Discs >= item.stack) Can = false;
            }
            if (item.shoot == ProjectileID.Hook || item.shoot == ProjectileID.IvyWhip || (item.shoot >= ProjectileID.GemHookAmethyst && item.shoot <= ProjectileID.GemHookDiamond) || item.shoot == ProjectileID.BatHook || item.shoot == ProjectileID.CandyCaneHook || item.shoot == ProjectileID.FishHook)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot && Main.projectile[i].ai[0] != 2f)
                    {
                        Can = false;
                        break;
                    }
                }
            }
            if (item.shoot == ProjectileID.ChristmasHook)
            {
                int Hooks = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot && Main.projectile[i].ai[0] != 2)
                    {
                        Hooks++;
                    }
                }
                if(Hooks >= 3) Can = false;
            }
            if (item.potion && potionDelay > 0) Can = false;
            if (item.mana > 0)
            {
                if(silence)
                    Can = false;
                else if(Can) Can = ItemCheck_PayMana(item, Can);
            }
            if ((item.type == ItemID.SuspiciousLookingEye || item.type == ItemID.MechanicalEye || item.type == ItemID.MechanicalWorm || item.type == ItemID.MechanicalSkull) && Main.dayTime)
            {
                Can = false;
            }
            if (item.type == ItemID.WormFood && !ZoneCorrupt)
                Can = false;
            if (item.type == ItemID.Abeemination && !ZoneJungle)
                Can = false;
            if (item.type == ItemID.DeerThing && !ZoneSnow)
                Can = false;
            if ((item.type == ItemID.PumpkinMoonMedallion || item.type == ItemID.NaughtyPresent) && (Main.dayTime || Main.pumpkinMoon || Main.snowMoon || DD2Event.Ongoing))
                Can = false;
            if (item.type == ItemID.SolarTablet && (!Main.dayTime || Main.eclipse || !Main.hardMode))
                Can = false;
            if (item.type == ItemID.BloodMoonStarter && (Main.dayTime || Main.bloodMoon))
                Can = false;
            if (item.type == ItemID.CelestialSigil && (!NPC.downedGolemBoss || !Main.hardMode || NPC.AnyDanger() || NPC.AnyoneNearCultists()))
                Can = false;
            if (!SummonItemCheck(item)) Can = false;
            if (item.shoot == ProjectileID.DirtBall && Can && (IsPlayerCharacter || IsLocalCompanion) && !ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY])) Can = false;
            if (item.fishingPole > 0) Can = ItemCheck_CheckFishingBobbers(Can);
            if (ItemID.Sets.HasAProjectileThatHasAUsabilityCheck[item.type]) Can = ItemCheck_CheckUsabilityOfProjectiles(Can);
            if (item.shoot == ProjectileID.DirtBall && Can && (IsLocalCompanion || IsPlayerCharacter))
            {
                if (ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY]))
                {
                    WorldGen.KillTile(MouseX, MouseY, false, false, true);
                    if (!Main.tile[MouseX, MouseY].HasTile)
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 4, MouseX, MouseY);
                    }
                    else
                        Can = false;
                }
                else
                {
                    Can = false;
                }
            }
            if (Can) Can = HasAmmo(item);
            return Can;
        }

        private bool ItemCheck_CheckUsabilityOfProjectiles(bool CanUse)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && ProjMod.IsThisCompanionProjectile(i, this))
                    proj.CheckUsability(this, ref CanUse);
            }
            return CanUse;
        }

        private bool ItemCheck_CheckFishingBobbers(bool CanUse)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active || !ProjMod.IsThisCompanionProjectile(proj, this) || !proj.bobber)
                    continue;
                CanUse = false;
                if (!(IsLocalCompanion || IsPlayerCharacter) || proj.ai[0] != 0)
                    continue;
                proj.ai[0] = 1f;
                float VelocityY = -10f;
                if(proj.wet && proj.velocity.Y > VelocityY)
                    proj.velocity.Y = VelocityY;
                proj.netUpdate2 = true;
                if (proj.ai[1] < 0 && proj.localAI[1] != 0)
                {
                    ItemCheck_CheckFishingBobber_PickAndConsumeBait(proj, out bool PullBobber, out int BaitUsed);
                    if (PullBobber)
                    {
                        ItemCheck_CheckFishingBobber_PullBobber(proj, BaitUsed);
                    }
                }
            }
            return CanUse;
        }

        private void ItemCheck_CheckFishingBobber_PullBobber(Projectile bobber, int BaitTypeUsed)
        {
            if (BaitTypeUsed == 2673)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) NPC.SpawnOnPlayer(whoAmI, 370);
                //else NetMessage.SendData(61, -1, -1, null, whoAmI, 370); //TODO - Need to figure out how to take companion position
                bobber.ai[0] = 2f;
            }
            else if (bobber.localAI[1] < 0)
            {
                Point pos = new Point((int)bobber.position.X, (int)bobber.position.Y);
                int NpcID = (int)(- bobber.localAI[1]);
                if(NpcID == 618) pos.Y += 64;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.FishOutNPC, -1, -1, null, pos.X / 16, pos.Y / 16, NpcID);
                }
                else
                {
                    NPC.NewNPC(new EntitySource_FishedOut(this), pos.X, pos.Y, NpcID);
                    bobber.ai[0] = 2;
                }
            }
            else if (Main.rand.NextBool(7)&& !accFishingLine)
            {
                bobber.ai[0] = 2f;
            }
            else
            {
                bobber.ai[1] = bobber.localAI[1];
            }
            bobber.netUpdate = true;
        }

        private void ItemCheck_CheckFishingBobber_PickAndConsumeBait(Projectile bobber, out bool PullTheBobber, out int BaitTypeUsed)
        {
            PullTheBobber = false;
            BaitTypeUsed = 0;
            int BaitPos = -1;
            for (int i = 54; i < 58; i++)
            {
                if (inventory[i].stack > 0 && inventory[i].bait > 0)
                {
                    BaitPos = i;
                    break;
                }
            }
            if(BaitPos == -1)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (inventory[i].stack > 0 && inventory[i].bait > 0)
                    {
                        BaitPos = i;
                        break;
                    }
                }
            }
            if (BaitPos == -1) return;
            Item bait = inventory[BaitPos];
            bool consume = false;
            float consumptionchance = 1f + bait.bait * (1f / 6);

            if(consumptionchance < 1) consumptionchance = 1;
            if(accTackleBox) consumptionchance += 1;
            if(Main.rand.NextFloat() * consumptionchance < 1) consume = true;
            if (bobber.localAI[1] == -1f) consume = true;
            if (bobber.localAI[1] > 0)
            {
                Item item = new Item();
                item.SetDefaults((int)bobber.localAI[1]);
                if (item.rare < ItemRarityID.White) consume = false;
            }
            BaitTypeUsed = ((byte)itemAnimation);
            if (BaitTypeUsed == 2673) consume = true;
            if (CombinedHooks.CanConsumeBait(this, bait) ?? consume)
            {
                if (bait.type == ItemID.LadyBug || bait.type == ItemID.GoldLadyBug)
                    NPC.LadyBugKilled(Center, bait.type == ItemID.GoldLadyBug);
                bait.stack--;
                if (bait.stack <= 0)
                {
                    bait.SetDefaults();
                }
            }
            PullTheBobber = true;
        }

        private static bool ItemCheck_IsValidDirtRodTarget(Tile tile)
        {
            if (tile.HasTile)
            {
                int type = tile.TileType;
                if(type != 0 && type != 2 && type != 23 && type != 109 && type != 199 && type != 477)
                {
                    return type == 492;
                }
                return true;
            }
                return false;
        }

        private bool ItemCheck_PayMana(Item item, bool CanUse)
        {
            bool IsAltUse = altFunctionUse == 2;
            bool NoPay = false;
            int Cost = (int)(item.mana * manaCost);
            if (item.type == ItemID.LaserMachinegun) NoPay = true;
            if (item.type == ItemID.BookStaff && IsAltUse)
                Cost = (int)(item.mana * 2 * manaCost);
            if (item.shoot > ProjectileID.None && IsAltUse)
            {
                if (ProjectileID.Sets.TurretFeature[item.shoot] || ProjectileID.Sets.MinionTargettingFeature[item.shoot])
                    NoPay = true;
            }
            if (item.type == ItemID.SoulDrain) NoPay = true;
            if (item.type != ItemID.MedusaHead && !CheckMana(item, -1, !NoPay))
                return false;
            return CanUse;
        }
        
        private bool CheckDD2CrystalPaymentLock(Item item)
        {
            if (!DD2Event.Ongoing) return true;
            return CountItem(3822) >= GetRequiredDD2CrystalsToUse(item);
        }

        private int GetRequiredDD2CrystalsToUse(Item item)
        {
            switch (item.type)
            {
                case 3818:
                case 3819:
                case 3820:
                case 3824:
                case 3825:
                case 3826:
                case 3832:
                case 3833:
                case 3834:
                case 3829:
                case 3830:
                case 3831:
                    return 10;
                default:
                    return 0;
            }
        }

        private void ApplyReuseDelay()
        {
            itemAnimation = itemTime = reuseDelay;
            reuseDelay = 0;
        }

        private void ItemCheck_HackHoldStyles(Item item)
        {
            if(item.fishingPole > 0)
            {
                item.holdStyle = 0;
                if (ItemTimeIsZero && itemAnimation == 0)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].bobber)
                        {
                            item.holdStyle = 1;
                            break;
                        }
                    }
                }
            }
            if (!ItemID.Sets.IsAKite[item.type]) return;
            item.holdStyle = 0;
            if (!ItemTimeIsZero || itemAnimation != 0)
                return;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                {
                    item.holdStyle = 1;
                    ChangeDir(!(Main.projectile[i].Center.X - Center.X < 0) ? 1 : -1);
                    break;
                }
            }
        }

        public override void CheckDrowning()
        {
            bool IsDrowning = Collision.DrownCollision(position, width, height, gravDir);
            if(armor[0].type == ItemID.FishBowl || armor[0].type == ItemID.GoldGoldfishBowl)
                IsDrowning = true;
            if (gills)
                IsDrowning = false;
            else if (inventory[selectedItem].type == ItemID.BreathingReed && itemAnimation == 0)
            {
                try
                {
                    int xc = (int)((position.X + width * 0.5f + 6 * direction) * DivisionBy16);
                    int yc = 0;
                    if (gravDir == -1) yc = height;
                    yc = (int)((position.Y + yc - (height - 12) * gravDir) / 16);
                    if (xc > Main.leftWorld && xc < Main.rightWorld && yc > Main.topWorld && yc < Main.bottomWorld && 
                        Main.tile[xc, yc] != null && Main.tile[xc, yc].LiquidAmount < 128)
                    {
                        if (!Main.tile[xc, yc].HasTile || !Main.tileSolid[Main.tile[xc, yc].TileType] || Main.tileSolidTop[Main.tile[xc, yc].TileType])
                            IsDrowning = false;
                    }
                }
                catch
                {

                }
            }
            if (IsLocalCompanion)
            {
                if (accMerman)
                {
                    if(IsDrowning)
                        merman = true;
                    IsDrowning = false;
                }
                if (IsDrowning)
                {
                    breathCD++;
                    if (breathCD >= breathCDMax)
                    {
                        breathCD = 0;
                        breath--;
                        if (breath == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Drown);
                        }
                        if (breath <= 0)
                        {
                            lifeRegenTime = 0;
                            breath = 0;
                            statLife -= (int)(MathF.Max(2, 2 * GetHealthScale));
                            if (statLife <= 0)
                            {
                                statLife = 0;
                                KillMe(PlayerDeathReason.ByOther(1), 10, 0);
                            }
                        }
                    }
                }
                else if (breath < breathMax)
                {
                    breath += 3;
                    if (breath > breathMax)
                    {
                        breath = breathMax;
                    }
                    breathCD = 0;
                }
            }
            if (IsDrowning && Main.rand.NextBool(20)&& !lavaWet && !honeyWet)
            {
                Dust.NewDust(new Vector2(Center.X + width * 0.4f * direction, Center.Y + height * 0.4f * -gravDir), 8, 8, DustID.BreatheBubble, Scale: 1.2f);
            }
        }

        private void ItemCheck_HandleMPItemAnimation(Item item)
        {
            if (CanAutoReuseItem(item)) releaseUseItem = true;
        }

        private void ItemCheck_HandleMount()
        {
            if (!mount.Active) return;
            MountLoader.UseAbility(this, Vector2.Zero, false);
            if (mount.Type == 8)
            {
                noItems = true;
                if (controlUseItem)
                {
                    channel = true;
                    if (releaseUseItem)
                    {
                        mount.UseAbility(this, Vector2.Zero, true);
                        releaseUseItem = false;
                    }
                }
            }
            if (IsPlayerCharacter && gravDir == -1f)
            {
                mount.Dismount(this);
            }
        }

        private void ItemCheck_ApplyManaRegenDelay(Item item)
        {
            if (GetManaCost(item) > 0)
            {
                manaRegenDelay = (int)maxRegenDelay;
            }
        }

        private void EmitMaxManaEffect()
        {
            SoundEngine.PlaySound(SoundID.SoundByIndex[25]);
            for (byte i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(position, width, height, DustID.ManaRegeneration, 0, 0, 255, default(Color), Main.rand.Next(20, 26) * 0.1f);
                Main.dust[d].noLight = true;
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
            }
        }
        #endregion

        public Rectangle GetAnimationFrame(int FrameID)
        {
            return GetAnimationFrame(FrameID, Base.SpriteWidth, Base.SpriteHeight, Base.FramesInRow);
        }

        public Rectangle GetAnimationFrame(int FrameID, int FrameWidth, int FrameHeight, int FramesInRow)
        {
            Rectangle rect = new Rectangle(FrameID, 0, FrameWidth, FrameHeight);
            if(rect.X >= FramesInRow)
            {
                rect.Y = rect.Y + rect.X / FramesInRow;
                rect.X = rect.X - rect.Y * FramesInRow;
            }
            rect.X = rect.X * rect.Width;
            rect.Y = rect.Y * rect.Height;
            return rect;
        }

        public override bool DrawCompanionHead(Vector2 Position, bool FacingLeft, float Scale = 1f, float MaxDimension = 0)
        {
            Texture2D HeadTexture = Base.GetSpriteContainer.HeadTexture;
            Rectangle HeadFrame = Base.GetHeadDrawFrame(HeadTexture, this);
            if (GetSelectedSkin != null)
            {
                GetSelectedSkin.ModifyHeadDraw(ref HeadTexture, ref HeadFrame);
            }
            if(MaxDimension > 0)
            {
                float DownscaledDimension = 1f;
                if(HeadFrame.Width > HeadFrame.Height)
                {
                    DownscaledDimension = (float)MaxDimension / (HeadFrame.Width * Scale);
                }
                else
                {
                    DownscaledDimension = (float)MaxDimension / (HeadFrame.Height * Scale);
                }
                Scale *= DownscaledDimension;
            }
            if (HeadTexture != null)
                Main.spriteBatch.Draw(HeadTexture, Position, HeadFrame, Color.White, 0f, new Vector2(HeadFrame.Width, HeadFrame.Height) * 0.5f, Scale, FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return true;
        }

        protected override void UpdateMountPositioning()
        {
            
        }

        public class HeldItemSetting
        {
            public int ItemAnimation = 0, ItemAnimationMax = 0;
            public int SelectedItem = 0;
            public int ItemWidth = 0, ItemHeight = 0;
            public int ItemTime = 0, ItemTimeMax = 0;
            public int ReuseDelay = 0;
            public bool channel = false;
            public int ToolTime = 0;
            public Item LastVisualizedSelectedItem = new Item();
            public Vector2 ItemPosition = Vector2.Zero;
            public float ItemRotation = 0;
            public bool releaseUseItem = true;
            private TerraGuardian tg;
            public bool IsActionHand = false;

            public HeldItemSetting(TerraGuardian tg)
            {
                this.tg = tg;
            }

            public void SetSettings(TerraGuardian tg)
            {
                ItemAnimation = tg.itemAnimation;
                ItemAnimationMax = tg.itemAnimationMax;
                SelectedItem = tg.selectedItem;
                ItemWidth = tg.itemWidth;
                ItemHeight = tg.itemHeight;
                ItemTime = tg.itemTime;
                ItemTimeMax = tg.itemTimeMax;
                ReuseDelay = tg.reuseDelay;
                channel = tg.channel;
                ToolTime = tg.toolTime;
                LastVisualizedSelectedItem = tg.lastVisualizedSelectedItem;
                ItemPosition = tg.itemLocation;
                ItemRotation = tg.itemRotation;
                releaseUseItem = tg.releaseUseItem;
            }

            public void ApplyToTg(TerraGuardian tg)
            {
                tg.itemAnimation = ItemAnimation;
                tg.itemAnimationMax = ItemAnimationMax;
                tg.selectedItem = SelectedItem;
                tg.itemWidth = ItemWidth;
                tg.itemHeight = ItemHeight;
                tg.itemTime = ItemTime;
                tg.itemTimeMax = ItemTimeMax;
                tg.reuseDelay = ReuseDelay;
                tg.channel = channel;
                tg.toolTime = ToolTime;
                tg.lastVisualizedSelectedItem = LastVisualizedSelectedItem;
                tg.itemLocation = ItemPosition;
                tg.itemRotation = ItemRotation;
                tg.releaseUseItem = releaseUseItem;
            }
        }

        public struct HatCompatibilityLogger
        {
            public bool IsCompatible;
            public int OtherHatID;
            public int LastHatID;

            public HatCompatibilityLogger()
            {
                LastHatID = -1;
                IsCompatible = false;
                OtherHatID = -1;
            }

            public HatCompatibilityLogger(int HeadID, int OtherHatID = -1)
            {
                LastHatID = HeadID;
                IsCompatible = HeadID > -1 && MainMod.HeadgearAbleEquipments.Contains(HeadID);
                this.OtherHatID = OtherHatID;
            }
        }

        internal struct ItemMask : IDisposable
        {
            private TerraGuardian tg;
            private byte ArmID;

            public ItemMask(TerraGuardian tg, byte ArmID)
            {
                this.tg = tg;
                this.ArmID = ArmID;
                HeldItemSetting held = tg.HeldItems[ArmID];
                tg.itemAnimation = held.ItemAnimation;
                tg.itemAnimationMax = held.ItemAnimationMax;
                tg.selectedItem = held.SelectedItem;
                tg.itemWidth = held.ItemWidth;
                tg.itemHeight = held.ItemHeight;
                tg.itemTime = held.ItemTime;
                tg.itemTimeMax = held.ItemTimeMax;
                tg.reuseDelay = held.ReuseDelay;
                tg.channel = held.channel;
                tg.toolTime = held.ToolTime;
                tg.lastVisualizedSelectedItem = held.LastVisualizedSelectedItem;
                tg.itemLocation = held.ItemPosition;
                tg.itemRotation = held.ItemRotation;
                tg.releaseUseItem = held.releaseUseItem;
            }

            public void Dispose()
            {
                HeldItemSetting held = tg.HeldItems[ArmID];
                held.ItemAnimation = tg.itemAnimation;
                held.ItemAnimationMax = tg.itemAnimationMax;
                held.SelectedItem = tg.selectedItem;
                held.ItemWidth = tg.itemWidth;
                held.ItemHeight = tg.itemHeight;
                held.ItemTime = tg.itemTime;
                held.ItemTimeMax = tg.itemTimeMax;
                held.ReuseDelay = tg.reuseDelay;
                held.channel = tg.channel;
                held.ToolTime = tg.toolTime;
                held.LastVisualizedSelectedItem = tg.lastVisualizedSelectedItem;
                held.ItemPosition = tg.itemLocation;
                held.ItemRotation = tg.itemRotation;
                held.releaseUseItem = tg.releaseUseItem;
            }
        }

        public enum AnimationStates : byte
        {
            Standing,
            Moving,
            Swiming,
            InAir,
            Defeated,
            RidingMount,
            UsingFurniture,
            WallSliding,
            IceSliding,
            Crouching,
            Sitting,
            Sleeping,
            Idle
        }
    }
}