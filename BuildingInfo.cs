using Terraria;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;

namespace terraguardians
{
    public class BuildingInfo
    {
        public int HomePointX = 0, HomePointY = 0;
        public int HouseStartX = -1, HouseEndX = -1,
            HouseStartY = -1, HouseEndY = -1;
        public List<FurnitureInfo> Furnitures = new List<FurnitureInfo>();
        private List<BytePoint> HousePoints = new List<BytePoint>();
        public bool ValidHouse = false;
        public List<CompanionTownNpcState> CompanionsLivingHere = new List<CompanionTownNpcState>();

        public struct BytePoint
        {
            public byte X, Y;
            public BytePoint(byte X, byte Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }
        
        public void UpdateTileState(ushort Type, int PositionX, int PositionY, bool Addition)
            {
                if (Addition)
                {
                retry:
                    Tile tile = Main.tile[PositionX, PositionY];
                    bool Add = false, FacingLeft = false;
                    switch (tile.TileType)
                    {
                        case TileID.Chairs:
                            FacingLeft = tile.TileFrameX < 18;
                            if (tile.TileFrameY % 40 >= 18)
                                Add = true;
                            else
                            {
                                PositionY++;
                                goto retry;
                            }
                            break;
                        case TileID.Thrones:
                            if (tile.TileFrameY % 72 >= 54 && tile.TileFrameX == 18)
                                Add = true;
                            else
                            {
                                if (tile.TileFrameX < 18)
                                    PositionX++;
                                else if (tile.TileFrameX > 18)
                                    PositionX--;
                                if (tile.TileFrameY % 72 < 54)
                                    PositionY++;
                            }
                            break;
                        case TileID.Benches:
                            if (tile.TileFrameY % 36 >= 18 && tile.TileFrameX == 18)
                                Add = true;
                            else
                            {
                                if (tile.TileFrameX < 18)
                                    PositionX++;
                                else if (tile.TileFrameX > 18)
                                    PositionX--;
                                if (tile.TileFrameY % 36 < 18)
                                    PositionY++;
                            }
                            break;
                        case TileID.Beds:
                            //FacingLeft = tile.TileFrameX < 72;
                            if (tile.TileFrameY % 36 >= 18 && tile.TileFrameX % 72 == 36)
                                Add = true;
                            else
                            {
                                int frameX = tile.TileFrameX % 72;
                                /*if (FacingLeft)
                                {
                                    if (frameX < 18)
                                        PositionX++;
                                    else if (frameX > 18)
                                        PositionX--;
                                }
                                else
                                {*/
                                    if (frameX < 36)
                                        PositionX++;
                                    else if (frameX > 36)
                                        PositionX--;
                                //}
                                if (tile.TileFrameY % 36 < 18)
                                    PositionY++;
                            }
                            break;
                        case TileID.Signs:
                        case TileID.AnnouncementBox:
                        case TileID.Tombstones:
                            {
                                PositionX -= (int)(tile.TileFrameX * (1f / 18)) % 2;
                                PositionY -= (int)(tile.TileFrameY * (1f / 18));
                                Add = true;
                            }
                            break;
                    }
                    foreach (FurnitureInfo fi in Furnitures)
                    {
                        if (fi.FurnitureID == Type && fi.FurnitureX == PositionX && fi.FurnitureY == PositionY)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                        Furnitures.Add(new FurnitureInfo(tile.TileType, PositionX, PositionY));
                }
                else
                {
                    for (int t = 0; t < Furnitures.Count; t++)
                    {
                        if (Furnitures[t].FurnitureID == Type)
                        {
                            FurnitureInfo fi = Furnitures[t];
                            switch (Type)
                            {
                                case TileID.Chairs:
                                    if (fi.FurnitureX == PositionX && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Thrones:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 3 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Benches:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Beds:
                                    if (PositionX >= fi.FurnitureX - (fi.FacingLeft ? 1 : 2) && PositionX <= fi.FurnitureX + (fi.FacingLeft ? 2 : 1) && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Signs:
                                case TileID.AnnouncementBox:
                                case TileID.Tombstones:
                                    if(PositionX >= fi.FurnitureX && PositionX < fi.FurnitureX + 1 && PositionY >= fi.FurnitureY &&PositionY < fi.FurnitureY + 1)
                                    {
                                        Furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            public void ValidateHouse()
            {
                if (HomePointX == -1 || HomePointY == -1)
                    return;
                HousePoints.Clear();
                if (WorldGen.StartRoomCheck(HomePointX, HomePointY))
                {
                    HouseStartX = WorldGen.roomX1;
                    HouseEndX = WorldGen.roomX2 + 1;
                    HouseStartY = WorldGen.roomY1;
                    HouseEndY = WorldGen.roomY2 + 1;
                    Furnitures.Clear();
                    for (int i = 0; i < WorldGen.numRoomTiles; i++)
                    {
                        int X = WorldGen.roomX[i], Y = WorldGen.roomY[i];
                        Tile tile = Main.tile[X, Y];
                        UpdateTileState(tile.TileType, X, Y, true);
                        HousePoints.Add(new BytePoint((byte)(X - WorldGen.roomX1), (byte)(Y - WorldGen.roomY1)));
                    }
                    ValidHouse = true;
                }
                else
                {
                    ValidHouse = false;
                }
            }

            public bool BelongsToThisHousing(int X, int Y)
            {
                foreach(BytePoint bp in HousePoints)
                {
                    if (X == bp.X + HouseStartX && Y == bp.Y + HouseStartY)
                        return true;
                }
                return false;
            }
    }
}