using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Companions.Fluffles;

public class FlufflesMourningBehavior : MournPlayerBehavior
{
    public override void UpdateMourning(Companion companion)
    {
        if (Target.ghost)
        {
            Vector2 Distance = Target.Center - companion.Center;
            companion.IgnoreCollision = true;
            companion.WalkMode = false;
            if (MathF.Abs(Distance.X) > 128)
            {
                if (Distance.X > 0) companion.MoveRight = true;
                else companion.MoveLeft = true;
            }
            if (MathF.Abs(Distance.Y) > 128)
            {
                if (Distance.Y > 0) companion.MoveDown = true;
                else companion.MoveUp = true;
            }
            companion.velocity.Y -= companion.Base.Gravity;
            if (companion.MoveUp)
            {
                if (companion.velocity.Y > 0f)
                {
                    companion.velocity.Y *= .95f;
                }
                companion.velocity.Y -= .3f;
                if (companion.velocity.Y < -9f)
                    companion.velocity.Y = -9f;
            }
            else if (companion.MoveDown)
            {
                if (companion.velocity.Y < 0f)
                {
                    companion.velocity.Y *= .95f;
                }
                companion.velocity.Y += .3f;
                if (companion.velocity.Y > 9f)
                    companion.velocity.Y = 9f;
            }
            else
            {
                if (MathF.Abs(companion.velocity.Y) > .1f)
                {
                    companion.velocity.Y *= .95f;
                }
                else
                {
                    companion.velocity.Y = 0f;
                }
            }
        }
        else
        {
            MourningTime++;
            base.UpdateMourning(companion);
        }
    }
}