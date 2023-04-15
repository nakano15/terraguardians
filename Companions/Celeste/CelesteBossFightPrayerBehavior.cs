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
        const int MaxPostPrayerTime = 5 * 30;

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
            for(int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && (Main.npc[n].boss || Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[Main.npc[n].type]))
                {
                    return;
                }
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