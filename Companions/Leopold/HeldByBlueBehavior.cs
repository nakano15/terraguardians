using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Leopold
{
    public class HeldByBlueBehavior : BehaviorBase
    {
        TerraGuardian Blue = null;
        ushort HeldTime = 0;

        public HeldByBlueBehavior(TerraGuardian companion)
        {
            if (!companion.IsSameID(CompanionDB.Blue))
            {
                Deactivate();
                return;
            }
            Blue = companion;
            HeldTime = (ushort)(Main.rand.Next(90, 180) * 60);
        }

        public override void ChangeLobbyDialogueOptions(MessageDialogue Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
            Message.AddOption("I wanted to talk with " + Blue.GetNameColored() + ".", OnAskToTalkWithBlue);
            Message.AddOption("Help him out.", OnHelpLeopoldOut);
        }

        void OnHelpLeopoldOut()
        {
            Dialogue.LobbyDialogue("*Thank you. She was holding me too tight.*");
            Deactivate();
        }

        void OnAskToTalkWithBlue()
        {
            if (Blue != null)
            {
                GetOwner.SaySomething("*I thought you was going to help me here!*");
                Dialogue.StartDialogue(Blue);
            }
        }

        public override void Update(Companion companion)
        {
            if(Blue == null || !Blue.active || Blue.dead || Blue.KnockoutStates > KnockoutStates.Awake || companion.Owner != null)
            {
                Deactivate();
                return;
            }
            DrawOrderInfo.AddDrawOrderInfo(Blue, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
            AffectCompanion(Blue);
            Vector2 Position = Vector2.Zero;
            RunCombatBehavior = Blue.BodyFrameID != 23;
            switch(Blue.BodyFrameID)
            {
                default:
                    Position.X = (3 - 6) * Blue.direction;
                    Position.Y = -6 + 8;
                    break;
                case 23:
                    Position.X = (14 - 6) * Blue.direction;
                    Position.Y = 22 + 8;
                    break;
                case 24:
                case 26:
                    Position.X = (3 - 2) * Blue.direction;
                    Position.Y = -6 + 18;
                    break;
                case 31:
                    Position.X = -Blue.direction * (7 - 6);
                    Position.Y = 6 + 8;
                    break;
                case 30:
                    Position.X = Blue.direction * (-6);
                    Position.Y = 16 + 8;
                    break;
            }
            Position.Y = -64 + Position.Y;
            if (companion.itemAnimation <= 0)
                companion.direction = Blue.direction;
            companion.position = Blue.Bottom + Position * Blue.Scale - (Blue.Base.Scale - Blue.Scale) * new Vector2(0.1f, 1f) * 24;
            if (companion.direction < 0)
                companion.position.X -= companion.width;
            companion.position.Y += Blue.gfxOffY;
            companion.velocity.Y = 0;
            companion.velocity.X = 0;
            companion.gfxOffY = 0;
            if (Blue.whoAmI < companion.whoAmI)
            {
                companion.position += Blue.velocity;
            }
            companion.SetFallStart();
            companion.MoveUp = companion.MoveDown = companion.MoveLeft = companion.MoveRight = false;
            companion.ControlJump = false;
            HeldTime--;
            if (HeldTime == 0)
            {
                Deactivate();
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            short FrameID = 29;
            switch(Blue.BodyFrameID)
            {
                case 23:
                    FrameID = 21;
                    break;
                case 26:
                    FrameID = 24;
                    break;
                case 31:
                    FrameID = 15;
                    break;
                case 30:
                    FrameID = 16;
                    break;
            }
            companion.BodyFrameID = FrameID;
            TerraGuardian tg = (TerraGuardian)companion;
            for(int i = 0; i < tg.ArmFramesID.Length; i++)
            {
                if (tg.HeldItems[i].ItemAnimation <= 0)
                {
                    tg.ArmFramesID[i] = FrameID;
                }
            }
        }

        public override void UpdateAffectedCompanionAnimationFrame(Companion companion)
        {
            BlueBase.ApplyHeldBunnyAnimation((TerraGuardian)companion, true);
        }

        public override void OnEnd()
        {
            if (Blue != null && GetOwner != null)
            {
                GetOwner.Teleport(Blue);
            }
        }

        public override void ChangeDrawMoment(Companion companion, ref CompanionDrawMomentTypes DrawMomentType)
        {
            if (Blue.IsBeingControlledBySomeone)
                DrawMomentType = CompanionDrawMomentTypes.DrawInBetweenOwner;
        }
    }
}