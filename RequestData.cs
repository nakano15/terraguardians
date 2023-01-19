using Terraria;
using System;

namespace terraguardians
{
    public class RequestData
    {
        private RequestBase Base = null;
        private int RequestID = 0;
        private string RequestModID = "";
        private Companion RequestGiver;
        public RequestStatus status = 0;
        public int LifeTime = 0;

        internal void SetRequestGiver(Companion companion)
        {
            RequestGiver = companion;
        }

        internal void SetRequestBase(RequestBase ReqBase)
        {
            Base = ReqBase;
        }

        public enum RequestStatus : byte
        {
            Cooldown = 0,
            Ready = 1,
            WaitingAccept = 2,
            Active = 3
        }
    }
}