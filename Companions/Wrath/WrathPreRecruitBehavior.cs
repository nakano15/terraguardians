using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using System;

namespace terraguardians.Companions.Wrath
{
    public class WrathPreRecruitBehavior : IdleBehavior
    {
        Behaviors behavior = Behaviors.Charge;
        bool WentBersek = false, PlayerLost = false, Defeated = false;
        int ActionTime = 0;
        bool FallHurt = false, ForceLeave = false;
        bool PlayerHasWrath = false;
        Player Target = null;
        bool LastWasReflect = false;

        public WrathPreRecruitBehavior()
        {
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            AllowSeekingTargets = false;
            AllowRevivingSomeone = false;
        }

        public override void Update(Companion companion)
        {
            if (ForceLeave)
            {
                if(companion.Target != null)
                {
                    if (companion.Target.Center.X < companion.Center.X)
                    {
                        companion.MoveLeft = true;
                        companion.MoveRight = false;
                    }
                    else
                    {
                        companion.MoveLeft = false;
                        companion.MoveRight = true;
                    }
                }
            }
            else if(Defeated)
            {
                CanBeAttacked = false;
                CanBeHurtByNpcs = false;
            }
            else if(!WentBersek)
            {
                Player Target = ViewRangeCheck(companion, 0, 512, 360, true, false);
                if (Target != null)
                {
                    this.Target = Target;
                    PlayerHasWrath = PlayerMod.PlayerHasCompanion(Target, companion);
                    WentBersek = true;
                    CanBeAttacked = true;
                    CanBeHurtByNpcs = true;
                    ActionTime = 0;
                    behavior = Behaviors.Charge;
                    if (!PlayerHasWrath)
                    {
                        switch (Main.rand.Next(5))
                        {
                            default:
                                companion.SaySomething("*You look like the perfect person for me to discount my rage.*");
                                break;
                            case 1:
                                companion.SaySomething("*I'm so glad to see you, time to unleash my rage!*");
                                break;
                            case 2:
                                companion.SaySomething("*Grrr!! You're going to help me get rid of this rage, even if you don't want!*");
                                break;
                            case 3:
                                companion.SaySomething("*You showed up in a bad moment.*");
                                break;
                            case 4:
                                companion.SaySomething("*Grrrreat! You just stand there!*");
                                break;
                        }
                    }
                    else
                    {
                        switch (Main.rand.Next(3))
                        {
                            default:
                                companion.SaySomething("*It's you again, I can use you to remove this unending anger!*");
                                break;
                            case 1:
                                companion.SaySomething("*You appeared right in time, prepare for my attacks!*");
                                break;
                            case 2:
                                companion.SaySomething("*Grr!! Help me!! I'm still burning out of rage!*");
                                break;
                        }
                    }
                }
                else
                {
                    UpdateIdle(companion);
                }
            }
            else if (PlayerLost)
            {
                if (ActionTime > 0)
                {
                    ActionTime++;
                    if (ActionTime >= 150)
                    {
                        if (ActionTime == 150 && companion.GetIsSubAttackInUse<WrathBodySlamAttack>())
                        {
                            companion.GetSubAttackActive.EndUse();
                        }
                        if(PlayerHasWrath)
                        {
                            if (PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake)
                            {
                                Target.GetModPlayer<PlayerMod>().ChangeReviveStack(3);
                            }
                        }
                        else
                        {
                            if (Target.Center.X > companion.Center.X)
                            {
                                companion.MoveLeft = true;
                                companion.MoveRight = false;
                            }
                            else
                            {
                                companion.MoveLeft = false;
                                companion.MoveRight = true;
                            }
                        }
                    }
                }
                else if (System.Math.Abs(Target.Center.X - companion.Center.X) > 40 && !companion.IsSubAttackInUse)
                {
                    if (Target.Center.X < companion.Center.X)
                    {
                        companion.MoveLeft = true;
                        companion.MoveRight = false;
                    }
                    else
                    {
                        companion.MoveLeft = false;
                        companion.MoveRight = true;
                    }
                }
                else
                {
                    ActionTime ++;
                    if (PlayerHasWrath)
                    {
                        companion.SaySomething("*It still didn't worked! Let me try helping you stand.*");
                    }
                    else
                    {
                        companion.SaySomething("*It didn't worked! I'm still furious! I'm outta here.*");
                    }
                }
            }
            else
            {
                if (Target != null)
                {
                    if (System.Math.Abs(Target.Center.X - companion.Center.X) >= 600 ||
                        System.Math.Abs(Target.Center.Y - companion.Center.Y) >= 400)
                    {
                        if (PlayerLost)
                        {
                            companion.SaySomething("*Loser!*");
                        }
                        else
                        {
                            companion.SaySomething("*Coward!*");
                        }
                        WentBersek = false;
                        PlayerLost = false;
                        Defeated = false;
                        ActionTime = 0;
                        Target = null;
                        CanBeAttacked =false;
                        CanBeHurtByNpcs = false;
                    }
                    else if (!Target.dead && PlayerMod.GetPlayerKnockoutState(Target) == KnockoutStates.Awake)
                    {
                        switch(behavior)
                        {
                            case Behaviors.Charge:
                                {
                                    if (Target.Center.X < companion.Center.X)
                                    {
                                        companion.MoveLeft = true;
                                        companion.MoveRight = false;
                                    }
                                    else
                                    {
                                        companion.MoveRight = true;
                                        companion.MoveLeft = false;
                                    }
                                    ActionTime++;
                                    if (ActionTime >= 360)
                                    {
                                        if (Math.Abs(Target.Center.Y - companion.Center.Y) >= 120 || Math.Abs(Target.Center.X - companion.Center.X) >= 180 || 
                                            !Collision.CanHitLine(Target.position, Target.width, Target.height, companion.position, companion.width, companion.height) || 
                                            Main.rand.NextFloat() < (Main.expertMode ? 0.3f : 0.1f))
                                        {
                                            behavior = Behaviors.ReachPlayer;
                                            companion.UseSubAttack<WrathAmbushAttack>(true, true);
                                        }
                                        else if (Main.rand.Next(3) == 0)
                                        {
                                            behavior = Behaviors.DestructiveRush;
                                            companion.UseSubAttack<WrathDestructiveRushAttack>(true, true);
                                        }
                                        else
                                        {
                                            behavior = Behaviors.BodySlam;
                                            companion.UseSubAttack<WrathBodySlamAttack>(true, true);
                                        }
                                        ActionTime = 0;
                                    }
                                }
                                break;
                            case Behaviors.PostJumpCooldown:
                                {
                                    ActionTime++;
                                    if (ActionTime >= 90)
                                    {
                                        if (!LastWasReflect && Main.rand.NextFloat() < 0.4f)
                                        {
                                            behavior = Behaviors.BulletReflectingBelly;
                                            companion.UseSubAttack<WrathBulletReflectingBellyAttack>(true, true);
                                        }
                                        else
                                        {
                                            behavior = Behaviors.Charge;
                                        }
                                        LastWasReflect = false;
                                        ActionTime = 0;
                                    }
                                }
                                break;
                            case Behaviors.BodySlam:
                                {
                                    behavior = Behaviors.PostJumpCooldown;
                                    ActionTime = 0;
                                }
                                break;
                            case Behaviors.DestructiveRush:
                                {
                                    if (Math.Abs(Target.Center.Y - companion.Center.Y) >= 120 || 
                                        Math.Abs(Target.Center.X - companion.Center.X) >= 240 || 
                                        !Collision.CanHitLine(Target.position, Target.width, Target.height, companion.position, companion.width, companion.height) || 
                                        Main.rand.NextDouble() < (Main.expertMode ? 0.4f : 0.2f))
                                    {
                                        behavior = Behaviors.ReachPlayer;
                                        companion.UseSubAttack<WrathAmbushAttack>(true, true);
                                    }
                                    else
                                    {
                                        behavior = Behaviors.PostJumpCooldown;
                                    }
                                    ActionTime = 0;
                                }
                                break;
                            case Behaviors.BulletReflectingBelly:
                                {
                                    LastWasReflect = true;
                                    behavior = Behaviors.PostJumpCooldown;
                                    ActionTime = 0;
                                }
                                break;
                            case Behaviors.ReachPlayer:
                                {
                                    if (Main.rand.NextFloat() < (Main.expertMode ? 0.3f : 0.66f))
                                    {
                                        behavior = Behaviors.PostJumpCooldown;
                                    }
                                    else
                                    {
                                        behavior = Behaviors.Charge;
                                    }
                                    ActionTime = 0;
                                }
                                break;
                        }
                    }
                    else
                    {
                        SetPlayerLost();
                        ActionTime = 0;
                    }
                }
                else
                {
                    UpdateIdle(companion);
                }
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (companion.IsSubAttackInUse)
                return;
            bool CloudForm = (companion.Data as WrathBase.PigGuardianFragmentData).IsCloudForm;
            if (!ForceLeave && Defeated)
            {
                companion.BodyFrameID = companion.ArmFramesID[0] = 
                    companion.ArmFramesID[1] = (short)(CloudForm ? 24 : 15);
            }
            else
            {
                switch (behavior)
                {
                    case Behaviors.PostJumpCooldown:
                        companion.ArmFramesID[0] = companion.ArmFramesID[1] = (short)((ActionTime * 0.25f) % 9);
                        if (CloudForm) companion.BodyFrameID = 19;
                        break;
                }
            }
        }

        public override bool CanKill(Companion companion)
        {
            if (PlayerMod.GetPlayerKnockoutState(companion) == KnockoutStates.Awake)
                companion.GetModPlayer<PlayerMod>().EnterKnockoutColdState(false);
            return false;
        }

        public override void WhenKOdOrKilled(Companion companion, bool Died)
        {
            if (Defeated) return;
            if (Main.rand.Next(2) == 0)
                companion.SaySomething("*I can't... Fight... Anymore...*");
            else
                companion.SaySomething("*You... Were better...*");
            SetDefeated();
            if (companion.GetIsSubAttackInUse<WrathBodySlamAttack>())
            {
                companion.GetSubAttackActive.EndUse();
            }
        }

        void SetPlayerLost()
        {
            PlayerLost = true;
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            AllowRevivingSomeone = true;
        }

        void SetDefeated()
        {
            Defeated = true;
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
        }

        public override void UpdateStatus(Companion companion)
        {
            switch(behavior)
            {
                case Behaviors.PostJumpCooldown:
                    companion.noKnockback = true;
                    break;
            }
            if (NPC.downedGolemBoss)
            {
                companion.MaxHealth = 18500;
                //Damage = 150;
                companion.statDefense += 50;
            }
            else if (NPC.downedMechBossAny)
            {
                companion.MaxHealth = 35000;
                //Damage = 90;
                companion.statDefense +=  35;
            }
            else if (Main.hardMode)
            {
                companion.MaxHealth = 15000;
                //Damage = 60;
                companion.statDefense +=  25;
            }
            else if (NPC.downedBoss3)
            {
                companion.MaxHealth = 8000;
                //Damage = 40;
                companion.statDefense +=  20;
            }
            else
            {
                companion.MaxHealth = 4000;
                //Damage = 20;
                companion.statDefense +=  10;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return !ForceLeave && (Defeated || PlayerLost);
        }

        public override bool IsHostileTo(Player target)
        {
            return !Defeated && !PlayerLost;
        }

        public override string CompanionNameChange(Companion companion)
        {
            return "Angry Pig Cloud";
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            return BeginDialogue();
        }

        MessageBase BeginDialogue()
        {
            MessageDialogue md = new MessageDialogue();
            if (PlayerHasWrath)
            {
                if (PlayerLost)
                {
                    md.AddOption("I thought the deal wasn't of causing problems around?", HRLoseMes02);
                }
                else
                {
                    md.AddOption("I thought the deal wasn't of causing problems around?", HRWinMes02);
                }
            }
            else
            {
                md.AddOption("You just tried to kill me!", NRMes01);
            }
            if (Defeated)
            {
                if (PlayerHasWrath)
                {
                    md.ChangeMessage("*Grr!! That wont help me get less furious!*");
                    return md;
                }
                else
                {
                    md.ChangeMessage("*Arrgh!! That didn't helped! I'm even more furious now!*");
                    return md;
                }
            }
            md.ChangeMessage("*I'm so angry that I even forgot what I should be saying!*");
            return md;
        }

        void HRLoseMes01()
        {
            MessageDialogue md = new MessageDialogue("*My anger continues, and I still don't have any clue on how to deal with It!*");
            md.AddOption("I thought the deal wasn't of causing problems around?", HRLoseMes02);
            md.RunDialogue();
        }

        void HRLoseMes02()
        {
            MessageDialogue md = new MessageDialogue("*Yes, but that was on the other world! Here is different.*");
            md.AddOption("How can It be different?", HRLoseMes03);
            md.RunDialogue();
        }

        void HRLoseMes03()
        {
            MessageDialogue md = new MessageDialogue("*I don't know why! And trying to think about that is making me even more furious.*");
            md.AddOption("No! Wait, It's okay.", HRLoseMes04);
            md.RunDialogue();
        }

        void HRLoseMes04()
        {
            Recruit();
            MessageDialogue md = new MessageDialogue("*I'm here if you need my help. I'd be glad to use my rage on your opposition.*");
            md.AddOption("Uh... Thanks?", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void HRWinMes01()
        {
            MessageDialogue md = new MessageDialogue("*My anger continues, and I still don't have any clue on how to deal with It!*");
            md.AddOption("I thought the deal wasn't of causing problems around?", HRWinMes02);
            md.RunDialogue();
        }

        void HRWinMes02()
        {
            MessageDialogue md = new MessageDialogue("*Yes, but that was on the other world! Here is different.*");
            md.AddOption("How can It be different?", HRWinMes03);
            md.RunDialogue();
        }

        void HRWinMes03()
        {
            MessageDialogue md = new MessageDialogue("*I don't know why! And trying to think about that is making me even more furious.*");
            md.AddOption("Don't you dare attacking me again", HRWinMes04);
            md.RunDialogue();
        }

        void HRWinMes04()
        {
            Recruit();
            MessageDialogue md = new MessageDialogue("*I won't. I'm here if you need my help. I'd be glad to use my rage on your opposition.*");
            md.AddOption("Uh... Thanks?", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void NRMes01()
        {
            MessageDialogue md = new MessageDialogue("*I wasn't! I was trying to lose my unending rage by unleashing It on someone.*");
            md.AddOption("Unending rage?", NRMes02);
            md.RunDialogue();
        }

        void NRMes02()
        {
            MessageDialogue md = new MessageDialogue("*Yes! I have this unending rage since I woke up some time ago!*");
            md.AddOption("How could you have rage until since you woke up?", NRMes03);
            md.RunDialogue();
        }

        void NRMes03()
        {
            MessageDialogue md = new MessageDialogue("*I don't know either! I don't even remember things from before I woke up, and that makes me even more furious!*");
            md.AddOption("May I help you in some way?", NRMes04);
            md.RunDialogue();
        }

        void NRMes04()
        {
            MessageDialogue md = new MessageDialogue("*Yes! Try ending this rage, I can barelly concentrate because of It.*");
            md.AddOption("Okay, I'll try to help you.", NRMes05_Accept);
            md.AddOption("No, I'll not help you.", NRMes05_Deny);
            md.RunDialogue();
        }

        void NRMes05_Accept()
        {
            Recruit();
            MessageDialogue md = new MessageDialogue("*Thanks, I'll try my best not to cause problems here, but I can't promisse anyhting.*");
            md.AddOption("Alright", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void NRMes05_Deny()
        {
            MessageDialogue md = new MessageDialogue("*I should have guessed you wouldn't help me! I don't know why I wasted my time.*");
            md.AddOption("Alright", Dialogue.EndDialogue);
            md.RunDialogue();
            ForceLeave = true;
        }

        void Recruit()
        {
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Wrath);
            WorldMod.AddCompanionMet(CompanionDB.Wrath);
        }

        public enum Behaviors : byte
        {
            Charge = 0,
            BodySlam = 1,
            PostJumpCooldown = 2,
            DestructiveRush = 3,
            BulletReflectingBelly = 4,
            ReachPlayer = 5
        }
    }
}