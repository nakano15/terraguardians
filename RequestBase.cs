using Terraria;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;

namespace terraguardians
{
    public class RequestBase
    {
        public int RewardValue = 0;
        public bool AllowTakingRequest = true;

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

        public virtual void ModifyNpcSpawns(ref IDictionary<int, float> pool, NPCSpawnInfo spawnInfo, RequestData data)
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
        public int[] AliasIDs = new int[0];
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
            int Count = (data.MaxKillCount - data.KillCount);
            return "Slay " + Count + " " + MainMod.PluralizeString(NpcName, Count) + " for "+rawdata.GetRequestGiver.GetNameColored()+".";
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "slay " + (data.GetRequestProgress as HuntRequestProgress).MaxKillCount + " " + NpcName;
        }

        public override void OnKillNpc(NPC npc, RequestData rawdata)
        {
            if (KilledRequestMob(npc))
            {
                HuntRequestProgress data = (HuntRequestProgress)rawdata.GetRequestProgress;
                data.KillCount++;
                if (data.KillCount == data.MaxKillCount)
                    Main.NewText("Killed all the " + NpcName + " necessary.");
            }
        }

        bool KilledRequestMob(NPC npc)
        {
            foreach (int i in AliasIDs)
            {
                if (NpcMod.IsSameMonster(npc, i)) return true;
            }
            return NpcMod.IsSameMonster(npc, NpcID);
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
                return "Deliver the " + data.MaxItemCount + " " + MainMod.PluralizeString(ItemName, data.MaxItemCount) + " to " + rawdata.GetRequestGiver.GetNameColored() + ".";
            int Count = (data.MaxItemCount - data.LastItemCount);
            return "Get " + Count + " " + MainMod.PluralizeString(ItemName, Count) + " for "+rawdata.GetRequestGiver.GetNameColored()+".";
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

    public class InvasionRequest : RequestBase
    {
        int MonsterID;
        string MonsterName;
        int InitialCount;
        float ExtraCountPerFriendshipLevel;
        int MaxSpawnCount = 7;

        public override RequestProgress GetRequestProgress(CompanionData companion)
        {
            return new InvasionProgress() { MaxKillCount = InitialCount + (int)(ExtraCountPerFriendshipLevel * companion.FriendshipLevel) };
        }

        public InvasionRequest(int MonsterID, int Count = 5, float ExtraCountPerFriendshipLevel = 0.2f, string MonsterName = "", int MaxSpawnCount = 7, int RewardValue = 0)
        {
            this.MaxSpawnCount = MaxSpawnCount;
            this.MonsterID = MonsterID;
            this.InitialCount = Count;
            this.ExtraCountPerFriendshipLevel = ExtraCountPerFriendshipLevel;
            NPC n = new NPC();
            n.SetDefaults(MonsterID);
            if (MonsterName == "")
                this.MonsterName = n.TypeName;
            else
                this.MonsterName = MonsterName;
            if (RewardValue != 0)
            {
                this.RewardValue = RewardValue;
            }
            else
            {
                this.RewardValue = (int)(n.value * .333f * Count);
            }
        }

        public override string GetBriefObjective(RequestData data)
        {
            return "survive a "+MonsterName + " invasion";
        }

        public override bool IsRequestCompleted(RequestData data)
        {
            InvasionProgress d = data.GetRequestProgress as InvasionProgress;
            return d.KillCount >= d.MaxKillCount;
        }

        public override string GetRequestObjective(RequestData data)
        {
            InvasionProgress d = data.GetRequestProgress as InvasionProgress;
            if (d.KillCount < d.MaxKillCount)
            {
                int Count = (d.MaxKillCount - d.KillCount);
                return "Kill " + Count + " Invading " + MainMod.PluralizeString(MonsterName, Count) + ".";
            }
            return "Survived the " + MonsterName + " invasion.";
        }

        /*public override void UpdateRequest(Player player, RequestData data)
        {
            InvasionProgress d = data.GetRequestProgress as InvasionProgress;
            if (d.KillCount < d.MaxKillCount && d.CanTrySpawn())
            {
                int MaxSpawns = Math.Min(7, d.MaxKillCount - d.KillCount);
                int Spawns = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == MonsterID)
                    {
                        Spawns++;
                    }
                }
                if (Spawns < MaxSpawns)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, MonsterID);
                }
            }
        }*/

        public override void ModifyNpcSpawns(ref IDictionary<int, float> pool, NPCSpawnInfo spawnInfo, RequestData data)
        {
            InvasionProgress d = data.GetRequestProgress as InvasionProgress;
            int MaxSpawns = Math.Min(7, d.MaxKillCount - d.KillCount);
            int Spawns = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == MonsterID)
                {
                    Spawns++;
                }
            }
            if (Spawns < MaxSpawns)
            {
                pool.Clear();
                pool.Add(MonsterID, 5);
            }
        }

        public override void OnKillNpc(NPC npc, RequestData data)
        {
            if (npc.type == MonsterID)
            {
                InvasionProgress d = data.GetRequestProgress as InvasionProgress;
                if (d.KillCount < d.MaxKillCount)
                {
                    d.KillCount ++;
                    if (d.KillCount == d.MaxKillCount)
                    {
                        Main.NewText("That was the last of the " + MainMod.PluralizeString(MonsterName, d.MaxKillCount) + ".");
                    }
                }
            }
        }

        public class InvasionProgress : RequestProgress
        {
            public int KillCount = 0;
            public int MaxKillCount = 0;
            const byte Version = 0;
            public int Delay = 180;

            public bool CanTrySpawn()
            {
                Delay--;
                if(Delay <= 0)
                {
                    Delay += Main.rand.Next(180, 301);
                    return true;
                }
                return false;
            }

            public override void Save(BinaryWriter writer)
            {
                writer.Write(Version);
                writer.Write(KillCount);
                writer.Write(MaxKillCount);
            }

            public override void Load(BinaryReader reader)
            {
                byte version = reader.ReadByte();
                KillCount = reader.ReadInt32();
                MaxKillCount = reader.ReadInt32();
            }
        }

        /*public class DeliveryRequest : RequestBase
        {

        }*/
    }
}