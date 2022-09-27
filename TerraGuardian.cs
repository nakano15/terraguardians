using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Audio;
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

namespace terraguardians
{
    public class TerraGuardian : Companion
    {
        public Rectangle BodyFrame = new Rectangle();
        public Rectangle LeftArmFrame = new Rectangle();
        public Rectangle RightArmFrame = new Rectangle();
        private float BodyFrameTime = 0;
        private AnimationStates PreviousAnimationState = AnimationStates.Standing;
        private short[] HandFrames = new short[2];

        protected override void UpdateAnimations()
        {
            PlayerFrame();
            AnimationStates NewState = AnimationStates.Standing;
            if(swimTime > 0) NewState = AnimationStates.Swiming;
            else if (velocity.Y != 0) NewState = AnimationStates.InAir;
            else if (mount.Active) NewState = AnimationStates.RidingMount;
            else if (sliding) NewState = AnimationStates.WallSliding;
            else if (velocity.X != 0 && (slippy || slippy2 || windPushed) && !controlLeft && !controlRight) NewState = AnimationStates.IceSliding;
            else if (velocity.X != 0) NewState = AnimationStates.Moving;
            if(NewState != PreviousAnimationState)
                BodyFrameTime = 0;
            PreviousAnimationState = NewState;
            short BodyFrameID = 0;
            short[] ArmFramesID = new short[Base.GetHands];
            if (mount.Active)
            {
                Animation anim = Base.GetAnimation(AnimationTypes.SittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.ChairSittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.StandingFrame);
                BodyFrameID = anim.UpdateTimeAndGetFrame(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse, but...
            {
                if (swimTime > 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(2, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.JumpingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
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
                        BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(System.Math.Abs(velocity.X) * 1.3f, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
            }
            if(BodyFrameID == -1) BodyFrameID = 0;
            for(int a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFramesID[a] = BodyFrameID;
            }
            bool CanVisuallyHoldItem = this.CanVisuallyHoldItem(HeldItem);
            bool HeldItemTypeIsnt4952 = HeldItem.type != 4952;
            //Item attack animations here
            if(sandStorm)
            {

            }
            else if (itemAnimation > 0 && HeldItem.useStyle != 10 && HeldItemTypeIsnt4952)
            {
                ArmFramesID[0] = GetItemUseArmFrame();
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            LeftArmFrame = GetAnimationFrame(ArmFramesID[0]);
            RightArmFrame = GetAnimationFrame(ArmFramesID[1]);
            HandFrames = ArmFramesID;
        }

        public short GetItemUseArmFrame()
        {
            short Frame = 0;
            if(HeldItem.useStyle == 1 || HeldItem.useStyle == 11 || HeldItem.type == 0)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 7)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.67f + 0.33f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 2)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 9 || HeldItem.useStyle == 8)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.StandingFrame);
                Frame = animation.GetFrame(0);
            }
            else if(HeldItem.useStyle == 6)
            {
                float AnimationPercentage = System.Math.Min(1, (1f - (float)itemAnimation / itemAnimationMax) * 6);
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.5f + 0.5f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 3 || HeldItem.useStyle == 12)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 4)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count * 0.5f));
            }
            else if(HeldItem.useStyle == 13)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 5)
            {
                float RotationValue = System.Math.Clamp((1f + itemRotation * direction) * 0.5f, 0, 1);
                //if(gravDir == -1)
                //    AnimationPercentage = 1f - AnimationPercentage;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(RotationValue * animation.GetTotalAnimationDuration);
            }
            return Frame;
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if(direction < 0)
                Position.X = Base.SpriteWidth - Position.X;
            if(gravDir < 0)
                Position.Y = Base.SpriteHeight - Position.Y;
            Position *= Scale;
            Position.X += (width - Base.SpriteWidth * Scale) * 0.5f;
            Position.Y += height - Base.SpriteHeight * Scale;
            if(AlsoTakePosition)
                Position += position;
            return Position;
        }

        protected override void UpdateItemScript()
        {
            if (PlayerLoader.PreItemCheck(this))
            {
                ItemCheck_Inner();
            }
            PlayerLoader.PostItemCheck(this);
        }

        #region Item Use Scripts
        private void ItemCheck_Inner()
        {
            if(CCed)
            {
                channel = false;
                itemAnimation = itemAnimationMax = 0;
                return;
            }
            float HeightOffsetHitboxCenter = this.HeightOffsetHitboxCenter;
            Item item = HeldItem;
            if (IsPlayerCharacter && PlayerInput.ShouldFastUseItem)
            {
                controlUseItem = true;
            }
            Item lastItem = (itemAnimation > 0) ? lastVisualizedSelectedItem : item;
            Rectangle drawHitbox = Item.GetDrawHitbox(lastItem.type, this);
            if(itemAnimation > 0)
            {
                if (item.mana > 0)
                {
                    ItemCheck_ApplyManaRegenDelay(item);
                }
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
            if(AllowUsage && controlUseItem && releaseUseItem && itemAnimation == 0 && item.useStyle != 0)
            {
                if (altFunctionUse == 1) altFunctionUse = 2;
                if (item.shoot == 0) itemRotation = 0;
                bool CanUse = ItemCheck_CheckCanUse(item);
                if (item.potion && CanUse) ApplyPotionDelay(item);
                if (item.mana > 0 && CanUse && (IsLocalCompanion || IsPlayerCharacter) && item.buffType != 0 && item.buffTime != 0) AddBuff(item.buffType, item.buffTime);
                if (item.shoot <= 0 || !ProjectileID.Sets.MinionTargettingFeature[item.shoot] || altFunctionUse != 2)
                    ItemCheck_ApplyPetBuffs(item);
                if ((IsLocalCompanion || IsPlayerCharacter) && gravDir == 1 && item.mountType != -1 && mount.CanMount(item.mountType, this))
                {
                    mount.SetMount(item.mountType, this);
                }
                if ((item.shoot <= 0 || !ProjectileID.Sets.MinionTargettingFeature[item.shoot]))
                {
                    FreeUpPetsAndMinions(item);
                }
                if(CanUse) ItemCheck_StartActualUse(item);
            }
            if (!controlUseItem) channel = false;
            ItemLoader.HoldItem(item, this);
            if(itemAnimation > 0)
            {
                //ApplyUseStyle script.
                ItemCheck_TerraGuardiansApplyUseStyle(HeightOffsetHitboxCenter, lastItem, drawHitbox, 0);
                ItemLoader.UseStyle(item, this, drawHitbox);
            }
            else
            {
                //ApplyHoldStyle script.
            }
        }

        private void ItemCheck_TerraGuardiansApplyUseStyle(float MountOffset, Item item, Rectangle HeldItemFrame, byte Hand)
        {
            if(Main.dedServ) return;
            switch(item.useStyle)
            {
                case 1:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(AttackPercentage);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        /*if(item.type > -1 && Item.claw[item.type])
                        {
                            if(AttackPercentage >= 0.666f)
                            {
                                itemLocation.X += (HeldItemFrame.Width * 0.5f - 10) * direction;
                                itemLocation.Y += (HeldItemFrame.Width * 0.5f - 10) * direction;
                            }
                        }*/
                        itemRotation = (AttackPercentage - 0.5f) * -direction * 3.5f - direction * 0.3f;
                    }
                    break;
                case 2: //For now, leave at this.
                case 6:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        itemRotation = (1f - (AttackPercentage * 6)) * direction * 2 - 1.4f * direction;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(System.Math.Clamp((1f - AttackPercentage) * 2, 0, 0.6f));
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                    }
                    break;
                case 3:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(0.7f);
                        if(itemAnimation > itemAnimationMax * 0.666f)
                        {
                            itemLocation.X = itemLocation.Y = -1000f;
                            itemRotation = -1.3f * direction;
                        }
                        else
                        {
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                            float MovementDirection = (float)itemAnimation / itemAnimationMax * HeldItemFrame.Width * direction * GetAdjustedItemScale(item) * 1.2f - 10f * direction;
                            if (MovementDirection * direction > 4f) MovementDirection = 8 * direction;
                            itemLocation.X -= MovementDirection;
                            itemRotation = 0.8f * direction;
                            if(item.type == 946 || item.type == 4707) itemLocation.X -= 6 * direction;
                        }
                    }
                    break;
                case 4:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(0.3f);
                        float OffsetX = 0, OffsetY = 0;
                        if (item.type == 3601) OffsetX = 10;
                        else if (item.type == 5114)
                        {
                            OffsetX = 10;
                            OffsetY = -1;
                        }
                        else if (item.type == 5120) OffsetX = 10;
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        itemLocation.X += OffsetX;
                        itemLocation.Y += OffsetY;
                    }
                    break;
                case 5:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        if(item.type == 3779)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        }
                        else if (item.type == 4262)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                            if (Main.rand.Next(20) == 0)
                            {
                                //Snake flute effect
                            }
                        }
                        else if (Item.staff[item.type])
                        {
                            float ScaleFactor = 6f;
                            if (item.type == 3476) ScaleFactor = 14f;
                            //Need to calculate use direction, but where/when is it set?
                        }
                    }
                    break;
                case 7: //Unused, it seems
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        itemRotation = AttackPercentage * direction * 2 + -1.4f * direction;
                    }
                    break;
                case 9:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        float t = Utils.GetLerpValue(0, 0.7f, 1f - AttackPercentage, true);
                        itemRotation = t * -direction * 2 + 0.7f + direction;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(System.Math.Clamp((1f - AttackPercentage) * 2, 0f, 0.6f));
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame);
                    }
                    break;
                case 11:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(1f);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame);
                    }
                    break;
            }
            if (gravDir == -1) itemRotation = -itemRotation;
        }

        private void ItemCheck_StartActualUse(Item item)
        {
            bool IsGravediggerShovel = item.type == 4711;
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
                if(IsPlayerCharacter && MinionSlotsSum + SlotsReq >= 9)
                {
                    //9 or more minions achievement
                }
                return;
            }
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this))
                {
                    if(Main.projectile[i].type == item.shoot)
                        Main.projectile[i].Kill();
                    if(item.shoot == 72 && (Main.projectile[i].type == 86 || Main.projectile[i].type == 87))
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
            if(item.type >= 4797 && item.type <= 4817)
                GetPet = true;
            if (GetPet) AddBuff(item.buffType, 3600);
        }

        private void ApplyPotionDelay(Item item)
        {
            if (item.type == 227) AddBuff(21, potionDelay = restorationDelayTime);
            else if (item.type == 5) AddBuff(21, potionDelay = mushroomDelayTime);
            else AddBuff(21, potionDelay = potionDelayTime);
        }

        private bool ItemCheck_CheckCanUse(Item item)
        {
            if (item.IsAir || !CombinedHooks.CanUseItem(this, item)) return false;
            bool Can = true;
            int MouseX = (int)((AimPosition.X) * DivisionBy16);
            int MouseY = gravDir == -1 ? 
                (int)((Main.screenHeight - AimPosition.Y)) : 
                (int)((AimPosition.Y) * DivisionBy16);
            if (item.type == 3335 && (extraAccessory || !Main.expertMode))
                Can = false;
            if (pulley && item.fishingPole > 0) Can = false;
            if (pulley && ItemID.Sets.IsAKite[item.type]) Can = false;
            if (item.type == 3611 && (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode.Red | WiresUI.Settings.MultiToolMode.Green | WiresUI.Settings.MultiToolMode.Blue | WiresUI.Settings.MultiToolMode.Yellow | WiresUI.Settings.MultiToolMode.Yellow)) == 0) Can = false;
            if ((item.type == 3611 || item.type == 3625) && wireOperationsCooldown > 0) Can = false;
            if (!CheckDD2CrystalPaymentLock(item)) Can = false;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && !downedDD2EventAnyDifficulty && !DD2Event.Ongoing) Can=false;
            int PushYUp;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && (IsPlayerCharacter || IsLocalCompanion))
            {
                FindSentryRestingSpot(item.shoot, out int WorldX, out int WorldY, out PushYUp);
                if (DD2Event.Ongoing)
                {
                    if (WouldSpotOverlapWithSentry(WorldX, WorldY, item.shoot == 688 || item.shoot == 689 || item.shoot == 690))
                        Can = false;
                }
                if (Can)
                {
                    WorldX = (int)(WorldX * DivisionBy16);
                    WorldY = (int)(WorldY * DivisionBy16);
                    WorldY--;
                    if (item.shoot == 688 || item.shoot == 689 || item.shoot == 690)
                    {
                        if (Collision.SolidTiles(WorldX, WorldX, WorldY - 2, WorldY)) Can = false;
                    }
                    else if (WorldGen.SolidTile(WorldX,WorldY)) Can = false;
                }
            }
            if (wet && (item.shoot == 85 || item.shoot == 15 || item.shoot == 34)) Can = false;
            if (item.makeNPC > 0 || !NPC.CanReleaseNPCs(whoAmI)) Can = false;
            if((IsLocalCompanion || IsPlayerCharacter) && item.type == 603 && !Main.runningCollectorsEdition) Can = false;
            if (item.type == 1071 || item.type == 1072)
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
            if (noItems) Can = false;
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
            if (item.shoot == 6 || item.shoot == 19 || item.shoot == 33 || item.shoot == 52 || item.shoot == 113 || item.shoot == 320 || item.shoot == 333 || item.shoot == 383 || item.shoot == 491 || item.shoot == 867 || item.shoot == 902 || item.shoot == 866)
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
            if (item.shoot == 106 || item.shoot == 272)
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
            if (item.shoot == 13 || item.shoot == 32 || (item.shoot >= 230 && item.shoot <= 235) || item.shoot == 315 || item.shoot == 331 || item.shoot == 372)
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
            if (item.shoot == 332)
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
            if ((item.type == 43 || item.type == 544 || item.type == 556 || item.type == 557) && Main.dayTime)
            {
                Can = false;
            }
            if (item.type == 70 && !ZoneCorrupt)
                Can = false;
            if (item.type == 1133 && !ZoneJungle)
                Can = false;
            if (item.type == 5120 && !ZoneSnow)
                Can = false;
            if ((item.type == 1844 || item.type == 1958) && (Main.dayTime || Main.pumpkinMoon || Main.snowMoon || DD2Event.Ongoing))
                Can = false;
            if (item.type == 2767 && (!Main.dayTime || Main.eclipse || !Main.hardMode))
                Can = false;
            if (item.type == 4271 && (Main.dayTime || Main.bloodMoon))
                Can = false;
            if (item.type == 3601 && (!NPC.downedGolemBoss || !Main.hardMode || NPC.AnyDanger() || NPC.AnyoneNearCultists()))
                Can = false;
            if (!SummonItemCheck()) Can = false;
            if (item.shoot == 17 && Can && (IsPlayerCharacter || IsLocalCompanion) && !ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY])) Can = false;
            if (item.fishingPole > 0) Can = ItemCheck_CheckFishingBobbers(Can);
            if (ItemID.Sets.HasAProjectileThatHasAUsabilityCheck[item.type]) Can = ItemCheck_CheckUsabilityOfProjectiles(Can);
            if (item.shoot == 17 && Can && (IsLocalCompanion || IsPlayerCharacter))
            {
                if (ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY]))
                {
                    WorldGen.KillTile(MouseX, MouseY, false, false, true);
                    if (!Main.tile[MouseX, MouseY].HasTile)
                    {
                        if (Main.netMode == 1)
                            NetMessage.SendData(17, -1, -1, null, 4, MouseX, MouseY);
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
                if (!proj.active || ProjMod.IsThisCompanionProjectile(proj, this) || !proj.bobber)
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
                if (Main.netMode != 1) NPC.SpawnOnPlayer(whoAmI, 370);
                //else NetMessage.SendData(61, -1, -1, null, whoAmI, 370); //TODO - Need to figure out how to take companion position
                bobber.ai[0] = 2f;
            }
            else if (bobber.localAI[1] < 0)
            {
                Point pos = new Point((int)bobber.position.X, (int)bobber.position.Y);
                int NpcID = (int)(- bobber.localAI[1]);
                if(NpcID == 618) pos.Y += 64;
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(130, -1, -1, null, pos.X / 16, pos.Y / 16, NpcID);
                }
                else
                {
                    NPC.NewNPC(new EntitySource_FishedOut(this), pos.X, pos.Y, NpcID);
                    bobber.ai[0] = 2;
                }
            }
            else if (Main.rand.Next(7) == 0 && !accFishingLine)
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
                if (item.rare < 0) consume = false;
            }
            BaitTypeUsed = ((byte)itemAnimation);
            if (BaitTypeUsed == 2673) consume = true;
            if (CombinedHooks.CanConsumeBait(this, bait) ?? consume)
            {
                if (bait.type == 4361 || bait.type == 4362)
                    NPC.LadyBugKilled(Center, bait.type == 4362);
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
            if (item.type == 2795) NoPay = true;
            if (item.type == 3852 && IsAltUse)
                Cost = (int)(item.mana * 2 * manaCost);
            if (item.shoot > 0 && IsAltUse)
            {
                if (ProjectileID.Sets.TurretFeature[item.shoot] || ProjectileID.Sets.MinionTargettingFeature[item.shoot])
                    NoPay = true;
            }
            if (item.type == 3006) NoPay = true;
            if (item.type != 3269 && !CheckMana(item, -1, !NoPay))
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
            if (GetManaCost(item) > 0) manaRegenDelay = (int)maxRegenDelay;
        }

        private void EmitMaxManaEffect()
        {
            SoundEngine.PlaySound(SoundID.SoundByIndex[25]);
            for (byte i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(position, width, height, 45, 0, 0, 255, default(Color), Main.rand.Next(20, 26) * 0.1f);
                Main.dust[d].noLight = true;
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
            }
        }
        #endregion

        public override void HoldItem(Item item)
        {
            //Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            //itemLocation.X = (int)HandPosition.X;
            //itemLocation.Y = (int)HandPosition.Y;
        }

        public override void UseItemHitbox(Item item, ref Rectangle hitbox, ref bool noHitbox)
        { //For item hitbox
            //Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            //hitbox.X = (int)(hitbox.X - position.X + HandPosition.X);
            //hitbox.Y = (int)(hitbox.Y - position.Y + HandPosition.Y);
            //itemLocation.X = (int)(itemLocation.X - position.X + HandPosition.X);
            //itemLocation.Y = (int)(itemLocation.Y - position.Y + HandPosition.Y);
            //itemLocation.X = (int)HandPosition.X;
            //itemLocation.Y = (int)HandPosition.Y;
            //hitbox.X = (int)HandPosition.X;
            //hitbox.Y = (int)HandPosition.Y;
            //hitbox.Width = (int)(hitbox.Width * Scale);
            //hitbox.Height = (int)(hitbox.Height * Scale);
            //itemWidth = (int)(itemWidth * Scale);
            //itemHeight = (int)(itemHeight * Scale);
        }

        public override void HoldStyle(Item item, Rectangle heldItemFrame)
        { //For item holding style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
            itemLocation.X = (int)HandPosition.X;
            itemLocation.Y = (int)HandPosition.Y;
        }

        public override void UseStyle(Item item, Rectangle heldItemFrame)
        { //For item use style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
            itemLocation.X = HandPosition.X;
            itemLocation.Y = HandPosition.Y;
        }

        public Rectangle GetAnimationFrame(int FrameID)
        {
            Rectangle rect = new Rectangle(FrameID, 0, Base.SpriteWidth, Base.SpriteHeight);
            if(rect.X >= Base.FramesInRow)
            {
                rect.Y = rect.Y + rect.X / Base.FramesInRow;
                rect.X = rect.X - rect.Y * Base.FramesInRow;
            }
            rect.X = rect.X * rect.Width;
            rect.Y = rect.Y * rect.Height;
            return rect;
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
            IceSliding
        }
    }
}