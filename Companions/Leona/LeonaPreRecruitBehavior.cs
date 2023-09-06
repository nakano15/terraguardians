using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Domino
{
    public class LeonaPreRecruitmentBehavior : PreRecruitBehavior
    {
        const int WalkStepsTime = 12;
        bool IsBrutusInGroup = false;
        SceneSteps Step = 0;
        int DialogueTime = 0;
        Player TargetCharacter = null;

        public override void Update(Companion companion)
        {
            if (TargetCharacter != null && (!TargetCharacter.active || TargetCharacter.dead))
            {
                if (TargetCharacter.dead)
                    companion.SaySomething("*They shouldn't have lowered their guard.*");
                else
                    companion.SaySomething("*I'd love chatting more, but whoever that was disappeared.*");
                TargetCharacter = null;
                Step = SceneSteps.IdleWander;
            }
            switch(Step)
            {
                case SceneSteps.WalkingOutOfPortal:
                    companion.WalkMode = true;
                    if (companion.direction < 0)
                        companion.MoveLeft = true;
                    else
                        companion.MoveRight = true;
                    if (DialogueTime >= WalkStepsTime)
                    {
                        DialogueTime = 0;
                        Step++;
                        float NearestDistance = 500;
                        for (int p = 0; p < 255; p++)
                        {
                            if (Main.player[p].active && !Main.player[p].dead && Main.player[p] is not Companion)
                            {
                                float Distance = (Main.player[p].Center - companion.Center).Length();
                                if (Distance < NearestDistance)
                                {
                                    TargetCharacter = Main.player[p];
                                    NearestDistance = Distance;
                                }
                            }
                        }
                        if (TargetCharacter != null)
                        {
                            IsBrutusInGroup = PlayerMod.PlayerHasCompanionSummoned(TargetCharacter, CompanionDB.Brutus);
                            if (IsBrutusInGroup)
                            {
                                companion.SaySomething("*Brutus?! So here's where you were at.*");
                            }
                            else
                            {
                                companion.SaySomething("*Ah, so that's what was outside this portal.*");
                            }
                        }
                        else
                        {
                            companion.SaySomething("*Odd. There's nothing here.*");
                        }
                    }
                    else
                    {
                        DialogueTime++;
                    }
                    break;
                case SceneSteps.Speak1:
                    {
                        DialogueTime++;
                        if (TargetCharacter != null)
                        {
                            companion.FaceSomething(TargetCharacter);
                            if (IsBrutusInGroup)
                            {
                                Companion Brutus = PlayerMod.PlayerGetSummonedCompanion(TargetCharacter, CompanionDB.Brutus);
                                if (Brutus == null)
                                {
                                    companion.SaySomething("*What? Where did Brutus go? Oh well, doesn't matter.*");
                                }
                                else
                                {
                                    if (PlayerMod.PlayerHasCompanion(TargetCharacter, CompanionDB.Domino))
                                    {
                                        Brutus.SaySomething("*Amazing... First Domino, now you too... Couldn't this be worse?*");
                                    }
                                    else
                                    {
                                        Brutus.SaySomething("*Great... Just who I didn't wanted to see.*");
                                    }
                                }
                                Step++;
                                DialogueTime = 0;
                            }
                            else
                            {
                                companion.SaySomething("*I guess you've been busy taking care of the monsters leaving the portal, huh?*");
                                Step++;
                                DialogueTime = 0;
                            }
                        }
                        else
                        {
                            companion.SaySomething("*Well, I guess I'll just explore around then.*");
                            Step = SceneSteps.IdleWander;
                            DialogueTime = 0;
                        }
                        if (DialogueTime >= 180)
                        {
                            Step++;
                            DialogueTime = 0;
                        }
                    }
                    break;
                case SceneSteps.Speak2:
                    {
                        DialogueTime++;
                        if (TargetCharacter != null)
                        {
                            companion.FaceSomething(TargetCharacter);
                            if (IsBrutusInGroup)
                            {
                                Companion Brutus = PlayerMod.PlayerGetSummonedCompanion(TargetCharacter, CompanionDB.Brutus);
                                if (Brutus == null)
                                {
                                    companion.SaySomething("*I found him again, and that's more than enough for me. I am Leona. Expect me to show up more often.*");
                                    Recruit(companion);
                                    return;
                                }
                                else
                                {
                                    if (PlayerMod.PlayerHasCompanion(TargetCharacter, CompanionDB.Domino))
                                    {
                                        companion.SaySomething("*Domino's here too? I guess the tides aren't in your favor here, huh?*");
                                    }
                                    else
                                    {
                                        companion.SaySomething("*Come on, don't be like that. Don't you just miss the old times?*");
                                    }
                                }
                                Step++;
                                DialogueTime = 0;
                            }
                            else
                            {
                                companion.SaySomething("*I actually respect that.*");
                                Step++;
                                DialogueTime = 0;
                            }
                        }
                        else
                        {
                            Step = SceneSteps.IdleWander;
                        }
                    }
                    break;
                case SceneSteps.Speak3:
                    {
                        DialogueTime++;
                        if (TargetCharacter != null)
                        {
                            companion.FaceSomething(TargetCharacter);
                            if (IsBrutusInGroup)
                            {
                                Companion Brutus = PlayerMod.PlayerGetSummonedCompanion(TargetCharacter, CompanionDB.Brutus);
                                if (Brutus == null)
                                {
                                    companion.SaySomething("*And he's gone... Oh well, at least I managed to find him again. Lets add a bit of Leona to his life.*");
                                    Recruit(companion);
                                    return;
                                }
                                else
                                {
                                    if (PlayerMod.PlayerHasCompanion(TargetCharacter, CompanionDB.Domino))
                                    {
                                        Brutus.SaySomething("*Sadly, seems like it...*");
                                    }
                                    else
                                    {
                                        Brutus.SaySomething("*No.*");
                                    }
                                }
                                Step++;
                                DialogueTime = 0;
                            }
                            else
                            {
                                companion.SaySomething("*Oh, where are my manners?*");
                                Step++;
                                DialogueTime = 0;
                            }
                        }
                        else
                        {
                            Step = SceneSteps.IdleWander;
                        }
                    }
                    break;
                case SceneSteps.Speak4:
                    {
                        DialogueTime++;
                        if (TargetCharacter != null)
                        {
                            companion.FaceSomething(TargetCharacter);
                            if (IsBrutusInGroup)
                            {
                                Companion Brutus = PlayerMod.PlayerGetSummonedCompanion(TargetCharacter, CompanionDB.Brutus);
                                if (Brutus == null)
                                {
                                    companion.SaySomething("*And he's gone... Oh well, at least I managed to find him again. Lets add a bit of Leona to his life.*");
                                    Recruit(companion);
                                    return;
                                }
                                else
                                {
                                    if (PlayerMod.PlayerHasCompanion(TargetCharacter, CompanionDB.Domino))
                                    {
                                        companion.SaySomething("*Poor, poor Brutus.. Oh well... I guess you'll have to bear my visits, then.*");
                                    }
                                    else
                                    {
                                        companion.SaySomething("*Now that's a rude thing to say to your old colleague.*");
                                    }
                                }
                                Step++;
                                DialogueTime = 0;
                            }
                            else
                            {
                                companion.SaySomething("*You speak with Leona, and I hope you don't mind if I show up sometimes in this world.*");
                                Recruit(companion);
                            }
                        }
                        else
                        {
                            Step = SceneSteps.IdleWander;
                        }
                    }
                    break;
                case SceneSteps.Speak5:
                    {
                        DialogueTime++;
                        if (TargetCharacter != null)
                        {
                            companion.FaceSomething(TargetCharacter);
                            if (IsBrutusInGroup)
                            {
                                Companion Brutus = PlayerMod.PlayerGetSummonedCompanion(TargetCharacter, CompanionDB.Brutus);
                                if (Brutus == null)
                                {
                                    companion.SaySomething("*And he's gone... Oh well, at least I managed to find him again. Lets add a bit of Leona to his life.*");
                                    Recruit(companion);
                                    return;
                                }
                                else
                                {
                                    if (PlayerMod.PlayerHasCompanion(TargetCharacter, CompanionDB.Domino))
                                    {
                                        companion.SaySomething("*Oh, where are my manners? Sorry for ignoring you. I am Leona. I hope you don't mind if I show up sometimes.*");
                                    }
                                    else
                                    {
                                        companion.SaySomething("*Oh, sorry. I got too carried away with meeting an old friend. I am Leona. I hope you don't mind if I show up sometimes.*");
                                    }
                                    Recruit(companion);
                                    return;
                                }
                            }
                            else
                            {
                                companion.SaySomething("*You speak with Leona, and I hope you don't mind if I show up sometimes in this world.*");
                                Recruit(companion);
                            }
                        }
                        else
                        {
                            Step = SceneSteps.IdleWander;
                        }
                    }
                    break;

                case SceneSteps.IdleWander:
                    {
                        if (TargetCharacter != null)
                        {
                            if ((TargetCharacter.Center - companion.Center).Length() > 500)
                            {
                                TargetCharacter = null;
                                companion.SaySomething("*They left. Oh well, back to exploring.*");
                                return;
                            }
                            Wandering = false;
                            if (companion.velocity.X == 0) companion.FaceSomething(TargetCharacter);
                        }
                        else
                        {
                            Wandering = true;
                            TargetCharacter = ViewRangeCheck(companion, companion.direction);
                            if (TargetCharacter != null)
                            {
                                companion.SaySomething("*A new face. Hey, can you understand me? Come speak to me.*");
                            }
                        }
                    }
                    break;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return Step == SceneSteps.IdleWander;
        }

        private void Recruit(Companion leona)
        {
            PlayerMod.PlayerAddCompanion(TargetCharacter, leona);
            WorldMod.AddCompanionMet(leona);
        }

        enum SceneSteps : byte
        {
            WalkingOutOfPortal = 0,
            Speak1 = 1,
            Speak2 = 2,
            Speak3 = 3,
            Speak4 = 4,
            Speak5 = 5,
            IdleWander = 200
        }
    }
}