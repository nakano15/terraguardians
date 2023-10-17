using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSkinContainer
    {
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

        public virtual void OnLoad()
        {
            
        }
    }
}