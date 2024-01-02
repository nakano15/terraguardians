using Terraria;
using System.Linq;
using System.Collections.Generic;

namespace terraguardians
{
    public struct DrawOrderInfo
    {
        private static DrawOrderInfo[] DrawOrdersLogged = new DrawOrderInfo[0];
        private static List<DrawOrderInfo> NewDrawOrders = new List<DrawOrderInfo>();
        public static DrawOrderInfo[] GetDrawOrdersInfo { get { return DrawOrdersLogged; } }

        public Entity Parent;
        public Entity Child;
        public DrawOrderMoment Moment;

        public static void AddDrawOrderInfo(Entity Target, Entity ActionDoer, DrawOrderMoment Moment)
        {
            NewDrawOrders.Add(new DrawOrderInfo(){ Parent = Target, Child = ActionDoer, Moment = Moment });
        }

        public static void Update()
        {
            DrawOrdersLogged = NewDrawOrders.ToArray();
            NewDrawOrders.Clear();
        }

        internal static void ClearDrawOrders()
        {
            NewDrawOrders.Clear();
            DrawOrdersLogged = new DrawOrderInfo[0];
        }

        public static void Unload()
        {
            NewDrawOrders.Clear();
            NewDrawOrders = null;
            DrawOrdersLogged = null;
        }

        public static bool IsChildrenOfDrawOrder(Entity Character)
        {
            foreach(DrawOrderInfo doi in DrawOrdersLogged)
            {
                if (doi.Child == Character)
                {
                    return true;
                }
            }
            return false;
        }

        public enum DrawOrderMoment : byte
        {
            InBetweenParent = 0,
            InFrontOfParent = 1,
            BehindParent = 2
        }
    }
}