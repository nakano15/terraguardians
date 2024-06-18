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

        const int Duration = 9 * 6;

        public override void OnBeginUse(Companion User, SubAttackData Data)
        {
            IsSheathing = (User as CaptainStenchBase.StenchCompanion).HoldingWeapon;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTime >= Duration)
            {
                (User as CaptainStenchBase.StenchCompanion).HoldingWeapon = !IsSheathing;
                Data.EndUse();
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            short Frame = (short)(82 + Math.Clamp(MathF.Abs((IsSheathing ? -8 : 0) + Data.GetTime * (1f / 6)), 0, 8));
            //User.BodyFrameID = Frame;
            for (int i = 0; i < 2; i++)
                User.ArmFramesID[i] = Frame;
        }
    }
}