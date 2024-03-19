using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System;
using System.IO;

namespace terraguardians
{
    public class RequestData
    {
        private RequestBase Base = null;
        public RequestBase GetBase { get { return Base; } }
        private int RequestID = 0;
        private string RequestModID = "";
        public int GetRequestID { get { return RequestID; } }
        public string GetRequestModID { get { return RequestModID; } }
        private CompanionData RequestGiver;
        public CompanionData GetRequestGiver { get { return RequestGiver; } }
        public RequestStatus status = 0;
        public bool IsActive { get { return status == RequestStatus.Active; } }
        private RequestProgress progress;
        public RequestProgress GetRequestProgress { get { return progress; } }
        public int LifeTime = 0;
        private bool ValidRequest = true;
        public bool IsValidRequest { get { return ValidRequest; } }
        public const byte MaxActiveRequests = 3;
        public Reward[] RequestRewards = new Reward[]{ new Reward(), new Reward(), new Reward() };

        public RequestData(CompanionData owner)
        {
            RequestGiver = owner;
            ValidRequest = false;
            //ChangeRequest(-1, "");
            SetRequestOnCooldown(true);
        }

        public void ChangeRequest(int ID, string ModID)
        {
            Base = RequestContainer.GetRequest(ID, ModID, out ValidRequest);
            progress = Base.GetRequestProgress(RequestGiver);
            if(ValidRequest)
            {
                RequestID = ID;
                RequestModID = ModID;
            }
        }

        public bool IsRequestCompleted()
        {
            return Base.IsRequestCompleted(this);
        }

        public string GetTimeLeft()
        {
            if (LifeTime >= 60 * 60 * 60) //Higher or equal than 1 hour
            {
                return ((int)(LifeTime / (60 * 60 * 60))) + "h left";
            }
            if (LifeTime >= 60 * 60) //Higher or equal than 1 minute
            {
                return ((int)(LifeTime / (60 * 60))) + "m left";
            }
            return "Ending soon.";
        }

        public void UpdateRequest(Player player, CompanionData companion)
        {
            switch(status)
            {
                case RequestStatus.Cooldown:
                    LifeTime--;
                    if (LifeTime <= 0)
                    {
                        status = RequestStatus.Ready;
                    }
                    break;
                case RequestStatus.Active:
                    LifeTime--;
                    Base.UpdateRequest(player, this);
                    if (LifeTime <= 0)
                    {
                        SetRequestOnCooldown();
                        Main.NewText("You took too long to complete " + companion.GetNameColored() + "'s request, that they forgot about it...", new Microsoft.Xna.Framework.Color(200, 0, 0));
                    }
                    break;
            }
        }

        public string GetRequestRewardInfo(byte Index)
        {
            if (Index > 2) Index = 2;
            string Text = "";
            if(RequestRewards[Index].item.type != 0)
            {
                Text = RequestRewards[Index].item.HoverName;
            }
            int p = 0, g = 0, s = 0, c = RequestRewards[Index].Value;
            if (c > 0)
            {
                if(Text != "") Text += " and ";
                if (c >= 100)
                {
                    s += c / 100;
                    c -= s * 100;
                }
                if (s >= 100)
                {
                    g += s / 100;
                    s -= g * 100;
                }
                if (g >= 100)
                {
                    p += g / 100;
                    g -= p * 100;
                }
                bool First = true;
                if (p > 0)
                {
                    Text += p + " platinum";
                    First = false;
                }
                if (g > 0)
                {
                    if (!First)
                        Text += ", ";
                    First = false;
                    Text += g + " gold";
                }
                if (s > 0)
                {
                    if (!First)
                        Text += ", ";
                    First = false;
                    Text += s + " silver";
                }
                if (c > 0)
                {
                    if (!First)
                        Text += ", ";
                    First = false;
                    Text += c + " copper";
                }
                Text += " coins";
            }
            if (Text == "") Text = "Nothing";
            else Text += ".";
            return Text;
        }

        public void GiveAtLeast30SecondsRequestTime()
        {
            if (LifeTime < 30 * 60)
                LifeTime = 30 * 60;
        }

        public bool PickNewRequest(Player player, CompanionData companion)
        {
            int NewID;
            string NewModID;
            if (RequestContainer.GetAnyPossibleRequest(player, companion, out NewID, out NewModID))
            {
                ChangeRequest(NewID, NewModID);
                status = RequestStatus.WaitingAccept;
                LifeTime = Main.rand.Next(72 * 3600, 120 * 3600);
                GetNewRequestRewards(player, companion);
                return true;
            }
            SetRequestOnCooldown(true);
            return false;
        }

        public void GetNewRequestRewards(Player player, CompanionData companion)
        {
            float GetNothingChance = 0;
            float VariationStart = 1, VariationRange = 0.6f;
            if(companion.FriendshipLevel < 1)
            {
                GetNothingChance = 0.5f;
                VariationStart = 0.5f;
                VariationRange = 0.3f;
            }
            else if(companion.FriendshipLevel < 3)
            {
                GetNothingChance = 0.25f;
                VariationStart = 0.7f;
                VariationRange = 0.4f;
            }
            else if(companion.FriendshipLevel < 5)
            {
                GetNothingChance = 0.15f;
                VariationStart = 0.9f;
                VariationRange = 0.45f;
            }
            else if(companion.FriendshipLevel < 7)
            {
                GetNothingChance = 0.05f;
                VariationStart = 0.9f;
                VariationRange = 0.5f;
            }
            RequestReward[] Rewards = RequestReward.GetPossibleRewards(player, companion, 3, GetNothingChance);
            for(int i = 0; i < 3; i++)
            {
                if(Rewards[i] == null)
                {
                    RequestRewards[i].item.SetDefaults(0);
                }
                else
                {
                    RequestRewards[i].item.SetDefaults(Rewards[i].ID);
                    RequestRewards[i].item.stack = Rewards[i].Stack;
                }
                float RewardValue = Base.RewardValue * (1f + companion.FriendshipLevel * 0.1f) * (VariationStart + VariationRange * Main.rand.NextFloat());
                RequestRewards[i].Value = (int)RewardValue;
            }
        }

        public bool CompleteRequest(Player player, CompanionData companion, byte PickedReward)
        {
            if (!Base.IsRequestCompleted(this)) return false;
            if(PickedReward > 2) PickedReward = 2;
            Base.OnCompleteRequest(player, this);
            SetRequestOnCooldown();
            companion.FriendshipProgress.ChangeFriendshipProgress(1);
            if(RequestRewards[PickedReward].item.type > 0)
                Item.NewItem(new EntitySource_Gift(player), player.Center, Microsoft.Xna.Framework.Vector2.Zero, RequestRewards[PickedReward].item.type, RequestRewards[PickedReward].item.stack);
            int p = 0, g = 0, s = 0, c = RequestRewards[PickedReward].Value;
            if(c >= 100)
            {
                s += c / 100;
                c -= s * 100;
            }
            if(s >= 100)
            {
                g += s / 100;
                s -= g * 100;
            }
            if(g >= 100)
            {
                p += g / 100;
                g -= p * 100;
            }
            if (p > 0)
            {
                Item.NewItem(new EntitySource_Gift(player), player.Center, Microsoft.Xna.Framework.Vector2.Zero, Terraria.ID.ItemID.PlatinumCoin, p);
            }
            if (g > 0)
            {
                Item.NewItem(new EntitySource_Gift(player), player.Center, Microsoft.Xna.Framework.Vector2.Zero, Terraria.ID.ItemID.GoldCoin, g);
            }
            if (s > 0)
            {
                Item.NewItem(new EntitySource_Gift(player), player.Center, Microsoft.Xna.Framework.Vector2.Zero, Terraria.ID.ItemID.SilverCoin, s);
            }
            if (c > 0)
            {
                Item.NewItem(new EntitySource_Gift(player), player.Center, Microsoft.Xna.Framework.Vector2.Zero, Terraria.ID.ItemID.CopperCoin, c);
            }
            return true;
        }

        public void OnKillNpc(NPC npc)
        {
            if(!IsActive) return;
            Base.OnKillNpc(npc, this);
        }

        public void ChangeRequestStatus(RequestStatus NewStatus)
        {
            status = NewStatus;
        }

        public void SetRequestOnCooldown(bool Shorter = false)
        {
            status = RequestStatus.Cooldown;
            LifeTime = Main.rand.Next(16 * 3600, 24 * 3600);
            if(Shorter) LifeTime /= 2;
        }

        public void Save(uint UniqueID, TagCompound tag)
        {
            tag.Add("RequestID_" + UniqueID, RequestID);
            tag.Add("RequestModID_" + UniqueID, RequestModID);
            tag.Add("RequestIsValid_" + UniqueID, ValidRequest);
            tag.Add("RequestLifetime_" + UniqueID, LifeTime);
            if (ValidRequest)
            {
                tag.Add("RequestStatus_" + UniqueID, (byte)status);
                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);
                {
                    progress.Save(writer);
                }
                stream.Position = 0;
                tag.Add("RequestProgressData_" + UniqueID, stream.ReadBytes(stream.Length));
                writer.Close();
                stream.Close();
                for(int i = 0; i < 3; i++)
                {
                    tag.Add("RequestRewardItem_" + i + "_" + UniqueID, RequestRewards[i].item);
                    tag.Add("RequestRewardValue_" + i + "_" + UniqueID, RequestRewards[i].Value);
                }
            }
        }

        public void Load(uint UniqueID, uint SaveVersion, TagCompound tag)
        {
            int RequestID = tag.GetInt("RequestID_" + UniqueID);
            string RequestModID = tag.GetString("RequestModID_" + UniqueID);
            ChangeRequest(RequestID, RequestModID);
            bool IsValidRequest = tag.GetBool("RequestIsValid_" + UniqueID);
            LifeTime = tag.GetInt("RequestLifetime_" + UniqueID);
            if (IsValidRequest)
            {
                status = (RequestStatus)tag.GetByte("RequestStatus_" + UniqueID);
                MemoryStream stream = new MemoryStream();
                stream.Write(tag.GetByteArray("RequestProgressData_" + UniqueID));
                stream.Position = 0;
                if (ValidRequest)
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        progress.Load(reader);
                    }
                }
                for(int i = 0; i < 3; i++)
                {
                    RequestRewards[i].item = tag.Get<Item>("RequestRewardItem_" + i + "_" + UniqueID);
                    RequestRewards[i].Value = tag.GetInt("RequestRewardValue_" + i + "_" + UniqueID);
                }
            }
        }

        public class Reward
        {
            public int Value = 0;
            public Item item = new Item();

            public Reward()
            {
                
            }

            public Reward(int Value, int ItemID, int Stack = 1)
            {
                this.Value = Value;
                item.SetDefaults(ItemID);
                item.stack = Stack;
            }
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