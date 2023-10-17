using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionHookContainer
    {
        Mod mod;
        internal string GetModName { get { return mod.Name; } }

        public CompanionHookContainer(Mod OwningMod)
        {
            mod = OwningMod;
        }
        
        public virtual void OnLoadSubAttacks(uint CompanionID, string CompanionModID, List<SubAttackBase> SubAttacks)
        {

        }

        public virtual CompanionSkinContainer OnLoadSkinsAndOutfitsContainer(uint CompanionID, string CompanionModID)
        {
            return null;
        }

        public virtual void OnLoadSpritesContainer(uint CompanionID, string CompanionModID, ref CompanionSpritesContainer sprites)
        {
            
        }

        public void Unload()
        {

        }

        protected virtual void OnUnload()
        {

        }
    }
}