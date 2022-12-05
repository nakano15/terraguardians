using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class BehaviorBase
    {
        #region Hooks
        public virtual void Update(Companion companion)
        {
            
        }

        public virtual string CompanionNameChange(Companion companion)
        {
            return companion.name;
        }

        public virtual MessageBase ChangeStartDialogue(Companion companion)
        {
            return null;
        }

        public virtual bool AllowStartingDialogue(Companion companion)
        {
            return true;
        }

        public virtual void ChangeLobbyDialogueOptions(MessageBase Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
        }

        public virtual void UpdateAnimationFrame(Companion companion)
        {

        }
        #endregion

        public Player ViewRangeCheck(Companion companion, int Direction, int DistanceX = 300, int DistanceY = 150, bool SpotPlayers = true, bool SpotCompanions = false)
        {
            Player Nearest = null;
            float NearestDistance = float.MaxValue;
            Rectangle rect = new Rectangle((int)companion.Center.X, (int)companion.Center.Y, DistanceX, DistanceY);
            rect.Y -= (int)(rect.Height * 0.5f);
            if(Direction < 0) rect.X -= rect.Width;
            for(int p = 0; p < 255; p++)
            {
                if(Main.player[p].active && !Main.player[p].dead && Main.player[p] != companion)
                {
                    if(Main.player[p] is Companion)
                    {
                        if(!SpotCompanions || (Main.player[p] as Companion).Owner == null) continue;
                    }
                    else
                    {
                        if (!SpotPlayers) continue;
                    }
                    if(rect.Intersects(Main.player[p].getRect()))
                    {
                        float Distance = (Main.player[p].Center - companion.Center).Length();
                        if(Distance < NearestDistance)
                        {
                            Nearest = Main.player[p];
                            NearestDistance = Distance;
                        }
                    }
                }
            }
            return Nearest;
        }

        public Point GetClosestBed(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null)
        {
            Point Pos = Position.ToTileCoordinates();
            Point[] Beds = GetFurnituresCloseBy(Pos, DistanceX, DistanceY, false, true, HouseLimitation);
            Point NearestPos = Point.Zero;
            float NearestDistance = float.MaxValue;
            foreach(Point p in Beds)
            {
                float Distance = (MathF.Abs(Pos.X - p.X) + MathF.Abs(Pos.Y - p.Y)) * 0.5f;
                if(Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestPos = p;
                }
            }
            return NearestPos;
        }

        public Point GetClosestChair(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null)
        {
            Point Pos = Position.ToTileCoordinates();
            Point[] Chairs = GetFurnituresCloseBy(Pos, DistanceX, DistanceY, true, false, HouseLimitation);
            Point NearestPos = Point.Zero;
            float NearestDistance = float.MaxValue;
            foreach(Point p in Chairs)
            {
                float Distance = (MathF.Abs(Pos.X - p.X) + MathF.Abs(Pos.Y - p.Y)) * 0.5f;
                if(Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestPos = p;
                }
            }
            return NearestPos;
        }

        public Point[] GetBedsCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null)
        {
            return GetFurnituresCloseBy(Position, DistanceX, DistanceY, false, true, HouseLimitation);
        }

        public Point[] GetChairsCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, BuildingInfo HouseLimitation = null)
        {
            return GetFurnituresCloseBy(Position, DistanceX, DistanceY, true, false, HouseLimitation);
        }

        public Point[] GetFurnituresCloseBy(Vector2 Position, int DistanceX = 8, int DistanceY = 6, bool GetChairs = true, bool GetBeds = true, BuildingInfo HouseLimitation = null)
        {
            int TileX = (int)(Position.X * (1f / 16));
            int TileY = (int)(Position.Y * (1f / 16));
            return GetFurnituresCloseBy(new Point(TileX, TileY), DistanceX, DistanceY, GetChairs, GetBeds, HouseLimitation);
        }

        public Point[] GetFurnituresCloseBy(Point Position, int DistanceX = 8, int DistanceY = 6, bool GetChairs = true, bool GetBeds = true, BuildingInfo HouseLimitation = null)
        {
            List<Point> FoundFurnitures = new List<Point>();
            for (int y = Position.Y - DistanceY; y <= Position.Y + DistanceY; y++)
            {
                for (int x = Position.X - DistanceX; x <= Position.X + DistanceX; x++)
                {
                    if(!WorldGen.InWorld(x, y)) continue;
                    if(HouseLimitation != null && !HouseLimitation.BelongsToThisHousing(x, y)) continue;
                    Tile tile = Main.tile[x, y];
                    if (tile != null && !tile.HasTile) continue;
                    bool TakeFurniture = false;
                    bool IsBed = false;
                    switch(tile.TileType)
                    {
                        case TileID.Chairs:
                            if (GetChairs && tile.TileFrameY % 40 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0)
                            {
                                TakeFurniture = true;
                            }
                            break;
                        case TileID.Thrones:
                            if (GetChairs && tile.TileFrameX % 54 == 18 && tile.TileFrameY % 72 == 54 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0)
                            {
                                TakeFurniture = true;
                            }
                            break;
                        case TileID.Benches:
                            if (GetChairs && tile.TileFrameX % 54 == 18 && tile.TileFrameY % 36 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0)
                            {
                                TakeFurniture = true;
                            }
                            break;
                        case TileID.PicnicTable:
                            {
                                if (GetChairs)
                                {
                                    int FrameX = tile.TileFrameX % 72;
                                    if((FrameX == 0 || FrameX == 54) && tile.TileFrameY % 36 == 18 && Main.sittingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0)
                                    {
                                        TakeFurniture = true;
                                    }
                                }
                            }
                            break;
                        case TileID.Beds:
                            {
                                IsBed = true;
                                bool FacingLeft = tile.TileFrameX < 72;
                                if (GetBeds && tile.TileFrameX % 72 == (FacingLeft ? 36 : 18) && tile.TileFrameY % 36 == 18 && Main.sleepingManager.GetNextPlayerStackIndexInCoords(new Point(x, y)) == 0)
                                {
                                    TakeFurniture = true;
                                }
                            }
                            break;
                    }
                    if (TakeFurniture)
                    {
                        byte FurnitureUsers = 0;
                        foreach(Companion c in MainMod.ActiveCompanions.Values)
                        {
                            if (c.GetFurnitureX == x && c.GetFurnitureY == y)
                            {
                                FurnitureUsers++;
                            }
                        }
                        if (FurnitureUsers < 1 || (IsBed && FurnitureUsers < 2))
                            FoundFurnitures.Add(new Point(x, y));
                    }
                }
            }
            return FoundFurnitures.ToArray();
        }

        public bool ThereIsPlayerInNpcViewRange(Companion companion)
        {
            bool PlayerClose = false;
            for(int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && !(Main.player[i] is Companion))
                {
                    if (MathF.Abs(Main.player[i].Center.X - companion.Center.X) < NPC.sWidth * 0.5f + NPC.safeRangeX && 
                        MathF.Abs(Main.player[i].Center.Y - companion.Center.Y) < NPC.sHeight * 0.5f + NPC.safeRangeY)
                        {
                            PlayerClose = true;
                            break;
                        }
                }
            }
            return PlayerClose;
        }
    }
}