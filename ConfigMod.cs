using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace terraguardians
{
    public class ClientConfiguration : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Use Path Finding System?")]
        [Tooltip("Enables the use of Path Finding system on the mod. If you're having lags, you can try disabling it.")]
        [DefaultValue(true)]
        public bool UsePathFinding;

        public override void OnChanged()
        {
            MainMod.UsePathfinding = UsePathFinding;
        }

    }
    public class ServerConfiguration : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Allow TerraGuardians Spawning?")]
        [Tooltip("Allows TerraGuardians to spawning naturally in the mod, either as recruitable companion or not.")]
        [DefaultValue(true)]
        public bool AllowTerraGuardians; //What about custom companions?

        [Label("Allow Terrarians Spawning?")]
        [Tooltip("Allows Terrarian companions to spawn naturally in the mod, either as recruitable companion or not.")]
        [DefaultValue(true)]
        public bool AllowTerrarians;
    }
}