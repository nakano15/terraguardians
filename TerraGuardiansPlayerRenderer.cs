using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Renderers;
using System.Collections.Generic;

namespace terraguardians
{
    public class TerraGuardiansPlayerRenderer : IPlayerRenderer
    {
        private static DrawContext _drawRule = DrawContext.AllParts;
        public static DrawContext GetDrawRule { get { return _drawRule; } }

        private LegacyPlayerRenderer pr = new LegacyPlayerRenderer();
        public void DrawPlayers(Camera camera, IEnumerable<Player> players)
        {
            SortedList<int, DrawOrderSetting> CharactersDrawOrder = new SortedList<int, DrawOrderSetting>();
            int CurrentIndex = 0;
            foreach(Player player in players)
            {
                int MyDrawIndex = 100000 + 200000 * CurrentIndex;
                CharactersDrawOrder.Add(MyDrawIndex, new DrawOrderSetting(player, DrawContext.BackLayer));
                PlayerMod pm = player.GetModPlayer<PlayerMod>();
                Companion[] Followers = pm.GetSummonedCompanions;
                int MountedFrontLayer = MyDrawIndex + 15000;
                int MountedBackLayer = MyDrawIndex - 15000;
                int InBetweenFront = MyDrawIndex + 501;
                int InBetweenBack = MyDrawIndex + 499;
                int FrontOfPlayerBody = MyDrawIndex + 10000;
                int BehindPlayerBody = MyDrawIndex - 10000;
                int DrawFront = MyDrawIndex + 20000;
                int DrawBack = MyDrawIndex - 20000;
                if(pm.GetCompanionMountedOnMe != null)
                {
                    CharactersDrawOrder.Add(MountedBackLayer--, 
                    new DrawOrderSetting(pm.GetCompanionMountedOnMe, DrawContext.BackLayer));
                    CharactersDrawOrder.Add(MountedFrontLayer++, 
                    new DrawOrderSetting(pm.GetCompanionMountedOnMe, DrawContext.FrontLayer));
                }
                if(pm.GetMountedOnCompanion != null)
                {
                    CharactersDrawOrder.Add(MountedBackLayer--, 
                    new DrawOrderSetting(pm.GetMountedOnCompanion, DrawContext.AllParts));
                    //CharactersDrawOrder.Add(MountedFrontLayer++, 
                    //new DrawOrderSetting(pm.GetMountedOnCompanion, DrawContext.FrontLayer));
                }
                for(int i = Followers.Length - 1; i >= 0; i--)
                {
                    Companion c = Followers[i];
                    if (c == null) continue;
                    switch(c.GetDrawMomentType())
                    {
                        case CompanionDrawMomentTypes.DrawBehindOwner:
                            CharactersDrawOrder.Add(DrawBack--, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            break;
                        case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                            CharactersDrawOrder.Add(DrawFront++, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            break;
                        case CompanionDrawMomentTypes.DrawInBetweenOwner:
                            CharactersDrawOrder.Add(InBetweenBack--, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            break;
                        case CompanionDrawMomentTypes.DrawOwnerInBetween:
                            CharactersDrawOrder.Add(BehindPlayerBody--, 
                                new DrawOrderSetting(c, DrawContext.BackLayer));
                            CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                new DrawOrderSetting(c, DrawContext.FrontLayer));
                            break;
                    }
                }
                CharactersDrawOrder.Add(InBetweenFront++, new DrawOrderSetting(player, DrawContext.FrontLayer));
                CurrentIndex++;
            }
            Player[] ToDraw = new Player[1];
            foreach(DrawOrderSetting dos in CharactersDrawOrder.Values)
            {
                _drawRule = dos.DrawParts;
                ToDraw[0] = dos.character;
                pr.DrawPlayers(camera, ToDraw);
            }
            _drawRule = DrawContext.AllParts;
        }

        void IPlayerRenderer.DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float scale)
        {
            pr.DrawPlayer(camera, drawPlayer, position, rotation, rotationOrigin, shadow, scale);
        }

        void IPlayerRenderer.DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha, float scale, Color borderColor)
        {
            pr.DrawPlayerHead(camera, drawPlayer, position, alpha, scale, borderColor);
        }

        public struct DrawOrderSetting
        {
            public Player character;
            public DrawContext DrawParts;

            public DrawOrderSetting(Player character)
            {
                this.character = character;
                DrawParts = DrawContext.AllParts;
            }

            public DrawOrderSetting(Player character, DrawContext DrawParts)
            {
                this.character = character;
                this.DrawParts = DrawParts;
            }
        }
    }
}