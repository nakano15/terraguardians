using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Celeste
{
    public class CelestePrayerBehavior : BehaviorBase
    {
        int PrayerTime = 0;
        const int PrayerTotalTime = 35 * 60;
        bool Praying = false;

        private readonly static string[] CelestePrayer = new string[]
        {
            "*Dear "+MainMod.TgGodName+", TerraGuardians creator.*",
            "*I call upon you to ask for your blessing.*",
            "*Please cast your paw blessing upon your children.*",
            "*And also cast your blessings upon those who live in harmony with them.*",
            "*Like you always wanted to happen.*",
            "*Thank you, "+MainMod.TgGodName+".*"
        };

        public override void Update(Companion companion)
        {
            if (companion.UsingFurniture)
                companion.LeaveFurniture();
            Praying = !Companion.Behaviour_AttackingSomething && !Companion.Behaviour_InDialogue && companion.itemAnimation == 0 && companion.velocity.X == 0 && companion.velocity.Y == 0;
            if(Praying)
            {
                PrayerTime++;
                const int PrayerSpeedhMoments = 5 * 60;
                if (PrayerTime % PrayerSpeedhMoments == 0)
                {
                    byte Times = (byte)(PrayerTime / PrayerSpeedhMoments - 1);
                    if (Times < CelestePrayer.Length)
                    {
                        companion.SaySomething(CelestePrayer[Times]);
                    }
                }
                if (PrayerTime >= PrayerTotalTime - 10 * 60)
                {
                    /*int BuffID = ModContent.BuffType<Buffs.TgGodClawBlessing>();
                    const int Time = 20 * 60 * 60;
                    foreach(Companion c in MainMod.ActiveCompanions.Values)
                        c.AddBuff(BuffID, Time);
                    for(int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active)
                        {
                            Main.player[p].AddBuff(BuffID, Time);
                        }
                    }*/
                    CelesteBase.PrayedToday = true;
                    CelesteBase.PrayerUnderEffect = true;
                    //Buff the world
                }
                if (PrayerTime >= PrayerTotalTime)
                {
                    Deactivate();
                }
            }
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