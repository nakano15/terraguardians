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
        public static Companion GetReferedCompanion { get { 
            return ReferedCompanion; } internal set { ReferedCompanion = value; } 
        }
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
        public bool IsHomeless
        {
            get
            {
                if (_TownNpcState != null)
                    return _TownNpcState.Homeless;
                return true;
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
        public RequestData GetRequest
        {
            get
            {
                return Data.GetRequest;
            }
        }
        public CompanionCommonData GetCommonData
        {
            get
            {
                return Data.GetCommonData;
            }
        }
        public CompanionDialogueContainer GetDialogues
        {
            get 
            {
                return Base.GetDialogues;
            }
        }
        public CompanionGroup GetGroup
        {
            get
            {
                return Base.GetCompanionGroup;
            }
        }
        public Genders Genders
        {
            get
            {
                return Data.Gender;
            }
        }
        public byte FriendshipLevel
        {
            get
            {
                return Data.FriendshipLevel;
            }
        }
        public sbyte FriendshipExp
        {
            get
            {
                return Data.FriendshipExp;
            }
        }
        public byte FriendshipMaxExp
        {
            get
            {
                return Data.FriendshipMaxExp;
            }
        }
        public float GetFriendshipProgress
        {
            get
            {
                if (FriendshipMaxExp <= 1) return 1;
                return (float)FriendshipExp / (FriendshipMaxExp - 1);
            }
        }
        public KnockoutStates KnockoutStates
        {
            get
            {
                return GetPlayerMod.KnockoutState;
            }
            set
            {
                GetPlayerMod.KnockoutState = value;
            }
        }
        public MountStyles MountStyle
        {
            get
            {
                if (TitanCompanion && Base.MountStyle == MountStyles.CompanionRidesPlayer) //Need to add support for small companions to let player mount on their shoulders, when they're giant.
                    return MountStyles.PlayerMountsOnCompanion;
                return Base.MountStyle;
            }
        }
        public CombatTactics CombatTactic { get { return Data.CombatTactic; } set { Data.CombatTactic = value; }}
        public CompanionID GetCompanionID { get { return Data.GetMyID; } }
        public uint ID { get { return Data.ID; } }
        public string ModID { get { return Data.ModID; } }
        public uint Index { get{ return Data.Index; } }
        public bool HasBeenMet { get { return WorldMod.HasMetCompanion(Data); }}
        public bool IsPlayerCharacter = false;
        public byte OutfitID { get { return Data.OutfitID; } set { Data.OutfitID = value; } }
        public byte SkinID { get { return Data.SkinID; } set { Data.SkinID = value; } }
        public Player Owner = null;
        public Vector2 GetCollisionPosition
        {
            get
            {
                return new Vector2(position.X + width * 0.5f - 10, position.Y + height - defaultHeight);
            }
        }
        public PathFinder Path = new PathFinder();
        public float SpriteWidth { get{ return Base.SpriteWidth * Scale; }}
        public float SpriteHeight { get{ return Base.SpriteHeight * Scale; }}
        #region Useful getter setters
        public int Health { get { return statLife; } set { statLife = value; } }
        public int MaxHealth { get { return statLifeMax2; } set { statLifeMax2 = value; } }
        public int Mana { get { return statMana; } set { statMana = value; } }
        public int MaxMana { get { return statManaMax2; } set { statManaMax2 = value; } }
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
            preRecruitBehavior = null,
            temporaryBehavior = null;
        public ReviveBehavior reviveBehavior = new ReviveBehavior();
        #endregion
        protected int furniturex = -1, furniturey = -1;
        protected bool reachedfurniture = false;
        public int GetFurnitureX { get{ return furniturex; } }
        public int GetFurnitureY { get{ return furniturey; } }
        public bool GetReachedFurniture { get { return reachedfurniture; } }
        public bool GoingToOrUsingFurniture { get { return furniturex > -1 && furniturey > -1; } }
        public bool UsingFurniture { get { return furniturex > -1 && furniturey > -1 && reachedfurniture; } }
        public bool IsUsingToilet { get { return UsingFurniture && Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Toilets; } }
        public bool IsUsingBed { get { return UsingFurniture && Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Beds; } }
        public bool IsUsingThroneOrBench { get { return UsingFurniture && (Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Thrones || Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Benches); } }
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
        public bool IsBeingControlledBySomeone { get { return CharacterControllingMe != null; } }
        public bool IsBeingControlledBy(Player player) { return CharacterControllingMe == player; }
        public bool IsMountedOnSomething { get { return CharacterMountedOnMe != null; } }
        public Player GetCharacterMountedOnMe { get { return CharacterMountedOnMe; }}
        public bool CompanionHasControl { get { return CharacterMountedOnMe == null || (CharacterMountedOnMe != null && (PlayerMod.GetPlayerKnockoutState(CharacterMountedOnMe) > KnockoutStates.KnockedOut || !PlayerMod.IsPlayerCharacter(CharacterMountedOnMe))); }}
        public Player GetCharacterControllingMe { get { return CharacterControllingMe; }}
        private Player CharacterMountedOnMe = null, CharacterControllingMe = null;
        public bool WalkMode = false;
        public float Scale = 1f;
        public float FinalScale = 1f;
        public bool Crouching { get{ return MoveDown && velocity.Y == 0; } set { MoveDown = value; } }
        public Entity Target = null;
        public bool IsStarter { get { return Data.IsStarter; } }
        public float GetHealthScale { get { return Base.HealthScale; } }
        public float GetManaScale { get { return Base.ManaScale; } }
        public bool IsFollower { get{ return Owner != null; }}
        public bool TargettingSomething { get { return Target != null; } }
        public string GetName { get { return GetGoverningBehavior().CompanionNameChange(this); }}
        public string GetRealName { get { return Data.GetRealName; }}
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
        public bool GoHomeTime
        {
            get
            {
                if (Main.eclipse || Main.bloodMoon) return false;
                if (!Base.IsNocturnal)
                {
                    return !Main.dayTime || (Main.dayTime && Main.time < 5400);
                }
                else
                {
                    return Main.dayTime && Main.time >= 5400 && Main.time < 48600;
                }
            }
        }
        public float DefenseRate = 0;
        public float BlockRate = 0;
        public float DodgeRate = 0;
        public bool IsSleeping { get { return sleeping.isSleeping; } }
        private byte TriggerStack = 0;
        private byte AppliedFoodLevel = 0;
        internal bool TitanCompanion = false;
        public FollowOrderSetting FollorOrder = new FollowOrderSetting(); 
        public byte GetAppliedFoodLevel { get { return AppliedFoodLevel; } }
        public short[] ArmFramesID = new short[0], ArmFrontFramesID = new short[0];
        public short BodyFrameID = 0, BodyFrontFrameID = -1;
        #region Flags for ease of using AI
        private BitsByte _statsValues = 0;
        public bool IsHungry { get { return _statsValues[0]; } set { _statsValues[0] = value; } }
        public bool IsSober { get { return _statsValues[1]; } set { _statsValues[1] = value; } }
        private BitsByte _accessoryMemory = 0, _accessoryMemory2 = 0;
        public bool HasDoubleJumpBottleAbility { get { return _accessoryMemory[0]; } set { _accessoryMemory[0] = value; } }
        public bool HasExtraJumpAbility { get { return _accessoryMemory[1]; } set { _accessoryMemory[1] = value; } }
        public bool HasSwimmingAbility { get { return _accessoryMemory[2]; } set { _accessoryMemory[2] = value; } }
        public bool HasWallClimbingAbility { get { return _accessoryMemory[3]; } set { _accessoryMemory[3] = value; } }
        public bool HasWaterbreathingAbility { get { return _accessoryMemory[4]; } set { _accessoryMemory[4] = value; } }
        public bool HasFlightAbility { get { return _accessoryMemory[5]; } set { _accessoryMemory[5] = value; } }
        public bool HasWaterWalkingAbility { get { return _accessoryMemory[6]; } set { _accessoryMemory[6] = value; } }
        public bool HasFallDamageImmunityAbility { get { return _accessoryMemory[7]; } set { _accessoryMemory[7] = value; } }
        public bool HasLavaImmunityAbility { get { return _accessoryMemory2[0]; } set { _accessoryMemory2[0] = value; } }
        public bool HasGravityFlippingAbility { get { return _accessoryMemory2[1]; } set { _accessoryMemory2[1] = value; } }
        public bool HasWeaponEnchanted { get { return _accessoryMemory2[2]; } set { _accessoryMemory2[2] = value; } }
        public bool HasFeatherfallAbility { get { return _accessoryMemory2[3]; } set { _accessoryMemory2[3] = value; } }
        public bool HasRunningAbility { get { return _accessoryMemory2[4]; } set { _accessoryMemory2[4] = value; } }
        public bool HasDashingdodgeAbility { get { return _accessoryMemory2[5]; } set { _accessoryMemory2[5] = value; } }
        public bool HasIceSkatesAbility { get { return _accessoryMemory2[6]; } set { _accessoryMemory2[6] = value; } }
        #endregion
        #region Permissions
        public bool ShareChairWithPlayer { get { return Data.ShareChairWithPlayer; } set { Data.ShareChairWithPlayer = value; } }
        public bool ShareBedWithPlayer { get { return Data.ShareBedWithPlayer; } set { Data.ShareBedWithPlayer = value; } }
        #endregion
        public bool PlayerSizeMode { get { return Data.PlayerSizeMode; } set { Data.PlayerSizeMode = value; } }

        public string GetPlayerNickname(Player player)
        {
            return Data.GetPlayerNickname(player);
        }

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner.whoAmI == Main.myPlayer) || (Main.netMode == 2 && Owner == null);
            }
        }

        public bool CanJump
        {
            get
            {
                return (velocity.Y == 0 || sliding) && releaseJump;
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

        public static bool Behavior_FollowingPath
        {
            get
            {
                return _Behaviour_Flags[3];
            }
            set
            {
                _Behaviour_Flags[3] = value;
            }
        }

        public static bool Behavior_RevivingSomeone
        {
            get
            {
                return _Behaviour_Flags[4];
            }
            set
            {
                _Behaviour_Flags[4] = value;
            }
        }

        public bool CanInviteOver(Player player)
        {
            if (MainMod.DebugMode) return true;
            return FriendshipLevel >= Base.GetFriendshipUnlocks.InviteUnlock;
        }

        public bool CanTakeRequests(Player player) { 
            if (MainMod.DebugMode || PlayerMod.GetIsPlayerBuddy(player, this)) return true;
            return FriendshipLevel >= Base.GetFriendshipUnlocks.RequestUnlock; 
        }
        

        public bool PlayerCanMountCompanion(Player player)
        {
            if (MainMod.DebugMode || PlayerMod.GetIsPlayerBuddy(player, this)) return true;
            if(Owner == player)
            {
                return FriendshipLevel >= Base.GetFriendshipUnlocks.MountUnlock;
            }
            return false;
        }

        public bool PlayerCanControlCompanion(Player player)
        {
            if (MainMod.DebugMode || PlayerMod.GetIsPlayerBuddy(player, this)) return true;
            if(Owner == player && this is TerraGuardian)
            {
                return FriendshipLevel >= Base.GetFriendshipUnlocks.ControlUnlock;
            }
            return false;
        }
        
        public void SaySomethingOnChat(string Message, Color? color = null)
        {
            Companion LastSpeaker = Dialogue.Speaker;
            Dialogue.Speaker = this;
            string NewMessage = "<" + GetNameColored() + "> ";
            NewMessage += Dialogue.ParseText(Message);
            Main.NewText(NewMessage, color);
            Dialogue.Speaker = LastSpeaker;
        }

        public void SaySomething(string Text, bool ShowOnChat = false)
        {
            chatOverhead.NewMessage(Text, 240 + Text.Length);
            if (ShowOnChat)
            {
                SaySomethingOnChat(Text);
            }
        }

        public void SaySomethingAtRandom(string[] Text)
        {
            if (Text.Length > 0)
            {
                string Mes = Text[Main.rand.Next(Text.Length)];
                chatOverhead.NewMessage(Mes, 180 + Mes.Length);
            }
        }

        public void SetFallStart()
        {
            fallStart = fallStart2 = (int)(position.Y * DivisionBy16);
        }

        public BehaviorBase GetGoverningBehavior()
        {
            if (temporaryBehavior != null && temporaryBehavior.IsActive)
                return temporaryBehavior;
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

        public void RunBehavior(BehaviorBase NewBehavior)
        {
            if (temporaryBehavior != null)
                temporaryBehavior.OnEnd();
            temporaryBehavior = NewBehavior;
            NewBehavior.SetOwner(this);
            temporaryBehavior.OnBegin();
        }

        public bool IsRunningBehavior{ get{ return temporaryBehavior != null && temporaryBehavior.IsActive; } }

        public void CancelBehavior()
        {
            if (temporaryBehavior != null)
                temporaryBehavior.OnEnd();
            temporaryBehavior = null;
        }

        public string GetPronoun()
        {
            switch(Data.Gender)
            {
                case Genders.Male:
                    return "him";
                case Genders.Female:
                    return "her";
            }
            return "them";
        }

        public bool CreatePathingTo(Vector2 Destination, bool WalkToPath = false, bool StrictPath = true, bool CancelOnFail = false)
        {
            return CreatePathingTo((int)(Destination.X * DivisionBy16), (int)(Destination.Y * DivisionBy16), WalkToPath, StrictPath, CancelOnFail);
        }

        public bool CreatePathingTo(int X, int Y, bool WalkToPath = false, bool StrictPath = true, bool CancelOnFail = false)
        {
            byte Attempts = 0;
            const byte MaxAttempts = 8;
            while (true)
            {
                bool HasSolidGround = false, HasSolidTile = false;
                Tile tile = Main.tile[X, Y];
                if (tile == null) return false;
                if (PathFinder.CheckForSolidBlocks(X, Y))
                {
                    Y --;
                    Attempts++;
                    HasSolidGround = true;
                }
                else if (!PathFinder.CheckForSolidGroundUnder(X, Y))
                {
                    Y++;
                }
                else
                {
                    HasSolidGround = true;
                }
                if (!HasSolidGround && !HasSolidTile)
                {
                    Attempts++;
                }
                if (HasSolidGround && !HasSolidTile) break;
                if (Attempts >= MaxAttempts)
                {
                    return false;
                }
            }
            if (!PathFinder.CheckForSolidBlocks(X, Y))
            {
                return Path.CreatePathTo(Bottom, X, Y, (int)((Base.JumpHeight * jumpSpeed) * DivisionBy16), GetFallTolerance, WalkToPath, StrictPath, CancelOnFail);
            }
            return false;
        }

        public void UpdateBehaviour()
        {
            _Behaviour_Flags = 0;
            if (!Is2PCompanion)
                MoveLeft = MoveRight = MoveUp = ControlJump = controlUseItem = false;
            if (KnockoutStates > KnockoutStates.Awake) return;
            if(!Base.CanCrouch || itemAnimation == 0)
                MoveDown = false;
            bool ControlledByPlayer = IsBeingControlledBySomeone;
            BehaviorBase Behavior = GetGoverningBehavior();
            Base.UpdateBehavior(this);
            if (!Is2PCompanion)
            {
                if (Behavior.AllowSeekingTargets) LookForTargets();
                if (!ControlledByPlayer && Behavior.UseHealingItems) CheckForItemUsage();
            }
            if (Behavior.RunCombatBehavior) combatBehavior.Update(this);
            UpdateDialogueBehaviour();
            if (!Is2PCompanion)
            {
                if(ControlledByPlayer)
                {
                    UpdateControlledBehavior();
                }
                else
                {
                    if (IsMountedOnSomething)
                        UpdateMountedBehavior();
                }
                FollowPathingGuide();
            }
            UpdateFurnitureUsageScript();
            if(!Is2PCompanion && !ControlledByPlayer && !Behaviour_AttackingSomething)
                ChangeAimPosition(Center + Vector2.UnitX * width * direction);
            if (Behavior.AllowRevivingSomeone) reviveBehavior.Update(this);
            Behavior.Update(this);
            if(MoveLeft || MoveRight)
            {
                CheckIfNeedToJumpTallTile();
            }
            if (!Is2PCompanion && !ControlledByPlayer)
            {
                CheckForCliffs();
                CheckForFallDamage();
            }
            //OffhandHeldAction();
        }

        private void OffhandHeldAction() //Script for offhand items - Needs work
        {
            if (this is TerraGuardian)
            {
                TerraGuardian tg = this as TerraGuardian;
                byte HoldArm = (byte)Math.Min(ArmFramesID.Length - 1, 1);
                TerraGuardian.HeldItemSetting held = tg.HeldItems[HoldArm];
                if(controlTorch && (HoldArm != 0 || !controlUseItem))
                {
                    if(HoldArm == 0)
                        held.SetSettings(tg);
                    using(TerraGuardian.ItemMask mask = new TerraGuardian.ItemMask(tg, HoldArm))
                    {
                        SmartSelectLookup();
                    }
                }
            }
            else
            {
                if (controlTorch && selectedItem != 58)
                    SmartSelectLookup();
            }
        }

        private void CheckForCliffs()
        {
            if (GetCharacterMountedOnMe != null || Behavior_FollowingPath) return;
            if (velocity.Y != 0) return;
            float Movement = velocity.X;
            if (Movement == 0)
            {
                if (controlRight)
                    Movement += runAcceleration;
                else if(controlLeft)
                    Movement -= runAcceleration;
            }
            if(Movement == 0)
            {
                return;
            }
            const int CheckAheadDistance = 5;
            int Direction = Movement > 0 ? 1 : -1;
            int CheckStart = (int)((Center.X + (width * 0.5f + 1) * Direction) * DivisionBy16);
            int CheckYFoot = (int)(Bottom.Y * DivisionBy16);
            bool Avoid = false;
            int AvoidRange = 0;
            byte HoleRange = 0;
            int RangeY = Math.Min(12, Base.FallHeightTolerance);
            for (int x = 0; x < CheckAheadDistance; x++)
            {
                int CheckX = CheckStart + x * Direction;
                bool HasTrap = false;
                bool HasSolidTile = false;
                int Liquid = 0;
                byte LiquidTiles = 0;
                byte LastCheckedYRange = 0;
                for(byte y = 0; y < RangeY; y++)
                {
                    int CheckY = CheckYFoot + y;
                    LastCheckedYRange = y;
                    if (WorldGen.InWorld(CheckX, CheckY))
                    {
                        Tile tile = Main.tile[CheckX, CheckY];
                        if (tile.LiquidType > 0 && tile.LiquidAmount > 50)
                        {
                            Liquid = tile.LiquidType;
                            LiquidTiles++;
                            if (LiquidTiles >= 3)break;
                        }
                        if(tile.HasTile && !tile.IsActuated)
                        {
                            switch(tile.TileType)
                            {
                                case TileID.Spikes:
                                case TileID.WoodenSpikes:
                                case TileID.LandMine:
                                    HasTrap = true;
                                    break;
                            }
                            if (HasTrap)
                                break;
                            if (Main.tileSolid[tile.TileType])
                            {
                                HasSolidTile = true;
                                break;
                            }
                        }
                    }
                }
                if (HasTrap)
                {
                    Avoid = true;
                    AvoidRange = x;
                }
                if (Liquid > 0)
                {
                    if(Liquid == 1 && LiquidTiles >= 3 && !HasSwimmingAbility)
                    {
                        Avoid = true;
                        AvoidRange = x;
                        break;
                    }
                    if (Liquid == 2)
                    {
                        Avoid = true;
                        AvoidRange = x;
                        break;
                    }
                }
                if (!HasSolidTile)
                {
                    HoleRange++;
                    if (HoleRange >= 2)
                    {
                        Avoid = true;
                        AvoidRange = x;
                        break;
                    }
                }
                else
                {
                    CheckYFoot += LastCheckedYRange;
                }
                /*else if (HasSolidTile && HoleRange < 2)
                {
                    Avoid = true;
                    break;
                }*/
            }
            if (Avoid)
            {
                float Range = Math.Abs((CheckStart + AvoidRange * Direction) * 16 + 8 * direction);
                if (Range >= width * 0.5f + 10)
                {
                    bool TooClose = Range < width * 0.5f;
                    if (Direction > 0)
                    {
                        if (TooClose) MoveLeft = true;
                        MoveRight = false;
                    }
                    else
                    {
                        if (TooClose) MoveRight = true;
                        MoveLeft = false;
                    }
                }
            }
        }

        private void CheckForFallDamage()
        {
            if (Behavior_FollowingPath || HasFallDamageImmunityAbility) return;
            if ((int)(position.Y * DivisionBy16) - fallStart >= GetFallTolerance)
            {
                int CheckStartX = (int)((position.X + width * 0.5f - 10) * DivisionBy16), CheckEndX = (int)((position.X + width * 0.5f + 10) * DivisionBy16);
                int CheckY = (int)((position.Y + velocity.Y + height) * DivisionBy16 + 1);
                bool DoJump = false;
                for(int x = CheckStartX; x <= CheckEndX; x++)
                {
                    if(WorldGen.InWorld(x, CheckY))
                    {
                        Tile tile = Main.tile[x, CheckY];
                        if(tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType])
                        {
                            DoJump = true;
                            break;
                        }
                    }
                }
                if (DoJump)
                {
                    if (HasDoubleJumpBottleAbility)
                        ControlJump = true;
                    else if(Owner != null)
                        Teleport(Owner.Bottom);
                }
            }
        }

        public bool FollowPathingGuide()
        {
            if (Path.State != PathFinder.PathingState.TracingPath || Behaviour_InDialogue) return false;
            if (Behaviour_AttackingSomething)
            {
                Path.PathingInterrupted = true;
                return false;
            }
            if (Path.PathingInterrupted)
            {
                Path.PathingInterrupted = false;
                if (Path.SavedPosX > -1 && Path.SavedPosY > -1)
                {
                    if (!Path.ResumePathingTo(Bottom, (int)((jumpHeight * jumpSpeed) * DivisionBy16), GetFallTolerance))
                        return false;
                }
                else
                {
                    Path.ClearPath();
                    return false;
                }
            }
            WalkMode = Path.WalkToPath;
            if (Path.CheckStuckTimer())
            {
                if (Path.CancelOnFail)
                    Path.CancelPathing();
                else
                    Path.ResumePathingTo(Bottom, (int)((jumpHeight * jumpSpeed) * DivisionBy16), GetFallTolerance);
                return false;
            }
            PathFinder.Breadcrumb checkpoint = Path.GetLastNode;
            bool ReachedNode = false;
            Vector2 Position = Bottom - Vector2.UnitY * 2;
            switch(checkpoint.NodeOrientation)
            {
                case PathFinder.Node.NONE:
                    ReachedNode = true;
                    break;
                case PathFinder.Node.DIR_UP:
                    {
                        float X = checkpoint.X * 16, Y = (checkpoint.Y + 1) * 16;
                        if (Math.Abs(Position.X - X) > 4)
                        {
                            if (Position.X < X)
                                MoveRight = true;
                            else
                                MoveLeft = true;
                        }
                        else if (Position.Y > Y) //Stairs...
                        {
                            if (CanJump || jumpHeight > 0)
                                ControlJump = true;
                        }
                        else
                        {
                            if(velocity.Y == 0 && Position.Y < Y - 8)
                            {
                                MoveDown = true;
                                if (this is TerraGuardian)
                                    ControlJump = true;
                            }
                            else
                            {
                                ReachedNode = true;
                            }
                        }
                        Path.IncreaseStuckTimer();
                    }
                    break;
                case PathFinder.Node.DIR_RIGHT:
                case PathFinder.Node.DIR_LEFT:
                    {
                        float X = checkpoint.X * 16 + 8;
                        if (Math.Abs(Position.X - X) < 10)
                        {
                            ReachedNode = true;
                        }
                        else
                        {
                            if (Position.X < X)
                                MoveRight = true;
                            else
                                MoveLeft = true;
                            if (velocity.X == 0 && velocity.Y == 0)
                                Path.IncreaseStuckTimer();
                        }
                    }
                    break;
                case PathFinder.Node.DIR_DOWN:
                    {
                        float X = checkpoint.X * 16, Y = checkpoint.Y * 16;
                        /*if (Math.Abs(Position.X - X) > 4)
                        {
                            if (Position.X < X) MoveRight = true;
                            else MoveLeft = true;
                            if (Path.StrictPathFinding) break;
                        }*/
                        if (Position.Y < Y + 8)
                        {
                            if (velocity.Y == 0)
                            {
                                if (!PathFinder.CheckForPlatform(Bottom, width))
                                {
                                    if (Position.X < X) MoveRight = true;
                                    else MoveLeft = true;
                                }
                                else
                                {
                                    MoveDown = true;
                                    if (this is TerraGuardian)
                                        ControlJump = true;
                                }
                            }
                        }
                        else
                        {
                            ReachedNode = true;
                        }
                    }
                    break;
            }
            if (ReachedNode)
            {
                Path.RemoveLastNode();
                Path.ResetStuckTimer();
            }
            Behavior_FollowingPath = true;
            return true;
        }

        public bool IsHostileTo(Player otherPlayer)
        {
            return GetGoverningBehavior().IsHostileTo(otherPlayer);
        }

        internal static void ResetLastID()
        {
            LastWhoAmID = 0;
        }

        public bool IsComfortPointsMaxed()
        {
            return Data.FriendshipProgress.IsComfortMaxed();
        }

        public void CheckForItemUsage()
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
            if (Main.rand.Next(5) == 0 && IsLocalCompanion && !CompanionInventoryInterface.IsInterfaceOpened)
            {
                byte StrongestFoodPosition = 255;
                byte StrongestFoodValue = 0;
                byte StatusIncreaseItem = 255;
                for (byte i = 0; i < 50; i++)
                {
                    if (inventory[i].type > 0)
                    {
                        if (inventory[i].buffType > 0)
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
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (!inventory[i].newAndShiny && !inventory[i].favorited)
                            {
                                switch(inventory[i].type)
                                {
                                    case ItemID.LifeCrystal:
                                        if(statLifeMax < 400)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.LifeFruit:
                                        if(statLifeMax >= 400 && statLifeMax < 500)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.ManaCrystal:
                                        if(statManaMax < 200)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.DemonHeart:
                                        if(!extraAccessory)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                if(StatusIncreaseItem < 255)
                {
                    selectedItem = StatusIncreaseItem;
                    controlUseItem = true;
                    Behavior_UsingPotion = true;
                    return;
                }
                if(StrongestFoodPosition < 255)
                {
                    selectedItem = StrongestFoodPosition;
                    controlUseItem = true;
                    Behavior_UsingPotion = true;
                    return;
                }
            }
        }

        public void SpawnEmote(int BubbleID, int Time)
        {
            Terraria.GameContent.UI.EmoteBubble.NewBubble(BubbleID, new Terraria.GameContent.UI.WorldUIAnchor((Entity)this), Time);
        }

        public void UpdateExtra()
        {
            UpdateComfortStack();
            if (Owner != null && velocity.X != 0)
            {
                Data.FriendshipProgress.IncreaseTravellingStack(velocity.X);
            }
        }

        public void IncreaseFriendshipPoint(sbyte Change)
        {
            Data.FriendshipProgress.ChangeFriendshipProgress(Change);
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
                default:
                    ComfortSum += 0.001f;
                    break;
                case 1:
                    ComfortSum += 0.00125f;
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

        public bool AttackTrigger()
        {
            int NewTriggerStack = TriggerStack + (int)((0.5f + Main.rand.NextFloat() * 0.5f) * Trigger);
            if(NewTriggerStack >= 100)
            {
                TriggerStack = 0;
                return true;
            }
            TriggerStack = (byte)NewTriggerStack;
            return false;
        }

        public bool DoTryAttacking()
        {
            if(AttackTrigger())
            {
                ControlAction = true;
                return true;
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
                            Path.CancelPathing();
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
            /*else
            {
                Vector2 Bottom = this.Bottom;
                bool ImproperTimeToTakeFurniture = Main.raining || !Main.dayTime || Main.eclipse || Main.slimeRain;
                for(int n = 0; n < 200; n++)
                {
                    NPC npc = Main.npc[n];
                    if(npc.active && npc.townNPC && npc.aiStyle == 7 && npc.ai[0] == 5 && npc.Bottom == Bottom)
                    {
                        if (ImproperTimeToTakeFurniture)
                        {
                            LeaveFurniture();
                        }
                        else
                        {
                            npc.ai[1] = 0;
                        }
                    }
                }
            }*/
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
                    if (!Teleport)
                    {
                        CreatePathingTo(x, y, true);
                    }
                    else
                    {
                        position.X = furniturex * 16 - width * 0.5f;
                        position.Y = (furniturey + 1) * 16 - height;
                        SetFallStart();
                    }
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

        private void UpdateControlledBehavior()
        {
            if (CharacterControllingMe == null) return;
            Player p = CharacterControllingMe;
            if(dead)
            {
                TogglePlayerControl(p, true);
                p.KillMe(PlayerDeathReason.ByCustomReason(p.name + " lost their life alongside " + name + "."), 999, 0);
                return;
            }
            MoveUp = p.controlUp;
            MoveRight = p.controlRight;
            MoveDown = p.controlDown;
            MoveLeft = p.controlLeft;
            controlJump = p.controlJump;
            controlUseItem = p.controlUseItem;
            controlUseTile = p.controlUseTile;
            selectedItem = p.selectedItem;
            controlQuickHeal = p.controlQuickHeal;
            controlQuickMana = p.controlQuickMana;
            controlHook = p.controlHook;
            GetAimedPosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            WalkMode = false;
            p.gfxOffY = 0;
            if(GoingToOrUsingFurniture)
            {
                if (MoveUp || MoveRight || MoveDown || MoveLeft)
                {
                    LeaveFurniture();
                }
            }
            if (Path.State == PathFinder.PathingState.TracingPath)
            {
                if (MoveUp || MoveRight || MoveDown || MoveLeft || ControlJump)
                {
                    Path.CancelPathing();
                }
            }
        }

        internal void UpdateMountedBehavior()
        {
            if(CharacterMountedOnMe == null || (MountStyle != MountStyles.CompanionRidesPlayer && !PlayerMod.IsPlayerCharacter(CharacterMountedOnMe)) || CompanionHasControl) return;
            if(GoingToOrUsingFurniture)
            {
                if(CharacterMountedOnMe.controlUp || CharacterMountedOnMe.controlDown || CharacterMountedOnMe.controlLeft || CharacterMountedOnMe.controlRight || CharacterMountedOnMe.controlJump)
                {
                    LeaveFurniture();
                }
                else
                {
                    return;
                }
            }
            if(Path.State == PathFinder.PathingState.TracingPath && MountStyle == MountStyles.PlayerMountsOnCompanion)
            {
                if(CharacterMountedOnMe.controlUp || CharacterMountedOnMe.controlDown || CharacterMountedOnMe.controlLeft || CharacterMountedOnMe.controlRight || CharacterMountedOnMe.controlJump)
                {
                    Path.CancelPathing();
                }
                else
                {
                    return;
                }
            }
            switch(MountStyle)
            {
                case MountStyles.CantMount:
                    {
                        ToggleMount(CharacterMountedOnMe, true);
                    }
                    return;
                case MountStyles.CompanionRidesPlayer:
                    {
                        MoveLeft = MoveRight = MoveUp = ControlJump = false;
                        Player mount = CharacterMountedOnMe;
                        if(mount.dead || tongued)
                        {
                            ToggleMount(mount, true);
                            return;
                        }
                        if(itemAnimation == 0)
                        {
                            direction = mount.direction;
                        }
                        bool InMineCart = mount.mount.Active && MountID.Sets.Cart[mount.mount.Type];
                        Vector2 MountPosition = GetAnimationPosition(AnimationPositions.SittingPosition, BodyFrameID, AlsoTakePosition: false);
                        //Implement the rest later.
                        if (!(mount is Companion)) MountPosition.X += SpriteWidth * 0.5f * direction;
                        /*if(!InMineCart && direction == -1)
                            MountPosition.X = SpriteWidth - MountPosition.X;
                        MountPosition.X = SpriteWidth * 0.5f - MountPosition.X;*/
                        bool SitOnMount = false;
                        //MountPosition.Y = SpriteHeight - MountPosition.Y;
                        if (!InMineCart)
                        {
                            short Frame = Base.GetAnimation(AnimationTypes.SittingFrames).GetFrame(0);
                            AnimationPositionCollection HandPositionCollection = Base.GetAnimationPosition(AnimationPositions.HandPosition);
                            if (mount is Companion)
                            {
                                Companion m = mount as Companion;
                                Vector2 SittingPosition = m.GetAnimationPosition(AnimationPositions.MountShoulderPositions, m.BodyFrameID);
                                MountPosition.X = SittingPosition.X - MountPosition.X;
                                MountPosition.Y = SittingPosition.Y - MountPosition.Y + mount.gfxOffY;// - SpriteHeight;
                            }
                            else
                            {
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
                                    if (direction < 0) MountPosition.X -= 4 * Scale;
                                    MountPosition.Y = mount.position.Y + 14 + (HandPosition.Y - Base.SpriteHeight) * Scale + mount.gfxOffY;
                                }
                            }
                            if (mount.mount.Active && SitOnMount)
                            {
                                MountPosition.Y += (-mount.mount.PlayerOffset - mount.mount.YOffset);
                            }
                            if (mount.sitting.isSitting)
                            {
                                MountPosition += mount.sitting.offsetForSeat;
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
                        if (mount.whoAmI > whoAmI) position += mount.velocity;
                        Companion PlayerMount = PlayerMod.PlayerGetMountedOnCompanion(mount);
                        /*if (PlayerMount != null)
                        {
                            position += PlayerMount.velocity;
                        }*/
                        velocity = Vector2.Zero; //mount.velocity;
                        SetFallStart();
                        ControlJump = false;
                    }
                    break;
                case MountStyles.PlayerMountsOnCompanion:
                    {
                        Player rider = CharacterMountedOnMe;
                        if (rider.dead)
                        {
                            ToggleMount(rider, true);
                            return;
                        }
                        MoveLeft = rider.controlLeft;
                        MoveRight = rider.controlRight;
                        MoveUp = rider.controlUp;
                        MoveDown = rider.controlDown;
                        ControlJump = rider.controlJump;
                        WalkMode = false;
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

        public void PlayerPetCompanion(Player player)
        {
            if (player.GetModPlayer<PlayerMod>().StartInteraction(MountStyle == MountStyles.CompanionRidesPlayer ? InteractionTypes.PettingAlternative : InteractionTypes.Petting))
            {
                FriendshipSystem.PettingAnnoyanceState Level;
                if (Data.FriendshipProgress.TriggerPettingFriendship(out Level))
                {
                    IncreaseFriendshipPoint((sbyte)(PlayerMod.GetIsPlayerBuddy(player, this) ? 2 : 1));
                }
                switch(Level)
                {
                    case FriendshipSystem.PettingAnnoyanceState.Liking:
                        SpawnEmote(Terraria.GameContent.UI.EmoteID.EmotionLove, 150);
                        break;
                    case FriendshipSystem.PettingAnnoyanceState.NotLiking:
                        SpawnEmote(Terraria.GameContent.UI.EmoteID.DebuffSilence, 150);
                        break;
                    case FriendshipSystem.PettingAnnoyanceState.GettingAnnoyed:
                        SpawnEmote(Terraria.GameContent.UI.EmoteID.EmotionAnger, 150);
                        break;
                    case FriendshipSystem.PettingAnnoyanceState.Hating:
                        SpawnEmote(Terraria.GameContent.UI.EmoteID.EmoteAnger, 150);
                        break;
                }
            }
        }

        private void UpdateDialogueBehaviour()
        {
            if(!Dialogue.InDialogue || !Dialogue.IsParticipatingDialogue(this)) return;
            if(IsBeingControlledBySomeone || !CompanionHasControl)
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
            if(sleeping.isSleeping)
            {
                if(Math.Abs(CenterX - WaitLocationX) >= 90 + width)
                    Dialogue.EndDialogue();
                return;
            }
            if (UsingFurniture)
            {
                if (!MainMod.GetLocalPlayer.sitting.isSitting && !IsBeingControlledBySomeone && ((direction < 0 && CenterX < WaitLocationX) || (direction > 0 && CenterX > WaitLocationX)))
                    LeaveFurniture();
                else
                    return;
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
            if(!Is2PCompanion && (ToLeft && CenterX < WaitLocationX) || (!ToLeft && CenterX > WaitLocationX))
            {
                if(CenterX < WaitLocationX)
                {
                    MoveRight = true;
                }
                else
                {
                    MoveLeft = true;
                }
                WalkMode = Math.Abs(CenterX - WaitLocationX) < width + 30;
            }
            else
            {
                if(velocity.X == 0 && velocity.Y == 0)
                {
                    direction = ToLeft ? 1 : -1;
                }
            }
        }

        public void LookForTargets()
        {
            if(Target != null && (!Target.active || (Target is Player && (((Player)Target).dead || !IsHostileTo((Player)Target)))))
                Target = null;
            float NearestDistance = 600f;
            Entity NewTarget = null;
            Vector2 MyCenter = Center; //It's focusing on player only.
            for (int i = 0; i < 255; i++)
            {
                if (i < 200 && GetGoverningBehavior().CanTargetNpcs && Main.npc[i].active)
                {
                    NPC npc = Main.npc[i];
                    if(!npc.friendly && npc.CanBeChasedBy(null))
                    {
                        float Distance = (MyCenter - npc.Center).Length();
                        if(Distance < NearestDistance && CanHit(npc))
                        {
                            NewTarget = npc;
                            NearestDistance = Distance;
                        }
                    }
                }
                if(Main.player[i] != this && Main.player[i].active && !(Main.player[i] is Companion))
                {
                    Player player = Main.player[i];
                    if(!player.dead && PlayerMod.IsEnemy(this, player))
                    {
                        float Distance = (MyCenter - player.Center).Length();
                        if(Distance < NearestDistance && CanHit(player))
                        {
                            NewTarget = player;
                            NearestDistance = Distance;
                        }
                    }
                }
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (c != this && !c.dead && PlayerMod.IsEnemy(this, c) && c.GetGoverningBehavior().CanBeAttacked)
                {
                    float Distance = (MyCenter - c.Center).Length() - c.aggro - (c.width + width) * 0.5f;
                    if(Distance < NearestDistance && CanHit(c))
                    {
                        NewTarget = c;
                        NearestDistance = Distance;
                    }
                }
            }
            if (NewTarget != null)
            {
                Target = NewTarget;
            }
        }

        public void CheckIfNeedToJumpTallTile()
        {
            if(CanDoJumping)
            {
                float MovementDirection = controlLeft ? -1 : controlRight ? 1 : direction;
                int TileX = (int)((Center.X + 11 * MovementDirection + velocity.X) * DivisionBy16);
                int TileY = (int)((Bottom.Y - 1) * DivisionBy16);
                byte BlockedTiles = 0, Gap = 0;
                int MaxTilesY = (int)(jumpSpeed * Base.JumpHeight * DivisionBy16 + 2) + 3;
                int XCheckStart = (int)((position.X + width * 0.5f - 10) * DivisionBy16), XCheckEnd = (int)((position.X + width * 0.5f + 10) * DivisionBy16);
                for(byte i = 0; i < MaxTilesY; i++)
                {
                    Tile tile = Main.tile[TileX, TileY - 3 - i];
                    bool Blocked = false;
                    for(int x = XCheckStart; x < XCheckEnd; x++)
                    {
                        tile = Main.tile[x, TileY - i];
                        if(tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
                        {
                            Blocked = true;
                            break;
                        }
                    }
                    if(Blocked)
                    {
                        Gap = 0;
                        BlockedTiles = 5;
                        break;
                    }
                    tile = Main.tile[TileX, TileY - i];
                    if(tile.HasTile && Main.tileSolid[tile.TileType] && !TileID.Sets.Platforms[tile.TileType] && tile.TileType != TileID.ClosedDoor && tile.TileType != TileID.TallGateClosed)
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

        public void InitializeCompanion(bool Spawn = false)
        {
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            name = Data.GetName;
            inventory = Data.Inventory;
            armor = Data.Equipments;
            miscEquips = Data.MiscEquipment;
            dye = Data.EquipDyes;
            miscDyes = Data.MiscEquipDyes;
            if (Spawn)
            {
                statLife = statLifeMax2;
            }
            float HealthPercentage = Math.Clamp((float)statLife / statLifeMax2, 0, 1);
            statLifeMax = Data.MaxHealth;
            statManaMax = Data.MaxMana;
            for(int b = 0; b < buffType.Length; b++)
            {
                if(b < Data.BuffType.Length)
                {
                    buffType[b] = Math.Max(0, Data.BuffType[b]);
                    buffTime[b] = Data.BuffTime[b];
                }
            }
            Data.BuffType = buffType;
            Data.BuffTime = buffTime;
            if(Base.CompanionType == CompanionTypes.Terrarian)
            {
                TerrarianCompanionInfo info = Base.GetTerrarianCompanionInfo;
                hair = info.HairStyle;
                skinVariant = info.SkinVariant;
                hairColor = info.HairColor;
                eyeColor = info.EyeColor;
                shirtColor = info.ShirtColor;
                underShirtColor= info.UndershirtColor;
                pantsColor = info.PantsColor;
                shoeColor = info.ShoesColor;
                skinColor = info.SkinColor;
            }
            Male = Data.Gender == Genders.Male;
            CheckIfHasNpcState();
            idleBehavior = Base.DefaultIdleBehavior;
            if (idleBehavior != null)
                idleBehavior.SetOwner(this);
            combatBehavior = Base.DefaultCombatBehavior;
            if (combatBehavior != null)
                combatBehavior.SetOwner(this);
            followBehavior = Base.DefaultFollowLeaderBehavior;
            if (followBehavior != null)
                followBehavior.SetOwner(this);
            preRecruitBehavior = Base.PreRecruitmentBehavior;
            if (preRecruitBehavior != null)
                preRecruitBehavior.SetOwner(this);
            reviveBehavior = Base.ReviveBehavior;
            if (reviveBehavior != null)
                reviveBehavior.SetOwner(this);
            if(this is TerraGuardian) (this as TerraGuardian).OnInitializeTgAnimationFrames();
            UpdateStatus(false);
            statLife = (int)(statLifeMax2 * HealthPercentage);
            ScaleUpdate(true);
        }

        public void Teleport(Entity Target)
        {
            Teleport(Target.Bottom);
        }

        public void Teleport(Vector2 Destination)
        {
            position.X = Destination.X - width * 0.5f;
            position.Y = Destination.Y - height;
            velocity.X = 0;
            velocity.Y = 0;
            fallStart = (int)(position.Y * DivisionBy16);
            immuneTime = 40;
            immuneNoBlink = true;
        }

        public void FaceSomething(Player Target)
        {
            FaceSomething(Target.Center);
        }

        public void FaceSomething(Vector2 Position)
        {
            if (Position.X > Center.X) direction = 1;
            else direction = -1;
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
                float MoveSpeed = 20 * Base.AgilityPercent;
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

        public Vector2 GetAimDestinationPosition(Vector2 AimPosition)
        {
            float Accuracy = System.Math.Min(1f - this.Accuracy, 1);
            int DistanceAccuracy = (int)((AimPosition - Center).Length() * DivisionBy16);
            AimPosition.X += Main.rand.Next(-DistanceAccuracy, DistanceAccuracy + 1) * Accuracy;
            AimPosition.Y += Main.rand.Next(-DistanceAccuracy, DistanceAccuracy + 1) * Accuracy;
            return AimPosition;
        }

        public void AddSkillProgress(float Progress, uint ID, string ModID = "")
        {
            if (!(this is TerraGuardian) || !HasBeenMet || MainMod.DebugMode) return;
            GetCommonData.IncreaseSkillProgress(Progress, ID, ModID);
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
            int dirbkp = direction;
            direction = FacingLeft ? -1 : 1;
            Main.PlayerRenderer.DrawPlayerHead(Main.Camera, this, Position, 1, Scale);
            direction = dirbkp;
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
            IPlayerRenderer renderer = Main.PlayerRenderer;
            SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
            Player BackedUpPlayer = Main.player[whoAmI];
            Main.player[whoAmI] = this;
            TerraGuardiansPlayerRenderer.DrawingCompanions = true;
            if(!UseSingleDrawScript)
            {
                renderer.DrawPlayers(Main.Camera, new Player[]{ this });
            }
            else
            {
                renderer.DrawPlayer(Main.Camera, this, position, 0, fullRotationOrigin);
            }
            TerraGuardiansPlayerRenderer.DrawingCompanions = false;
            Main.player[whoAmI] = BackedUpPlayer;
            if (!UseSingleDrawScript) Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true, bool DiscountCharacterDimension = true, bool DiscountDirections = true, bool ConvertToCharacterPosition = true, bool BottomCentered = false)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if (BottomCentered)
            {
                Position.X = (Position.X - Base.SpriteWidth * 0.5f) * (!DiscountDirections ? 1 : direction);
                Position.Y = (Position.Y - Base.Height) * (!DiscountDirections ? 1 : gravDir);
            }
            else
            {
                if(DiscountDirections && direction < 0) //This...
                    Position.X = Base.SpriteWidth - Position.X;
                if(DiscountDirections && gravDir < 0)
                    Position.Y = Base.SpriteHeight - Position.Y;
            }
            Position *= Scale;
            if(ConvertToCharacterPosition)
            {
                if(DiscountCharacterDimension) 
                {
                    Position.X += (width - Base.SpriteWidth * Scale) * 0.5f; //I think this is totally wrong.
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

        public Vector2 GetBetweenAnimationPosition(AnimationPositions Animation, short Frame, bool AlsoTakePosition = true, bool DiscountCharacterDimension = true)
        {
            if(Base.GetHands <= 1)
                return GetAnimationPosition(Animation, Frame, 0, AlsoTakePosition, DiscountCharacterDimension);
            Vector2 OriginPosition = GetAnimationPosition(Animation, Frame, 0, false, DiscountCharacterDimension);
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

        public bool HasHouse
        {
            get
            {
                CompanionTownNpcState tns = GetTownNpcState;
                if(tns == null) return false;
                return !tns.Homeless;
            }
        }

        public bool CanFollowPlayer()
        {
            if (MainMod.DebugMode) return true;
            return Owner == null && FriendshipLevel >= Base.GetFriendshipUnlocks.FollowerUnlock;
        }

        public bool CanStopFollowingPlayer()
        {
            if ((Owner != null && PlayerMod.GetIsPlayerBuddy(Owner, this)) || GetPlayerMod.GetMountedOnCompanion != null || GetCharacterMountedOnMe != null) return false;
            return true;
        }

        public bool CanLiveHere(out bool LackFriendshipLevel)
        {
            LackFriendshipLevel = FriendshipLevel >= Base.GetFriendshipUnlocks.MoveInUnlock;
            if (MainMod.DebugMode)
            {
               return true;
            }
            return LackFriendshipLevel;
        }

        public bool CanAppointBuddy(out bool LackFriendship)
        {
            LackFriendship = FriendshipLevel < Base.GetFriendshipUnlocks.BuddyUnlock;
            if (MainMod.DebugMode) return true;
            return Base.CanBeAppointedAsBuddy && !LackFriendship;
        }

        public bool ToggleMount(Player Target, bool Forced = false)
        {
            if (!Forced && (CCed)) return false;
            {
                Companion controlled = PlayerMod.PlayerGetControlledCompanion(Target);
                if (controlled != null)
                    Target = controlled;
            }
            bool CharacterMountedIsTarget = Target == CharacterMountedOnMe;
            if(CharacterMountedOnMe != null)
            {
                switch(MountStyle)
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
                    if (!Forced && MountStyle == MountStyles.PlayerMountsOnCompanion)
                        RunBehavior(new MountDismountCompanionBehavior(this, Target, false));
                    return true;
                }
            }
            {
                CharacterMountedOnMe = Target;
                PlayerMod TargetModPlayer = Target.GetModPlayer<PlayerMod>();
                switch (MountStyle)
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
                if (!Forced && MountStyle == MountStyles.PlayerMountsOnCompanion)
                    RunBehavior(new MountDismountCompanionBehavior(this, Target, true));
                return true;
            }
            //return false;
        }

        public bool TogglePlayerControl(Player Target, bool Forced = false)
        {
            if (!Forced && CCed) return false;
            bool TargetIsControllingMe = Target == CharacterControllingMe;
            if (!TargetIsControllingMe)
            {
                if(PlayerMod.PlayerGetControlledCompanion(Target) != null)
                {
                    if (!PlayerMod.PlayerGetControlledCompanion(Target).TogglePlayerControl(Target, Forced))
                        return false;
                }
            }
            else
            {
                Target.position.X = Center.X - Target.width * 0.5f;
                Target.position.Y = Bottom.Y - Target.height;
                CharacterControllingMe = null;
                Target.GetModPlayer<PlayerMod>().GetCompanionControlledByMe = null;
                Companion c;
                if((c = PlayerMod.PlayerGetMountedOnCompanion(this)) != null)
                {
                    c.ToggleMount(this, true);
                }
                if((c = PlayerMod.PlayerGetCompanionMountedOnMe(this)) != null)
                {
                    c.ToggleMount(this, true);
                }
                return true;
            }
            Companion TargetMount = PlayerMod.PlayerGetCompanionMountedOnMe(Target);
            if (TargetMount != null)
            {
                if (TargetMount == this)
                {
                    TargetMount.ToggleMount(Target, true);
                }
                else
                {
                    TargetMount.ToggleMount(this, true);
                }
            }
            TargetMount = PlayerMod.PlayerGetMountedOnCompanion(Target);
            if (TargetMount != null)
            {
                if (TargetMount == this)
                {
                    TargetMount.ToggleMount(Target, true);
                }
                else
                {
                    TargetMount.ToggleMount(this, true);
                }
            }
            CharacterControllingMe = Target;
            Target.GetModPlayer<PlayerMod>().GetCompanionControlledByMe = this;
            Target.sitting.SitUp(Target);
            Target.sleeping.StopSleeping(Target);
            return true;
        }

        public void DespawnMinions()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && (Main.projectile[i].minionSlots > 0 || Main.projectile[i].type == ProjectileID.StardustDragon1 || Main.projectile[i].type == ProjectileID.StardustDragon4) && ProjMod.IsThisCompanionProjectile(i, this))
                {
                    Main.projectile[i].Kill();
                }
            }
        }

        public void GetBuddiesModeBenefits(out float HealthBonus, out float DamageBonus, out int DefenseBonus)
        {
            float Effective = Owner == null ? 1f : Owner.GetModPlayer<PlayerMod>().BuddiesModeEffective;
            HealthBonus = FriendshipLevel * 0.01f * Effective;
            DamageBonus = FriendshipLevel * 0.01f * Effective;
            DefenseBonus = (int)(FriendshipLevel * 0.3334f * Effective);
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

        public bool InDrawRange()
        {
            float DistanceX = Math.Abs(Center.X - (Main.screenPosition.X + Main.screenWidth * 0.5f)),
                DistanceY = Math.Abs(Center.Y - (Main.screenPosition.Y + Main.screenHeight * 0.5f));
            return DistanceX < Main.screenWidth * 0.5f + SpriteWidth + 200 && 
                    DistanceY < Main.screenHeight * 0.5f + SpriteHeight + 200;
        }

        public CompanionDrawMomentTypes GetDrawMomentType()
        {
            CompanionDrawMomentTypes DrawMoment = InternalGetDrawMoment();
            foreach(BehaviorBase.AffectedByBehaviorInfo affected in BehaviorBase.AffectedList)
            {
                if (affected.Contains(this))
                {
                    affected.TheBehavior.AffectedCompanionChangeDrawMoment(this, ref DrawMoment);
                }
            }
            GetGoverningBehavior().ChangeDrawMoment(this, ref DrawMoment);
            return DrawMoment;
        }

        private CompanionDrawMomentTypes InternalGetDrawMoment()
        {
            foreach(DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
            {
                if (doi.Child == this)
                {
                    switch(doi.Moment)
                    {
                        case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                            return CompanionDrawMomentTypes.DrawInBetweenParent;
                        case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                            return CompanionDrawMomentTypes.DrawInFrontOfParent;
                        case DrawOrderInfo.DrawOrderMoment.BehindParent:
                            return CompanionDrawMomentTypes.DrawBehindParent;
                    }
                }
            }
            if (Owner != null && (sitting.isSitting || sleeping.isSleeping))
            {
                if (MountStyle == MountStyles.CompanionRidesPlayer)
                {
                    if (Owner.sitting.isSitting && (Owner.Bottom == Bottom || IsBeingControlledBy(Owner)))
                    {
                        return CompanionDrawMomentTypes.DrawInBetweenOwner;
                    }
                    if (Owner.sleeping.isSleeping && (Owner.Bottom == Bottom || IsBeingControlledBy(Owner)))
                    {
                        return CompanionDrawMomentTypes.DrawInFrontOfOwner;
                    }
                }
                else if (!Owner.sleeping.isSleeping && !Owner.sitting.isSitting)
                {
                    return CompanionDrawMomentTypes.AfterTiles;
                }
                if(sleeping.isSleeping && Base.DrawBehindWhenSharingBed)
                    return CompanionDrawMomentTypes.DrawBehindOwner;
                if (sitting.isSitting)
                {
                    if (IsUsingThroneOrBench)
                    {
                        if (Base.DrawBehindWhenSharingThrone)
                            return CompanionDrawMomentTypes.DrawBehindOwner;
                    }
                    else
                    {
                        if (Base.DrawBehindWhenSharingChair)
                            return CompanionDrawMomentTypes.DrawBehindOwner;
                    }
                }
                return CompanionDrawMomentTypes.DrawOwnerInBetween;
            }
            if (IsMountedOnSomething)
            {
                if(MountStyle == MountStyles.CompanionRidesPlayer)
                    return CompanionDrawMomentTypes.DrawInBetweenMountedOne;
                return CompanionDrawMomentTypes.DrawBehindOwner;
            }
            if(Owner != null)
            {
                if ((Owner.sitting.isSitting || Owner.sleeping.isSleeping) != UsingFurniture)
                {
                    if (UsingFurniture)
                    {
                        return CompanionDrawMomentTypes.AfterTiles;
                    }
                    else
                    {
                        return CompanionDrawMomentTypes.DrawInFrontOfOwner;
                    }
                }
                return CompanionDrawMomentTypes.DrawBehindOwner;
            }
            return CompanionDrawMomentTypes.AfterTiles;
        }

        public string GetOtherMessage(string Context, string DefaultMessage = "")
        {
            string Mes = GetDialogues.GetOtherMessage(this, Context);
            if (Mes == "" && DefaultMessage != "") return DefaultMessage;
            return Mes;
        }
    }

    public struct FollowOrderSetting
    {
        public float Distance;
        public bool Front;
    }

    public enum CompanionDrawMomentTypes : byte
    {
        AfterTiles,
        DrawInBetweenMountedOne,
        DrawBehindOwner,
        DrawOwnerInBetween,
        DrawInBetweenOwner,
        DrawInFrontOfOwner,
        DrawInBetweenParent,
        DrawInFrontOfParent,
        DrawBehindParent,
        BehindTiles
    }
}