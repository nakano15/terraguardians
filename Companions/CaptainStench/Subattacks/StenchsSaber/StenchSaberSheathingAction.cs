using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;

namespace terraguardians.Companions.CaptainStench.Subattacks
{
    public class StenchSaberSheathingAction : SubAttackBase
    {
        public override string Name => "Sheathing Sword";
        public override string Description => "Stench will Sheath/Unsheath her sword.\nShe need to have her sword unsheathed to have it ready for combat.";
        public override float Cooldown => base.Cooldown;

        bool IsSheathing = false;

        public override void OnBeginUse(Companion User, SubAttackData Data)
        {
            IsSheathing = (User as CaptainStenchBase.StenchCompanion).HoldingWeapon;
            
        }
    }
}