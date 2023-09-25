using terraguardians;
using Terraria;

namespace terraguardians
{
    public class PersonalityBase
    {
        public virtual string Name { get { return ""; } }
        protected virtual CompanionDialogueContainer SetDialogueContainer { get { return new CompanionDialogueContainer(); } }
        private CompanionDialogueContainer _Dialogues = null;
        public CompanionDialogueContainer GetDialogues 
        {
            get
            {
                if (_Dialogues == null)
                    _Dialogues = SetDialogueContainer;
                return _Dialogues;
            }
        }
    }
}