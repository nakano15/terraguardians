using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class SubAttackBase
    {
        public virtual string Name {get { return "MissingNAME"; }}
        public virtual string Description { get { return "Description is missing!"; }}
        public virtual float Cooldown { get { return 0; } }
        public virtual bool AllowItemUsage { get { return false; } }
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

        public virtual void UpdateStatus(Companion User, SubAttackData Data)
        {

        }

        public virtual void Update(Companion User, SubAttackData Data)
        {

        }

        public virtual void UpdateAnimation(Companion User, SubAttackData Data)
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

        public void HurtCharactersInRectangle(Companion User, Rectangle Rect, int Damage, DamageClass DamageType, float Knockback, SubAttackData Data, int HitDirection = 0, byte Cooldown = 20)
        {
            int TotalCrit = (int)(User.GetTotalCritChance(DamageType) + 5E-06f);
            for(int i = 0; i < 255; i++)
            {
                if (i < 200 && Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, Main.npc[i], Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, HitDirection, Cooldown);
                }
                if (Main.player[i].active && Main.player[i] is not Companion && !Main.player[i].dead && !Main.player[i].ghost && User.IsHostileTo(Main.player[i]) && Main.player[i].Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, Main.player[i], Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, HitDirection, Cooldown);
                }
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (!c.dead && !c.ghost && User.IsHostileTo(c) && c.Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, c, Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, HitDirection, Cooldown);
                }
            }
        }

        public bool HurtCharacter(Companion User, Entity target, int Damage, bool Critical, DamageClass DamageType, float Knockback, SubAttackData Data, int HitDirection = 0, byte Cooldown = 8)
        {
            if (HitDirection == 0)
            {
                HitDirection = User.direction;
            }
            if (Data.IsInHitCooldown(target)) return false;
            Damage = (int)(MathF.Max(0, User.GetTotalDamage(DamageType).ApplyTo(Damage)));
            if (target is Player)
            {
                Player p = target as Player;
                int NewDamage = Main.DamageVar(Damage, User.luck);
                User.OnHit(p.Center.X, p.Center.Y, p);
                double dmg = p.Hurt(PlayerDeathReason.ByCustomReason(p.name + " was slain by " + User.name + "..."), NewDamage, HitDirection, true, false, knockback: Knockback);
                if (dmg > 0)
                {
                    Data.RegisterCooldown(target, Cooldown);
                }
                else
                {
                    return false;
                }
            }
            else if (target is NPC)
            {
                NPC n = target as NPC;
                if (n.dontTakeDamage || !User.CanNPCBeHitByPlayerOrPlayerProjectile(n))
                    return false;
                NPC.HitModifiers mod = n.GetIncomingStrikeModifiers(DamageType, HitDirection);
                NPC.HitInfo info = mod.ToHitInfo(Damage, Critical, Knockback, true, User.luck);
                //info.Damage = Damage;
                /*info.Crit = Critical;
                info.HitDirection = HitDirection;
                info.DamageType = DamageType;
                if (n.knockBackResist != 0)
                    info.Knockback = Knockback;
                info.Damage = Damage;*/
                int dmg = n.StrikeNPC(info);
                if (dmg > 0)
                {
                    Data.RegisterCooldown(target, Cooldown);
                }
                else
                {
                    return false;
                }
            }
            return true;
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
        private Dictionary<Entity, byte> HitCooldown = new Dictionary<Entity, byte>();
        private int Cooldown = 0;
        public bool IsInCooldown => Cooldown <= 0;

        public SubAttackData()
        {
        }

        public bool IsInHitCooldown(Entity entity)
        {
            return HitCooldown.ContainsKey(entity);
        }

        public void RegisterCooldown(Entity entity, byte Duration)
        {
            if (!HitCooldown.ContainsKey(entity))
            {
                HitCooldown.Add(entity, (byte)MathF.Max(1, Duration));
            }
            else
            {
                HitCooldown[entity] = (byte)MathF.Max(1, Duration);
            }
        }

        internal void SetSubAttackInfos(Companion User, byte Index, SubAttackBase Base)
        {
            this.User = User;
            SubAttackIndex = Index;
            _Base = Base;
            Base.OnInitialize(User, this);
        }

        public bool CheckAutoUseCondition(Companion User)
        {
            return Cooldown <= 0 && GetBase.AutoUseCondition(User, this);
        }

        public bool UseSubAttack()
        {
            if (Cooldown <= 0 && User.GetSubAttackInUse == 255 && (User.itemAnimation == 0 || GetBase.AllowItemUsage) && GetBase.CanUse(User, this))
            {
                _Active = true;
                User.GetSubAttackInUse = SubAttackIndex;
                TimePassed = 0;
                StepStartTime = 0;
                GetBase.OnBeginUse(User, this);
                if (User.controlUseItem && !GetBase.AllowItemUsage) User.controlUseItem = false;
                return true;
            }
            return false;
        }

        public void EndUse()
        {
            GetBase.OnEndUse(User, this);
            _Active = false;
            User.GetSubAttackInUse = 255;
            HitCooldown.Clear();
            Cooldown = (int)(GetBase.Cooldown * 60);
        }

        public void Update(Companion User)
        {
            if (!_Active)
            {
                if (Cooldown > 0) Cooldown--;
            }
            else
            {
                GetBase.Update(User, this);
                Entity[] Keys = HitCooldown.Keys.ToArray();
                foreach(Entity k in Keys)
                {
                    HitCooldown[k]--;
                    if (HitCooldown[k] == 0)
                        HitCooldown.Remove(k);
                }
                TimePassed++;
            }
        }

        public void UpdateStatus(Companion User)
        {
            GetBase.UpdateStatus(User, this);
        }

        public void UpdateAnimation(Companion User)
        {
            GetBase.UpdateAnimation(User, this);
        }

        public void PreDraw(Companion User, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            GetBase.PreDraw(User, this, ref drawSet, ref Holder);
        }

        public void Draw(Companion User, bool DrawingFront, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            GetBase.Draw(User, this, DrawingFront, drawSet, ref Holder, ref DrawDatas);
        }

        public void ChangeStep(byte StepID = 255)
        {
            StepStartTime = TimePassed;
            Step = StepID;
        }
    }
}