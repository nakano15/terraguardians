using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class IdleBehavior : BehaviorBase
    {
        public IdleStates CurrentState = IdleStates.Waiting;
        public int IdleTime = 0;

        public override void Update(Companion companion)
        {
            UpdateIdle(companion);
        }

        public void UpdateIdle(Companion companion)
        {
            if(Companion.Behaviour_AttackingSomething || Companion.Behaviour_InDialogue)
                return;
            switch(CurrentState)
            {
                case IdleStates.Waiting:
                    {
                        IdleTime--;
                        if(IdleTime <= 0)
                        {
                            if(Main.rand.Next(3) == 0)
                            {
                                ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                                if(companion.velocity.X == 0 && companion.velocity.Y == 0)
                                {
                                    companion.direction *= -1;
                                }
                            }
                            else
                            {
                                ChangeIdleState(IdleStates.Wandering, Main.rand.Next(200, 601));
                            }
                        }
                    }
                    break;
                case IdleStates.Wandering:
                    {
                        IdleTime --;
                        if(IdleTime <= 0)
                        {
                            ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                        }
                        else
                        {
                            companion.WalkMode = true;
                            if(companion.direction > 0)
                                companion.MoveRight = true;
                            else
                                companion.MoveLeft = true;
                        }
                    }
                    break;
            }
        }

        public void ChangeIdleState(IdleStates NewState, int NewTime)
        {
            CurrentState = NewState;
            IdleTime = NewTime;
        }

        public enum IdleStates : byte
        {
            Waiting,
            Wandering
        }
    }
}