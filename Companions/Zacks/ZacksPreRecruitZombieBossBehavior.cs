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
                    companion.GetDamage<MagicDamageClass>() += .15f;
                    companion.statDefense += 5;
                    break;
                case 1:
                    companion.statLifeMax2 = 4500;
                    companion.GetDamage<MagicDamageClass>() += .45f;
                    companion.statDefense += 20;
                    break;
                case 2:
                    companion.statLifeMax2 = 9000;
                    companion.GetDamage<MagicDamageClass>() += .56f;
                    companion.statDefense += 24;
                    break;
                case 3:
                    companion.statLifeMax2 = 18000;
                    companion.GetDamage<MagicDamageClass>() += .64f;
                    companion.statDefense += 28;
                    break;
                case 4:
                    companion.statLifeMax2 = 36000;
                    companion.GetDamage<MagicDamageClass>() += .78f;
                    companion.statDefense += 32;
                    break;
                case 5:
                    companion.statLifeMax2 = 42000;
                    companion.GetDamage<MagicDamageClass>() += 1.06f;
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