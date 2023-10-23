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
        private Texture2D[] _ArmFrontTextures;
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
        public Texture2D[] ArmFrontSpritesTexture
        {
            get
            {
                return _ArmFrontTextures;
            }
        }

        internal void SetSpritesContainerInfos(CompanionBase companionBase, Mod mod)
        {
            ReferedCompanionInfo = companionBase;
            ReferedCompanionMod = mod;
            if(ReferedCompanionMod == null)
                ReferedCompanionMod = MainMod.GetMod;
            ContentLocation = ReferedCompanionMod.Name + "/Companions/" + ReferedCompanionInfo.CompanionContentFolderName + "/";
        }

        public virtual byte ArmTextures => 2;
        public virtual string HeadTextureDirectory => "head";
        public virtual string BodyTextureDirectory => "body";
        public virtual string BodyFrontTextureDirectory => "body_front";
        public virtual string ArmTextureDirectory(byte Arm)
        {
            if (Arm == 0) return "left_arm";
            return "right_arm";
        }
        public virtual string ArmFrontTextureDirectory(byte Arm)
        {
            if (Arm == 0) return "left_arm_front";
            return "right_arm_front";
        }

        public void LoadContent()
        {
            if (loadState == SpritesLoadState.Error)
                return;
            try
            {
                if (ReferedCompanionInfo.CompanionType == CompanionTypes.TerraGuardian)
                {
                    if(!ModContent.HasAsset(ContentLocation + BodyFrontTextureDirectory))
                    {
                        loadState = SpritesLoadState.Error;
                        return;
                    }
                    HeadTexture = TryLoading(ContentLocation + HeadTextureDirectory);
                    BodyTexture = TryLoading(ContentLocation + BodyTextureDirectory);
                    BodyFrontTexture = TryLoading(ContentLocation + BodyFrontTextureDirectory);
                    _ArmTextures = new Texture2D[ArmTextures];
                    _ArmFrontTextures = new Texture2D[ArmTextures];
                    for(byte i = 0; i < ArmTextures; i++)
                    {
                        _ArmTextures[i] = TryLoading(ContentLocation + ArmTextureDirectory(i));
                        _ArmFrontTextures[i] = TryLoading(ContentLocation + ArmFrontTextureDirectory(i));
                    }
                }
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
                    _ArmFrontTextures[i] = null;
                }
                _ArmTextures = null;
                _ArmFrontTextures = null;
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

        public class ExtraTexture
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
                this.loadstate = state;
            }

            public void Unload()
            {
                texture = null;
                path = null;
            }
        }
    }
}