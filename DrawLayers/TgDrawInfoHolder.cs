using Microsoft.Xna.Framework;
using Terraria;
using terraguardians;

public struct TgDrawInfoHolder
{
    public Vector2 DrawPosition;
    public Vector2 Origin;
    public Color DrawColor;

    public TgDrawInfoHolder(TerraGuardian tg, Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        DrawPosition = drawInfo.Position + new Vector2(tg.width * 0.5f, tg.height + 2) + tg.DeadBodyPosition;
        DrawPosition -= Main.screenPosition;
        DrawPosition.X = (int)DrawPosition.X;
        DrawPosition.Y = (int)DrawPosition.Y;
        Origin = new Vector2(tg.Base.SpriteWidth * 0.5f, tg.Base.SpriteHeight);
        DrawColor = Lighting.GetColor((int)(tg.Center.X * (1f / 16)), (int)(tg.Center.Y * (1f / 16)), Color.White) * ((255 - tg.immuneAlpha) * (1f / 255));
    }
}