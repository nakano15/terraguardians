using Terraria;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.Map;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class CompanionHeadsMapLayer : ModMapLayer
    {
        private static PlayerHeadDrawRenderTargetContent playerHead = new PlayerHeadDrawRenderTargetContent();

        public override string Name => "TerraGuardians : Companion Heads Map Layer";

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
			if (Main.gameMenu)
			{
				DrawMenuMode(ref context, ref text);
				return;
			}
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (!WorldMod.HasMetCompanion(c.Data) || c.IsHostileTo(MainMod.GetLocalPlayer) || c.invis || !c.GetGoverningBehavior().IsVisible)
                    continue;
                if (c is TerraGuardian)
                {
                    Texture2D head = c.Base.GetSpriteContainer.HeadTexture;
                    if (head != null && context.Draw(head, (c.Bottom - Vector2.UnitY * c.SpriteHeight) * (1f / 16), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 1f, Terraria.UI.Alignment.Center).IsMouseOver)
                    {
                        text = c.GetName;
                    }
                }
                /*else
                {
                    playerHead = new PlayerHeadDrawRenderTargetContent();
                    playerHead.UsePlayer(c);
                    playerHead.UseColor(default(Color));
                    playerHead.Request();
                    if (playerHead.IsReady)
                    {
                        RenderTarget2D target = playerHead.GetTarget();
                        if (context.Draw(target, c.Top * (1f / 16), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 2f, Terraria.UI.Alignment.Center).IsMouseOver)
                        {
                            text = c.name;
                        }
                    }
                }*/
            }
        }
		
		void DrawMenuMode(ref MapOverlayDrawContext context, ref string text)
		{
			Companion[] ToDraw = MainMod.ActiveCompanions.Values.ToArray();
            foreach(Companion c in ToDraw)
            {
                if (!WorldMod.HasMetCompanion(c.Data) || c.IsHostileTo(MainMod.GetLocalPlayer) || c.invis || !c.GetGoverningBehavior().IsVisible)
                    continue;
                if (c is TerraGuardian)
                {
                    Texture2D head = c.Base.GetSpriteContainer.HeadTexture;
                    if (head != null && context.Draw(head, (c.Bottom - Vector2.UnitY * c.SpriteHeight) * (1f / 16), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 1f, Terraria.UI.Alignment.Center).IsMouseOver)
                    {
                        text = c.GetName;
                    }
                }
                /*else
                {
                    playerHead = new PlayerHeadDrawRenderTargetContent();
                    playerHead.UsePlayer(c);
                    playerHead.UseColor(default(Color));
                    playerHead.Request();
                    if (playerHead.IsReady)
                    {
                        RenderTarget2D target = playerHead.GetTarget();
                        if (context.Draw(target, c.Top * (1f / 16), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 2f, Terraria.UI.Alignment.Center).IsMouseOver)
                        {
                            text = c.name;
                        }
                    }
                }*/
            }
		}

        internal static void OnUnload()
        {
            playerHead = null;
        }
    }
}