using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace terraguardians.Companions.Scaleforth.Actions;

public class ServeDinnerAction : BehaviorBase
{
    int Slot = -1;
    byte Step = 0;
    int Time = 0;
    const int FramesDuration = 30, MaxFrames = FramesDuration * 3;
    const float NormalizedFrameDuration = 1f / FramesDuration;
    static readonly string[] Messages = ["Your meal is ready.", "Here's your dinner."];
    static readonly string[] InvalidItemMessages = ["Wait. This is not edible.", "What is this? This isn't your dinner."];

    public ServeDinnerAction(int ItemSlot)
    {
        Slot = Math.Clamp(ItemSlot, 0, 50);
        Time = 0;
        RunCombatBehavior = false;
        AllowSeekingTargets = false;
    }

    public override void Update(Companion companion)
    {
        Player Target = companion.Owner;
        if (Target == null)
        {
            Deactivate();
            return;
        }
        bool IsFarFromPlayer = MathF.Abs(companion.Center.X - Target.Center.X) >= 80;
        if (IsFarFromPlayer)
            MoveTowards(companion, Target.Bottom);
        switch (Step)
        {
            case 0: //Approach Player
                {
                    if (!IsFarFromPlayer)
                    {
                        Step++;
                        companion.SaySomethingAtRandom(Messages);
                    }
                }
                break;
            case 1:
                {
                    Time++;
                    if (Time >= MaxFrames)
                    {
                        Step++;
                        Time = 0;
                        Item food = companion.inventory[Slot];
                        if (food.type > 0 && food.buffType >= 0 && BuffID.Sets.IsFedState[food.buffType])
                        {
                            Target.AddBuff(food.buffType, food.buffTime);
                            foreach (Companion c in PlayerMod.PlayerGetSummonedCompanions(Target))
                            {
                                c.AddBuff(food.buffType, food.buffTime);
                            }
                            food.stack--;
                            if (food.stack <= 0)
                            {
                                food.TurnToAir();
                            }
                        }
                        else
                        {
                            companion.SaySomethingAtRandom(InvalidItemMessages);
                        }
                    }
                }
                break;
            case 2:
                {
                    Time++;
                    if (Time >= 90)
                    {
                        Deactivate();
                    }
                }
                break;
        }
    }

    public override void UpdateAnimationFrame(Companion companion)
    {
        short Frame = 1;
        switch (Step)
        {
            case 1:
                {
                    Frame = (short)MathF.Min(25 + Time * NormalizedFrameDuration, 27);
                }
                break;
            case 2:
                {
                    Frame = 27;
                }
                break;
        }
        companion.ArmFramesID[0] = companion.ArmFramesID[1] = Frame;
    }

    public override void CompanionDrawLayerSetup(Companion companion, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
    {
        if (IsDrawingFrontLayer)
            return;
        Vector2 DrawPosition = Holder.GetCompanion.GetAnimationPosition(AnimationPositions.HandPosition, Holder.GetCompanion.ArmFramesID[1], 1/*, AlsoTakePosition: false, BottomCentered: true*/) - Main.screenPosition;
        Texture2D texture;
        Rectangle rect;
        Main.GetItemDrawFrame(companion.inventory[Slot].type, out texture, out rect);
        DrawPosition.Y -= (int)(rect.Height * .5f);
        DrawPosition.X -= (int)(rect.Width * .25f);
        DrawData dd = new DrawData(texture, DrawPosition, rect, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        DrawDatas.Add(dd);
    }
}