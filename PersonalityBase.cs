using terraguardians;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class PersonalityBase //How to track mod owner?
    {
        public virtual string Name { get { return ""; } }
        protected virtual CompanionDialogueContainer SetDialogueContainer { get { return new CompanionDialogueContainer(); } }
        private CompanionDialogueContainer _Dialogues = null;
        public CompanionDialogueContainer GetDialogues 
        {
            get
            {
                if (_Dialogues == null)
                {
                    _Dialogues = SetDialogueContainer;
                }
                return _Dialogues;
            }
        }
    }
}