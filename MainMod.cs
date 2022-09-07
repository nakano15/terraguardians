using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace terraguardians
{
	public class MainMod : Mod
	{
		internal static Mod mod;
		internal static Mod GetMod { get { return mod; } }
		internal static string GetModName { get { return mod.Name; } }
		private static Dictionary<string, CompanionContainer> ModCompanionContainer = new Dictionary<string, CompanionContainer>();
		
        public override void Load()
        {
			mod = this;
		}
		
        public override void Unload()
        {
			foreach(string Mod in ModCompanionContainer.Keys) ModCompanionContainer[Mod].Unload();
			ModCompanionContainer.Clear();
		}

		public static CompanionBase GetCompanionBase(uint ID, string ModID = "")
		{
			if(ModID != "") ModID = GetModName;
			if(ModCompanionContainer.ContainsKey(ModID))
				return ModCompanionContainer[ModID].ReturnCompanionBase(ID);
			return new CompanionBase();
		}
	}
}