using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class BehaviorBase
    {
        #region Hooks
        public virtual void Update(Companion companion)
        {
            
        }

        public virtual string CompanionNameChange(Companion companion)
        {
            return companion.name;
        }

        public virtual MessageBase ChangeStartDialogue(Companion companion)
        {
            return null;
        }

        public virtual bool AllowStartingDialogue(Companion companion)
        {
            return true;
        }

        public virtual void ChangeLobbyDialogueOptions(MessageBase Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
        }

        public virtual void UpdateAnimationFrame(Companion companion)
        {

        }
        
        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        public virtual void UpdateStatus(Companion companion)
        {
            
        }
        #endregion

        public Player ViewRangeCheck(Companion companion, int Direction, int DistanceX = 300, int DistanceY = 150, bool SpotPlayers = true, bool SpotCompanions = false)
        {
            Player Nearest = null;
            float NearestDistance = float.MaxValue;
            Rectangle rect = new Rectangle((int)companion.Center.X, (int)companion.Center.Y, DistanceX, DistanceY);
            rect.Y -= (int)(rect.Height * 0.5f);
            if(Direction < 0) rect.X -= rect.Width;
            for(int p = 0; p < 255; p++)
            {
                if(Main.player[p].active && !Main.player[p].dead && Main.player[p] != companion)
                {
                    if(Main.player[p] is Companion)
                    {
                        if(!SpotCompanions || (Main.player[p] as Companion).Owner == null) continue;
                    }
                    else
                    {
                        if (!SpotPlayers) continue;
                    }
                    if(rect.Intersects(Main.player[p].getRect()))
                    {
                        float Distance = (Main.player[p].Center - companion.Center).Length();
                        if(Distance < NearestDistance)
                        {
                            Nearest = Main.player[p];
                            NearestDistance = Distance;
                        }
                    }
                }
            }
            return Nearest;
        }

        public bool ThereIsPlayerInNpcViewRange(Companion companion)
        {
            bool PlayerClose = false;
            for(int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && !(Main.player[i] is Companion))
                {
                    if (MathF.Abs(Main.player[i].Center.X - companion.Center.X) < NPC.sWidth * 0.5f + NPC.safeRangeX && 
                        MathF.Abs(Main.player[i].Center.Y - companion.Center.Y) < NPC.sHeight * 0.5f + NPC.safeRangeY)
                        {
                            PlayerClose = true;
                            break;
                        }
                }
            }
            return PlayerClose;
        }
    }
}