using System.Collections.Generic;
using Terraria;
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

        public override void OnEnterWorld(Player player)
        {
            if(IsPlayerCharacter(player)) //Character spawns, but can't be seen on the world.
            {
                TestCompanion = MainMod.GetCompanionBase(0).GetCompanionObject;
                TestCompanion.whoAmI = player.whoAmI;
                TestCompanion.name = "Test";
                TestCompanion.InitializeCompanion();
                TestCompanion.hair = Main.rand.Next(1, 9);
                TestCompanion.immuneAlpha = 0;
                TestCompanion.Owner = player.whoAmI;
                TestCompanion.Spawn(PlayerSpawnContext.SpawningIntoWorld);
                TestCompanion.inventory[0].SetDefaults(Terraria.ID.ItemID.CopperBroadsword);
                for(int i = 0; i < 10; i++)
                    Main.NewText(i + " = " + TestCompanion.inventory[i].Name);
            }
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            //Need to extend this to companions.
        }

        public override void PostUpdate()
        {
            if(IsPlayerCharacter(Player) && TestCompanion != null)
            {
                TestCompanion.UpdateCompanion();
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }
    }
}