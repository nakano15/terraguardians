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
        internal static bool DrawingCompanions = false, SingleCompanionDraw = false;

        private LegacyPlayerRenderer pr = new LegacyPlayerRenderer();
        public void DrawPlayers(Camera camera, IEnumerable<Player> players)
        {
            NewDrawPlayers(camera, players);
            //OldDrawPlayers(camera, players);
        }

        public static void ChangeDrawContext(DrawContext context)
        {
            _drawRule = context;
        }

        private void NewDrawPlayers(Camera camera, IEnumerable<Player> players)
        {
            List<DrawOrderSetting> TotalDrawOrders = new List<DrawOrderSetting>(),
                CurrentPlayerDrawOrders = new List<DrawOrderSetting>(),
                BatchOrders = new List<DrawOrderSetting>();
            foreach (Player player in players)
            {
                if (player is Companion)
                {
                    if (!DrawingCompanions || (player as Companion).Owner != null)
                        continue;
                }
                bool InsertAhead = true;
                bool OwnerIsUsingFurniture = player.sitting.isSitting || player.sleeping.isSleeping;
                DoPlayerDrawingSorting(player, CurrentPlayerDrawOrders);
                PlayerMod pm = player.GetModPlayer<PlayerMod>();
                //Check DoI for player
                Companion[] Followers = pm.GetSummonedCompanions;
                for (int i = Followers.Length - 1; i >= 0; i--)
                {
                    Companion c = Followers[i];
                    if (c == null || c.IsBeingControlledBySomeone || c.IsMountedOnSomething) continue;
                    DoPlayerDrawingSorting(c, BatchOrders);
                    if (c.Data.FollowAhead || (OwnerIsUsingFurniture != c.UsingFurniture))
                    {
                        CurrentPlayerDrawOrders.AddRange(BatchOrders);
                    }
                    else
                    {
                        CurrentPlayerDrawOrders.InsertRange(0, BatchOrders);
                    }
                    BatchOrders.Clear();
                }
                //Post Ordering Part
                if (InsertAhead)
                    TotalDrawOrders.AddRange(CurrentPlayerDrawOrders);
                else
                    TotalDrawOrders.InsertRange(0, CurrentPlayerDrawOrders);
                CurrentPlayerDrawOrders.Clear();
            }
            Player[] ToDraw = new Player[1];
            foreach(DrawOrderSetting dos in TotalDrawOrders)
            {
                _drawRule = dos.DrawParts;
                ToDraw[0] = dos.character;
                if(dos.character is Companion)
                {
                    Player backedupPlayer = Main.player[dos.character.whoAmI];
                    Main.player[dos.character.whoAmI] = dos.character;
                    pr.DrawPlayers(camera, ToDraw);
                    Main.player[dos.character.whoAmI] = backedupPlayer;
                }
                else
                {
                    pr.DrawPlayers(camera, ToDraw);
                }
            }
            TotalDrawOrders.Clear();
            _drawRule = DrawContext.AllParts;
            SingleCompanionDraw = false;
        }

        void DoPlayerDrawingSorting(Player player, List<DrawOrderSetting> DrawOrders)
        {
            foreach ( DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
            {
                if (doi.Child == player)
                    return;
            }
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            Player character = player;
            Companion MountedOn = null, MountedOnMe = null;
            if (pm.GetCompanionControlledByMe != null)
            {
                character = pm.GetCompanionControlledByMe;
                pm = pm.GetCompanionControlledByMe.GetPlayerMod;
            }
            MountedOn = PlayerMod.PlayerGetMountedOnCompanion(character);
            MountedOnMe = pm.GetCompanionMountedOnMe;
            bool UsingFurniture = character.sitting.isSitting || character.sleeping.isSleeping;
            Companion FurnitureSharing = null;
            if (UsingFurniture)
            {
                Vector2 Bottom = character.Bottom;
                foreach (Companion c in MainMod.ActiveCompanions.Values)
                {
                    if (c.UsingFurniture && c.Bottom == Bottom)
                    {
                        FurnitureSharing = c;
                        break;
                    }
                }
            }
            //Ordering Part Player
            if(FurnitureSharing != null && FurnitureSharing.Base.SitOnPlayerLapOnChair)
            {
                DrawOrders.Add(new DrawOrderSetting(character, DrawContext.BackLayer));
                DrawOrders.Add(new DrawOrderSetting(FurnitureSharing, DrawContext.AllParts));
                CheckDoIFor(FurnitureSharing, DrawOrders);
                DrawOrders.Add(new DrawOrderSetting(character, DrawContext.FrontLayer));
            }
            else
            {
                DrawOrders.Add(new DrawOrderSetting(character, DrawContext.AllParts));
            }
            if (MountedOnMe != null && (MountedOnMe != FurnitureSharing || !MountedOnMe.Base.SitOnPlayerLapOnChair))
            {
                DrawOrders.Insert(0, new DrawOrderSetting(MountedOnMe, DrawContext.BackLayer));
                DrawOrders.Add(new DrawOrderSetting(MountedOnMe, DrawContext.FrontLayer));
                CheckDoIFor(MountedOn, DrawOrders);
            }
            if (MountedOn != null && (MountedOn != FurnitureSharing || !MountedOn.Base.SitOnPlayerLapOnChair))
            {
                DrawOrders.Insert(0, new DrawOrderSetting(MountedOn, DrawContext.AllParts));
                CheckDoIFor(MountedOn, DrawOrders);
            }
            CheckDoIFor(character, DrawOrders);
        }

        void CheckDoIFor(Player player, List<DrawOrderSetting> DrawOrders)
        {
            foreach (DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
            {
                if (doi.Parent == player && doi.Child is Player)
                {
                    switch(doi.Moment)
                    {
                        case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                            {
                                DrawOrders.Insert(0, new DrawOrderSetting((Player)doi.Child, DrawContext.BackLayer));
                                DrawOrders.Add(new DrawOrderSetting((Player)doi.Child, DrawContext.FrontLayer));
                            }
                            return;
                        case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                            {
                                DrawOrders.Insert(0, new DrawOrderSetting((Player)doi.Child, DrawContext.AllParts));
                            }
                            return;
                        case DrawOrderInfo.DrawOrderMoment.BehindParent:
                            {
                                DrawOrders.Add(new DrawOrderSetting((Player)doi.Child, DrawContext.AllParts));
                            }
                            return;
                    }
                }
            }
        }

        private void OldDrawPlayers(Camera camera, IEnumerable<Player> players)
        {
            SortedList<int, DrawOrderSetting> CharactersDrawOrder = new SortedList<int, DrawOrderSetting>();
            int CurrentIndex = 0;
            foreach(Player player in players)
            {
                if (player is Companion)
                {
                    if (!DrawingCompanions || (player as Companion).Owner != null)
                        continue;
                }
                int MyDrawIndex = 100000 + 200000 * CurrentIndex;
                PlayerMod pm = player.GetModPlayer<PlayerMod>();
                Companion controlled = pm.GetCompanionControlledByMe;
                const int FurnitureUsageReduction = int.MaxValue;
                int PlayerDrawLayer = MyDrawIndex;
                Companion[] Followers = pm.GetSummonedCompanions;
                int MountedFrontLayer = MyDrawIndex + 15000;
                int MountedBackLayer = MyDrawIndex - 15000;
                int InBetweenFront = MyDrawIndex + 5001;
                int InBetweenBack = MyDrawIndex + 4999;
                int FrontOfPlayerBody = MyDrawIndex + 10000;
                int BehindPlayerBody = MyDrawIndex - 10000;
                int DrawFront = MyDrawIndex + 20000;
                int DrawBack = MyDrawIndex - 20000;
                int DrawFrontFurniture = MyDrawIndex + 20000 - FurnitureUsageReduction;
                int DrawBackFurniture = MyDrawIndex - 20000 - FurnitureUsageReduction;
                Companion Mount = controlled == null ? pm.GetCompanionMountedOnMe : controlled.GetPlayerMod.GetCompanionMountedOnMe;
                if ((player.sitting.isSitting || player.sleeping.isSleeping) || 
                    (controlled != null && controlled.UsingFurniture))
                {
                    PlayerDrawLayer -= FurnitureUsageReduction;
                    //DrawFront -= FurnitureUsageReduction;
                    //DrawBack -= FurnitureUsageReduction;
                    InBetweenFront -= FurnitureUsageReduction;
                    InBetweenBack -= FurnitureUsageReduction;
                    FrontOfPlayerBody -= FurnitureUsageReduction;
                    BehindPlayerBody -= FurnitureUsageReduction;
                }
                if (controlled == null)
                {
                    CharactersDrawOrder.Add(PlayerDrawLayer, new DrawOrderSetting(player, (SingleCompanionDraw ? _drawRule : DrawContext.BackLayer)));
                }
                else
                {
                    CharactersDrawOrder.Add(PlayerDrawLayer, 
                        new DrawOrderSetting(controlled, (SingleCompanionDraw ? _drawRule : DrawContext.BackLayer)));
                }
                if(Mount != null) //Mounted on Companion
                {
                    if (Mount.UsingFurniture)
                    {
                        MountedBackLayer -= FurnitureUsageReduction;
                        MountedFrontLayer -= FurnitureUsageReduction;
                    }
                    CharactersDrawOrder.Add(MountedBackLayer--, 
                    new DrawOrderSetting(Mount, DrawContext.BackLayer));
                    CharactersDrawOrder.Add(MountedFrontLayer++, 
                    new DrawOrderSetting(Mount, DrawContext.FrontLayer));
                }
                Mount = controlled == null ? pm.GetMountedOnCompanion : controlled.GetPlayerMod.GetMountedOnCompanion;
                if(Mount != null) //Mounted on Player
                {
                    if (Mount.UsingFurniture)
                    {
                        MountedBackLayer -= FurnitureUsageReduction;
                        MountedFrontLayer -= FurnitureUsageReduction;
                    }
                    CharactersDrawOrder.Add(MountedBackLayer--, 
                    new DrawOrderSetting(Mount, DrawContext.AllParts));
                    //CharactersDrawOrder.Add(MountedFrontLayer++, 
                    //new DrawOrderSetting(Mount, DrawContext.FrontLayer));
                }
                if (controlled != null)
                {
                    if (controlled.UsingFurniture)
                    {
                        MountedBackLayer -= FurnitureUsageReduction;
                        MountedFrontLayer -= FurnitureUsageReduction;
                    }
                    CharactersDrawOrder.Add(MountedFrontLayer++, 
                        new DrawOrderSetting(controlled, DrawContext.FrontLayer));
                }
                foreach(DrawOrderInfo doi in DrawOrderInfo.GetDrawOrdersInfo)
                {
                    if (doi.Parent == player && doi.Parent is Player)
                    {
                        switch(doi.Moment)
                        {
                            case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Player)doi.Child, DrawContext.BackLayer));
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Player)doi.Child, DrawContext.FrontLayer));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Player)doi.Child, DrawContext.AllParts));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.BehindParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Player)doi.Child, DrawContext.AllParts));
                                break;
                        }
                    }
                    else if (doi.Child == player && doi.Parent is Player)
                    {
                        switch(doi.Moment)
                        {
                            case DrawOrderInfo.DrawOrderMoment.InBetweenParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Player)doi.Parent, DrawContext.BackLayer));
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Player)doi.Parent, DrawContext.FrontLayer));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.InFrontOfParent:
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting((Player)doi.Parent, DrawContext.AllParts));
                                break;
                            case DrawOrderInfo.DrawOrderMoment.BehindParent:
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting((Player)doi.Parent, DrawContext.AllParts));
                                break;
                        }
                    }
                }
                List<DrawOrderSetting> DrawBackLayer = new List<DrawOrderSetting>(), 
                    DrawFrontLayer = new List<DrawOrderSetting>();
                for(int i = Followers.Length - 1; i >= 0; i--)
                {
                    Companion c = Followers[i];
                    if (c == null || c.IsBeingControlledBySomeone) continue;
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
                    bool FurnitureVersion = c.UsingFurniture;
                    switch(c.GetDrawMomentType())
                    {
                        case CompanionDrawMomentTypes.DrawBehindOwner:
                            if (FurnitureVersion)
                            {
                                foreach(DrawOrderSetting d in DrawFrontLayer)
                                {
                                    CharactersDrawOrder.Add(DrawBackFurniture--, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                                CharactersDrawOrder.Add(DrawBackFurniture--, 
                                    new DrawOrderSetting(c, DrawContext.AllParts));
                                foreach(DrawOrderSetting d in DrawBackLayer)
                                {
                                    CharactersDrawOrder.Add(DrawBackFurniture--, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                            }
                            else
                            {
                                foreach(DrawOrderSetting d in DrawFrontLayer)
                                {
                                    CharactersDrawOrder.Add(DrawBack--, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                                CharactersDrawOrder.Add(DrawBack--, 
                                    new DrawOrderSetting(c, DrawContext.AllParts));
                                foreach(DrawOrderSetting d in DrawBackLayer)
                                {
                                    CharactersDrawOrder.Add(DrawBack--, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                            if (FurnitureVersion)
                            {
                                foreach(DrawOrderSetting d in DrawBackLayer)
                                {
                                    CharactersDrawOrder.Add(DrawFrontFurniture++, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                                CharactersDrawOrder.Add(DrawFrontFurniture++, 
                                    new DrawOrderSetting(c, DrawContext.AllParts));
                                foreach(DrawOrderSetting d in DrawFrontLayer)
                                {
                                    CharactersDrawOrder.Add(DrawFrontFurniture++, 
                                        new DrawOrderSetting(d.character, d.DrawParts));
                                }
                            }
                            else
                            {
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
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawInBetweenOwner:
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(InBetweenBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(InBetweenBack--, 
                                new DrawOrderSetting(c, DrawContext.AllParts));
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(InBetweenBack--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            break;
                        case CompanionDrawMomentTypes.DrawOwnerInBetween:
                            foreach(DrawOrderSetting d in DrawFrontLayer)
                            {
                                CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            foreach(DrawOrderSetting d in DrawBackLayer)
                            {
                                CharactersDrawOrder.Add(BehindPlayerBody--, 
                                    new DrawOrderSetting(d.character, d.DrawParts));
                            }
                            CharactersDrawOrder.Add(FrontOfPlayerBody++, 
                                new DrawOrderSetting(c, DrawContext.FrontLayer));
                            CharactersDrawOrder.Add(BehindPlayerBody--, 
                                new DrawOrderSetting(c, DrawContext.BackLayer));
                            break;
                    }
                    DrawBackLayer.Clear();
                    DrawFrontLayer.Clear();
                }
                if (controlled != null)
                {
                    CharactersDrawOrder.Add(InBetweenFront++, new DrawOrderSetting(controlled, DrawContext.FrontLayer));
                }
                else
                {
                    CharactersDrawOrder.Add(InBetweenFront++, new DrawOrderSetting(player, DrawContext.FrontLayer));
                }
                CurrentIndex++;
            }
            DoDrawOrderRules(camera, CharactersDrawOrder);
            CharactersDrawOrder.Clear();
            _drawRule = DrawContext.AllParts;
            SingleCompanionDraw = false;
        }

        public void DoDrawOrderRules(Camera camera, SortedList<int, DrawOrderSetting> CharactersDrawOrder)
        {
            Player[] ToDraw = new Player[1];
            foreach(DrawOrderSetting dos in CharactersDrawOrder.Values)
            {
                _drawRule = dos.DrawParts;
                ToDraw[0] = dos.character;
                if(dos.character is Companion)
                {
                    Player backedupPlayer = Main.player[dos.character.whoAmI];
                    Main.player[dos.character.whoAmI] = dos.character;
                    pr.DrawPlayers(camera, ToDraw);
                    Main.player[dos.character.whoAmI] = backedupPlayer;
                }
                else
                {
                    pr.DrawPlayers(camera, ToDraw);
                }
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