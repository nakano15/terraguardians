using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionsDoorHelper
    {
        List<Point> OpenedDoors = new List<Point>();
        int LastX = 0;
        public void Update(Companion companion, bool IgnoreDoors = false)
        {
            float CenterX = companion.Center.X;
            CheckDoorsToClose(CenterX, companion);
            if (!IgnoreDoors)
            {
                CheckForOpenedDoorsAtPosition(CenterX, companion);
                CheckForDoorsToOpen(companion, CenterX);
            }
        }

        bool IsClosedDoor(int TileType)
        {
            return TileType == TileID.ClosedDoor || TileID.Sets.OpenDoorID[TileType] > -1;
        }

        bool IsOpenDoor(int TileType)
        {
            return TileType == TileID.OpenDoor || TileID.Sets.CloseDoorID[TileType] > -1;
        }

        void CheckForDoorsToOpen(Companion companion, float CenterX)
        {
            int direction = companion.direction;
            if (companion.MoveRight)
                direction = 1;
            else if(companion.MoveLeft)
                direction = -1;
            int NextX = (int)((CenterX + 22 * direction + companion.velocity.X) * Companion.DivisionBy16);
            int StartY = (int)((companion.Bottom.Y - 1) * Companion.DivisionBy16);
            for (int y = 0; y < 3; y++)
            {
                int NextY = StartY - y;
                Tile tile = Main.tile[NextX, NextY];
                if (tile != null && tile.HasTile && IsClosedDoor(tile.TileType))
                {
                    int TileType = tile.TileType;
                    ModTile modt = ModContent.GetModTile(TileType);
                    if (modt != null) //If mod tile, set as door type 10.
                        TileType = 10;
                    bool TryToOpen = false;
                    switch(TileType)
                    {
                        case Terraria.ID.TileID.ClosedDoor:
                            {
                                int TileY = 1 - tile.TileFrameY % 54 / 18;
                                NextY += TileY;
                                TryToOpen = true;
                            }
                            break;
                    }
                    if (TryToOpen)
                    {
                        foreach (Point door in OpenedDoors)
                        {
                            if (door.X == NextX && door.Y == NextY)
                            {
                                return;
                            }
                        }
                        int Direction = companion.direction;
                        if (companion.velocity.X != 0)
                        {
                            if (companion.velocity.X < 0) Direction = -1;
                            else Direction = 1;
                        }
                        WorldGen.OpenDoor(NextX, NextY, Direction);
                        if (CheckForHouse(NextX, NextY))
                            OpenedDoors.Add(new Point(NextX, NextY));
                    }
                }
            }
        }

        void CheckForOpenedDoorsAtPosition(float CenterX, Companion companion)
        {
            int NewX = (int)(CenterX * Companion.DivisionBy16);
            if (LastX != NewX)
            {
                LastX = NewX;
                int NewY = (int)((companion.Bottom.Y - 20) * Companion.DivisionBy16);
                Tile tile = Main.tile[NewX, NewY];
                if (tile != null && tile.HasTile && IsOpenDoor(tile.TileType) && CheckForHouse(NewX, NewY))
                {
                    int TileType = tile.TileType;
                    ModTile modt = ModContent.GetModTile(TileType);
                    if (modt != null) //If mod tile, set as door type 10.
                        TileType = 11;
                    bool TryAddDoor = false;
                    switch(tile.TileType)
                    {
                        case TileID.OpenDoor:
                            {
                                int FrameX = tile.TileFrameX % 72;
                                if (FrameX == 0 || FrameX == 54)
                                {
                                    TryAddDoor = true;
                                }
                            }
                            break;
                        case TileID.TallGateOpen:
                            {
                                TryAddDoor = true;
                            }
                            break;
                    }
                    if (TryAddDoor)
                    {
                        foreach(Point door in OpenedDoors)
                        {
                            if (door.X == NewX && door.Y == NewY)
                            {
                                return;
                            }
                        }
                        OpenedDoors.Add(new Point(NewX, NewY));
                    }
                }
            }
        }

        void CheckDoorsToClose(float CenterX, Companion companion)
        {
            for (int i = 0; i < OpenedDoors.Count; i++)
            {
                float DistanceFromDoor = Math.Abs(OpenedDoors[i].X * 16 + 8 - CenterX);
                float DistanceYFromDoor = Math.Abs(OpenedDoors[i].Y * 16 + 8 - companion.Bottom.Y - 20);
                if (DistanceFromDoor > 28 || DistanceYFromDoor > 28)
                {
                    WorldGen.CloseDoor(OpenedDoors[i].X, OpenedDoors[i].Y);
                    OpenedDoors.RemoveAt(i);
                }
            }
        }

        bool CheckForHouse(int TileX, int TileY)
        {
            for (int x = -1; x < 2; x++)
            {
                Tile tile = Main.tile[x + TileX, TileY];
                if (tile != null && Main.wallHouse[tile.WallType])
                {
                    return true;
                }
            }
            return false;
        }
    }
}