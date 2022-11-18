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

        public byte MaxProgress
        {
            get
            {
                return (byte)Math.Clamp((1 + Level * 0.5f) * (Level > 0 ? 2 : 1), 1, sbyte.MaxValue);
            }
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
        }

        public void Load(TagCompound tag, uint UniqueID, uint Version)
        {
            Level = tag.GetByte("FriendshipLevel_" + UniqueID);
            Progress = (sbyte)tag.GetShort("FriendshipProgress_" + UniqueID);
        }
    }
}