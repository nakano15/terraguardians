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

        [Label("Knockout Fade Effect Type")]
        [Tooltip("If Knockout system on player is enabled, allows you to change the fading bar styles.")]
        [DefaultValue(0)]
        public ReviveBarStyles ReviveBar;

        public override void OnChanged()
        {
            MainMod.UsePathfinding = UsePathFinding;
            ReviveInterface.ReviveBarStyle = (int)ReviveBar;
        }

        public enum ReviveBarStyles : int
        {
            Jaws = 0,
            Bars = 1
        }
    }
    public class ServerConfiguration : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Debug Mode")]
        [Tooltip("Removes some locks like friendship level requirement for some things regarding companions. Yes, it's a cheat mode.")]
        [DefaultValue(false)]
        public bool DebugMode;

        /*[Label("Allow TerraGuardians Spawning?")]
        [Tooltip("Allows TerraGuardians to spawning naturally in the mod, either as recruitable companion or not.")]
        [DefaultValue(true)]
        public bool AllowTerraGuardians; //What about custom companions?

        [Label("Allow Terrarians Spawning?")]
        [Tooltip("Allows Terrarian companions to spawn naturally in the mod, either as recruitable companion or not.")]
        [DefaultValue(true)]
        public bool AllowTerrarians;*/

        [Label("Enable Knockout System for Players?")]
        [Tooltip("With this enabled, players will enter Knockout state if their health drops to 0 or under.")]
        [DefaultValue(false)]
        public bool PlayerKnockoutEnable;

        [Label("Players can enter Knockout Cold state?")]
        [Tooltip("With this enabled, players in Knockout state will enter Knockout Cold state if their health drops to 0 or under.")]
        [DefaultValue(false)]
        public bool PlayerKnockoutColdEnable;

        [Label("Enable Knockout System for Companions?")]
        [Tooltip("With this enabled, companions will enter Knockout state if their health drops to 0 or under.")]
        [DefaultValue(true)]
        public bool CompanionKnockoutEnable;

        [Label("Companions can enter Knockout Cold state?")]
        [Tooltip("With this enabled, companions in Knockout state will enter Knockout Cold state if their health drops to 0 or under.")]
        [DefaultValue(false)]
        public bool CompanionKnockoutColdEnable;

        public override void OnChanged()
        {
            MainMod.DebugMode = DebugMode;
            MainMod.PlayerKnockoutEnable = PlayerKnockoutEnable;
            MainMod.PlayerKnockoutColdEnable = PlayerKnockoutColdEnable;
            MainMod.CompanionKnockoutEnable = CompanionKnockoutEnable;
            MainMod.CompanionKnockoutColdEnable = CompanionKnockoutColdEnable;
        }
    }
}