using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public partial class Companion : Player
    {
        private static Companion ReferedCompanion = null;
        public static Companion GetReferedCompanion { get { return ReferedCompanion; } }
        private static Vector2 NewAimDirectionBackup = Vector2.Zero;

        private PlayerMod _PlayerMod;
        public PlayerMod GetPlayerMod { get { return _PlayerMod; } }
        public const float DivisionBy16 = 1f / 16;
        public ushort GetWhoAmID { get{ return WhoAmID; }}
        private ushort WhoAmID = 0;
        private static ushort LastWhoAmID = 0;
        private CompanionTownNpcState _TownNpcState;
        public CompanionTownNpcState GetTownNpcState
        {
            get
            {
                return _TownNpcState;
            }
        }
        public bool IsTownNpc
        {
            get
            {
                return _TownNpcState != null;
            }
        }

        public CompanionBase Base
        {
            get
            {
                return Data.Base;
            }
        }
        private CompanionData _data = new CompanionData();
        public CompanionData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public uint ID { get { return Data.ID; } }
        public string ModID { get { return Data.ModID; } }
        public uint Index { get{ return Data.Index; } }
        public bool HasBeenMet { get { return WorldMod.HasMetCompanion(Data); }}
        public bool IsPlayerCharacter = false;
        public byte OutfitID { get { return Data.OutfitID; } set { Data.OutfitID = value; } }
        public byte SkinID { get { return Data.SkinID; } set { Data.SkinID = value; } }
        public Entity Owner = null;
        public float SpriteWidth { get{ return Base.SpriteWidth * Scale; }}
        public float SpriteHeight { get{ return Base.SpriteHeight * Scale; }}
        #region Useful getter setters
        public bool MoveLeft { get { return controlLeft; } set { controlLeft = value; } }
        public bool LastMoveLeft { get { return releaseLeft; } set { releaseRight = value; } }
        public bool MoveRight { get { return controlRight; } set { controlRight = value; } }
        public bool LastMoveRight { get { return releaseRight; } set { releaseRight = value; } }
        public bool MoveUp { get { return controlUp; } set { controlUp = value; } }
        public bool LastMoveUp { get { return releaseUp; } set { releaseUp = value; } }
        public bool MoveDown { get { return controlDown; } set { controlDown = value; } }
        public bool LastMoveDown { get { return releaseDown; } set { releaseDown = value; } }
        public bool ControlJump { get { return controlJump; } set { controlJump = value; } }
        public bool LastControlJump { get { return releaseJump; } set{ releaseJump = value; } }
        public bool ControlAction { get { return controlUseItem; } set { controlUseItem = value; } }
        public bool LastControlAction { get { return releaseUseItem; } set { releaseUseItem = value; } }
        #endregion
        public bool FlipWeaponUsageHand = false;
        #region Behaviors
        public BehaviorBase idleBehavior = new IdleBehavior(),
            combatBehavior = new CombatBehavior(),
            followBehavior = new FollowLeaderBehavior(),
            preRecruitBehavior = null;
        #endregion
        protected int furniturex = -1, furniturey = -1;
        protected bool reachedfurniture = false;
        public int GetFurnitureX { get{ return furniturex; } }
        public int GetFurnitureY { get{ return furniturey; } }
        public bool GetReachedFurniture { get { return reachedfurniture; } }
        public bool GoingToOrUsingFurniture { get { return furniturex > -1 && furniturey > -1; } }
        public bool UsingFurniture { get { return furniturex > -1 && furniturey > -1 && reachedfurniture; } }
        public Vector2 AimDirection = Vector2.Zero;
        public Vector2 GetAimedPosition
        {
            get
            {
                return Center + AimDirection;
            }
            set
            {
                AimDirection = value - Center;
            }
        }
        public bool IsMountedOnSomething { get { return CharacterMountedOnMe != null; } }
        public Player GetCharacterMountedOnMe { get { return CharacterMountedOnMe; }}
        private Player CharacterMountedOnMe = null;
        public bool WalkMode = false;
        public float Scale = 1f;
        public bool Crouching { get{ return MoveDown; } set { MoveDown = value; } }
        public Entity Target = null;
        public bool IsStarter { get { return Data.IsStarter; } }
        public float GetHealthScale { get { return Base.HealthScale; } }
        public float GetManaScale { get { return Base.ManaScale; } }
        public bool IsFollower { get{ return Owner != null; }}
        public bool TargettingSomething { get { return Target != null; } }
        public string GetName{ get { return GetGoverningBehavior().CompanionNameChange(this); }}
        public string GetRealName{ get { return Data.GetName; }}
        public bool IsOnSleepTime
        {
            get
            {
                if (Main.eclipse || Main.bloodMoon) return false;
                if (!Base.IsNocturnal)
                {
                    return (!Main.dayTime && Main.time >= 9000) || (Main.dayTime && Main.time < 3600);
                }
                else
                {
                    return Main.dayTime && Main.time >= 19800 && Main.time < 48600;
                }
            }
        }
        private byte AppliedFoodLevel = 0;
        public byte GetAppliedFoodLevel { get { return AppliedFoodLevel; } }
        private BitsByte _statsValues = 0;
        public bool IsHungry { get { return _statsValues[0]; } set { _statsValues[0] = value; } }
        public bool IsSober { get { return _statsValues[1]; } set { _statsValues[1] = value; } }

        public string GetPlayerNickname(Player player)
        {
            return Data.GetPlayerNickname(player);
        }

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner is Player && ((Player)Owner).whoAmI == Main.myPlayer) || (Main.netMode == 2 && (Owner == null || Owner is NPC));
            }
        }

        public bool CanJump
        {
            get
            {
                return (velocity.Y == 0 || sliding) && !controlJump;
            }
        }

        public bool CanDoJumping
        {
            get
            {
                return CanJump || jump > 0;
            }
        }

        public bool IsSameID (uint ID, string ModID = "")
        {
            return Data.IsSameID(ID, ModID);
        }

        public bool IsSameID (CompanionID ID)
        {
            return Data.IsSameID(ID);
        }

        public Companion() : base()
        {
            WhoAmID = LastWhoAmID++;
            _PlayerMod = GetModPlayer<PlayerMod>();
        }

        public string GetNameColored()
        {
            return Data.GetNameColored();
        }

        private static BitsByte _Behaviour_Flags;
        public static bool Behaviour_AttackingSomething
        {
            get
            {
                return _Behaviour_Flags[0];
            }
            set
            {
                _Behaviour_Flags[0] = value;
            }
        }

        public static bool Behaviour_InDialogue
        {
            get
            {
                return _Behaviour_Flags[1];
            }
            set
            {
                _Behaviour_Flags[1] = value;
            }
        }

        public static bool Behavior_UsingPotion
        {
            get
            {
                return _Behaviour_Flags[2];
            }
            set
            {
                _Behaviour_Flags[2] = value;
            }
        }

        public void SaySomething(string Text)
        {
            chatOverhead.NewMessage(Text, 180 + Text.Length);
        }

        public void SetFallStart()
        {
            fallStart = fallStart2 = (int)(position.Y * DivisionBy16);
        }

        public BehaviorBase GetGoverningBehavior()
        {
            if(Owner != null)
            {
                return followBehavior;
            }
            else 
            {
                if (!HasBeenMet && preRecruitBehavior != null)
                {
                    return preRecruitBehavior;
                }
                return idleBehavior;
            }
        }

        public void UpdateBehaviour()
        {
            _Behaviour_Flags = 0;
            MoveLeft = MoveRight = MoveUp = ControlJump = controlUseItem = false;
            if(!Base.CanCrouch || itemAnimation == 0)
                MoveDown = false;
            LookForTargets();
            CheckForPotionUsage();
            combatBehavior.Update(this);
            UpdateFurnitureUsageScript();
            UpdateDialogueBehaviour();
            if(!Behaviour_AttackingSomething)
                ChangeAimPosition(Center + Vector2.UnitX * width * direction);
            GetGoverningBehavior().Update(this);
            UpdateMountedBehavior();
            if(MoveLeft || MoveRight)
            {
                CheckIfNeedToJumpTallTile();
            }
        }

        public bool IsComfortPointsMaxed()
        {
            return Data.FriendshipProgress.IsComfortMaxed();
        }

        public void CheckForPotionUsage()
        {
            if(itemAnimation > 0) return;
            if (statLife < statLifeMax2 * 0.4f)
            {
                byte HighestHPPot = 255;
                int HighestHealValue = 0;
                for (byte i = 0; i < 50; i++)
                {
                    if (inventory[i].type > 0 && inventory[i].healLife > 0)
                    {
                        if(inventory[i].healLife > HighestHealValue)
                        {
                            HighestHealValue = inventory[i].healLife;
                            HighestHPPot = i;
                        }
                    }
                }
                if (HighestHPPot < 255)
                {
                    selectedItem = HighestHPPot;
                    controlUseItem = true;
                    Behavior_UsingPotion = true;
                    return;
                }
            }
            if (statMana < statManaMax2 * 0.2f)
            {
                byte HighestMPPot = 255;
                int HighestHealValue = 0;
                for (byte i = 0; i < 50; i++)
                {
                    if (inventory[i].type > 0 && inventory[i].healMana > 0)
                    {
                        if(inventory[i].healLife == 0 && inventory[i].healMana > HighestHealValue)
                        {
                            HighestHealValue = inventory[i].healMana;
                            HighestMPPot = i;
                        }
                    }
                }
                if (HighestMPPot < 255)
                {
                    selectedItem = HighestMPPot;
                    controlUseItem = true;
                    Behavior_UsingPotion = true;
                    return;
                }
            }
            if (Main.rand.Next(5) == 0)
            {
                byte StrongestFoodPosition = 255;
                byte StrongestFoodValue = 0;
                bool IsFood = true;
                IsHungry = true;
                for (byte i = 0; i < 50; i++)
                {
                    if (inventory[i].type > 0 && inventory[i].buffType > 0)
                    {
                        switch (inventory[i].buffType)
                        {
                            case BuffID.WellFed:
                                {
                                    const byte FoodValue = 5;
                                    if(IsHungry && StrongestFoodValue < FoodValue)
                                    {
                                        StrongestFoodPosition = i;
                                        StrongestFoodValue = FoodValue;
                                        IsFood = true;
                                    }
                                }
                                break;
                            case BuffID.WellFed2:
                                {
                                    const byte FoodValue = 6;
                                    if(IsHungry && StrongestFoodValue < FoodValue)
                                    {
                                        StrongestFoodPosition = i;
                                        StrongestFoodValue = FoodValue;
                                        IsFood = true;
                                    }
                                }
                                break;
                            case BuffID.WellFed3:
                                {
                                    const byte FoodValue = 7;
                                    if(IsHungry && StrongestFoodValue < FoodValue)
                                    {
                                        StrongestFoodPosition = i;
                                        StrongestFoodValue = FoodValue;
                                        IsFood = true;
                                    }
                                }
                                break;
                            case BuffID.Tipsy:
                                {
                                    const byte FoodValue = 1;
                                    if(IsSober && StrongestFoodValue < FoodValue)
                                    {
                                        StrongestFoodPosition = i;
                                        StrongestFoodValue = FoodValue;
                                        IsFood = false;
                                    }
                                }
                                break;
                        }
                    }
                }
                if(StrongestFoodPosition < 255)
                {
                    selectedItem = StrongestFoodPosition;
                    controlUseItem = true;
                    Behavior_UsingPotion = true;
                }
            }
        }

        public void UpdateExtra()
        {
            UpdateComfortStack();
        }

        public void IncreaseComfortStack(float Value)
        {
            Data.FriendshipProgress.ChangeComfortProgress(Value);
        }

        /*protected void FoodUsageComfortIncrease(int Buff) //Lack of hooks for buffs unallows this.
        {
            if(Buff == BuffID.WellFed || Buff == BuffID.WellFed2 || Buff == BuffID.WellFed3)
            {
                if(HasBuff(BuffID.WellFed) || HasBuff(BuffID.WellFed2) || HasBuff(BuffID.WellFed3)) return;
            }
            if(Buff == BuffID.Tipsy || Buff == BuffID.WellFed2 || Buff == BuffID.WellFed3)
            {
                if(HasBuff(BuffID.Tipsy)) return;
            }
            float Increase = 0;
            switch(Buff)
            {
                case BuffID.Tipsy:
                    Increase = 75;
                    break;
                case BuffID.WellFed:
                    Increase = 100;
                    break;
                case BuffID.WellFed2:
                    Increase = 175;
                    break;
                case BuffID.WellFed3:
                    Increase = 250;
                    break;
            }
            IncreaseComfortStack(Increase);
        }*/

        public void UpdateComfortStack()
        {
            if (TargettingSomething || Main.bloodMoon || Main.eclipse) return;
            if (NPC.TowerActiveNebula || NPC.TowerActiveSolar || NPC.TowerActiveStardust || NPC.TowerActiveVortex || NPC.AnyNPCs(Terraria.ID.NPCID.MoonLordCore))
                return;
            float ComfortSum = 0;
            switch(AppliedFoodLevel)
            {
                case 1:
                    ComfortSum += 0.001f;
                    break;
                case 2:
                    ComfortSum += 0.00175f;
                    break;
                case 3:
                    ComfortSum += 0.0025f;
                    break;
            }
            if (UsingFurniture)
            {
                ComfortSum += 0.03f;
                switch(Main.tile[furniturex, furniturey].TileType)
                {
                    case TileID.Chairs:
                        ComfortSum += 0.035f;
                        break;
                    case TileID.Thrones:
                        ComfortSum += 0.05f;
                        break;
                    case TileID.Benches:
                        ComfortSum += 0.043f;
                        break;
                    case TileID.Beds:
                        ComfortSum += 0.06f;
                        break;
                }
            }
            else
            {
                if (velocity.X == 0 && velocity.Y == 0)
                    ComfortSum += 0.01f;
                else
                    ComfortSum += 0.0033f;
            }
            ComfortSum += ComfortSum * 0.05f * MathF.Min(townNPCs, 5);
            if (ZoneCorrupt || ZoneCrimson)
                ComfortSum *= 0.2f;
            if (Main.invasionProgress > 0)
                ComfortSum *= 0.1f;
            IncreaseComfortStack(ComfortSum);
        }

        public bool CanUseFurniture(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile != null && tile.HasTile)
            {
                switch(tile.TileType)
                {
                    case TileID.Chairs:
                    case TileID.Thrones:
                    case TileID.Benches:
                    case TileID.PicnicTable:
                        return Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0;
                    case TileID.Beds:
                        {

                            return Main.sleepingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) < 2;
                        }
                }
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (c != this && c.furniturex == x && c.furniturey == y)
                    return false;
            }
            return false;
        }

        protected void UpdateFurnitureUsageScript()
        {
            if(!GoingToOrUsingFurniture)
                return;
            Tile tile = Main.tile[furniturex, furniturey];
            if(tile == null || !tile.HasTile || tile.IsActuated)
            {
                LeaveFurniture();
                return;
            }
            if (reachedfurniture) return;
            if(!sleeping.isSleeping && !sitting.isSitting)
            {
                float TileCenterX = furniturex * 16 + 8;
                MoveLeft = MoveRight = false;
                if(Math.Abs(TileCenterX - (position.X + width * 0.5f)) < 20)
                {
                    if (tile != null)
                    {
                        bool IsBed = tile.TileType == TileID.Beds;
                        if (!IsBed)
                        {
                            sitting.SitDown(this, furniturex, furniturey);
                        }
                        else
                        {
                            if(IsBedUseable(furniturex, furniturey))
                                sleeping.StartSleeping(this, furniturex, furniturey);
                            else
                            {
                                LeaveFurniture();
                                return;
                            }
                        }
                        if (sitting.isSitting || sleeping.isSleeping)
                        {
                            reachedfurniture = true;
                        }
                        else
                        {
                            LeaveFurniture();
                        }
                    }
                    else
                    {
                        LeaveFurniture();
                    }
                }
                else
                {
                    if (TileCenterX < position.X + width * 0.5f)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                }
            }
        }

        public bool UseFurniture(int x, int y, bool Teleport = false)
        {
            Tile tile = Main.tile[x, y];
            if (tile != null && tile.HasTile)
            {
                bool HasFurniture = false;
                bool IsBed = false;
                switch(tile.TileType)
                {
                    default:
                        return false;
                    case TileID.Chairs:
                        if(tile.TileFrameY % 40 < 18)
                            y++;
                        HasFurniture = true;
                        break;
                    case TileID.Thrones:
                    case TileID.Benches:
                        {
                            int FramesY = tile.TileType == TileID.Thrones ? 4 : 2;
                            x += 1 - (int)((tile.TileFrameX * (1f / 18)) % 3);
                            y += (int)((FramesY - 1) - (tile.TileFrameY * (1f / 18)) % FramesY);
                            HasFurniture = true;
                        }
                        break;
                    case TileID.PicnicTable:
                        {
                            int FrameX = tile.TileFrameX % 72;
                            if (FrameX < 36)
                            {
                                if(FrameX == 18)
                                    x--;
                            }
                            else
                            {
                                if(FrameX == 36)
                                    x++;
                            }
                            if (tile.TileFrameY % 36 < 18)
                                y++;
                            HasFurniture = true;
                        }
                        break;
                    case TileID.Beds:
                        {
                            bool FacingLeft = tile.TileFrameX < 72;
                            x += (FacingLeft ? 2 : 1) - (int)((tile.TileFrameX * (1f / 18)) % 4);
                            y += 1 - (int)((tile.TileFrameY * (1f / 18)) % 2);
                            HasFurniture = true;
                            IsBed = true;
                        }
                        break;
                }
                if (HasFurniture)
                {
                    if (IsBed && !IsBedUseable(x, y)) return false;
                    furniturex = x;
                    furniturey = y;
                    reachedfurniture = false;
                    SetFallStart();
                    return true;
                }
            }
            return false;
        }

        public void LeaveFurniture()
        {
            if(sitting.isSitting)
            {
                sitting.SitUp(this, false);
            }
            if(sleeping.isSleeping)
            {
                sleeping.StopSleeping(this, false);
            }
            furniturex = furniturey = -1;
            reachedfurniture = false;
        }

        public bool IsBedUseable(int x, int y)
        {
            if (!WorldGen.InWorld(x, y)) return false;
            Tile tile = Main.tile[x, y];
            if(tile.HasTile && tile.TileType == TileID.Beds)
            {
                return Main.sleepingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) < 2;
            }
            return false;
        }

        private void UpdateMountedBehavior()
        {
            if(CharacterMountedOnMe == null) return;
            MoveLeft = MoveRight = MoveUp = ControlJump = false;
            if(!Base.CanCrouch || itemAnimation == 0)
                MoveDown = false;
            switch(Base.MountStyle)
            {
                case MountStyles.CompanionRidesPlayer:
                    {
                        Player mount = CharacterMountedOnMe;
                        if(mount.dead)
                        {
                            ToggleMount(mount, true);
                            return;
                        }
                        if(itemAnimation == 0)
                        {
                            direction = mount.direction;
                        }
                        bool InMineCart = mount.mount.Active && MountID.Sets.Cart[mount.mount.Type];
                        Vector2 MountPosition = Base.GetAnimationPosition(AnimationPositions.SittingPosition).GetPositionFromFrame(0);
                        //Implement the rest later.
                        MountPosition.X += SpriteWidth * 0.5f * direction;
                        /*if(!InMineCart && direction == -1)
                            MountPosition.X = SpriteWidth - MountPosition.X;
                        MountPosition.X = SpriteWidth * 0.5f - MountPosition.X;*/
                        bool SitOnMount = false;
                        //MountPosition.Y = SpriteHeight - MountPosition.Y;
                        if (!InMineCart)
                        {
                            short Frame = Base.GetAnimation(AnimationTypes.PlayerMountedArmFrame).GetFrame(0);
                            AnimationPositionCollection HandPositionCollection = Base.GetAnimationPosition(AnimationPositions.HandPosition);
                            Vector2 HandPosition = HandPositionCollection.GetPositionFromFrame(Frame);
                            if (HandPosition == HandPositionCollection.DefaultCoordinate)
                            {
                                HandPosition = HandPositionCollection.GetPositionFromFrame(Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrameFromPercentage(0.8f));
                                MountPosition.Y = mount.position.Y + mount.height + 6 + mount.gfxOffY;
                                MountPosition.X += mount.Center.X - 18 * direction;
                                MountPosition.Y += -SpriteHeight + HandPosition.Y;
                            }
                            else
                            {
                                MountPosition.X = mount.Center.X - 12 * direction; //-10
                                MountPosition.X += ((HandPosition.X - Base.SpriteWidth * 0.5f) * direction - 6) * Scale; //-4
                                MountPosition.Y = mount.position.Y + 14 + (HandPosition.Y - Base.SpriteHeight) * Scale + mount.gfxOffY;
                            }
                            if (mount.mount.Active && SitOnMount)
                            {
                                MountPosition.Y += (-mount.mount.PlayerOffset - mount.mount.YOffset);
                            }
                        }
                        else
                        {
                            float MountedOffset = 0;
                            MountPosition.X += -16 * direction + MountedOffset * direction;
                            if (InMineCart)
                            {
                                if (direction < 0) MountPosition.X -= 10;
                                else MountPosition.X -= 2;
                                MountPosition.Y += 8f;
                            }
                            MountPosition.Y += (SitOnMount ? -2 : -14) - 14 - mount.mount.PlayerOffset - mount.mount.YOffset;
                            MountPosition.X += mount.Center.X;
                            MountPosition.Y += mount.position.Y + mount.height;
                        }
                        gfxOffY = 0;
                        position = MountPosition;
                        velocity = Vector2.Zero; //mount.velocity;
                        SetFallStart();
                        ControlJump = false;
                    }
                    break;
                case MountStyles.PlayerMountsOnCompanion:
                    {
                        Player rider = CharacterMountedOnMe;
                        MoveLeft = rider.controlLeft;
                        MoveRight = rider.controlRight;
                        MoveUp = rider.controlUp;
                        MoveDown = rider.controlDown;
                        ControlJump = rider.controlJump;
                        /*if(rider.itemAnimation > 0)
                        {
                            switch(rider.HeldItem.type)
                            {
                                case ItemID.MagicMirror:
                                case ItemID.IceMirror:
                                case ItemID.RecallPotion:
                                    if((rider.Center - Center).Length() >= height + 50)
                                    {
                                        Bottom = rider.Bottom;
                                    }
                                    break;
                            }
                        }*/
                    }
                    break;
            }
        }

        private void UpdateDialogueBehaviour()
        {
            if(Dialogue.InDialogue && Dialogue.IsParticipatingDialogue(this))
            {
                if(IsMountedOnSomething)
                    return;
                if(Behaviour_AttackingSomething)
                {
                    /*if(Dialogue.Speaker == this)
                    {
                        Dialogue.EndDialogue();
                    }*/
                    return;
                }
                Behaviour_InDialogue = true;
                const int DistanceAwayFromPlayer = 20;
                float CenterX = position.X + width * 0.5f;
                float InitialDistance = MainMod.GetLocalPlayer.width * 0.8f + DistanceAwayFromPlayer;
                float WaitLocationX = MainMod.GetLocalPlayer.position.X + MainMod.GetLocalPlayer.width * 0.5f;
                {
                    Companion MountedOn = PlayerMod.PlayerGetMountedOnCompanion(MainMod.GetLocalPlayer);
                    if(MountedOn != null)
                    {
                        WaitLocationX = MountedOn.position.X + MountedOn.width * 0.5f;
                        InitialDistance = MountedOn.width * 0.8f + DistanceAwayFromPlayer;
                    }
                }
                if (UsingFurniture && (direction < 0 && CenterX < WaitLocationX) || (direction > 0 && CenterX > WaitLocationX))
                {
                    LeaveFurniture();
                }
                float WaitDistance = InitialDistance + width * 0.8f + 8;
                if (GoingToOrUsingFurniture)
                {
                    WaitDistance += 40;
                }
                bool ToLeft = false;
                if(CenterX < WaitLocationX)
                {
                    WaitLocationX -= Dialogue.DistancingLeft + WaitDistance * 0.5f + 12;
                    Dialogue.DistancingLeft += WaitDistance;
                    ToLeft = true;
                }
                else
                {
                    WaitLocationX += Dialogue.DistancingRight + WaitDistance * 0.5f + 12;
                    Dialogue.DistancingRight += WaitDistance;
                }
                if((ToLeft && CenterX < WaitLocationX) || (!ToLeft && CenterX > WaitLocationX))
                {
                    if(CenterX < WaitLocationX)
                    {
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                    }
                }
                else
                {
                    if(velocity.X == 0 && velocity.Y == 0)
                    {
                        direction = ToLeft ? 1 : -1;
                    }
                }
            }
        }

        private void LookForTargets()
        {
            if(Target != null && (!Target.active || (Target is Player && ((Player)Target).dead))) Target = null;
            float NearestDistance = 600f;
            Entity NewTarget = null;
            Vector2 MyCenter = Center;
            for (int i = 0; i < 200; i++)
            {
                if(!Main.npc[i].active) continue;
                NPC npc = Main.npc[i];
                if(npc.active && !npc.friendly && npc.CanBeChasedBy(null))
                {
                    float Distance = (MyCenter - npc.Center).Length();
                    if(Distance < NearestDistance && CanHit(npc))
                    {
                        NewTarget = npc;
                        NearestDistance = Distance;
                    }
                }
            }
            if (NewTarget != null)
                Target = NewTarget;
        }

        public void CheckIfNeedToJumpTallTile()
        {
            if(CanDoJumping)
            {
                float MovementDirection = controlLeft ? -1 : controlRight ? 1 : direction;
                int TileX = (int)((Center.X + (width * 0.5f + 1) * MovementDirection) * DivisionBy16);
                int TileY = (int)((Bottom.Y - 1) * DivisionBy16);
                byte BlockedTiles = 0, Gap = 0;
                for(byte i = 0; i < 9; i++)
                {
                    Tile tile = Main.tile[TileX, TileY - i];
                    if(tile.HasTile && Main.tileSolid[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
                    {
                        BlockedTiles++;
                        Gap = 0;
                    }
                    else
                    {
                        if(i == 1)
                            BlockedTiles = 0;
                        Gap++;
                        if(Gap >= 3)
                        {
                            break;
                        }
                    }
                }
                if(BlockedTiles >= 1 && Gap >= 3)
                {
                    controlJump = true;
                }
            }
        }

        public void InitializeCompanion()
        {
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            name = Data.GetName;
            Male = Data.Gender == Genders.Male;
            inventory = Data.Inventory;
            armor = Data.Equipments;
            miscEquips = Data.MiscEquipment;
            dye = Data.EquipDyes;
            miscDyes = Data.MiscEquipDyes;
            statLifeMax = Data.MaxHealth;
            statManaMax = Data.MaxMana;
            buffType = Data.BuffType;
            buffTime = Data.BuffTime;
            DoResetEffects();
            statLife = statLifeMax2;
            statMana = statManaMax2;
            CheckIfHasNpcState();
            idleBehavior = Base.DefaultIdleBehavior;
            combatBehavior = Base.DefaultCombatBehavior;
            followBehavior = Base.DefaultFollowLeaderBehavior;
            preRecruitBehavior = Base.PreRecruitmentBehavior;
            if(this is TerraGuardian) (this as TerraGuardian).OnInitializeTgAnimationFrames();
        }

        public void Teleport(Vector2 Destination)
        {
            position.X = Destination.X - width * 0.5f;
            position.Y = Destination.Y - height;
            fallStart = (int)(position.Y * DivisionBy16);
            immuneTime = 40;
            immuneNoBlink = true;
        }

        public bool AimAtTarget(Vector2 TargetPosition, int TargetWidth, int TargetHeight)
        {
            ChangeAimPosition(new Vector2(TargetPosition.X + TargetWidth * 0.5f, TargetPosition.Y + TargetHeight * 0.5f));
            Vector2 AimLocation = AimDirection + Center;
            return AimLocation.X >= TargetPosition.X && 
                AimLocation.Y >= TargetPosition.Y && 
                AimLocation.X < TargetPosition.X + TargetWidth && 
                AimLocation.Y < TargetPosition.Y + TargetHeight;
        }

        public void ChangeAimPosition(Vector2 NewPosition)
        {
            NewAimDirectionBackup = NewPosition - Center;
        }

        private void UpdateAimMovement()
        {
            if(NewAimDirectionBackup != AimDirection)
            {
                Vector2 Diference = NewAimDirectionBackup - AimDirection;
                float Distance = Diference.Length();
                if(Distance < 1)
                {
                    AimDirection = NewAimDirectionBackup;
                    return;
                }
                float MoveSpeed = 10;
                if(Distance < MoveSpeed)
                    MoveSpeed = Distance * 0.8f;
                Diference.Normalize();
                AimDirection += Diference * MoveSpeed;
            }
        }

        internal void OnSpawnOrTeleport()
        {
            Target = null;
            AimDirection = NewAimDirectionBackup = Vector2.Zero;
            if(this is TerraGuardian)
            {
                TerraGuardian tg = (TerraGuardian)this;
                tg.DeadBodyPosition = Vector2.Zero;
                tg.DeadBodyVelocity = Vector2.Zero;
            }
        }

        ///<summary><para>
        ///Allows making a custom companion head drawing script.
        ///The custom drawing script is used by many of the mod scripts to draw a character head.</para>
        ///<param name="Position">The centered position of the head.</param>
        ///<param name="FacingLeft"> Wether the sprite should be facing left or not.</param>
        ///<param name="Scale"> The scale of the sprite.</param>
        ///<param name="MaxDimension"> If value is higher than 0, the sprite should be downscaled if either width or height is bigger than this value.</param>
        ///</summary>
        ///<returns>Return true if you made a custom head drawing script, to avoid drawing default Terrarian version.</returns>
        public virtual bool DrawCompanionHead(Vector2 Position, bool FacingLeft, float Scale = 1f, float MaxDimension = 0)
        {
            return false;
        }

        public void DrawCompanionInterfaceOnly(DrawContext context = DrawContext.AllParts, bool UseSingleDrawScript = false)
        {
            DoResetEffects();
            ResetVisibleAccessories();
            UpdateMiscCounter();
            UpdateDyes();
            UpdateAnimations();
            DrawCompanion(context, UseSingleDrawScript);
        }

        public virtual void DrawCompanion(DrawContext context = DrawContext.AllParts, bool UseSingleDrawScript = false)
        {
            if (!UseSingleDrawScript) Main.spriteBatch.End();
            LegacyPlayerRenderer renderer = new LegacyPlayerRenderer();
            SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
            TerraGuardianDrawLayersScript.Context = context;
            if(!UseSingleDrawScript)
            {
                renderer.DrawPlayers(Main.Camera, new Player[]{ this });
            }
            else
            {
                renderer.DrawPlayer(Main.Camera, this, position, fullRotation, fullRotationOrigin);
            }
            if (!UseSingleDrawScript) Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true, bool DiscountCharacterDimension = true, bool DiscountDirections = true, bool ConvertToCharacterPosition = true)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if(DiscountDirections && direction < 0)
                Position.X = Base.SpriteWidth - Position.X;
            if(DiscountDirections &&gravDir < 0)
                Position.Y = Base.SpriteHeight - Position.Y;
            Position *= Scale;
            if(ConvertToCharacterPosition)
            {
                if(DiscountCharacterDimension)
                {
                    Position.X += (width - Base.SpriteWidth * Scale) * 0.5f;
                    Position.Y += height - Base.SpriteHeight * Scale;
                }
                else
                {
                    Position.X += Base.SpriteWidth * Scale * 0.5f;
                    Position.Y -= Base.SpriteHeight * Scale;
                }
            }
            if(AlsoTakePosition)
                Position += position + Vector2.UnitY * HeightOffsetHitboxCenter;
            return Position;
        }

        public Vector2 GetBetweenAnimationPosition(AnimationPositions Animation, short Frame, bool AlsoTakePosition = true)
        {
            if(Base.GetHands <= 1)
                return GetAnimationPosition(Animation, Frame, 0, AlsoTakePosition);
            Vector2 OriginPosition = GetAnimationPosition(Animation, Frame, 0, false);
            Vector2 Position = OriginPosition + (GetAnimationPosition(Animation, Frame, 1, false) - OriginPosition) * 0.5f;
            if (AlsoTakePosition)
                Position += position + Vector2.UnitY * HeightOffsetHitboxCenter;
            return Position;
        }

        public void CheckIfHasNpcState()
        {
            foreach(CompanionTownNpcState npcState in WorldMod.CompanionNPCsInWorld)
            {
                if (npcState != null && npcState.CharID.IsSameID(ID, ModID))
                {
                    ChangeTownNpcState(npcState);
                    return;
                }
            }
            ChangeTownNpcState(null);
        }

        public bool IsAtHome
        {
            get
            {
                CompanionTownNpcState tns = GetTownNpcState;
                if(tns == null) return false;
                return tns.IsAtHome(Bottom);
            }
        }

        public bool ToggleMount(Player Target, bool Forced = false)
        {
            if (!Forced && (CCed)) return false;
            bool CharacterMountedIsTarget = Target == CharacterMountedOnMe;
            if(CharacterMountedOnMe != null)
            {
                switch(Base.MountStyle)
                {
                    case MountStyles.CompanionRidesPlayer:
                        position.X = CharacterMountedOnMe.Center.X - width * 0.5f;
                        position.Y = CharacterMountedOnMe.Bottom.Y - height;
                        velocity = CharacterMountedOnMe.velocity;
                        fallStart = fallStart2 = (int)(position.Y * DivisionBy16);
                        CharacterMountedOnMe.GetModPlayer<PlayerMod>().GetCompanionMountedOnMe = null;
                        break;

                    case MountStyles.PlayerMountsOnCompanion:
                        CharacterMountedOnMe.position.X = Center.X - CharacterMountedOnMe.width * 0.5f;
                        CharacterMountedOnMe.position.Y = Bottom.Y - CharacterMountedOnMe.height;
                        CharacterMountedOnMe.velocity = velocity;
                        CharacterMountedOnMe.fallStart = fallStart;
                        CharacterMountedOnMe.GetModPlayer<PlayerMod>().GetMountedOnCompanion = null;
                        break;
                }
                CharacterMountedOnMe = null;
                if (CharacterMountedIsTarget)
                {
                    return true;
                }
            }
            {
                CharacterMountedOnMe = Target;
                PlayerMod TargetModPlayer = Target.GetModPlayer<PlayerMod>();
                switch (Base.MountStyle)
                {
                    case MountStyles.PlayerMountsOnCompanion:
                        if(TargetModPlayer.GetMountedOnCompanion != null)
                            TargetModPlayer.GetMountedOnCompanion.ToggleMount(Target, true);
                        TargetModPlayer.GetMountedOnCompanion = this;
                        break;
                    case MountStyles.CompanionRidesPlayer:
                        if(TargetModPlayer.GetCompanionMountedOnMe != null)
                            TargetModPlayer.GetCompanionMountedOnMe.ToggleMount(Target, true);
                        TargetModPlayer.GetCompanionMountedOnMe = this;
                        break;
                }
                return true;
            }
            //return false;
        }

        public void ChangeTownNpcState(CompanionTownNpcState NewState)
        {
            _TownNpcState = NewState;
        }

        public void DrawOverheadMessage()
        {
            if(chatOverhead.timeLeft > 0 && !dead)
            {
                Vector2 MessageSize = chatOverhead.messageSize;
                Vector2 Position = Top.ToScreenPosition();
                if(gravDir == -1f) Position.Y += 2;
                Position.X -= MessageSize.X * 0.5f;
                Position.Y += gfxOffY - (MessageSize.Y + 2);
                Position = Position.Floor();
                Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, 
                    chatOverhead.snippets, Position, 0, chatOverhead.color, Vector2.Zero, Vector2.One, out int hover);
            }
        }

        public void PlayerMeetCompanion(Player PlayerWhoMetHim)
        {
            WorldMod.AddCompanionMet(Data);
            PlayerMod.PlayerAddCompanion(PlayerWhoMetHim, ID, ModID);
        }

        public CompanionDrawMomentTypes GetDrawMomentType()
        {
            if (Owner != null && (sitting.isSitting || sleeping.isSleeping))
            {
                return CompanionDrawMomentTypes.DrawBehindOwner;
            }
            if (IsMountedOnSomething)
            {
                return CompanionDrawMomentTypes.DrawInBetweenMountedOne;
            }
            if(Owner != null)
            {
                return CompanionDrawMomentTypes.DrawBehindOwner;
            }
            return CompanionDrawMomentTypes.AfterTiles;
        }
    }

    public enum CompanionDrawMomentTypes : byte
    {
        AfterTiles,
        DrawInBetweenMountedOne,
        DrawBehindOwner,
        DrawInBetweenOwner,
        DrawInFrontOfOwner
    }
}