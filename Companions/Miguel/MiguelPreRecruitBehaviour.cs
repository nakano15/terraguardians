using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Miguel
{
    public class MiguelPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        Player Target = null;
        const byte MaxJumpTimes = 25;
        byte DialogueStep = 0, JumpTimes = 0;
        bool PlayerMovedAway = false, PlayerHasMiguelRecruited = false;

        public override string CompanionNameChange(Companion companion)
        {
            return "Horse Guardian";
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return DialogueStep == 6;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            return ExerciseMessage();
        }

        MessageBase ExerciseMessage()
        {
            return new MessageDialogue("*You forgot what I told you? I told you to jump "+(MaxJumpTimes - JumpTimes)+" times.*");
        }

        public override void Update(Companion companion)
        {
            CheckForPlayerAway(companion);
            switch (DialogueStep)
            {
                case 0:
                    {
                        Target = SeekCharacterInViewRange(companion);
                        if (Target != null)
                        {
                            companion.FaceSomething(Target);
                            DialogueStep = 1;
                            companion.SaySomething("*Hey, you!*");
                            PlayerHasMiguelRecruited = PlayerMod.PlayerHasCompanion(Target, companion);
                        }
                        else
                        {
                            base.Update(companion);
                        }
                    }
                    break;
                case 1:
                    {
                        if (!companion.IsSpeaking)
                        {
                            if (PlayerHasMiguelRecruited)
                            {
                                companion.SaySomething("*Don't you go thinking that just because you changed worlds, you'll slack on the exercises!*");
                            }
                            else
                            {
                                companion.SaySomething("*Are you carrying many loots in that pouch of yours?*");
                            }
                            DialogueStep = 2;
                        }
                    }
                    break;
                case 2:
                    {
                        if (!companion.IsSpeaking)
                        {
                            if (PlayerHasMiguelRecruited)
                            {
                                companion.SaySomething("*I will be here to give you a new batch of exercises.*");
                                WorldMod.AddCompanionMet(companion);
                                Target = null;
                                return;
                            }
                            else
                            {
                                companion.SaySomething("*Or is that because you haven't been doing abdominal exercises?*");
                            }
                            DialogueStep = 3;
                        }
                    }
                    break;
                case 3:
                    {
                        if (!companion.IsSpeaking)
                        {
                            companion.SaySomething("*Hahahaha.*");
                            DialogueStep = 4;
                        }
                    }
                    break;
                case 4:
                    {
                        if (!companion.IsSpeaking)
                        {
                            companion.SaySomething("*Don't worry, though, I will help you take carry of that belly.*");
                            DialogueStep = 5;
                        }
                    }
                    break;
                case 5:
                    {
                        if (!PlayerMovedAway && !companion.IsSpeaking)
                        {
                            companion.SaySomething("*Why don't you try jumping "+MaxJumpTimes+" times.*");
                            DialogueStep = 6;
                        }
                    }
                    break;
                case 6:
                    {
                        if (!PlayerMovedAway && Target.justJumped)
                        {
                            JumpTimes++;
                            if (JumpTimes >= MaxJumpTimes)
                            {
                                companion.SaySomething("*Nice job. As you can see, you got a little more fit for jumping all those times.*");
                                Target.AddBuff(ModContent.BuffType<Buffs.Fit>(), 5 * 60 * 60);
                                DialogueStep = 7;
                            }
                            else
                            {
                                if (JumpTimes % 5 == 0)
                                {
                                    if (JumpTimes > MaxJumpTimes / 2)
                                    {
                                        companion.SaySomething("*"+ JumpTimes +"! Good job!*");
                                    }
                                    else
                                    {
                                        companion.SaySomething("*"+ JumpTimes +"! Nice!*");
                                    }
                                }
                                else
                                    companion.SaySomething("*"+ JumpTimes +"!*");
                            }
                        }
                    }
                    break;
                case 7:
                    {
                        if (!companion.IsSpeaking)
                        {
                            companion.SaySomething("*Don't worry, by the way. The great "+companion.GetRealName+" will handle your training from now on.*");
                            DialogueStep = 8;
                        }
                    }
                    break;
                case 8:
                    {
                        if (!companion.IsSpeaking)
                        {
                            companion.SaySomething("*And don't you dare to deny my training.*");
                            PlayerMod.PlayerAddCompanion(Target, companion);
                            WorldMod.AddCompanionMet(companion);
                            Target = null;
                        }
                    }
                    break;
            }
        }

        void CheckForPlayerAway(Companion companion)
        {
            if (DialogueStep == 0) return;
            if (!Target.active)
            {
                companion.SaySomething("*And " + (Target.Male ? "he's" : "she's") + " gone. Oh well, until we meet again.*");
                DialogueStep = 0;
                Target = null;
                PlayerMovedAway = false;
            }
            else if (Target.dead)
            {
                companion.SaySomething("*That was really horrible!*");
                if (DialogueStep >= 5)
                    DialogueStep = 5;
                else
                    DialogueStep = 0;
            }
            else if (companion.velocity.X == 0 && companion.velocity.Y == 0)
            {
                if (DialogueStep == 6)
                {
                    if (!PlayerMovedAway)
                    {
                        if (MathF.Abs(Target.Center.X - companion.Center.X) >= 400 ||
                            MathF.Abs(Target.Center.Y - companion.Center.Y) >= 300)
                        {
                            PlayerMovedAway = true;
                            companion.SaySomething("*And "+ (Target.Male ? "he" : "she")+" went away.*");
                        }
                    }
                    else
                    {
                        if (MathF.Abs(Target.Center.X - companion.Center.X) < 200 &&
                            MathF.Abs(Target.Center.Y - companion.Center.Y) < 100)
                        {
                            PlayerMovedAway = false;
                            companion.SaySomething("*Returned? Now you can try jumping "+(25 - JumpTimes)+", right?*");
                        }
                    }
                }
            }
            if (Target != null)
            {
                companion.FaceSomething(Target);
            }
        }
    }
}