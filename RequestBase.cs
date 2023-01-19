using Terraria;
using System;

namespace terraguardians
{
    public class RequestBase
    {
        public Func<Player, Companion, bool> CanTakeRequest = delegate(Player p, Companion c)
        {
            return true;
        };
        
        public virtual string GetRequestObjective(RequestData data)
        {
            return "???";
        }
        
        public virtual string GetBriefObjective(RequestData data)
        {
            return "???";
        }

        public virtual RequestData GetRequestData(Companion companion)
        {
            return new RequestData();
        }

        public virtual void UpdateRequest(Player player, RequestData data)
        {

        }

        public virtual void OnKillNpc(NPC npc, RequestData data)
        {

        }

        public virtual void OnCompleteRequest(Player player, RequestData data)
        {
            
        }

        public virtual void OnFailRequest(Player player, RequestData data)
        {
            
        }

        public virtual bool IsRequestCompleted(RequestData data)
        {
            return true;
        }
    }

    public class HuntRequest : RequestBase
    {
        string NpcName = "";
        int NpcID = 0;
        int InitialCount;
        float FriendshipLevelExtraCount;

        public HuntRequest(int NpcID, string NpcName = "", int InitialCount = 5, float FriendshipLevelExtraCount = 0.3334f)
        {
            if (NpcName == "")
            {
                NPC n = new NPC();
                n.SetDefaults(NpcID);
                this.NpcName = n.GivenOrTypeName;
            }
            else
            {
                this.NpcName = NpcName;
            }
            this.NpcID = NpcID;
            this.InitialCount = InitialCount;
            this.FriendshipLevelExtraCount = FriendshipLevelExtraCount;
        }

        public override RequestData GetRequestData(Companion companion)
        {
            HuntRequestData data = new HuntRequestData() { MaxKillCount = (int)(InitialCount + FriendshipLevelExtraCount * companion.FriendshipLevel) };
            return data;
        }

        public override string GetRequestObjective(RequestData rawdata)
        {
            HuntRequestData data = (HuntRequestData)rawdata;
            if (data.KillCount >= data.MaxKillCount)
                return "Report back to " + rawdata.RequestGiver.GetNameColored() + ".";
            return "Slay " + (data.MaxKillCount - data.KillCount) + " " + NpcName + ".";
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "slay " + (data as HuntRequestData).MaxKillCount + " " + NpcName;
        }

        public override void OnKillNpc(NPC npc, RequestData rawdata)
        {
            if (npc.type == NpcID)
            {
                HuntRequestData data = (HuntRequestData)rawdata;
                data.KillCount++;
                if (data.KillCount == data.MaxKillCount)
                    Main.NewText("Killed all the " + NpcName + " necessary.");
            }
        }

        public override bool IsRequestCompleted(RequestData rawdata)
        {
            HuntRequestData data = (HuntRequestData)rawdata;
            return data.KillCount >= data.MaxKillCount;
        }

        public class HuntRequestData : RequestData
        {
            public int KillCount = 0;
            public int MaxKillCount = 0;
        }
    }

    public class ItemRequest : RequestBase
    {
        int ItemID;
        string ItemName;
        int InitialCount;
        float ExtraCountPerFriendshipLevel;

        public ItemRequest(int ItemID, int Count = 5, float ExtraCount = 0.3334f, string Name = "")
        {
            if (Name == "")
            {
                Item i = new Item();
                i.SetDefaults(ItemID);
                ItemName = i.Name;
            }
            else
            {
                ItemName = Name;
            }
            this.ItemID = ItemID;
            InitialCount = Count;
            ExtraCountPerFriendshipLevel = ExtraCount;
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "bring me " + (data as ItemRequestData).MaxItemCount + " " + ItemName;
        }

        public override string GetRequestObjective(RequestData rawdata)
        {
            ItemRequestData data = (ItemRequestData)rawdata;
            if (data.LastItemCount >= data.MaxItemCount)
                return "Deliver the " + data.MaxItemCount + " " + ItemName + " to " + rawdata.RequestGiver.GetNameColored() + ".";
            return "Get " + (data.MaxItemCount - data.LastItemCount) + " " + ItemName + ".";
        }

        public override RequestData GetRequestData(Companion companion)
        {
            ItemRequestData data = new ItemRequestData() { MaxItemCount = (int)(InitialCount + ExtraCountPerFriendshipLevel * companion.FriendshipLevel) }; //How do I setup the max count?
            return data;
        }

        public override void UpdateRequest(Player player, RequestData data)
        {
            (data as ItemRequestData).LastItemCount = player.CountItem(ItemID);
        }

        public override bool IsRequestCompleted(RequestData rawdata)
        {
            ItemRequestData data = (ItemRequestData)rawdata;
            return data.LastItemCount >= data.MaxItemCount;
        }
        
        public override void OnCompleteRequest(Player player, RequestData data)
        {
            int StackToDelete = (data as ItemRequestData).MaxItemCount;
            for(int i = 57; i >= 0; i--)
            {
                Item item = player.inventory[i];
                if (item.type == ItemID)
                {
                    int Stack = item.maxStack;
                    if (Stack > StackToDelete)
                        Stack = StackToDelete;
                    item.stack -= Stack;
                    if (item.stack == 0)
                        item.SetDefaults(0);
                    StackToDelete -= Stack;
                    if (StackToDelete <= 0)
                        break;
                }
            }
        }

        public class ItemRequestData : RequestData
        {
            public int LastItemCount = 0;
            public int MaxItemCount = 0;
        }
    }
}