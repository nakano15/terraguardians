using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Brutus
{
    public class BrutusPreRecruitmentBehavior : PreRecruitBehavior
    {
        TerraGuardian guardian;
        private int SteelTestingTime = 0;
        private const int MaxSteelTestingTime = 5;
        private int HirePrice
        {
            get
            {
                if (NPC.downedMoonlord) return 840;
                if (NPC.downedGolemBoss) return 320;
                if (NPC.downedPlantBoss) return 80;
                if (Main.hardMode) return 20;
                return 5;
            }
        }
        private byte Scene = 0;
        private int SceneTime = 0;
        public const byte SCENE_PLAYERWINS = 1,
            SCENE_PLAYERCHEATS = 2,
            SCENE_TIMEUP = 3;
        private Player PlayerHiringBrutus;
        bool AnnouncedSpawn = false;

        public static int ChanceCounter()
        {
            int Chance = 0;
            if (NPC.downedBoss1)
                Chance++;
            if (NPC.downedBoss2)
                Chance++;
            if (NPC.downedBoss3)
                Chance++;
            if (NPC.downedQueenBee)
                Chance++;
            if (NPC.downedSlimeKing)
                Chance++;
            if (Main.hardMode)
                Chance++;
            if (NPC.downedMechBoss1)
                Chance++;
            if (NPC.downedMechBoss2)
                Chance++;
            if (NPC.downedMechBoss3)
                Chance++;
            if (NPC.downedPlantBoss)
                Chance++;
            if (NPC.downedGolemBoss)
                Chance++;
            if (NPC.downedFishron)
                Chance++;
            if (NPC.downedMoonlord)
                Chance++;
            if (NPC.downedGoblins)
                Chance++;
            if (NPC.downedPirates)
                Chance++;
            if (NPC.downedMartians)
                Chance++;
            if (NPC.downedFrost)
                Chance++;
            return Chance;
        }

        public override void Update(Companion companion)
        {
            if (Companion.Behavior_RevivingSomeone) return;
            if (!AnnouncedSpawn)
            {
                NPC n = null;
                float nearest = float.MaxValue;
                for(int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].townNPC)
                    {
                        float distance = (Main.npc[i].Center - companion.Center).Length();
                        if (distance < nearest)
                        {
                            n = Main.npc[i];
                            nearest = distance;
                        }
                    }
                }
                if (n != null)
                {
                    Main.NewText("<Brutus> *If someone is interested in having a bodyguard, come see me "+MainMod.GetDirectionText(companion.Center - n.Center) +" of "+n.GivenOrTypeName+".*", MainMod.MysteryCloseColor.R, MainMod.MysteryCloseColor.G, MainMod.MysteryCloseColor.B);
                }
                else
                {
                    Main.NewText("<Brutus> *If someone is interested in having a bodyguard, come see me.*", MainMod.MysteryCloseColor.R, MainMod.MysteryCloseColor.G, MainMod.MysteryCloseColor.B);
                }
                AnnouncedSpawn = true;
            }
            if (SteelTestingTime > 0)
            {
                bool CaughtADebuff = false;
                for (int i = 0; i < companion.buffType.Length; i++)
                {
                    if (companion.buffType[i] > 0 && companion.buffType[i] != Terraria.ID.BuffID.PotionSickness && Main.debuff[companion.buffType[i]])
                    {
                        CaughtADebuff = true;
                        break;
                    }
                }
                if (CaughtADebuff)
                {
                    EndDamageTest();
                    PlayScene(SCENE_PLAYERCHEATS);
                }
                else
                {
                    SteelTestingTime--;
                    companion.FaceSomething(PlayerHiringBrutus);
                    if(companion.statLife <= 0)
                    {
                        EndDamageTest();
                        PlayScene(SCENE_PLAYERWINS);
                        companion.statLife = companion.statLifeMax2;
                    }
                    else if (SteelTestingTime <= 0)
                    {
                        EndDamageTest();
                        PlayScene(SCENE_TIMEUP);
                    }
                }
            }
            switch(Scene)
            {
                default:
                    {
                        if (SteelTestingTime > 0)
                            companion.FaceSomething(PlayerHiringBrutus);
                        else
                            WanderAI(companion);
                        SceneTime++;
                    }
                    break;
                case SCENE_TIMEUP:
                    {
                        EndDamageTest();
                        if (SceneTime == 0)
                            companion.SaySomething("*Growl! Times up.*");
                        else if (SceneTime == 180)
                            companion.SaySomething("*You're not strong enough to hire my blade.*");
                        else if (SceneTime == 360)
                        {
                            companion.SaySomething("*Show me how deep your pockets are, if you want to hire me in another way.*");
                            PlayScene(0);
                            break;
                        }
                        SceneTime++;
                    }
                    break;
                case SCENE_PLAYERWINS:
                    {
                        if (SceneTime == 0)
                        {
                            companion.FaceSomething(PlayerHiringBrutus);
                            companion.velocity.X -= companion.direction * 3.5f;
                            companion.velocity.Y -= 7.5f;
                            companion.SaySomething("*Rooow!! Ugh... Urgh...*");
                            SceneTime = 1;
                        }
                        else
                        {
                            if(companion.velocity.X == 0 && companion.velocity.Y == 0)
                            {
                                if (SceneTime == 180)
                                {
                                    companion.SaySomething("*I might have gotten a bit rusty..*");
                                }
                                else if (SceneTime == 480)
                                {
                                    companion.SaySomething("*Maybe helping you on your quest will help making me tougher again.*");
                                }
                                else if (SceneTime == 780)
                                {
                                    companion.SaySomething("*I am " + companion.GetRealName + ", and I am your body guard from now on.*");
                                    WorldMod.AddCompanionMet(CompanionDB.Brutus);
                                    PlayerMod.PlayerAddCompanion(PlayerHiringBrutus, CompanionDB.Brutus);
                                    CompanionData data = PlayerMod.PlayerGetCompanionData(PlayerHiringBrutus, CompanionDB.Brutus);
                                    companion.IncreaseFriendshipPoint(1);
                                }
                                SceneTime++;
                            }
                        }

                    }
                    break;

                case SCENE_PLAYERCHEATS:
                    {
                        if (SceneTime == 0)
                        {
                            EndDamageTest();
                            companion.velocity.Y = -5.5f;
                            companion.FaceSomething(PlayerHiringBrutus);
                            companion.SaySomething("*ROAR!*");
                            SceneTime++;
                        }
                        else
                        {
                            if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                            {
                                SceneTime++;
                                bool PlayerPickedUp = SceneTime >= 30;
                                if (SceneTime == 30)
                                    companion.SaySomething("*I said no tricks, idiot.*");
                                short RightArmFrame = -1;
                                byte t = 0;
                                const int FrameTime = 8;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(0);
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(1);
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(2);
                                    if (SceneTime == 60 + t * FrameTime + FrameTime / 2)
                                    {
                                        int Damage = (int)((70f / PlayerHiringBrutus.statLifeMax2) * PlayerHiringBrutus.statLifeMax2);
                                        PlayerHiringBrutus.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(PlayerHiringBrutus.name + " has got what they deserved."), Damage, companion.direction);
                                    }
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(1);
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(0);
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    PlayScene(0);
                                }

                                if (PlayerPickedUp)
                                {
                                    Vector2 Position = companion.GetAnimationPosition(AnimationPositions.HandPosition, RightArmFrame, 1);
                                    Position.X -= PlayerHiringBrutus.width * 0.5f;
                                    Position.Y -= PlayerHiringBrutus.height * 0.5f;
                                    PlayerHiringBrutus.position = Position;
                                    PlayerHiringBrutus.fallStart = (int)(Position.Y * (1f / 16));
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public override void UpdateStatus(Companion companion)
        {
            if (SteelTestingTime > 0)
                companion.noKnockback = true;
            companion.statLifeMax2 = companion.Base.InitialMaxHealth;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if(SteelTestingTime > 0)
            {
                companion.ArmFramesID[1] = 5;
                return;
            }
            switch(Scene)
            {
                case SCENE_PLAYERWINS:
                    {
                        if (SceneTime < 480 && companion.velocity.X == 0 && companion.velocity.Y == 0)
                        {
                            short Frame = companion.Base.GetAnimation(AnimationTypes.CrouchingFrames).GetFrame(0);
                            companion.BodyFrameID = Frame;
                            companion.ArmFramesID[0] = companion.ArmFramesID[1] = Frame;
                        }
                    }
                    break;

                case SCENE_PLAYERCHEATS:
                    {
                        short RightArmFrame = -1;
                        byte t = 0;
                        const int FrameTime = 8;
                        if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                        {
                            RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(0);
                        }
                        t++;
                        if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                        {
                            RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(1);
                        }
                        t++;
                        if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                        {
                            RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(2);
                            if (SceneTime == 60 + t * FrameTime + FrameTime / 2)
                            {
                                int Damage = (int)((70f / PlayerHiringBrutus.statLifeMax2) * PlayerHiringBrutus.statLifeMax2);
                                PlayerHiringBrutus.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(PlayerHiringBrutus.name + " has got what they deserved."), Damage, companion.direction);
                            }
                        }
                        t++;
                        if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                        {
                            RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(1);
                        }
                        t++;
                        if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                        {
                            RightArmFrame = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrame(0);
                        }
                        if (RightArmFrame > -1) companion.ArmFramesID[1] = RightArmFrame;
                    }
                    break;
            }
        }

        public override string CompanionNameChange(Companion companion)
        {
            return "Lion TerraGuardian";
        }

        public override bool IsHostileTo(Player target)
        {
            return SteelTestingTime > 0;
        }

        private void PlayScene(byte Scene)
        {
            this.Scene = Scene;
            SceneTime = 0;
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return SteelTestingTime <= 0 && Scene == 0;
        }

        public void StartDamageTest()
        {
            SteelTestingTime = MaxSteelTestingTime * 60;
            guardian.statLife = guardian.statLifeMax2;
            for (int b = 0; b < guardian.buffTime.Length; b++)
            {
                if (guardian.buffType[b] > 0 && guardian.buffType[b] != Terraria.ID.BuffID.PotionSickness)
                    guardian.DelBuff(b);
            }
            guardian.SaySomething("*Show me what you are made of, and don't use dirty tricks.*");
            CanBeAttacked = true;
            AllowSeekingTargets = false;
            UseHealingItems = false;
        }

        public void EndDamageTest()
        {
            SteelTestingTime = 0;
            guardian.statLife = guardian.statLifeMax2;
            for (int b = 0; b < guardian.buffTime.Length; b++)
            {
                if (guardian.buffType[b] > 0 && guardian.buffType[b] != Terraria.ID.BuffID.PotionSickness)
                    guardian.DelBuff(b);
            }
            CanBeAttacked = false;
            AllowSeekingTargets = true;
            UseHealingItems = true;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            guardian = (companion as TerraGuardian);
            MessageDialogue m = new MessageDialogue();
            if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion))
            {
                m.ChangeMessage("*We still have a contract up. You don't need to pay me again for my job.*");
                m.AddOption("That's great.", DialogueRecruitKnownCompanion);
            }
            else
            {
                m.ChangeMessage("*I want to test your steel. If you are able to do so in 5 seconds, he'll join you.\nUse poison or any other cheap method and you'll regret it.\nOr you may just pay me to be your bodyguard. You choose.*");
                m.AddOption("Let's do this!", DialogueStartSteelTesting);
                m.AddOption("Pay " + HirePrice + " Gold Coins to hire.", DialoguePayForHire);
                m.AddOption("Not now.", Dialogue.EndDialogue);
            }
            return m;
        }

        private void DialogueRecruitKnownCompanion()
        {
            WorldMod.AddCompanionMet(CompanionDB.Brutus);
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Brutus);
            Dialogue.LobbyDialogue();
        }

        private void DialogueStartSteelTesting()
        {
            PlayerHiringBrutus = MainMod.GetLocalPlayer;
            StartDamageTest();
            Dialogue.EndDialogue();
        }

        private void DialoguePayForHire()
        {
            if(MainMod.GetLocalPlayer.BuyItem(Item.buyPrice(0, HirePrice)))
            {
                PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, CompanionDB.Brutus);
                WorldMod.AddCompanionMet(CompanionDB.Brutus);
                CompanionData data = PlayerMod.PlayerGetCompanionData(MainMod.GetLocalPlayer, CompanionDB.Brutus);
                guardian.IncreaseFriendshipPoint(1);
                MessageDialogue m = new MessageDialogue("*I accept the offer. I, "+data.GetName+", will protect you until the end of my contract.*");
                m.AddOption("Thanks.", Dialogue.LobbyDialogue);
                m.RunDialogue();
            }
            else
            {
                MessageDialogue m = new MessageDialogue("*The only way you can hire my blade is by either showing how strong you are, or how deep your pockets are. Your pockets aren't deep enough right now, but you can still show me how strong you are.*");
                m.AddOption("Sorry.", Dialogue.EndDialogue);
                m.RunDialogue();
            }
        }

        public override bool CanKill(Companion companion)
        {
            return SteelTestingTime <= 0;
        }
    }
}
