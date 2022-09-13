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
                TestCompanion = new Companion();
                TestCompanion.whoAmI = player.whoAmI;
                TestCompanion.name = "Test";
                TestCompanion.InitializeCompanion();
                TestCompanion.hair = Main.rand.Next(1, 9);
                TestCompanion.immuneAlpha = 0;
                TestCompanion.Spawn(PlayerSpawnContext.SpawningIntoWorld);
                Main.NewText("Spawned " + TestCompanion.name + " at " + TestCompanion.position.ToString() + ".");
            }
        }

        public override void PostUpdate()
        {
            if(IsPlayerCharacter(Player) && TestCompanion != null)
            {
                TestCompanion.UpdateCompanion();
            }
        }

        /*public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if(IsPlayerCharacter(Player) && TestCompanion != null)
            {
                //Main.NewText("AAaaaa");
                Main.PlayerRenderer.DrawPlayers(Main.Camera, new Player[]{ TestCompanion });
            }
        }*/
    }
}