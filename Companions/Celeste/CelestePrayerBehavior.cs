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
        const int PrayerTotalTime = 30 * 60;
        bool Praying = false;

        public override void Update(Companion companion)
        {
            Praying = !Companion.Behaviour_AttackingSomething && !Companion.Behaviour_InDialogue && companion.itemAnimation == 0 && companion.velocity.X == 0 && companion.velocity.Y == 0;
            if(Praying)
            {
                PrayerTime++;
                if (PrayerTime >= PrayerTotalTime)
                {
                    int BuffID = ModContent.BuffType<Buffs.TgGodClawBlessing>();
                    const int Time = 24 * 60 * 60;
                    foreach(Companion c in MainMod.ActiveCompanions.Values)
                        c.AddBuff(BuffID, Time);
                    for(int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active)
                        {
                            Main.player[p].AddBuff(BuffID, Time);
                        }
                    }
                    //Buff the world
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
    }
}