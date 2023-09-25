using Terraria;
using terraguardians;

namespace terraguardians.Companions.Castella
{
    public class CastellaMetamorphose : BehaviorBase
    {
        private bool WereTransform = false;
        byte Step = 0;
        int Time = 0;

        public override void Update(Companion companion)
        {
            if (Step > 0)
            {
                companion.MoveLeft = companion.MoveRight = 
                companion.MoveUp = companion.MoveDown =
                companion.controlJump = false;
                RunCombatBehavior = false;
                AllowSeekingTargets = false;
            }
            switch (Step)
            {
                case 0:
                    if (companion.velocity.Y == 0)
                    {
                        WereTransform = (companion as CastellaCompanion).OnWerewolfForm;
                        ChangeStep();
                    }
                    break;
                case 1:
                    if (Time >= 11)
                    {
                        ChangeStep();
                    }
                    break;
                case 2:
                case 3:
                case 4:
                    if (Time >= 30)
                    {
                        ChangeStep();
                    }
                    break;
                case 5:
                    if (Time >= 15)
                    {
                        Deactivate();
                    }
                    break;
            }
            Time++;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            short Frame;
            switch(Step)
            {
                default:
                    if (companion.velocity.Y != 0)
                        Frame = WereTransform ? (short)9 : (short)38;
                    else
                        Frame = WereTransform ? (short)0 : (short)29;
                    break;
                case 1:
                    Frame = WereTransform ? (short)55 : (short)59;
                    break;
                case 2:
                    Frame = WereTransform ? (short)56 : (short)58;
                    break;
                case 3:
                    Frame = (short)57;
                    break;
                case 4:
                    Frame = WereTransform ? (short)58 : (short)56;
                    break;
                case 5:
                    Frame = WereTransform ? (short)59 : (short)55;
                    break;
            }
            companion.BodyFrameID = companion.ArmFramesID[0] = 
            companion.ArmFramesID[1] = Frame;
        }

        private void ChangeStep()
        {
            Step++;
            Time = 0;
        }
    }
}