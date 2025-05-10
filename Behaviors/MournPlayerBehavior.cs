using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class MournPlayerBehavior : BehaviorBase
    {
        protected Vector2 Position = Vector2.Zero;
        protected Player Target;
        protected const int MourningMaxTime = 60 * 30, MourningExtraTimePerFriendshipLevel = 30;
        protected int MourningTime = 0;
        bool FirstFrame = true;

        public BehaviorBase SetTarget(Player Target)
        {
            this.Target = Target;
            if (Target == null)
            {
                Deactivate();
                return this;
            }
            Position = Target.Bottom;
            return this;
        }

        public override void Update(Companion companion)
        {
            if (FirstFrame)
            {
                if (Target == null)
                {
                    Deactivate();
                    return;
                }
                FirstFrame = false;
                MourningTime = MourningMaxTime + companion.FriendshipLevel * MourningExtraTimePerFriendshipLevel;
                if (companion.FriendshipLevel >= 10)
                {
                    companion.SpawnEmote(Terraria.GameContent.UI.EmoteID.EmotionCry, 5 * 60);
                }
                else if (companion.FriendshipLevel >= 3)
                {
                    companion.SpawnEmote(Terraria.GameContent.UI.EmoteID.EmoteSadness, 5 * 60);
                }
            }
            UpdateMourning(companion);
        }

        public virtual void UpdateMourning(Companion companion)
        {
            if (companion.TargettingSomething) return;
            if (MourningTime > 0)
            {
                const float MaxDistanceFromBody = 128f;
                const float MinDistanceFromBody = 48f;
                float Distance = Position.X - companion.Center.X;
                if (MathF.Abs(Distance) > MaxDistanceFromBody)
                {
                    companion.WalkMode = true;
                    if (Distance > 0)
                    {
                        companion.MoveRight = true;
                    }
                    else
                    {
                        companion.MoveLeft = true;
                    }
                }
                else if (Math.Abs(Distance) < MinDistanceFromBody)
                {
                    companion.WalkMode = true;
                    if (Distance < 0)
                    {
                        companion.MoveRight = true;
                    }
                    else
                    {
                        companion.MoveLeft = true;
                    }
                }
                if (companion.MoveRight && companion.direction == 1 && (IsDangerousAhead(companion, 3) || CheckForHoles(companion, ExtraCheckRangeX: 1)))
                {
                    companion.MoveRight = false;
                }
                if (companion.MoveLeft && companion.direction == -1 && (IsDangerousAhead(companion, 3) || CheckForHoles(companion, ExtraCheckRangeX: 1)))
                {
                    companion.MoveLeft = false;
                }
                if (companion.velocity.X == 0 && !companion.MoveLeft && !companion.MoveRight)
                {
                    MourningTime--;
                    companion.MoveDown = true;
                    companion.FaceSomething(Position);
                    if (MourningTime <= 0)
                    {
                        Deactivate();
                    }
                }
            }
        }
    }
}