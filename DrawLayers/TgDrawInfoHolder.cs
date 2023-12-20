using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using terraguardians;

public class TgDrawInfoHolder
{
    private Companion companion;
    private TerraGuardian tg;
    public Vector2 DrawPosition;
    public Vector2 Origin;
    public Color DrawColor, HatColor;
    public int MountYOffsetBackup;
    public int GetMountYOffsetChange { get{ if (tg == null) return 0; return (int)(tg.height * 0.5f - 21); }}
    public DrawContext Context;
    public bool ThroneMode;
    public bool IsTerraGuardian { get { return tg != null; } }
    public Companion GetCompanion { get { return tg != null ? tg : companion; } }
    public Texture2D BodyTexture, BodyFrontTexture;
    public Texture2D[] ArmTexture, ArmFrontTexture;
    public Rectangle BodyFrame, BodyFrontFrame;
    public Rectangle[] ArmFrame, ArmFrontFrame;

    public TgDrawInfoHolder(Companion tg, Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        Context = TerraGuardiansPlayerRenderer.GetDrawRule;
        if (tg is TerraGuardian)
        {
            this.tg = tg as TerraGuardian;
            float Width = tg.SpriteWidth, Height = tg.SpriteHeight;
            DrawPosition = drawInfo.Position + this.tg.DeadBodyPosition;
            DrawPosition.X += (tg.width - Width) * 0.5f + Width * tg.fullRotationOrigin.X;
            DrawPosition.Y += (tg.height - Height) + Height * tg.fullRotationOrigin.Y + 2;
            Origin = new Vector2(tg.Base.SpriteWidth * tg.fullRotationOrigin.X, tg.Base.SpriteHeight * tg.fullRotationOrigin.Y);
            CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
            if (spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
            {
                BodyTexture = spritecontainer.BodyTexture;
                BodyFrontTexture = spritecontainer.BodyFrontTexture;
                ArmTexture = new Texture2D[spritecontainer.ArmTextures];
                ArmFrontTexture = new Texture2D[spritecontainer.ArmTextures];
                ArmFrame = new Rectangle[spritecontainer.ArmTextures];
                ArmFrontFrame = new Rectangle[spritecontainer.ArmTextures];
                BodyFrame = this.tg.BodyFrame;
                BodyFrontFrame = this.tg.BodyFrontFrame;
                for (int i = 0; i < spritecontainer.ArmTextures; i++)
                {
                    ArmTexture[i] = spritecontainer.ArmSpritesTexture[i];
                    ArmFrontTexture[i] = spritecontainer.ArmFrontSpritesTexture[i];
                    ArmFrame[i] = this.tg.ArmFrame[i];
                    ArmFrontFrame[i] = this.tg.ArmFrontFrame[i];
                }
            }
            else
            {
                BodyTexture = null;
                BodyFrontTexture = null;
                ArmTexture = null;
                ArmFrontTexture = null;
                BodyFrame = default(Rectangle);
                BodyFrontFrame = default(Rectangle);
                ArmFrame = null;
                ArmFrontFrame = null;
            }
        }
        else
        {
            companion = tg;
            Origin = new Vector2(20, 56);
            DrawPosition = drawInfo.Position + new Vector2(tg.width * 0.5f, tg.height + 2);
            //DrawPosition += Origin;
            BodyTexture = null;
            BodyFrontTexture = null;
            ArmTexture = null;
            ArmFrontTexture = null;
        }
        DrawPosition -= Main.screenPosition;
        DrawPosition.X = (int)DrawPosition.X;
        DrawPosition.Y = (int)DrawPosition.Y;
        Color LightingColor = tg.GetImmuneAlpha(Lighting.GetColorClamped((int)(tg.Center.X * (1f / 16)), (int)(tg.Center.Y * (1f / 16)), Color.White), drawInfo.shadow);
        HatColor = (tg.ShouldNotDraw || !tg.GetGoverningBehavior().IsVisible) ? Color.Transparent :
            LightingColor;
        DrawColor = (tg.ShouldNotDraw || !tg.GetGoverningBehavior().IsVisible) ? 
            Color.Transparent : 
            LightingColor;
        /*if(tg.mount.Active)
        {
            MountYOffsetBackup = tg.mount._data.yOffset;
            tg.mount._data.yOffset += GetMountYOffsetChange;
        }
        else*/
        MountYOffsetBackup = 0; //Even backing up the YOffset, the values are ending up broken as game runs. And reloading mods doesn't reverts the yOffset back to normal.
        ThroneMode = tg.IsUsingThroneOrBench;
    }

    public void RevertMountOffset(ref int OffsetY)
    {
        OffsetY -= GetMountYOffsetChange;
    }
}

public enum DrawContext : byte
{
    AllParts,
    BackLayer,
    FrontLayer
}