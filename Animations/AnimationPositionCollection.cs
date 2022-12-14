using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace terraguardians
{
    public class AnimationPositionCollection
    {
        public Vector2 DefaultCoordinate = Vector2.Zero;
        private Dictionary<short, AnimationPosition> SpecificCoordinates = new Dictionary<short, AnimationPosition>();

        public AnimationPositionCollection()
        {

        }

        public AnimationPositionCollection(Vector2 DefaultCoordinate, bool Position2x = false)
        {
            this.DefaultCoordinate = DefaultCoordinate * (Position2x ? 2 : 1);
        }

        public AnimationPositionCollection(float X, float Y, bool Position2x = false)
        {
            this.DefaultCoordinate.X = X * (Position2x ? 2 : 1);
            this.DefaultCoordinate.Y = Y * (Position2x ? 2 : 1);
        }

        public Vector2 GetPositionFromFrame(short Frame)
        {
            if(!SpecificCoordinates.ContainsKey(Frame))
                return DefaultCoordinate;
            return SpecificCoordinates[Frame].Position;
        }

        public bool HasCoordinatesFor(short Frame)
        {
            if(DefaultCoordinate.X != 0 || DefaultCoordinate.Y != 0)
                return true;
            return HasSpecificCoordinate(Frame);
        }

        public bool HasSpecificCoordinate(short Frame)
        {
            return SpecificCoordinates.ContainsKey(Frame);
        }

        public AnimationPositionCollection AddFramePoint2X(short FrameID, float FrameX, float FrameY)
        {
            return AddFramePoint(FrameID, FrameX * 2, FrameY * 2);
        }

        public AnimationPositionCollection AddFramePoint(short FrameID, float FrameX, float FrameY)
        {
            if(!SpecificCoordinates.ContainsKey(FrameID))
                SpecificCoordinates.Add(FrameID, new AnimationPosition(FrameX, FrameY));
            return this;
        }
    }
}