using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class BlessedSoul : ModBuff
    {
        float GetBuffPower => Companions.LiebreBase.BlessedSoulBuffPower;
        const float MoveSpeedBuff = .1f;
        const float AttackSpeedBuff = .08f;
        const float DamageBuff = .04f;
        const int MaxHealthBuff = 40;

        public override void SetStaticDefaults()
        {
            Main.debuff[this.Type] = Main.pvpBuff[this.Type] = false;
            Main.buffNoSave[this.Type] = Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            float BuffPower = GetBuffPower;
            player.moveSpeed += .1f * BuffPower;
            player.GetAttackSpeed<GenericDamageClass>() += .08f * BuffPower;
            player.GetDamage<GenericDamageClass>() += .04f * BuffPower;
            player.statLifeMax2 += (int)(40 * BuffPower);
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            float BuffPower = GetBuffPower;
            tip = string.Format(tip, Math.Round(MoveSpeedBuff * BuffPower * 100, 1),
                    Math.Round(AttackSpeedBuff * BuffPower * 100, 1),
                    Math.Round(DamageBuff * BuffPower * 100, 1),
                    (int)(MaxHealthBuff * BuffPower),
                    (int)MathF.Max(1, Companions.LiebreBase.BlessedSoulBuffDuration * (1f /  60)));
        }
    }
}