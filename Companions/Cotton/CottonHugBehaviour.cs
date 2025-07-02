using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Cotton;

public class CottonHugBehaviour : BehaviorBase
{
    public Player Target;
    byte Frame = 0;
    byte Time = 0;

    public CottonHugBehaviour(Player Target)
    {
        this.Target = Target;
        RunCombatBehavior = false;
        AllowSeekingTargets = false;
        AllowRevivingSomeone = false;
        CanAggroNpcs = false;
    }

    public override void Update(Companion companion)
    {
        Vector2 PlayerCenterPosition = companion.Bottom + new Vector2(6 * companion.direction, -26f) * companion.Scale * 2f;
        Target.Center = PlayerCenterPosition;
        Target.direction = -companion.direction;
        Target.fallStart = (int)(Target.position.Y * Companion.DivisionBy16);
        Target.velocity.Y = -Player.defaultGravity;
        Target.velocity.X = 0;
        Target.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
        DrawOrderInfo.AddDrawOrderInfo(companion, Target, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
        if (Target.controlLeft || Target.controlRight || Target.controlJump || Target.controlDown)
        {
            companion.SaySomething("*There. Feeling better now?*");
            Deactivate();
        }
    }

    public override void UpdateAnimationFrame(Companion companion)
    {
        const int FrameDuration = 12;
        Time++;
        if (Time >= FrameDuration)
        {
            Time -= FrameDuration;
            Frame++;
            if (Frame >= 4)
            {
                Frame -= 4;
            }
        }
        byte NewFrame;
        switch (Frame)
        {
            default:
                NewFrame = 27;
                break;
            case 0:
                NewFrame = 26;
                break;
            case 2:
                NewFrame = 28;
                break;
        }
        for (byte a = 0; a < companion.ArmFramesID.Length; a++)
        {
            companion.ArmFramesID[a] = NewFrame;
        }
        if (companion.velocity.X == 0 && companion.velocity.Y == 0)
        {
            companion.BodyFrameID = NewFrame;
        }
    }
}