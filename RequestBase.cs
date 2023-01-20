using Terraria;
using System;
using System.IO;

namespace terraguardians
{
    public class RequestBase
    {
        public int RewardValue = 0;

        public Func<Player, CompanionData, bool> CanTakeRequest = delegate(Player p, CompanionData c)
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

        public virtual RequestProgress GetRequestProgress(CompanionData companion)
        {
            return new RequestProgress();
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

        public HuntRequest(int NpcID, string NpcName = "", int InitialCount = 5, float FriendshipLevelExtraCount = 0.3334f, int RewardValue = 0)
        {
            NPC n = new NPC();
            n.SetDefaults(NpcID);
            if (NpcName == "")
            {
                this.NpcName = n.GivenOrTypeName;
            }
            else
            {
                this.NpcName = NpcName;
            }
            if (RewardValue != 0)
                this.RewardValue = RewardValue;
            else
                this.RewardValue = (int)(n.lifeMax * 0.333f * InitialCount);
            this.NpcID = NpcID;
            this.InitialCount = InitialCount;
            this.FriendshipLevelExtraCount = FriendshipLevelExtraCount;
        }

        public override RequestProgress GetRequestProgress(CompanionData companion)
        {
            HuntRequestProgress data = new HuntRequestProgress() { MaxKillCount = (int)(InitialCount + FriendshipLevelExtraCount * companion.FriendshipLevel) };
            return data;
        }

        public override string GetRequestObjective(RequestData rawdata)
        {
            HuntRequestProgress data = (HuntRequestProgress)rawdata.GetRequestProgress;
            if (data.KillCount >= data.MaxKillCount)
                return "Report back to " + rawdata.GetRequestGiver.GetNameColored() + ".";
            return "Slay " + (data.MaxKillCount - data.KillCount) + " " + NpcName + " for "+rawdata.GetRequestGiver.GetNameColored()+".";
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "slay " + (data.GetRequestProgress as HuntRequestProgress).MaxKillCount + " " + NpcName;
        }

        public override void OnKillNpc(NPC npc, RequestData rawdata)
        {
            if (npc.type == NpcID)
            {
                HuntRequestProgress data = (HuntRequestProgress)rawdata.GetRequestProgress;
                data.KillCount++;
                if (data.KillCount == data.MaxKillCount)
                    Main.NewText("Killed all the " + NpcName + " necessary.");
            }
        }

        public override bool IsRequestCompleted(RequestData rawdata)
        {
            HuntRequestProgress data = (HuntRequestProgress)rawdata.GetRequestProgress;
            return data.KillCount >= data.MaxKillCount;
        }

        public class HuntRequestProgress : RequestProgress
        {
            public int KillCount = 0;
            public int MaxKillCount = 0;

            const byte Version = 0;

            public override void Save(BinaryWriter writer)
            {
                writer.Write(Version);
                writer.Write(KillCount);
                writer.Write(MaxKillCount);
            }

            public override void Load(BinaryReader reader)
            {
                byte LastVersion = reader.ReadByte();
                KillCount = reader.ReadInt32();
                MaxKillCount = reader.ReadInt32();
            }
        }
    }

    public class ItemRequest : RequestBase
    {
        int ItemID;
        string ItemName;
        int InitialCount;
        float ExtraCountPerFriendshipLevel;

        public ItemRequest(int ItemID, int Count = 5, float ExtraCount = 0.3334f, string Name = "", int RewardValue = 0)
        {
            Item i = new Item();
            i.SetDefaults(ItemID);
            if (Name == "")
            {
                ItemName = i.Name;
            }
            else
            {
                ItemName = Name;
            }
            if (RewardValue != 0)
            {
                this.RewardValue = RewardValue;
            }
            else
            {
                this.RewardValue = (int)(i.value * 0.333f * Count);
            }
            this.ItemID = ItemID;
            InitialCount = Count;
            ExtraCountPerFriendshipLevel = ExtraCount;
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "bring me " + (data.GetRequestProgress as ItemRequestProgress).MaxItemCount + " " + ItemName;
        }

        public override string GetRequestObjective(RequestData rawdata)
        {
            ItemRequestProgress data = (ItemRequestProgress)rawdata.GetRequestProgress;
            if (data.LastItemCount >= data.MaxItemCount)
                return "Deliver the " + data.MaxItemCount + " " + ItemName + " to " + rawdata.GetRequestGiver.GetNameColored() + ".";
            return "Get " + (data.MaxItemCount - data.LastItemCount) + " " + ItemName + " for "+rawdata.GetRequestGiver.GetNameColored()+".";
        }

        public override RequestProgress GetRequestProgress(CompanionData companion)
        {
            ItemRequestProgress data = new ItemRequestProgress() { MaxItemCount = (int)(InitialCount + ExtraCountPerFriendshipLevel * companion.FriendshipLevel) }; //How do I setup the max count?
            return data;
        }

        public override void UpdateRequest(Player player, RequestData data)
        {
            (data.GetRequestProgress as ItemRequestProgress).LastItemCount = player.CountItem(ItemID);
        }

        public override bool IsRequestCompleted(RequestData rawdata)
        {
            ItemRequestProgress data = (ItemRequestProgress)rawdata.GetRequestProgress;
            return data.LastItemCount >= data.MaxItemCount;
        }
        
        public override void OnCompleteRequest(Player player, RequestData data)
        {
            int StackToDelete = (data.GetRequestProgress as ItemRequestProgress).MaxItemCount;
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

        public class ItemRequestProgress : RequestProgress
        {
            public int LastItemCount = 0;
            public int MaxItemCount = 0;

            const byte Version = 0;

            public override void Save(BinaryWriter writer)
            {
                writer.Write(Version);
                writer.Write(MaxItemCount);
            }

            public override void Load(BinaryReader reader)
            {
                byte LastVersion = reader.ReadByte();
                MaxItemCount = reader.ReadInt32();
            }
        }
    }
}