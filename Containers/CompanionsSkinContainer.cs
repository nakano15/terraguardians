using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSkinContainer
    {
        ///Upon changing the companion skin or outfit, the skin/outfit info should be stored somewhere in the companion object,
        ///so it doesn't relies on keeping on picking the skin/outfit object from the companion base.
        ///If a skin/outfit is null, of course it should be ignored.
        internal Dictionary<byte, CompanionSkinInfo> SkinsContainer = new Dictionary<byte, CompanionSkinInfo>(),
            OutfitsContainer = new Dictionary<byte, CompanionSkinInfo>();

        public void Unload()
        {
            foreach(byte b in SkinsContainer.Keys)
            {
                SkinsContainer[b].Unload();
                SkinsContainer[b] = null;
            }
            SkinsContainer.Clear();
            SkinsContainer = null;
            foreach(byte b in OutfitsContainer.Keys)
            {
                OutfitsContainer[b].Unload();
                OutfitsContainer[b] = null;
            }
            OutfitsContainer.Clear();
            OutfitsContainer = null;
        }
        
        protected bool AddSkin(byte ID, CompanionSkinInfo info)
        {
            if (SkinsContainer.ContainsKey(ID)) return false;
            SkinsContainer.Add(ID, info);
            info.Load();
            return true;
        }
        
        protected bool AddOutfit(byte ID, CompanionSkinInfo info)
        {
            if (OutfitsContainer.ContainsKey(ID)) return false;
            OutfitsContainer.Add(ID, info);
            return true;
        }

        public CompanionSkinInfo GetSkin(byte ID)
        {
            if (SkinsContainer.ContainsKey(ID))
                return SkinsContainer[ID];
            return null;
        }

        public CompanionSkinInfo GetOutfit(byte ID)
        {
            if (OutfitsContainer.ContainsKey(ID))
                return OutfitsContainer[ID];
            return null;
        }

        public void Load()
        {
            OnLoad();
        }

        protected virtual void OnLoad()
        {

        }
    }
}