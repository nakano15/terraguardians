using System;
using Microsoft.Xna.Framework;
using Terraria;
using nterrautils;

namespace terraguardians
{
    public class QuestBase : nterrautils.QuestBase
    {

        public virtual void AddDialogueOptions(QuestData data, bool IsTalkDialogue, Companion companion, MessageDialogue message)
        {
            
        }

        public virtual MessageBase ImportantDialogueMessage(QuestData data, Companion companion)
        {
            return null;
        }
    }
}