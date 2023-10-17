using Terraria;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSkinInfo
    {
        public virtual string Name => "Unknown";
        public virtual string Description => "";
        Dictionary<string, CompanionSpritesContainer.ExtraTexture> Textures = new Dictionary<string, CompanionSpritesContainer.ExtraTexture>();

        public void Load() //Need to work on how loading works.
        {
            OnLoad();
        }

        public void Unload()
        {
            OnUnload();
            foreach(string s in Textures.Keys)
            {
                Textures[s].Unload();
                Textures[s] = null;
            }
            Textures.Clear();
            Textures = null;
        }
        
        protected virtual void OnLoad()
        {

        }

        protected virtual void OnUnload()
        {

        }

        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }
    }
}