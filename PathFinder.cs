using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace terraguardians
{
    public class PathFinder
    {
        public const int MaxTileCheck = 500;
        public List<Breadcrumb> Path = new List<Breadcrumb>();
        public bool PathingInterrupted = false;
        public int SavedPosX = -1, SavedPosY = -1;
        public bool WalkToPath = false;
        public PathingState State = PathingState.NotSet;
        private byte StuckTimer = 0;
        public Breadcrumb GetLastNode { get { if (Path.Count > 0) return Path[0]; return null; } }
        public bool StrictPathFinding = true, CancelOnFail = false;
        
        public bool CreatePathTo(Vector2 StartPosition, Vector2 Destination, int JumpDistance = 6, int FallDistance = 6, bool WalkToPath = false, bool Strict = true, bool CancelOnFail = false)
        {
            return CreatePathTo(StartPosition, (int)(Destination.X * Companion.DivisionBy16),
                (int)(Destination.Y * Companion.DivisionBy16), JumpDistance, FallDistance, WalkToPath, Strict, CancelOnFail);
        }

        public bool CreatePathTo(Vector2 StartPosition, int EndPosX, int EndPosY, int JumpDistance = 6, int FallDistance = 6, bool WalkToPath = false, bool Strict = true, bool CancelOnFail = false)
        {
            if (!MainMod.UsePathfinding) return false;
            Path = DoPathFinding(StartPosition, EndPosX, EndPosY, JumpDistance, FallDistance);
            PathingInterrupted = false;
            SavedPosX = EndPosX;
            SavedPosY = EndPosY;
            this.WalkToPath = WalkToPath;
            State = Path.Count > 0 ? PathingState.TracingPath : PathingState.PathingFailed;
            StuckTimer = 0;
            StrictPathFinding = Strict;
            this.CancelOnFail = CancelOnFail;
            return Path.Count > 0;
        }

        public bool ResumePathingTo(Vector2 StartPosition, int JumpDistance = 6, int FallDistance = 6)
        {
            if (SavedPosX == -1 || SavedPosY == -1 || State != PathingState.TracingPath) return false;
            return CreatePathTo(StartPosition, SavedPosX, SavedPosY, JumpDistance, FallDistance, WalkToPath);
        }

        public void CancelPathing()
        {
            Path.Clear();
            State = PathingState.NotSet;
            PathingInterrupted = true;
            SavedPosX = -1;
            SavedPosY = -1;
        }

        public void ClearPath()
        {
            Path.Clear();
        }

        public bool CheckStuckTimer()
        {
            return StuckTimer >= 30;
        }

        public void IncreaseStuckTimer()
        {
            StuckTimer++;
        }

        public void ResetStuckTimer()
        {
            StuckTimer = 0;
        }

        public void RemoveLastNode()
        {
            if (Path.Count > 0) Path.RemoveAt(0);
            if (Path.Count == 0)
                State = PathingState.ReachedDestination;
        }

        public static List<Breadcrumb> DoPathFinding(Vector2 StartPosition, int EndPosX, int EndPosY, int JumpDistance = 6, int FallDistance = 6)
        {
            int StartPosX = (int)(StartPosition.X * (1f / 16)),
                StartPosY = (int)(StartPosition.Y * (1f / 16));
            {
                int Attempts = 0;
                while (CheckForSolidBlocks(StartPosX, StartPosY, 3))
                {
                    StartPosY--;
                    Attempts++;
                    if (Attempts >= 8)
                    {
                        return new List<Breadcrumb>();
                    }
                }
                Attempts = 0;
                while (!CheckForSolidGroundUnder(StartPosX, StartPosY))
                {
                    StartPosY++;
                    Attempts++;
                    if (Attempts >= 8)
                    {
                        return new List<Breadcrumb>();
                    }
                }
            }
            List<Node> LastNodeList = new List<Node>(),
                NextNodeList = new List<Node>();
            List<Point> VisitedNodes = new List<Point>();
            NextNodeList.Add(new Node(StartPosX, StartPosY, Node.NONE));
            VisitedNodes.Add(new Point(StartPosX, StartPosY));
            const int MaxDistance = 80; //50
            Node DestinationFound = null;
            int HangPreventer = 0;
            while (DestinationFound == null)
            {
                LastNodeList.Clear();
                LastNodeList.AddRange(NextNodeList);
                NextNodeList.Clear();
                if (LastNodeList.Count == 0) break;
                while (LastNodeList.Count > 0)
                {
                    Node n = LastNodeList[0];
                    LastNodeList.RemoveAt(0);
                    int X = n.NodeX, 
                        Y = n.NodeY;
                    if (!WorldGen.InWorld(X, Y)) continue;
                    /*Dust d = Dust.NewDustPerfect(new Vector2(X * 16 + 8, Y * 16 + 8), Terraria.ID.DustID.Flare, Vector2.Zero);
                    d.scale = 6;
                    d.noGravity = true;
                    d.noLight = false;*/
                    if (X == EndPosX && Y == EndPosY)
                    {
                        DestinationFound = n;
                        break;
                    }
                    if (MathF.Abs(n.NodeX - StartPosX) >= MaxDistance || MathF.Abs(n.NodeY - StartPosY) >= MaxDistance) continue;
                    for (byte dir = 0; dir < 4; dir++)
                    {
                        switch(dir)
                        {
                            case Node.DIR_UP:
                                {
                                    bool HasPlatform = false, HasSolidBlock = false;
                                    int PlatformNodeY = -1;
                                    for (int y = 0; y < JumpDistance; y++)
                                    {
                                        int YCheck = Y - y;
                                        if (y > 0)
                                        {
                                            for (sbyte x = -1; x < 2; x += 2)
                                            {
                                                if (!CheckForSolidBlocks(X + x, YCheck, PassThroughDoors: true) && CheckForSolidGroundUnder(X + x, YCheck, true) && !VisitedNodes.Contains(new Point(X, YCheck)))
                                                {
                                                    NextNodeList.Add(CreateNextNode(X, YCheck, Node.DIR_UP, n));
                                                    VisitedNodes.Add(new Point(X, YCheck));
                                                    break;
                                                }
                                            }
                                        }
                                        if ((y == 0 ? CheckForSolidBlocks(X, Y, 4) : CheckForSolidBlocksCeiling(X, YCheck)))
                                        {
                                            HasSolidBlock = true;
                                            break;
                                        }
                                        if (CheckForPlatform(X, YCheck) && !CheckForPlatform(X, YCheck + 1) && !CheckForSolidBlocks(X, YCheck - 1))
                                        {
                                            HasPlatform = true;
                                            PlatformNodeY = YCheck - 1;
                                            break;
                                        }
                                        //if (!CheckForSolidBlock(X, YCheck))
                                    }
                                    if (HasSolidBlock) continue;
                                    if (HasPlatform && !VisitedNodes.Contains(new Point(X, PlatformNodeY)))
                                    {
                                        NextNodeList.Add(CreateNextNode(X, PlatformNodeY, Node.DIR_UP, n));
                                        VisitedNodes.Add(new Point(X, PlatformNodeY));
                                    }
                                }
                                break;

                            case Node.DIR_DOWN:
                                {
                                    if (CheckForPlatform(X, Y + 1))
                                    {
                                        for (int y = 2; y <= FallDistance; y++)
                                        {
                                            int Yp = Y + y;
                                            if (CheckForSolidBlocks(X, Yp) || IsDangerousTile(X, Yp, false)) break;
                                            if (CheckForSolidGroundUnder(X, Yp) && !VisitedNodes.Contains(new Point(X, Yp)))
                                            {
                                                NextNodeList.Add(CreateNextNode(X, Yp, Node.DIR_DOWN, n));
                                                VisitedNodes.Add(new Point(X, Yp));
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;

                            case Node.DIR_LEFT:
                            case Node.DIR_RIGHT:
                                {
                                    sbyte Dir = (sbyte)(dir == Node.DIR_LEFT ? -1 : 1);
                                    int nx = X + Dir, ny = Y;
                                    if ((n.NodeDirection == Node.DIR_LEFT && dir == Node.DIR_RIGHT) ||
                                        (n.NodeDirection == Node.DIR_RIGHT && dir == Node.DIR_LEFT)) continue;
                                    bool Blocked = false;
                                    for (int zy = -1; zy < JumpDistance; zy++)
                                    {
                                        if (zy > 0 && (CheckForSolidBlocksCeiling(nx - Dir, ny - zy) || IsDangerousTile(nx - Dir, ny - zy, false)))
                                        {
                                            Blocked = true;
                                            break;
                                        }
                                        if (!CheckForSolidBlocks(nx, ny - zy, PassThroughDoors: true) && 
                                            CheckForSolidGroundUnder(nx, ny - zy, PassThroughDoors: true))
                                        {
                                            ny -= zy;
                                            break;
                                        }
                                    }
                                    if (Blocked) continue;
                                    sbyte MinCheckY = -1, MaxCheckY = 3;
                                    /*if (!CheckForSolidGroundUnder(nx, ny, true))
                                    {
                                        if (n.NodeDirection != Node.DIR_UP)
                                        {
                                            for (int y = 1; y <= FallDistance; y++)
                                            {
                                                int yc = ny + y;
                                                if (CheckForSolidBlocks(nx, yc, PassThroughDoors: true) || IsDangerousTile(nx, yc, false))
                                                    break;
                                                if (CheckForSolidGroundUnder(nx, yc, true) && CheckForPlatform(X, yc) && !CheckForStairFloor(nx, yc - 1))
                                                {
                                                    if (!VisitedNodes.Contains(new Point(nx, yc)))
                                                    {
                                                        if (!VisitedNodes.Contains(new Point(X, yc)))
                                                        {
                                                            n = CreateNextNode(X, yc, Node.DIR_DOWN, n);
                                                            NextNodeList.Add(n);
                                                            VisitedNodes.Add(new Point(X, yc));
                                                        }
                                                        NextNodeList.Add(CreateNextNode(nx, yc, dir, n)); //here?
                                                        VisitedNodes.Add(new Point(nx, yc));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MinCheckY = 0;
                                            MaxCheckY = 1;
                                        }
                                    }*/
                                    //else
                                    {
                                        for (int y = MinCheckY; y < MaxCheckY; y++)
                                        {
                                            int yc = ny - y;
                                            if (CheckForSolidGroundUnder(nx, yc, true) && !IsDangerousTile(nx, yc, false) && !CheckForStairFloor(nx, yc - 1) && !CheckForSolidBlocks(nx, yc, PassThroughDoors: true))
                                            {
                                                if (!VisitedNodes.Contains(new Point(nx, yc)))
                                                {
                                                    NextNodeList.Add(CreateNextNode(nx, yc, dir, n));
                                                    VisitedNodes.Add(new Point(nx, yc));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    HangPreventer++;
                    if (HangPreventer >= MaxTileCheck || VisitedNodes.Count > 400)
                    {
                        NextNodeList.Clear();
                        VisitedNodes.Clear();
                        return new List<Breadcrumb>();
                    }
                }
            }
            List<Breadcrumb> PathGuide = new List<Breadcrumb>();
            byte LastDirection = Node.NONE;
            //if (DestinationFound == null)
            //    Main.NewText("Didn't found destination.");
            while(DestinationFound != null)
            {
                if (DestinationFound.NodeDirection != LastDirection)
                {
                    Breadcrumb guide = new Breadcrumb(){ X = DestinationFound.NodeX, Y = DestinationFound.NodeY, NodeOrientation = DestinationFound.NodeDirection };
                    PathGuide.Insert(0, guide);
                    LastDirection = DestinationFound.NodeDirection;
                }
                DestinationFound = DestinationFound.LastNode;
            }
            return PathGuide;
        }

        static Node CreateNextNode(int X, int Y, byte Dir, Node LastNode)
        {
            Node node = new Node(X, Y, Dir);
            /*if (LastNode.LastNode != null && LastNode.NodeDirection == LastNode.LastNode.NodeDirection)
            {
                node.LastNode = LastNode.LastNode;
            }
            else
            {*/
                node.LastNode = LastNode;
            //}
            return node;
        }

        private static void CreateDebugDust(Point p)
        {
            CreateDebugDust(p.X, p.Y);
        }

        private static void CreateDebugDust(int X, int Y)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(new Vector2(X * 16, Y * 16), 16, 16, 5);
            }
        }

        public static bool CheckForSolidGroundUnder(int px, int py, bool PassThroughDoors = false)
        {
            for (sbyte x = -1; x < 1; x++)
            {
                byte State = 0;
                for (int y = 0; y < 2; y++)
                {
                    if (!WorldGen.InWorld(px + x, py + y)) return false;
                    Tile tile = Main.tile[px + x, py + y];
                    if (y == 0 && (!tile.HasTile || tile.IsActuated || (!PassThroughDoors || (tile.TileType != TileID.ClosedDoor && tile.TileType != TileID.TallGateClosed)) || !Main.tileSolid[tile.TileType]))
                        State++;
                    if (y == 1 && tile.HasTile && !tile.IsActuated && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
                        State++;
                }
                if (State >= 2) return true;
            }
            return false;
        }

        public static bool CheckForStairFloor(int px, int py)
        {
            for (int x = -1; x < 1; x++)
            {
                Tile tile = Main.tile[px + x, py];
                SlopeType Slope = tile.Slope;
                if (tile != null && tile.HasTile && TileID.Sets.Platforms[tile.TileType] && ((int)Slope == 1 || (int)Slope == 2))
                    return true;
            }
            return false;
        }

        public static bool IsDangerousTile(int tx, int ty, bool FireRes)
        {
            Tile t = Main.tile[tx, ty];
            if (t != null)
            {
                if (t.HasTile && !t.IsActuated)
                {
                    if ( (t.TileType != TileID.Cactus || Main.dontStarveWorld) && (TileID.Sets.TouchDamageBleeding[t.TileType] || 
                        (!FireRes && TileID.Sets.TouchDamageHot[t.TileType]) || 
                        TileID.Sets.TouchDamageImmediate[t.TileType] > 0))
                        {
                            return true;
                        }
                }
                else
                {
                    if (t.LiquidType == LiquidID.Lava && t.LiquidAmount > 0)
                        return true;
                }
            }
            return false;
        }

        public static bool CheckForSolidBlocks(int tx, int ty, int Height = 3, bool PassThroughDoors = false)
        {
            for (int x = -1; x < 1; x++)
            {
                for (int y = -(Height - 1); y <= 0; y++)
                {
                    Tile t = Main.tile[tx + x, ty + y];
                    if (t != null && t.HasTile && !t.IsActuated && Main.tileSolid[t.TileType] && !TileID.Sets.Platforms[t.TileType] && (!PassThroughDoors || (t.TileType != TileID.ClosedDoor && t.TileType != TileID.TallGateClosed)))
                        return true;
                }
            }
            return false;
        }

        public static bool CheckForSolidBlocksCeiling(int tx, int ty, int Height = 3)
        {
            return CheckForSolidBlocks(tx, ty - Height, 1);
        }

        public static bool CheckForPlatform(Vector2 Position)
        {
            return CheckForPlatform((int)(Position.X * Companion.DivisionBy16), (int)(Position.Y * Companion.DivisionBy16));
        }

        public static bool CheckForPlatform(Vector2 Position, int Width)
        {
            int xstart = (int)((Position.X - Width * 0.5f + .5f) * Companion.DivisionBy16), xend = (int)((Position.X + Width * 0.5f + .5f) * Companion.DivisionBy16);
            int ypos = (int)(Position.Y * Companion.DivisionBy16);
            for(int x = xstart; x <= xend; x++)
            {
                if (!CheckForPlatformAt(x, ypos))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckForPlatform(Vector2 Position, int Width, out sbyte MoveDir)
        {
            MoveDir = 0;
            int xstart = (int)((Position.X - Width * 0.5f - .5f) * Companion.DivisionBy16), xend = (int)((Position.X + Width * 0.5f + .5f) * Companion.DivisionBy16);
            int ypos = (int)(Position.Y * Companion.DivisionBy16);
            if (!CheckForPlatformAt(xstart, ypos))
            {
                MoveDir += 1;
            }
            if (!CheckForPlatformAt(xend, ypos))
            {
                MoveDir -= 1;
            }
            return MoveDir == 0;
        }

        public static bool CheckForPlatformAt(int tx, int ty)
        {
            Tile t = Main.tile[tx, ty];
            if (t != null && t.HasTile)
            {
                if (TileID.Sets.Platforms[t.TileType])
                    return true;
                else if (Main.tileSolid[t.TileType])
                    return false;
            }
            return true;
        }

        public static bool CheckForPlatform(int tx, int ty)
        {
            bool s;
            return CheckForPlatform(tx, ty, out s);
        }

        public static bool CheckForPlatform(int tx, int ty, out bool Stair)
        {
            byte PlatformTiles = 0;
            Stair = false;
            for (int x = -1; x <= 0; x++)
            {
                Tile t = Main.tile[tx + x, ty];
                if (t != null && t.HasTile)
                {
                    if (TileID.Sets.Platforms[t.TileType] || (Main.tileSolidTop[t.TileType] && !Main.tileSolid[t.TileType]))
                    {
                        PlatformTiles++;
                        if (CheckForStairFloor(tx + x, ty))
                        {
                            Stair = true;
                        }
                    }
                    else if (Main.tileSolid[t.TileType])
                        return false;
                }
            }
            return PlatformTiles > 0;
        }

        public static bool AnyPlatform(int tx, int ty)
        {
            byte PlatformTiles = 0;
            for (int x = -1; x <= 0; x++)
            {
                Tile t = Main.tile[tx + x, ty];
                if (t != null && t.HasTile && !t.IsActuated && TileID.Sets.Platforms[t.TileType])
                    PlatformTiles++;
            }
            return PlatformTiles > 0;
        }
        
        public class Node
        {
            public byte NodeDirection = 0;
            public const byte DIR_UP = 0, DIR_RIGHT = 1, DIR_DOWN = 2, DIR_LEFT = 3, NONE = 255;
            public Node LastNode;
            public int NodeX = 0, NodeY = 0;

            public Node(int X, int Y, byte Dir = 0, Node LastNode = null)
            {
                NodeDirection = Dir;
                NodeX = X;
                NodeY = Y;
                this.LastNode = LastNode;
            }
        }

        public class Breadcrumb
        {
            public int X = 0, Y = 0;
            public byte NodeOrientation = 0;
        }

        public enum PathingState : byte
        {
            NotSet,
            TracingPath,
            ReachedDestination,
            PathingFailed
        }
    }
}