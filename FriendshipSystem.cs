using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;

namespace terraguardians
{
    public class FriendshipSystem
    {
        public byte Level = 0;
        public sbyte Progress = 0;
        protected float ComfortStack = 0;
        protected byte ComfortPoints = 0;
        public const int MaxComfortStack = 600;
        private float TravellingStack = 0;
        public byte MaxComfortPoints { get { return checked((byte)(5 + Level * 0.3333)); } }

        public float GetComfortStack { get { return ComfortStack; } }
        public byte GetComfortPoints { get { return ComfortPoints; } }
        public float MaxTravellingStack { get { return 50000 + Level; } }

        public byte MaxProgress
        {
            get
            {
                return (byte)Math.Clamp((1 + Level * 0.5f) * (Level > 0 ? 2 : 1), 1, sbyte.MaxValue);
            }
        }

        public void ChangeComfortProgress(float Change)
        {
            if (ComfortPoints >= MaxComfortPoints)
                return;
            ComfortStack += Change;
            if(Change > 0)
            {
                if(ComfortStack >= MaxComfortStack)
                {
                    ComfortStack -= MaxComfortStack;
                    ComfortPoints++;
                    /*if(ComfortPoints >= MaxComfortPoints)
                    {
                        ComfortPoints -= MaxComfortPoints;
                        ChangeFriendshipProgress(1);
                    }*/
                }
                return;
            }
            if (Change < 0 && ComfortStack < 0) ComfortStack = 0;
        }

        public bool IncreaseTravellingStack(float Speed)
        {
            if (Level == 0) return false;
            TravellingStack += Math.Abs(Speed);
            if (TravellingStack >= MaxTravellingStack)
            {
                TravellingStack -= MaxTravellingStack;
                ChangeFriendshipProgress(1);
                return true;
            }
            return false;
        }

        public bool IsComfortMaxed()
        {
            if(ComfortPoints >= MaxComfortPoints)
            {
                ComfortPoints -= MaxComfortPoints;
                ChangeFriendshipProgress(1);
                return true;
            }
            return false;
        }

        public bool ChangeFriendshipProgress(sbyte Change)
        {
            Progress = (sbyte)Math.Clamp((int)Progress + Change, sbyte.MinValue, sbyte.MaxValue);
            if(Math.Abs(Progress) >= MaxProgress)
            {
                if (Progress < 0)
                {
                    if(Level > 0)
                    {
                        Progress += (sbyte)MaxProgress;
                        Level--;
                    }
                    else
                    {
                        Progress = 0;
                    }
                }
                else
                {
                    if(Level < byte.MaxValue)
                    {
                        Progress -= (sbyte)MaxProgress;
                        Level++;
                    }
                }
                return true;
            }
            return false;
        }

        public void Save(TagCompound tag, uint UniqueID)
        {
            tag.Add("FriendshipLevel_" + UniqueID, Level);
            tag.Add("FriendshipProgress_" + UniqueID, (short)Progress);
            tag.Add("ComfortStack_" + UniqueID, ComfortStack);
            tag.Add("ComfortPoints_" + UniqueID, ComfortPoints);
            tag.Add("TravellingStack_" + UniqueID, TravellingStack);
        }

        public void Load(TagCompound tag, uint UniqueID, uint Version)
        {
            Level = tag.GetByte("FriendshipLevel_" + UniqueID);
            Progress = (sbyte)tag.GetShort("FriendshipProgress_" + UniqueID);
            if(Version >= 3)
            {
                ComfortStack = tag.GetFloat("ComfortStack_" + UniqueID);
                ComfortPoints = tag.GetByte("ComfortPoints_" + UniqueID);
            }
            if (Version >= 9)
                TravellingStack = tag.GetFloat("TravellingStack_" + UniqueID);
        }
    }
}