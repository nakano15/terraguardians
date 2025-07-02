using Terraria;
using Terraria.Audio;
using Terraria.IO;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;

namespace terraguardians
{
	public class MainMod : Mod
	{
		public const uint ModVersion = 50;
		public static int MaxCompanionFollowers { get { return _MaxCompanionFollowers; } set { if (Main.gameMenu) _MaxCompanionFollowers = (int)Math.Clamp(value, 0, 50); } }
		private static int _MaxCompanionFollowers = 5;
		public static int MyPlayerBackup = 0;
		public static Player GetLocalPlayer { get { return Main.player[MyPlayerBackup]; } }
		internal static Mod mod;
		internal static Mod GetMod { get { return mod; } }
		internal static string GetModName { get { return mod.Name; } }
		internal static Dictionary<string, CompanionHookContainer> ModCompanionHooks = new Dictionary<string, CompanionHookContainer>();
		private static Dictionary<string, CompanionContainer> ModCompanionContainer = new Dictionary<string, CompanionContainer>();
		public static Asset<Texture2D> LosangleOfUnknown;
		public static Asset<Texture2D> IronSwordTexture;
		public static Asset<Texture2D> CompanionInfoIconsTexture;
		public static Asset<Texture2D> ErrorTexture;
		public static Asset<Texture2D> PathGuideTexture;
		public static Asset<Texture2D> GuardianHealthBarTexture, GuardianInventoryInterfaceButtonsTexture, GuardianFriendshipHeartTexture, GuardianInfosNotificationTexture, ReviveBarsEffectTexture, ReviveHealthBarTexture;
		public static Asset<Texture2D> TrappedCatTexture;
		public static Asset<Texture2D> RenamePencilTexture;
		public static Asset<Texture2D> TGMouseTexture;
		public static Asset<Texture2D> NinjaTextureBackup;
		public static Asset<Texture2D> ContributorBadgeTexture;
		public static Asset<Texture2D> FlufflesCatchPlayerViewTexture;
		internal static Dictionary<uint, Companion> ActiveCompanions = new Dictionary<uint, Companion>();
		public static Companion[] GetActiveCompanions { get{ return ActiveCompanions.Values.ToArray();} }
		private static Dictionary<CompanionID, CompanionCommonData> CommonDatas = new Dictionary<CompanionID, CompanionCommonData>();
		private static List<CompanionID> StarterCompanions = new List<CompanionID>();
        public static List<CompanionID> GetStarterCompanions { get { return StarterCompanions; }}
		private static TerrariansGroup _terrariangroup = new TerrariansGroup();
		private static TerraGuardiansGroup _tggroup = new TerraGuardiansGroup();
		private static CaitSithGroup _csgroup = new CaitSithGroup();
		private static GiantDogGroup _gdgroup = new GiantDogGroup();
		public static TerrariansGroup GetTerrariansGroup { get { return _terrariangroup; } }
		public static TerraGuardiansGroup GetTerraGuardiansGroup { get { return _tggroup; } }
		public static CaitSithGroup GetCaitSithGroup { get { return _csgroup; } }
		public static GiantDogGroup GetGiantDogGroup { get { return _gdgroup; } }
		private static List<int> FemaleNpcs = new List<int>();
		public static Color SkillUpColor = new Color(132, 208, 192), 
			MysteryCloseColor = new Color(152, 90, 214), 
			BirthdayColor = new Color(112, 148, 192), 
            RecruitColor = Color.CornflowerBlue, 
			BountyProgressUpdate = Color.PaleGreen;
        public const int NemesisFadeCooldown = 15 * 60, NemesisFadingTime = 3 * 60;
		public static float NemesisFadeEffect = -NemesisFadeCooldown;
		public static bool UsePathfinding = true;
		internal static bool DebugMode = false, SkillsEnabled = true, DisableHalloweenJumpscares = false;
		internal static bool Gameplay2PMode = false, Gameplay2PInventory = false, Show2PNotification = true, ShowPathFindingTags = false, DebugPathFinding = false;
		internal static bool MoveLeft2P = false, MoveUp2P = false, MoveRight2P = false, MoveDown2P = false, Confirm2P = false, Cancel2P = false;
		internal static bool DisableModCompanions = false, EnableProfanity = true, IndividualCompanionProgress = false, IndividualCompanionSkillProgress = false, SharedHealthAndManaProgress = false, ShowBackwardAnimations = false, TeleportInsteadOfRopePull = false, EnableGenericCompanions = false;
		internal static bool PlayerKnockoutEnable = false, PlayerKnockoutColdEnable = false, 
			CompanionKnockoutEnable = true, CompanionKnockoutColdEnable = false, PreventKnockedOutDeath = false;
		public static CompanionMaxDistanceFromPlayer MaxDistanceFromPlayer { get{ return _MaxDistancePlayer; } internal set { _MaxDistancePlayer = value; } }
		static CompanionMaxDistanceFromPlayer _MaxDistancePlayer = CompanionMaxDistanceFromPlayer.Normal;
		public static float DamageNerfByCompanionCount = 0.1f;
		public const string TgGodName = "Raye Filos"; //(Rigé Filos)striped friend translated to Greek. Raye (Rayé) is striped in French.
		internal static List<Func<Player, Vector2, float>> GroupInterfaceBarsHooks = new List<Func<Player, Vector2, float>>();
		internal static List<int> DualWieldableWeapons = new List<int>();
		internal static PlayerIndex SecondPlayerPort = PlayerIndex.Two;
		private static GamePadState SecondPlayerControlState = new GamePadState(),
			oldSecondPlayerControlState = new GamePadState();
		private static RasterizerState MagicRasterizerOfAwesomeness = new RasterizerState() { CullMode = 0, ScissorTestEnable = true };
		public static RasterizerState GetRasterizerState { get { return MagicRasterizerOfAwesomeness; } }
		public const float Deg2Rad = 0.0174533f, Rad2Deg = 57.2958f;
		public static ModKeybind UseSubAttackKey, ScrollPreviousSubAttackKey, ScrollNextSubAttackKey, OpenOrderWindowKey;
		internal static List<int> HeadgearAbleEquipments = new List<int>();
		internal static bool StarlightRiverModInstalled = false, MrPlagueRacesInstalled = false;
		internal static float FlufflesHauntOpacity = 0;
		public static float GetGhostColorMod {
			get
			{
				return MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 3) * .3f + .3f;
			}
		}
		static Dictionary<int, WeaponProfile> WeaponProfiles = new Dictionary<int, WeaponProfile>();
		public static bool IsDebugMode => DebugMode || (!Main.gameMenu && GetLocalPlayer.GetModPlayer<PlayerMod>().IsDebugModeCharacter);

		public static bool IsNpcFemale(int ID)
		{
			return FemaleNpcs.Contains(ID);
		}

		public override void Load()
        {
			mod = this;
			CompanionCommonData.OnLoad();
			AddCompanionDB(new CompanionDB(), this);
			nterrautils.QuestContainer.AddQuestContainer(this, new QuestDB());
			if(Main.netMode < 2)
			{
				ErrorTexture = ModContent.Request<Texture2D>("terraguardians/Content/ErrorTexture");
				PathGuideTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/PathGuide");
				LosangleOfUnknown = ModContent.Request<Texture2D>("terraguardians/Content/LosangleOfUnnown");
				TGMouseTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianMouse");
				CompanionInfoIconsTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/infoicons");
				GuardianHealthBarTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianHealthBar");
				GuardianFriendshipHeartTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/FriendshipHeart");
				GuardianInventoryInterfaceButtonsTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianEquipButtons");
				GuardianInfosNotificationTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/CompanionInfosNotificationIcons");
				RenamePencilTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/EditButton");
				ReviveBarsEffectTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/KnockoutEffect");
				ReviveHealthBarTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/ReviveHealthBar");
				TrappedCatTexture = ModContent.Request<Texture2D>("terraguardians/Content/Extra/TrappedCat");
				ContributorBadgeTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/Contributor_Icon");
				IronSwordTexture = ModContent.Request<Texture2D>("terraguardians/Items/Weapons/TwoHandedSword");
				FlufflesCatchPlayerViewTexture = ModContent.Request<Texture2D>("terraguardians/Content/Extra/FlufflesCatchView");
				NinjaTextureBackup = TextureAssets.Ninja;
				Main.PlayerRenderer = new TerraGuardiansPlayerRenderer();
				UseSubAttackKey = KeybindLoader.RegisterKeybind(this, "UseSubAttack", "G");
				ScrollPreviousSubAttackKey = KeybindLoader.RegisterKeybind(this, "ScrollPreviousSubAttack", "Q");
				ScrollNextSubAttackKey = KeybindLoader.RegisterKeybind(this, "ScrollNextSubAttack", "E");
				OpenOrderWindowKey = KeybindLoader.RegisterKeybind(this, "OpenOrderWindow", "'");
			}
			GenericCompanionRandomizer.Initialize();
            Companions.AlexanderDefaultStatusBoosts.SetDefaultBonuses();
			PersonalityDB.Load();
			SardineBountyBoard.OnModLoad();
			StarterCompanions.Add(new CompanionID(CompanionDB.Rococo));
			StarterCompanions.Add(new CompanionID(CompanionDB.Blue));
			Companions.MinervaBase.Initialize();
			SetupDualwieldable();
			PopulateFemaleNpcsList();
			SetupHatableEquipments();
			SetupWeaponProfiles();
		}
		
        public override void Unload()
        {
			CompanionContainer.UnloadStatic();
			foreach(string Mod in ModCompanionContainer.Keys) ModCompanionContainer[Mod].Unload();
			ModCompanionContainer.Clear();
			CompanionCommonData.OnUnload();
			CommonDatas.Clear();
			CommonDatas = null;
			WorldMod.OnUnload();
			StarterCompanions.Clear();
			StarterCompanions = null;
			TextureAssets.Ninja = NinjaTextureBackup;
			_tggroup = null;
			_terrariangroup = null;
			_csgroup = null;
			_gdgroup = null;
			FemaleNpcs.Clear();
			FemaleNpcs = null;
			Main.PlayerRenderer = new Terraria.Graphics.Renderers.LegacyPlayerRenderer();
			RequestContainer.Unload();
			RequestReward.Unload();
			CompanionSkillContainer.Unload();
			DrawOrderInfo.Unload();
			CompanionSpawningTips.Unload();
			GenericCompanionInfos.Unload();
			{
				ErrorTexture = null;
				PathGuideTexture = null;
				LosangleOfUnknown = null;
				TGMouseTexture = null;
				CompanionInfoIconsTexture = null;
				GuardianHealthBarTexture = null;
				GuardianFriendshipHeartTexture = null;
				GuardianInventoryInterfaceButtonsTexture = null;
				RenamePencilTexture = null;
				ReviveBarsEffectTexture = null;
				ReviveHealthBarTexture = null;
				TrappedCatTexture = null;
				ContributorBadgeTexture = null;
				IronSwordTexture = null;
				FlufflesCatchPlayerViewTexture = null;
			}
			CompanionSelectionInterface.Unload();
			PersonalityDB.Unload();
			CompanionHeadsMapLayer.OnUnload();
			BuddyModeSetupInterface.Unload();
			GroupInterfaceBarsHooks.Clear();
			GroupInterfaceBarsHooks = null;
			SardineBountyBoard.Unload();
			Companions.MinervaBase.Unlock();
			ModCompatibility.NExperienceModCompatibility.Unload();
			ModCompatibility.ThoriumModCompatibility.Unload();
			ModCompatibility.CalamityModCompatibility.Unload();
			ModCompatibility.TerrariaOverhaulMod.Unload();
			ModCompatibility.MrPlagueRacesMod.Unload();
			BehaviorBase.Unload();
			Interfaces.CompanionOrderInterface.OnUnload();
			CompanionInventoryInterface.Unload();
			Companions.VladimirBase.CarryBlacklist.Clear();
			Companions.VladimirBase.CarryBlacklist = null;
			GenericCompanionRandomizer.Unload();
			foreach(string s in ModCompanionHooks.Keys)
			{
				ModCompanionHooks[s].Unload();
			}
			ModCompanionHooks.Clear();
			ModCompanionHooks = null;
			UseSubAttackKey = null;
			ScrollPreviousSubAttackKey = null;
			ScrollNextSubAttackKey = null;
			MagicRasterizerOfAwesomeness = null;
			mod = null;
			HeadgearAbleEquipments.Clear();
			HeadgearAbleEquipments = null;
			WeaponProfiles.Clear();
			WeaponProfiles = null;
		}

		public static void AddStarterCompanion(uint ID, string ModID = "")
		{
			CompanionID id = new CompanionID(ID, ModID);
			foreach(CompanionID id2 in StarterCompanions)
			{
				if (id2.IsSameID(id)) return;
			}
			StarterCompanions.Add(id);
		}

		public static void ClearStarterCompanions()
		{
			StarterCompanions.Clear();
		}

		public static void RemoveStarterCompanion(uint ID, string ModID = "")
		{
			for (int i = 0; i < StarterCompanions.Count; i++)
			{
				if (StarterCompanions[i].IsSameID(ID, ModID))
				{
					StarterCompanions.RemoveAt(i);
				}
			}
		}

		private static void SetupHatableEquipments()
		{
			AddTGWearableHat(1, 8, 11, 13, 14, 15, 16, 18, 
			19, 21, 24, 25, 26, 29, 33, 34, 35, 40, 41, 
			42, 44, 50, 51, 52, 53, 54, 55, 56, 62, 63,
			64, 65, 67, 68, 71, 72, 73, 75, 80, 81, 92, 
			94, 95, 96, 100, 106, 113, 116, 126, 130, 
			133, 138, 139, 140, 143, 144, 145, 158, 159, 
			161, 163, 166, 167, 181, 182, 183, 184, 188, 
			190, 195, 197, 199, 203, 205, 215, 217, 218, 
			219, 222, 223, 224, 225, 226, 227, 228, 229, 
			231, 232, 233, 234, 235, 242, 243, 244, 245, 
			250, 252, 253, 254, 256, 257, 259, 262, 263, 
			264, 265, 267, 272, 273, 275, 278, 279, 280);
		}

		public static void AddTGWearableHat(params int[] hatid)
		{
			foreach (int i in hatid)
				HeadgearAbleEquipments.Add(i);
		}

		static void SetupWeaponProfiles()
		{
			//Yoyo
			RegisterWeaponProfile(ItemID.WoodYoyo, new WeaponProfile().SetTilesInRange(7f));
			RegisterWeaponProfile(ItemID.Rally, new WeaponProfile().SetTilesInRange(10.5f));
			RegisterWeaponProfile(3279, new WeaponProfile().SetTilesInRange(12f));
			RegisterWeaponProfile(3280, new WeaponProfile().SetTilesInRange(13f));
			RegisterWeaponProfile(3281, new WeaponProfile().SetTilesInRange(13.5f));
			RegisterWeaponProfile(3262, new WeaponProfile().SetTilesInRange(13.5f));
			RegisterWeaponProfile(3317, new WeaponProfile().SetTilesInRange(14f));
			RegisterWeaponProfile(3282, new WeaponProfile().SetTilesInRange(14.5f));
			RegisterWeaponProfile(ItemID.HiveFive, new WeaponProfile().SetTilesInRange(14f));
			RegisterWeaponProfile(ItemID.FormatC, new WeaponProfile().SetTilesInRange(14.5f));
			RegisterWeaponProfile(ItemID.Gradient, new WeaponProfile().SetTilesInRange(15.5f));
			RegisterWeaponProfile(ItemID.Chik, new WeaponProfile().SetTilesInRange(17f));
			RegisterWeaponProfile(ItemID.HelFire, new WeaponProfile().SetTilesInRange(17f));
			RegisterWeaponProfile(ItemID.Amarok, new WeaponProfile().SetTilesInRange(17f));
			RegisterWeaponProfile(ItemID.Code2, new WeaponProfile().SetTilesInRange(17.5f));
			RegisterWeaponProfile(ItemID.Yelets, new WeaponProfile().SetTilesInRange(18f));
			RegisterWeaponProfile(3287, new WeaponProfile().SetTilesInRange(23f));
			RegisterWeaponProfile(ItemID.ValkyrieYoyo, new WeaponProfile().SetTilesInRange(23f));
			RegisterWeaponProfile(ItemID.Kraken, new WeaponProfile().SetTilesInRange(21f));
			RegisterWeaponProfile(ItemID.TheEyeOfCthulhu, new WeaponProfile().SetTilesInRange(22.5f));
			RegisterWeaponProfile(ItemID.Terrarian, new WeaponProfile().SetTilesInRange(25f));

			//Boomerang
			RegisterWeaponProfile(ItemID.WoodenBoomerang, new WeaponProfile().SetTilesInRange(13f));
			RegisterWeaponProfile(ItemID.EnchantedBoomerang, new WeaponProfile().SetTilesInRange(22f));
			RegisterWeaponProfile(ItemID.FruitcakeChakram, new WeaponProfile().SetTilesInRange(23f));
			RegisterWeaponProfile(ItemID.BloodyMachete, new WeaponProfile().SetTilesInRange(10f));
			RegisterWeaponProfile(ItemID.Shroomerang, new WeaponProfile().SetTilesInRange(23f));
			RegisterWeaponProfile(ItemID.IceBoomerang, new WeaponProfile().SetTilesInRange(26f));
			RegisterWeaponProfile(ItemID.ThornChakram, new WeaponProfile().SetTilesInRange(28f));
			RegisterWeaponProfile(ItemID.CombatWrench, new WeaponProfile().SetTilesInRange(18f));
			RegisterWeaponProfile(ItemID.Flamarang, new WeaponProfile().SetTilesInRange(27f));
			RegisterWeaponProfile(ItemID.Trimarang, new WeaponProfile().SetTilesInRange(26f).SetLaunchLimit(3));
			RegisterWeaponProfile(ItemID.FlyingKnife, new WeaponProfile().SetTilesInRange(18f));
			RegisterWeaponProfile(ItemID.BouncingShield, new WeaponProfile().SetTilesInRange(22f));
			RegisterWeaponProfile(ItemID.LightDisc, new WeaponProfile().SetTilesInRange(32f).SetLaunchLimit(5));
			RegisterWeaponProfile(ItemID.Bananarang, new WeaponProfile().SetTilesInRange(32f).SetLaunchLimit(10));
			RegisterWeaponProfile(ItemID.PossessedHatchet, new WeaponProfile().SetTilesInRange(23f));
			RegisterWeaponProfile(ItemID.PaladinsHammer, new WeaponProfile().SetTilesInRange(15f));

			//Flails
			RegisterWeaponProfile(5011, new WeaponProfile().SetFlail().SetTilesInRange(10f)); //Mace
			RegisterWeaponProfile(5012, new WeaponProfile().SetFlail().SetTilesInRange(10f)); //Flaming Mace
			RegisterWeaponProfile(ItemID.BallOHurt, new WeaponProfile().SetFlail().SetTilesInRange(14f));
			RegisterWeaponProfile(ItemID.TheMeatball, new WeaponProfile().SetFlail().SetTilesInRange(14f));
			RegisterWeaponProfile(ItemID.BlueMoon, new WeaponProfile().SetFlail().SetTilesInRange(16f));
			RegisterWeaponProfile(ItemID.Sunfury, new WeaponProfile().SetFlail().SetTilesInRange(18f));
			RegisterWeaponProfile(ItemID.ChainKnife, new WeaponProfile().SetTilesInRange(10f));
			RegisterWeaponProfile(ItemID.DripplerFlail, new WeaponProfile().SetFlail().SetTilesInRange(20f));
			RegisterWeaponProfile(ItemID.DaoofPow, new WeaponProfile().SetFlail().SetTilesInRange(19f));
			RegisterWeaponProfile(ItemID.FlowerPow, new WeaponProfile().SetFlail().SetTilesInRange(20f));
			RegisterWeaponProfile(ItemID.Anchor, new WeaponProfile().SetTilesInRange(20f));
			RegisterWeaponProfile(ItemID.ChainGuillotines, new WeaponProfile().SetTilesInRange(30f));
			RegisterWeaponProfile(ItemID.KOCannon, new WeaponProfile().SetTilesInRange(13f));
			RegisterWeaponProfile(ItemID.GolemFist, new WeaponProfile().SetTilesInRange(30f));
			RegisterWeaponProfile(ItemID.Flairon, new WeaponProfile().SetTilesInRange(24f));

			//Magic Items
			RegisterWeaponProfile(ItemID.CrimsonRod, new WeaponProfile().SetTilesInRange(30f));
			RegisterWeaponProfile(ItemID.WandofSparking, new WeaponProfile().SetTilesInRange(16f));
			RegisterWeaponProfile(ItemID.WandofFrosting, new WeaponProfile().SetTilesInRange(16f));
		}

		public static void RegisterWeaponProfile(int ItemID, WeaponProfile NewProfile)
		{
			if (WeaponProfiles.ContainsKey(ItemID))
				WeaponProfiles[ItemID] = NewProfile;
			else
				WeaponProfiles.Add(ItemID, NewProfile);
		}

		public static WeaponProfile GetWeaponProfile(int ItemID)
		{
			if (WeaponProfiles.ContainsKey(ItemID))
				return WeaponProfiles[ItemID];
			return null;
		}

		private void SetupDualwieldable()
		{
			DualWieldableWeapons.Add(ItemID.WoodenSword);
            DualWieldableWeapons.Add(ItemID.BorealWoodSword);
            DualWieldableWeapons.Add(ItemID.CopperBroadsword);
            DualWieldableWeapons.Add(ItemID.PalmWoodSword);
            DualWieldableWeapons.Add(ItemID.RichMahoganySword);
            DualWieldableWeapons.Add(ItemID.CactusSword);
            DualWieldableWeapons.Add(ItemID.EbonwoodSword);
            DualWieldableWeapons.Add(ItemID.IronBroadsword);
            DualWieldableWeapons.Add(ItemID.ShadewoodSword);
            DualWieldableWeapons.Add(ItemID.LeadBroadsword);
            DualWieldableWeapons.Add(ItemID.BladedGlove);
            DualWieldableWeapons.Add(ItemID.TungstenBroadsword);
            DualWieldableWeapons.Add(ItemID.ZombieArm);
            DualWieldableWeapons.Add(ItemID.GoldBroadsword);
            DualWieldableWeapons.Add(ItemID.AntlionClaw); //This is actually the Mandible Blade
            DualWieldableWeapons.Add(ItemID.StylistKilLaKillScissorsIWish);
            DualWieldableWeapons.Add(ItemID.PlatinumBroadsword);
            DualWieldableWeapons.Add(ItemID.BoneSword);
            DualWieldableWeapons.Add(ItemID.Katana);
            DualWieldableWeapons.Add(ItemID.IceBlade);
            DualWieldableWeapons.Add(ItemID.Muramasa);
            DualWieldableWeapons.Add(ItemID.Arkhalis);
            DualWieldableWeapons.Add(ItemID.DyeTradersScimitar);
            //Phaseblades and Phasesabers
            DualWieldableWeapons.Add(ItemID.BluePhaseblade);
            DualWieldableWeapons.Add(ItemID.BluePhasesaber);
            DualWieldableWeapons.Add(ItemID.GreenPhaseblade);
            DualWieldableWeapons.Add(ItemID.GreenPhasesaber);
            DualWieldableWeapons.Add(ItemID.PurplePhaseblade);
            DualWieldableWeapons.Add(ItemID.PurplePhasesaber);
            DualWieldableWeapons.Add(ItemID.RedPhaseblade);
            DualWieldableWeapons.Add(ItemID.RedPhasesaber);
            DualWieldableWeapons.Add(ItemID.WhitePhaseblade);
            DualWieldableWeapons.Add(ItemID.WhitePhasesaber);
            DualWieldableWeapons.Add(ItemID.YellowPhaseblade);
            DualWieldableWeapons.Add(ItemID.YellowPhasesaber);
            //PHM melee weapons continue
            DualWieldableWeapons.Add(ItemID.BloodButcherer);
            DualWieldableWeapons.Add(ItemID.Starfury);
            DualWieldableWeapons.Add(ItemID.EnchantedSword);
            DualWieldableWeapons.Add(ItemID.BeeKeeper);
            DualWieldableWeapons.Add(ItemID.FalconBlade);
            //HM melee weapons
            DualWieldableWeapons.Add(ItemID.PearlwoodSword);
            DualWieldableWeapons.Add(ItemID.TaxCollectorsStickOfDoom);
            DualWieldableWeapons.Add(ItemID.SlapHand);
            DualWieldableWeapons.Add(ItemID.CobaltSword);
            DualWieldableWeapons.Add(ItemID.PalladiumSword);
            DualWieldableWeapons.Add(3823); //Brand of Inferno
            DualWieldableWeapons.Add(ItemID.MythrilSword);
            DualWieldableWeapons.Add(ItemID.OrichalcumSword);
            DualWieldableWeapons.Add(ItemID.Cutlass);
            DualWieldableWeapons.Add(ItemID.Frostbrand);
            DualWieldableWeapons.Add(ItemID.AdamantiteSword);
            DualWieldableWeapons.Add(ItemID.BeamSword);
            DualWieldableWeapons.Add(ItemID.TitaniumSword);
            DualWieldableWeapons.Add(ItemID.FetidBaghnakhs);
            DualWieldableWeapons.Add(ItemID.Bladetongue);
            DualWieldableWeapons.Add(ItemID.Excalibur);
            DualWieldableWeapons.Add(ItemID.ChlorophyteSaber);
            DualWieldableWeapons.Add(ItemID.PsychoKnife);
            DualWieldableWeapons.Add(ItemID.Keybrand);
            DualWieldableWeapons.Add(ItemID.TheHorsemansBlade);
            DualWieldableWeapons.Add(ItemID.ChristmasTreeSword);
            DualWieldableWeapons.Add(ItemID.Seedler);
            DualWieldableWeapons.Add(ItemID.TerraBlade);
            DualWieldableWeapons.Add(ItemID.InfluxWaver);
            DualWieldableWeapons.Add(ItemID.StarWrath);

            //HM Repeaters
            DualWieldableWeapons.Add(ItemID.CobaltRepeater);
            DualWieldableWeapons.Add(ItemID.PalladiumRepeater);
            DualWieldableWeapons.Add(ItemID.MythrilRepeater);
            DualWieldableWeapons.Add(ItemID.OrichalcumRepeater);
            DualWieldableWeapons.Add(ItemID.AdamantiteRepeater);
            DualWieldableWeapons.Add(ItemID.TitaniumRepeater);
            DualWieldableWeapons.Add(ItemID.HallowedRepeater);
            DualWieldableWeapons.Add(ItemID.ChlorophyteShotbow);
            DualWieldableWeapons.Add(ItemID.StakeLauncher);

            //PHM guns
            //DualWieldableWeapons.Add(ItemID.RedRyder);
            DualWieldableWeapons.Add(ItemID.FlintlockPistol);
            //DualWieldableWeapons.Add(ItemID.Musket);
            DualWieldableWeapons.Add(ItemID.TheUndertaker);
            DualWieldableWeapons.Add(ItemID.Revolver);
            DualWieldableWeapons.Add(ItemID.Handgun);
            DualWieldableWeapons.Add(ItemID.PhoenixBlaster);

            //HM guns
            DualWieldableWeapons.Add(ItemID.Uzi);
            DualWieldableWeapons.Add(ItemID.VenusMagnum);
            DualWieldableWeapons.Add(ItemID.CandyCornRifle);
            DualWieldableWeapons.Add(ItemID.BorealWood);

            //Other guns
            DualWieldableWeapons.Add(ItemID.SnowballCannon);
            DualWieldableWeapons.Add(ItemID.PainterPaintballGun);
            DualWieldableWeapons.Add(ItemID.StarCannon);
            DualWieldableWeapons.Add(ItemID.Toxikarp);
            DualWieldableWeapons.Add(ItemID.DartPistol);
            DualWieldableWeapons.Add(ItemID.Flamethrower);
            DualWieldableWeapons.Add(ItemID.ElfMelter);

            //PHM magic weapons
            DualWieldableWeapons.Add(ItemID.AmethystStaff);
            DualWieldableWeapons.Add(ItemID.TopazStaff);
            DualWieldableWeapons.Add(ItemID.SapphireStaff);
            DualWieldableWeapons.Add(ItemID.EmeraldStaff);
            DualWieldableWeapons.Add(ItemID.RubyStaff);
            DualWieldableWeapons.Add(ItemID.DiamondStaff);
            DualWieldableWeapons.Add(ItemID.AmberStaff);
            DualWieldableWeapons.Add(ItemID.Vilethorn);
            DualWieldableWeapons.Add(ItemID.MagicMissile);
            DualWieldableWeapons.Add(ItemID.AquaScepter);
            DualWieldableWeapons.Add(ItemID.Flamelash);
            DualWieldableWeapons.Add(ItemID.FlowerofFire);

            //HM magic weapons
            DualWieldableWeapons.Add(ItemID.FlowerofFrost);
            DualWieldableWeapons.Add(ItemID.SkyFracture);
            DualWieldableWeapons.Add(ItemID.CrystalSerpent);
            DualWieldableWeapons.Add(ItemID.CrystalVileShard);
            DualWieldableWeapons.Add(ItemID.MeteorStaff);
            DualWieldableWeapons.Add(ItemID.UnholyTrident);
            DualWieldableWeapons.Add(ItemID.PoisonStaff);
            DualWieldableWeapons.Add(ItemID.FrostStaff);
            DualWieldableWeapons.Add(ItemID.RainbowRod);
            DualWieldableWeapons.Add(ItemID.VenomStaff);
            DualWieldableWeapons.Add(ItemID.NettleBurst);
            DualWieldableWeapons.Add(ItemID.ShadowbeamStaff);
            DualWieldableWeapons.Add(ItemID.InfernoFork);
            DualWieldableWeapons.Add(ItemID.SpectreStaff);
            DualWieldableWeapons.Add(ItemID.StaffofEarth);
            DualWieldableWeapons.Add(ItemID.BatScepter);
            DualWieldableWeapons.Add(ItemID.Razorpine);
            DualWieldableWeapons.Add(ItemID.BlizzardStaff);
            DualWieldableWeapons.Add(3870); //Betsy's Wrath

            //PHM Magic guns
            DualWieldableWeapons.Add(ItemID.SpaceGun);

            //HM Magic guns
            DualWieldableWeapons.Add(ItemID.LaserRifle);
            DualWieldableWeapons.Add(ItemID.LeafBlower);
            DualWieldableWeapons.Add(ItemID.HeatRay);

            //HM other magic weapons
            DualWieldableWeapons.Add(ItemID.MagicDagger);
            DualWieldableWeapons.Add(ItemID.ToxicFlask);
            DualWieldableWeapons.Add(ItemID.NebulaBlaze);

            //PHM thrown weapons
            DualWieldableWeapons.Add(ItemID.Shuriken);
            DualWieldableWeapons.Add(ItemID.ThrowingKnife);
            DualWieldableWeapons.Add(ItemID.PoisonedKnife);
            DualWieldableWeapons.Add(ItemID.Snowball);
            DualWieldableWeapons.Add(ItemID.AleThrowingGlove);
            DualWieldableWeapons.Add(ItemID.Bone);
            DualWieldableWeapons.Add(ItemID.BoneGlove);
            DualWieldableWeapons.Add(ItemID.RottenEgg);
            DualWieldableWeapons.Add(ItemID.StarAnise);
            DualWieldableWeapons.Add(ItemID.FrostDaggerfish);
            DualWieldableWeapons.Add(ItemID.Javelin);
            DualWieldableWeapons.Add(ItemID.BoneJavelin);
            DualWieldableWeapons.Add(ItemID.BoneDagger);
		}

		public static bool IsDualWieldableWeapon(int Type)
		{
			return DualWieldableWeapons.Contains(Type);
		}

		public override void PostSetupContent()
		{
			RequestContainer.InitializeRequests();
			RequestReward.Initialize();
			CompanionSkillContainer.Initialize();
			ModCompatibility.NExperienceModCompatibility.Load();
			ModCompatibility.ThoriumModCompatibility.Load();
			ModCompatibility.CalamityModCompatibility.Load();
			ModCompatibility.TerrariaOverhaulMod.Load();
			ModCompatibility.MrPlagueRacesMod.Load();
			StarlightRiverModInstalled = ModLoader.HasMod("StarlightRiver");
			MrPlagueRacesInstalled = ModLoader.HasMod("MrPlagueRaces");
			nterrautils.Interfaces.LeftScreenInterface.AddInterfaceElement(new GroupMembersInterface());
			GenericCompanionInfos.LoadGenericInfos();
		}

		private void PopulateFemaleNpcsList()
		{
			AddFemaleNpc(
				NPCID.Dryad,
				NPCID.Mechanic,
				NPCID.Nurse,
				NPCID.PartyGirl,
				NPCID.Stylist,
				NPCID.BestiaryGirl,
				NPCID.Princess,
				NPCID.Steampunker
			);
		}

		public static void AddFemaleNpc(params int[] ID)
		{
			FemaleNpcs.AddRange(ID);
		}

		public static CompanionID[] GetPossibleStarterCompanions(string SpecificModID = null)
		{
			List<CompanionID> Companions = new List<CompanionID>();
			foreach(CompanionID id in StarterCompanions)
			{
				if (!DisableModCompanions || id.ModID != GetModName || (SpecificModID != null && id.ModID == SpecificModID))
					Companions.Add(id);
			}
			foreach(PlayerFileData pfd in Main.PlayerList)
			{
				PlayerMod pm = pfd.Player.GetModPlayer<PlayerMod>();
				foreach(uint id in pm.GetCompanionDataKeys)
				{
					CompanionData cd = pm.GetCompanionDataByIndex(id);
					CompanionID cid = cd.GetMyID;
					if (!Companions.Contains(cid) && cd.FriendshipLevel >= cd.Base.GetFriendshipUnlocks.FollowerUnlock && (!DisableModCompanions || cid.ModID != GetModName || (SpecificModID != null && cid.ModID == SpecificModID)) && !cd.Base.IsInvalidCompanion)
					{
						Companions.Add(cid);
					}
				}
			}
			return Companions.ToArray();
		}

		public static void CheckForFreebies(PlayerMod player)
		{
			if (DisableModCompanions) return;
			if(CanGetFreeNemesis() && !player.HasCompanion(CompanionDB.Nemesis))
			{
				player.AddCompanion(CompanionDB.Nemesis, IsStarter: true);
                Main.NewText("You gained a free Nemesis guardian as halloween reward.", MainMod.RecruitColor);
			}
			if (CanGetFreeCotton() && !player.HasCompanion(CompanionDB.Cotton))
			{
				player.AddCompanion(CompanionDB.Cotton, IsStarter: true);
                Main.NewText(player.GetCompanionData(CompanionDB.Cotton).GetNameColored() + " has moved from the tutorial video series to your companions list. Go check him out :).", MainMod.RecruitColor);
			}
			if (CanGetFreeVladimir() && !player.HasCompanion(CompanionDB.Vladimir))
			{
				player.AddCompanion(CompanionDB.Vladimir, IsStarter: true);
				int DaysCounter = (int)(new DateTime(DateTime.Now.Year, 05, 19) - DateTime.Now).TotalDays;
				if (DaysCounter == 0)
				{
					Main.NewText("Today is Terraria's Birthday! You got Vladimir for starting playing today. Enjoy. :3", MainMod.RecruitColor);
				}
				else
				{
					Main.NewText("With Terraria's birthday just " + DaysCounter + " days away, you've got Vladimir to help you celebrate the day.", MainMod.RecruitColor);
				}
			}
		}

		public static bool CanGetFreeCotton()
		{
			DateTime dt = DateTime.Now;
			return dt.Year == 2025 && dt.Month == 7 && dt.Day >= 1 && dt.Day <= 14;
		}

		public static bool CanGetFreeNemesis()
		{
			return Main.halloween;
		}

		public static bool CanGetFreeVladimir()
		{
			DateTime dt = DateTime.Now;
			return dt.Month == 5 && dt.Day >= 4 && dt.Day <= 19;
		}

		public static CompanionCommonData GetCommonData(uint CompanionID, string CompanionModID = "")
		{
			if(CompanionModID == "") CompanionModID = GetModName;
			foreach(CompanionID id in CommonDatas.Keys)
			{
				if(id.IsSameID(CompanionID, CompanionModID))
				{
					return CommonDatas[id];
				}
			}
			CompanionID NewID = new CompanionID(CompanionID, CompanionModID);
			CompanionCommonData d = new CompanionCommonData();
			CommonDatas.Add(NewID, d);
			return d;
		}

		public static void DrawFriendshipHeart(Vector2 Position, int Level, float Percentage)
		{
			DrawFriendshipHeart(Position, Level, Percentage, 1f);
		}

		public static void DrawFriendshipHeart(Vector2 Position, int Level, float Percentage, float Opacity)
		{
			Texture2D HeartTexture = GuardianFriendshipHeartTexture.Value;
			Vector2 HeartCenter = Position;
			Position -= Vector2.One * 12;
			Color color = Color.White * Opacity;
			Main.spriteBatch.Draw(HeartTexture, Position, new Rectangle(0, 0, 24, 24), color);
			int Height = (int)(20 * Percentage);
			Position.X += 2;
			Position.Y += (2 + 20 - Height);
			Main.spriteBatch.Draw(HeartTexture, Position, new Rectangle(26, 2 + (20 - Height), 20, Height), color);
			Utils.DrawBorderString(Main.spriteBatch, Level.ToString(), HeartCenter, color, 0.7f, .5f, 0.4f);
		}

		public static void SetGenderColoring(Genders gender, ref string Text)
		{
			switch(gender)
            {
                case Genders.Male:
                    Text = "[c/80A6FF:" + Text + "]"; //4079FF
					break;
                case Genders.Female:
                    Text = "[c/FF80A6:" + Text + "]"; //FF4079
					break;
				case Genders.Genderless:
					Text = "[c/CCCCCC:" + Text + "]";
					break;
            }
		}

		public override object Call(params object[] args)
		{
			if (args[0] is string)
			{
				switch((string)args[0])
				{
					case "IsPC":
						if (args[1] is Player p)
							return p == GetLocalPlayer;
						return false;
					case "GetPC":
						return GetLocalPlayer;
					case "IsCompanionDelegate":
						return delegate(Player player) { return player is Companion; };
					case "IsCompanion":
						if (args[1] is Player)
							return !PlayerMod.IsPlayerCharacter(args[1] as Player);
						break;
					case "IsTerraguardian":
						if (args[1] is Player)
							return args[1] is TerraGuardian;
						break;
					case "AddGroupInterfaceHook":
						if (args[1] is Func<Player, Vector2, float>)
						{
							GroupInterfaceBarsHooks.Add((Func<Player, Vector2, float>)args[1]);
						}
						break;
				}
			}
			return base.Call(args);
		}

		public static bool AddCompanionDB(CompanionContainer container, Mod mod)
		{
			if(mod == null || container == null || ModCompanionContainer.ContainsKey(mod.Name))
				return false;
			ModCompanionContainer.Add(mod.Name, container);
			container.SetReferedMod(mod);
			return true;
		}

		public static CompanionBase GetCompanionBase(CompanionData Data)
		{
			return GetCompanionBase(Data.ID, Data.ModID);
		}

		public static CompanionBase GetCompanionBase(CompanionID ID)
		{
			return GetCompanionBase(ID.ID, ID.ModID);
		}

		public static CompanionBase GetCompanionBase(uint ID, string ModID = "")
		{
			if(ModID == "") ModID = GetModName;
			if(ModCompanionContainer.ContainsKey(ModID))
			{
				return ModCompanionContainer[ModID].ReturnCompanionBase(ID);
			}
			return CompanionContainer.InvalidCompanionBase;
		}

		public static bool AddCompanionHookContainer(CompanionHookContainer container, Mod mod)
		{
			if (container == null) return false;
			container.SetOwningMod(mod);
			string mid = container.GetModName;
			if (ModCompanionHooks.ContainsKey(mid)) return false;
			ModCompanionHooks.Add(mid, container);
			return true;
		}

		public static Companion SpawnCompanion(uint ID, string ModID = "")
		{
			return SpawnCompanion(ID, ModID, 0);
		}

		internal static Companion SpawnCompanion(uint ID, string ModID = "", ushort GenericID = 0, bool Starter = false)
		{
			return SpawnCompanion(Vector2.Zero, ID, ModID, GenericID: GenericID, Starter: Starter);
		}

		public static Companion SpawnCompanion(Vector2 Position, CompanionData data, Player Owner = null)
		{
			if (data.Base.IsInvalidCompanion) return null;
			Companion companion = GetCompanionBase(data).GetCompanionObject;
			companion.Data = data;
			ActiveCompanions.Add(companion.GetWhoAmID, companion);
			companion.InitializeCompanion(true);
			companion.Spawn(PlayerSpawnContext.SpawningIntoWorld);
			if(Owner != null) companion.Owner = Owner;
			if(Position.Length() > 0)
			{
				companion.Teleport(Position);
			}
			return companion;
		}

		public static Companion SpawnCompanion(Vector2 Position, uint ID, string ModID = "", Player Owner = null)
		{
			return SpawnCompanion(Position, ID, ModID, Owner, 0);
		}

		internal static Companion SpawnCompanion(Vector2 Position, uint ID, string ModID = "", Player Owner = null, ushort GenericID = 0, bool Starter = false)
		{
			if (GetCompanionBase(ID, ModID).IsInvalidCompanion) return null;
			CompanionData data = null;
			if(Main.netMode == 0)
			{
				PlayerMod pm = GetLocalPlayer.GetModPlayer<PlayerMod>();
				if (pm.HasCompanion(ID, GenericID, ModID))
				{
					data = pm.GetCompanionData(ID, GenericID, ModID);
				}
			}
			if (data == null)
			{
				data = GetCompanionBase(ID, ModID).CreateCompanionData;
				data.IsStarter = Starter;
				data.ChangeCompanion(ID, ModID);
				if (data.IsGeneric)
				{
					if (GenericID > 0)
					{
						data.AssignGenericID(GenericID);
					}
					else
					{
						data.AssignGenericID();
						GenericCompanionRandomizer.RandomizeCompanion(data);
					}	
				}
				data.Index = 0;
			}
			return SpawnCompanion(Position, data, Owner);
		}

		public static void DespawnCompanion(uint WhoAmID)
		{
			if(ActiveCompanions.ContainsKey(WhoAmID))
			{
				Companion companion = ActiveCompanions[WhoAmID];
				companion.active = false;
				companion.GetGoverningBehavior().Deactivate();
				if (WorldMod.CompanionNPCs.Contains(companion))
					WorldMod.CompanionNPCs.Remove(companion);
			}
		}

		public static bool HasCompanionInWorld(CompanionID ID)
		{
			return HasCompanionInWorld(ID.ID, ID.ModID);
		}

		public static bool HasCompanionInWorld(uint ID, string ModID = "")
		{
			return HasCompanionInWorld(ID, 0, ModID);
		}

		public static bool HasCompanionInWorld(uint ID, ushort GenericID, string ModID = "")
		{
			if (ModID == "") ModID = GetModName;
			foreach(Companion c in ActiveCompanions.Values)
			{
				if (c.IsSameID(ID, ModID) && (GenericID == 0 || GenericID == c.GenericID)) return true;
			}
			return false;
		}

		public static string PluralizeString(string Text, int Count)
		{
			return nterrautils.InterfaceHelper.PluralizeString(Text, Count);
		}
		
		public static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            int HalfHeight = (int)(Height * 0.5f);
            Texture2D ChatBackground = TextureAssets.ChatBack.Value;
            for(byte y = 0; y < 3; y++)
            {
                for(byte x = 0; x < 3; x++)
                {
                    const int DrawDimension = 30;
                    int px = (int)Position.X, py = (int)Position.Y, pw = DrawDimension, ph = DrawDimension, 
                        dx = 0, dy = 0, dh = DrawDimension;
                    if (x == 2)
                    {
                        px += Width - pw;
                        dx = ChatBackground.Width - DrawDimension;
                    }
                    else if (x == 1)
                    {
                        px += pw;
                        pw = Width - pw * 2;
                        dx = DrawDimension;
                    }
                    if (y == 2)
                    {
                        py += Height - ph;
                        dy = ChatBackground.Height - DrawDimension;
                        if (ph > HalfHeight)
                        {
                            dy += DrawDimension - HalfHeight;
                            py += (int)(DrawDimension - HalfHeight);
                            ph = dh = HalfHeight;
                        }
                    }
                    else if (y == 1)
                    {
                        py += ph;
                        ph = Height - ph * 2;
                        dy = DrawDimension;
                    }
                    else
                    {
                        if (ph > HalfHeight)
                        {
                            ph = dh = HalfHeight;
                        }
                    }
                    if (pw > 0 && ph > 0)
                    {
                        Main.spriteBatch.Draw(ChatBackground, new Rectangle(px, py, pw, ph), new Rectangle(dx, dy, DrawDimension, dh), color);
                    }
                }
            }
        }

		public static string NameGenerator(string[] Syllabes, bool AllowRepeated = false)
		{
			string NewName = "";
			double Chance = 2f;
			bool First = true;
			List<int> UsedSyllabes = new List<int>();
			byte MaxSyllabes = 6;
			while(Main.rand.NextDouble() < Chance)
			{
				int Selected = Main.rand.Next(Syllabes.Length);
				int SyllabesDisponible = 0;
				for (int s = 0; s < Syllabes.Length; s++)
				{
					if (!UsedSyllabes.Contains(s))
					{
						SyllabesDisponible++;
					}
				}
				if (SyllabesDisponible == 0)
					break;
				if (UsedSyllabes.Contains(Selected)) continue;
				if (!AllowRepeated) UsedSyllabes.Add(Selected);
				foreach(char Letter in Syllabes[Selected])
				{
					NewName += Letter;
					if (First)
					{
						NewName = NewName.ToUpper();
						First = false;
					}
				}
				if (Chance > 1f)
					Chance--;
				else if(Chance > 0.5f)
					Chance -= 0.2f;
				else
					Chance *= 0.5f;
				MaxSyllabes--;
				if (MaxSyllabes == 0) break;
			}
			return NewName;
		}

		public static string GetDirectionText(Vector2 Direction)
        {
			return nterrautils.InterfaceHelper.GetDirectionText(Direction);
        }

		internal static void Update2PControls(Companion companion)
		{
			oldSecondPlayerControlState = SecondPlayerControlState;
			SecondPlayerControlState = GamePad.GetState(SecondPlayerPort);
			if (Gameplay2PMode && !SecondPlayerControlState.IsConnected)
			{
				Gameplay2PMode = false;
				Gameplay2PInventory = false;
				Main.NewText("Controller disconnected: 2P mode deactivated.", Color.Red);
				return;
			}
			if (Is2PButtonPressed(Buttons.Start))
			{
				Gameplay2PMode = !Gameplay2PMode;
				if(Gameplay2PMode && companion == null)
				{
					Gameplay2PMode = false;
					Gameplay2PInventory = false;
					Main.NewText("You must have a Companion following you to start 2P mode.", Color.Red);
				}
				else
				{
					Main.NewText("2P gameplay is now " + (Gameplay2PMode ? "ON" : "OFF") + ".", (Gameplay2PMode ? Color.Green : Color.Red));
					if (Gameplay2PMode)
					{
						SoundEngine.PlaySound(SoundID.Coins);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.MoonLord);
					}
					if (!Gameplay2PMode) 
						Gameplay2PInventory = false;
				}
				return;
			}
			Vector2 Thumbstick = SecondPlayerControlState.ThumbSticks.Left;
			MoveUp2P = Thumbstick.Y > 0.2f;
			MoveDown2P = Thumbstick.Y < -0.2f;
			MoveLeft2P = Thumbstick.X < -0.2f;
			MoveRight2P = Thumbstick.X > 0.2f;
			Confirm2P = Is2PButtonPressed(Buttons.RightTrigger, true);
			if (Gameplay2PMode)
			{
				if (Is2PButtonPressed(Buttons.Back))
				{
					Gameplay2PInventory = !Gameplay2PInventory;
				}
				if (!Gameplay2PInventory)
				{
					companion.MoveUp = MoveUp2P;
					companion.MoveDown = MoveDown2P;
					companion.MoveLeft = MoveLeft2P;
					companion.MoveRight = MoveRight2P;
					companion.ControlAction = Confirm2P;
					if (Is2PButtonPressed(Buttons.B))
						companion.WalkMode = !companion.WalkMode;
					int SlotChange = companion.selectedItem;
					if (Is2PButtonPressed(Buttons.LeftShoulder)) SlotChange--;
					if (Is2PButtonPressed(Buttons.RightShoulder)) SlotChange++;
					if (SlotChange < 0) SlotChange += 10;
					if (SlotChange >= 10) SlotChange -= 10;
					companion.selectedItem = SlotChange;
					companion.ControlJump = Is2PButtonPressed(Buttons.LeftTrigger, true);
					if (Is2PButtonPressed(Buttons.Y))
					{
						companion.ChangeSelectedSubAttackSlot(true);
					}
					if (Is2PButtonPressed(Buttons.A))
					{
						companion.UseSubAttack();
					}
					Vector2 RightThumbstick = SecondPlayerControlState.ThumbSticks.Right * 128f;
					if (RightThumbstick.X == 0 && RightThumbstick.Y == 0)
					{
						RightThumbstick.X = companion.direction * companion.SpriteWidth * 0.5f;
					}
					companion.AimDirection.X = RightThumbstick.X;
					companion.AimDirection.Y = -RightThumbstick.Y;
				}
			}
			Companion2PInventoryInterface.UpdateInterface();
		}

		public static bool Is2PButtonPressed(Buttons button, bool Hold = false)
		{
			return SecondPlayerControlState.IsButtonDown(button) && (Hold || oldSecondPlayerControlState.IsButtonUp(button));
		}

		public static string[] WordwrapText(string Text, DynamicSpriteFont font, float MaxWidth = 100, float Scale = 1f)
		{
			return nterrautils.InterfaceHelper.WordwrapText(Text, font, MaxWidth, Scale);
		}

		public static string GlyphfyItem(Item item)
		{
			return nterrautils.InterfaceHelper.GlyphfyItem(item);
		}

		public static string GlyphfyItem(int ID, int Stack = 1, int Prefix = 0)
		{
			return nterrautils.InterfaceHelper.GlyphfyItem(ID, Stack, Prefix);
		}

		public static string GlyphfyCoins(int value)
		{
			return nterrautils.InterfaceHelper.GlyphfyCoins(value);
		}

		public static string GlyphfyCoins(int c, int s = 0, int g = 0, int p = 0)
		{
			return nterrautils.InterfaceHelper.GlyphfyCoins(c, s, g, p);
		}

		public enum CompanionMaxDistanceFromPlayer : byte
		{
			Nearer = 0,
			Normal = 1,
			Far = 2,
			Farther = 3
		}
	}
}