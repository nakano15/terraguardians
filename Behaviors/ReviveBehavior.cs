using Terraria;
using Terraria.ID;
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
        byte PathFindingDelay = 0;
        ushort ResTeleportTime = 0;

        public ReviveBehavior()
        {
            Delay = (byte)Main.rand.Next(15);
        }

        public void ClearReviveTarget()
        {
            CurrentTarget = null;
        }

        public virtual bool TryingToReviveSomeone => CurrentTarget != null;

        public override void Update(Companion companion)
        {
            TryFindingCharacterToRevive(companion);
            UpdateReviveBehavior(companion);
        }

        public void TryFindingCharacterToRevive(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake) return;
            bool Force = (Companion.Is2PCompanion || (companion.IsMountedOnSomething && !companion.CompanionHasControl)) && companion.controlDown && companion.releaseDown;
            if (!Force)
            {
                if (Delay > 0)
                {
                    Delay--;
                    return;
                }
                Delay += 15;
                if ((CurrentTarget != null && !companion.Data.PrioritizeHelpingAlliesOverFighting) || companion.IsMountedOnSomething)
                {
                    return;
                }
            }
            if(!Companion.Behaviour_AttackingSomething || companion.Data.PrioritizeHelpingAlliesOverFighting)
            {
                Vector2 MyPosition = companion.Bottom;
                Player ToRevive = null;
                float NearestDistance = 350;
                for(int p = 0; p < 255; p++)
                {
                    Player player = Main.player[p];
                    if (player.active && PlayerMod.IsPlayerCharacter(player) && PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake && !player.dead && (!player.lavaWet || companion.lavaImmune) && PlayerMod.PlayerGetMountedOnCompanion(player) == null)
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
                    if (c != companion && c.KnockoutStates > KnockoutStates.Awake && !c.dead && (!c.lavaWet || companion.lavaImmune) && c.GetPlayerMod.CanBeHelpedToRevive && PlayerMod.PlayerGetMountedOnCompanion(c) == null)
                    {
                        bool CanTryRevive = true;
                        foreach (Point p in c.TouchedTiles)
                        {
                            Tile t = Main.tile[p.X, p.Y];
                            if (t != null && t.HasTile && !t.IsActuated)
                            {
                                if (TileID.Sets.TouchDamageImmediate[t.TileType] > 0 || 
                                    TileID.Sets.TouchDamageHot[t.TileType] && !companion.fireWalk)
                                {
                                    CanTryRevive = false;
                                    break;
                                }
                            }
                        }
                        if (CanTryRevive)
                        {
                            float Distance = (c.Bottom - MyPosition).Length() - ((1f - (float)c.Health / c.MaxHealth) * 50);
                            if (Distance < NearestDistance)
                            {
                                ToRevive = c;
                                NearestDistance = Distance;
                            }
                        }
                    }
                }
                if (ToRevive != null)
                {
                    CurrentTarget = ToRevive;
                    ResTeleportTime = 0;
                }
            }
        }

        public void UpdateReviveBehavior(Companion companion)
        {
            RevivingSomeone = false;
            if (companion.KnockoutStates > KnockoutStates.Awake || Companion.Behavior_FollowingPath || companion.itemAnimation > 0) return;
            if (CurrentTarget == null || (!companion.Data.PrioritizeHelpingAlliesOverFighting && Companion.Behaviour_AttackingSomething)) return;
            if (Companion.Behaviour_AttackingSomething && Math.Abs(companion.Target.Center.X - companion.Center.X) < 96 + (companion.Target.width + companion.width) * .5f && 
                Math.Abs(companion.Target.Center.Y - companion.Center.Y) < 96 + (companion.Target.height + companion.height) * .5f)
            {
                return;
            }
            PlayerMod pm = CurrentTarget.GetModPlayer<PlayerMod>();
            if (pm.KnockoutState == KnockoutStates.Awake || CurrentTarget.dead || CurrentTarget.lavaWet && !companion.lavaImmune)
            {
                CurrentTarget = null;
                return;
            }
            if (companion.UsingFurniture)
                companion.LeaveFurniture();
            bool MountedVersion = companion.IsMountedOnSomething && !companion.CompanionHasControl;
            companion.WalkMode = false;
            Companion.Behavior_RevivingSomeone = true;
            Vector2 RevivePosition = CurrentTarget.Bottom;
            if (!(CurrentTarget is TerraGuardian))
            {
                RevivePosition.X += CurrentTarget.width * 0.5f * CurrentTarget.direction;
            }
            float DistanceX = RevivePosition.X - companion.Center.X;
            bool TargetIsMountedOnMe = companion.GetCharacterMountedOnMe == CurrentTarget;
            if (!TargetIsMountedOnMe && (Math.Abs(DistanceX) > 8 + CurrentTarget.width * 0.5f || Math.Abs(RevivePosition.Y - companion.Bottom.Y) > 20 + CurrentTarget.height * 0.5f))
            {
                if (!Companion.Is2PCompanion && !MountedVersion)
                {
                    if (PathFindingDelay == 0)
                    {
                        companion.CreatePathingTo(CurrentTarget.Bottom);
                        PathFindingDelay = 30;
                        return;
                    }
                    if (DistanceX > 0)
                    {
                        companion.MoveRight = true;
                    }
                    else
                    {
                        companion.MoveLeft = true;
                    }
                    ResTeleportTime++;
                    if (ResTeleportTime == 300)
                    {
                        companion.Teleport(RevivePosition);
                        ResTeleportTime++;
                    }
                }
            }
            else
            {
                if (!TargetIsMountedOnMe && Math.Abs(companion.velocity.X) > companion.runAcceleration * 2)
                {
                    if (!Companion.Is2PCompanion && !MountedVersion)
                    {
                        if(companion.velocity.X > 0)
                            companion.MoveLeft = true;
                        else
                            companion.MoveRight = true;
                    }
                }
                else if (companion.velocity.Y == 0)
                {
                    if (!TargetIsMountedOnMe && !MountedVersion)
                        companion.MoveDown = true;
                    else if (!companion.MoveDown) return;
                    pm.ChangeReviveStack(1);
                    float ReviveTargetPosition = RevivePosition.X - CurrentTarget.direction * 20;
                    if(companion.Center.X < ReviveTargetPosition)
                        companion.direction = 1;
                    else
                        companion.direction = -1;
                    RevivingSomeone = true;
                }
            }
            if (PathFindingDelay > 0) PathFindingDelay--;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake) return;
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