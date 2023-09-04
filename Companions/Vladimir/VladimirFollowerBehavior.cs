using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Companions.Vladimir
{
    public class VladimirFollowerBehavior : FollowLeaderBehavior
    {
        public VladimirFollowerBehavior() : base()
        {

        }

        public override void Update(Companion companion)
        {
            Vladimir.VladimirCompanion data = (Vladimir.VladimirCompanion)companion;
            if (data.CarrySomeone && data.PickedUpPerson && data.CarriedCharacter == companion.Owner)
            {
                return;
            }
            UpdateFollow(companion);
        }
    }
}