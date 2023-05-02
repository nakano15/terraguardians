using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class ReviveBehavior : BehaviorBase
    {
        Player CurrentTarget = null;
        byte Delay = 0;
        bool RevivingSomeone = false;
        float AnimationFrame = 0;

        public ReviveBehavior()
        {
            Delay = (byte)Main.rand.Next(15);
        }

        public void ClearReviveTarget()
        {
            CurrentTarget = null;
        }

        public override void Update(Companion companion)
        {
            TryFindingCharacterToRevive(companion);
            UpdateReviveBehavior(companion);
        }

        public void TryFindingCharacterToRevive(Companion companion)
        {
            bool Force = companion.IsMountedOnSomething && companion.controlDown && companion.releaseDown;
            if (!Force)
            {
                if (Delay > 0)
                {
                    Delay--;
                    return;
                }
                Delay += 15;
                if (CurrentTarget != null || companion.IsMountedOnSomething)
                {
                    return;
                }
            }
            if(!Companion.Behaviour_AttackingSomething)
            {
                Vector2 MyPosition = companion.Bottom;
                Player ToRevive = null;
                float NearestDistance = 350;
                for(int p = 0; p < 255; p++)
                {
                    Player player = Main.player[p];
                    if (player.active && PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake && !player.dead)
                    {
                        float Distance = (player.Bottom - MyPosition).Length() - ((1f - (float)player.statLife / player.statLifeMax2) * 50);
                        if (Distance < NearestDistance)
                        {
                            ToRevive = player;
                            NearestDistance = Distance;
                        }
                    }
                }
                foreach(Companion c in MainMod.ActiveCompanions.Values)
                {
                    if (c.KnockoutStates > KnockoutStates.Awake && !c.dead)
                    {
                        float Distance = (c.Bottom - MyPosition).Length() - ((1f - (float)c.Health / c.MaxHealth) * 50);
                        if (Distance < NearestDistance)
                        {
                            ToRevive = c;
                            NearestDistance = Distance;
                        }
                    }
                }
                if (ToRevive != null)
                {
                    CurrentTarget = ToRevive;
                }
            }
        }

        public void UpdateReviveBehavior(Companion companion)
        {
            RevivingSomeone = false;
            if (CurrentTarget == null || Companion.Behaviour_AttackingSomething) return;
            PlayerMod pm = CurrentTarget.GetModPlayer<PlayerMod>();
            if (pm.KnockoutState == KnockoutStates.Awake || CurrentTarget.dead)
            {
                CurrentTarget = null;
                return;
            }
            if (companion.UsingFurniture)
                companion.LeaveFurniture();
            companion.WalkMode = false;
            Companion.Behavior_RevivingSomeone = true;
            Vector2 RevivePosition = CurrentTarget.Bottom;
            if (!(CurrentTarget is TerraGuardian))
            {
                RevivePosition.X += CurrentTarget.width * 0.5f * CurrentTarget.direction;
            }
            float DistanceX = RevivePosition.X - companion.Center.X;
            if (Math.Abs(DistanceX) > 8 + CurrentTarget.width * 0.5f)
            {
                if (DistanceX > 0)
                {
                    companion.MoveRight = true;
                }
                else
                {
                    companion.MoveLeft = true;
                }
            }
            else
            {
                if (Math.Abs(companion.velocity.X) > companion.runAcceleration * 2)
                {
                    if(companion.velocity.X > 0)
                        companion.MoveLeft = true;
                    else
                        companion.MoveRight = true;
                }
                else if (companion.velocity.Y == 0)
                {
                    companion.MoveDown = true;
                    pm.ChangeReviveStack(1);
                    if(companion.Center.X < RevivePosition.X)
                        companion.direction = 1;
                    else
                        companion.direction = -1;
                    RevivingSomeone = true;
                }
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (RevivingSomeone)
            {
                bool Backward = false;
                short Frame = 0;
                if (Backward)
                {
                    Frame = companion.Base.GetAnimation(AnimationTypes.BackwardsRevivingFrames).UpdateTimeAndGetFrame(1, ref AnimationFrame);
                }
                else
                {
                    Frame = companion.Base.GetAnimation(AnimationTypes.RevivingFrames).UpdateTimeAndGetFrame(1, ref AnimationFrame);
                }
                companion.BodyFrameID = Frame;
                for(int i = 0; i < companion.ArmFramesID.Length; i++)
                {
                    companion.ArmFramesID[i] = Frame;
                }
            }
            else
            {
                AnimationFrame = 0;
            }
        }
    }
}