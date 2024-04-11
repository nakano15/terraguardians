using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions.Liebre
{
    public class LiebrePreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        public static byte EncounterTimes
        {
            get{
                return LiebreBase.EncounterTimes;
            }
            set
            {
                LiebreBase.EncounterTimes = value;
            }
        }
        bool PlayerLeft = false, SpottedPlayer = false, FinishedTalking = false;
        byte DialogueStep = 0;
        ushort TalkTime = 0;

        public override string CompanionNameChange(Companion companion)
        {
            return "???";
        }

        public override void Update(Companion companion)
        {
            bool Idle = true;
            if (!FinishedTalking)
            {
                if (!SpottedPlayer)
                {
                    if (Target != null)
                    {
                        if (!PlayerLeft)
                        {
                            switch (EncounterTimes)
                            {
                                case 0:
                                    companion.SaySomething("*A Terrarian..*");
                                    break;
                                case 1:
                                    companion.SaySomething("*Hm... Interesting place..*");
                                    break;
                                case 2:
                                    companion.SaySomething("*This... This place...*");
                                    break;
                                case 3:
                                    companion.SaySomething("*Terrarian, can we talk?*");
                                    break;
                            }
                        }
                        else
                        {
                            switch (EncounterTimes)
                            {
                                case 0:
                                    companion.SaySomething("*Is that Terrarian again..*");
                                    break;
                                case 3:
                                    companion.SaySomething("*Now we can talk?*");
                                    break;
                            }
                        }
                        TalkTime = 180;
                    }
                }
                else
                {
                    Vector2 Distance = Target.Center - companion.Center;
                    if (TalkTime > 0 && EncounterTimes == 0)
                    {
                        TalkTime--;
                        if (TalkTime <= 0)
                        {
                            if (Math.Abs(Distance.X) < 180f && Math.Abs(Distance.Y) < 120f &&
                                Collision.CanHitLine(Target.position, Target.width, Target.height, companion.position, companion.width, companion.height))
                            {
                                if (!PlayerLeft)
                                    companion.SaySomething("*Terrarian, can we talk?*");
                                else
                                    companion.SaySomething("*Please don't run away again, I must speak to you.*");
                            }
                            else
                            {
                                if (!PlayerLeft)
                                    companion.SaySomething("*Wait, Terrarian, I need to talk to you.*");
                                else
                                    companion.SaySomething("*Wait, don't go again!*");
                            }
                        }
                    }
                    if (Math.Abs(Distance.X) >= 600 || Math.Abs(Distance.Y) >= 240)
                    {
                        SpottedPlayer = false;
                        if (!PlayerLeft)
                            companion.SaySomethingCanSchedule("*I guess I scared them...*");
                        else
                            companion.SaySomethingCanSchedule("*They left... Again...*");
                        PlayerLeft = true;
                    }
                }
            }
            if (Idle)
                base.WanderAI(companion);
            SpottedPlayer = Target != null;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion))
            {
                return GetAlreadyMetMessage();
            }
            switch(EncounterTimes)
            {
                default:
                    return GetFirstEncounterMessage();
                case 1:
                    return GetSecondEncounterMessage();
                case 2:
                    return GetThirdEncounterMessage();
                case 3:
                    return GetFourthEncounterMessage();
            }
            return base.ChangeStartDialogue(companion);
        }

        MessageDialogue GetAlreadyMetMessage()
        {
            WorldMod.AddCompanionMet(GetOwner);
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Ah, you're here too. Well, now you know I'm here too.*");
            md.AddOption("Hello, again.", Dialogue.LobbyDialogue);
            return md;
        }

        MessageDialogue GetFirstEncounterMessage()
        {
            EncounterTimes = 0;
            MessageDialogue md = new MessageDialogue();
            if (!FinishedTalking)
            {
                md.ChangeMessage("*Please don't be afraid of my presence. I'm not here after your soul or anything.\nI'm here because TerraGuardians have been moving here.*");
                md.AddOption("Because TerraGuardians have been moving here?", Mes1_1);
            }
            else
            {
                md.ChangeMessage("*I will be moving away from here soon, if you're worried.*");
            }
            return md;
        }

        void Mes1_1()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Yes, I must ensure that in the case some of them ends up having their time up, I can take their soul to where they belong.*");
            md.AddOption("To where they belong?", Mes1_2);
            md.RunDialogue();
        }

        void Mes1_2()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Yes. Just like you, there is a place you'll be taken when you die. In the TerraGuardians case, it's a different place.*");
            md.AddOption("How different?", Mes1_3);
            md.RunDialogue();
        }

        void Mes1_3()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*I will not tell you. Anyways, I hope you don't mind if I stay around your world for the time being.*");
            md.AddOption("I don't mind at all.", Mes1_4_1);
            md.AddOption("Of course I mind! You're scary!", Mes1_4_2);
            md.RunDialogue();
        }

        void Mes1_4_1()
        {
            FinishedTalking = true;
            EncounterTimes++;
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Thank you. You will probably not even notice me around, or at least see me few times.*");
            md.RunDialogue();
        }

        void Mes1_4_2()
        {
            FinishedTalking = true;
            EncounterTimes++;
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Hmph. Don't worry, you wont notice me around. Maybe some times. Just be sure to not stay around wherever has a TerraGuardian about to die, then.*");
            md.RunDialogue();
        }

        MessageDialogue GetSecondEncounterMessage()
        {
            MessageDialogue md = new MessageDialogue();
            if (!FinishedTalking)
            {
                md.ChangeMessage("*So, we meet again, Terrarian. What kind of place is this? " + (WorldGen.crimson ? "I feel the energy of a godly creature in this organic place" : "This place seems infested with sickness and parasites") + "*");
                md.AddOption("This is the " + (WorldGen.crimson ? "Crimson" : "Corruption") + ".", Mes2_1);
            }
            else
            {
                md.ChangeMessage("*I'm still intrigued about this place, so It may take a while before I leave.*");
            }
            return md;
        }

        void Mes2_1()
        {
            MessageDialogue md = new MessageDialogue();
            if (WorldGen.crimson)
                md.ChangeMessage("*The Crimson? Hm, the name actually makes sense.*");
            else
                md.ChangeMessage("*The Corruption? Hm, interesting choice of name.*");
            md.AddOption("Continue", Mes2_2);
            md.RunDialogue();
        }

        void Mes2_2()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*I was exploring this world, since there isn't a need for me to take any action right now.*");
            md.AddOption("Exploring?", Mes2_3);
            md.RunDialogue();
        }

        void Mes2_3()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Yes. Whenever I'm not necessary, I like to explore the place I will harvest from. That way I don't feel bored, and also see new things.*");
            md.AddOption("That's actually cool.", Mes2_4);
            md.RunDialogue();
        }

        void Mes2_4()
        {
            FinishedTalking = true;
            EncounterTimes++;
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Indeed it is. Anyways, I will return to my travels.*");
            md.RunDialogue();
        }

        MessageDialogue GetThirdEncounterMessage()
        {
            MessageDialogue md = new MessageDialogue();
            if (!FinishedTalking)
            {
                md.ChangeMessage("*What... What is this place? This place... It's so horrible.*");
                md.AddOption("We call this the Dungeon.", Mes3_1);
            }
            else
            {
                md.ChangeMessage("*I still need some time to process this. I think better when I'm alone.*");
            }
            return md;
        }

        void Mes3_1()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Who built this place?*");
            md.AddOption("I have no idea. Why?", Mes3_2);
            md.RunDialogue();
        }

        void Mes3_2()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Can't you feel? The grudge, the horrors, the despair, the anger... Of all those souls...*");
            md.AddOption("I don't.", Mes3_3);
            md.RunDialogue();
        }

        void Mes3_3()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*I can understand that... You're not like me anyways... There's no way you could sense that...*");
            md.AddOption("Are you alright?", Mes3_4);
            md.RunDialogue();
        }

        void Mes3_4()
        {
            FinishedTalking = true;
            EncounterTimes++;
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*I'm sorry... But leave me be for now... I need to process all this..*");
            md.RunDialogue();
        }

        MessageDialogue GetFourthEncounterMessage()
        {
            MessageDialogue md = new MessageDialogue();
            if (!FinishedTalking)
            {
                md.ChangeMessage("*Terrarian, we need to talk.*");
                md.AddOption("Talk?", Mes4_1);
            }
            else
            {
                md.ChangeMessage("*Terrarian, haven't we introduced ourselves before?*");
            }
            return md;
        }

        void Mes4_1()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*At ease, Terrarian, It isn't trouble, but It will involve you.*");
            md.AddOption("Involve me?", Mes4_2);
            md.RunDialogue();
        }

        void Mes4_2()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*I spoke to the ones who sent me here about the dungeon, and they appointed me to do my job in this entire world.*");
            md.AddOption("You what?!", Mes4_3);
            md.RunDialogue();
        }

        void Mes4_3()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Yes. They want to avoid more souls from being twisted, by whatever caused that in the dungeon, so I will be making delivery of souls of the deceased from here, includding Terrarians.*");
            md.AddOption("I-Includding T-Terrarians?!", Mes4_4);
            md.RunDialogue();
        }

        void Mes4_4()
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage("*Don't worry, I will not harm anyone, at least unless something threatens to attack me. And reapers aren't allowed to inflate their soul contribution count, beside I wouldn't do that either.*");
            md.AddOption("Uh.. Okay...?", Mes4_5);
            md.RunDialogue();
        }

        void Mes4_5()
        {
            EncounterTimes = 0;
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, GetOwner);
            WorldMod.AddCompanionMet(GetOwner);
            Dialogue.LobbyDialogue("*I guess we will have enough time to know each other, or at least before your end of line. You can call me Liebre, which was my name, when I used to be among the living. I am now Terra Realm's reaper.*");
        }
    }
}