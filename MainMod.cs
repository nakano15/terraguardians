using Terraria;
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
		public const uint CompanionSaveVersion = 1;
		public const int MaxCompanionFollowers = 2;
		internal static Mod mod;
		internal static Mod GetMod { get { return mod; } }
		internal static string GetModName { get { return mod.Name; } }
		private static Dictionary<string, CompanionContainer> ModCompanionContainer = new Dictionary<string, CompanionContainer>();
		public static Asset<Texture2D> ErrorTexture;
		public static Asset<Texture2D> GuardianHealthBarTexture;
		public static Asset<Texture2D> GuardianInventoryInterfaceButtonsTexture;
		internal static Dictionary<uint, Companion> ActiveCompanions = new Dictionary<uint, Companion>();
		public static Companion[] GetActiveCompanions { get{ return ActiveCompanions.Values.ToArray();} }
		private static Dictionary<CompanionID, CompanionCommonData> CommonDatas = new Dictionary<CompanionID, CompanionCommonData>();
        public override void Load()
        {
			mod = this;
			AddCompanionDB(new Containers.CompanionDB(), this);
			if(Main.netMode < 2)
			{
				ErrorTexture = ModContent.Request<Texture2D>("terraguardians/Content/ErrorTexture");
				GuardianHealthBarTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianHealthBar");
				GuardianInventoryInterfaceButtonsTexture = ModContent.Request<Texture2D>("terraguardians/Content/Interface/GuardianEquipButtons");
			}
		}
		
        public override void Unload()
        {
			CompanionContainer.UnloadStatic();
			foreach(string Mod in ModCompanionContainer.Keys) ModCompanionContainer[Mod].Unload();
			ModCompanionContainer.Clear();
			UnloadInterfaces();
			CommonDatas.Clear();
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

		private void UnloadInterfaces()
		{
			GroupMembersInterface.Unload();
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
				Position.X -= companion.width * 0.5f;
				Position.Y -= companion.height;
				companion.Teleport(Position, 2);
			}
			return companion;
		}

		public static Companion SpawnCompanion(Vector2 Position, uint ID, string ModID = "", Entity Owner = null)
		{
			CompanionData data = new CompanionData();
			data.ChangeCompanion(ID, ModID);
			return SpawnCompanion(Position, data, Owner);
		}

		public static void DespawnCompanion(uint WhoAmID)
		{
			if(ActiveCompanions.ContainsKey(WhoAmID))
			{
				ActiveCompanions.Remove(WhoAmID);
			}
		}
	}
}