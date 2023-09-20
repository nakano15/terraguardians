using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class PreRecruitBehavior : BehaviorBase
    {
        public bool Wandering = false;
        public short ActionTime = 0;
        public Player Target = null;
        byte TargetCheckDelay = 0;
        public bool FollowPlayers = true;
        public bool NoticePlayers = true;

        public PreRecruitBehavior()
        {
            TargetCheckDelay = (byte)Main.rand.Next(10);
        }

        public override void Update(Companion companion)
        {
            WanderAI(companion);
        }

        public void WanderAI(Companion companion)
        {
            if (companion.IsBeingControlledBySomeone || companion.KnockoutStates > 0 || Companion.Behavior_RevivingSomeone) return;
            if(Target == null)
            {
                ActionTime--;
                if (Wandering)
                {
                    MoveTowardsDirection(companion);
                    companion.WalkMode = true;
                    if (ActionTime <= 0)
                    {
                        Wandering = false;
                        ActionTime = (short)Main.rand.Next(200, 401);
                    }
                }
                else
                {
                    if (ActionTime <= 0)
                    {
                        Wandering = Main.rand.NextFloat() < 0.6f;
                        ActionTime = (short)Main.rand.Next(200, 401);
                        if (!Wandering)
                            companion.direction *= -1;
                    }
                }
                if (NoticePlayers && TargetCheckDelay == 0)
                {
                    Player NearestPlayer = null;
                    float NearestDistance = 300;
                    for(int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active && !Main.player[p].dead && !Main.player[p].ghost && !(Main.player[p] is Companion) && ((companion.direction > 0 && companion.position.X < Main.player[p].position.X) || (companion.direction < 0 && companion.position.X > Main.player[p].position.X)))
                        {
                            float Distance = Main.player[p].Distance(companion.Center);
                            if (Distance < NearestDistance)
                            {
                                NearestPlayer = Main.player[p];
                                NearestDistance = Distance;
                            }
                        }
                    }
                    if (NearestPlayer != null)
                    {
                        Target = NearestPlayer;
                        ActionTime = (short)Main.rand.Next(100, 201);
                        Wandering = false;
                        if (PlayerMod.PlayerHasCompanion(Target, companion))
                        {
                            companion.SpawnEmote(EmoteID.EmotionAlert, 180);
                        }
                        else
                        {
                            companion.SpawnEmote(EmoteID.EmoteConfused, 180);
                        }
                    }
                    TargetCheckDelay += 10;
                }
                TargetCheckDelay--;
            }
            else
            {
                if (!Target.active || Target.dead || Target.Distance(companion.Center) >= 450)
                {
                    Target = null;
                    Wandering = false;
                    ActionTime = (short)Main.rand.Next(100, 201);
                    return;
                }
                if (FollowPlayers && Wandering && System.MathF.Abs(companion.Center.X - Target.Center.X) >= 120)
                {
                    int direction = 1;
                    if(Target.Center.X < companion.Center.X)
                        direction = -1;
                    MoveTowardsDirection(companion, direction);
                    companion.WalkMode = true;
                }
                else if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                {
                    if(Target.Center.X < companion.Center.X)
                        companion.direction = -1;
                    else
                        companion.direction = 1;
                }
                ActionTime--;
                if (ActionTime <= 0)
                {
                    ActionTime += (short)(500 + Main.rand.Next(100, 201));
                    Wandering = Main.rand.NextFloat() < 0.4f;
                    if (PlayerMod.PlayerHasCompanion(Target, companion))
                    {
                        companion.SpawnEmote(Main.rand.NextFloat() < 0.5f ? EmoteID.RPSPaper : EmoteID.EmoteHappiness, 180);
                    }
                    else
                    {
                        companion.SpawnEmote(Main.rand.NextFloat() < 0.5f ? EmoteID.RPSPaper : EmoteID.DebuffSilence, 180);
                    }
                }
            }
        }
    }
}