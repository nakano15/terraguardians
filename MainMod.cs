using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace terraguardians
{
	public class MainMod : Mod
	{
		internal static Mod mod;
		internal static Mod GetMod { get { return mod; } }
		internal static string GetModName { get { return mod.Name; } }
		private static Dictionary<string, CompanionContainer> ModCompanionContainer = new Dictionary<string, CompanionContainer>();
		public static Asset<Texture2D> ErrorTexture;

        public override void Load()
        {
			mod = this;
			AddCompanionDB(new Containers.CompanionDB(), this);
			if(Main.netMode < 2)
			{
				ErrorTexture = ModContent.Request<Texture2D>("terraguardians/ErrorTexture");
			}
		}
		
        public override void Unload()
        {
			CompanionContainer.UnloadStatic();
			foreach(string Mod in ModCompanionContainer.Keys) ModCompanionContainer[Mod].Unload();
			ModCompanionContainer.Clear();
		}

		public static bool AddCompanionDB(CompanionContainer container, Mod mod)
		{
			if(mod == null || container == null || ModCompanionContainer.ContainsKey(mod.Name))
				return false;
			ModCompanionContainer.Add(mod.Name, container);
			container.SetReferedMod(mod);
			return true;
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
	}
}