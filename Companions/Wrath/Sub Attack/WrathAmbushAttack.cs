using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace terraguardians.Companions.Wrath
{
    internal class WrathAmbushAttack : SubAttackBase
    {
        public override string Name => "Ambush";
        public override string Description => "Wrath evaporates and materializes themselves ahead of their target, picking them up if they're in contact.";
        public override bool AllowItemUsage => false;
        public override float Cooldown => 60;
        public override SubAttackData GetSubAttackData => new WrathAmbushAttackData();

        public override void OnBeginUse(Companion User, SubAttackData RawData)
        {
            WrathAmbushAttackData Data = (WrathAmbushAttackData)RawData;
            Data.InternalTime = 0;
            Data.TargetGot = false;
            Data.Target = null;
            if (User.Target != null)
            {
                Data.Target = User.Target;
            }
            else
            {
                if (User.GetCharacterControllingMe == MainMod.GetLocalPlayer)
                {
                    Data.Target = GetTargetInAimRange(User);
                }
            }
            if (Data.Target == null) Data.EndUse();
        }

        public override void Update(Companion User, SubAttackData RawData)
        {
            WrathAmbushAttackData Data = (WrathAmbushAttackData)RawData;
            User.immune = true;
            User.immuneNoBlink = true;
            User.immuneTime = 60;
            if (Data.Target == null || !Data.Target.active || (Data.Target is Player && (Data.Target as Player).dead))
            {
                Data.Target = null;
                Data.EndUse();
                return;
            }
            User.MoveLeft = User.MoveRight = User.ControlJump = User.MoveDown = false;
            if (Data.InternalTime == 0)
            {
                User.direction = Data.Target.Center.X < User.Center.X ? -1 : 1;
            }
            if (Data.InternalTime < 60)
            {
                Data.InternalTime++;
            }
            else if (Data.InternalTime == 60)
            {
                for (int Attempts = 1; Attempts >= -1; Attempts -= 2)
                {
                    Vector2 TargetPos = Data.Target.position;
                    TargetPos.X += (Data.Target.velocity.X < 0 ? -1 : 1) * Attempts * 32 + Data.Target.velocity.X;
                    if (WrathDestructiveRushAttack.CanRushThrough(ref TargetPos, -Data.Target.direction > 0))
                    {
                        if (User.Center.X < Data.Target.Center.X)
                            User.direction = -1;
                        else
                            User.direction = 1;
                        User.position = TargetPos;
                        User.position.Y += Data.Target.height - User.height;
                        User.SetFallStart();
                        Data.InternalTime++;
                        break;
                    }
                }
            }
            else if((!Data.TargetGot && Data.InternalTime < 120) || (Data.TargetGot && Data.InternalTime < 240))
            {
                Data.InternalTime++;
                retry:
                if (!Data.TargetGot)
                {
                    if ((Data.Target is NPC || (Data.Target is Player && User.IsHostileTo((Player)Data.Target))) && Data.Target.Hitbox.Intersects(User.Hitbox))
                    {
                        Data.TargetGot = true;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                User.SaySomething("*WHERE ARE YOU GOING?*");
                                break;
                            case 1:
                                User.SaySomething("*THERES NO ESCAPE!*");
                                break;
                            case 2:
                                User.SaySomething("*I GOT YOU!*");
                                break;
                        }
                        goto retry;
                    }
                }
                else
                {
                    Vector2 BindPosition = User.position;
                    BindPosition.X += User.width * .5f + (User.width * .5f + 6) * User.direction;
                    BindPosition.Y += 4;
                    Data.Target.Center = BindPosition;
                    if(Data.Target is Player && ((Player)Data.Target).itemAnimation == 0)
                    {
                        Data.Target.direction = -User.direction;
                    }
                    DrawOrderInfo.AddDrawOrderInfo(Data.Target, User, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                }
            }
            else
            {
                Data.EndUse();
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData RawData)
        {
            WrathAmbushAttackData Data = (WrathAmbushAttackData)RawData;
            if (Data.InternalTime > 60 && Data.TargetGot)
            {
                User.ArmFramesID[0] = User.ArmFramesID[1] = 11;
                if ((User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm)
                {
                    User.BodyFrameID = 19;
                }
            }
        }

        public override void PreDraw(Companion User, SubAttackData RawData, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            WrathAmbushAttackData Data = (WrathAmbushAttackData)RawData;
            bool CloudForm = (User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm;
            float Alpha = 0;
            if (Data.InternalTime > 60)
            {
                Alpha = (float)(Data.InternalTime - 60) * (1f / 60);
            }
            else if (Data.InternalTime < 60)
            {
                Alpha = (float)(60 - Data.InternalTime) * (1f / 60);
            }
            Holder.DrawColor *= Math.Clamp(Alpha, 0f, 1f);
            if (Data.InternalTime != 60 && Data.InternalTime < 120 && Main.rand.Next(3) == 0)
            {
                Gore cloud = Gore.NewGoreDirect(User.GetSource_Misc(""), User.Center, new Vector2(1f - Main.rand.NextFloat() * 2, 1f - Main.rand.NextFloat() * 2), Main.rand.Next(11, 14));
                cloud.alpha = 120;
            }
        }

        public class WrathAmbushAttackData : SubAttackData
        {
            public Entity Target;
            public short InternalTime = 0;
            public bool TargetGot = false;
        }
    }
}
