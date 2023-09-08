using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace terraguardians.NPCs.Hostiles
{
    public class HeadSnatcher : ModNPC
    {
        new float AIType { get { return NPC.ai[0]; } set { NPC.ai[0] = value; } }
        float AIValue { get { return NPC.ai[1]; } set { NPC.ai[1] = value; } }
        Player AttachedOn = null;
        byte HitTimes = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 44;
            NPC.damage = 30;
            NPC.lifeMax = 300;
            NPC.aiStyle = -1;
            NPC.townNPC = false;
            NPC.frame.Width = 44;
            NPC.frame.Height = 52;
        }

        public override void AI()
        {
            if (AttachedOn != null)
            {
                NPC.Center = AttachedOn.Center - new Vector2(-2 * AttachedOn.direction, 18);
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
                NPC.direction = AttachedOn.direction;
                AttachedOn.AddBuff(BuffID.Suffocation, 5);
            }
            switch(AIType)
            {
                case 0 :

                    break;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (AttachedOn == target) return false;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (AttachedOn == null)
            {
                AttachedOn = target;
                HitTimes = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter ++;
            const int FrameDuration = 8;
            const int MaxAnimationDuration = FrameDuration * 4;
            const float DivisionOfFrameDuration = 1f / FrameDuration;
            int FrameIndex = (int)(NPC.frameCounter * DivisionOfFrameDuration);
            switch(FrameIndex)
            {
                default:
                    NPC.frame.Y = 52 + 4;
                    break;
                case 0:
                    NPC.frame.Y = 0;
                    break;
                case 2:
                    NPC.frame.Y = 104 + 8;
                    break;
            }
            if (NPC.frameCounter >= MaxAnimationDuration)
            {
                NPC.frameCounter -= MaxAnimationDuration;
            }
            NPC.spriteDirection = -NPC.direction;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AttachedOn != null && !Main.instance.DrawCacheNPCsOverPlayers.Contains(NPC.whoAmI))
            {
                Main.instance.DrawCacheNPCsOverPlayers.Add(NPC.whoAmI);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}