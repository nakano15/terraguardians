using Microsoft.Xna.Framework;
using Terraria;
using terraguardians;

public class TgDrawInfoHolder
{
    private Companion companion;
    private TerraGuardian tg;
    public Vector2 DrawPosition;
    public Vector2 Origin;
    public Color DrawColor;
    public int MountYOffsetBackup;
    public int GetMountYOffsetChange { get{ if (tg == null) return 0; return (int)(tg.height * 0.5f - 21); }}
    public DrawContext Context;
    public bool ThroneMode;
    public bool IsTerraGuardian { get { return tg != null; } }
    public Companion GetCompanion { get { return tg != null ? tg : companion; } }

    public TgDrawInfoHolder(Companion tg, Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        Context = TerraGuardiansPlayerRenderer.GetDrawRule;
        if (tg is TerraGuardian)
        {
            this.tg = tg as TerraGuardian;
            DrawPosition = drawInfo.Position + new Vector2(tg.width * 0.5f, tg.height + 2) + this.tg.DeadBodyPosition;
            Origin = new Vector2(tg.Base.SpriteWidth * 0.5f, tg.Base.SpriteHeight);
        }
        else
        {
            companion = tg;
            Origin = new Vector2(20, 56);
            DrawPosition = drawInfo.Position + new Vector2(tg.width * 0.5f, tg.height + 2);
            //DrawPosition += Origin;
        }
        DrawPosition -= Main.screenPosition;
        DrawPosition.X = (int)DrawPosition.X;
        DrawPosition.Y = (int)DrawPosition.Y;
        DrawColor = (tg.ShouldNotDraw || !tg.GetGoverningBehavior().IsVisible) ? 
            Color.Transparent : 
            tg.GetImmuneAlpha(Lighting.GetColorClamped((int)(tg.Center.X * (1f / 16)), (int)(tg.Center.Y * (1f / 16)), Color.White), drawInfo.shadow);
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