using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace terraguardians.Companions.Leona
{
    public class LeonaCompanion : TerraGuardian
    {
        public bool HoldingSword = true;
        public Vector2? SwordPosition = null;
        public float SwordRotation = 0;

        public override void ModifyAnimation()
        {
            if (sleeping.isSleeping && Owner != null)
            {
                Player p = Owner;
                if (p.sleeping.isSleeping && p.Bottom == Bottom)
                {
                    BodyFrameID = 
                    ArmFramesID[0] = 
                    ArmFramesID[1] = 22;
                }
            }
            else if (HoldingSword)
            {
                if (ArmFramesID[1] < 20 && (ArmFramesID[1] < 15 || ArmFramesID[1] >= 19))
                {
                    ArmFramesID[1] = 1;
                }
            }
            else if (ArmFramesID[1] == 27)
            {
                ArmFramesID[1] = 31;
            }
            SwordRotation = 0;
        }

        public override void PostUpdateAnimation()
        {
            SwordPosition = null;
            if (HoldingSword)
            {
                switch(ArmFramesID[1])
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 20:
                    case 27:
                        SwordPosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[1], 1);
                        break;
                    case 33:
                        SwordPosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[0], 0);
                        break;
                }
            }
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (SwordPosition.HasValue)
            {
                switch(Holder.tg.BodyFrameID)
                {
                    default:
                        if(!IsDrawingFrontLayer)
                        {
                            DrawData dd = GetSwordDrawData(drawSet, ref Holder, ref DrawDatas);
                            DrawDatas.Insert(0, dd);
                        }
                        break;
                    case 33:
                        if(IsDrawingFrontLayer)
                        {
                            DrawData dd = GetSwordDrawData(drawSet, ref Holder, ref DrawDatas);
                            DrawDatas.Insert((int)MathF.Max(0, DrawDatas.Count - 2), dd);
                        }
                        break;
                }
            }
        }

        private DrawData GetSwordDrawData(PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            SwordPosition = SwordPosition.Value - Main.screenPosition + Vector2.UnitY * gfxOffY;
            if (IsUsingAnyChair)
            {
                SwordPosition = SwordPosition.Value + sitting.offsetForSeat;
            }
            Vector2 Origin = new Vector2(drawSet.playerEffect == Microsoft.Xna.Framework.Graphics.SpriteEffects.None ? 69 : 11, 10);
            return new DrawData(Base.GetSpriteContainer.GetExtraTexture(LeonaBase.giantswordtextureid), SwordPosition.Value, null, Holder.DrawColor, drawSet.rotation + SwordRotation, Origin, Scale, drawSet.playerEffect, 0);
        }
    }
}