using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class CombatBehavior : BehaviorBase
    {
        public override void Update(Companion companion)
        {
            UpdateCombat(companion);
        }

        public void UpdateCombat(Companion companion)
        {Entity Target = companion.Target;
            if(Target == null) return;
            Companion.Behaviour_AttackingSomething = true;
            Vector2 FeetPosition = companion.Bottom;
            Vector2 TargetPosition = Target.Bottom;
            int TargetWidth = Target.width;
            int TargetHeight = Target.height;
            float HorizontalDistance = MathF.Abs(TargetPosition.X - FeetPosition.X) - (TargetWidth + companion.width) * 0.5f;
            float VerticalDistance = MathF.Abs(Target.Center.Y - companion.Center.Y) - (TargetHeight + companion.height) * 0.5f;
            if(companion.itemAnimation == 0)
            {
                byte StrongestMelee = 0, StrongestRanged = 0, StrongestMagic = 0;
                int HighestMeleeDamage = 0, HighestRangedDamage = 0, HighestMagicDamage = 0;
                byte StrongestItem = 0;
                int HighestDamage = 0;
                for(byte i = 0; i < 10; i++)
                {
                    Item item = companion.inventory[i];
                    if(item.type > 0 && item.damage > 0 && CombinedHooks.CanUseItem(companion, item))
                    {
                        int DamageValue = companion.GetWeaponDamage(item);
                        if(!companion.HasAmmo(item) || companion.statMana < companion.GetManaCost(item)) continue;
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
                        if(DamageValue > HighestDamage)
                        {
                            HighestDamage = DamageValue;
                            StrongestItem = i;
                        }
                    }
                }
                if (HighestMeleeDamage > 0 && HorizontalDistance < 60 * companion.Scale + companion.width * 0.5f)
                {
                    companion.selectedItem = StrongestMelee;
                }
                else if (HighestMagicDamage > 0)
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
            }
            bool TargetInAim = companion.AimAtTarget(Target.position + Target.velocity, Target.width, Target.height);
            bool Left = false, Right = false, Attack = false, Jump = false;
            if(companion.HeldItem.type == 0) //Run for your lives!
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
            }
            else
            {
                companion.WalkMode = false;
                if(companion.HeldItem.DamageType.CountsAsClass(DamageClass.Melee))
                {
                    //Close Ranged Combat
                    float ItemSize = companion.GetAdjustedItemScale(companion.HeldItem);
                    float AttackRange = (TargetWidth - companion.width) * 0.5f + companion.HeldItem.width * ItemSize * 1.2f + Math.Abs(companion.velocity.X);
                    if(HorizontalDistance < AttackRange)
                    {
                        Attack = true;
                        bool TooClose = false;
                        if(HorizontalDistance < AttackRange * 0.9f)
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
                                Animation anim = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames);
                                if((TargetPosition.Y - TargetHeight) - companion.GetAnimationPosition(AnimationPositions.HandPosition, (short)(anim.GetFrameCount - 1)).Y >= companion.itemHeight * 0.7f)
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
                    if(HorizontalDistance < 100 + (TargetWidth + companion.width) * 0.5f)
                    {
                        if(FeetPosition.X < TargetPosition.X)
                            Left = true;
                        else
                            Right = true;
                    }
                    else if(companion.CanHit(Target))
                    {
                        if(HorizontalDistance >= 250 + (TargetWidth + companion.width) * 0.5f)
                        {
                            if(FeetPosition.X < TargetPosition.X)
                                Right = true;
                            else
                                Left = true;
                        }
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
                    companion.ControlAction = true;
            }
        }
    }
}