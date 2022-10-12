using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace terraguardians
{
    public class PlayerMod : ModPlayer
    {
        public Companion TestCompanion = null;

        public override bool IsCloneable => false;
        protected override bool CloneNewInstances => false;

        public static bool IsPlayerCharacter(Player player)
        {
            return !(player is Companion) || ((Companion)player).IsPlayerCharacter;
        }

        public override void OnRespawn(Player player)
        {
            if(player is Companion)
            {
                ((Companion)player).OnSpawnOrTeleport();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if(IsPlayerCharacter(player)) //Character spawns, but can't be seen on the world.
            {
                TestCompanion = MainMod.SpawnCompanion(player.Bottom, 0);
                TestCompanion.whoAmI = player.whoAmI;
                TestCompanion.hair = Main.rand.Next(1, 9);
                TestCompanion.immuneAlpha = 0;
                TestCompanion.Owner = player.whoAmI;
                TestCompanion.inventory[0].SetDefaults(Terraria.ID.ItemID.FieryGreatsword);
                TestCompanion.inventory[1].SetDefaults(Terraria.ID.ItemID.ClockworkAssaultRifle);
                TestCompanion.inventory[2].SetDefaults(Terraria.ID.ItemID.EndlessMusketPouch);
            }
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            //Need to extend this to companions.
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if(Player is Companion)
            {
                playSound = false;
            }
            return true;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if(!Player.dead && Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.HurtSound, Player.position);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if(Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.DeathSound, Player.position);
            }
            if(Player is TerraGuardian)
            {
                ((TerraGuardian)Player).OnDeath();
            }
        }

        public override void PostUpdate()
        {
            
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }
    }
}