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
using System.Linq;

namespace terraguardians.Companions.Castella
{
    public class CastellaCompanion : TerraGuardian
    {
        public bool LastWerewolf = false;

        public bool OnWerewolfForm
        {
            get
            {
                return (wereWolf && !Main.dayTime) || (Main.moonPhase == 0 && !Main.dayTime);
            }
        }

        public override void UpdateAttributes()
        {
            if (OnWerewolfForm)
            {
                maxRunSpeed += 1.2f;
                if (accRunSpeed == 0)
                    accRunSpeed = Base.MaxRunSpeed * 2;
                else
                    accRunSpeed += 2f;
                jumpSpeed += 0.6f;
                GetDamage<MeleeDamageClass>() += 0.08f;
                GetCritChance<MeleeDamageClass>() += 10;
                statDefense += 6;
                statLifeMax2 += 1200;
                noKnockback = true;
            }
        }

        public override void ModifyAnimation()
        {
            if (!LastWerewolf) return;
            GetWerewolfFrame(ref BodyFrameID);
            GetWerewolfFrame(ref ArmFramesID[0]);
            GetWerewolfFrame(ref ArmFramesID[1]);
        }

        private void GetWerewolfFrame(ref short Frame)
        {
            switch (Frame)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    Frame += 29;
                    return;
                case 17:
                case 18:
                    Frame += 26;
                    return;
                case 19:
                    Frame = 46;
                    return;
                case 20:
                    Frame = 45;
                    return;
                case 21:
                    Frame = 48;
                    return;
                case 22:
                    Frame = 47;
                    return;
                case 23:
                case 24:
                case 25:
                case 26:
                    Frame += 26;
                    return;
            }
        }

        public override void UpdateCompanionHook()
        {
            bool IsWerewolf = OnWerewolfForm;
            if (LastWerewolf != IsWerewolf)
            {

            }
            if (IsWerewolf && Main.moonPhase == 0 && !IsRunningBehavior && (Owner == null || FriendshipLevel < 3))
            {

            }
            LastWerewolf = IsWerewolf;
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (!IsDrawingFrontLayer)
            {
                Rectangle rect = GetAnimationFrame(BodyFrameID);
                Texture2D texture = Base.GetSpriteContainer.GetExtraTexture(CastellaBase.HairBackTextureID);
                DrawData dd = new DrawData(texture, Holder.DrawPosition, rect, Holder.DrawColor, drawSet.rotation, Holder.Origin, Holder.GetCompanion.Scale, drawSet.playerEffect, 0);
                DrawDatas.Insert(Math.Min(1, DrawDatas.Count), dd);
            }
        }
    }
}