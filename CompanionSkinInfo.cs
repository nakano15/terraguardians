using Terraria;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
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

        public void AddTexture(string name, string path)
        {
            if(!Textures.ContainsKey(name))
            {
                Textures.Add(name, new CompanionSpritesContainer.ExtraTexture(path));
            }
        }

        public Texture2D GetTexture(string name)
        {
            if (!Textures.ContainsKey(name)) return MainMod.ErrorTexture.Value;
            if(Textures[name].loadstate == CompanionSpritesContainer.SpritesLoadState.NotLoaded)
            {
                bool success;
                Textures[name].SetTexture(TryLoading(Textures[name].path, out success));
                Textures[name].SetLoadState(success ? CompanionSpritesContainer.SpritesLoadState.Loaded : CompanionSpritesContainer.SpritesLoadState.Error);
            }
            return Textures[name].texture;
        }

        private Texture2D TryLoading(string Path, out bool Success)
        {
            ReLogic.Content.Asset<Texture2D> texture;
            if(ModContent.RequestIfExists<Texture2D>(Path, out texture, ReLogic.Content.AssetRequestMode.ImmediateLoad))
            {
                Success = true;
                return texture.Value;
            }
            Success = false;
            return MainMod.ErrorTexture.Value;
        }

        public virtual bool Availability(Companion companion)
        {
            return true;
        }
        
        protected virtual void OnLoad()
        {

        }

        protected virtual void OnUnload()
        {

        }

        public virtual void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        void ReplaceTexture(Texture2D NewTexture, ref DrawData dd) //Method made to help replace texture.
        {
            DrawData ndd = new DrawData(NewTexture, dd.position, dd.sourceRect, dd.color, dd.rotation, dd.origin, dd.scale.Y, dd.effect, 0);
            dd = ndd;
        }

        public virtual void ModifyHeadDraw(ref Texture2D HeadTexture, ref Rectangle DrawRect)
        {
            
        }
    }
}