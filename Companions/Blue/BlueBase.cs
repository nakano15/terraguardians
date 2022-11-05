using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class BlueBase : CompanionBase
    {
        public override string Name => "Blue";
        public override string Description => "";
        public override int Age => 17;
        public override Genders Gender => Genders.Female;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override bool CanCrouch => true;
        public override int Width => 26;
        public override int Height => 82;
        public override float Scale => 99f / 82;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 175; //1150
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 15;
        public override float MaxRunSpeed => 4.75f;
        public override float RunAcceleration => 0.13f;
        public override float RunDeceleration => 0.5f;
        public override int JumpHeight => 19;
        public override float JumpSpeed => 7.52f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.NPCHit6;
        //public override SoundStyle DeathSound => Terraria.ID.SoundID.DD2_KoboldDeath;
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
                for(short i = 16; i <= 19; i++)
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
        protected override Animation SetCrouchingFrames => new Animation(20);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(21, 1);
                anim.AddFrame(22, 1);
                anim.AddFrame(23, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(24);
        protected override Animation SetPlayerMountedArmFrame => new Animation(25);
        protected override Animation SetThroneSittingFrames => new Animation(27);
        protected override Animation SetBedSleepingFrames => new Animation(28);
        protected override Animation SetRevivingFrames => new Animation(33);
        protected override Animation SetDownedFrames => new Animation(32);
        protected override Animation SetPetrifiedFrames => new Animation(34);
        protected override Animation SetBackwardStandingFrames => new Animation(35);
        protected override Animation SetBackwardReviveFrames => new Animation(37);
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(new Vector2(18, 31), true), 
                    new AnimationPositionCollection(new Vector2(30, 31), true)
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 6, 14);
                Hands[0].AddFramePoint2X(11, 40, 9);
                Hands[0].AddFramePoint2X(12, 43, 41);
                
                Hands[0].AddFramePoint2X(16, 12, 5);
                Hands[0].AddFramePoint2X(17, 30, 7);
                Hands[0].AddFramePoint2X(18, 37, 19);
                Hands[0].AddFramePoint2X(19, 31, 32);
                
                Hands[0].AddFramePoint2X(21, 43, 22);
                Hands[0].AddFramePoint2X(22, 43, 31);
                Hands[0].AddFramePoint2X(23, 40, 42);
                
                Hands[0].AddFramePoint2X(33, 43, 43);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 9, 14);
                Hands[1].AddFramePoint2X(11, 42, 9);
                Hands[1].AddFramePoint2X(12, 45, 41);
                
                Hands[1].AddFramePoint2X(16, 15, 5);
                Hands[1].AddFramePoint2X(17, 34, 7);
                Hands[1].AddFramePoint2X(18, 39, 19);
                Hands[1].AddFramePoint2X(19, 33, 32);
                
                Hands[1].AddFramePoint2X(21, 42, 22);
                Hands[1].AddFramePoint2X(22, 45, 31);
                Hands[1].AddFramePoint2X(23, 43, 42);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16);
        #endregion
        #region Dialogue
        public override string NormalMessages(Companion guardian)
        {
            bool ZacksRecruited = false; //He's not implemented yet
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("*Yes, [nickname]. Need something?*"); //"*[name] is looking at me with a question mark face, while wondering what you want.*");
                Mes.Add("*I'm so happy to see you.*"); //"*[name] looks to me while smiling.*");
                if (Main.LocalPlayer.head == 17)
                {
                    Mes.Add("*That's one cute little hood you got. Makes me want to hug you.*");//"*[name] says that you look cute with that hood.*");
                }
                /*if (MainMod.IsPopularityContestRunning)
                {
                    Mes.Add("*Hey [nickname]. Did you knew that the TerraGuardians Popularity Contest is running right now? Be sure to vote some time.*");
                    Mes.Add("*The TerraGuardians Popularity Contest is currently running. You can access the voting by speaking to me about it.*");
                }*/
                Mes.Add("*Have you heard Deadraccoon5? Oh, I feel bad for you right now.*");
            }
            else
            {
                Mes.Add("*Grrr.... What do you want?*"); //"*[name] is growling and showing her teeth as I approached her.*");
                Mes.Add("*Have you came to annoy me?!* (Her facial expression is very scary. I should avoid talking to her.)"); //"*[name]'s facial expressions is very scary, I should avoid talking to her at the moment.*");
            }
            if (!ZacksRecruited)
            {
                if (true || !Main.bloodMoon) //Todo - When Zacks is implemented, I need to remove the true flag.
                {
                    if (Main.raining)
                        Mes.Add("*This weather was a lot better when I was with...*"); //"*[name] looks sad.*");
                    if (!Main.dayTime)
                        Mes.Add("*Awooooo. Snif~ Snif~* (She looks in sorrow)");//"*[name] howls to the moon, showing small signs of sorrow.*");
                }
                else
                {
                    if (!Main.dayTime)
                        Mes.Add("*I'm feeling the presence of... [nickname], could you take me and check the border of this world? I think someone I'm looking for may be found there.*"); //"*[name] is saying that she is feeling a familiar presence, coming from the far lands of the world. Saying that we should check.*");
                }
            }
            //Outfit messages
            /*if (!Main.bloodMoon)
            {
                switch (guardian.OutfitID)
                {
                    case RedHoodOutfitID:
                        Mes.Add("*I really love this outfit! I feel very much into starting a new adventure when wearing this.*"); //"*[name] is saying that she likes that outfit. She also tells you that feels very adventurous when wearing It.*");
                        break;
                    case CloaklessOutfitID:
                        Mes.Add("*I really love this outfit, but I preffer using my cloak too...*"); //"*[name] says that likes this outfit, but would like having her cloak on too.*");
                        break;
                }
            }*/
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*Yawn... Feeling sleepy, [nickname]? Me too.*"); //"*[name] looks sleepy.*");
                    Mes.Add("*Why I'm circling the room? I... Have no idea..*"); //"*[name] is circling the room, I wonder what for.*");
                }
            }
            /*switch (guardian.OutfitID)
            {
                case RedHoodOutfitID:
                    Mes.Add("*Now I'm ready for adventure.*"); //"*[name] says that now she's ready for adventure.*");
                    Mes.Add("*This outfit has everything: It's comfy and has style. What else could I want?*"); //"*[name] is saying that she finds this outfit comfy and style.*");
                    Mes.Add("*The cloak is the most important part of this outfit. I'd feel naked without it.*"); //"*[name] is saying that the cloak is the most important part of her outfit.*");
                    Mes.Add("*Hey, [nickname]. What do you think of my outfit?*"); //"*[name] asks what you think of her outfit.*");
                    break;
                case CloaklessOutfitID:
                    Mes.Add("*Now I'm ready for adventure.*"); //"*[name] says that now she's ready for adventure.*");
                    Mes.Add("*This outfit doesn't feel the same without the cloak...*"); //"*[name] seems to be missing the cloak.*");
                    Mes.Add("*Hey, [nickname]. What do you think of my outfit?*"); //"*[name] asks what you think of her outfit.*");
                    break;
            }*/
            Mes.Add("*There was a weird Terrarian I met once, named beaverrac. It was so weird that he didn't even tried to speak to me, but I can't blame him, since a lot of weird things were happening.*"); //"*[name] tells you of a Terrarian she met, named beaverrac. She said that found weird that he didn't talked with her, beside there were a lot of weird things happening around too.");
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*Let's dance, [nickname].* (She's stealing all the spotlights of the party.)"); //"*[name] is stealing all the spotlights of the party.*");
                if (PlayerMod.PlayerHasCompanionSummoned(Main.LocalPlayer, 3))
                {
                    Mes.Add("*Hey [gn:3], let's dance!*"); //"*[name] is calling [gn:3] for a dance.*");
                    Mes.Add("(She's is dancing with [gn:3], they seems to be enjoying.)");
                }
            }
            Player player = Main.LocalPlayer;
            if ((guardian.ID != 2 || guardian.ModID != MainMod.mod.Name) && !PlayerMod.PlayerHasCompanionSummoned(player, 2))
            {
                Mes.Add("*I'm so bored... I want to play a game, but nobody seems good enough for that...*"); //"*[name] is bored. She would like to play a game, but nobody seems good for that.*");
            }
            if (guardian.ID == 3 && guardian.ModID == MainMod.mod.Name && PlayerMod.PlayerHasCompanionSummoned(player, 2))
                Mes.Add("(First, [name] called [gn:3] to play a game, now they are arguing about what game they want to play. Maybe I should sneak away very slowly.)");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3) && PlayerMod.PlayerHasCompanionSummoned(player, 2))
                Mes.Add("*Hey, [nickname], may I borrow [gn:3] for a few minute? I want to play a game with [gn:2] and would love having his company.*"); //"*[name] is asking if she could borrow [gn:3] for a minute, so they could play a game with [gn:2].*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.WitchDoctor))
                Mes.Add("(She seems to be playing with flasks of poison.)");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                Mes.Add("*Check out my hair. I visitted [nn:"+Terraria.ID.NPCID.Stylist+"] and she did wonders to it.*"); //"*[name] wants you to check her hair.*");
            if (MainMod.HasCompanionInWorld(0))
                Mes.Add("*I really don't like talking to [gn:0], he's childish and annoying. I feel like I babysit him.*"); //"*[name] seems to be complaining about [gn:0], saying he's childish and annoying.*");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 0))
                Mes.Add("*Urgh... You came too... Nice...* (She doesn't seems to like having [gn:0]'s presence.)"); //"*[name]'s mood goes away as soon as she saw [gn:0].*");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3))
                Mes.Add("*Oh, hello. I'm glad to see you and [gn:3] visitting me...* (She looks a bit saddened)"); //"*[name] said that she feels good for knowing that [gn:3] is around, but she also looks a bit saddened.*");
            if (MainMod.HasCompanionInWorld(2))
                Mes.Add("*My teeth are itching right now. Do you know where [gn:2] is?*"); //"*[name] is saying that wants to bite something, and is asking If I've seen [gn:2] somewhere.*");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 2))
                Mes.Add("*Hey [gn:2], wanna play a game?* ([gn:2] ran away)"); //"*[name] said that she wants to play. For some reason, [gn:2] ran away.*");
            if (MainMod.HasCompanionInWorld(2) && MainMod.HasCompanionInWorld(5))
            {
                Mes.Add("(She is watching [gn:2] and [gn:5] playing together, with a worry face.)");
                Mes.Add("*Ever since [gn:5] arrived, I didn't had much chances of playing with [gn:2]...*"); //"*[name] says that didn't had much chances to play with [gn:2], since most of the time he ends up playing with [gn:5].*");
            }
            if (MainMod.HasCompanionInWorld(5) && !PlayerMod.PlayerHasCompanionSummoned(player, 5))
            {
                Mes.Add("(She's is whistling, like as if was calling a dog, and trying to hide the broom she's holding on her back.)");
                Mes.Add("*Alright, do tell that mutt [gn:5] that the next time he leaves a smelly surprise in my front door, I'll show him how resistant to impact is my broom!*"); //"*[name] is telling me that the next time [gn:5] leaves a smelly surprise on her front door, she'll chase him with her broom.*");
            }
            if (MainMod.HasCompanionInWorld(7))
                Mes.Add("*I really hate when [gn:7] interrupts me, when I'm playing with [gn:2]. She's just plain boring.*"); //"*[name] says that really hates when [gn:7] interrupts when playing with [gn:2].*");
            if (MainMod.HasCompanionInWorld(8))
            {
                Mes.Add("*The audacity [gn:8] have... Insulting my looks in my presence! How she dares!*"); //"*[name] is angry, because [gn:8] insulted her hair earlier.*");
                Mes.Add("*Who does [gn:8] think she is? I'm prettier than her!*"); //"*[name] is complaining about [gn:8], asking who she thinks she is.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Leopold))
            {
                Mes.Add("*I'm really happy for having [gn:10] around my arms... I mean... Around. Yes, around.*"); //"*[name] is very happy for having [gn:10] around.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Vladimir))
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer) && NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                {
                    Mes.Add("*[nn:"+Terraria.ID.NPCID.Nurse+"] came earlier to me, asking for tips for her date with [nn:"+Terraria.ID.NPCID.ArmsDealer+"]. Of course I had the perfect tip, I hope she executes it well.*"); //"*[name] tells that [nn:" + Terraria.ID.NPCID.Nurse + "] appeared earlier, asking for tips on what to do on a date with [nn:"+Terraria.ID.NPCID.ArmsDealer+"]. She said that she gave some tips that she can use at that moment.*");
                }
                if (!MainMod.HasCompanionInWorld(CompanionDB.Zacks))
                    Mes.Add("*Hey. Say... Have you seen [gn:"+CompanionDB.Vladimir+"]? I... Really need to see him...* (She seems to be wiping some tears from her face)"); //"*[name] asks If you have seen [gn:"+Vladimir+"], after removing a tear from her face. She seems to need to speak with him.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Michelle))
            {
                Mes.Add("*I really hate when [gn:"+CompanionDB.Michelle+"] pets my hair, she ruins my haircare.*"); //"*[name] says that hates when [gn:" + GuardianBase.Michelle + "] pets her hair.*");
                Mes.Add("*I keep telling [gn:"+CompanionDB.Michelle+"] that I need some space, but she just don't get it!*"); //"*[name] is saying taht needs some space, but [gn:" + GuardianBase.Michelle + "] doesn't get it.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Malisha))
            {
                Mes.Add("(She seems to have tried casting some spell on you) *Hm... It didn't worked. Did I do it right? Better I research* (The book cover says something about polymorphing.)"); //"*[name] seems to have casted some kind of spell on you, but It didn't seem to work. With a disappointment look, she tells herself that needs to research some more.*");
                if(!PlayerMod.PlayerHasCompanion(Main.LocalPlayer, CompanionDB.Zacks))
                    Mes.Add("(She seems to be reading some kind of magic book.)");
                else
                    Mes.Add("(She seems focused into reading books about necromancy and biology.)");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles))
            {
                Mes.Add("*I really enjoy having [gn:"+CompanionDB.Fluffles+"] around. She always comes up to check up if I'm fine.*"); //"*[name] seems to be enjoying having [gn:" + Fluffles + "] around. They seems to be get along very well.*");
                Mes.Add("*I've been sharing some beauty tips with [gn:"+CompanionDB.Fluffles+"]. Beside she can't speak, she managed to teach me some new tips related to that.*"); //"*[name] told you that she's sharing some beauty tips with [gn:" + Fluffles + "]. She said that learned something new with that.*");
                if (MainMod.HasCompanionInWorld(CompanionDB.Sardine))
                {
                    Mes.Add("*Playing Cat and Wolf with [gn:"+CompanionDB.Sardine+"] got more fun after I invited [gn:"+CompanionDB.Fluffles+"] to play too. She often catches him off guard, but that kind of makes the game easier.*"); //"*[name] says that always teams up with [gn:"+Fluffles+"] to catch [gn:"+Sardine+"] on Cat and Wolf. [gn:"+Fluffles+"] catches him off guard more easier than her, but she also said that the game got easier too.*");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Minerva))
            {
                Mes.Add("*I'm not in the mood now.... Grr....* (She seems to have came angry from [gn:17]'s place. I wonder what happened.)"); //"*[name] seems to have came from [gn:17]'s place angry. I wonder what happened.*");
                Mes.Add("(She seems to be eating a Squirrel on a Spit.) Oh, hi. I'm just nibbling something.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Luna))
            {
                Mes.Add("*I'm so happy to have [gn:" + CompanionDB.Luna + "] around. She has so many good points.*");
                Mes.Add("*Sometimes, [gn:" + CompanionDB.Luna + "] and I compare whose fur has better texture.*");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Cille))
            {
                Mes.Add("*I greeted [gn:" + CompanionDB.Cille + "] the other day, but she told me to go away.*");
                Mes.Add("*There is something wrong with [gn:" + CompanionDB.Cille + "], I visited her some night, and she attacked me! Then the other day, she was back to being the shy person we know. What is wrong with her?*");
            }
            if (Main.moonPhase == 0 && !Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("*I'm sorry for calling your attention, [nickname]. I wasn't actually calling you.*"); //"*[name] apologizes, saying that she wasn't calling you at the moment.*");
                Mes.Add("(She's staring at the moon.)");
            }
            if (HasBunnyInInventory(guardian))
            {
                Mes.Add("*How did you knew I loved bunnies? I really loved this gift. Thank you.*"); //"*[name] asks how did you know, and tells you that she loved the pet you gave her.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I was getting bored of staying at home, anyways. Let's go on an adventure!*";
                case JoinMessageContext.Fail:
                    return "*I'm not interessed in going on an adventure right now.*";
                case JoinMessageContext.FullParty:
                    return "*I dislike crowds.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Farewell, [nickname]. Remember to find the other wolf TerraGuardian I seek.*";
                case LeaveMessageContext.Fail:
                    return "*I'm going nowhere else right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*You really want to leave me here?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Well, I think It will be entertaining slashing my way back home. See you there, [nickname].*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Phew...*";
            }
            return base.LeaveGroupMessages(companion, context);
        }
        #endregion

        public static bool HasBunnyInInventory(Companion companion)
        {
            for(int i = 0; i < 50; i++)
            {
                if(companion.inventory[i].type == 0) continue;
                switch(companion.inventory[i].type)
                {
                    case ItemID.Bunny:
                    case ItemID.GemBunnyAmber:
                    case ItemID.GemBunnyAmethyst:
                    case ItemID.GemBunnyDiamond:
                    case ItemID.GemBunnyEmerald:
                    case ItemID.GemBunnyRuby:
                    case ItemID.GemBunnySapphire:
                    case ItemID.GemBunnyTopaz:
                    case ItemID.GoldBunny:
                        return true;
                }
            }
            return false;
        }
    }
}