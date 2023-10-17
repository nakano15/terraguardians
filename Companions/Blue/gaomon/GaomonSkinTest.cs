using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class GaomonSkinTest : CompanionSkinInfo //Need to relocate this to Gaomon Mod
    {
        public override string Name => "Gaomon Skin";
        public override string Description => "It's just one frame.";

        protected override void OnLoad()
        {
            AddTexture("body", "terraguardians/Companions/Blue/gaomon/body_g");
            AddTexture("left_arm", "terraguardians/Companions/Blue/gaomon/left_arm_g");
            AddTexture("right_arm", "terraguardians/Companions/Blue/gaomon/right_arm_g");
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            CompanionSpritesContainer spr = c.Base.GetSpriteContainer;
            for(int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == spr.BodyTexture)
                {
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(GetTexture("body"), ref dd);
                    DrawDatas[i] = dd;
                }
                else if (DrawDatas[i].texture == spr.ArmSpritesTexture[0])
                {
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(GetTexture("left_arm"), ref dd);
                    DrawDatas[i] = dd;
                }
                else if (DrawDatas[i].texture == spr.ArmSpritesTexture[1])
                {
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(GetTexture("right_arm"), ref dd);
                    DrawDatas[i] = dd;
                }
            }
        }

        void ReplaceTexture(Texture2D NewTexture, ref DrawData dd)
        {
            DrawData ndd = new DrawData(NewTexture, dd.position, dd.sourceRect, dd.color, dd.rotation, dd.origin, dd.scale.Y, dd.effect, 0);
            dd = ndd;
        }
    }
}