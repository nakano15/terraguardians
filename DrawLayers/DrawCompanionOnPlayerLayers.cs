using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Renderers;
using System.Collections.Generic;

namespace terraguardians
{
    public class DrawCompanionOnPlayerLayers
    {
        private static IList<Companion> ArrangeCompanionOrdering(Player player)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            Companion[] Followers = pm.GetSummonedCompanions;
            SortedList<short, Companion> FinalCompanionsList = new SortedList<short, Companion>();
            short MountedFrontLayer = 1, MountedBackLayer = -1, Front = 1000, Back = -1000;
            if(pm.GetCompanionMountedOnMe != null)
                FinalCompanionsList.Add(MountedBackLayer, pm.GetCompanionMountedOnMe);
            if(pm.GetMountedOnCompanion != null)
                FinalCompanionsList.Add(MountedFrontLayer, pm.GetMountedOnCompanion);
            for(int i = Followers.Length - 1; i >= 0; i--)
            {
                Companion c = Followers[i];
                if(c == null) continue;
                switch(c.GetDrawMomentType())
                {
                    case CompanionDrawMomentTypes.DrawBehindOwner:
                        FinalCompanionsList.Add(Back++, c);
                        break;
                    case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                        FinalCompanionsList.Add(Front++, c);
                        break;
                    case CompanionDrawMomentTypes.DrawInBetweenOwner:
                        FinalCompanionsList.Add(Front++, c);
                        break;
                }
            }
            return FinalCompanionsList.Values;
        }

        public class DrawCompanionBehindLayer : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return true; //PlayerMod.IsPlayerCharacter(drawInfo.drawPlayer);
            }

            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.Leggings);
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                PlayerMod pm = drawInfo.drawPlayer.GetModPlayer<PlayerMod>();
                try
                {
                    IList<Companion> Companions = ArrangeCompanionOrdering(drawInfo.drawPlayer);
                    foreach(Companion c in Companions)
                    {
                        switch(c.GetDrawMomentType())
                        {
                            case CompanionDrawMomentTypes.DrawInBetweenMountedOne:
                                c.DrawCompanion(DrawContext.BackLayer);
                                break;
                            case CompanionDrawMomentTypes.DrawBehindOwner:
                                c.DrawCompanion(DrawContext.AllParts);
                                break;
                            case CompanionDrawMomentTypes.DrawInBetweenOwner:
                                c.DrawCompanion(DrawContext.BackLayer);
                                break;
                        }
                    }
                }
                catch{}
            }
        }
        public class DrawCompanionFrontLayer : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return true; //PlayerMod.IsPlayerCharacter(drawInfo.drawPlayer);
            }

            public override Position GetDefaultPosition()
            {
                return new AfterParent(PlayerDrawLayers.ArmOverItem);
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                //Is being ignored by tModLoader, it seems.
                PlayerMod pm = drawInfo.drawPlayer.GetModPlayer<PlayerMod>();
                try
                {
                    IList<Companion> Companions = ArrangeCompanionOrdering(drawInfo.drawPlayer);
                    foreach(Companion c in Companions)
                    {
                        switch(c.GetDrawMomentType())
                        {
                            case CompanionDrawMomentTypes.DrawInBetweenMountedOne:
                                c.DrawCompanion(DrawContext.FrontLayer);
                                break;
                            case CompanionDrawMomentTypes.DrawInBetweenOwner:
                                c.DrawCompanion(DrawContext.FrontLayer);
                                break;
                            case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                                c.DrawCompanion(DrawContext.AllParts);
                                break;
                        }
                    }
                }
                catch{}
            }
        }
    }
}