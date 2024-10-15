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
        int SavedJumpDistance = 6, SavedFallDistance = 6;
        Entity TargetEntity = null;
        public Entity GetTargetEntity => TargetEntity;
        
        public bool CreatePathTo(Vector2 StartPosition, Vector2 Destination, int JumpDistance = 6, int FallDistance = 6, bool WalkToPath = false, bool Strict = true, bool CancelOnFail = false)
        {
            return CreatePathTo(StartPosition, (int)(Destination.X * Companion.DivisionBy16),
                (int)(Destination.Y * Companion.DivisionBy16), JumpDistance, FallDistance, WalkToPath, Strict, CancelOnFail);
        }

        public bool CreatePathTo(Vector2 StartPosition, int EndPosX, int EndPosY, int JumpDistance = 6, int FallDistance = 6, bool WalkToPath = false, bool Strict = true, bool CancelOnFail = false)
        {
            if (!MainMod.UsePathfinding) return false;
            TargetEntity = null;
            bool PathingMade = SetupPathing(StartPosition, EndPosX, EndPosY, JumpDistance, FallDistance);
            this.WalkToPath = WalkToPath;
            StrictPathFinding = Strict;
            this.CancelOnFail = CancelOnFail;
            SavedJumpDistance = JumpDistance;
            SavedFallDistance = FallDistance;
            return PathingMade;
        }

        public bool CreatePathTo(Vector2 StartPosition, Entity Target, int JumpDistance = 6, int FallDistance = 6, bool WalkToPath = false, bool Strict = true, bool CancelOnFail = false)
        {
            if (!MainMod.UsePathfinding) return false;
            TargetEntity = Target;
            int EndPosX = (int)(Target.Center.X * Companion.DivisionBy16), EndPosY = (int)((Target.Bottom.Y - 2) * Companion.DivisionBy16);
            bool PathingMade = SetupPathing(StartPosition, EndPosX, EndPosY, JumpDistance, FallDistance);
            this.WalkToPath = WalkToPath;
            StrictPathFinding = Strict;
            this.CancelOnFail = CancelOnFail;
            SavedJumpDistance = JumpDistance;
            SavedFallDistance = FallDistance;
            return PathingMade;
        }

        bool SetupPathing(Vector2 StartPosition, int EndX, int EndY, int JumpDistance = 6, int FallDistance = 6)
        {
            if (!MainMod.UsePathfinding) return false;
            Path = DoPathFinding(StartPosition, EndX, EndY, JumpDistance, FallDistance);
            PathingInterrupted = false;
            SavedPosX = EndX;
            SavedPosY = EndY;
            State = Path.Count > 0 ? PathingState.TracingPath : PathingState.PathingFailed;
            StuckTimer = 0;
            return Path.Count > 0;
        }

        public bool ResumePathingTo(Vector2 StartPosition)
        {
            if ((TargetEntity == null && (SavedPosX == -1 || SavedPosY == -1)) || (TargetEntity != null && !TargetEntity.active) || State != PathingState.TracingPath) return false;
            if (TargetEntity != null)
                return CreatePathTo(StartPosition, TargetEntity, SavedJumpDistance, SavedFallDistance, WalkToPath);
            return CreatePathTo(StartPosition, SavedPosX, SavedPosY, SavedJumpDistance, SavedFallDistance, WalkToPath);
        }

        public void CancelPathing()
        {
            CancelPathing(true);
        }

        public void CancelPathing(bool CountAsInterrupted)
        {
            Path.Clear();
            State = PathingState.NotSet;
            PathingInterrupted = CountAsInterrupted;
            if (!CountAsInterrupted)
            {
                TargetEntity = null;
                SavedPosX = -1;
                SavedPosY = -1;
            }
        }

        public bool CheckIfNearTarget(Vector2 BottomPosition)
        {
            if (TargetEntity == null) return false;
            if (MathF.Abs(BottomPosition.X - TargetEntity.Bottom.X) < 40f && MathF.Abs(BottomPosition.Y - TargetEntity.Bottom.Y) < 8f)
            {
                CancelPathing(false);
                return true;
            }
            return false;
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
            IncreaseStuckTimer(1);
        }

        public void IncreaseStuckTimer(byte Count)
        {
            StuckTimer += Count;
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
                    for (byte dir = 0; dir < 5; dir++)
                    {
                        switch(dir)
                        {
                            case Node.DIR_JUMP: //A ping pong effect must be happening. Where it checks if can jump right, then checks if jump left repeatedly on each ledge.
                                {
                                    //Don't try to check where to jump if the companion is moving vertically.
                                    if (n.NodeDirection == Node.DIR_DOWN || n.NodeDirection == Node.DIR_UP || n.NodeDirection == Node.DIR_JUMP) continue;
                                    for (int d = -1; d <= 1; d += 2)
                                    {
                                        bool IsDrop = true; //Checks if there's a opening wide enough to fall. If there is, then check for jumping.
                                        for (int xcheck = 1; xcheck <= 2; xcheck++)
                                        {
                                            int tx = X + d * xcheck;
                                            for (int ycheck = -3; ycheck <= 2; ycheck++)
                                            {
                                                int ty = Y + ycheck;
                                                if (WorldGen.InWorld(tx, ty))
                                                {
                                                    Tile tile = Main.tile[tx, ty];
                                                    if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType])
                                                    {
                                                        IsDrop = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!IsDrop) continue;
                                        for (int ydist = 0; ydist < 6; ydist++)
                                        {
                                            for (int yor = -1; yor <= 1; yor += 2)
                                            {
                                                if (ydist == 0 && yor == 1) continue;
                                                int TileY = Y + ydist * yor;
                                                for (int xdist = 2; xdist <= 8; xdist++)
                                                {
                                                    int TileX = X + xdist * d;
                                                    if (!WorldGen.InWorld(TileX, TileY)) break;
                                                    Tile tile = Main.tile[TileX, TileY];
                                                    if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType])
                                                    {
                                                        if (WorldGen.InWorld(TileX, TileY - 1))
                                                        {
                                                            tile = Main.tile[TileX, TileY - 1];
                                                            if (tile != null && (!tile.HasTile || !Main.tileSolid[tile.TileType]) && !CheckForSolidBlocks(TileX, TileY - 1, PassThroughDoors: true))
                                                            {
                                                                if (WorldGen.InWorld(TileX - d, TileY))
                                                                {
                                                                    tile = Main.tile[TileX - d, TileY];
                                                                    if (tile != null && (!tile.HasTile || !Main.tileSolid[tile.TileType]))
                                                                    {
                                                                        int EndX = TileX - d * 2;
                                                                        bool MovingDown = yor == -1;
                                                                        /*bool Blocked = false;
                                                                        int ystart = yor == -1 ? TileY : Y;
                                                                        for (int x = X; x != EndX; X += d)
                                                                        {
                                                                            if (CheckForSolidBlocks(x, ystart))
                                                                            {
                                                                                Blocked = true;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (Blocked) break;
                                                                        int xstart = yor == -1 ? X : TileX;
                                                                        for (int y = Y; y != TileY - 1; y += yor)
                                                                        {
                                                                            if (CheckForSolidBlocks(xstart, ystart))
                                                                            {
                                                                                Blocked = true;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (Blocked) break;*/
                                                                        if (!VisitedNodes.Contains(new Point(TileX, TileY - 1)))
                                                                        {
                                                                            NextNodeList.Add(CreateNextNode(TileX, TileY - 1, Node.DIR_JUMP, n));
                                                                            VisitedNodes.Add(new Point(TileX, TileY - 1));
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case Node.DIR_UP:
                                {
                                    if (n.NodeDirection == Node.DIR_DOWN || (n.NodeDirection == Node.DIR_UP && !CheckForPlatformAt(n.NodeX - 1, n.NodeY + 1) && !CheckForPlatformAt(n.NodeX, n.NodeY + 1))) continue;
                                    bool HasPlatform = false;
                                    int PlatformNodeY = -1;
                                    for (int y = 0; y < JumpDistance; y++)
                                    {
                                        int YCheck = Y - y;
                                        if (CheckForSolidBlocks(X, YCheck)/* || (y >= 3 && CheckForSolidBlocksCeiling(X, YCheck))*/)
                                        {
                                            break;
                                        }
                                        if (y > 0)
                                        {
                                            for (sbyte x = -1; x < 2; x += 2)
                                            {
                                                if (!CheckForSolidBlocks(X + x, YCheck, PassThroughDoors: true) && CheckForSolidGroundUnder(X + x, YCheck, true, true) && !VisitedNodes.Contains(new Point(X, YCheck)))
                                                {
                                                    NextNodeList.Add(CreateNextNode(X, YCheck, Node.DIR_UP, n));
                                                    VisitedNodes.Add(new Point(X, YCheck));
                                                    break;
                                                }
                                            }
                                        }
                                        if (CheckForPlatform(X, YCheck) && !CheckForPlatform(X, YCheck + 1) && !CheckForSolidBlocks(X, YCheck - 1))
                                        {
                                            HasPlatform = true;
                                            PlatformNodeY = YCheck - 1;
                                            break;
                                        }
                                        //if (!CheckForSolidBlock(X, YCheck))
                                    }
                                    if (HasPlatform && !VisitedNodes.Contains(new Point(X, PlatformNodeY)) && !CheckForSolidBlocks(X, PlatformNodeY))
                                    {
                                        NextNodeList.Add(CreateNextNode(X, PlatformNodeY, Node.DIR_UP, n));
                                        VisitedNodes.Add(new Point(X, PlatformNodeY));
                                    }
                                    /*if (HasSolidBlock)
                                    {
                                        continue;
                                    }*/
                                }
                                break;

                            case Node.DIR_DOWN:
                                {
                                    if (n.NodeDirection == Node.DIR_UP || CheckForSolidGroundUnder(X, Y, false, true)) continue;
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
                                    else
                                    {
                                        for (int x = -1; x <= 2; x += 2)
                                        {
                                            int Xp = X + x;
                                            for (int y = 2; y <= FallDistance; y++)
                                            {
                                                int Yp = Y + y;
                                                if (CheckForSolidBlocks(Xp, Yp) || IsDangerousTile(Xp, Yp, false)) break;
                                                if (CheckForSolidGroundUnder(Xp, Yp) && !VisitedNodes.Contains(new Point(Xp, Yp)))
                                                {
                                                    NextNodeList.Add(CreateNextNode(Xp, Yp, Node.DIR_DOWN, n));
                                                    VisitedNodes.Add(new Point(Xp, Yp));
                                                    break;
                                                }
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
                                            //ny -= zy;
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
            while(DestinationFound != null)
            {
                const byte DoNothing = 0, Save = 1, Replace = 2;
                byte Action = DoNothing;
                switch (DestinationFound.NodeDirection)
                {
                    default:
                        if (DestinationFound.NodeDirection != LastDirection || MainMod.DebugPathFinding)
                            Action = Save;
                        break;
                }
                switch (Action)
                {
                    case Save:
                        {
                            Breadcrumb guide = new Breadcrumb(){ X = DestinationFound.NodeX, Y = DestinationFound.NodeY, NodeOrientation = DestinationFound.NodeDirection };
                            PathGuide.Insert(0, guide);
                            //LastDirection = DestinationFound.NodeDirection;
                        }
                        break;
                    case Replace:
                        {
                            Breadcrumb guide = new Breadcrumb(){ X = DestinationFound.NodeX, Y = DestinationFound.NodeY, NodeOrientation = DestinationFound.NodeDirection };
                            PathGuide[0] = guide;
                        }
                        break;
                }
                LastDirection = DestinationFound.NodeDirection;
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

        bool SlopedTileUnder(int TileX, int TileY, bool MovingRight = false)
        {
            bool Slope = false, NormalBlock = false;
            for (int x = -1; x < 1; x++)
            {
                if (WorldGen.InWorld(TileX + x, TileY))
                {
                    Tile tile = Main.tile[TileX + x, TileY];
                    if (tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType])
                    {
                        switch (tile.Slope)
                        {
                            case SlopeType.SlopeDownLeft:
                            case SlopeType.SlopeDownRight:
                                Slope = MovingRight && (tile.Slope == SlopeType.SlopeDownLeft);
                                break;
                            default:
                                if (tile.IsHalfBlock)
                                    Slope = true;
                                else
                                    NormalBlock = true;
                                break;
                        }
                    }
                }
            }
            return Slope && !NormalBlock;
        }

        public static bool CheckForSolidGroundUnder(int px, int py, bool PassThroughDoors = false, bool NotPlatforms = false)
        {
            for (sbyte x = -1; x < 1; x++)
            {
                byte State = 0;
                for (int y = 0; y < 2; y++)
                {
                    if (!WorldGen.InWorld(px + x, py + y)) return false;
                    Tile tile = Main.tile[px + x, py + y];
                    switch (y)
                    {
                        case 0:
                            if (!tile.HasTile || tile.IsActuated || (!PassThroughDoors || (tile.TileType != TileID.ClosedDoor && tile.TileType != TileID.TallGateClosed)) || !Main.tileSolid[tile.TileType])
                                State++;
                            break;
                        case 1:
                            if (tile.HasTile && !tile.IsActuated && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]) && (!NotPlatforms || !TileID.Sets.Platforms[tile.TileType]))
                                State++;
                            break;
                    }
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
            public const byte DIR_UP = 0, DIR_RIGHT = 1, DIR_DOWN = 2, DIR_LEFT = 3, DIR_JUMP = 4, NONE = 255;
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

            public override string ToString()
            {
                return "{"+X + ":"+ Y +" - "+NodeOrientation+"}";
            }
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