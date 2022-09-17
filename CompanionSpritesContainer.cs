using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public SpritesLoadState LoadState { get{ return loadState; }}

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
        }

        public void LoadContent()
        {
            if (loadState == SpritesLoadState.Error)
                return;
            try
            {
                string ContentLocation = ReferedCompanionMod.Name + "/Companions/" + ReferedCompanionInfo.CompanionContentFolderName + "/";
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

        internal void Unload()
        {
            if(HeadTexture != null)
            {
                //HeadTexture.Dispose();
                _HeadTexture = null;
            }
            if(BodyTexture != null)
            {
                //BodyTexture.Dispose();
                _BodyTexture = null;
            }
            if(BodyFrontTexture != null)
            {
                //BodyFrontTexture.Dispose();
                _BodyFrontTexture = null;
            }
            if(ArmSpritesTexture != null)
            {
                for(byte i = 0; i < ArmSpritesTexture.Length; i++)
                {
                    //ArmSpritesTexture[i].Dispose();
                    _ArmTextures[i] = null;
                }
                _ArmTextures = null;
            }
            ReferedCompanionInfo = null;
            ReferedCompanionMod = null;
        }

        private Texture2D TryLoading(string Path)
        {
            ReLogic.Content.Asset<Texture2D> texture;
            if(ModContent.RequestIfExists<Texture2D>(Path, out texture, ReLogic.Content.AssetRequestMode.ImmediateLoad))
            {
                return texture.Value;
            }
            return MainMod.ErrorTexture.Value;
        }

        public enum SpritesLoadState : byte
        {
            NotLoaded = 0,
            Error = 1,
            Loaded = 2
        }
    }
}