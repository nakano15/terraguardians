using Terraria;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class SubAttackBase
    {
        public virtual string Name {get { return "MissingNAME"; }}
        public virtual string Description { get { return "Description is missing!"; }}
        public virtual float Cooldown { get { return 0; } }
        public virtual bool AllowItemUsage { get { return false; } }
        public virtual bool UseableWhenKnockedOut { get { return false; } }
        public virtual int ManaCost { get { return 0; } }
        public virtual SubAttackData GetSubAttackData => new SubAttackData();
        private Asset<Texture2D> Icon = null;
        internal void LoadIcon()
        {
            Type type = this.GetType();
            string ResourceDirectory = type.Namespace.Replace('.', '/') + "/" + type.Name;
            if (ModContent.HasAsset(ResourceDirectory))
            {
                Icon = ModContent.Request<Texture2D>(ResourceDirectory);
            }
            Load();
        }
        public Texture2D GetIcon => Icon != null ? Icon.Value : null;

        public void OnUnload()
        {
            Icon = null;
            Unload();
        }

        public int GetHighestWeaponDamage(Companion character, DamageClass dtype, float NonDamageTypeDamageMult = .75f)
        {
            int Highest = 0;
            for (int i = 0; i < 10; i++)
            {
                if (character.inventory[i].type > 0 && character.inventory[i].damage > 0 && character.inventory[i].useAnimation > 0)
                {
                    int Damage = 0;
                    if (character.inventory[i].DamageType.CountsAsClass(dtype))
                    {
                        Damage = character.inventory[i].damage;
                    }
                    else
                    {
                        Damage = (int)(character.inventory[i].damage * NonDamageTypeDamageMult);
                    }
                    if (Damage > Highest)
                    {
                        Highest = Damage;
                    }
                }
            }
            return Highest;
        }

        public Entity GetTargetInAimRange(Companion User, float MaxDistance = 50, bool GetHostiles = true, bool TakePlayers = true, bool TakeNpcs = true, bool TakeCompanions = true)
        {
            float NearestDistance = MaxDistance;
            Vector2 MouseDist = User.GetAimedPosition;
            Entity Target = null;
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && Main.player[i] != User && User.IsHostileTo(Main.player[i]) == GetHostiles && (TakeCompanions || Main.player[i] is not Companion) && (TakePlayers || Main.player[i] is Companion))
                {
                    float Distance = (Main.player[i].Center - MouseDist).Length();
                    if (Distance < NearestDistance)
                    {
                        Target = Main.player[i];
                        NearestDistance = Distance;
                    }
                }
                if (i < 200 && TakeNpcs && Main.npc[i].active && !Main.npc[i].friendly)
                {
                    float Distance = (Main.npc[i].Center - MouseDist).Length();
                    if (Distance < NearestDistance)
                    {
                        Target = Main.npc[i];
                        NearestDistance = Distance;
                    }
                }
            }
            return Target;
        }

        public virtual bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            return false;
        }

        public virtual bool CanUse(Companion User, SubAttackData Data)
        {
            return true;
        }

        public virtual void Load()
        {

        }

        public virtual void Unload()
        {

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

        public virtual bool PreHitAvoidDamage(Companion User, SubAttackData Data, Player.HurtInfo info)
        {
            return false;
        }

        public virtual void WhenHurt(Companion User, SubAttackData Data, Player.HurtInfo info)
        {
            
        }

        public virtual bool ImmuneTo(Companion User, SubAttackData Data, PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return false;
        }

        public void HurtCharactersInRectangle(Companion User, Rectangle Rect, int Damage, DamageClass DamageType, float Knockback, SubAttackData Data, int HitDirection = 0, byte Cooldown = 20)
        {
            HurtCharactersInRectangleAndGetTargets(User, Rect, Damage, DamageType, Knockback, Data, HitDirection, Cooldown);
        }

        public Entity[] HurtCharactersInRectangleAndGetTargets(Companion User, Rectangle Rect, int Damage, DamageClass DamageType, float Knockback, SubAttackData Data, int HitDirection = 0, byte Cooldown = 20, int CritRate = -1)
        {
            return HurtCharactersInRectangleAndGetTargets(User, Rect, Damage, DamageType, Knockback, Data, out int[] Dealt, HitDirection, Cooldown, CritRate);
        }

        public Entity[] HurtCharactersInRectangleAndGetTargets(Companion User, Rectangle Rect, int Damage, DamageClass DamageType, float Knockback, SubAttackData Data, out int[] DamageValues, int HitDirection = 0, byte Cooldown = 20, int CritRate = -1)
        {
            List<Entity> Targets = new List<Entity>();
            List<int> DamageDealt = new List<int>();
            int TotalCrit = (CritRate < 0 ? (int)(User.GetTotalCritChance(DamageType) + 5E-06f) : CritRate);
            for(int i = 0; i < 255; i++)
            {
                if (i < 200 && Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, Main.npc[i], Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, out int Dealt, HitDirection, Cooldown);
                    DamageDealt.Add(Dealt);
                    Targets.Add(Main.npc[i]);
                }
                if (Main.player[i].active && Main.player[i] is not Companion && !Main.player[i].dead && !Main.player[i].ghost && User.IsHostileTo(Main.player[i]) && Main.player[i].Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, Main.player[i], Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, out int Dealt, HitDirection, Cooldown);
                    DamageDealt.Add(Dealt);
                    Targets.Add(Main.player[i]);
                }
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (!c.dead && !c.ghost && User.IsHostileTo(c) && c.Hitbox.Intersects(Rect))
                {
                    HurtCharacter(User, c, Damage, Main.rand.Next(1, 101) <= TotalCrit, DamageType, Knockback, Data, out int Dealt, HitDirection, Cooldown);
                    DamageDealt.Add(Dealt);
                    Targets.Add(c);
                }
            }
            DamageValues = DamageDealt.ToArray();
            return Targets.ToArray();
        }

        public bool HurtCharacter(Companion User, Entity target, int Damage, bool Critical, DamageClass DamageType, float Knockback, SubAttackData Data, int HitDirection = 0, byte Cooldown = 8)
        {
            return HurtCharacter(User, target, Damage, Critical, DamageType, Knockback, Data, out int DamageDealt, HitDirection, Cooldown);
        }

        public bool HurtCharacter(Companion User, Entity target, int Damage, bool Critical, DamageClass DamageType, float Knockback, SubAttackData Data, out int DamageDealt, int HitDirection = 0, byte Cooldown = 8)
        {
            if (HitDirection == 0)
            {
                HitDirection = User.direction;
            }
            DamageDealt = -1;
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
                    DamageDealt = (int)dmg;
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
                int dmg = n.StrikeNPC(info);
                if (dmg > 0)
                {
                    DamageDealt = dmg;
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
        public bool IsInCooldown => Cooldown > 0;
        public float GetCooldown => Cooldown;
        bool IsNonLethal = false;

        public SubAttackData()
        {
        }

        public void ChangeCurrentCooldown(float Change)
        {
            ChangeCurrentCooldown((int)(Change * 60));
        }

        public void ChangeCurrentCooldown(int Change)
        {
            Cooldown += Math.Max(0, Cooldown + Change);
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
            IsNonLethal = false;
            this.User = User;
            SubAttackIndex = Index;
            _Base = Base;
            Base.OnInitialize(User, this);
        }

        public bool CheckAutoUseCondition(Companion User)
        {
            return Cooldown <= 0 && User.Mana >= GetBase.ManaCost && GetBase.AutoUseCondition(User, this);
        }

        public bool UseSubAttack(bool IgnoreCooldown = false, bool DoCooldown = true)
        {
            if ((IgnoreCooldown || Cooldown <= 0) && User.GetSubAttackInUse == 255 && User.Mana >= GetBase.ManaCost && (User.itemAnimation == 0 || GetBase.AllowItemUsage) && (GetBase.UseableWhenKnockedOut || User.KnockoutStates == KnockoutStates.Awake) && GetBase.CanUse(User, this))
            {
                _Active = true;
                User.GetSubAttackInUse = SubAttackIndex;
                TimePassed = 0;
                StepStartTime = 0;
                User.Mana -= GetBase.ManaCost;
                User.manaRegen = 0;
                if (DoCooldown)
                    Cooldown = (int)(GetBase.Cooldown * 60);
                GetBase.OnBeginUse(User, this);
                if (User.controlUseItem && !GetBase.AllowItemUsage) User.controlUseItem = false;
                return true;
            }
            return false;
        }

        public void EndUse()
        {
            EndUse(-1f);
        }

        public void EndUse(float Cooldown = -1f)
        {
            GetBase.OnEndUse(User, this);
            _Active = false;
            User.GetSubAttackInUse = 255;
            HitCooldown.Clear();
            if (Cooldown >= 0)
                this.Cooldown = (int)(Cooldown * 60);
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

        public bool PreHitAvoidDamage(Companion User, Player.HurtInfo info)
        {
            return GetBase.PreHitAvoidDamage(User, this, info);
        }

        public void WhenHurt(Companion User, Player.HurtInfo info)
        {
            GetBase.WhenHurt(User, this, info);
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