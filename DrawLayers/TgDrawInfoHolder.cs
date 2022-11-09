using Microsoft.Xna.Framework;
using Terraria;
using terraguardians;

public struct TgDrawInfoHolder
{
    public TerraGuardian tg;
    public Vector2 DrawPosition;
    public Vector2 Origin;
    public Color DrawColor;
    public int MountYOffsetBackup;
    public int GetMountYOffsetChange { get{ return (int)(tg.height * 0.5f - 21); }}
    public DrawContext Context;

    public TgDrawInfoHolder(TerraGuardian tg, Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        Context = TerraGuardianDrawLayersScript.Context;
        this.tg = tg;
        DrawPosition = drawInfo.Position + new Vector2(tg.width * 0.5f, tg.height + 2) + tg.DeadBodyPosition;
        DrawPosition -= Main.screenPosition;
        DrawPosition.X = (int)DrawPosition.X;
        DrawPosition.Y = (int)DrawPosition.Y;
        Origin = new Vector2(tg.Base.SpriteWidth * 0.5f, tg.Base.SpriteHeight);
        DrawColor = Lighting.GetColor((int)(tg.Center.X * (1f / 16)), (int)(tg.Center.Y * (1f / 16)), Color.White) * ((255 - tg.immuneAlpha) * (1f / 255));
        /*if(tg.mount.Active)
        {
            MountYOffsetBackup = tg.mount._data.yOffset;
            tg.mount._data.yOffset += GetMountYOffsetChange;
        }
        else*/
        MountYOffsetBackup = 0; //Even backing up the YOffset, the values are ending up broken as game runs. And reloading mods doesn't reverts the yOffset back to normal.
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