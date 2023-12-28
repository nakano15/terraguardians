using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace terraguardians
{
    public class QuestBase
    {
        public virtual string Name => "";
        public virtual string QuestStory(QuestData data)
        {
            return "";
        }
        public virtual string GetQuestCurrentObjective(QuestData data)
        {
            return "";
        }
        public virtual bool IsQuestActive(QuestData data)
        {
            return false;
        }
        public virtual bool IsQuestFinished(QuestData data)
        {
            return false;
        }
        public virtual QuestData GetQuestData => new QuestData();
        bool Invalid = false;
        public bool IsInvalid => Invalid;

        public QuestBase()
        {

        }

        internal QuestBase(bool Invalid)
        {
            this.Invalid = true;
        }

        public virtual void AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID, MessageDialogue message)
        {
            
        }

        public virtual MessageBase ImportantDialogueMessage(QuestData data, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            return null;
        }

        public virtual void UpdatePlayer(Player player, QuestData data)
        {

        }

        public virtual void OnMobKill(NPC killedNpc, QuestData data)
        {

        }

        public virtual void OnTalkToNpc(NPC npc, QuestData data)
        {

        }

        public virtual string QuestNpcDialogue(NPC npc, QuestData data)
        {
            return "";
        }
    }
}