using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.Localization;
using terraguardians.Interfaces.Orders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace terraguardians.Interfaces
{
    public class CompanionOrderInterface : GameInterfaceLayer
    {
        new public static bool Active = false;
        public static int SelectedCompanion = 255;
        private static List<OrdersContainer> OrdersThread = new List<OrdersContainer>(); //Need to not be as accessible
        public static List<CompanionOrderStep> LobbyOrders = new List<CompanionOrderStep>();
        public static List<CompanionOrderStep> OrderOrders = new List<CompanionOrderStep>();
        public static List<CompanionOrderStep> TacticsOrders = new List<CompanionOrderStep>();
        public static List<CompanionOrderStep> ActionOrders = new List<CompanionOrderStep>();
        public static List<CompanionOrderStep> InteractionOrders = new List<CompanionOrderStep>();
        public static List<CompanionOrderStep> ItemOrders = new List<CompanionOrderStep>();
        //Companion Selection Related
        private static List<CompanionOrderStep> CompanionsWithEveryoneList = new List<CompanionOrderStep>();
        private static List<CompanionOrderStep> CompanionsWithoutEveryoneList = new List<CompanionOrderStep>();
        private static int BackedUpInventoryRow = 0;
        const string InterfaceKey = "Mods.terraguardians.Interface.Orders.";

        public CompanionOrderInterface() : 
            base("TerraGuardians: Orders UI", InterfaceScaleType.UI)
        {
            SetupBasicOrders();
            CompanionsWithEveryoneList.Add(new CompanionIndexStep(255));
            for (byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                CompanionIndexStep Index = new CompanionIndexStep(i);
                CompanionsWithEveryoneList.Add(Index);
                CompanionsWithoutEveryoneList.Add(Index);
            }
        }
        //Look for stances where interface draw returns false.

        private void SetupBasicOrders()
        {
            LobbyOrders.Add(new PullCompanionsOption());
            LobbyOrders.Add(new OrderOption());
            LobbyOrders.Add(new InteractionOption());
            LobbyOrders.Add(new ActionOption());
            LobbyOrders.Add(new ItemOption());
            LobbyOrders.Add(new TacticsOption());
            //
            OrderOrders.Add(new FollowOrder());
            OrderOrders.Add(new WaitOrder());
            OrderOrders.Add(new GuardOrder());
            OrderOrders.Add(new GoAheadOrBehindOrder());
            OrderOrders.Add(new AvoidCombatOrder());
            //
            TacticsOrders.Add(new ChargeOnTargetOrders());
            TacticsOrders.Add(new AvoidContactOrders());
            TacticsOrders.Add(new AttackFromFarOrders());
            TacticsOrders.Add(new StickCloseOrders());
            TacticsOrders.Add(new FreeWillOrders());
            //
            ActionOrders.Add(new GoSellLootOrders());
            //ActionOrders.Add(new UseFurnitureOrders());
            ActionOrders.Add(new LiftMeOrders());
            ActionOrders.Add(new FreeControlOrders());
            //
            InteractionOrders.Add(new MountOrders());
            InteractionOrders.Add(new PlayerControlOrders());
            InteractionOrders.Add(new SetLeaderOrders());
            //
            ItemOrders.Add(new UseBuffsOrders());
        }

        public static void Open()
        {
            if (MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().KnockoutState > KnockoutStates.Awake) return;
            BackedUpInventoryRow = MainMod.GetLocalPlayer.selectedItem;
            Active = true;
            OpenNewOrderList(LobbyOrders);
        }

        public static void Close()
        {
            Active = false;
            OrdersThread.Clear();
            MainMod.GetLocalPlayer.selectedItem = BackedUpInventoryRow;
        }

        public static void OnOrderKeyPressed()
        {
            if (!Active)
                Open();
            else
            {
                if (OrdersThread.Count > 1)
                {
                    OrdersThread.RemoveAt(OrdersThread.Count - 1);
                    OrdersThread[OrdersThread.Count - 1].Selected = 255;
                }
                else
                    Close();
            }
        }

        public static void ExecuteOrders()
        {
            List<Companion> Companions = new List<Companion>();
            Companion[] PlayerCompanions = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().GetSummonedCompanions;
            for(byte i = 0; i < PlayerCompanions.Length; i++)
            {
                if (PlayerCompanions[i] != null)
                {
                    if (SelectedCompanion == 255 || SelectedCompanion == i)
                    {
                        Companions.Add(PlayerCompanions[i]);
                    }
                }
            }
            foreach(OrdersContainer o in OrdersThread)
            {
                if (o.Selected < 255)
                {
                    o.OrderList[o.Selected].FinallyDo(Companions);
                }
            }
            Close();
        }

        public static void OpenNewOrderList(List<CompanionOrderStep> OrderList)
        {
            List<CompanionOrderStep> NewSteps = new List<CompanionOrderStep>();
            foreach(CompanionOrderStep s in OrderList)
            {
                if (s != null && s.Visible)
                    NewSteps.Add(s);
            }
            OrdersThread.Add(new OrdersContainer(NewSteps));
        }

        public static void OpenCompanionSelectionStep(bool WithEveryoneOption)
        {
            if (WithEveryoneOption)
                OpenNewOrderList(CompanionsWithEveryoneList);
            else
                OpenNewOrderList(CompanionsWithoutEveryoneList);
        }

        protected override bool DrawSelf()
        {
            Vector2 DrawPosition = new Vector2(8, Main.screenHeight - 52);
            foreach(OrdersContainer o in OrdersThread)
            {
                int MaxOptions = Math.Min(o.OrderList.Count, 10);
                Vector2 DrawStart = new Vector2(DrawPosition.X, DrawPosition.Y - MaxOptions * 20);
                float HighestTextWidth = 0;
                for(int i = 0; i < MaxOptions; i++)
                {
                    Color c = i == o.Selected ? Color.Yellow : Color.White;
                    int NumberIndex = i + 1;
                    if (NumberIndex == 10)
                        NumberIndex = 0;
                    Vector2 Dimension = Utils.DrawBorderString(Main.spriteBatch, NumberIndex + ". " + o.OrderList[i].Text, DrawStart, c, 0.85f);
                    DrawStart.Y += 20;
                    if (Dimension.X > HighestTextWidth)
                        HighestTextWidth = Dimension.X;
                }
                DrawPosition.X += HighestTextWidth + 8;
            }
            return base.DrawSelf();
        }
        
        public static void OnUnload()
        {
            OrdersThread.Clear();
            LobbyOrders.Clear();
            OrderOrders.Clear();
            TacticsOrders.Clear();
            ActionOrders.Clear();
            InteractionOrders.Clear();
            ItemOrders.Clear();
            CompanionsWithEveryoneList.Clear();
            CompanionsWithoutEveryoneList.Clear();
            OrdersThread = null;
            LobbyOrders = null;
            OrderOrders = null;
            TacticsOrders = null;
            ActionOrders = null;
            InteractionOrders = null;
            ItemOrders = null;
            CompanionsWithEveryoneList = null;
            CompanionsWithoutEveryoneList = null;
        }

        internal static void ProcessKeyPress()
        {
            if (MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().KnockoutState > KnockoutStates.Awake)
            {
                Close();
                return;
            }
            byte Pressed = 255;
            for (byte k = 0; k < 10; k++)
            {
                Microsoft.Xna.Framework.Input.Keys key = Microsoft.Xna.Framework.Input.Keys.D1 + k;
                if (Main.keyState.IsKeyDown(key) && Main.oldKeyState.IsKeyUp(key))
                {
                    Pressed = k;
                }
            }
            if (Pressed < 255 && OrdersThread.Count > 0)
            {
                OrdersContainer c = OrdersThread[OrdersThread.Count - 1];
                if (Pressed < c.OrderList.Count)
                {
                    c.Selected = Pressed;
                    c.OrderList[Pressed].OnActivate();
                }
            }
        }

        public class CompanionOrderStep
        {
            public virtual string Text => "???";
            public virtual bool Visible => true;
            const string InterfaceKey = "Mods.terraguardians.Interface.Orders.";

            internal string GetTranslation(string Key)
            {
                return Language.GetTextValue(InterfaceKey + Key);
            }

            public virtual void OnActivate() //Avoid taking actions on OnActivate(). Use FinallyDo instead.
            {
                
            }

            public virtual void FinallyDo(List<Companion> Companions) //Called when a menu closes and carries on with the actions
            {
                
            }
        }

        public class CompanionIndexStep : CompanionOrderStep
        {
            public byte Index = 255;
            public override string Text
            {
                get
                {
                    if (Index == 255)
                        return Language.GetTextValue(InterfaceKey + "Everyone");
                    Companion c = PlayerMod.PlayerGetSummonedCompanionByOrder(MainMod.GetLocalPlayer, Index);
                    if (c != null)
                        return c.GetName;
                    return "???";
                }
            }

            public override bool Visible => Index == 255 || PlayerMod.PlayerGetSummonedCompanionByOrder(MainMod.GetLocalPlayer, Index) != null;

            public CompanionIndexStep(byte Index)
            {
                this.Index = Index;
            }

            public override void OnActivate()
            {
                SelectedCompanion = Index;
                ExecuteOrders();
            }
        }

        public class OrdersContainer
        {
            public List<CompanionOrderStep> OrderList = new List<CompanionOrderStep>();
            public byte Selected = 255; 

            public OrdersContainer(List<CompanionOrderStep> OrderList)
            {
                this.OrderList = OrderList;
            }
        }
    }
}