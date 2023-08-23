using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class VladimirData : CompanionData
    {
        public bool PickedUpPerson = false, CarrySomeone = false;
        public Entity CarriedCharacter = null;
        public int Duration = 0, Time = 0;
        public bool WasFollowingPlayerBefore = false;
    }
}