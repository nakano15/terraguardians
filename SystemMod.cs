using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class SystemMod : ModSystem
    {
        public static int HandyCounter = 0;
        private Player[] BackedUpPlayers = new Player[Main.maxPlayers];
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
            Dialogue.Unload();
        }

        private void BackupAndPlaceCompanionsOnPlayerArray(bool FollowersOnly = false)
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
                BackedUpPlayers[i] = Main.player[i];
            byte LastSlot = 254;
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(FollowersOnly && c.Owner == null) continue;
                Main.player[LastSlot] = c;
                c.whoAmI = LastSlot;
                LastSlot--;
                if(LastSlot == 0) break;
            }
        }

        private void RestoreBackedUpPlayers()
        {
            for(byte i = 0; i < Main.maxPlayers; i++)
                Main.player[i] = BackedUpPlayers[i];
            if(ProjMod.BackupMyPlayer > -1)
            {
                 Main.myPlayer = ProjMod.BackupMyPlayer;
                 ProjMod.BackupMyPlayer = -1;
            }
        }

        public override void PreUpdatePlayers()
        {
            UpdateActiveCompanions();
        }

        public override void PostUpdatePlayers()
        {
            Dialogue.Update();
            HandyCounter++;
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
            BackupAndPlaceCompanionsOnPlayerArray(true);
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