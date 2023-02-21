using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Brutus
{
    public class BrutusFollowerBehavior : FollowLeaderBehavior
    {
        float AfkCounter = 0;
        const float ProtectModeTriggerTime = 3600;
        public bool ProtectionModeActivated { get { return AfkCounter >= ProtectModeTriggerTime; } }

        public override void Update(Companion companion)
        {
            Entity Leader = companion.GetLeaderCharacter();
            if (Leader is Player)
            {
                Player p = Leader as Player;
                if(((p.sitting.isSitting && companion.sitting.isSitting) || (p.sleeping.isSleeping && companion.sleeping.isSleeping)) && companion.Bottom == p.Bottom)
                {
                    p.AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                }
                else if (!p.controlLeft && !p.controlRight && !p.controlJump && !p.controlDown && !p.controlUp)
                {
                    AfkCounter++;
                }
                else
                {
                    AfkCounter = 0;
                }
            }
            else
            {
                if(Leader.velocity.X == 0 && Leader.velocity.Y == 0) AfkCounter++;
                else AfkCounter = 0;
            }
            if (AfkCounter >= ProtectModeTriggerTime)
            {
                UpdateProtectMode(companion, Leader);
            }
            else
            {
                UpdateFollow(companion);
            }
        }

        public override void UpdateStatus(Companion companion)
        {
            if (ProtectionModeActivated)
            {
                companion.noKnockback = true;
                companion.DefenseRate += 0.05f;
            }
        }

        private void UpdateProtectMode(Companion companion, Entity Leader)
        {
            if (Math.Abs(companion.Center.X - Leader.Center.X) > 8)
            {
                if (companion.Center.X < Leader.Center.X)
                    companion.MoveRight = true;
                else
                    companion.MoveLeft = true;
            }
            else
            {
                companion.Bottom = Leader.Bottom - Vector2.UnitX * 4 * Leader.direction;
                companion.velocity.X = 0;
                companion.velocity.Y = 0;
                companion.SetFallStart();
                if(companion.itemAnimation == 0)
                {
                    companion.direction = Leader.direction;
                }
                companion.controlDown = true;
                if (Leader is Player)
                {
                    (Leader as Player).AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                }
                else if (Leader is NPC)
                {
                    (Leader as NPC).AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                }
            }
        }
    }
}