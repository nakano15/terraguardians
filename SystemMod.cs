using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class SystemMod : ModSystem
    {
        private static Point? MousePositionBackup = null;
        public static int HandyCounter = 0;
        private static Player[] BackedUpPlayers = new Player[Main.maxPlayers];
        private static CompanionMouseOverInterface CompanionMouseOverInterfaceDefinition;
        private static GroupMembersInterface GroupMembersInterfaceDefinition;
        private static CompanionInventoryInterface CompanionInventoryInterfaceDefinition;
        private static CompanionDialogueInterface CompanionDialogueInterfaceDefinition;
        private static CompanionOverheadTextAndHealthbarInterface CompanionOverheadTextAndHealthbarInterfaceDefinition;
        private static CompanionSelectionInterface CompanionSelectionInterfaceDefinition;
        private static CompanionHousesInWorldInterface CompanionHousesInWorldInterfaceDefinition;

        public override void Load()
        {
            CompanionMouseOverInterfaceDefinition = new CompanionMouseOverInterface();
            GroupMembersInterfaceDefinition = new GroupMembersInterface();
            CompanionInventoryInterfaceDefinition = new CompanionInventoryInterface();
            CompanionDialogueInterfaceDefinition = new CompanionDialogueInterface();
            CompanionOverheadTextAndHealthbarInterfaceDefinition = new CompanionOverheadTextAndHealthbarInterface();
            CompanionSelectionInterfaceDefinition = new CompanionSelectionInterface();
            CompanionHousesInWorldInterfaceDefinition = new CompanionHousesInWorldInterface();
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
            BackedUpPlayers = null;
            Dialogue.Unload();
        }

        public enum CompanionMaskingContext : byte
        {
            All = 0,
            FollowersOnly = 1,
            ChaseableByNpcsFollowerOnly = 2
        }

        public static void BackupAndPlaceCompanionsOnPlayerArray(CompanionMaskingContext context = CompanionMaskingContext.All)
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
                BackedUpPlayers[i] = Main.player[i];
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
                }
                if(Skip) continue;
                Main.player[LastSlot] = c;
                c.whoAmI = LastSlot;
                LastSlot--;
                if(LastSlot == 0) break;
            }
        }

        public static void RestoreBackedUpPlayers()
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
            {
                if (BackedUpPlayers[i] != null) Main.player[i] = BackedUpPlayers[i];
            }
            Main.myPlayer = MainMod.MyPlayerBackup;
        }

        public override void PreUpdatePlayers()
        {
        }

        public override void PostUpdatePlayers()
        {
            UpdateActiveCompanions();
            Dialogue.Update();
            HandyCounter++;
            WorldMod.RefreshCompanionInWorldCount();
        }

        private void UpdateActiveCompanions()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
            uint[] Keys = MainMod.ActiveCompanions.Keys.ToArray();
            foreach(uint i in Keys)
            {
                if(!MainMod.ActiveCompanions[i].active)
                {
                    MainMod.ActiveCompanions.Remove(i);
                }
                else
                {
                    MainMod.ActiveCompanions[i].UpdateCompanion();
                }
            }
            RestoreBackedUpPlayers();
        }

        public override void PreUpdateNPCs()
        {
            BackupAndPlaceCompanionsOnPlayerArray(CompanionMaskingContext.ChaseableByNpcsFollowerOnly);
        }

        public override void PostUpdateNPCs()
        {
            RestoreBackedUpPlayers();
        }

        public override void PreUpdateProjectiles()
        {
            BackupAndPlaceCompanionsOnPlayerArray();
        }

        public override void PostUpdateProjectiles()
        {
            RestoreBackedUpPlayers();
            RevertMousePosition();
        }

        public override void PreUpdateWorld()
        {
            WorldMod.PreUpdate();
        }

        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseInterfacePosition = -1, ResourceBarsPosition = -1, InventoryInterfacePosition = -1, 
                NpcChatPosition = -1, HealthbarsPosition = -1, TownNpcHouseBanners = -1;
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
                        break;
                    case "Vanilla: Inventory":
                        InventoryInterfacePosition = i;
                        break;
                    case "Vanilla: NPC / Sign Dialog":
                        NpcChatPosition = i;
                        break;
                    case "Vanilla: Entity Health Bars":
                        HealthbarsPosition = i;
                        break;
                    case "Vanilla: Town NPC House Banners":
                        TownNpcHouseBanners = i;
                        break;
                }
            }
            if(InventoryInterfacePosition > -1)
            {
                layers.Insert(InventoryInterfacePosition, CompanionSelectionInterfaceDefinition);
                layers.Insert(InventoryInterfacePosition, CompanionInventoryInterfaceDefinition);
            }
            if(MouseInterfacePosition > -1) layers.Insert(MouseInterfacePosition, CompanionMouseOverInterfaceDefinition);
            if(ResourceBarsPosition > -1) layers.Insert(ResourceBarsPosition, GroupMembersInterfaceDefinition);
            if(NpcChatPosition > -1) layers.Insert(NpcChatPosition, CompanionDialogueInterfaceDefinition);
            if(HealthbarsPosition > -1) layers.Insert(HealthbarsPosition, CompanionOverheadTextAndHealthbarInterfaceDefinition);
            if(TownNpcHouseBanners > -1) layers.Insert(TownNpcHouseBanners, CompanionHousesInWorldInterfaceDefinition);
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
        }

        public void Initialize()
        {
            MainMod.ActiveCompanions.Clear();
            WorldMod.OnInitializeWorldGen();
        }

        public override void PostDrawTiles()
        {
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(c.GetDrawMomentType() == CompanionDrawMomentTypes.AfterTiles)
                {
                    SpriteBatch spriteBatch = Main.Camera.SpriteBatch;
                    SamplerState samplerState = Main.Camera.Sampler;
                    if (c.mount.Active && c.fullRotation != 0f)
                    {
                        samplerState = LegacyPlayerRenderer.MountedSamplerState;
                    }
                    spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.Camera.Rasterizer, (Effect)null, Main.Camera.GameViewMatrix.TransformationMatrix);
                    c.DrawCompanion();
                    spriteBatch.End();
                }
            }
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

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
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
    }
}