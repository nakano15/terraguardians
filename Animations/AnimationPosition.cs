using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace terraguardians
{
    public class AnimationPosition
    {
        public Vector2 Position = Vector2.Zero;
        
        public AnimationPosition(float PositionX, float PositionY)
        {
            Position.X = PositionX;
            Position.Y = PositionY;
        }
        
        public AnimationPosition(Vector2 Position)
        {
            this.Position = Position;
        }
    }
}