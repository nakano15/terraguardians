using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians
{
    public class ZacksPreRecruitZombieBossBehavior : BehaviorBase
    {
        uint ZacksID { get { return CompanionDB.Zacks; } }
        private bool IsKnownCompanion { get { return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, ZacksID); } }
        private byte BossLevel = 255;
        private int Damage = 5;

        public override string CompanionNameChange(Companion companion)
        {
            if(!IsKnownCompanion) return "Zombie Guardian";
            return base.CompanionNameChange(companion);
        }

        public override void UpdateStatus(Companion companion)
        {
            if(BossLevel == 255) BossLevel = GetBossLevel();
            switch(BossLevel)
            {
                default:
                    companion.statLifeMax2 = 3000;
                    Damage = 15;
                    companion.statDefense += 5;
                    break;
                case 1:
                    companion.statLifeMax2 = 4500;
                    Damage = 45;
                    companion.statDefense += 20;
                    break;
                case 2:
                    companion.statLifeMax2 = 9000;
                    Damage = 56;
                    companion.statDefense += 24;
                    break;
                case 3:
                    companion.statLifeMax2 = 18000;
                    Damage = 64;
                    companion.statDefense += 28;
                    break;
                case 4:
                    companion.statLifeMax2 = 36000;
                    Damage = 78;
                    companion.statDefense += 32;
                    break;
                case 5:
                    companion.statLifeMax2 = 42000;
                    Damage = 106;
                    companion.statDefense += 36;
                    break;
            }
            companion.noKnockback = true;
        }

        public byte GetBossLevel()
        {
            byte BossLevel = 0;
            if (NPC.downedMoonlord)
                BossLevel = 5;
            else if (NPC.downedGolemBoss)
                BossLevel = 4;
            else if (NPC.downedMechBossAny)
                BossLevel = 3;
            else if (Main.hardMode)
                BossLevel = 2;
            else if (NPC.downedBoss3)
                BossLevel = 1;
            return BossLevel;
        }
    }
}