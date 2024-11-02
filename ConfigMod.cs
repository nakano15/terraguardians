using Terraria.ModLoader.Config;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class ClientConfiguration : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool UsePathFinding;

        [DefaultValue(false)]
        public bool ShowPathFinding;

        [DefaultValue(0)]
        public ReviveBarStyles ReviveBar;

        [DefaultValue(PlayerIndex.Two)]
        public PlayerIndex Index; //For 2P mode.

        [DefaultValue(MainMod.CompanionMaxDistanceFromPlayer.Normal)]
        public MainMod.CompanionMaxDistanceFromPlayer MaxDistanceFromPlayer;

        [DefaultValue(true)]
        public bool EnableProfanity;
        
        [DefaultValue(true)]
        public bool Show2PNotification;

        [DefaultValue(false)]
        public bool DisableHalloweenJumpscares;

        [DefaultValue(Cutscenes.FlufflesCatchPlayerCutscene.CutsceneType.Brief)]
        public Cutscenes.FlufflesCatchPlayerCutscene.CutsceneType FlufflesSceneType;

        public override void OnChanged()
        {
            MainMod.DisableHalloweenJumpscares = DisableHalloweenJumpscares;
            MainMod.UsePathfinding = UsePathFinding;
            ReviveInterface.ReviveBarStyle = (int)ReviveBar;
            MainMod.SecondPlayerPort = Index;
            MainMod.MaxDistanceFromPlayer = MaxDistanceFromPlayer;
            MainMod.Show2PNotification = Show2PNotification;
            MainMod.EnableProfanity = EnableProfanity;
            Cutscenes.FlufflesCatchPlayerCutscene.SceneType = FlufflesSceneType;
            MainMod.ShowPathFindingTags = ShowPathFinding;
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

        [DefaultValue(false)]
        public bool DebugMode;

        [DefaultValue(true)]
        public bool AllowModCompanions;

        [DefaultValue(false)]
        public bool PlayerKnockoutEnable;

        [DefaultValue(false)]
        public bool PlayerKnockoutColdEnable;

        [DefaultValue(true)]
        public bool CompanionKnockoutEnable;

        [DefaultValue(false)]
        public bool CompanionKnockoutColdEnable;

        [DefaultValue(false)]
        public bool PreventKnockedOutDeath;

        [DefaultValue(true)]
        public bool SkillsEnabled;

        [DefaultValue(0.1f)]
        public float DamageNerfByCompanionCount;

        [DefaultValue(30)]
        [Range(0, 50)]
        public int MaxCompanionTownNpcs;

        [DefaultValue(false)]
        public bool IndividualCompanionProgress;

        [DefaultValue(false)]
        public bool IndividualCompanionSkillProgress;

        [DefaultValue(false)]
        public bool SharedHealthAndManaProgress;

        [DefaultValue(false)]
        public bool CompanionsCanFaceBackgroundWhenIdle;

        [DefaultValue(false)]
        public bool UseNewCombatBehavior;

        [DefaultValue(false)]
        public bool TeleportInsteadOfRopePull;

        public override void OnChanged()
        {
            MainMod.DebugMode = DebugMode;
            MainMod.PlayerKnockoutEnable = PlayerKnockoutEnable;
            MainMod.PlayerKnockoutColdEnable = PlayerKnockoutColdEnable;
            MainMod.CompanionKnockoutEnable = CompanionKnockoutEnable;
            MainMod.CompanionKnockoutColdEnable = CompanionKnockoutColdEnable;
            MainMod.PreventKnockedOutDeath = PreventKnockedOutDeath;
            MainMod.DamageNerfByCompanionCount = DamageNerfByCompanionCount;
            MainMod.ShowBackwardAnimations = CompanionsCanFaceBackgroundWhenIdle;
            MainMod.SkillsEnabled = SkillsEnabled;
            bool CompanionProgressChanged = MainMod.IndividualCompanionProgress != IndividualCompanionProgress,
                CompanionSkillProgressChanged = MainMod.IndividualCompanionSkillProgress != IndividualCompanionSkillProgress;
            MainMod.IndividualCompanionProgress = IndividualCompanionProgress;
            MainMod.IndividualCompanionSkillProgress = IndividualCompanionSkillProgress;
            MainMod.SharedHealthAndManaProgress = SharedHealthAndManaProgress;
            MainMod.TeleportInsteadOfRopePull = TeleportInsteadOfRopePull;
            if (CompanionProgressChanged)
            {
                foreach (Companion c in MainMod.ActiveCompanions.Values)
                    c.RefreshLifeAndManaCrystalsUsed();
            }
            if (CompanionSkillProgressChanged)
            {
                CompanionInventoryInterface.RefreshCompanionInfos();
            }
            if (MainMod.DisableModCompanions == AllowModCompanions)
            {
                if(Terraria.Main.gameMenu)
                {
                    MainMod.DisableModCompanions = !AllowModCompanions;
                }
                else
                {
                    Terraria.Main.NewText("You can't change this while in-game.");
                }
            }
            if (WorldMod.MaxCompanionNpcsInWorld != MaxCompanionTownNpcs)
            {
                if(Terraria.Main.gameMenu)
                {
                    WorldMod.MaxCompanionNpcsInWorld = MaxCompanionTownNpcs;
                }
                else
                {
                    Terraria.Main.NewText("You can't change this while in-game.");
                }
            }
            CombatBehavior.UsingNewCombatBehavior = UseNewCombatBehavior;
        }
    }
}