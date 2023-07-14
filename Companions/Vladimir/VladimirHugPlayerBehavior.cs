using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Companions.Vladimir
{
    public class VladimirHugPlayerBehavior : BehaviorBase
    {
        public int ChatTime = 0;
        public ushort BuffRefreshTime = 0;
        public byte FriendshipPoints = 0, BuffRefreshStack = 0;
        public Player Target = null;

        public VladimirHugPlayerBehavior(TerraGuardian Vladimir, Player Target)
        {
            if (!Vladimir.IsSameID(CompanionDB.Vladimir))
            {
                Deactivate();
                return;
            }
            this.Target = Target;
            ChatTime = 300;
            //(Vladimir.Base as VladimirBase).CarrySomeoneAction(Vladimir, (VladimirData)Vladimir.Data, Target, InstantPickup: true);
        }

        public override void Update(Companion companion)
        {
            companion.idleBehavior.Update(companion);
            const ushort MaxBuffRefreshTime = 10 * 60;
            BuffRefreshTime++;
            DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
            if (BuffRefreshTime >= MaxBuffRefreshTime)
            {
                BuffRefreshTime -= MaxBuffRefreshTime;
                if (BuffRefreshStack < 3) BuffRefreshStack++;
                Target.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * BuffRefreshStack);
                Companion controlled = PlayerMod.PlayerGetControlledCompanion(Target);
                if (controlled != null)
                {
                    controlled.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * BuffRefreshStack);
                }
            }
            VladimirData data = (VladimirData)companion.Data;
            bool End = Target.controlJump;
            if (Main.bloodMoon)
                End = true;
            if (End)
            {
                Deactivate();
                Target.Bottom = companion.Bottom;
                Companion controlled = PlayerMod.PlayerGetControlledCompanion(Target);
                if (controlled != null)
                {
                    controlled.Bottom = Target.Bottom;
                }
                companion.SaySomething((companion.GetDialogues as VladimirDialogues).GetEndHugMessage((TerraGuardian)companion));
                return;
            }
            else
            {
                bool FaceBear = (companion.BodyFrameID != 20 && companion.BodyFrameID != 21) || companion.BodyFrameID == 25;
                Player Character = PlayerMod.PlayerGetControlledCompanion(Target);
                if (Character == null) Character = Target;
                if (Character.mount.Active)
                    Character.mount.Dismount(Character);
                TerraGuardian Vladimir = (TerraGuardian)companion;
                Character.position = Vladimir.GetMountShoulderPosition;
                Character.position.X -= Character.width * 0.5f;
                Character.position.Y -= Character.height * 0.5f;
                if (data.CarrySomeone)
                    Character.position.X -= 6 * companion.direction;
                Character.gfxOffY = 0;
                Character.velocity.X = 0;
                Character.velocity.Y = -Player.defaultGravity;
                Character.fallStart = (int)(Character.position.Y * Companion.DivisionBy16);
                if (Character.itemAnimation == 0)
                    Character.ChangeDir(companion.direction * (FaceBear ? -1 : 1));
                Character.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                if (PlayerMod.GetPlayerKnockoutState(Character) > KnockoutStates.Awake)
                    Character.GetModPlayer<PlayerMod>().ChangeReviveStack(3);
                if (!Main.bloodMoon)
                {
                    Character.immuneTime = 3;
                    Character.immuneNoBlink = true;
                }
                if ((companion.chatOverhead.timeLeft == 0 || Main.bloodMoon) && (Character.controlLeft || Character.controlRight || Character.controlUp || Character.controlDown || (Main.bloodMoon && Character.controlJump)))
                {
                    if (Main.bloodMoon)
                    {
                        Character.controlJump = false;
                        byte CharacterHurtState = 0;
                        const byte Hurt = 1, Defeated = 2;
                        if (Character.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Character.name + " were crushed by " + companion.name + "'s arms."), (int)(Character.statLifeMax2 * 0.22f), companion.direction) != 0)
                        {
                            if (Character.dead || PlayerMod.GetPlayerKnockoutState(Character) > KnockoutStates.Awake)
                                CharacterHurtState = Defeated;
                            else
                                CharacterHurtState = Hurt;
                        }
                        switch(CharacterHurtState)
                        {
                            case Hurt:
                                if (companion.BodyFrameID == companion.Base.GetAnimation(AnimationTypes.ChairSittingFrames).GetFrame(0))
                                {
                                    switch (Main.rand.Next(5))
                                    {
                                        default:
                                            companion.SaySomething("*I'll crush you if you move again!*");
                                            break;
                                        case 1:
                                            companion.SaySomething("*I'll hurt you worser than those monsters would If you keep moving!*");
                                            break;
                                        case 2:
                                            companion.SaySomething("*I can crush you with my arms or my legs, you pick!*");
                                            break;
                                        case 3:
                                            companion.SaySomething("*Want me to turn your bones to dust?*");
                                            break;
                                        case 4:
                                            companion.SaySomething("*You are angering me, more than this night does!*");
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Main.rand.Next(5))
                                    {
                                        default:
                                            companion.SaySomething("*Stay quiet!*");
                                            break;
                                        case 1:
                                            companion.SaySomething("*I'll crush your bones If you continue doing that!*");
                                            break;
                                        case 2:
                                            companion.SaySomething("*I have my arms around you, I can pull them against my body, and you wont like it!*");
                                            break;
                                        case 3:
                                            companion.SaySomething("*Want to try that again?*");
                                            break;
                                        case 4:
                                            companion.SaySomething("*This is what you want?*");
                                            break;
                                    }
                                }
                                break;
                            case Defeated:
                                switch (Main.rand.Next(5))
                                {
                                    default:
                                        companion.SaySomething("*Finally! You got quiet!*");
                                        break;
                                    case 1:
                                        companion.SaySomething("*See what you made me do?!*");
                                        break;
                                    case 2:
                                        companion.SaySomething("*My mood is already bad, you didn't helped either!*");
                                        break;
                                    case 3:
                                        companion.SaySomething("*At least you stopped moving around!*");
                                        break;
                                    case 4:
                                        companion.SaySomething("*You behave better when unconscious!*");
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        companion.SaySomething("*Press Jump button If that's enough.*");
                    }
                }
                if (Target.whoAmI == Main.myPlayer)
                {
                    ChatTime--;
                    if (ChatTime <= 0)
                    {
                        ChatTime += 600;
                        FriendshipPoints++;
                        if (FriendshipPoints >= 10 + companion.FriendshipLevel / 3)
                        {
                            FriendshipPoints = 0;
                            companion.IncreaseFriendshipPoint(1);
                        }
                        /*string Message;
                        if (Main.rand.Next(10) == 0)
                            Message = companion.GetDialogues.TalkMessages(companion);
                        else
                            Message = companion.GetDialogues.NormalMessages(companion);*/
                    }
                }
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            short Frame = 1;
            switch (companion.BodyFrameID)
            {
                case 11:
                    Frame = 12;
                    break;
                case 22:
                    Frame = 23;
                    break;
                case 24:
                    Frame = 25;
                    break;
                case 27:
                    Frame = 28;
                    break;
                case 29:
                    Frame = 30;
                    break;
                case 31:
                    Frame = 32;
                    break;
            }
            if (companion.BodyFrameID == 0 || companion.BodyFrameID == 11 || companion.BodyFrameID == 22 || companion.BodyFrameID == 24)
                companion.BodyFrameID = Frame;
            if ((companion as TerraGuardian).HeldItems[1].ItemAnimation == 0)
            {
                companion.ArmFramesID[1] = Frame;
            }
            if (companion.itemAnimation == 0)
            {
                companion.ArmFramesID[0] = Frame;
            }
        }
    }
}