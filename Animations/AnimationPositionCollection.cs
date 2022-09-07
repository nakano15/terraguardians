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

        public AnimationPositionCollection(Vector2 DefaultCoordinate)
        {
            this.DefaultCoordinate = DefaultCoordinate;
        }

        public Vector2 GetPositionFromFrame(short Frame)
        {
            if(!SpecificCoordinates.ContainsKey(Frame))
                return DefaultCoordinate;
            return SpecificCoordinates[Frame].Position;
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
            if(!SpecificCoordinates.ContainsKey(FrameID)) SpecificCoordinates.Add(FrameID, new AnimationPosition(FrameX, FrameY));
            return this;
        }
    }
}