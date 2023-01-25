using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace terraguardians
{
	public class MainMod : Mod
	{
		public const uint ModVersion = 8;
		public const int MaxCompanionFollowers = 2;
		public static int MyPlayerBackup = 0;
		public static Player GetLocalPlayer { get { return Main.player[MyPlayerBackup]; } }
		internal static Mod mod;
		internal static Mod GetMod { get { return mod; } }
		internal static string GetModName { get { return mod.Name; } }
		private static Dictionary<string, CompanionContainer> ModCompanionContainer = new Dictionary<string, CompanionContainer>();
		public static Asset<Texture2D> IronSwordTexture;
		public static Asset<Texture2D> ErrorTexture;
		public static Asset<Texture2D> GuardianHealthBarTexture, GuardianInventoryInterfaceButtonsTexture, GuardianFriendshipHeartTexture;
		public static Asset<Texture2D> TrappedCatTexture;
		public static Asset<Texture2D> NinjaTextureBackup;
		internal static Dictionary<uint, Companion> ActiveCompanions = new Dictionary<uint, Companion>();
		public static Companion[] GetActiveCompanions { get{ return ActiveCompanions.Values.ToArray();} }
		private static Dictionary<CompanionID, CompanionCommonData> CommonDatas = new Dictionary<CompanionID, CompanionCommonData>();
		private static List<CompanionID> StarterCompanions = new List<CompanionID>();
        public static List<CompanionID> GetStarterCompanions { get { return StarterCompanions; }}
		private static TerrariansGroup _terrariangroup = new TerrariansGroup();
		private static TerraGuardiansGroup _tggroup = new TerraGuardiansGroup();
		private static CaitSithGroup _csgroup = new CaitSithGroup();
		public static TerrariansGroup GetTerrariansGroup { get { return _terrariangroup; } }
		public static TerraGuardiansGroup GetTerraGuardiansGroup { get { return _tggroup; } }
		public static CaitSithGroup GetCaitSithGroup { get { return _csgroup; } }
		private static List<int> FemaleNpcs = new List<int>();

		public static bool IsNpcFemale(int ID)
		{
			return FemaleNpcs.Contains(ID);
		}

		public override void Load()
        {
			mod = this;
			AddCompanionDB(new CompanionDB(), this);
			if(Main.netMode < 2)
			{
				ErrorTexture = ModContent.Request<Texture2D>("terraguardians/Content/ErrorTexture");
				GuardianHealthBarTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianHealthBar");
				GuardianFriendshipHeartTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/FriendshipHeart");
				GuardianInventoryInterfaceButtonsTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianEquipButtons");
				TrappedCatTexture = ModContent.Request<Texture2D>("terraguardians/Content/Extra/TrappedCat");
				IronSwordTexture = ModContent.Request<Texture2D>("terraguardians/Items/Weapons/TwoHandedSword");
				NinjaTextureBackup = TextureAssets.Ninja;
				Main.PlayerRenderer = new TerraGuardiansPlayerRenderer();
			}
			StarterCompanions.Add(new CompanionID(CompanionDB.Rococo));
			StarterCompanions.Add(new CompanionID(CompanionDB.Blue));
			PopulateFemaleNpcsList();
		}

		public override void PostSetupContent()
		{
			RequestContainer.InitializeRequests();
			RequestReward.Initialize();
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
			FemaleNpcs.Clear();
			FemaleNpcs = null;
			Main.PlayerRenderer = new Terraria.Graphics.Renderers.LegacyPlayerRenderer();
			RequestContainer.Unload();
			RequestReward.Unload();
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
					case "IsCompanion":
						if (args[1] is Player)
							return !PlayerMod.IsPlayerCharacter(args[1] as Player);
						break;
					case "IsTerraguardian":
						if (args[1] is Player)
							return args[1] is TerraGuardian;
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

		public static Companion SpawnCompanion(uint ID, string ModID = "")
		{
			return SpawnCompanion(Vector2.Zero, ID, ModID);
		}

		public static Companion SpawnCompanion(Vector2 Position, CompanionData data, Entity Owner = null)
		{
			Companion companion = GetCompanionBase(data).GetCompanionObject;
			companion.Data = data;
			ActiveCompanions.Add(companion.GetWhoAmID, companion);
			companion.InitializeCompanion();
			companion.Spawn(PlayerSpawnContext.SpawningIntoWorld);
			if(Owner != null) companion.Owner = Owner;
			if(Position.Length() > 0)
			{
				companion.Teleport(Position);
			}
			return companion;
		}

		public static Companion SpawnCompanion(Vector2 Position, uint ID, string ModID = "", Entity Owner = null)
		{
			CompanionData data = new CompanionData();
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
	}
}