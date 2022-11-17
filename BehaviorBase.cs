using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class BehaviorBase
    {
        public virtual void Update(Companion companion)
        {
            
        }

        public virtual MessageBase ChangeDialogue(Companion companion)
        {
            return null;
        }

        public virtual bool AllowStartingDialogue(Companion companion)
        {
            return true;
        }
    }
}