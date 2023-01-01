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
        private static List<Player> OwnershipChain = new List<Player>();
        private static Dictionary<Player, List<PlayerDrawSet>> TakenCompanionsDrawSet = new Dictionary<Player, List<PlayerDrawSet>>();
        internal static bool TakingDrawSets {
            get { return OwnershipChain.Count > 0; }
        }
        private static void AddDrawSetHolder(Player Owner)
        {
            if(!TakenCompanionsDrawSet.ContainsKey(Owner)) TakenCompanionsDrawSet.Add(Owner, new List<PlayerDrawSet>());
            OwnershipChain.Add(Owner);
        }
        internal static void AddPlayerDrawSet(PlayerDrawSet set)
        {
            if(OwnershipChain.Count == 0) return;
            Player Owner = OwnershipChain[OwnershipChain.Count - 1];
            if(!TakenCompanionsDrawSet.ContainsKey(Owner)) TakenCompanionsDrawSet.Add(Owner, new List<PlayerDrawSet>());
            TakenCompanionsDrawSet[Owner].Add(set);
        }

        internal static List<DrawData> GetDrawDatas(PlayerDrawSet drawset)
        {
            if(OwnershipChain.Count == 0) return new List<DrawData>();
            Player Owner = OwnershipChain[OwnershipChain.Count - 1];
            OwnershipChain.RemoveAt(OwnershipChain.Count - 1);
            List<DrawData> list = new List<DrawData>();
            foreach(PlayerDrawSet ds in TakenCompanionsDrawSet[Owner])
            {
                for(int i = 0; i < ds.DrawDataCache.Count; i++)
                {
                    DrawData dd = ds.DrawDataCache[i];
                    if(dd.color.A > 0)
                    {
                        dd.ignorePlayerRotation = true;
                        list.Add(dd);
                    }
                }
            }
            TakenCompanionsDrawSet.Remove(Owner);
            return list;
        }

        private static IList<Companion> ArrangeCompanionOrdering(Player player)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            Companion[] Followers = pm.GetSummonedCompanions;
            SortedList<short, Companion> FinalCompanionsList = new SortedList<short, Companion>();
            short MountedFrontLayer = 1, MountedBackLayer = -1, InBetweenFront = 5000, InBetweenBack = -5000, Front = 10000, Back = -10000;
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
                        FinalCompanionsList.Add(InBetweenFront--, c);
                        break;
                    case CompanionDrawMomentTypes.DrawOwnerInBetween:
                        FinalCompanionsList.Add(InBetweenBack--, c);
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
                    AddDrawSetHolder(drawInfo.drawPlayer);
                    foreach(Companion c in Companions)
                    {
                        switch(c.GetDrawMomentType())
                        {
                            case CompanionDrawMomentTypes.DrawInBetweenMountedOne:
                            case CompanionDrawMomentTypes.DrawOwnerInBetween:
                                c.DrawCompanion(DrawContext.BackLayer);
                                break;
                            case CompanionDrawMomentTypes.DrawBehindOwner:
                                c.DrawCompanion(DrawContext.AllParts);
                                break;
                        }
                    }
                    drawInfo.DrawDataCache.AddRange(GetDrawDatas(drawInfo));
                }
                catch{}
            }
        }
        public class DrawCompanionBeforeArm : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return true; //PlayerMod.IsPlayerCharacter(drawInfo.drawPlayer);
            }

            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.ArmOverItem);
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                PlayerMod pm = drawInfo.drawPlayer.GetModPlayer<PlayerMod>();
                try
                {
                    IList<Companion> Companions = ArrangeCompanionOrdering(drawInfo.drawPlayer);
                    AddDrawSetHolder(drawInfo.drawPlayer);
                    foreach(Companion c in Companions)
                    {
                        switch(c.GetDrawMomentType())
                        {
                            case CompanionDrawMomentTypes.DrawInBetweenOwner:
                                c.DrawCompanion(DrawContext.AllParts);
                                break;
                        }
                    }
                    drawInfo.DrawDataCache.AddRange(GetDrawDatas(drawInfo));
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
                //Is being drawn behind player.
                PlayerMod pm = drawInfo.drawPlayer.GetModPlayer<PlayerMod>();
                try
                {
                    IList<Companion> Companions = ArrangeCompanionOrdering(drawInfo.drawPlayer);
                    AddDrawSetHolder(drawInfo.drawPlayer);
                    foreach(Companion c in Companions)
                    {
                        switch(c.GetDrawMomentType())
                        {
                            case CompanionDrawMomentTypes.DrawInBetweenMountedOne:
                            case CompanionDrawMomentTypes.DrawOwnerInBetween:
                                c.DrawCompanion(DrawContext.FrontLayer);
                                break;
                            case CompanionDrawMomentTypes.DrawInFrontOfOwner:
                                c.DrawCompanion(DrawContext.AllParts);
                                break;
                        }
                    }
                    drawInfo.DrawDataCache.AddRange(GetDrawDatas(drawInfo));
                }
                catch{}
            }
        }
    }
}