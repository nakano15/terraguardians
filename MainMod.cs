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

namespace terraguardians
{
	public class MainMod : Mod
	{
		public const uint ModVersion = 32;
		public static int MaxCompanionFollowers { get { return _MaxCompanionFollowers; } set { if (Main.gameMenu) _MaxCompanionFollowers = (int)MathF.Min(value, 50); } }
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
		public static Asset<Texture2D> GuardianHealthBarTexture, GuardianInventoryInterfaceButtonsTexture, GuardianFriendshipHeartTexture, ReviveBarsEffectTexture, ReviveHealthBarTexture;
		public static Asset<Texture2D> TrappedCatTexture;
		public static Asset<Texture2D> RenamePencilTexture;
		public static Asset<Texture2D> TGMouseTexture;
		public static Asset<Texture2D> NinjaTextureBackup;
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
		internal static bool DebugMode = false, SkillsEnabled = true;
		internal static bool Gameplay2PMode = false, Show2PNotification = true;
		internal static bool DisableModCompanions = false, EnableProfanity = true;
		internal static bool PlayerKnockoutEnable = false, PlayerKnockoutColdEnable = false, 
			CompanionKnockoutEnable = true, CompanionKnockoutColdEnable = false;
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
		internal static bool StarlightRiverModInstalled = false;

		public static bool IsNpcFemale(int ID)
		{
			return FemaleNpcs.Contains(ID);
		}

		public override void Load()
        {
			mod = this;
			CompanionCommonData.OnLoad();
			AddCompanionDB(new CompanionDB(), this);
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
				RenamePencilTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/EditButton");
				ReviveBarsEffectTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/KnockoutEffect");
				ReviveHealthBarTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/ReviveHealthBar");
				TrappedCatTexture = ModContent.Request<Texture2D>("terraguardians/Content/Extra/TrappedCat");
				IronSwordTexture = ModContent.Request<Texture2D>("terraguardians/Items/Weapons/TwoHandedSword");
				NinjaTextureBackup = TextureAssets.Ninja;
				Main.PlayerRenderer = new TerraGuardiansPlayerRenderer();
				UseSubAttackKey = KeybindLoader.RegisterKeybind(this, "UseSubAttack", "G");
				ScrollPreviousSubAttackKey = KeybindLoader.RegisterKeybind(this, "ScrollPreviousSubAttack", "Q");
				ScrollNextSubAttackKey = KeybindLoader.RegisterKeybind(this, "ScrollNextSubAttack", "E");
				OpenOrderWindowKey = KeybindLoader.RegisterKeybind(this, "OpenOrderWindow", "'");
			}
			PersonalityDB.Load();
			SardineBountyBoard.OnModLoad();
			StarterCompanions.Add(new CompanionID(CompanionDB.Rococo));
			StarterCompanions.Add(new CompanionID(CompanionDB.Blue));
			SetupDualwieldable();
			PopulateFemaleNpcsList();
			SetupHatableEquipments();
		}
		
        public override void Unload()
        {
			CompanionContainer.UnloadStatic();
			foreach(string Mod in ModCompanionContainer.Keys) ModCompanionContainer[Mod].Unload();
			ModCompanionContainer.Clear();
			UnloadInterfaces();
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
			{
				ErrorTexture = null;
				PathGuideTexture = null;
				LosangleOfUnknown = null;
				GuardianHealthBarTexture = null;
				GuardianFriendshipHeartTexture = null;
				GuardianInventoryInterfaceButtonsTexture = null;
				TrappedCatTexture = null;
				IronSwordTexture = null;
				NinjaTextureBackup = null;
				ReviveBarsEffectTexture = null;
				ReviveHealthBarTexture = null;
			}
			CompanionSelectionInterface.Unload();
			PersonalityDB.Unload();
			CompanionHeadsMapLayer.OnUnload();
			BuddyModeSetupInterface.Unload();
			GroupInterfaceBarsHooks.Clear();
			GroupInterfaceBarsHooks = null;
			SardineBountyBoard.Unload();
			ModCompatibility.NExperienceModCompatibility.Unload();
			BehaviorBase.Unload();
			Interfaces.CompanionOrderInterface.OnUnload();
			CompanionInventoryInterface.Unload();
			Companions.VladimirBase.CarryBlacklist.Clear();
			Companions.VladimirBase.CarryBlacklist = null;
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
			StarlightRiverModInstalled = ModLoader.HasMod("StarlightRiver");
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
					if (!Companions.Contains(cid) && cd.FriendshipLevel >= cd.Base.GetFriendshipUnlocks.FollowerUnlock && (!DisableModCompanions || cid.ModID != GetModName || (SpecificModID != null && cid.ModID == SpecificModID)))
					{
						Companions.Add(cid);
					}
				}
			}
			return Companions.ToArray();
		}

		public static void CheckForFreebies(PlayerMod player)
		{
			if(CanGetFreeNemesis() && !player.HasCompanion(CompanionDB.Nemesis))
			{
				player.AddCompanion(CompanionDB.Nemesis, IsStarter: true);
                Main.NewText("You gained a free Nemesis guardian as halloween reward.", MainMod.RecruitColor);
			}
		}

		public static bool CanGetFreeNemesis()
		{
			return Main.halloween;
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
			Texture2D HeartTexture = GuardianFriendshipHeartTexture.Value;
			Vector2 HeartCenter = Position;
			Position -= Vector2.One * 12;
			Main.spriteBatch.Draw(HeartTexture, Position, new Rectangle(0, 0, 24, 24), Color.White);
			int Height = (int)(20 * Percentage);
			Position.X += 2;
			Position.Y += (2 + 20 - Height);
			Main.spriteBatch.Draw(HeartTexture, Position, new Rectangle(26, 2 + (20 - Height), 20, Height), Color.White);
			Utils.DrawBorderString(Main.spriteBatch, Level.ToString(), HeartCenter, Color.White, 0.7f, .5f, 0.4f);
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

		private void UnloadInterfaces()
		{
			GroupMembersInterface.Unload();
			CompanionCommonData.OnUnload();
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
			return SpawnCompanion(Vector2.Zero, ID, ModID);
		}

		public static Companion SpawnCompanion(Vector2 Position, CompanionData data, Player Owner = null)
		{
			if (data.Base.IsInvalidCompanion) return null;
			Companion companion = GetCompanionBase(data).GetCompanionObject;
			companion.Data = data;
			/*if (data.FileData != null)
			{
				Terraria.IO.PlayerFileData pd = Player.LoadPlayer(data.FileData.Path, false);

			}*/
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
			if (GetCompanionBase(ID, ModID).IsInvalidCompanion) return null;
			CompanionData data = GetCompanionBase(ID, ModID).CreateCompanionData;
            data.ChangeCompanion(ID, ModID);
            data.Index = 0;
			bool GotCompanionInfo = false;
			if(Main.netMode == 0)
			{
				PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
				if (pm.HasCompanion(ID, ModID))
				{
					GotCompanionInfo = true;
					data = pm.GetCompanionData(ID, ModID);
				}
			}
			if (!GotCompanionInfo) data.ChangeCompanion(ID, ModID);
			return SpawnCompanion(Position, data, Owner);
		}

		public static void DespawnCompanion(uint WhoAmID)
		{
			if(ActiveCompanions.ContainsKey(WhoAmID))
			{
				ActiveCompanions[WhoAmID].active = false;
				ActiveCompanions.Remove(WhoAmID);
			}
		}

		public static bool HasCompanionInWorld(CompanionID ID)
		{
			return HasCompanionInWorld(ID.ID, ID.ModID);
		}

		public static bool HasCompanionInWorld(uint ID, string ModID = "")
		{
			if (ModID == "") ModID = GetModName;
			foreach(Companion c in ActiveCompanions.Values)
			{
				if (c.IsSameID(ID, ModID)) return true;
			}
			return false;
		}

		public static string PluralizeString(string Text, int Count)
		{
			if (System.Math.Abs(Count) <= 1 || Text.EndsWith('s')) return Text;
			if(Text.EndsWith("fe"))
				return Text.Substring(0, Text.Length - 2) + "ves";
			if(Text.EndsWith("o"))
				return Text + "es";
			return Text + 's';
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
            Direction.Normalize();
            bool CountVerticalDiference = Math.Abs(Direction.Y) >= 0.33f, CountHorizontalDiference = Math.Abs(Direction.X) >= 0.33f;
            string DirectionText = "";
            if (CountVerticalDiference && CountHorizontalDiference)
            {
                if (Direction.Y > 0) DirectionText += "South";
                else DirectionText += "North";
                if (Direction.X > 0) DirectionText += "east";
                else DirectionText += "west";
            }
            else if (CountVerticalDiference)
            {
                if (Direction.Y > 0) DirectionText = "South";
                else DirectionText = "North";
            }
            else if (CountHorizontalDiference)
            {
                if (Direction.X > 0) DirectionText = "East";
                else DirectionText = "West";
            }
            return DirectionText;
        }

		internal static void Update2PControls(Companion companion)
		{
			SecondPlayerControlState = GamePad.GetState(SecondPlayerPort);
			if (Gameplay2PMode && !SecondPlayerControlState.IsConnected)
			{
				Gameplay2PMode = false;
				Main.NewText("Controller disconnected: 2P mode deactivated.", Color.Red);
				oldSecondPlayerControlState = SecondPlayerControlState;
				return;
			}
			if (Is2PButtonPressed(Buttons.Start))
			{
				Gameplay2PMode = !Gameplay2PMode;
				if(Gameplay2PMode && companion == null)
				{
					Gameplay2PMode = false;
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
				}
				oldSecondPlayerControlState = SecondPlayerControlState;
				return;
			}
			if (Gameplay2PMode)
			{
				Vector2 Thumbstick = SecondPlayerControlState.ThumbSticks.Left;
				companion.MoveUp = Thumbstick.Y > 0.2f;
				companion.MoveDown = Thumbstick.Y < -0.2f;
				companion.MoveLeft = Thumbstick.X < -0.2f;
				companion.MoveRight = Thumbstick.X > 0.2f;
				companion.ControlAction = Is2PButtonPressed(Buttons.RightTrigger, true);
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
			oldSecondPlayerControlState = SecondPlayerControlState;
		}

		public static bool Is2PButtonPressed(Buttons button, bool Hold = false)
		{
			return SecondPlayerControlState.IsButtonDown(button) && (Hold || oldSecondPlayerControlState.IsButtonUp(button));
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