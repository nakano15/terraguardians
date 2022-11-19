using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class SardineBase : CompanionBase
    {
        public override string Name => "Sardine";
        public override string Description => "He's an adventurer that has visited many worlds,\nearns his life as a bounty hunter. But actually forgot\nwhich world his house is at.";
        public override int Age => 25;
        public override int SpriteWidth => 72;
        public override int SpriteHeight => 56;
        public override bool CanCrouch => false;
        public override int Width => 14;
        public override int Height => 38;
        public override float Scale => 34f / 38;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 80; //320
        public override int HealthPerLifeCrystal => 12;
        public override int HealthPerLifeFruit => 3;
        //public override float Gravity => 0.5f;
        public override float MaxRunSpeed => 4.82f;
        public override float RunAcceleration => 0.15f;
        public override float RunDeceleration => 0.5f;
        public override int JumpHeight => 12;
        public override float JumpSpeed => 9.76f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.NPCHit51;
        public override SoundStyle DeathSound => Terraria.ID.SoundID.NPCDeath54;
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer;
        #region  Animations
        protected override Animation SetWalkingFrames {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24); //8
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames 
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 13; i <= 16; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 12; i++) anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(17);
        protected override Animation SetPlayerMountedArmFrame => new Animation(17);
        protected override Animation SetChairSittingFrames => new Animation(18);
        protected override Animation SetThroneSittingFrames => new Animation(19);
        protected override Animation SetBedSleepingFrames => new Animation(20);
        protected override Animation SetRevivingFrames => new Animation(22);
        protected override Animation SetDownedFrames => new Animation(21);
        protected override Animation SetPetrifiedFrames => new Animation(23);
        protected override Animation SetBackwardStandingFrames => new Animation(24);
        protected override Animation SetBackwardReviveFrames => new Animation(25);
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(), 
                    new AnimationPositionCollection()
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 10, 12);
                Hands[0].AddFramePoint2X(11, 27, 14);
                Hands[0].AddFramePoint2X(12, 31, 26);
                
                Hands[0].AddFramePoint2X(13, 12, 9);
                Hands[0].AddFramePoint2X(14, 22, 12);
                Hands[0].AddFramePoint2X(15, 25, 18);
                Hands[0].AddFramePoint2X(16, 21, 23);
                
                Hands[0].AddFramePoint2X(17, 16, 18);
                
                Hands[0].AddFramePoint2X(22, 21, 23);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 12, 12);
                Hands[1].AddFramePoint2X(11, 29, 14);
                Hands[1].AddFramePoint2X(12, 33, 26);
                
                Hands[1].AddFramePoint2X(13, 15, 4);
                Hands[1].AddFramePoint2X(14, 24, 12);
                Hands[1].AddFramePoint2X(15, 27, 18);
                Hands[1].AddFramePoint2X(16, 23, 23);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition 
        {
            get
            {
                AnimationPositionCollection animation = new AnimationPositionCollection(new Vector2(16, 25), true);
                return animation;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(new Vector2(17, 25));
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * -2);
        #endregion
        #region Dialogue
        public override string GreetMessages(Companion companion)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Hey, aren't you an adventurer? Cool! I am too!";
            return "Tarararan-Taran! Meet the worlds biggest smallest bounty hunter ever! Me!";
        }

        public override string NormalMessages(Companion guardian)
        {
            Player player = Main.LocalPlayer;
            List<string> Mes = new List<string>();
            if (Main.dayTime)
            {
                Mes.Add("Why female humans keep wanting to try scratching the back of my head?");
                Mes.Add("This place surelly is livelly, but I'd rather go out and beat some creatures.");
            }
            else
            {
                Mes.Add("I'm so sleepy, do you know of any window I could be at?");
                Mes.Add("Looks like a perfect moment to explore, even more with my night eyes.");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("Do you know what time it is? It's fun time! Let's go outside and beat some ugly creatures!");
            }
            if (Main.raining)
            {
                Mes.Add("I hate this weather, but at least gives me good reason to stay at home.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Where did those weird creatures came from?");
            }
            bool HasBreeMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Bree), HasGlennMet = PlayerMod.PlayerHasCompanionSummoned(player, CompanionDB.Glenn);
            /*switch (guardian.OutfitID)
            {
                case CaitSithOutfitID:
                    Mes.Add("I really like this outfit, but the cloak on my tail is bothering me a bit.");
                    Mes.Add("For some reason, this outfit also came with some kind of sword.");
                    Mes.Add("[nickname], do you know what Tokyo is?");
                    break;
            }*/
            if (HasBreeMet && HasGlennMet)
            {
                Mes.Add("I'm so glad that my son and my wife are safe.");
                Mes.Add("I caused my son and wife so much trouble when I disappeared, now neither of us knows the way home...");
                Mes.Add("I thank you for finding my wife and my son, [nickname]. You have my eternal gratitude.");
            }
            else if (!HasBreeMet && HasGlennMet)
            {
                Mes.Add("[nickname], I heard from my son that my wife left home to look for me. If you find a white cat during your travels, please bring her here.");
                Mes.Add("I'm sorry for asking this [nickname], but I just heard from my son that my wife is still looking for me. She's a white cat, please find her.");
            }
            if (MainMod.HasCompanionInWorld(1))
            {
                Mes.Add("Do you have some spare medicine? [gn:1] seems to be wanting to play with me again...");
                Mes.Add("I have tried to outrun [gn:1] so I don't play that stupid game, but she's faster than me on 4 legs.");
                Mes.Add("If you see [gn:1], say that you didn't see me. I'm tired of playing \"Cat and the Wolf\" with her, I didn't even knew that game existed, and my body is still wounded because of her teeth from the last game.");
            }
            if (MainMod.HasCompanionInWorld(3))
            {
                Mes.Add("I really don't want to play with [gn:3], I could even run away from him, but everytime I do so, he pulls me back with... Whatever is that thing! It's smelly and yuck!");
                Mes.Add("I really hate when [gn:3] plays his game with me, everytime he acts like as if was devouring my brain I feel like my heart was going to jump out of my mouth.");
                Mes.Add("Eugh, [gn:3] \"played\" a game with me, and now I'm not only bitten on many places, but also with smelly sticky stuffs around. Wait, will I turn into a Zombie because of that?! Should I begin panicking?");
                Mes.Add("I want to remove all that stinky stuff i've got from being bullied by [gn:3] from my fur, but I don't even know what is that, so I can't really lick it away. Maybe I should... *Gulp* Take a bath? With water?");
            }
            if (MainMod.HasCompanionInWorld(1) && MainMod.HasCompanionInWorld(3))
            {
                Mes.Add("You want to know what is worse than a wolf playing \"Cat and Wolf\" with you? Two wolves!!! And one is a Zombie!!");
                Mes.Add("First she invented that \"Cat and Wolf\" game, now that creepy [gn:3] invented the \"The Walking Guardian\" game. Why does they love bullying me? Is that a wolf thing?");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic) && NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("Everytime I chat with [nn:" + Terraria.ID.NPCID.Mechanic + "], she is always in good mood and happy. On other hand, [nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] stares at me with a killer face. Maybe I should start sharpening my knife, meow?");
            }
            if(PlayerMod.PlayerHasCompanionSummoned(player, 0))
                Mes.Add("Hey [gn:0], want to play some \"Hide and Seek\"? Don't take me wrong, I like games, depending on them...");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("Hello, I- Waaaaaah!!! *He ran away, as fast as he could.*");
                Mes.Add("No! Go away! I don't want to play with you, I don't even want to see you, I.. I... Have some important things to do, I mean... Somewhere veeeeeeery far away from you!");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3))
            {
                Mes.Add("Yikes! Go away! Your \"game\" spooks me out so hard.");
                Mes.Add("No way, not again. *He ran away*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("I really feel bad about [nn:" + Terraria.ID.NPCID.Angler + "], he'll never know my trick for catching more than one fish. Neither you will do.");
            if (NPC.downedBoss1)
                Mes.Add("So, you have defeated the Eye of Cthulhu, right? Psh, Easy. Do you ever wanted to know who killed Cthulhu? Well, It was me. Hey, what are you laughing at?");
            if (NPC.downedBoss3)
                Mes.Add("My next bounty is set to the Skeletron, at the Dungeon Entrance. Let's go face him!");
            /*if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
            {
                Mes.Add("Hey, do you have a minute? I want to discuss with you about my newest bounty hunting service.");
            }
            else
            {
                if (GuardianBountyQuest.SignID == -1)
                    Mes.Add("I need a sign in my house to be able to place my bounties in it. I only need one, no more than that. If It's an Announcement Box, will be even better.");
            }*/
            if (MainMod.HasCompanionInWorld(5))
            {
                Mes.Add("Playing with [gn:5] is really fun. But he has a little problem to know what \"enough\" is.");
                Mes.Add("One of the best things that ever happened was when you brought [gn:5] here, at least playing with him doesn't hurts or cause wounds... Most of the time.");
            }
            if (MainMod.HasCompanionInWorld(7))
            {
                Mes.Add("I really love [gn:7], but she keeps controlling me. She could at least give me more freedom, It's not like as if I would run or something.");
                Mes.Add("[gn:7] was the most lovely and cheerful person I've ever met, but for some reason, she started to get grumpy after we married. What happened?");
                Mes.Add("Even though [gn:7] tries to hog all my attention to her, I still love her.");
                Mes.Add("I wonder, does [gn:7] carrying that bag all day wont do bad for her back?");
                if (MainMod.HasCompanionInWorld(1))
                    Mes.Add("Woah, you should have seen [gn:7] fighting with [gn:1] earlier. That remembered me of the day we met.");
                if (MainMod.HasCompanionInWorld(3))
                    Mes.Add("Ever since [gn:7] saw [gn:3] playing that stupid hateful game with me, she has been asking frequently If I'm fine, and If I wont... Turn? What is that supposed to mean?");
            }
            if (MainMod.HasCompanionInWorld(8))
            {
                Mes.Add("I love having [gn:8] around, but she asks me to do too many things. It's a bit tiring. Refuse? Are you nuts? Did you look at her?!");
                Mes.Add("I have a favor to ask of you. If you see me staring at [gn:8] with a goof face, slap my face.");
                if (MainMod.HasCompanionInWorld(7))
                {
                    Mes.Add("[gn:7] keeps asking me if there is something happening between me and [gn:8]. No matter how many times I say no, she still remains furious.");
                }
                if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles) && MainMod.HasCompanionInWorld(CompanionDB.Blue))
                {
                    Mes.Add("I have to tell you something bizarre that happened to me the other day. I managed to run away from [gn:" + CompanionDB.Blue + "] and [gn:" + CompanionDB.Fluffles + "] game, or so I thought, and managed to return home. When [gn:" + CompanionDB.Bree + "] looked at me, she saw [gn:" + CompanionDB.Fluffles + "] hanging on my shoulder. [gn:"+CompanionDB.Bree+"] screamed so loud that scared her off.");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Domino))
            {
                Mes.Add("Lies! I'm not buying catnip! Where did you brought that idea from?");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Vladimir))
            {
                Mes.Add("Hey, you know that guy, [gn:"+CompanionDB.Vladimir+"]? He's really helping me with some personal matters. If you feel troubled, have a talk with him.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Michelle))
            {
                Mes.Add("Whenever I tell stories about my adventures, [gn:" + CompanionDB.Michelle + "] listen attentiously to every details of it. I think I got a fan.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles))
            {
                bool HasBlue = MainMod.HasCompanionInWorld(CompanionDB.Blue),
                    HasZacks = MainMod.HasCompanionInWorld(CompanionDB.Zacks);
                if (HasBlue && HasZacks)
                {
                    Mes.Add("Great, now I have a narcisistic wolf, a rotting wolf and a hair rising ghost fox trying to have a piece of me. You have some kind of grudge against me?");
                    Mes.Add("If you see [gn:"+CompanionDB.Blue+"], [gn:"+CompanionDB.Zacks+"] and [gn:"+CompanionDB.Fluffles+"] looking like they are biting something on the floor, that must be me. Help me if you can?");
                }
                else if (HasBlue)
                {
                    Mes.Add("I really hate having [gn:" + CompanionDB.Fluffles + "] around, because there are now TWO to bite me on [gn:" + CompanionDB.Blue + "]'s stupid game.");
                    Mes.Add("How can you escape from something you can't even see? [gn:"+CompanionDB.Fluffles+"] always catches me because she's nearly invisible during day!");
                }
                Mes.Add("I have to tell you what happened to me the other day. I was on the toilet doing my things, having a hard time, until [gn:"+CompanionDB.Fluffles+"] surged from nowhere. She spooked me really hard! But at least solved my constipation issue.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Minerva))
            {
                Mes.Add("Like [gn:" + CompanionDB.Minerva + "], you're wondering why I only eat fish? It's because fishs are the best!");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Glenn))
            {
                Mes.Add("[gn:" + CompanionDB.Glenn + "] is more the studious type than a fighter.");
                Mes.Add("Sometimes I don't have the chance of doing something with [gn:" + CompanionDB.Glenn + "], our interests are different.");
                Mes.Add("[gn:" + CompanionDB.Glenn + "] should stop reading so many fairy tales books, since we are literally living in one.");
                if (MainMod.HasCompanionInWorld(CompanionDB.Zacks))
                {
                    Mes.Add("If [gn:"+CompanionDB.Zacks+"] keep scaring my son whenever he's outside at dark, I'll show him a version of me that he didn't met when biting me!");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Miguel))
            {
                Mes.Add("Could [gn:"+CompanionDB.Miguel+"] stop making jokes about my belly? They hurt!");
                Mes.Add("I'm really getting some exercise tips from [gn:"+CompanionDB.Miguel+"] to turn my fat into muscles, but he keeps making jokes about my belly.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Green))
            {
                Mes.Add("You may think ghosts and stuff are scary, but you wont know what is scary, until you wake up and see [gn:"+CompanionDB.Green+"] staring directly at your face.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I'm always ready for a new adventure.";
                case JoinMessageContext.FullParty:
                    return "There's way too many people in your group. There is no way I can join.";
                case JoinMessageContext.Fail:
                    return "I can't go on an adventure right now...";
            }
            return base.JoinGroupMessages(companion, context);
        }
        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "Awww... I were having so much fun. Let's adventure some more in the future.";
                case LeaveMessageContext.Fail:
                    return "Now is not the best moment for that.";
                case LeaveMessageContext.AskIfSure:
                    return "Yes, I can leave the group but... Here?";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "Okay, okay. I wont judge your decision. I see you back at home. Be safe.";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Yes, let's look for a town so I can leave the group.";
            }
            return base.LeaveGroupMessages(companion, context);
        }
        #endregion
    }
}