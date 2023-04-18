using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Celeste
{
    public class CelesteBossFightPrayerBehavior : BehaviorBase
    {
        bool Praying = false;
        int BuffID = 0;
        int PostPrayerTime = 0;
        const int MaxPostPrayerTime = 5 * 60;
        int PrayerTime = 0;

        public CelesteBossFightPrayerBehavior()
        {
            BuffID = ModContent.BuffType<Buffs.TgGodTailBlessing>();
        }

        public override void Update(Companion companion)
        {
            if (companion.UsingFurniture)
                companion.LeaveFurniture();
            Praying = !Companion.Behaviour_AttackingSomething && !Companion.Behaviour_InDialogue && companion.itemAnimation == 0 && companion.velocity.X == 0 && companion.velocity.Y == 0;
            if(Praying)
            {
                const int Time = 5 * 60;
                if (PrayerTime >= Time * 4)
                {
                    foreach(Companion c in MainMod.ActiveCompanions.Values)
                        c.AddBuff(BuffID, Time);
                    for(int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active)
                        {
                            Main.player[p].AddBuff(BuffID, Time);
                        }
                    }
                }
                PrayerTime++;
                if (PrayerTime % Time == 0)
                {
                    int PrayerStep = (PrayerTime / Time) - 1;
                    switch(PrayerStep)
                    {
                        case 0:
                            companion.SaySomething("*Dear "+MainMod.TgGodName+", TerraGuardians Creator.*");
                            break;
                        case 1:
                            companion.SaySomething("*Your children are in danger, and so are the ones who live with them.*");
                            break;
                        case 2:
                            companion.SaySomething("*Please cast your Tail Blessing upon those brave warriors facing the menace.*");
                            break;
                        default:
                            switch(Main.rand.Next(4))
                            {
                                default:
                                    companion.SaySomething("*Please protect those warriors from harm.*");
                                    break;
                                case 1:
                                    companion.SaySomething("*I ask of you, please protect them.*");
                                    break;
                                case 2:
                                    companion.SaySomething("*Don't let those warriors fall before the threat.*");
                                    break;
                                case 3:
                                    companion.SaySomething("*Please "+MainMod.TgGodName+", protect them.*");
                                    break;
                            }
                            break;
                    }
                }
            }
            for(int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && (Main.npc[n].boss || Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[Main.npc[n].type]))
                {
                    return;
                }
            }
            if (PrayerTime > 0 && PostPrayerTime >= 2 * 60)
            {
                PrayerTime = 0;
                bool SomeoneAlive = false;
                for(int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        SomeoneAlive = true;
                    }
                }
                if (SomeoneAlive)
                    companion.SaySomething("*Thank you for protecting them, "+MainMod.TgGodName+".*");
                else
                    companion.SaySomething("*...May your reapers comfort their souls...*");
            }
            PostPrayerTime++;
            if (PostPrayerTime >= MaxPostPrayerTime)
                Deactivate();
        }

        public override bool AllowDespawning => false;

        public override void UpdateAnimationFrame(Companion companion)
        {
            if(!Praying) return;
            const short Frame = 11;
            companion.BodyFrameID = Frame;
            for(int i = 0; i < companion.ArmFramesID.Length; i++)
            {
                companion.ArmFramesID[i] = Frame;
            }
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return false;
        }
    }
}