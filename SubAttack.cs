using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians
{
    public class SubAttackBase
    {
        public virtual string Name {get { return "MissingNAME"; }}
        public virtual string Description { get { return "Description is missing!"; }}
        public virtual float Cooldown { get { return 0; } }
        public virtual SubAttackData GetSubAttackData => new SubAttackData();

        public virtual bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            return false;
        }

        public virtual bool CanUse(Companion User, SubAttackData Data)
        {
            return true;
        }

        public virtual void OnBeginUse(Companion User, SubAttackData Data)
        {
            
        }

        public virtual void OnEndUse(Companion User, SubAttackData Data)
        {
            
        }

        public virtual void OnInitialize(Companion User, SubAttackData Data)
        {

        }

        public virtual void Update(Companion User, SubAttackData Data)
        {

        }
        public virtual void PreDraw(Companion User, SubAttackData Data, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void Draw(Companion User, SubAttackData Data, bool DrawingFront, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            
        }

        public virtual void OnChangeStep(Companion User, SubAttackData Data)
        {

        }
    }

    public class SubAttackData
    {
        private byte SubAttackIndex = 0;
        public byte GetSubAttackIndex => SubAttackIndex;
        private bool _Active = false;
        public bool IsActive => _Active;
        private int TimePassed = 0, StepStartTime = 0;
        public int GetTime => TimePassed;
        public int GetStepTime => TimePassed - StepStartTime;
        public float GetTimeSecs => TimePassed * (1f / 60);
        public float GetStepTimeSecs => (TimePassed - StepStartTime) * (1f / 60);
        private byte Step = 0;
        public byte GetStep => Step;
        private Companion User;
        public Companion GetUser => User;
        private SubAttackBase _Base;
        public SubAttackBase GetBase => _Base;

        public SubAttackData()
        {
        }

        internal void SetSubAttackInfos(Companion User, byte Index, SubAttackBase Base)
        {
            this.User = User;
            SubAttackIndex = Index;
            _Base = Base;
            Base.OnInitialize(User, this);
        }

        public bool UseSubAttack()
        {
            if (User.GetSubAttackInUse == 255 && GetBase.CanUse(User, this))
            {
                _Active = true;
                User.GetSubAttackInUse = SubAttackIndex;
                TimePassed = 0;
                StepStartTime = 0;
                GetBase.OnBeginUse(User, this);
                return true;
            }
            return false;
        }

        public void EndUse()
        {
            GetBase.OnEndUse(User, this);
            _Active = false;
            User.GetSubAttackInUse = 255;
        }
    }
}