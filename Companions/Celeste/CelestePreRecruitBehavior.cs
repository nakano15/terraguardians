using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace terraguardians.Companions.Celeste
{
    //She's intended to arrive 9am. Start trying to pray 10am, and then try leaving 12pm.
    public class CelesteRecruitmentBehavior : PreRecruitBehavior
    {
        int BuffID = 0;
        byte PrayingState = 0;
        byte Delay = 0;
        byte PostSpawningWander = 0;
        const double PrayerBeginTime = 5.5f * 60; //10am
        const double LeavingTime = 7.5f * 60; //12pm

        public CelesteRecruitmentBehavior()
        {
            BuffID = ModContent.BuffType<Buffs.TgGodClawBlessing>();
            PostSpawningWander = (byte)Main.rand.Next(20, 41);
        }

        public override void Update(Companion companion)
        {
            if (Companion.Behavior_RevivingSomeone) return;
            if (PostSpawningWander > 0)
            {
                PostSpawningWander--;
                Wandering = true;
                ActionTime = PostSpawningWander;
                WanderAI(companion);
                return;
            }
            if (Delay > 0)
            {
                Delay --;
                if (!companion.IsRunningBehavior)
                    WanderAI(companion);
                return;
            }
            Delay = (byte)Main.rand.Next(12, 17); //To not feel robotic, lets add a bit of variability to when she'll check when to pray and stuff.
            if (!Main.dayTime || Main.time < PrayerBeginTime)
            {
                if (!companion.IsRunningBehavior)
                    WanderAI(companion);
                return;
            }
            if (Main.time >= PrayerBeginTime)
            {
                switch(PrayingState)
                {
                    case 0:
                        if (!companion.IsRunningBehavior)
                        {
                            companion.ClearBuff(BuffID);
                            companion.RunBehavior(new CelestePrayerBehavior());
                            PrayingState = 1;
                        }
                        break;
                    case 1:
                        if (companion.HasBuff(BuffID))
                        {
                            PrayingState = 2;
                        }
                        else
                        {
                            if (!companion.IsRunningBehavior && Main.time < LeavingTime)
                            {
                                companion.RunBehavior(new CelestePrayerBehavior());
                            }
                        }
                        break;
                    case 2:
                        if (!companion.IsRunningBehavior)
                            WanderAI(companion);
                            break;
                }
            }
        }

        public override bool AllowDespawning
        {
            get
            {
                return !Main.dayTime || Main.time >= LeavingTime;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return PrayingState != 1;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*Good morning, Terrarian. Allow me to introduce myself, I am "+MainMod.TgGodName+"'s Priestess, "+companion.GetNameColored()+", and I've came here bless your world.*");
            GetPossibleQuestions(m);
            return m;
        }

        bool AskedBlessWorld = false, AskedWhoTheirGodIs = false, AskedWhySheCame = false, AskAboutTgGodPossiblyLookingAtTheWorld = false;

        private void GetPossibleQuestions(MultiStepDialogue m)
        {
            if(!AskedBlessWorld) m.AddOption("Bless my world?", OnAskBlessWorld);
            if(!AskedWhoTheirGodIs) m.AddOption("Who's " + MainMod.TgGodName + "?", OnAskWhoTheirGodIs);
            if(!AskedWhySheCame) m.AddOption("Why you came here?", OnAskWhySheCame);
            if(AskedWhoTheirGodIs && !AskAboutTgGodPossiblyLookingAtTheWorld) m.AddOption("Rise "+MainMod.TgGodName + "'s awareness of this world?", OnAskAboutTgGodPossiblyLookingAtTheWorld);
            if (AskedBlessWorld && AskedWhoTheirGodIs && AskedWhySheCame && AskAboutTgGodPossiblyLookingAtTheWorld)
            {
                m.AddOption("That's all I had to ask.", FinishingDialogue);
            }
        }

        private void OnAskBlessWorld()
        {
            AskedBlessWorld = true;
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*Yes. I prayed for "+MainMod.TgGodName+" to spread his blessing upon the world and its inhabitants.*");
            m.AddDialogueStep("*That also means his blessing went to the Terrarians too, for treating well the TerraGuardians living here.*");
            GetPossibleQuestions(m);
            m.RunDialogue();
        }

        private void OnAskWhoTheirGodIs()
        {
            AskedWhoTheirGodIs = true;
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*He's the creator of the TerraGuardians. Said to be a giant, striped creature.*");
            m.AddDialogueStep("*In the beginning, TerraGuardians were created by him to be the protectors of the Terra Realms against the dangers of the outer worlds.*");
            m.AddDialogueStep("*Until a war among Terrarians and TerraGuardians made us move to the Ether Realm.*");
            m.AddDialogueStep("*Whatever the reason is causing those portals to appear, is making the TerraGuardians return here.*");
            m.AddDialogueStep("*And that probably is making "+MainMod.TgGodName+" very pleased to see Terrarians and TerraGuardians coexisting once again.*");
            GetPossibleQuestions(m);
            m.RunDialogue();
        }

        private void OnAskWhySheCame()
        {
            AskedWhySheCame = true;
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*The blessing of "+MainMod.TgGodName+" must be bestowed upon his children, and I am the one to relay it to them.*");
            m.AddDialogueStep("*This might also call his awareness towards this world, as more TerraGuardians appears.*");
            GetPossibleQuestions(m);
            m.RunDialogue();
        }

        private void OnAskAboutTgGodPossiblyLookingAtTheWorld()
        {
            AskAboutTgGodPossiblyLookingAtTheWorld = true;
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*Don't worry about that. From what I know, he likes Terrarians.*");
            m.AddDialogueStep("*The maximum you might worry about is being pranked on by him.*");
            m.AddDialogueStep("*But don't worry about that.*");
            GetPossibleQuestions(m);
            m.RunDialogue();
        }

        private void FinishingDialogue()
        {
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            Dialogue.LobbyDialogue("*I will keep returning to bestow "+MainMod.TgGodName+"'s blessing on this world. At least we could formally meet each other. \nI hope to see you again, Terrarian.*");
        }
    }
}
