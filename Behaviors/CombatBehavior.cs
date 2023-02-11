using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class CombatBehavior : BehaviorBase
    {
        public bool EngagedInCombat = false;
        public ushort TargetMemoryTime = 0;
        const ushort MaxTargetMemory = 7 * 60;
        float AttackWidth = 0;
        byte StrongestMelee = 0, StrongestRanged = 0, StrongestMagic = 0, StrongestWhip = 0;
        public override void Update(Companion companion)
        {
            UpdateCombat(companion);
        }
        int SummonItemUsed = 0;

        public void UpdateCombat(Companion companion)
        {
            Entity Target = companion.Target;
            CheckSummons(companion);
            if(Target == null)
            {
                EngagedInCombat = false;
                return;
            }
            CombatTactics tactic = companion.CombatTactic;
            Vector2 FeetPosition = companion.Bottom;
            Vector2 TargetPosition = Target.Bottom;
            int TargetWidth = Target.width;
            int TargetHeight = Target.height;
            float HorizontalDistance = MathF.Abs(TargetPosition.X - FeetPosition.X) - (TargetWidth + companion.width) * 0.5f;
            float VerticalDistance = MathF.Abs(Target.Center.Y - companion.Center.Y) - (TargetHeight + companion.height) * 0.5f;
            if (!EngagedInCombat)
            {
                if (HorizontalDistance < 300 + (TargetWidth + companion.width) * 0.5f && 
                    VerticalDistance < 260 + (TargetHeight + companion.height) * 0.5f)
                {
                    EngagedInCombat = true;
                    TargetMemoryTime = MaxTargetMemory;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (HorizontalDistance >= 400 + TargetWidth * 0.5f || 
                    VerticalDistance >= 350 + TargetHeight * 0.5f || 
                    (Target is NPC && !(Target as NPC).behindTiles && 
                    !Collision.CanHitLine(companion.position, companion.width, companion.height, companion.Target.position, TargetWidth, TargetHeight)))
                {
                    TargetMemoryTime--;
                    if (TargetMemoryTime <= 0)
                    {
                        EngagedInCombat = false;
                        companion.Target = null;
                        return;
                    }
                }
                else
                {
                    TargetMemoryTime = MaxTargetMemory;
                }
            }
            Companion.Behaviour_AttackingSomething = true;
            Animation anim = companion.Base.GetAnimation(companion.Crouching ? AnimationTypes.CrouchingSwingFrames : AnimationTypes.ItemUseFrames);
            bool Danger = companion.statLife < companion.statLifeMax2 * 0.2f;
            if(!Companion.Behavior_UsingPotion && companion.itemAnimation == 0)
            {
                StrongestMelee = 0;
                StrongestRanged = 0;
                StrongestMagic = 0;
                StrongestWhip = 0;
                int HighestMeleeDamage = 0, HighestRangedDamage = 0, HighestMagicDamage = 0, HighestDamageWhip = 0;
                byte StrongestItem = 0;
                int HighestDamage = 0;
                for(byte i = 0; i < 10; i++)
                {
                    Item item = companion.inventory[i];
                    if(item.type > 0 && item.damage > 0 && CombinedHooks.CanUseItem(companion, item))
                    {
                        int DamageValue = companion.GetWeaponDamage(item);
                        if((item.useAmmo > 0 && !companion.HasAmmo(item)) || companion.statMana < companion.GetManaCost(item)) continue;
                        if(item.DamageType.CountsAsClass(DamageClass.Melee))
                        {
                            if (DamageValue > HighestMeleeDamage)
                            {
                                HighestMeleeDamage = DamageValue;
                                StrongestMelee = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Ranged))
                        {
                            if (item.ammo == 0 && DamageValue > HighestRangedDamage && companion.HasAmmo(item))
                            {
                                HighestRangedDamage = DamageValue;
                                StrongestRanged = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Magic))
                        {
                            if (DamageValue > HighestMagicDamage)
                            {
                                HighestMagicDamage = DamageValue;
                                StrongestMagic = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed))
                        {
                            if (DamageValue > HighestDamageWhip)
                            {
                                HighestDamageWhip = DamageValue;
                                StrongestWhip = i;
                            }
                        }
                        if(DamageValue > HighestDamage)
                        {
                            HighestDamage = DamageValue;
                            StrongestItem = i;
                        }
                    }
                }
                if (HighestMagicDamage > 0)
                {
                    companion.selectedItem = StrongestMagic;
                }
                else if (HighestRangedDamage > 0)
                {
                    companion.selectedItem = StrongestRanged;
                }
                else
                {
                    companion.selectedItem = StrongestItem;
                }
                if(HighestDamageWhip > 0) //Need to fix invisible whip issue first.
                {
                    if(HorizontalDistance < 160 && VerticalDistance < 160)
                    {
                        companion.selectedItem = StrongestWhip;
                    }
                }
                if (HighestMeleeDamage > 0)
                {
                    float ItemHeight = companion.GetAdjustedItemScale(companion.inventory[StrongestMelee]) * companion.inventory[StrongestItem].height;
                    float LowestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(1f)).Y + ItemHeight;
                    float HighestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.26f)).Y - ItemHeight * 1.5f;
                    AttackWidth = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.55f), AlsoTakePosition: false, DiscountDirections: false).X + ItemHeight;
                    /*for (byte i = 0; i < 4; i++) //For testing bounds of attack.
                    {
                        Vector2 Position;
                        switch(i)
                        {
                            default:
                                Position = new Vector2(companion.Center.X - AttackWidth, HighestHeight);
                                break;
                            case 1:
                                Position = new Vector2(companion.Center.X + AttackWidth, HighestHeight);
                                break;
                            case 2:
                                Position = new Vector2(companion.Center.X - AttackWidth, LowestHeight);
                                break;
                            case 3:
                                Position = new Vector2(companion.Center.X + AttackWidth, LowestHeight);
                                break;
                        }
                        Dust.NewDust(Position, 1, 1, 5, 0, 0);
                    }*/
                    if (HorizontalDistance < AttackWidth && TargetPosition.Y - TargetHeight < LowestHeight && TargetPosition.Y >= HighestHeight)
                    {
                        companion.selectedItem = StrongestMelee;
                    }
                }
            }
            float EvadeDistance, ApproachDistance, MeleeEvadeDistance;
            switch(tactic)
            {
                default:
                    ApproachDistance = 250;
                    EvadeDistance = 100;
                    MeleeEvadeDistance = 50;
                    break;
                case CombatTactics.CloseRange:
                    EvadeDistance = 0;
                    ApproachDistance = 0;
                    MeleeEvadeDistance = 0;
                    break;
                case CombatTactics.LongRange:
                    EvadeDistance = 250;
                    ApproachDistance = 400;
                    MeleeEvadeDistance = 150;
                    break;
            }
            if (Danger)
            {
                ApproachDistance += 80;
                EvadeDistance += 80;
                MeleeEvadeDistance += 120;
            }
            bool Left = false, Right = false, Attack = false, Jump = false;
            if(companion.HeldItem.type == 0 || Companion.Behavior_UsingPotion) //Run for your lives!
            {
                companion.WalkMode = HorizontalDistance < 150;
                if(HorizontalDistance < 200 + (TargetWidth + companion.width) * 0.5)
                {
                    if (FeetPosition.X > TargetPosition.X)
                        Right = true;
                    else
                        Left = true;
                }
                else
                {
                    if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                    {
                        companion.direction = FeetPosition.X < TargetPosition.X ? 1 : -1;
                    }
                }
                companion.AimAtTarget(Target.position + Target.velocity, Target.width, Target.height);
            }
            else
            {
                Vector2 AimDestination = Target.position + Target.velocity;
                if(companion.HeldItem.shoot > 0)
                {
                    float ShootSpeed = 1f / companion.HeldItem.shootSpeed;
                    Vector2 Direction = TargetPosition - companion.Center;
                    AimDestination += Target.velocity * ShootSpeed; //Direction;
                    if(companion.HeldItem.shoot == Terraria.ID.ProjectileID.WoodenArrowFriendly)
                    {
                        AimDestination.Y -= Direction.Length() * (1f / 16);
                    }
                }
                bool TargetInAim = companion.AimAtTarget(AimDestination, Target.width, Target.height);
                companion.WalkMode = false;
                bool IsWhip = companion.HeldItem.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed);
                if(companion.HeldItem.DamageType.CountsAsClass(DamageClass.Melee) || IsWhip)
                {
                    //Close Ranged Combat
                    float ItemSize = companion.GetAdjustedItemScale(companion.HeldItem);
                    float AttackRange = MeleeEvadeDistance + (TargetWidth - companion.width) * 0.5f + Math.Abs(companion.velocity.X);
                    float LowestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(1f)).Y ;
                    float HighestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.26f)).Y;
                    if (IsWhip)
                    {
                        AttackRange += 160;
                        LowestHeight += 160;
                        HighestHeight -= 160;
                    }
                    else
                    {
                        AttackRange += AttackWidth;
                        LowestHeight += ItemSize * companion.HeldItem.height;
                        HighestHeight -= ItemSize* companion.HeldItem.height * 1.5f;
                    }
                    if(TargetPosition.Y < HighestHeight)
                    {
                        Jump = true;
                    }
                    if(HorizontalDistance < AttackRange)
                    {
                        if (TargetPosition.Y - TargetHeight < LowestHeight)
                        {
                            if (TargetInAim && companion.CanHit(Target)) Attack = true;
                        }
                        bool TooClose = false;
                        if(HorizontalDistance < 15)//companion.width * 0.5f + 10)
                        {
                            TooClose = true;
                            if (FeetPosition.X > TargetPosition.X)
                            {
                                Right = true;
                            }
                            else
                            {
                                Left = true;
                            }
                        }
                        if(companion.itemAnimation == 0)
                        {
                            companion.direction = FeetPosition.X < TargetPosition.X ? 1 : -1;
                            Left = Right = false;
                            if(!TooClose && companion.Base.CanCrouch)
                            {
                                if(TargetPosition.Y - TargetHeight >= LowestHeight)
                                {
                                    companion.Crouching = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (FeetPosition.X < TargetPosition.X)
                        {
                            Right = true;
                        }
                        else
                        {
                            Left = true;
                        }
                    }
                }
                else if(companion.HeldItem.DamageType.CountsAsClass(DamageClass.Ranged) || 
                        companion.HeldItem.DamageType.CountsAsClass(DamageClass.Magic))
                {
                    if(HorizontalDistance >= ApproachDistance + (TargetWidth + companion.width) * 0.5f)
                    {
                        if(FeetPosition.X < TargetPosition.X)
                            Right = true;
                        else
                            Left = true;
                    }
                    else if(HorizontalDistance < EvadeDistance + (TargetWidth + companion.width) * 0.5f)
                    {
                        if(FeetPosition.X < TargetPosition.X)
                            Left = true;
                        else
                            Right = true;
                    }
                    if(companion.CanHit(Target))
                    {
                        if(TargetInAim)
                        {
                            Attack = true;
                        }
                    }
                }
            }
            if (Left != Right)
            {
                if(Left) companion.controlLeft = true;
                if(Right) companion.controlRight = true;
            }
            if(Jump && (companion.velocity.Y == 0 || Player.jumpHeight > 0))
                companion.ControlJump = true;
            if (Attack)
            {
                if(companion.itemAnimation == 0)
                {
                    //companion.ControlAction = true;
                    if(companion.DoTryAttacking())
                        companion.direction = companion.GetAimedPosition.X < companion.Center.X ? -1 : 1;
                }
                else
                {
                    if(companion.HeldItem.autoReuse)
                    {
                        companion.controlUseItem = true;
                    }
                    else if(companion.heldProj > -1)
                    {
                        Projectile proj = Main.projectile[companion.heldProj];
                        switch(proj.aiStyle)
                        {
                            case Terraria.ID.ProjAIStyleID.Yoyo:
                                companion.controlUseItem = true;
                                break;
                        }
                    }
                }
            }
        }

        private void CheckSummons(Companion c)
        {
            if (c.itemAnimation > 0) return;
            int NewSummon = 0;
            int SummonPosition = -1;
            int HighestDamage = 0;
            for(int i = 0; i < 10; i++)
            {
                Item item = c.inventory[i];
                if (item.type > 0 && item.DamageType.CountsAsClass<SummonDamageClass>())
                {
                    if (!item.sentry)
                    {
                        if (item.damage > HighestDamage)
                        {
                            HighestDamage = item.damage;
                            SummonPosition = i;
                            NewSummon = item.type;
                        }
                    }
                }
            }
            if (SummonItemUsed != NewSummon)
            {
                c.DespawnMinions();
            }
            if (SummonPosition > -1)
            {
                if (c.numMinions < c.maxMinions)
                {
                    c.selectedItem = SummonPosition;
                    c.controlUseItem = true;
                }
            }
            SummonItemUsed = NewSummon;
        }
    }
}