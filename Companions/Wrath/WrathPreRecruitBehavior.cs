using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using System;
using terraguardians.Companions.Wrath.SubAttacks;

namespace terraguardians.Companions.Wrath
{
    public class WrathPreRecruitBehavior : PreRecruitBehavior
    {
        Behaviors behavior = Behaviors.Charge;
        bool WentBersek = false, PlayerLost = false, Defeated = false;
        new int ActionTime = 0;
        bool FallHurt = false, ForceLeave = false;
        bool PlayerHasWrath = false;
        new Player Target = null;
        bool LastWasReflect = false;
        private byte BulletsHit = 0, HitsReceived = 0;
        public override bool AllowDespawning => true;

        public WrathPreRecruitBehavior()
        {
            CanBeAttacked = false;
            CanBeHurtByNpcs = false;
            AllowSeekingTargets = false;
            AllowRevivingSomeone = false;
            RunCombatBehavior = false;
            NoticePlayers = false;
            FollowPlayers = false;
            UseHealingItems = false;
        }

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake || Companion.Behaviour_InDialogue) return;
            if (ForceLeave)
            {
                if(companion.Target != null)
                {
                    if (companion.Target.Center.X > companion.Center.X)
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
                companion.MoveLeft = false;
                companion.MoveRight = false;
                companion.controlJump = false;
                companion.MoveDown = false;
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
                    companion.WalkMode = false;
                    if (!PlayerHasWrath)
                    {
                        switch (Main.rand.Next(5))
                        {
                            default:
                                companion.SaySomething("*NIGHT TIME! THATS THE RIGHT TIME!*");
                                break;
                            case 1:
                                companion.SaySomething("*HEY SPECIAL DELIVERY! KNUCKLE SANDWICH!*");
                                break;
                            case 2:
                                companion.SaySomething("*IM ABOUT TO CRASH OUT! YOU'RE NO EXCEPTION!*");
                                break;
                            case 3:
                                companion.SaySomething("*UH OH! stumbled around the wrong territory buddy!*");
                                break;
                            case 4:
                                companion.SaySomething("*ITS ONSIGHT WITH ME! PREPARE FOR COMBAT!*");
                                break;
                        }
                    }
                    else
                    {
                        switch (Main.rand.Next(3))
                        {
                            default:
                                companion.SaySomething("*Ugh you again! Better not waste my time!*");
                                break;
                            case 1:
                                companion.SaySomething("*Just in time to receive your daily dose of " + (!MainMod.EnableProfanity ? "ass whooping" : "beating") + "?!*");
                                break;
                            case 2:
                                companion.SaySomething("*Grr!! Your no help!! My frenzy continues!*");
                                break;
                        }
                    }
                }
                else
                {
                    WanderAI(companion);
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
                        companion.SaySomething("*WHAT IT DIDN'T WORK?! GET" + (!MainMod.EnableProfanity ? " THE HELL" : "") + " UP!*");
                    }
                    else
                    {
                        companion.SaySomething("*IT DIDN'T WORK? WHAT IS THIS?!*");
                    }
                }
            }
            else
            {
                PlayerMod.SetNonLethal();
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
                        companion.statLife = companion.statLifeMax2;
                        WentBersek = false;
                        PlayerLost = false;
                        Defeated = false;
                        ActionTime = 0;
                        Target = null;
                        CanBeAttacked = false;
                        CanBeHurtByNpcs = false;
                        companion.WalkMode = true;
                    }
                    else if (!Target.dead && PlayerMod.GetPlayerKnockoutState(Target) == KnockoutStates.Awake)
                    {
                        companion.WalkMode = false;
                        switch(behavior)
                        {
                            case Behaviors.Charge:
                                {
                                    if (!companion.IsSubAttackInUse)
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
                                        if (companion.velocity.Y == 0 && Target.Bottom.Y < companion.Bottom.Y - 20)
                                        {
                                            companion.velocity.Y -= 8f;
                                        }
                                    }
                                    ActionTime++;
                                    if (companion.Hitbox.Intersects(Target.Hitbox))
                                    {
                                        companion.UseSubAttack<WrathBellyTackleAttack>(true, true);
                                    }
                                    if (!companion.IsSubAttackInUse && ActionTime >= 360)
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
                                    if (!companion.IsSubAttackInUse)
                                    {
                                        behavior = Behaviors.PostJumpCooldown;
                                        ActionTime = 0;
                                    }
                                }
                                break;
                            case Behaviors.DestructiveRush:
                                {
                                    if (!companion.IsSubAttackInUse)
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
                                }
                                break;
                            case Behaviors.BulletReflectingBelly:
                                {
                                    if (!companion.IsSubAttackInUse)
                                    {
                                        LastWasReflect = true;
                                        behavior = Behaviors.PostJumpCooldown;
                                        ActionTime = 0;
                                    }
                                }
                                break;
                            case Behaviors.ReachPlayer:
                                {
                                    if (!companion.IsSubAttackInUse)
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
                    WanderAI(companion);
                }
            }
            companion.Target = Target;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (companion.IsSubAttackInUse)
                return;
            bool CloudForm = (companion.Data as WrathBase.PigGuardianFragmentData).IsCloudForm;
            if (!ForceLeave && Defeated)
            {
                companion.BodyFrameID = companion.ArmFramesID[0] = 
                    companion.ArmFramesID[1] = 15;
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
            {
                companion.statLife = companion.statLifeMax2;
                if (Main.rand.Next(2) == 0)
                    companion.SaySomething("*Is this the taste of defeat?! NOOO!!*");
                else
                    companion.SaySomething("*YOU DEFEATED ME HOW?!*");
                SetDefeated();
                if (companion.GetIsSubAttackInUse<WrathBodySlamAttack>())
                {
                    companion.GetSubAttackActive.EndUse();
                }
            }
            return false;
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

        public override void ModifyHurt(Companion companion, ref Player.HurtModifiers modifiers)
        {
            if(modifiers.DamageSource.SourceProjectileLocalIndex >= 0)
            {
                Projectile proj = Main.projectile[modifiers.DamageSource.SourceProjectileLocalIndex];
                if (proj.DamageType.CountsAsClass<MeleeDamageClass>())
                {
                    IncreaseHitsReceived(companion);
                }
                else
                {
                    IncreaseProjsHitsReceived(companion);
                }
            }
            else
            {
                IncreaseHitsReceived(companion);
            }
        }

        void IncreaseHitsReceived(Companion companion)
        {
            if (behavior != Behaviors.Charge) return;
            HitsReceived++;
            if (HitsReceived >= 20)
            {
                HitsReceived = 0;
                if (Main.rand.NextFloat() < 0.3f)
                {
                    behavior = Behaviors.BodySlam;
                    ActionTime = 0;
                    companion.UseSubAttack<WrathBodySlamAttack>(true, true);
                    companion.SaySomething("*This will flatten you!*");
                }
                else if (Main.rand.NextFloat() < 0.6f)
                {
                    behavior = Behaviors.ReachPlayer;
                    ActionTime = 0;
                    companion.UseSubAttack<WrathAmbushAttack>(true, true);
                    companion.SaySomething("*Lets see how you like this!*");
                }
                else
                {
                    behavior = Behaviors.DestructiveRush;
                    ActionTime = 0;
                    companion.UseSubAttack<WrathDestructiveRushAttack>(true, true);
                    companion.SaySomething("*BOOM" + (!MainMod.EnableProfanity ? " BITCH" : "! COMING FOR YOU")+"!*");
                }
            }
        }

        void IncreaseProjsHitsReceived(Companion companion)
        {
            if (behavior != Behaviors.Charge) return;
            if (companion.chatOverhead.timeLeft == 0)
            {
                if (Main.rand.Next(2) == 0)
                    companion.SaySomething("*Oh you play far?! I got something for you!*");
                else
                    companion.SaySomething("*HEY STOP THAT! GET OVER HERE!*");
            }
            BulletsHit++;
            if (BulletsHit > 10)
            {
                BulletsHit = 0;
                if (Main.rand.NextFloat() < 0.3f)
                {
                    behavior = Behaviors.BodySlam;
                    ActionTime = 0;
                    companion.UseSubAttack<WrathBodySlamAttack>(true, true);
                    companion.SaySomething("*HERE IS THE FINISHER!*");
                }
                else
                {
                    behavior = Behaviors.BulletReflectingBelly;
                    ActionTime = 0;
                    companion.UseSubAttack<WrathBulletReflectingBellyAttack>(true, true);
                    companion.SaySomething("*FIRE AT ME AGAIN! YOUR ONLY HURTING YOURSELF!*");
                }
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
                    md.AddOption("Still causing issues?", HRMes02);
                }
                else
                {
                    md.AddOption("Still causing issues?", HRMes02);
                }
            }
            else
            {
                md.AddOption("YOU TRIED TO KILL ME!", NRMes01);
            }
            if (Defeated)
            {
                if (PlayerHasWrath)
                {
                    md.ChangeMessage("*Grr!! YOU ONLY MADE ME ANGRIER!*");
                    return md;
                }
                else
                {
                    md.ChangeMessage("*Arrgh!! THAT WAS POINTLESS!*");
                    return md;
                }
            }
            md.ChangeMessage("*My mind is clouded in red! I forgot what I should be saying!" + (!MainMod.EnableProfanity ? " FUCK!" : "") + "*");
            return md;
        }

        void HRMes01()
        {
            MessageDialogue md = new MessageDialogue("*My wrath continues! It never ends!*");
            md.AddOption("I thought the deal was not to cause problems?", HRMes02);
            md.RunDialogue();
        }

        void HRMes02()
        {
            MessageDialogue md = new MessageDialogue("*IDIOT! That was the other world! This is different!*");
            md.AddOption("How can It be different?", HRMes03);
            md.RunDialogue();
        }

        void HRMes03()
        {
            MessageDialogue md = new MessageDialogue("*I don't know why! And trying to think about that is making me aggrevated!*");
            md.AddOption("No. Thats okay.", HRMes04);
            md.RunDialogue();
        }

        void HRMes04()
        {
            Recruit();
            MessageDialogue md = new MessageDialogue("*I don't care what happens! just call me when there is stuff to ruin!*");
            md.AddOption("Uh...ok... Thanks?", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void NRMes01()
        {
            MessageDialogue md = new MessageDialogue("*My mind is always in a frenzy! I have perpetual tantrums!*");
            md.AddOption("Perpetual tantrums?!", NRMes02);
            md.RunDialogue();
        }

        void NRMes02()
        {
            MessageDialogue md = new MessageDialogue("*I DONT KNOW WHY I AM LIKE THIS! I JUST AM!*");
            md.AddOption("How could you have such outburst?", NRMes03);
            md.RunDialogue();
        }

        void NRMes03()
        {
            MessageDialogue md = new MessageDialogue("*I DON'T KNOW WHY! And trying to think about that is making me aggrevated!*");
            md.AddOption("I could possibly help you in some way?", NRMes04);
            md.RunDialogue();
        }

        void NRMes04()
        {
            MessageDialogue md = new MessageDialogue("*You need to find a solution to my problem...NOW!*");
            md.AddOption("Okay, I'll try to help you. Try to chill out for a second.", NRMes05_Accept);
            md.AddOption("No, I will not help you.", NRMes05_Deny);
            md.RunDialogue();
        }

        void NRMes05_Accept()
        {
            Recruit();
            MessageDialogue md = new MessageDialogue("*I can't promise you anything, just stay out of my way!*");
            md.AddOption("Alright", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void NRMes05_Deny()
        {
            MessageDialogue md = new MessageDialogue("*ERRRR YOUR WASTING MY TIME!*");
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
