
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions
{
    internal class AlexanderDefaultStatusBoosts
    {

        internal static void SetDefaultBonuses()
        {
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Rococo), RococoBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Blue), BlueBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Sardine), SardineBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Alex), AlexBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Brutus), BrutusBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Bree), BreeBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Mabel), MabelBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Domino), DominoBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Leopold), LeopoldBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Vladimir), VladimirBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Malisha), MalishaBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Wrath), WrathBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Fluffles), FlufflesBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Minerva), MinervaBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Daphne), DaphneBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Glenn), GlennBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.CaptainStench), CaptainStenchBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Liebre), LiebreBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Cinnamon), CinnamonBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Miguel), MiguelBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Luna), LunaBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Celeste), CelesteBuff);
            AlexanderBase.AddStatusBoost(new CompanionID(CompanionDB.Leona), LeonaBuff);
        }

        public static void RococoBuff(Companion c)
        {
            c.statDefense++;
        }
        
        public static void BlueBuff(Companion c)
        {
            c.GetAttackSpeed<MeleeDamageClass>() += .03f;
        }
        public static void SardineBuff(Companion c)
        {
            //should've been jump height but...
            c.DodgeRate += 3f;
        }
        public static void AlexBuff(Companion c)
        {
            c.moveSpeed += .04f;
        }
        public static void BrutusBuff(Companion c)
        {
            c.GetDamage<MeleeDamageClass>() += .02f;
            c.DefenseRate += .01f;
        }
        public static void BreeBuff(Companion c)
        {
            c.GetDamage<MeleeDamageClass>() += .02f;;
        }
        public static void MabelBuff(Companion c)
        {
            c.MaxHealth += 3;
        }
        public static void DominoBuff(Companion c)
        {
            c.GetDamage<RangedDamageClass>() += .03f;
        }
        public static void LeopoldBuff(Companion c)
        {
            c.GetDamage<MagicDamageClass>() += .03f;
        }
        public static void VladimirBuff(Companion c)
        {
            c.MaxHealth += 8;
            c.lifeRegen++;
        }
        public static void MalishaBuff(Companion c)
        {
            c.MaxMana +=5;
        }
        public static void WrathBuff(Companion c)
        {
            c.GetDamage<MeleeDamageClass>() += .02f;
        }
        public static void FlufflesBuff(Companion c)
        {
            c.DodgeRate += 3f;
        }
        public static void MinervaBuff(Companion c)
        {
            c.statDefense += 2;
        }
        public static void DaphneBuff(Companion c)
        {
            //Cover rate should return
            //c.GetDamage<RangedDamageClass>() += .03f;
        }
        public static void GlennBuff(Companion c)
        {
            c.Accuracy += .03f;
        }
        public static void CaptainStenchBuff(Companion c)
        {
            c.aggro -= 30;
        }
        public static void LiebreBuff(Companion c)
        {
            c.DodgeRate += 2;
            c.MaxHealth += 3;
        }
        public static void CinnamonBuff(Companion c)
        {
            c.lifeRegen++;
        }
        public static void MiguelBuff(Companion c)
        {
            c.GetDamage<MeleeDamageClass>() += .03f;
            c.DefenseRate += .02f;
        }
        public static void LunaBuff(Companion c)
        {
            c.moveSpeed += .03f;
        }
        public static void CelesteBuff(Companion c)
        {
            c.MaxMana += 8;
        }
        public static void LeonaBuff(Companion c)
        {
            c.GetCritChance<MeleeDamageClass>() += 3;
        }
    }
}