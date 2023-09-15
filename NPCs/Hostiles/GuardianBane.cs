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
    public class GuardianBane : ModNPC
    {
        new float AIType { get { return NPC.ai[0]; } set { NPC.ai[0] = value; } }
        float AIValue { get { return NPC.ai[1]; } set { NPC.ai[1] = value; } }
        Player Victim = null;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 84;
            NPC.damage = 30;
            NPC.lifeMax = 400;
            NPC.aiStyle = -1;
            NPC.townNPC = false;
            NPC.frame.Width = 96;
            NPC.frame.Height = 96;
        }

        public override void AI()
        {
            switch(AIType)
            {
                case 0: //Will need to study npcs AI to make this guy scripts work.
                    
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            const int FrameDuration = 12;
            const int MaxAnimationDuration = FrameDuration * 4;
            const float DivisionOfFrameDuration = 1f / FrameDuration;
            int FrameIndex = (int)(NPC.frameCounter * DivisionOfFrameDuration);
            switch(FrameIndex)
            {
                default:
                    NPC.frame.Y = 100; //96 + 4;
                    break;
                case 0:
                    NPC.frame.Y = 0;
                    break;
                case 2:
                    NPC.frame.Y = 200; //192 + 4;
                    break;
            }
            if (NPC.frameCounter >= MaxAnimationDuration)
            {
                NPC.frameCounter -= MaxAnimationDuration;
            }
        }
    }
}