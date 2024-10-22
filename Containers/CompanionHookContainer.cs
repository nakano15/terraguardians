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
        
        internal void SetOwningMod(Mod mod)
        {
            this.mod = mod;
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

        public virtual void ModifyLobbyDialogue(Companion Speaker, MessageDialogue Dialogue)
        {
            
        }

        public virtual void ModifyOtherTopicsDialogue(Companion Speaker, MessageDialogue Dialogue)
        {
            
        }

        public virtual void ModifyGamesDialogue(Companion Speaker, MessageDialogue Dialogue)
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