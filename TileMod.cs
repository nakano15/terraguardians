using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Graphics;
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
    public class TileMod : GlobalTile
    {
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            DrawBehindTileCompanion();
            return base.PreDraw(i, j, type, spriteBatch);
        }

        private void DrawBehindTileCompanion()
        {
            if (!SystemMod.DrawCompanionBehindTileFlag)
            {
                return;
            }
            SystemMod.DrawCompanionBehindTileFlag = false;
            SamplerState samplerState = Main.Camera.Sampler;
            TerraGuardianDrawLayersScript.DrawingOnTiles = true;
            //Main.NewText("Position: " + PositionDiference.ToString());
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(c.GetDrawMomentType() == CompanionDrawMomentTypes.BehindTiles)
                {
                    if (c.InDrawRange()) c.DrawCompanion(UseSingleDrawScript: true);
                }
            }
            TerraGuardianDrawLayersScript.DrawingOnTiles = false;
        }

        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            foreach (BuildingInfo house in WorldMod.HouseInfos)
            {
                if (house.BelongsToThisHousing(i, j))
                {
                    house.UpdateTileState((ushort)type, i, j, true);
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                foreach (BuildingInfo house in WorldMod.HouseInfos)
                {
                    if (house.BelongsToThisHousing(i, j))
                    {
                        house.UpdateTileState((ushort)type, i, j, false);
                    }
                }
                TryTriggeringHG(type);
            }
        }

        void TryTriggeringHG(int Type)
        {
            switch (Type)
            {
                case TileID.Pots:
                case TileID.Heart:
                case TileID.LifeCrystalBoulder:
                case TileID.LifeFruit:
                    Interfaces.HallowsGreet.TryTriggerHallowsGreet(10);
                    break;
            }
        }

        public override void RightClick(int i, int j, int type)
        {
            switch (type)
            {
                case TileID.AmmoBox:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, BuffID.AmmoBox);
                    break;
                case TileID.BewitchingTable:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, BuffID.Bewitched);
                    break;
                case TileID.CrystalBall:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, BuffID.Clairvoyance);
                    break;
                case TileID.SharpeningStation:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, BuffID.Sharpened);
                    break;
                case TileID.WarTable:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, 348);
                    break;
                case TileID.SliceOfCake:
                    PlayerMod.ShareBuffAcrossCompanion(MainMod.GetLocalPlayer, BuffID.SugarRush);
                    break;
            }
        }
    }
}