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
                        case TileID.OpenDoor:
                        case TileID.ClosedDoor:
                        case TileID.TallGateOpen:
                        case TileID.TallGateClosed:
                        case TileID.TrapdoorOpen:
                        case TileID.TrapdoorClosed:
                            return;
                        /*default:
                            if (!Main.tileSolid[tile.TileType])
                            {
                                //Find out how to discover tile size, and then log its center position.
                                WorldGen.TileType
                            }
                            break;*/
                        case TileID.WorkBenches: //2x1
                        case TileID.Anvils:
                        case TileID.MythrilAnvil:
                            {
                                if (tile.TileFrameX % 36 == 18)
                                {
                                    Add = true;
                                }
                                else
                                {
                                    PositionX += 1 - tile.TileFrameX % 36 / 18;
                                    goto retry;
                                }
                            }
                            break;
                        case TileID.Sinks: //2x2
                        case TileID.CookingPots:
                        case TileID.CrystalBall:
                        case TileID.Kegs:
                        case TileID.TeaKettle:
                        case TileID.Containers:
                        case TileID.Containers2:
                            {
                                if (tile.TileFrameX % 36 == 18 && tile.TileFrameY % 36 == 18)
                                {
                                    Add = true;
                                }
                                else
                                {
                                    PositionX += 1 - tile.TileFrameX % 36 / 18;
                                    PositionY += 1 - tile.TileFrameY % 36 / 18;
                                    goto retry;
                                }
                            }
                            break;
                        case TileID.Tables: //3x2
                        case TileID.Tables2:
                        case TileID.Furnaces:
                        case TileID.Loom:
                        case TileID.TinkerersWorkbench:
                        case TileID.DemonAltar:
                        case TileID.AdamantiteForge:
                        case TileID.Blendomatic:
                        case TileID.MeatGrinder:
                        case TileID.Campfire:
                        case TileID.Dressers:
                        case TileID.Fireplace:
                            {
                                if (tile.TileFrameX % 54 == 18 && tile.TileFrameY % 36 == 18)
                                {
                                    Add = true;
                                }
                                else
                                {
                                    PositionX += 1 - tile.TileFrameX % 54 / 18;
                                    PositionY += 1 - tile.TileFrameY % 36 / 18;
                                    goto retry;
                                }
                            }
                            break;
                        case TileID.AlchemyTable: //3x3
                        case TileID.Sawmill:
                        case TileID.ImbuingStation:
                        case TileID.DyeVat:
                        case TileID.HeavyWorkBench:
                        case TileID.Autohammer:
                        case TileID.LunarCraftingStation:
                        case TileID.BoneWelder:
                        case TileID.GlassKiln:
                        case TileID.HoneyDispenser:
                        case TileID.IceMachine:
                        case TileID.LivingLoom:
                        case TileID.SkyMill:
                        case TileID.Solidifier:
                        case 499: //Decay Chamber
                        case TileID.FleshCloningVat:
                        case TileID.SteampunkBoiler:
                        case TileID.LihzahrdFurnace:
                        case TileID.Extractinator:
                        case TileID.ChlorophyteExtractinator:
                            {
                                if (tile.TileFrameX % 54 == 18 && tile.TileFrameY % 54 == 36)
                                {
                                    Add = true;
                                }
                                else
                                {
                                    PositionX += 1 - tile.TileFrameX % 54 / 18;
                                    PositionY += 2 - tile.TileFrameY % 36 / 18;
                                    goto retry;
                                }
                            }
                            break;
                        case TileID.Bookcases: //3x4
                            {
                                if (tile.TileFrameX % 54 == 18 && tile.TileFrameY % 72 == 54)
                                {
                                    Add = true;
                                }
                                else
                                {
                                    PositionX += 1 - tile.TileFrameX % 54 / 18;
                                    PositionY += 3 - tile.TileFrameY % 72 / 18;
                                    goto retry;
                                }
                            }
                            break;
                        case TileID.Bottles: //1x1
                            {
                                Add = true;
                            }
                            break;
                        case TileID.Chairs:
                        case TileID.Toilets:
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
                                goto retry;
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
                                goto retry;
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
                                goto retry;
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
                    {
                        Furnitures.Add(new FurnitureInfo(tile.TileType, PositionX, PositionY));
                    }
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
                                case TileID.WorkBenches: //2x1
                                case TileID.Anvils:
                                case TileID.MythrilAnvil:
                                    if (fi.FurnitureX >= PositionX - 1 && fi.FurnitureX <= PositionX && PositionY == PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Sinks: //2x2
                                case TileID.CookingPots:
                                case TileID.CrystalBall:
                                case TileID.Kegs:
                                case TileID.TeaKettle:
                                case TileID.Containers:
                                case TileID.Containers2:
                                    if (fi.FurnitureX >= PositionX - 1 && fi.FurnitureX <= PositionX && 
                                        fi.FurnitureY >= PositionY - 1 && fi.FurnitureY <= PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Tables: //3x2
                                case TileID.Tables2:
                                case TileID.Furnaces:
                                case TileID.Loom:
                                case TileID.TinkerersWorkbench:
                                case TileID.DemonAltar:
                                case TileID.AdamantiteForge:
                                case TileID.Blendomatic:
                                case TileID.MeatGrinder:
                                case TileID.Campfire:
                                case TileID.Dressers:
                                case TileID.Fireplace:
                                    if (fi.FurnitureX >= PositionX - 1 && fi.FurnitureX <= PositionX + 1 && 
                                        fi.FurnitureY >= PositionY - 1 && fi.FurnitureY <= PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.AlchemyTable: //3x3
                                case TileID.Sawmill:
                                case TileID.ImbuingStation:
                                case TileID.DyeVat:
                                case TileID.HeavyWorkBench:
                                case TileID.Autohammer:
                                case TileID.LunarCraftingStation:
                                case TileID.BoneWelder:
                                case TileID.GlassKiln:
                                case TileID.HoneyDispenser:
                                case TileID.IceMachine:
                                case TileID.LivingLoom:
                                case TileID.SkyMill:
                                case TileID.Solidifier:
                                case 499: //Decay Chamber
                                case TileID.FleshCloningVat:
                                case TileID.SteampunkBoiler:
                                case TileID.LihzahrdFurnace:
                                case TileID.Extractinator:
                                case TileID.ChlorophyteExtractinator:
                                    if (fi.FurnitureX >= PositionX - 1 && fi.FurnitureX <= PositionX + 1 && 
                                        fi.FurnitureY >= PositionY - 2 && fi.FurnitureY <= PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Bookcases: //3x4
                                    if (fi.FurnitureX >= PositionX - 1 && fi.FurnitureX <= PositionX + 1 && 
                                        fi.FurnitureY >= PositionY - 3 && fi.FurnitureY <= PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Bottles: //1x1
                                    if (fi.FurnitureX == PositionX && fi.FurnitureY == PositionY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Chairs:
                                case TileID.Toilets:
                                    if (fi.FurnitureX == PositionX && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Thrones:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 3 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Benches:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Beds:
                                    if (PositionX >= fi.FurnitureX - (fi.FacingLeft ? 1 : 2) && PositionX <= fi.FurnitureX + (fi.FacingLeft ? 2 : 1) && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
                                    }
                                    break;
                                case TileID.Signs:
                                case TileID.AnnouncementBox:
                                case TileID.Tombstones:
                                    if(PositionX >= fi.FurnitureX && PositionX < fi.FurnitureX + 1 && PositionY >= fi.FurnitureY &&PositionY < fi.FurnitureY + 1)
                                    {
                                        Furnitures.RemoveAt(t);
                                        return;
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
                    HouseStartX = WorldGen.roomX1 - 1;
                    HouseEndX = WorldGen.roomX2 + 1;
                    HouseStartY = WorldGen.roomY1 - 1;
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