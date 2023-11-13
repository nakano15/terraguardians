using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Wrath
{
    internal class WrathDestructiveRushAttack : SubAttackBase
    {
        public override string Name => "Destructive Rush";
        public override string Description => "Rushes in a direction on front of the character, hitting any foe in the way.";
        public override bool AllowItemUsage => false;
        public override float Cooldown => base.Cooldown;

        public override void Update(Companion User, SubAttackData Data)
        {
            User.MoveLeft = User.MoveRight = User.ControlJump = User.MoveDown = false;
            if (Data.GetTime == 10)
            {
                bool MovingLeft = User.direction < 0;
                if (!User.IsBeingControlledBy(MainMod.GetLocalPlayer) && User.Target != null)
                {
                    MovingLeft = User.Center.X > User.Target.Center.X;
                }
                //DO the destructive rush of death.
            }
        }

        public override void UpdateStatus(Companion User, SubAttackData Data)
        {
            User.noKnockback = true;
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            
        }
    }
}