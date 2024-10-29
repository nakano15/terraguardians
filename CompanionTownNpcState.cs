using Terraria;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace terraguardians
{
    public class CompanionTownNpcState
    {
        public CompanionID CharID = new CompanionID();
        public ushort GenericID = 0;
        public bool Homeless = true;
        public int HomeX = -1, HomeY = -1;
        public BuildingInfo HouseInfo;
        public Companion GetCompanion
        {
            get
            {
                foreach(Companion c in MainMod.ActiveCompanions.Values)
                {
                    if(c.IsSameID(CharID) && c.GetTownNpcState == this)
                    {
                        return c;
                    }
                }
                return null;
            }
        }

        public CompanionTownNpcState()
        {
            
        }

        public CompanionTownNpcState(uint ID, string ModID = "")
        {
            CharID = new CompanionID(ID, ModID);
        }

        public CompanionTownNpcState(CompanionID ID)
        {
            CharID = new CompanionID(ID);
        }

        public bool IsAtHome(Vector2 FeetPosition)
        {
            if(Homeless || HomeX == -1 || HomeY == -1 || HouseInfo == null)
            {
                return true;
            }
            FeetPosition.Y -= 2;
            FeetPosition *= 1f / 16;
            //Main.NewText("Feet Position: " + FeetPosition.ToString() + " House Pos X: " + HouseInfo.HouseStartX + "~" + HouseInfo.HouseEndX + "  Y: " + HouseInfo.HouseStartY + "~" + HouseInfo.HouseEndY);
            return FeetPosition.X >= HouseInfo.HouseStartX && FeetPosition.X < HouseInfo.HouseEndX && 
                FeetPosition.Y >= HouseInfo.HouseStartY && FeetPosition.Y < HouseInfo.HouseEndY;
        }

        public void KickCompanionOut()
        {
            if (HouseInfo != null)
            {
                HouseInfo.CompanionsLivingHere.Remove(this);
            }
            Homeless = true;
            HomeX = -1;
            HomeY = -1;
        }

        public void ValidateHouse()
        {
            if (HomeX == -1 || HomeY == -1 || Homeless)
            {
                Homeless = true;
                foreach(BuildingInfo ghi in WorldMod.HouseInfos)
                {
                    for (int i = 0; i < ghi.CompanionsLivingHere.Count; i++)
                    {
                        CompanionTownNpcState tns = ghi.CompanionsLivingHere[i];
                        if (tns == this)
                            ghi.CompanionsLivingHere.RemoveAt(i);
                    }
                }
                return;
            }
            foreach (BuildingInfo ghi in WorldMod.HouseInfos)
            {
                if(ghi.BelongsToThisHousing(HomeX, HomeY))
                {
                    if (ghi.ValidHouse)
                    {
                        Homeless = false;
                        HouseInfo = ghi;
                        bool CompanionAlreadyHere = false;
                        foreach(CompanionTownNpcState gtns in ghi.CompanionsLivingHere)
                        {
                            if(gtns.CharID.ID == CharID.ID && gtns.CharID.ModID == CharID.ModID)
                            {
                                CompanionAlreadyHere = true;
                                break;
                            }
                        }
                        if (!CompanionAlreadyHere)
                        {
                            ghi.CompanionsLivingHere.Add(this);
                        }
                    }
                    else
                    {
                        Homeless = true;
                    }
                    return;
                }
            }
            BuildingInfo newhouseinfo = new BuildingInfo();
            newhouseinfo.HomePointX = HomeX;
            newhouseinfo.HomePointY = HomeY;
            newhouseinfo.ValidateHouse();
            if (!newhouseinfo.ValidHouse)
            {
                HouseInfo = null;
                Homeless = true;
            }
            else
            {
                WorldMod.HouseInfos.Add(newhouseinfo);
                Homeless = false;
                HouseInfo = newhouseinfo;
            }
        }
    }
}