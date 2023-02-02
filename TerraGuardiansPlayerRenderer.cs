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
                int InBetweenFront = MyDrawIndex + 5001;
                int InBetweenBack = MyDrawIndex + 4999;
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
                foreach(DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
                {
                    if (doi.Parent == player)
                    {
                        switch(doi.Moment)
                        {
                            case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Companion)doi.Child, DrawContext.BackLayer));
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Companion)doi.Child, DrawContext.FrontLayer));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Companion)doi.Child, DrawContext.AllParts));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.BehindParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Companion)doi.Child, DrawContext.AllParts));
                                break;
                        }
                    }
                    else if (doi.Child == player)
                    {
                        switch(doi.Moment)
                        {
                            case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Companion)doi.Parent, DrawContext.BackLayer));
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Companion)doi.Parent, DrawContext.FrontLayer));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Companion)doi.Parent, DrawContext.AllParts));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.BehindParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Companion)doi.Parent, DrawContext.AllParts));
                                break;
                        }
                    }
                }
                List<DrawOrderSetting> DrawBackLayer = new List<DrawOrderSetting>(), 
                    DrawFrontLayer = new List<DrawOrderSetting>();
                for(int i = Followers.Length - 1; i >= 0; i--)
                {
                    Companion c = Followers[i];
                    if (c == null) continue;
                    foreach(DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
                    {
                        if (doi.Parent == c)
                        {
                            switch(doi.Moment)
                            {
                                case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                                    DrawBackLayer.Add(new DrawOrderSetting((Companion)doi.Child, DrawContext.BackLayer));
                                    DrawFrontLayer.Add(new DrawOrderSetting((Companion)doi.Child, DrawContext.FrontLayer));
                                    break;
                                case DrawOrderInfo.DrawOrderMoment.BehindParent:
                                    DrawBackLayer.Add(new DrawOrderSetting((Companion)doi.Child, DrawContext.AllParts));
                                    break;
                                case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                                    DrawFrontLayer.Add(new DrawOrderSetting((Companion)doi.Child, DrawContext.AllParts));
                                    break;
                            }
                        }
                    }
                    switch(c.GetDrawMomentType())
                    {
                        case CompanionDrawMomentTypes.DrawBehindOwner:
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(DrawBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(DrawBack--, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(DrawBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(DrawFront++, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(DrawFront++, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(DrawFront++, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawInBetweenOwner:
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(InBetweenBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(InBetweenBack--, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(InBetweenBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawOwnerInBetween:
                            CharactersDrawOrder.Add(BehindPlayerBody--, 
                                new DrawOrderSetting(c, DrawContext.BackLayer));
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                new DrawOrderSetting(c, DrawContext.FrontLayer));
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            break;
                    }
                    DrawBackLayer.Clear();
                    DrawFrontLayer.Clear();
                }
                CharactersDrawOrder.Add(InBetweenFront++, new DrawOrderSetting(player, DrawContext.FrontLayer));
                CurrentIndex++;
            }
            DoDrawOrderRules(camera, CharactersDrawOrder);
            CharactersDrawOrder.Clear();
            _drawRule = DrawContext.AllParts;
        }

        public void DoDrawOrderRules(Camera camera, SortedList<int, DrawOrderSetting> CharactersDrawOrder)
        {
            Player[] ToDraw = new Player[1];
            foreach(DrawOrderSetting dos in CharactersDrawOrder.Values)
            {
                _drawRule = dos.DrawParts;
                ToDraw[0] = dos.character;
                Player backedupPlayer = null;
                if(dos.character is Companion)
                {
                    backedupPlayer = Main.player[dos.character.whoAmI];
                    Main.player[dos.character.whoAmI] = dos.character;
                }
                pr.DrawPlayers(camera, ToDraw);
                if (backedupPlayer != null) Main.player[dos.character.whoAmI] = backedupPlayer;
            }
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