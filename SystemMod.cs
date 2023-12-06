using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class SystemMod : ModSystem
    {
        internal static bool IsQuittingWorld = false;
        internal static bool DrawCompanionBehindTileFlag = true;
        private static Point? MousePositionBackup = null;
        public static int HandyCounter = 0;
        private static Player[] BackedUpPlayers = new Player[Main.maxPlayers];
        private static bool[] BackedUpPlayerDead = new bool[Main.maxPlayers];
        private static CompanionMouseOverInterface CompanionMouseOverInterfaceDefinition;
        private static GroupMembersInterface GroupMembersInterfaceDefinition;
        private static CompanionInventoryInterface CompanionInventoryInterfaceDefinition;
        private static CompanionDialogueInterface CompanionDialogueInterfaceDefinition;
        private static CompanionOverheadTextAndHealthbarInterface CompanionOverheadTextAndHealthbarInterfaceDefinition;
        private static CompanionSelectionInterface CompanionSelectionInterfaceDefinition;
        private static CompanionHousesInWorldInterface CompanionHousesInWorldInterfaceDefinition;
        private static CompanionPlayerHealthReplacerInterface CompanionPlayerHealthReplacerInterfaceDefinition;
        private static CompanionPlayerHotbarReplacerInterface CompanionPlayerHotbarReplacerInterfaceDefinition;
        private static ClearCompanionsFromPlayerListInterface ClearCompanionsFromPlayerListInterfaceDefinition;
        private static ReviveInterface ReviveInterfaceDefinition;
        private static VanillaMouseOverReplacerInterface VanillaMouseOverReplacerInterfaceDefinition;
        private static BuddyModeSetupInterface BuddyModeSetupInterfaceDefinition;
        private static Companion2PMouseInterface Companion2PMouseInterfaceDefinition;
        private static Interfaces.CompanionOrderInterface CompanionOrderInterfaceDefinition;
        private static uint LastScanTargetIndex = uint.MaxValue;

        public override void Load()
        {
            CompanionMouseOverInterfaceDefinition = new CompanionMouseOverInterface();
            GroupMembersInterfaceDefinition = new GroupMembersInterface();
            CompanionInventoryInterfaceDefinition = new CompanionInventoryInterface();
            CompanionDialogueInterfaceDefinition = new CompanionDialogueInterface();
            CompanionOverheadTextAndHealthbarInterfaceDefinition = new CompanionOverheadTextAndHealthbarInterface();
            CompanionSelectionInterfaceDefinition = new CompanionSelectionInterface();
            CompanionHousesInWorldInterfaceDefinition = new CompanionHousesInWorldInterface();
            ClearCompanionsFromPlayerListInterfaceDefinition = new ClearCompanionsFromPlayerListInterface();
            CompanionPlayerHealthReplacerInterfaceDefinition = new CompanionPlayerHealthReplacerInterface();
            CompanionPlayerHotbarReplacerInterfaceDefinition = new CompanionPlayerHotbarReplacerInterface();
            VanillaMouseOverReplacerInterfaceDefinition = new VanillaMouseOverReplacerInterface();
            ReviveInterfaceDefinition = new ReviveInterface();
            BuddyModeSetupInterfaceDefinition = new BuddyModeSetupInterface();
            Companion2PMouseInterfaceDefinition = new Companion2PMouseInterface();
            CompanionOrderInterfaceDefinition = new Interfaces.CompanionOrderInterface();
        }

        public override void Unload()
        {
            BackedUpPlayers = null;
            CompanionMouseOverInterfaceDefinition = null;
            GroupMembersInterfaceDefinition = null;
            CompanionInventoryInterfaceDefinition = null;
            CompanionDialogueInterfaceDefinition = null;
            CompanionOverheadTextAndHealthbarInterfaceDefinition = null;
            CompanionSelectionInterfaceDefinition = null;
            CompanionHousesInWorldInterfaceDefinition = null;
            ClearCompanionsFromPlayerListInterfaceDefinition = null;
            CompanionPlayerHealthReplacerInterfaceDefinition = null;
            CompanionPlayerHotbarReplacerInterfaceDefinition = null;
            VanillaMouseOverReplacerInterfaceDefinition = null;
            ReviveInterfaceDefinition = null;
            BuddyModeSetupInterfaceDefinition = null;
            BackedUpPlayers = null;
            BackedUpPlayerDead = null;
            Companion2PMouseInterfaceDefinition = null;
            CompanionOrderInterfaceDefinition = null;
            Dialogue.Unload();
        }

        public enum CompanionMaskingContext : byte
        {
            All = 0,
            FollowersOnly = 1,
            ChaseableByNpcsFollowerOnly = 2,
            AwakeFollowersOnly = 3
        }

        public static void BackupAndPlaceCompanionsOnPlayerArray(CompanionMaskingContext context = CompanionMaskingContext.All)
        {
            if (IsQuittingWorld) return;
            for(byte i = 0; i < Main.maxPlayers; i++)
            {
                BackedUpPlayers[i] = Main.player[i];
                BackedUpPlayerDead[i] = Main.player[i].dead;
                if (Main.player[i].active && (context == CompanionMaskingContext.ChaseableByNpcsFollowerOnly || context == CompanionMaskingContext.AwakeFollowersOnly) && (PlayerMod.GetPlayerKnockoutState(Main.player[i]) > KnockoutStates.KnockedOut || PlayerMod.PlayerGetControlledCompanion(Main.player[i]) != null))
                {
                    Main.player[i].dead = true;
                }
            }
            byte LastSlot = 254;
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                bool Skip = false;
                switch(context)
                {
                    case CompanionMaskingContext.FollowersOnly:
                        Skip = c.Owner == null;
                        break;
                    case CompanionMaskingContext.ChaseableByNpcsFollowerOnly:
                        Skip = c.Owner == null || !c.GetGoverningBehavior().CanBeAttacked;
                        break;
                    case CompanionMaskingContext.AwakeFollowersOnly:
                        Skip = c.Owner == null || !c.GetGoverningBehavior().CanBeAttacked;
                        if (!Skip && !c.dead)
                            c.dead = PlayerMod.GetPlayerKnockoutState(c) > KnockoutStates.KnockedOut;
                        break;
                }
                if(Skip) continue;
                Main.player[LastSlot] = c;

                /*if (Main.player[LastSlot].active && context == CompanionMaskingContext.ChaseableByNpcsFollowerOnly && (PlayerMod.GetPlayerKnockoutState(Main.player[LastSlot]) > KnockoutStates.Awake || PlayerMod.PlayerGetControlledCompanion(Main.player[LastSlot]) != null))
                {
                    Main.player[LastSlot].dead = true;
                }*/
                c.whoAmI = LastSlot;
                LastSlot--;
                if(LastSlot == 0) break;
            }
        }

        public static void RestoreBackedUpPlayers(bool PlayerUpdate = false)
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
            {
                if (BackedUpPlayers[i] != null)
                {
                    if (Main.player[i] is Companion)
                    {
                        Companion c = Main.player[i] as Companion;
                        if (c.KnockoutStates == KnockoutStates.KnockedOutCold)
                            c.dead = false;
                    }
                    Main.player[i] = BackedUpPlayers[i];
                    if (!PlayerUpdate || Main.player[i] != BackedUpPlayers[i])
                        Main.player[i].dead = BackedUpPlayerDead[i];
                }
            }
            Main.myPlayer = MainMod.MyPlayerBackup;
        }

        public override void PreUpdatePlayers()
        {
            //RestoreBackedUpPlayers();
        }

        public override void PostUpdatePlayers()
        {
            Main.myPlayer = MainMod.MyPlayerBackup;
            RevertMousePosition();
            if (IsQuittingWorld) return;
            UpdateActiveCompanions();
            Dialogue.Update();
            HandyCounter++;
            WorldMod.RefreshCompanionInWorldCount();
            MainMod.NemesisFadeEffect++;
            if (MainMod.NemesisFadeEffect >= MainMod.NemesisFadingTime)
            {
                MainMod.NemesisFadeEffect -= MainMod.NemesisFadingTime + MainMod.NemesisFadeCooldown;
            }
            DrawOrderInfo.Update();
            ModCompatibility.NExperienceModCompatibility.UpdatePlayerLevel(MainMod.GetLocalPlayer);
            BehaviorBase.UpdateAffectedCompanions();
        }

        private void UpdateActiveCompanions()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
            uint[] Keys = MainMod.ActiveCompanions.Keys.ToArray();
            bool PickedScanTarget = false;
            uint LastKey = 0;
            foreach(uint i in Keys)
            {
                if(!MainMod.ActiveCompanions[i].active)
                {
                    MainMod.ActiveCompanions.Remove(i);
                }
                else
                {
                    if (!PickedScanTarget && (LastScanTargetIndex == uint.MaxValue || LastScanTargetIndex < i))
                    {
                        Companion.ScanBiomes = true;
                        PickedScanTarget = true;
                        LastScanTargetIndex = i;
                    }
                    else
                    {
                        Companion.ScanBiomes = false;
                    }
                    MainMod.ActiveCompanions[i].UpdateCompanionVersion();
                    LastKey = i;
                }
            }
            if (LastKey == LastScanTargetIndex)
            {
                LastScanTargetIndex = uint.MaxValue;
            }
            RestoreBackedUpPlayers(true);
        }

        public override void PreUpdateNPCs()
        {
            BackupAndPlaceCompanionsOnPlayerArray(CompanionMaskingContext.AwakeFollowersOnly);
        }

        public override void PostUpdateNPCs()
        {
            NpcMod.UpdateLastAnyBossAlive();
            RestoreBackedUpPlayers();
            SardineBountyBoard.Update();
            NPCs.EtherPortal.CheckForPortalSpawning();
        }

        public override void PreUpdateProjectiles()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
        }

        public override void PostUpdateProjectiles()
        {
            RestoreBackedUpPlayers(true);
            Main.myPlayer = MainMod.MyPlayerBackup;
            RevertMousePosition();
        }

        public override void PreUpdateWorld()
        {
            WorldMod.PreUpdate();
        }

        public override void PostUpdateWorld()
        {
            AlexRecruitmentScript.UpdateTombstoneScript();
            Companions.CelesteBase.UpdateCelestePrayerStatus();
        }

        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseInterfacePosition = -1, ResourceBarsPosition = -1, InventoryInterfacePosition = -1, 
                NpcChatPosition = -1, HealthbarsPosition = -1, TownNpcHouseBanners = -1;
            bool HideInterfacesWhenKOd = false;
            {
                Player player = MainMod.GetLocalPlayer;
                Companion controlled = PlayerMod.PlayerGetControlledCompanion(player);
                if (controlled != null)
                {
                    player = controlled;
                }
                HideInterfacesWhenKOd = PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake;
            }
            for(int i = 0; i < layers.Count; i++)
            {
                switch(layers[i].Name)
                {
                    default: continue;
                    case "Vanilla: Mouse Text":
                        MouseInterfacePosition = i;
                        break;
                    case "Vanilla: Resource Bars":
                        ResourceBarsPosition = i;
                        if (HideInterfacesWhenKOd)
                            layers[i].Active = false;
                        if (PlayerMod.PlayerGetControlledCompanion(Main.LocalPlayer) != null)
                        {
                            layers[i].Active = false;
                            layers.Insert(i++, CompanionPlayerHealthReplacerInterfaceDefinition);
                        }
                        break;
                    case "Vanilla: Inventory":
                        InventoryInterfacePosition = i;
                        if (HideInterfacesWhenKOd)
                            layers[i].Active = false;
                        break;
                    case "Vanilla: Hotbar":
                        if (HideInterfacesWhenKOd)
                            layers[i].Active = false;
                        if (PlayerMod.PlayerGetControlledCompanion(Main.LocalPlayer) != null)
                        {
                            layers[i].Active = false;
                            layers.Insert(i++, CompanionPlayerHotbarReplacerInterfaceDefinition);
                        }
                        break;
                    case "Vanilla: NPC / Sign Dialog":
                    case "DialogueTweak: Reworked Dialog Panel": //Dialogue Tweak Support
                        if (HideInterfacesWhenKOd)
                            layers[i].Active = false;
                        NpcChatPosition = i;
                        break;
                    case "Vanilla: Entity Health Bars":
                        HealthbarsPosition = i;
                        break;
                    case "Vanilla: Town NPC House Banners":
                        if (HideInterfacesWhenKOd)
                            layers[i].Active = false;
                        TownNpcHouseBanners = i;
                        break;
                    case "Vanilla: Mouse Over":
                        //layers[i] = UpdateMouseOverCompanionEdition();
                        layers[i].Active = false;
                        if (!HideInterfacesWhenKOd) layers.Insert(i++, VanillaMouseOverReplacerInterfaceDefinition); //Necessary to increase index by 1, or else, every frame it will repeat this.
                        break;
                }
            }
            if(InventoryInterfacePosition > -1 && !HideInterfacesWhenKOd)
            {
                layers.Insert(InventoryInterfacePosition, CompanionSelectionInterfaceDefinition);
                layers.Insert(InventoryInterfacePosition, CompanionInventoryInterfaceDefinition);
                if (BuddyModeSetupInterface.IsActive)
                    layers.Insert(InventoryInterfacePosition, BuddyModeSetupInterfaceDefinition);
            }
            if(MouseInterfacePosition > -1)
            {
                if (MainMod.Gameplay2PMode || (MainMod.DebugMode && PlayerMod.PlayerGetControlledCompanion(MainMod.GetLocalPlayer) == null))
                {
                    layers.Insert(MouseInterfacePosition, Companion2PMouseInterfaceDefinition);
                }
                layers.Insert(MouseInterfacePosition, CompanionMouseOverInterfaceDefinition);
            }
            if(ResourceBarsPosition > -1) layers.Insert(ResourceBarsPosition, GroupMembersInterfaceDefinition);
            if(NpcChatPosition > -1) layers.Insert(NpcChatPosition, CompanionDialogueInterfaceDefinition);
            if(HealthbarsPosition > -1) layers.Insert(HealthbarsPosition, CompanionOverheadTextAndHealthbarInterfaceDefinition);
            if(TownNpcHouseBanners > -1) layers.Insert(TownNpcHouseBanners, CompanionHousesInWorldInterfaceDefinition);
            //layers.Insert(0, ClearCompanionsFromPlayerListInterfaceDefinition);
            if (Interfaces.CompanionOrderInterface.Active)
                layers.Insert(0, CompanionOrderInterfaceDefinition);
            layers.Insert(0, ReviveInterfaceDefinition);
        }

        public override void PreWorldGen() //Need to fix the issue with double characters appearing after creating a world and entering it.
        {
            Initialize();
        }

        public override void OnWorldLoad()
        {
            Initialize();
        }

        public override void OnWorldUnload()
        {
            Companion.ResetLastID();
            Interfaces.CompanionOrderInterface.Close();
        }

        public void Initialize()
        {
            MainMod.ActiveCompanions.Clear();
            WorldMod.OnInitializeWorldGen();
            SardineBountyBoard.Reset();
            IsQuittingWorld = false;
            for(int p = 0; p < 255; p++)
            {
                BackedUpPlayers[p] = null;
            }
        }

        public override void PostDrawTiles()
        {
            SpriteBatch spriteBatch = Main.Camera.SpriteBatch;
            SamplerState samplerState = Main.Camera.Sampler;
            spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.Camera.Rasterizer, (Effect)null, Main.Camera.GameViewMatrix.TransformationMatrix);
            //BackupAndPlaceCompanionsOnPlayerArray();
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (c.InDrawRange())
                {
                    c.DrawChain();
                    if(c.GetDrawMomentType() == CompanionDrawMomentTypes.AfterTiles)
                    {
                        if (c.mount.Active && c.fullRotation != 0f)
                        {
                            samplerState = LegacyPlayerRenderer.MountedSamplerState;
                        }
                        c.DrawCompanion();
                    }
                }
            }
            DrawCompanionBehindTileFlag = true;
            spriteBatch.End();
        }

        public static void BackupMousePosition()
        {
            if (!MousePositionBackup.HasValue)
            {
                MousePositionBackup = new Point(Main.mouseX, Main.mouseY);
            }
        }

        public static void RevertMousePosition()
        {
            if(MousePositionBackup.HasValue)
            {
                Main.mouseX = MousePositionBackup.Value.X;
                Main.mouseY = MousePositionBackup.Value.Y;
                MousePositionBackup = null;
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            WorldMod.ModifyWorldGenTasks(tasks, ref totalWeight);
        }

        public override void SaveWorldData(TagCompound tag)
        {
            WorldMod.SaveWorldData(tag);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            WorldMod.LoadWorldData(tag);
        }

        public override void PreSaveAndQuit()
        {
            RestoreBackedUpPlayers();
            for(int p = 0; p < 255; p++)
            {
                BackedUpPlayers[p] = null;
            }
            IsQuittingWorld = true;
        }

        public override void PreUpdateEntities()
        {
            RestoreBackedUpPlayers(true);
        }

        public override void PostUpdateEverything()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
        }

        private void Update2PMode()
        {
            
        }
    }
}