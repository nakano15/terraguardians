using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSpritesContainer
    {
        private CompanionBase ReferedCompanionInfo;
        private Mod ReferedCompanionMod;
        private SpritesLoadState loadState = SpritesLoadState.NotLoaded;
        private Texture2D _HeadTexture;
        private Texture2D _BodyTexture;
        private Texture2D _BodyFrontTexture;
        private Texture2D[] _ArmTextures;
        public SpritesLoadState LoadState { get{ return loadState; } }
        private Dictionary<string, ExtraTexture> extratextures = new Dictionary<string, ExtraTexture>();
        string ContentLocation;

        public Texture2D HeadTexture
        {
            get
            {
                return _HeadTexture;
            }
            private set
            {
                _HeadTexture = value;
            }
        }
        public Texture2D BodyTexture
        {
            get
            {
                return _BodyTexture;
            }
            private set
            {
                _BodyTexture = value;
            }
        }
        public Texture2D BodyFrontTexture
        {
            get
            {
                return _BodyFrontTexture;
            }
            private set
            {
                _BodyFrontTexture = value;
            }
        }
        public Texture2D[] ArmSpritesTexture
        {
            get
            {
                return _ArmTextures;
            }
        }

        public CompanionSpritesContainer(CompanionBase companionBase, Mod mod)
        {
            ReferedCompanionInfo = companionBase;
            ReferedCompanionMod = mod;
            if(ReferedCompanionMod == null)
                ReferedCompanionMod = MainMod.GetMod;
            ContentLocation = ReferedCompanionMod.Name + "/Companions/" + ReferedCompanionInfo.CompanionContentFolderName + "/";
        }

        public void LoadContent()
        {
            if (loadState == SpritesLoadState.Error)
                return;
            try
            {
                if(!ModContent.HasAsset(ContentLocation + "body"))
                {
                    loadState = SpritesLoadState.Error;
                    return;
                }
                HeadTexture = TryLoading(ContentLocation + "head");
                BodyTexture = TryLoading(ContentLocation + "body");
                BodyFrontTexture = TryLoading(ContentLocation + "body_front");
                _ArmTextures = new Texture2D[2];
                _ArmTextures[0] = TryLoading(ContentLocation + "left_arm");
                _ArmTextures[1] = TryLoading(ContentLocation + "right_arm");
                loadState = SpritesLoadState.Loaded;
            }
            catch
            {
                loadState = SpritesLoadState.Error;
            }
        }

        public void AddExtraTexture(string name, string path)
        {
            if(!extratextures.ContainsKey(name))
                extratextures.Add(name, new ExtraTexture(path));
        }

        public Texture2D GetExtraTexture(string name)
        {
            if (!extratextures.ContainsKey(name)) return MainMod.ErrorTexture.Value;
            if(extratextures[name].loadstate == SpritesLoadState.NotLoaded)
            {
                bool success;
                extratextures[name].SetTexture(TryLoading(ContentLocation + extratextures[name].path, out success));
                extratextures[name].SetLoadState(success ? SpritesLoadState.Loaded : SpritesLoadState.Error);
            }
            return extratextures[name].texture;
        }

        internal void Unload()
        {
            if(HeadTexture != null)
            {
                _HeadTexture = null;
            }
            if(BodyTexture != null)
            {
                _BodyTexture = null;
            }
            if(BodyFrontTexture != null)
            {
                _BodyFrontTexture = null;
            }
            if(ArmSpritesTexture != null)
            {
                for(byte i = 0; i < ArmSpritesTexture.Length; i++)
                {
                    _ArmTextures[i] = null;
                }
                _ArmTextures = null;
            }
            ReferedCompanionInfo = null;
            ReferedCompanionMod = null;
            foreach(ExtraTexture extra in extratextures.Values)
            {
                extra.Unload();
            }
            extratextures.Clear();
            extratextures = null;
        }

        private Texture2D TryLoading(string Path)
        {
            bool success;
            return TryLoading(Path, out success);
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

        public enum SpritesLoadState : byte
        {
            NotLoaded = 0,
            Error = 1,
            Loaded = 2
        }

        protected struct ExtraTexture
        {
            public Texture2D texture;
            public SpritesLoadState loadstate;
            public string path;

            public ExtraTexture(string texturepath)
            {
                path = texturepath;
                texture = null;
                loadstate = SpritesLoadState.NotLoaded;
            }

            public void SetTexture(Texture2D texture)
            {
                this.texture = texture;
            }

            public void SetLoadState(SpritesLoadState state)
            {
                loadstate = state;
            }

            public void Unload()
            {
                texture = null;
                path = null;
            }
        }
    }
}