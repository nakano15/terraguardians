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
        public bool HoldingSword 
        {
            get
            {
                return (Data as LeonaData).HoldingSword;
            }
            set
            {
                (Data as LeonaData).HoldingSword = value;
            }
        }
        public Vector2? SwordPosition = null;
        public float SwordRotation = 0;

        public override void ModifyAnimation()
        {
            if (sleeping.isSleeping && Owner != null)
            {
                if (Owner.sleeping.isSleeping && Owner.Bottom == Bottom)
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
            if (Main.gamePaused)
                UpdateSwordPosition();
        }

        public override void PostUpdateAnimation()
        {
            UpdateSwordPosition();
        }

        private void UpdateSwordPosition()
        {
            SwordPosition = null;
            if (KnockoutStates > 0)
                return;
            if (HoldingSword || SubAttackInUse < 255)
            {
                switch(ArmFramesID[1])
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 20:
                    case 24:
                    case 27:
                        SwordPosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[1], 1) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, 1, false, false, false, false, false);
                        break;
                    case 26:
                    case 33:
                        SwordPosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[0], 0) + GetAnimationPosition(AnimationPositions.ArmPositionOffset, BodyFrameID, 0, false, false, false, false, false);
                        break;
                }
            }
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (SwordPosition.HasValue)
            {
                switch(Holder.GetCompanion.BodyFrameID)
                {
                    default:
                        if(!IsDrawingFrontLayer)
                        {
                            DrawData dd = GetSwordDrawData(drawSet, ref Holder, ref DrawDatas);
                            int Index = 0;
                            Texture2D TextureToSeek = Holder.GetCompanion.BodyFrameID == 20 ? Holder.BodyTexture : Holder.ArmTexture[0];
                            for(int i = 0; i < DrawDatas.Count; i ++)
                            {
                                if (DrawDatas[i].texture == TextureToSeek)
                                {
                                    Index = i;
                                    break;
                                }
                            }
                            DrawDatas.Insert(Index, dd);
                        }
                        break;
                    case 33:
                        if(IsDrawingFrontLayer)
                        {
                            DrawData dd = GetSwordDrawData(drawSet, ref Holder, ref DrawDatas);
                            int Index = (int)MathF.Max(0, DrawDatas.Count - 2);
                            for(int i = 0; i < DrawDatas.Count; i ++)
                            {
                                if (DrawDatas[i].texture == Holder.ArmTexture[0])
                                {
                                    Index = i;
                                    break;
                                }
                            }
                            DrawDatas.Insert(Index, dd);
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
            return new DrawData(Base.GetSpriteContainer.GetExtraTexture(LeonaBase.giantswordtextureid), SwordPosition.Value, null, Holder.DrawColor, 0f + SwordRotation, Origin, Scale, drawSet.playerEffect, 0);
        }
    }
}