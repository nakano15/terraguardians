using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
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
        private TgDrawInfoHolder DrawInfoHolder = null;

        public TgDrawInfoHolder GetDrawInfo { get { return DrawInfoHolder; } }
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
        public PersonalityBase GetPersonality
        {
            get
            {
                return Base.GetPersonality(this);
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
                //if (TitanCompanion && Base.MountStyle == MountStyles.CompanionRidesPlayer) //Need to add support for small companions to let player mount on their shoulders, when they're giant.
                //    return MountStyles.PlayerMountsOnCompanion;
                return Base.MountStyle;
            }
        }
        new public CompanionsDoorHelper doorHelper = new CompanionsDoorHelper();
        internal bool MaskLastWasDead = false;
        public CombatTactics? TacticsOverride = null;
        public CombatTactics CombatTactic { get { if (TacticsOverride.HasValue) return TacticsOverride.Value; return Data.CombatTactic; } set { Data.CombatTactic = value; }}
        public CompanionID GetCompanionID { get { return Data.GetMyID; } }
        public uint ID { get { return Data.ID; } }
        public string ModID { get { return Data.ModID; } }
        public bool IsGeneric { get { return Data.IsGeneric; } }
        public ushort GenericID { get { return Data.GetGenericID; } }
        public uint Index { get{ return Data.Index; } }
        public bool HasBeenMet { get { return IsGeneric || WorldMod.HasMetCompanion(Data); }}
        public bool IsPlayerCharacter = false;
        private CompanionSkinInfo _SelectedSkin = null, _SelectedOutfit = null;
        public CompanionSkinInfo GetSelectedSkin => _SelectedSkin;
        public CompanionSkinInfo GetSelectedOutfit => _SelectedOutfit;
        public byte OutfitID { get { return Data.OutfitID; } set { Data.OutfitID = value; } }
        public string OutfitModID { get { return Data.OutfitModID; } set { Data.OutfitModID = value; } }
        public byte SkinID { get { return Data.SkinID; } set { Data.SkinID = value; } }
        public string SkinModID { get { return Data.SkinModID; } set { Data.SkinModID = value; } }
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
        public bool BlockDirectionChange { get { return LockCharacterDirection; } set { LockCharacterDirection = value; }}
        public bool FlipWeaponUsageHand = false, LockCharacterDirection = false;
        #region Behaviors
        public BehaviorBase idleBehavior = new IdleBehavior(),
            combatBehavior = new CombatBehavior(),
            followBehavior = new FollowLeaderBehavior(),
            preRecruitBehavior = new PreRecruitBehavior(),
            temporaryBehavior = null;
        public ReviveBehavior reviveBehavior = new ReviveBehavior();
        #endregion
        #region Furniture Stuff
        protected int furniturex = -1, furniturey = -1;
        protected bool reachedfurniture = false;
        public int GetFurnitureX { get{ return furniturex; } }
        public int GetFurnitureY { get{ return furniturey; } }
        public bool GetReachedFurniture { get { return reachedfurniture; } }
        public bool GoingToOrUsingFurniture { get { return furniturex > -1 && furniturey > -1; } }
        public bool UsingFurniture { get { return furniturex > -1 && furniturey > -1 && reachedfurniture; } }
        public bool IsUsingAnyChair { get { return UsingFurniture && (Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Chairs || Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Toilets); } }
        public bool IsUsingToilet { get { return UsingFurniture && Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Toilets; } }
        public bool IsUsingBed { get { return UsingFurniture && Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Beds; } }
        public bool IsUsingThroneOrBench { get { return UsingFurniture && (Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Thrones || Main.tile[furniturex, furniturey].TileType == Terraria.ID.TileID.Benches); } }
        #endregion
        public Vector2 GetCompanionCenter
        {
            get
            {
                return Bottom - Vector2.UnitY * (SpriteHeight - gfxOffY) * .5f;
            }
        }
        public Vector2 AimDirection = Vector2.Zero;
        public Vector2 GetAimedPosition
        {
            get
            {
                return GetCompanionCenter + AimDirection;
            }
            set
            {
                AimDirection = value - GetCompanionCenter;
            }
        }
        public bool IsBeingControlledBySomeone { get { return CharacterControllingMe != null; } }
        public bool IsBeingControlledBy(Player player) { return CharacterControllingMe == player; }
        public bool IsMountedOnSomething { get { return CharacterMountedOnMe != null; } }
        public Player GetCharacterMountedOnMe { get { return CharacterMountedOnMe; }}
        public Player GetMountedOnCharacter { get { return GetPlayerMod.GetMountedOnCompanion; }}
        public bool CompanionHasControl { get { return (CharacterMountedOnMe == null && CharacterControllingMe == null) || (CharacterMountedOnMe != null && (PlayerMod.GetPlayerKnockoutState(CharacterMountedOnMe) > KnockoutStates.Awake || !PlayerMod.IsPlayerCharacter(CharacterMountedOnMe) || PlayerMod.IsCompanionFreeControlEnabled(Owner != null ? Owner : CharacterMountedOnMe))) || (IsBeingControlledBySomeone && PlayerMod.IsCompanionFreeControlEnabled(CharacterControllingMe)); }}
        public Player GetCharacterControllingMe { get { return CharacterControllingMe; } }
        private Player CharacterMountedOnMe = null, CharacterControllingMe = null;
        public bool IsBeingPulledByPlayer = false, SuspendedByChains = false, FallProtection = false;
        public bool WalkMode = false;
        public float Scale = 1f;
        float ScaleMinusBaseScale = 1f;
        public float GetScaleMinusBaseScale => ScaleMinusBaseScale;
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
        public int GetAge { get { return Base.Age; } }
        public int GetBirthday { get { return Base.GetBirthday; } }
        public string GetBirthdayString { get { 
            Seasons season;
            byte Day;
            BirthdayCalculator.ReturnSeasonAndDay(GetBirthday, out season, out Day);
            string Text = SeasonTranslator.Translate(season) + " " + Day;
            switch(Day)
            {
                case 1:
                    Text += "st";
                    break;
                case 2:
                    Text += "nd";
                    break;
                case 3:
                    Text += "rd";
                    break;
                default:
                    Text += "th";
                    break;
            }
            return Text;
        }}
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
                if (Main.eclipse || Main.bloodMoon) return true;
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
        public FollowOrderSetting FollowOrder = new FollowOrderSetting(); 
        public byte GetAppliedFoodLevel { get { return AppliedFoodLevel; } }
        public short[] ArmFramesID = new short[0], ArmFrontFramesID = new short[0];
        public short BodyFrameID = 0, BodyFrontFrameID = -1;
        public Vector2 BodyOffset = Vector2.Zero;
        public Vector2[] ArmOffset = new Vector2[0];
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
        #region Sub Attack Stuff
        public bool IsSubAttackInUse
        {
            get
            {
                return SubAttackInUse < 255;
            }
        }
        public byte GetSubAttackInUse 
        {
            get
            {
                return SubAttackInUse;
            }
            internal set
            {
                SubAttackInUse = value;
            }
        }
        public byte[] SubAttackIndexes { get { return Data.GetSubAttackIndexes; } internal set { Data.GetSubAttackIndexes = value; } }
        public byte SelectedSubAttack = 0; //For 2P
        internal byte SubAttackInUse = 255; //255 = No Sub Attack in use.
        internal List<SubAttackData> SubAttackList = new List<SubAttackData>();
        public SubAttackData GetSubAttackActive
        {
            get
            {
                if (SubAttackInUse == 255) return null;
                return SubAttackList[SubAttackInUse];
            }
        }
        #endregion
        byte InternalDelay = 0;
        public byte GetInternalDelayValue => InternalDelay;
        internal CompanionInventoryStatsContainer InventorySupplyStatus = new CompanionInventoryStatsContainer();
        internal HeartDisplayHelper HeartDisplay = new HeartDisplayHelper();
        List<string> ScheduledMessages = new List<string>();

        public string GetPlayerNickname(Player player)
        {
            return Data.GetPlayerNickname(player);
        }

        public void ChangePlayerNickname(string NewNickname)
        {
            Data.ChangePlayerNickname(NewNickname);
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
                return CanJump || jump > 0 || (AnyExtraJumpUsable() && !releaseJump);
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

        private static BitsByte _Behaviour_Flags2;
        public static bool UnallowAutoJump
        {
            get
            {
                return _Behaviour_Flags2[0];
            }
            set
            {
                _Behaviour_Flags2[0] = value;
            }
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
            if (MainMod.IsDebugMode) return true;
            return FriendshipLevel >= Base.GetFriendshipUnlocks.InviteUnlock;
        }

        public bool CanTakeRequests(Player player) { 
            if (!PlayerMod.PlayerHasCompanion(player, this)) return false;
            if (MainMod.IsDebugMode || PlayerMod.GetIsPlayerBuddy(player, this)) return true;
            return Base.GetFriendshipUnlocks.RequestUnlock < 255 && FriendshipLevel >= Base.GetFriendshipUnlocks.RequestUnlock; 
        }
        

        public bool PlayerCanMountCompanion(Player player)
        {
            if (MainMod.IsDebugMode || PlayerMod.GetIsPlayerBuddy(player, this) || (MainMod.Gameplay2PMode && PlayerMod.IsCompanionLeader(player, this))) return true;
            if(Owner == player)
            {
                return Base.GetFriendshipUnlocks.MountUnlock < 255 && FriendshipLevel >= Base.GetFriendshipUnlocks.MountUnlock;
            }
            return false;
        }

        public bool PlayerCanControlCompanion(Player player)
        {
            if (MainMod.Gameplay2PMode && PlayerMod.IsCompanionLeader(player, this)) return false;
            if (MainMod.IsDebugMode || PlayerMod.GetIsPlayerBuddy(player, this)) return true;
            if(Owner == player && this is TerraGuardian)
            {
                return Base.GetFriendshipUnlocks.ControlUnlock < 255 && FriendshipLevel >= Base.GetFriendshipUnlocks.ControlUnlock;
            }
            return false;
        }

        public void ClearChat()
        {
            chatOverhead.timeLeft = 0;
        }

        public bool IsSpeaking => chatOverhead.timeLeft > 0;
        
        public void SaySomethingOnChat(string Message, Color? color = null)
        {
            Companion LastSpeaker = Dialogue.Speaker;
            Dialogue.Speaker = this;
            string NewMessage = "<" + GetNameColored() + "> ";
            NewMessage += Dialogue.ParseText(Message);
            Main.NewText(NewMessage, color);
            Dialogue.Speaker = LastSpeaker;
        }

        public int SaySomething(string Text, bool ShowOnChat = false)
        {
            Companion SpeakerBackup = Dialogue.Speaker;
            Dialogue.Speaker = this;
            Text = Dialogue.ParseText(Text);
            Dialogue.Speaker = SpeakerBackup;
            int Time = 240 + Text.Length;
            chatOverhead.NewMessage(Text, Time);
            if (ShowOnChat)
            {
                SaySomethingOnChat(Text);
            }
            return Time;
        }

        public int SaySomethingCanSchedule(string Text)
        {
            Companion SpeakerBackup = Dialogue.Speaker;
            Dialogue.Speaker = this;
            Text = Dialogue.ParseText(Text);
            Dialogue.Speaker = SpeakerBackup;
            if (chatOverhead.timeLeft > 0)
            {
                ScheduledMessages.Add(Text);
                return -1;
            }
            int Time = 240 + Text.Length;
            chatOverhead.NewMessage(Text, Time);
            return Time;
        }

        public int SaySomethingAtRandom(string[] Text)
        {
            if (Text.Length > 0)
            {
                string Mes = Text[Main.rand.Next(Text.Length)];
                int Time = 180 + Mes.Length;
                chatOverhead.NewMessage(Mes, Time);
                return Time;
            }
            return 0;
        }

        public void SetFallStart()
        {
            fallStart = fallStart2 = (int)(position.Y * DivisionBy16);
        }

        public BehaviorBase GetGoverningBehavior(bool GetTemporaryBehavior = true)
        {
            if (GetTemporaryBehavior && temporaryBehavior != null && temporaryBehavior.IsActive)
                return temporaryBehavior;
            if (IsBeingControlledBySomeone && CompanionHasControl)
            {
                return idleBehavior;
            }
            if(Owner != null)
            {
                if (!Owner.ghost)
                    return followBehavior;
                return idleBehavior;
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

        public void OnDeath()
        {
            OnDeathHook();
            Base.OnDeath(this);
        }

        public void OnPlayerDeath(Player Target)
        {
            OnPlayerDeathHook(Target);
            Base.OnPlayerDeath(this, Target);
        }

        public void OnNpcDeath(NPC Target)
        {
            OnNpcDeathHook(Target);
            Base.OnNpcDeath(this, Target);
        }

        public void OnCompanionDeath(Companion Target)
        {
            OnCompanionDeathHook(Target);
            Base.OnCompanionDeath(this, Target);
        }

        public virtual void OnDeathHook()
        {
            
        }

        public virtual void OnPlayerDeathHook(Player Target)
        {
            
        }

        public virtual void OnNpcDeathHook(NPC Target)
        {
            
        }

        public virtual void OnCompanionDeathHook(Companion Target)
        {
            
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

        public bool IsPlayerRoomMate(Player player)
        {
            if (player.SpawnX > -1 && player.SpawnY > -1)
            {
                CompanionTownNpcState tns = GetTownNpcState;
                if (tns != null && !tns.Homeless && tns.HouseInfo != null)
                {
                    return tns.HouseInfo.BelongsToThisHousing(player.SpawnX, player.SpawnY - 1);
                }
            }
            return false;
        }

        public bool CreatePathingTo(Vector2 Destination, bool WalkToPath = false, bool StrictPath = true, bool CancelOnFail = false)
        {
            if (IsBeingPulledByPlayer) return false;
            return CreatePathingTo((int)(Destination.X * DivisionBy16), (int)(Destination.Y * DivisionBy16), WalkToPath, StrictPath, CancelOnFail);
        }

        public bool CreatePathingTo(int X, int Y, bool WalkToPath = false, bool StrictPath = true, bool CancelOnFail = false)
        {
            if (!WorldGen.InWorld(X, Y)) return false;
            if (!GetTileGroundPosition(ref X, ref Y)) return false;
            if (!PathFinder.CheckForSolidBlocks(X, Y))
            {
                //float JumpDecelerationCalculation = jumpSpeed / gravity;
                return Path.CreatePathTo(Bottom, X, Y, (int)(GetMaxJumpHeight * DivisionBy16), GetFallTolerance, WalkToPath, StrictPath, CancelOnFail);
            }
            return false;
        }

        public bool CreatePathingTo(Entity Target, bool WalkToPath = false, bool StrictPath = true, bool CancelOnFail = false)
        {
            return Path.CreatePathTo(Bottom, Target, (int)(GetMaxJumpHeight * DivisionBy16), GetFallTolerance, WalkToPath, StrictPath, CancelOnFail);
        }

        public bool GetTileGroundPosition(ref int X, ref int Y)
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
            return true;
        }

        public void ChangeSubAttackIndex(byte SlotIndex, byte SubAttackIndex)
        {
            if (SlotIndex < SubAttackIndexes.Length && SubAttackIndex < SubAttackList.Count)
            {
                SubAttackIndexes[SlotIndex] = SubAttackIndex;
            }
        }

        public byte GetSubAttackIndexFromSlotIndex(int SlotIndex)
        {
            if (SlotIndex < SubAttackIndexes.Length)
            {
                return SubAttackIndexes[SlotIndex];
            }
            return 0;
        }

        public bool UseSubAttack<T>(bool IgnoreCooldown = false, bool DoCooldown = true) where T : SubAttackBase
        {
            if (SubAttackInUse < 255) return false;
            foreach(SubAttackData d in SubAttackList)
            {
                if (d.GetBase is T)
                {
                    return d.UseSubAttack(IgnoreCooldown, DoCooldown);
                }
            }
            return false;
        }

        public bool UseSubAttack(int Position, bool IgnoreCooldown = false, bool DoCooldown = true)
        {
            if (SubAttackInUse < 255 || Position >= SubAttackIndexes.Length || SubAttackIndexes[Position] == 255) return false;
            return SubAttackList[SubAttackIndexes[Position]].UseSubAttack(IgnoreCooldown, DoCooldown);
        }

        public bool UseSubAttack(bool IgnoreCooldown = false, bool DoCooldown = true)
        {
            if (SubAttackInUse < 255 || SelectedSubAttack >= SubAttackIndexes.Length || SubAttackIndexes[SelectedSubAttack] >= SubAttackList.Count) return false;
            return SubAttackList[SubAttackIndexes[SelectedSubAttack]].UseSubAttack(IgnoreCooldown, DoCooldown);
        }

        public bool SubAttackInCooldown<T>() where T : SubAttackBase
        {
            foreach(SubAttackData d in SubAttackList)
            {
                if (d.GetBase is T)
                {
                    return d.IsInCooldown;
                }
            }
            return false;
        }

        public bool SubAttackInCooldown(int i)
        {
            if (i < SubAttackList.Count) return SubAttackList[i].IsInCooldown;
            return false;
        }

        public bool GetIsSubAttackInUse<T>() where T : SubAttackBase
        {
            return SubAttackInUse < 255 && GetSubAttackActive.GetBase is T;
        }

        public void ChangeSelectedSubAttackSlot(bool Next)
        {
            if (SubAttackList.Count == 0) return;
            if (!Next)
            {
                int New = SelectedSubAttack -1;
                while (New != SelectedSubAttack)
                {
                    if (New < 0) New += SubAttackIndexes.Length;
                    if(SubAttackIndexes[New] < SubAttackList.Count)
                    {
                        break;
                    }
                    New--;
                }
                SelectedSubAttack = (byte)New;
                /*if (SelectedSubAttack == 0)
                {
                    SelectedSubAttack = (byte)SubAttackIndexes.Length;
                    if (SelectedSubAttack > 0) SelectedSubAttack--;
                }
                else
                {
                    SelectedSubAttack--;
                }*/
            }
            else
            {
                int New = SelectedSubAttack + 1;
                while (New != SelectedSubAttack)
                {
                    if (New >= SubAttackIndexes.Length) New -= SubAttackIndexes.Length;
                    if(SubAttackIndexes[New] < SubAttackList.Count)
                    {
                        break;
                    }
                    New++;
                }
                SelectedSubAttack = (byte)New;
                /*if (SelectedSubAttack >= SubAttackIndexes.Length - 1)
                {
                    SelectedSubAttack = 0;
                }
                else
                {
                    SelectedSubAttack ++;
                }*/
            }
        }

        void UpdateCreativeModePowers()
        {
            if (Owner == null || !Main.GameModeInfo.IsJourneyMode) return;
            CreativePowers.SpawnRateSliderPerPlayerPower srl = CreativePowerManager.Instance.GetPower<CreativePowers.SpawnRateSliderPerPlayerPower>();
            float val;
            srl.GetRemappedSliderValueFor(Owner.whoAmI, out val);
            srl.RemapSliderValueToPowerValue(val);
        }

        public void UpdateBehaviour()
        {
            _Behaviour_Flags = 0;
            _Behaviour_Flags2 = 0;
            if (Owner == MainMod.GetLocalPlayer && PlayerMod.IsCompanionLeader(Owner, this))
            {
                MainMod.Update2PControls(this);
            }
            if (Owner != null)
            {
                team = Owner.team;
            }
            else
            {
                team = 0;
            }
            bool AutoMode = !Is2PCompanion || MainMod.Gameplay2PInventory;
            if (AutoMode)
                MoveLeft = MoveRight = MoveUp = ControlJump = controlUseItem = false;
            UpdateBehaviorHook();
            Base.UpdateBehavior(this);
            bool IsKOd = dead || KnockoutStates > KnockoutStates.Awake;
            bool ControlledByPlayer = IsBeingControlledBySomeone;
            BehaviorBase Behavior = GetGoverningBehavior();
            if (!IsKOd && AutoMode)
            {
                if (Behavior.AllowSeekingTargets) 
                {
                    LookForTargets();
                }
                if (!ControlledByPlayer && Behavior.UseHealingItems) CheckForItemUsage();
            }
            if (Behavior.RunCombatBehavior) combatBehavior.Update(this);
            if (!IsKOd)
            {
                UpdateDialogueBehaviour();
                if (AutoMode)
                {
                    if (!NpcMode)
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
                    }
                    FollowPathingGuide();
                }
                UpdateFurnitureUsageScript();
            }
            if(AutoMode && !ControlledByPlayer && !Behaviour_AttackingSomething)
                ChangeAimPosition(Center + Vector2.UnitX * width * direction);
            if (!IsKOd && Behavior.AllowRevivingSomeone) reviveBehavior.Update(this);
            Behavior.Update(this);
            if (IsKOd) return;
            if(MoveLeft || MoveRight)
            {
                CheckIfNeedToJumpTallTile();
            }
            if (AutoMode && !ControlledByPlayer && !IsMountedOnSomething)
            {
                CheckForCliffs();
                CheckForFallDamage();
                CheckForDrowningJumpCheck();
            }
            if (GetSubAttackInUse == 255)
            {
                if (!IsBeingControlledBySomeone && !Data.UnallowAutoUseSubattacks)
                {
                    for(byte i = 0; i < CompanionData.MaxSubAttackSlots; i++)
                    {
                        if (SubAttackIndexes[i] < SubAttackList.Count)
                        {
                            SubAttackData sad = SubAttackList[SubAttackIndexes[i]];
                            if (sad.CheckAutoUseCondition(this))
                            {
                                if (sad.UseSubAttack())
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!GetSubAttackActive.GetBase.AllowItemUsage)
                {
                    controlUseItem = false;
                }
            }
            //OffhandHeldAction();
        }

        void CheckForDrowningJumpCheck()
        {
            if (breath < breathMax * .5f)
            {
                if (!controlJump)
                {
                    if (velocity.Y == 0f)
                    {
                        if (HasAirPocketAbove())
                        {
                            controlJump = true;
                        }
                    }
                }
                else
                {
                    if (jump > 0 && HasAirPocketAbove())
                    {
                        controlJump = true;
                    }
                }
            }
        }

        bool HasAirPocketAbove()
        {
            int StartX = (int)(Center.X * DivisionBy16), StartY = (int)(position.Y * DivisionBy16);
            for (int y = 1; y < 6; y++)
            {
                bool FoundAirPocket = false;
                for (int x = -1; x <= 0; x++)
                {
                    int PX = StartX + x, PY = StartY - y;
                    if (!WorldGen.InWorld(PX, PY)) return false;
                    Tile tile = Main.tile[PX, PY];
                    if (tile != null)
                    {
                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                        {
                            return false;
                        }
                        if (tile.LiquidAmount < 16)
                        {
                            FoundAirPocket = true;
                        }
                    }
                }
                if (FoundAirPocket)
                    return true;
            }
            return false;
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

        static int[] _CliffCheckPreviousHeight = new int[5];
        static byte[] _CliffCheckPreviousDanger = new byte[5];
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
            for (int i = 0; i < 5; i++)
            {
                _CliffCheckPreviousHeight[i] = -1;
                _CliffCheckPreviousDanger[i] = 255;
            }
            const int CheckAheadDistance = 5;
            int Direction = Movement > 0 ? 1 : -1;
            int CheckStart = (int)((Center.X + (width * 0.5f + 1) * Direction) * DivisionBy16);
            int CheckYFoot = (int)(Bottom.Y * DivisionBy16);
            int RangeY = Math.Min(12, Base.FallHeightTolerance);
            const byte DT_Solid = 0, DT_Water = 1, DT_Trap = 2, DT_Lava = 3;
            for (int x = 0; x < CheckAheadDistance; x++)
            {
                int CheckX = CheckStart + x * Direction;
                byte LiquidTiles = 0;
                for(byte y = 0; y < RangeY; y++)
                {
                    int CheckY = CheckYFoot + y;
                    if (WorldGen.InWorld(CheckX, CheckY))
                    {
                        Tile tile = Main.tile[CheckX, CheckY];
                        if (tile.LiquidType >= 0 && tile.LiquidAmount > 50)
                        {
                            int Liquid = tile.LiquidType;
                            LiquidTiles++;
                            if (LiquidTiles >= 3 || Liquid == LiquidID.Lava)
                            {
                                if (_CliffCheckPreviousDanger[x] == DT_Solid)
                                    _CliffCheckPreviousHeight[x] = y;
                                _CliffCheckPreviousDanger[x] = Liquid == LiquidID.Lava ? DT_Lava : DT_Water;
                                break;
                            }
                        }
                        if(tile.HasTile && !tile.IsActuated)
                        {
                            bool HasTrap = false;
                            if (tile.TileType == TileID.PressurePlates && townNPCs < 2)
                                HasTrap = true;
                            switch(tile.TileType)
                            {
                                case TileID.Spikes:
                                case TileID.WoodenSpikes:
                                case TileID.LandMine:
                                    _CliffCheckPreviousDanger[x] = DT_Trap;
                                    _CliffCheckPreviousHeight[x] = y;
                                    HasTrap = true;
                                    break;
                            }
                            if (HasTrap)
                                break;
                            if (Main.tileSolid[tile.TileType])
                            {
                                _CliffCheckPreviousDanger[x] = DT_Solid;
                                _CliffCheckPreviousHeight[x] = y;
                                break;
                            }
                        }
                    }
                }
            }
            bool Avoid = false;
            int AvoidRange = 0;
            byte GapSize = 0;
            byte AvoidCount = 0; //Has issues checking for tiles ahead.
            for (int x = 0; x < CheckAheadDistance; x++)
            {
                switch (_CliffCheckPreviousDanger[x])
                {
                    case DT_Solid:
                        if (_CliffCheckPreviousHeight[x] == -1)
                        {
                            GapSize++;
                            if (GapSize >= 2)
                            {
                                Avoid = true;
                                break;
                            }
                            AvoidRange = x;
                        }
                        else
                        {
                            if (AvoidCount > 0)
                            {
                                int PrePreviousTileH = x - 2, PreviousTileH = x - 1;
                                if (PrePreviousTileH < 0)
                                    PrePreviousTileH = 0;
                                else
                                    PrePreviousTileH = _CliffCheckPreviousHeight[PrePreviousTileH];
                                if (PreviousTileH < 0)
                                    PreviousTileH = 0;
                                else
                                    PreviousTileH = _CliffCheckPreviousHeight[PreviousTileH];
                                if (PrePreviousTileH <= PreviousTileH && _CliffCheckPreviousHeight[x] <= PreviousTileH)
                                {
                                    AvoidCount = 0;
                                }
                            }
                            GapSize = 0;
                        }
                        break;
                    case DT_Trap:
                        AvoidRange = x;
                        Avoid = true;
                        break;
                    case DT_Lava:
                        if (!HasLavaImmunityAbility)
                        {
                            if (AvoidRange == -1)
                                AvoidRange = x;
                            AvoidCount++;
                            if (AvoidCount >= 2)
                            {
                                Avoid = true;
                            }
                        }
                        break;
                    case DT_Water:
                        if (!HasSwimmingAbility)
                        {
                            if (AvoidRange == -1)
                                AvoidRange = x;
                            AvoidCount++;
                            if (AvoidCount >= 2)
                            {
                                Avoid = true;
                            }
                        }
                        break;
                }
                if (Avoid)
                    break;
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

        bool SlopedTileUnder()
        {
            int CenterX = (int)(Center.X * DivisionBy16), BottomY = (int)(Bottom.Y * DivisionBy16);
            return SlopedTileUnder(CenterX, BottomY);
        }

        bool SlopedTileUnder(int X, int Y)
        {
            Y++;
            bool Slope = false, NormalBlock = false;
            for (int x = -1; x < 1; x++)
            {
                if (WorldGen.InWorld(X + x, Y))
                {
                    Tile tile = Main.tile[X + x, Y];
                    if (tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType])
                    {
                        switch (tile.Slope)
                        {
                            case SlopeType.SlopeDownLeft:
                            case SlopeType.SlopeDownRight:
                                Slope = true;
                                break;
                            default:
                                if (tile.IsHalfBlock)
                                    Slope = true;
                                else
                                    NormalBlock = true;
                                break;
                        }
                    }
                }
            }
            return Slope && !NormalBlock;
        }

        bool FollowPathingGuide()
        {
            if (Path.State != PathFinder.PathingState.TracingPath || Behaviour_InDialogue || MainMod.DebugPathFinding) return false;
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
                    if (!Path.ResumePathingTo(Bottom))
                    {
                        Path.CancelPathing();
                        return false;
                    }
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
                    Path.CancelPathing(true);
                else
                    Path.ResumePathingTo(Bottom);
                return false;
            }
            if (velocity.Y == 0 && Path.CheckIfNearTarget(Bottom)) return false;
            PathFinder.Breadcrumb checkpoint = Path.GetLastNode;
            bool ReachedNode = false;
            Vector2 Position = Bottom + velocity * 2;
            switch(checkpoint.NodeOrientation)
            {
                case PathFinder.Node.NONE:
                    ReachedNode = true;
                    break;
                case PathFinder.Node.DIR_JUMP:
                    {
                        float EndX = checkpoint.X * 16;
                        if (velocity.Y == 0 || (jump > 0 && MathF.Abs(Position.X - EndX) > 12f))
                        {
                            ControlJump = true;
                        }
                        if (velocity.Y != 0)
                        {
                            if (Position.X < EndX)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        float Y = checkpoint.Y * 16 + 16;
                        if (SlopedTileUnder(checkpoint.X, checkpoint.Y))
                        {
                            Y += 8;
                        }
                        if (Position.Y <= Y)
                        {
                            if (velocity.Y == 0 && MathF.Abs(Position.X - EndX) < 8f)
                            {
                                ReachedNode = true;
                                ControlJump = false;
                            }
                        }
                        else
                        {
                            if (velocity.Y == 0)
                                Path.IncreaseStuckTimer(12);
                        }
                    }
                    break;
                case PathFinder.Node.DIR_UP:
                    {
                        //Position.Y -= 2;
                        float X = checkpoint.X * 16, Y = (checkpoint.Y + 1) * 16;
                        if (SlopedTileUnder(checkpoint.X, checkpoint.Y))
                        {
                            Y += 8;
                        }
                        bool IsFarFromCenter = Math.Abs(Position.X - X) > 10;
                        if (IsFarFromCenter)
                        {
                            if (Position.X < X)
                                MoveRight = true;
                            else
                                MoveLeft = true;
                        }
                        /*if (Math.Abs(velocity.X * 2f) > Math.Abs(Position.X - X) * 16)
                        {
                            if (Position.X < X)
                            {
                                MoveLeft = true;
                            }
                            else
                            {
                                MoveRight = true;
                            }
                        }
                        else */if (!IsFarFromCenter)
                        {
                            if (Position.Y > Y) //Stairs...
                            {
                                if (CanJump || jump > 0 || (releaseJump && AnyExtraJumpUsable()))
                                {
                                    ControlJump = true;
                                }
                                else
                                {
                                    if (velocity.Y == 0)
                                        Path.IncreaseStuckTimer(12);
                                }
                            }
                            else
                            {
                                if (Position.Y < Y - 8)
                                {
                                    if(velocity.Y == 0)
                                    {
                                        MoveDown = true;
                                        if (this is TerraGuardian && releaseJump)
                                            ControlJump = true;
                                    }
                                    /*else
                                    {
                                        ReachedNode = true;
                                    }*/
                                }
                                else
                                {
                                    //if (velocity.Y == 0)
                                    ReachedNode = true;
                                }
                            }
                        }
                    }
                    break;
                case PathFinder.Node.DIR_RIGHT:
                case PathFinder.Node.DIR_LEFT:
                    {
                        Position.Y -= 2;
                        float X = checkpoint.X * 16/* + 8*/;
                        /*if (Math.Abs(velocity.X * 2f) > Math.Abs(Position.X - X))
                        {
                            if (Position.X < X)
                            {
                                MoveLeft = true;
                            }
                            else
                            {
                                MoveRight = true;
                            }
                        }
                        else */if (Math.Abs(Position.X - X) < 4)
                        {
                            if (velocity.Y == 0)
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
                        if (SlopedTileUnder(checkpoint.X, checkpoint.Y))
                        {
                            Y += 8;
                        }
                        if (Math.Abs(velocity.X * 2f) / runSlowdown > Math.Abs(Position.X - X))
                        {
                            if (Position.X < X)
                            {
                                MoveLeft = true;
                            }
                            else
                            {
                                MoveRight = true;
                            }
                        }
                        else if (Position.Y < Y + 8)
                        {
                            if (Math.Abs(Position.X - X) > 5)
                            {
                                if (Position.X < X) MoveRight = true;
                                else MoveLeft = true;
                            }
                            else
                            {
                                UnallowAutoJump = true;
                                if (velocity.Y == 0)
                                {
                                    if (!PathFinder.CheckForPlatform(Position, 20, out sbyte dir))
                                    {
                                        if (dir == -1) MoveRight = true;
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
                        }
                        else
                        {
                            if (velocity.Y == 0)
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

        public bool IsHostileTo(NPC otherNPC)
        {
            return !otherNPC.friendly;
        }

        internal static void ResetLastID()
        {
            LastWhoAmID = 0;
        }

        public bool IsFacingTarget(Entity Target)
        {
            return IsFacingDirection(Target.position.X + Target.width * .5f);
        }

        public bool IsFacingDirection(float XPosition)
        {
            float MyCenter = position.X + width * .5f;
            return (XPosition < MyCenter && direction == -1) || (XPosition > MyCenter && direction == 1);
        }

        public bool IsComfortPointsMaxed()
        {
            return Data.FriendshipProgress.IsComfortMaxed();
        }

        public void CheckForItemUsage()
        {
            if(itemAnimation > 0) return;
            if (potionDelay <= 0 && statLife < statLifeMax2 * 0.4f)
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
                                    case ItemID.AegisCrystal:
                                        if (!usedAegisCrystal)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.AegisFruit:
                                        if (!usedAegisFruit)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.ArcaneCrystal:
                                        if (!usedArcaneCrystal)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.Ambrosia:
                                        if (!usedAmbrosia)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.GummyWorm:
                                        if (!usedGummyWorm)
                                        {
                                            StatusIncreaseItem = i;
                                        }
                                        break;
                                    case ItemID.GalaxyPearl:
                                        if (!usedGalaxyPearl)
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

        public bool IsPlayerBuddy(Player player)
        {
            return PlayerMod.GetIsPlayerBuddy(player, this);
        }

        public void UpdateExtra()
        {
            UpdateComfortStack();
            if (Owner != null && velocity.X != 0)
            {
                Data.FriendshipProgress.IncreaseTravellingStack(velocity.X);
            }
            HeartDisplay.Update(this);
            //
            /*fullRotationOrigin.X = 0.5f * width;
            fullRotationOrigin.Y = 0.5f * height;
            fullRotation = MathHelper.ToRadians((float)Main.time);*/
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

        public bool IsPartnerOf(Companion companion)
        {
            CompanionID? id = Base.IsPartnerOf;
            return id.HasValue && id.Value.IsSameID(companion.GetCompanionID);
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
                            int Width = width, Height = height;
                            width = 40;
                            height = 56;
                            position.X += (Width - width) * .5f;
                            position.Y += (Height - height);
                            sitting.SitDown(this, furniturex, furniturey);
                            position.X -= (Width - width) * .5f;
                            position.Y -= (Height - height);
                            width = Width;
                            height = Height;
                        }
                        else
                        {
                            if(IsBedUseable(furniturex, furniturey))
                            {
                                int Width = width, Height = height;
                                width = 40;
                                height = 56;
                                position.X += (Width - width) * .5f;
                                position.Y += (Height - height);
                                sleeping.StartSleeping(this, furniturex, furniturey);
                                position.X -= (Width - width) * .5f;
                                position.Y -= (Height - height);
                                width = Width;
                                height = Height;
                            }
                            else
                            {
                                LeaveFurniture();
                                return;
                            }
                        }
                        if (sitting.isSitting || sleeping.isSleeping)
                        {
                            reachedfurniture = true;
                            Path.CancelPathing(false);
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
                int Count = Main.sleepingManager.GetNextPlayerStackIndexInCoords(new Point(x, y));
                return Count < 2;
            }
            return false;
        }

        private void UpdateControlledBehavior()
        {
            if (CharacterControllingMe == null || CompanionHasControl) return;
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
            controlHook = p.controlHook;
            selectedItem = p.selectedItem;
            controlQuickHeal = p.controlQuickHeal;
            controlQuickMana = p.controlQuickMana;
            controlHook = p.controlHook;
            GetAimedPosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            ApplyCompanionMousePosition();
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
                    Path.CancelPathing(false);
                }
            }
        }

        internal int FestiveHatSetup()
        {
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                return 195;
            }
            else if (Main.xMas)
            {
                if (Base.Gender == Genders.Female)
                    return 140;
                else
                    return 44;
            }
            return -1;
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
                    Path.CancelPathing(false);
                }
                else
                {
                    return;
                }
            }
            if (KnockoutStates > KnockoutStates.Awake)
            {
                ToggleMount(CharacterMountedOnMe, true);
                return;
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
                        if(mount.dead || tongued || PlayerMod.GetPlayerKnockoutState(mount) > KnockoutStates.Awake)
                        {
                            ToggleMount(mount, true);
                            return;
                        }
                        if(itemAnimation == 0)
                        {
                            direction = mount.direction;
                        }
                        bool InMineCart = mount.mount.Active && MountID.Sets.Cart[mount.mount.Type];
                        Vector2 MountPosition = Vector2.Zero;
                        //Implement the rest later.
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
                                Vector2 SittingPosition = m.GetAnimationPosition(AnimationPositions.MountShoulderPositions, Frame);
                                MountPosition.X = SittingPosition.X - MountPosition.X;
                                MountPosition.Y = SittingPosition.Y - MountPosition.Y + mount.gfxOffY;// - SpriteHeight;
                            }
                            else
                            {
                                Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, Frame, 0, false, BottomCentered: true);// HandPositionCollection.GetPositionFromFrame(Frame);
                                if (HandPosition == HandPositionCollection.DefaultCoordinate)
                                {
                                    HandPosition = HandPositionCollection.GetPositionFromFrame(Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrameFromPercentage(0.8f));
                                    MountPosition.X = mount.Center.X - 18 * direction;
                                    MountPosition.Y = mount.position.Y + mount.height + 6 + mount.gfxOffY - SpriteHeight + HandPosition.Y;
                                }
                                else
                                {
                                    MountPosition.X = mount.Center.X - 12f * direction + HandPosition.X;
                                    MountPosition.Y = mount.position.Y - 14f + mount.gfxOffY + HandPosition.Y;
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
                            MountPosition = GetAnimationPosition(AnimationPositions.SittingPosition, BodyFrameID, AlsoTakePosition: false);
                            if (!(mount is Companion)) MountPosition.X += SpriteWidth * 0.5f * direction;
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
                        ModifyMountedCharacterPosition(mount, ref MountPosition);
                        Base.ModifyMountedCharacterPosition(this, mount, ref MountPosition);
                        position = MountPosition;
                        if (mount.whoAmI > whoAmI) position += mount.velocity;
                        //Companion PlayerMount = PlayerMod.PlayerGetMountedOnCompanion(mount);
                        /*if (PlayerMount != null)
                        {
                            position += PlayerMount.velocity;
                        }*/
                        velocity = Vector2.Zero; //mount.velocity;
                        SetFallStart();
                        ControlJump = false;
                        //DrawOrderInfo.AddDrawOrderInfo(this, CharacterMountedOnMe, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
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
            if(!Dialogue.InDialogue || IsRunningBehavior || !Dialogue.IsParticipatingDialogue(this)) return;
            if(IsBeingControlledBySomeone || !CompanionHasControl)
                return;
            if(Behaviour_AttackingSomething)
            {
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
            float FaceDirectionX = WaitLocationX;
            if (Dialogue.Speaker != this)
            {
                FaceDirectionX = Dialogue.Speaker.position.X + Dialogue.Speaker.width * .5f;
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
            bool ToLeft = CenterX > FaceDirectionX;
            if(CenterX < WaitLocationX)
            {
                //WaitLocationX -= Dialogue.DistancingLeft + WaitDistance * 0.5f + 12;
                Dialogue.DistancingLeft += WaitDistance;
            }
            else
            {
                //WaitLocationX += Dialogue.DistancingRight + WaitDistance * 0.5f + 12;
                Dialogue.DistancingRight += WaitDistance;
            }
            float Distance = MathF.Abs(CenterX - WaitLocationX) - WaitDistance;
            if(!Is2PCompanion && Distance > 8f)
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
                    direction = ToLeft ? -1 : 1;
                }
            }
        }

        public void ChangeTarget(Entity NewTarget)
        {
            Target = NewTarget;
            if (combatBehavior is CombatBehavior)
                (combatBehavior as CombatBehavior).OnTargetChange(this, NewTarget);
        }

        public void LookForTargets()
        {
            if(Target != null && (!Target.active || (Target is Player && (((Player)Target).dead || !IsHostileTo((Player)Target)))))
            {
                Target = null;
            }
            float NearestDistance = 600f;
            Entity NewTarget = null;
            Vector2 MyCenter = Center;
            Vector2 CollisionPos = GetCollisionPosition;
            for (int i = 0; i < 255; i++)
            {
                if (i < 200 && Main.npc[i].active)
                {
                    NPC npc = Main.npc[i];
                    if(GetGoverningBehavior().CanTargetNpcs && !npc.friendly && npc.CanBeChasedBy(null))
                    {
                        float Distance = (MyCenter - npc.Center).Length();
                        if(Distance < NearestDistance && Collision.CanHit(CollisionPos, defaultWidth, defaultHeight, npc.position, npc.width, npc.height))
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
                ChangeTarget(NewTarget);
            }
        }

        public void CheckIfNeedToJumpTallTile()
        {
            if (UnallowAutoJump) return;
            if(CanDoJumping)
            {
                float MovementDirection = controlLeft ? -1f : (controlRight ? 1f : direction);
                int TileX = (int)((Center.X + 11f * MovementDirection + velocity.X) * DivisionBy16);
                int TileY = (int)((Bottom.Y - 1) * DivisionBy16);
                byte BlockedTiles = 0, Gap = 0;
                int MaxTilesY = (int)(GetMaxJumpHeight * DivisionBy16 + 2) + 3;
                int XCheckStart = (int)((position.X + width * 0.5f - 10) * DivisionBy16), XCheckEnd = (int)((position.X + width * 0.5f + 10) * DivisionBy16);
                for(byte i = 0; i < MaxTilesY; i++)
                {
                    Tile tile/* = Main.tile[TileX, TileY - 3 - i]*/;
                    bool Blocked = false;
                    for(int x = XCheckStart; x <= XCheckEnd; x++)
                    {
                        tile = Main.tile[x, TileY - 3 - i];
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
                if(BlockedTiles >= 1 && Gap >= 3 && (CanJump || jump > 0 || (releaseJump && AnyExtraJumpUsable())))
                {
                    controlJump = true;
                }
            }
        }

        public void ChangeSkin(byte ID, string ModID = "")
        {
            _SelectedSkin = Base.GetSkin(ID, ModID);
            SkinID = ID;
            SkinModID = ModID;
        }

        public void ChangeOutfit(byte ID, string ModID = "")
        {
            _SelectedOutfit = Base.GetOutfit(ID, ModID);
            OutfitID = ID;
            OutfitModID = ModID;
        }

        public bool IsSkinActive(byte ID, string ModID = "")
        {
            return SkinID == ID && SkinModID == ModID;
        }

        public bool IsOutfitActive(byte ID, string ModID = "")
        {
            return OutfitID == ID && OutfitModID == ModID;
        }

        public void ChangeOwner(Player NewOwner)
        {
            Owner = NewOwner;
            if (Owner == null)
                CreativePowerManager.Instance.ResetPowersForPlayer(this);
        }

        public void InitializeCompanion(bool Spawn = false)
        {
            PreInitialize();
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            //CreativePowerManager.Instance.ResetDataForNewPlayer(this);
            name = Data.GetName;
            inventory = Data.Inventory;
            armor = Data.Equipments;
            miscEquips = Data.MiscEquipment;
            dye = Data.EquipDyes;
            miscDyes = Data.MiscEquipDyes;
            InternalDelay = (byte)Main.rand.Next(10);
            for (int i = 0; i < CompanionData.MaxSubAttackSlots; i++)
            {
                if (SubAttackIndexes[i] < SubAttackList.Count)
                {
                    SelectedSubAttack = (byte)i;
                    break;
                }
            }
            //float HealthPercentage = Math.Clamp((float)statLife / statLifeMax2, 0, 1);
            ConsumedLifeCrystals = Data.LifeCrystalsUsed;
            ConsumedLifeFruit = Data.LifeFruitsUsed;
            ConsumedManaCrystals = Data.ManaCrystalsUsed;
            usedAegisCrystal = GetCommonData.VitalCrystalUsed;
            usedAegisFruit = GetCommonData.AegisFruitUsed;
            usedAmbrosia = GetCommonData.AmbrosiaUsed;
            usedArcaneCrystal = GetCommonData.ArcaneCrystalUsed;
            usedGalaxyPearl = GetCommonData.GalaxyPearlUsed;
            usedGummyWorm = GetCommonData.GummyWormUsed;
            for(int b = 0; b < MaxBuffs; b++)
            {
                if(b < Data.BuffType.Length)
                {
                    buffType[b] = Math.Max(0, Data.BuffType[b]);
                    buffTime[b] = Data.BuffTime[b];
                }
            }
            Data.BuffType = buffType;
            Data.BuffTime = buffTime;
            if (Base.IsGeneric)
            {
                SetCompanionLookBasedTerrarianInfos(Data.GetGenericCompanionInfo);
            }
            else if(Base.CompanionType == CompanionTypes.Terrarian)
            {
                SetCompanionLookBasedTerrarianInfos(Base.GetTerrarianCompanionInfo);
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
            if (Spawn) InitializeSubAttackSetting();
            UpdateMaxLifeAndMana();
            //
            const int PlayerToMask = 200;
            Player backupplayer = Main.player[PlayerToMask];
            Main.player[PlayerToMask] = this;
            whoAmI = PlayerToMask;
            UpdateStatus(false, false);
            //PlayerLoader.OnEnterWorld(this.whoAmI);
            Main.player[PlayerToMask] = backupplayer;
            backupplayer = null;
            //
            statLife = statLifeMax2;
            ScaleUpdate(true);
            isDisplayDollOrInanimate = true;
            //
            //mount.SetMount(1, this);
            mount.Dismount(this); //Better keep this.
            ChangeSkin(SkinID, SkinModID);
            ChangeOutfit(OutfitID, OutfitModID);
            PostInitialize();
            HeartDisplay.OnInitialize(this);
            //test
            //SetMount(Terraria.ID.MountID.WallOfFleshGoat);
        }

        protected virtual void PreInitialize()
        {

        }

        protected virtual void PostInitialize()
        {

        }

        void UpdateInventorySupplyStatus()
        {
            if (InternalDelay == 0)
                InventorySupplyStatus = new CompanionInventoryStatsContainer(this);
        }

        public void SetMount(int MountID)
        {
            if (MountID < 0 || MountID >= Mount.mounts.Length) return;
            mount.SetMount(MountID, this);
        }

        void SetCompanionLookBasedTerrarianInfos(TerrarianCompanionInfo info)
        {
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

        internal void UpdateLookBasedOnGenericInfos()
        {
            if (Data.IsGeneric)
            {
                SetCompanionLookBasedTerrarianInfos(Data.GetGenericCompanionInfo);
            }
        }

        private void InitializeSubAttackSetting()
        {
            Base.InitializeSubAttackLoading(ID, ModID);
            IReadOnlyList<SubAttackBase> SubAttackBases = Base.GetSubAttackBases;
            SubAttackList.Clear();
            for(byte i = 0; i < SubAttackBases.Count; i++)
            {
                SubAttackData SAD = SubAttackBases[i].GetSubAttackData;
                SAD.SetSubAttackInfos(this, i, SubAttackBases[i]);
                SubAttackList.Add(SAD);
            }
        }

        public void Teleport(Entity Target)
        {
            IsBeingPulledByPlayer = false;
            SuspendedByChains = false;
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
            immune = true;
            immuneNoBlink = true;
            AimDirection = Vector2.Zero;
            shimmering = false;
            shimmerWet = false;
            BordersMovement();
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
            Vector2 AimLocation = GetAimedPosition;
            return AimLocation.X >= TargetPosition.X && 
                AimLocation.Y >= TargetPosition.Y && 
                AimLocation.X < TargetPosition.X + TargetWidth && 
                AimLocation.Y < TargetPosition.Y + TargetHeight;
        }

        public void ChangeAimPosition(Vector2 NewPosition)
        {
            NewAimDirectionBackup = NewPosition - GetCompanionCenter;
        }

        bool LastNan = false;

        private void UpdateAimMovement()
        {
            if (Is2PCompanion || IsBeingControlledBySomeone)
                return;
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
                //Diference.Normalize();
                if (!LastNan)
                {
                    LastNan = Diference.HasNaNs();
                    /*Main.NewText("Aim Pos: " + AimDirection + "  Backup Pos: " + NewAimDirectionBackup);
                    Main.NewText("Diference Length: " + Distance + "  Is diference Nan? " + LastNan);
                    Main.NewText("Move Speed: " + MoveSpeed);*/
                }
                AimDirection += Vector2.Normalize(Diference) * MoveSpeed;
                if (AimDirection.HasNaNs())
                    AimDirection = Vector2.Zero;
            }
        }

        public void ApplyCompanionMousePosition()
        {
            if (IsBeingControlledBySomeone)
                return;
            Vector2 AimPosition = GetAimedPosition;
            Main.mouseX = (int)(AimPosition.X - Main.screenPosition.X);
            Main.mouseY = (int)(AimPosition.Y - Main.screenPosition.Y);
        }

        public static bool IsCompanion(Player player, uint ID, string ModID = "")
        {
            return PlayerMod.IsCompanion(player, ID, ModID);
        }

        public void AddItem(Item item, bool AvoidFirstSlots = false)
        {
            int StackCount = item.stack;
            int ItemType = item.type;
            for (int i = 0; i < 58; i++)
            {
                ChangeItemStacks(ref inventory[i], item);
                if (item.type == 0)
                {
                    OnInventoryStackChange(ItemType);
                    return;
                }
            }
            if (item.type >= ItemID.CopperCoin && item.type <= ItemID.PlatinumCoin)
            {
                for (int i = 50; i < 54; i++)
                {
                    ChangeItemStacks(ref inventory[i], item, true);
                    if (item.type == 0)
                    {
                        OnInventoryStackChange(ItemType);
                        return;
                    }
                }
            }
            else if (item.FitsAmmoSlot())
            {
                for (int i = 54; i < 58; i++)
                {
                    ChangeItemStacks(ref inventory[i], item, true);
                    if (item.type == 0)
                    {
                        OnInventoryStackChange(ItemType);
                        return;
                    }
                }
            }
            int StartingSlot = AvoidFirstSlots ? 10 : 0;
            for (int i = StartingSlot; i < 50; i++)
            {
                ChangeItemStacks(ref inventory[i], item, true);
                if (item.type == 0)
                {
                    OnInventoryStackChange(ItemType);
                    return;
                }
            }
            if (StackCount != item.stack)
                OnInventoryStackChange(ItemType);
        }

        public void ChangeItemStacks(ref Item ItemToChangeStack, Item ItemToDeplete, bool CreateNewIfPossible = false)
        {
            if (CreateNewIfPossible && ItemToChangeStack.type == 0)
            {
                ItemToChangeStack = ItemToDeplete.Clone();
                ItemToDeplete.SetDefaults(0);
                return;
            }
            if (ItemToChangeStack.type == ItemToDeplete.type && ItemToChangeStack.stack < ItemToChangeStack.maxStack)
            {
                int StackToChange = ItemToChangeStack.maxStack - ItemToChangeStack.stack;
                if (StackToChange > ItemToDeplete.stack)
                {
                    StackToChange = ItemToDeplete.stack;
                }
                ItemToChangeStack.stack += StackToChange;
                ItemToDeplete.stack -= StackToChange;
                if (ItemToDeplete.stack <= 0)
                {
                    ItemToDeplete.SetDefaults(0);
                }
            }
        }

        public void DepleteItemStacks(int Type, int Stack = 1)
        {
            if (Stack < 1) return;
            bool StackChanged = false;
            for (int i = 0; i < 58; i++)
            {
                if (inventory[i].type == Type)
                {
                    int StackToDeplete = inventory[i].maxStack - inventory[i].stack;
                    if (StackToDeplete > Stack)
                        StackToDeplete = Stack;
                    inventory[i].stack -= StackToDeplete;
                    Stack -= StackToDeplete;
                    StackChanged = true;
                    if (inventory[i].stack <= 0)
                    {
                        inventory[i].SetDefaults(0);
                    }
                    if (Stack <= 0)
                    {
                        OnInventoryStackChange(Type);
                        return;
                    }
                }
            }
            if (StackChanged)
                OnInventoryStackChange(Type);
        }

        public void OnInventoryStackChange(int ItemID)
        {
            OnUpdateInventory();
        }

        public void OnUpdateInventory()
        {
            if (!IsRunningBehavior)
            {
                if (Owner != null && Data.AutoSellItemsWhenInventoryIsFull)
                {
                    bool AnyOpenSlot = false, AnyLootToSell = false;
                    for (int i = 10; i < 50; i++)
                    {
                        if (inventory[i].type == 0)
                        {
                            AnyOpenSlot = true;
                            break;
                        }
                        if (!inventory[i].favorited)
                        {
                            AnyLootToSell = true;
                        }
                    }
                    if (AnyLootToSell && !AnyOpenSlot)
                    {
                        RunBehavior(new Behaviors.Actions.SellLootAction());
                    }
                }
            }
        }

        #region Other Hooks
        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        public virtual void CompanionDrawHeadSetup(PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        public virtual void UpdateAttributes()
        {
            
        }

        public virtual void ModifyAnimation()
        {

        }

        public virtual void PostUpdateAnimation()
        {

        }

        public virtual void UpdateCompanionHook()
        {

        }

        public virtual void UpdateBehaviorHook()
        {

        }

        public virtual bool FreeDodge(Player.HurtInfo info)
        {
            return false;
        }

        public virtual bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return false;
        }

        public virtual void ModifyHurt(ref Player.HurtModifiers modifiers)
        {

        }

        public virtual void OnAttackedByPlayer(Player attacker, int Damage, bool Critical)
        {
            
        }

        public virtual void OnAttackedByNpc(NPC attacker, int Damage, bool Critical)
        {
            
        }

        public virtual void OnAttackedByProjectile(Projectile proj, int Damage, bool Critical)
        {
            
        }

        public virtual void ModifyMountedCharacterPosition(Player MountedCharacter, ref Vector2 Position)
        {
            
        }
        #endregion

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
            if (!HasBeenMet || MainMod.IsDebugMode) return;
            Data.IncreaseSkillProgress(Progress, ID, ModID);
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
            DoResetEffects(false);
            ResetVisibleAccessories();
            UpdateMiscCounter();
            UpdateDyes();
            UpdateAnimations();
            RasterizerState state = MainMod.GetRasterizerState;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, state, (Effect)null, Main.UIScaleMatrix);
            DrawCompanion(context, UseSingleDrawScript);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, state, (Effect)null, Main.UIScaleMatrix);
        }

        public virtual void DrawCompanion(DrawContext context = DrawContext.AllParts, bool UseSingleDrawScript = false)
        {
            if (!UseSingleDrawScript)
            {
                Main.spriteBatch.End();
            }
            IPlayerRenderer renderer = Main.PlayerRenderer;
            SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
            Player BackedUpPlayer = Main.player[whoAmI];
            Main.player[whoAmI] = this;
            TerraGuardiansPlayerRenderer.DrawingCompanions = true;
            TerraGuardiansPlayerRenderer.SingleCompanionDraw = UseSingleDrawScript;
            TerraGuardiansPlayerRenderer.ChangeDrawContext(context);
            if(!UseSingleDrawScript)
            {
                renderer.DrawPlayers(Main.Camera, [this]);
            }
            else
            {
                renderer.DrawPlayer(Main.Camera, this, position, 0, fullRotationOrigin);
            }
            TerraGuardiansPlayerRenderer.DrawingCompanions = false;
            Main.player[whoAmI] = BackedUpPlayer;
            if (!UseSingleDrawScript) Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true, bool DiscountCharacterDimension = true, bool DiscountDirections = true, bool ConvertToCharacterPosition = true, bool BottomCentered = false)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if (BottomCentered)
            {
                //Bottom centered doesn't seems to be doing what it says...
                Position.X = (Position.X - Base.SpriteWidth * 0.5f) * (!DiscountDirections ? 1f : direction);
                Position.Y = (Position.Y - Base.SpriteHeight) * (!DiscountDirections ? 1f : gravDir);
            }
            else if (DiscountDirections)
            {
                if(direction < 0)
                    Position.X = Base.SpriteWidth - Position.X;
                if(gravDir < 0)
                    Position.Y = Base.SpriteHeight - Position.Y;
            }
            Position *= Scale;
            if(ConvertToCharacterPosition)
            {
                if (BottomCentered)
                {
                    Position.X -= width * 0.5f; //Maybe issue is here instead
                    Position.Y += -height + SpriteHeight;
                }
                else
                {
                    float XMod = width * 0.5f;
                    Position.Y += height;
                    if(DiscountCharacterDimension) 
                    {
                        XMod -= SpriteWidth * 0.5f;
                        Position.Y -= SpriteHeight;
                    }
                    Position.X += XMod;// * (DiscountDirections ? direction : 1f);
                }
            }
            if(AlsoTakePosition)
                Position += position + Vector2.UnitY * HeightOffsetHitboxCenter;
            if (this is TerraGuardian && Animation != AnimationPositions.BodyPositionOffset && Animation != AnimationPositions.ArmPositionOffset)
            {
                Position += Animation == AnimationPositions.HandPosition ? (this as TerraGuardian).ArmOffset[MultipleAnimationsIndex] : (this as TerraGuardian).BodyOffset;
            }
            return Position;
        }

        public Vector2 GetBetweenAnimationPosition(AnimationPositions Animation, short Frame, bool AlsoTakePosition = true, bool DiscountCharacterDimension = true, bool BottomCentered = false)
        {
            if(Base.GetHands <= 1)
                return GetAnimationPosition(Animation, Frame, 0, AlsoTakePosition, DiscountCharacterDimension, BottomCentered: BottomCentered);
            Vector2 OriginPosition = GetAnimationPosition(Animation, Frame, 0, false, DiscountCharacterDimension, BottomCentered: BottomCentered);
            Vector2 Position = OriginPosition + (GetAnimationPosition(Animation, Frame, 1, false, BottomCentered: BottomCentered) - OriginPosition) * 0.5f;
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
            if (MainMod.IsDebugMode)
            {
                return true;
            }
            return Owner == null && (IsStarter || FriendshipLevel >= Base.GetFriendshipUnlocks.FollowerUnlock);
        }

        public bool CanStopFollowingPlayer()
        {
            if ((Owner != null && PlayerMod.GetIsPlayerBuddy(Owner, this)) || GetPlayerMod.GetMountedOnCompanion != null || GetCharacterMountedOnMe != null) return false;
            return true;
        }

        public bool CanLiveHere(out bool LackFriendshipLevel)
        {
            LackFriendshipLevel = FriendshipLevel < Base.GetFriendshipUnlocks.MoveInUnlock;
            if (MainMod.IsDebugMode)
            {
               return true;
            }
            return IsStarter || !LackFriendshipLevel;
        }

        public bool CanAppointBuddy(out bool LackFriendship)
        {
            LackFriendship = FriendshipLevel < Base.GetFriendshipUnlocks.BuddyUnlock;
            if (MainMod.IsDebugMode) return true;
            return Base.CanBeAppointedAsBuddy && !LackFriendship;
        }

        public bool CanMount(Player Target)
        {
            return true;
        }

        public bool ToggleMount(Player Target, bool Forced = false)
        {
            if (!Forced && CCed) return false;
            if (IsBeingControlledBy(Target)) return false;
            {
                Companion controlled = PlayerMod.PlayerGetControlledCompanion(Target);
                if (controlled != null)
                    Target = controlled;
            }
            if (Target == this) return false;
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
                        RunBehavior(new Behaviors.Actions.MountDismountCompanionBehavior(this, Target, false));
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
                    RunBehavior(new Behaviors.Actions.MountDismountCompanionBehavior(this, Target, true));
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
            PlayerMod.PlayerAddCompanion(PlayerWhoMetHim, Data.IsStarter, ID, ModID);
        }

        public bool InDrawRange()
        {
            float DistanceX = Math.Abs(Center.X - (Main.screenPosition.X + Main.screenWidth * 0.5f)),
                DistanceY = Math.Abs(Center.Y - (Main.screenPosition.Y + Main.screenHeight * 0.5f));
            return DistanceX < Main.screenWidth * 0.5f + SpriteWidth + 200 && 
                    DistanceY < Main.screenHeight * 0.5f + SpriteHeight + 200;
        }

        public void BePulledByPlayer()
        {
            if (Owner != null && PlayerMod.PlayerGetMountedOnCompanion(this) == null && GetMountedOnCharacter == null && CharacterMountedOnMe != Owner && !IsBeingControlledBy(Owner))
            {
                if (!IsBeingPulledByPlayer)// && (KnockoutState == KnockoutStates.Awake))
                {
                    Path.CancelPathing(false);
                    IsBeingPulledByPlayer = true;
                    SuspendedByChains = false;
                    Target = null;
                }
            }
        }

        public bool UpdatePulledByPlayerAndIgnoreCollision(out bool LiquidCollision)
        {
            LiquidCollision = true;
            Companion m;
            if ((m = PlayerMod.PlayerGetMountedOnCompanion(this)) != null)
            {
                if (m.IsBeingPulledByPlayer)
                    return true;
            }
            if (!IsBeingPulledByPlayer) return false;
            if (dead || Owner == null || Owner.dead || (!Owner.gross && gross))
            {
                IsBeingPulledByPlayer = false;
                return false;
            }
            bool IgnoreCollision = false;
            Vector2 ResultingPosition = Owner.Center;
            Vector2 MovementDirection = ResultingPosition - Center;
            float Distance = MovementDirection.Length();
            if (GoingToOrUsingFurniture)
            {
                LeaveFurniture();
            }
            if (Distance >= 1512f || PlayerMod.GetPlayerKnockoutState(Owner) > KnockoutStates.Awake)
            {
                Teleport(Owner);
                return false;
            }
            float Speed = 12f;
            float OwnerSpeed = Owner.velocity.Length();
            if (Speed < OwnerSpeed)
            {
                Speed = OwnerSpeed + 6f;
            }
            bool OwnerIsFlying = Owner.velocity.Y != 0 || Owner.pulley,
                OwnerInAMount = Owner.mount.Active || PlayerMod.PlayerGetMountedOnCompanion(Owner) != null,
                OwnerInMineCart = Owner.mount.Active && Owner.mount.Cart;
            bool CollidingWithSolidTile = Collision.SolidCollision(position, width, height);
            if (CollidingWithSolidTile || !OwnerIsFlying || Distance >= 144f)
            {
                dashDelay = 30;
                if (Distance > 0)
                    MovementDirection.Normalize();
                if (SuspendedByChains && (OwnerIsFlying || OwnerInMineCart))
                {
                    velocity.Y -= gravity;
                    velocity += MovementDirection * (Distance - 144f) * 0.02f;
                }
                else
                {
                    MovementDirection *= Speed;
                    MovementDirection.Y -= gravity;
                    velocity += MovementDirection;
                    if (velocity.Length() > MovementDirection.Length())
                        velocity = MovementDirection;
                }
                position += velocity;
                IgnoreCollision = true;
            }
            else
            {
                if (!SuspendedByChains)
                {
                    SuspendedByChains = true;
                }
            }
            SetFallStart();
            if (Distance < Speed * 2 && !OwnerIsFlying && !OwnerInMineCart)
            {
                IsBeingPulledByPlayer = false;
                Teleport(Owner);
                FallProtection = true;
                velocity = Owner.velocity;
            }
            LiquidCollision = !SuspendedByChains && !IgnoreCollision;
            return IgnoreCollision;
        }

        public TgDrawInfoHolder GetNewDrawInfoHolder(PlayerDrawSet drawInfo)
        {
            DrawInfoHolder = new TgDrawInfoHolder(this, drawInfo);
            return DrawInfoHolder;
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
                if (Base.SitOnPlayerLapOnChair)
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
                switch(Base.MountedDrawOrdering)
                {
                    default: 
                        return CompanionDrawMomentTypes.DrawBehindOwner;
                    case PartDrawOrdering.InBetween:
                        return CompanionDrawMomentTypes.DrawOwnerInBetween;
                    case PartDrawOrdering.InFront:
                        return CompanionDrawMomentTypes.DrawInFrontOfOwner;
                }
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

        public string GetTranslation(string Key)
        {
            return Base.GetTranslation(Key);
        }

        public string GetTranslation(string Key, Mod mod)
        {
            return Base.GetTranslation(Key, mod);
        }
    }

    public struct CompanionInventoryStatsContainer
    {
        public byte HealthPotionsStatus, 
        ManaPotionsStatus, 
        ArrowsStatus, 
        BulletsStatus, 
        RocketsStatus,
        FoodStatus;

        public const byte STATUS_NEEDMORE = 2, STATUS_HASNONE = 1, STATUS_IGNORE = 0;

        public CompanionInventoryStatsContainer()
        {

        }

        public CompanionInventoryStatsContainer(Companion c)
        {
            bool NeedManaPot = false, NeedArrow = false, NeedBullets = false, NeedRockets = false;
            int HPPotsCount = 0, MPPotsCount = 0, ArrowCount = 0, BulletsCount = 0, RocketsCount = 0, FoodCount = 0;
            for (int i = 0; i < 58; i++)
            {
                if (c.inventory[i].type > 0)
                {
                    Item item = c.inventory[i];
                    if (i < 10)
                    {
                        if (item.useAmmo > AmmoID.None)
                        {
                            if (item.useAmmo == AmmoID.Arrow)
                                NeedArrow = true;
                            else if (item.useAmmo == AmmoID.Bullet)
                                NeedBullets = true;
                            else if (item.useAmmo == AmmoID.Rocket)
                                NeedRockets = true;
                        }
                        if (item.mana > 0)
                            NeedManaPot = true;
                    }
                    if (item.healLife > 0)
                    {
                        HPPotsCount += item.stack;
                    }
                    if (item.healMana > 0)
                    {
                        MPPotsCount += item.stack;
                    }
                    if (item.ammo > AmmoID.None)
                    {
                        if (item.ammo == AmmoID.Arrow)
                            ArrowCount += item.stack;
                        else if (item.ammo == AmmoID.Bullet)
                            BulletsCount += item.stack;
                        else if (item.ammo == AmmoID.Rocket)
                            RocketsCount += item.stack;
                    }
                    if (item.buffType == BuffID.WellFed || item.buffType == BuffID.WellFed2 || item.buffType == BuffID.WellFed3)
                    {
                        FoodCount += item.stack;
                    }
                }
            }
            if (HPPotsCount < 10)
            {
                HealthPotionsStatus = HPPotsCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
            if (NeedManaPot && MPPotsCount < 10)
            {
                ManaPotionsStatus = MPPotsCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
            if (NeedArrow && ArrowCount < 250)
            {
                ArrowsStatus = ArrowCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
            if (NeedBullets && BulletsCount < 250)
            {
                BulletsStatus = BulletsCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
            if (NeedRockets && RocketsCount < 250)
            {
                RocketsStatus = RocketsCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
            if (FoodCount < 3)
            {
                FoodStatus = FoodCount == 0 ? STATUS_HASNONE : STATUS_NEEDMORE;
            }
        }
    }

    public class HeartDisplayHelper
    {
        public byte LastLevel = 0;
        public sbyte LastProgress = 0;
        public byte LastMaxProgress = 0;
        public int AnimationTime = 0;

        const int FadingDuration = 90;
        const int DelayBetweenFillingAnimation = 120;
        const int FillingAnimationDuration = 150;
        int MaxDuration => (FadingDuration + DelayBetweenFillingAnimation) * 2 + FillingAnimationDuration;

        public void OnInitialize(Companion c)
        {
            LastLevel = c.FriendshipLevel;
            LastProgress = c.FriendshipExp;
            LastMaxProgress = c.FriendshipMaxExp;
        }

        public void Update(Companion c)
        {
            if (LastLevel != c.FriendshipLevel || LastProgress != c.FriendshipExp)
            {
                AnimationTime++;
                if (AnimationTime >= MaxDuration)
                {
                    AnimationTime = 0;
                    LastLevel = c.FriendshipLevel;
                    LastProgress = c.FriendshipExp;
                    LastMaxProgress = c.FriendshipMaxExp;
                }
            }
        }

        public void GetHeartDisplayProgress(Companion c, out DisplayStates State, out float Percentage)
        {
            State = DisplayStates.Hidden;
            Percentage = 0;
            if (AnimationTime > 0)
            {
                int Stack = 0;
                if(AnimationTime < FadingDuration)
                {
                    State = DisplayStates.FadingIn;
                    Percentage = (float)AnimationTime / FadingDuration;
                }
                else
                {
                    Stack += FadingDuration;
                    if (AnimationTime < DelayBetweenFillingAnimation + Stack)
                    {
                        State = DisplayStates.Delay;
                        Percentage = 0;
                    }
                    else
                    {
                        Stack += DelayBetweenFillingAnimation;
                        if (AnimationTime < FillingAnimationDuration + Stack)
                        {
                            State = DisplayStates.FillingDisplay;
                            Percentage = (float)(AnimationTime - Stack) / FillingAnimationDuration;
                        }
                        else
                        {
                            Stack += FillingAnimationDuration;
                            if (AnimationTime < DelayBetweenFillingAnimation + Stack)
                            {
                                State = DisplayStates.Delay;
                                Percentage = 1;
                            }
                            else
                            {
                                Stack += DelayBetweenFillingAnimation;
                                State = DisplayStates.FadingOut;
                                Percentage = MathF.Max(0, 1f - (float)(AnimationTime - Stack) / FadingDuration);
                            }
                        }
                    }
                }
            }
        }

        public enum DisplayStates : byte
        {
            Hidden = 0,
            FadingIn = 1,
            FillingDisplay = 2,
            FadingOut = 3,
            Delay = 4
        }
    }

    public class FollowOrderSetting
    {
        public float Distance = 0;
        public bool Front = false;
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