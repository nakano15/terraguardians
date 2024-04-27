using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class CombatBehavior : BehaviorBase
    {
        internal static bool UsingNewCombatBehavior = true;
        public bool EngagedInCombat = false;
        public ushort TargetMemoryTime = 0;
        const ushort MaxTargetMemory = 7 * 60;
        float AttackWidth = 0;
        byte StrongestMelee = 0, 
            StrongestRanged = 0, 
            StrongestMagic = 0, 
            StrongestHealing = 0,
            StrongestWhip = 0;
        byte StrongestSummon = 0;
        int[] HotbarItemIDs = new int[10];
        WeaponProfile[] CurrentProfiles = new WeaponProfile[10];

        public CombatBehavior()
        {
            for (int i = 0; i < 10; i++)
            {
                CurrentProfiles[i] = null;
            }
        }

        public override void Update(Companion companion)
        {
            UpdateWeaponProfiles(companion);
            if (UsingNewCombatBehavior)
                NewUpdateCombat(companion);
            else
                UpdateCombat(companion);
        }

        public void NewUpdateCombat(Companion companion)
        {
            if (companion.KnockoutStates > 0 || Companion.Is2PCompanion || (companion.IsBeingControlledBySomeone && !companion.CompanionHasControl)) return;
            if (!companion.controlUseItem && StrongestSummon < 255 && companion.numMinions < companion.maxMinions)
            {
                companion.selectedItem = StrongestSummon;
                companion.controlUseItem = true;
                return;
            }
            Entity Target = companion.Target;
            if (Target != null)
            {
                if (!EngagedInCombat)
                {
                    if (Target is NPC && Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[(Target as NPC).type])
                    {
                        EngagedInCombat = true;
                        TargetMemoryTime = MaxTargetMemory;
                    }
                    else
                    {
                        Vector2 TargetCenter = Target.Center, CompanionCenter = companion.Center;
                        if (MathF.Abs(TargetCenter.X - CompanionCenter.X) < 400 + (companion.width + Target.width) * .5f && 
                            MathF.Abs(TargetCenter.Y - CompanionCenter.Y) < 400 + (companion.height + Target.height) * .5f)
                        {
                            EngagedInCombat = true;
                            TargetMemoryTime = MaxTargetMemory;
                        }
                    }
                }
                else
                {
                    if (Target is not NPC || !Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[(Target as NPC).type])
                    {
                        Vector2 TargetCenter = Target.Center, CompanionCenter = companion.Center;
                        if (MathF.Abs(TargetCenter.X - CompanionCenter.X) < 700 + (companion.width + Target.width) * .5f && 
                            MathF.Abs(TargetCenter.Y - CompanionCenter.Y) < 600 + (companion.height + Target.height) * .5f && 
                            companion.CanHit(Target))
                        {
                            EngagedInCombat = true;
                            TargetMemoryTime = MaxTargetMemory;
                        }
                        else
                        {
                            TargetMemoryTime--;
                            if (TargetMemoryTime <= 0)
                            {
                                EngagedInCombat = false;
                                companion.ChangeTarget(null);
                            }
                        }
                    }
                }
                if (EngagedInCombat)
                    OnEngageInCombat(companion);
            }
        }

        void OnEngageInCombat(Companion companion)
        {
            Entity Target = companion.Target;
            Companion.Behaviour_AttackingSomething = true;
            byte StrongestWeapon = 0;
            if (companion.itemAnimation == 0)
            {
                StrongestMelee = 255;
                StrongestRanged = 255;
                StrongestMagic = 255;
                StrongestHealing = 255;
                float HMeleeDamage = 0,
                    HRangedDamage = 0,
                    HMagicDamage = 0,
                    HHealingDamage = 0;
                float HighestDamage = 0;
                AttackWidth = -1;
                for (byte i = 0; i < 10; i++)
                {
                    Item item = companion.inventory[i];
                    WeaponProfile profile = CurrentProfiles[i];
                    if (item.type > 0 && item.ammo == Terraria.ID.AmmoID.None && item.damage > 0)
                    {
                        if (item.useAmmo > 0 && !companion.HasAmmo(item) || companion.statMana < companion.GetManaCost(item)) continue;
                        float Damage = companion.GetWeaponDamage(item) * (item.useTime * (1f / 60));
                        if (Damage > HighestDamage)
                        {
                            StrongestWeapon = i;
                            HighestDamage = Damage;
                        }
                        if (item.DamageType.CountsAsClass(DamageClass.Melee) ||
                            item.DamageType.CountsAsClass(DamageClass.MeleeNoSpeed) || 
                            item.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed) || 
                            CheckDamageClass(item, ModCompatibility.CalamityModCompatibility.TrueMeleeDamage))
                        {
                            if (Damage > HMeleeDamage)
                            {
                                StrongestMelee = i;
                                HMeleeDamage = Damage;
                                AttackWidth = profile != null && profile.AttackRange > -1 ? profile.AttackRange : companion.GetAdjustedItemScale(item) * item.height;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Ranged) || 
                            CheckDamageClass(item, ModCompatibility.ThoriumModCompatibility.BardDamage) || 
                            CheckDamageClass(item, ModCompatibility.CalamityModCompatibility.RogueDamage))
                        {
                            if (Damage > HRangedDamage)
                            {
                                StrongestRanged = i;
                                HRangedDamage = Damage;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Magic))
                        {
                            if (Damage > HMagicDamage)
                            {
                                StrongestMagic = i;
                                HMagicDamage = Damage;
                            }
                        }
                        else if(CheckDamageClass(item, ModCompatibility.ThoriumModCompatibility.HealerDamage))
                        {
                            if (Damage > HHealingDamage)
                            {
                                StrongestHealing = i;
                                HHealingDamage = Damage;
                            }
                        }
                    }
                }
            }
            companion.WalkMode = false;
            Vector2 CompanionCenter = companion.Center,
                TargetCenter = Target.Center;
            CombatTactics tactic = companion.CombatTactic;
            Vector2 DistanceAbs = TargetCenter - CompanionCenter;
            DistanceAbs.X = MathF.Abs(DistanceAbs.X) - (companion.SpriteWidth + Target.width) * .5f;
            DistanceAbs.Y = MathF.Abs(DistanceAbs.Y) - (companion.SpriteHeight + Target.height) * .5f;
            Vector2 TargetPosition = Target.position;
            int TargetWidth = Target.width, TargetHeight = Target.height;
            float Distance = DistanceAbs.Length();
            if (companion.itemAnimation == 0)
            {
                if (StrongestMelee < 255 && DistanceAbs.X < AttackWidth && DistanceAbs.Y < AttackWidth)
                {
                    companion.selectedItem = StrongestMelee;
                }
                else
                {
                    if (StrongestMagic < 255)
                        companion.selectedItem = StrongestMagic;
                    else if (StrongestRanged < 255)
                        companion.selectedItem = StrongestRanged;
                    else if (StrongestWeapon < 255)
                        companion.selectedItem = StrongestWeapon;
                }
            }
            Item HeldItem = companion.HeldItem;
            BehaviorFlags Flags = new BehaviorFlags();
            Flags.Left = companion.MoveLeft;
            Flags.Right = companion.MoveRight;
            bool ForceFollowOwner = false;
            if (companion.Owner != null)
            {
                float OwnerCenterX = companion.Owner.Center.X;
                float OwnerBottomY = companion.Owner.Bottom.Y;
                float DistanceX = MathF.Abs(OwnerCenterX - companion.Center.X);
                float DistanceY = MathF.Abs(OwnerBottomY - companion.Bottom.Y);
                ForceFollowOwner = DistanceX >= 600 || DistanceY >= 500;
                if (ForceFollowOwner)
                {
                    Flags.SetMoveLeft (OwnerCenterX < companion.Owner.Center.X);
                }
            }
            if (HeldItem.type == 0 || HeldItem.damage == 0 || companion.selectedItem >= 10)
            {
                if (!ForceFollowOwner && DistanceAbs.X < 15)
                {
                    Flags.SetMoveLeft (CompanionCenter.X < TargetCenter.X);
                }
            }
            else
            {
                WeaponProfile profile = CurrentProfiles[companion.selectedItem];
                float AttackRange;
                if(profile != null && profile.AttackRange > -1)
                {
                    AttackRange = profile.AttackRange;
                }
                else
                {
                    if(HeldItem.DamageType.CountsAsClass(DamageClass.Melee) || CheckDamageClass(HeldItem, ModCompatibility.CalamityModCompatibility.TrueMeleeDamage))
                    {
                        AttackRange = companion.GetAdjustedItemScale(HeldItem) * HeldItem.height;
                    }
                    else if (HeldItem.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed))
                    {
                        AttackRange = 140;
                    }
                    else
                    {
                        AttackRange = 500;
                    }
                }
                Vector2 TargetAimPosition = TargetCenter;
                switch(tactic)
                {
                    case CombatTactics.CloseRange:
                        if (!ForceFollowOwner)
                        {
                            if (DistanceAbs.X > AttackWidth * .9f)
                            {
                                Flags.SetMoveLeft(TargetCenter.X < CompanionCenter.X);
                            }
                            else if(DistanceAbs.X < AttackWidth * .3f)
                            {
                                Flags.SetMoveRight(TargetCenter.X < CompanionCenter.X);
                            }
                        }
                        if (DistanceAbs.Y > AttackWidth)
                        {
                            if (TargetCenter.Y < CompanionCenter.Y)
                            {
                                Flags.Jump = true;
                            }
                            else
                            {
                                Flags.Crouch = true;
                            }
                        }
                        Flags.Attack = DistanceAbs.X < AttackRange && DistanceAbs.Y < AttackRange && companion.CanHit(Target);
                        break;
                    case CombatTactics.StickClose:
                        {
                            Player Owner = companion.Owner;
                            if (Owner != null)
                            {
                                if (!ForceFollowOwner)
                                {
                                    float OwnerX = Owner.Center.X;
                                    if (MathF.Abs(OwnerX - CompanionCenter.X) > 6f)
                                    {
                                        Flags.SetMoveLeft(OwnerX < CompanionCenter.X);
                                    }
                                }
                            }
                            else if (!ForceFollowOwner)
                            {
                                if (DistanceAbs.X > AttackRange * .9f)
                                {
                                    Flags.SetMoveLeft(TargetCenter.X < CompanionCenter.X);
                                }
                                else if(DistanceAbs.X < AttackRange * .3f)
                                {
                                    Flags.SetMoveRight(TargetCenter.X < CompanionCenter.X);
                                }
                            }
                            Flags.Attack = DistanceAbs.X < AttackRange && DistanceAbs.Y < AttackRange && companion.CanHit(Target);
                        }
                        break;
                    default:
                        if (!ForceFollowOwner)
                        {
                            if (DistanceAbs.X > AttackRange * .9f)
                            {
                                Flags.SetMoveLeft(TargetCenter.X < CompanionCenter.X);
                            }
                            else if(DistanceAbs.X < AttackRange * .3f)
                            {
                                Flags.SetMoveRight(TargetCenter.X < CompanionCenter.X);
                            }
                        }
                        Flags.Attack = DistanceAbs.X < AttackRange && DistanceAbs.Y < AttackRange && companion.CanHit(Target);
                        break;
                }
            }
            //Mouse aim is not moving to target...
            bool MouseInAim = companion.AimAtTarget(TargetPosition, TargetWidth, TargetHeight);
            if (Flags.Attack && MouseInAim)
            {
                WeaponProfile profile = companion.selectedItem < 10 ? CurrentProfiles[companion.selectedItem] : null;
                if ((companion.itemAnimation <= 0 && companion.releaseUseItem) || 
                    (HeldItem.channel && (profile == null || !profile.IsFlail) && ((companion.channel && companion.heldProj > -1 && Main.projectile[companion.heldProj].active) || (!companion.channel && !companion.controlUseItem))) || 
                    (HeldItem.autoReuse && !HeldItem.channel))
                {
                    companion.ControlAction = true;
                    if (companion.itemAnimation <= 0 && 
                        companion.HeldItem.DamageType.CountsAsClass<MeleeDamageClass>() && 
                        !companion.HeldItem.noMelee && !companion.HeldItem.useTurn)
                    {
                        companion.direction = CompanionCenter.X < TargetCenter.X ? 1 : -1;
                        Flags.ClearMovement();
                    }
                }
            }
            //if (Flags.Left || Flags.Right)
            {
                companion.MoveLeft = Flags.Left;
                companion.MoveRight = Flags.Right;
            }
            if (Flags.Jump)
            {
                if (companion.CanDoJumping)
                {
                    companion.ControlJump = true;
                }
            }
            if(Flags.Crouch)
            {
                if (companion.Base.CanCrouch)
                {
                    companion.MoveDown = true;
                }
            }
        }

        int SummonItemUsed = -1;
        byte ReSummonDelay = 10;
        public bool AllowMovement = true;
        byte PathFindingDelay = 0;

        bool CheckDamageClass(Item item, DamageClass Class)
        {
            return Class != null && item.DamageType.CountsAsClass(Class);
        }

        public void UpdateWeaponProfiles(Companion companion)
        {
            byte LastStrongestSummon = StrongestSummon;
            StrongestSummon = 255;
            int StrongestSummonDamage = 0;
            for (byte i = 0; i < 10; i++)
            {
                if (companion.inventory[i].type != HotbarItemIDs[i])
                {
                    HotbarItemIDs[i] = companion.inventory[i].type;
                    CurrentProfiles[i] = MainMod.GetWeaponProfile(HotbarItemIDs[i]);
                }
                if (companion.inventory[i].DamageType.CountsAsClass(DamageClass.Summon) && 
                    !companion.inventory[i].DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed))
                {
                    if (companion.inventory[i].damage > StrongestSummonDamage)
                    {
                        StrongestSummonDamage = companion.inventory[i].damage;
                        StrongestSummon = i;
                    }
                }
            }
            if (LastStrongestSummon != StrongestSummon)
            {
                companion.DespawnMinions();
            }
        }

        public void UpdateCombat(Companion companion)
        {
            if (companion.KnockoutStates > 0 || Companion.Is2PCompanion || (companion.IsBeingControlledBySomeone && !companion.CompanionHasControl)) return;
            Entity Target = companion.Target;
            bool UsedSummon = CheckSummons(companion);
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
            float ScreenWidth = Main.screenWidth * .4f, ScreenHeight = Main.screenHeight * .4f;
            if (!EngagedInCombat)
            {
                if (HorizontalDistance < ScreenWidth * .9f + (TargetWidth + companion.width) * 0.5f && 
                    VerticalDistance < ScreenHeight * .9f + (TargetHeight + companion.height) * 0.5f)
                {
                    EngagedInCombat = true;
                    TargetMemoryTime = MaxTargetMemory;
                    if (companion.Path.State == PathFinder.PathingState.TracingPath)
                    {
                        companion.Path.CancelPathing();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (HorizontalDistance >= ScreenWidth + TargetWidth * 0.5f || 
                    VerticalDistance >= ScreenHeight + TargetHeight * 0.5f || 
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
            Animation anim = companion.Base.GetAnimation(companion.Base.CanCrouch && companion.Crouching ? AnimationTypes.CrouchingSwingFrames : AnimationTypes.ItemUseFrames);
            bool Danger = companion.statLife < companion.statLifeMax2 * 0.25f;
            if(!UsedSummon && !Companion.Behavior_UsingPotion && companion.itemAnimation <= 0)
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
                        if(item.DamageType.CountsAsClass(DamageClass.Melee) || CheckDamageClass(companion.HeldItem, ModCompatibility.CalamityModCompatibility.TrueMeleeDamage))
                        {
                            if (DamageValue > HighestMeleeDamage)
                            {
                                HighestMeleeDamage = DamageValue;
                                StrongestMelee = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Ranged) || CheckDamageClass(companion.HeldItem, ModCompatibility.CalamityModCompatibility.RogueDamage))
                        {
                            if (item.ammo == 0 && DamageValue > HighestRangedDamage && companion.HasAmmo(item))
                            {
                                HighestRangedDamage = DamageValue;
                                StrongestRanged = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Magic) || CheckDamageClass(item, ModCompatibility.ThoriumModCompatibility.BardDamage)|| CheckDamageClass(item, ModCompatibility.ThoriumModCompatibility.HealerDamage))
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
                    WeaponProfile profile = CurrentProfiles[StrongestMelee];
                    float ItemHeight = companion.GetAdjustedItemScale(companion.inventory[StrongestMelee]) * companion.inventory[StrongestItem].height;
                    float LowestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(1f)).Y + (profile != null ? profile.AttackRange : ItemHeight);
                    float HighestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.26f)).Y - (profile != null ? profile.AttackRange : (ItemHeight * 1.5f));
                    AttackWidth = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.55f), AlsoTakePosition: false, DiscountDirections: false).X + (profile != null ? profile.AttackRange : (ItemHeight * 0.6f));
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
                    MeleeEvadeDistance = 0; //50
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
            companion.WalkMode = false;
            bool Left = false, Right = false, Attack = false, Jump = false;
            if(tactic != CombatTactics.StickClose && (companion.HeldItem.type == 0 || companion.Data.AvoidCombat || Companion.Behavior_UsingPotion)) //Run for your lives!
            {
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
                WeaponProfile profile = CurrentProfiles[companion.selectedItem];
                Vector2 AimDestination = Target.position + Target.velocity;
                if(companion.HeldItem.shoot > 0 && companion.HeldItem.shootSpeed > 0)
                {
                    float ShootSpeed = 1f / companion.HeldItem.shootSpeed;
                    //Vector2 Direction = TargetPosition - companion.Center;
                    //AimDestination += Target.velocity * ShootSpeed; //Direction;
                    /*if(companion.HeldItem.shoot == Terraria.ID.ProjectileID.WoodenArrowFriendly)
                    {
                        AimDestination.Y -= Direction.Length() * (1f / 16);
                    }*/
                }
                bool TargetInAim = companion.AimAtTarget(AimDestination, Target.width, Target.height);
                bool IsWhip = companion.HeldItem.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed);
                if(companion.HeldItem.DamageType.CountsAsClass(DamageClass.Melee) || CheckDamageClass(companion.HeldItem, ModCompatibility.CalamityModCompatibility.TrueMeleeDamage) || IsWhip)
                {
                    //Close Ranged Combat
                    float ItemSize = companion.GetAdjustedItemScale(companion.HeldItem);
                    float AttackRange = (TargetWidth) * 0.5f + Math.Abs(companion.velocity.X) - 16f;
                    float LowestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(1f)).Y - 8f;
                    float HighestHeight = companion.GetAnimationPosition(AnimationPositions.HandPosition, anim.GetFrameFromPercentage(0.26f)).Y - 16;
                    if (IsWhip)
                    {
                        AttackRange += 160;
                        LowestHeight += 160;
                        HighestHeight -= 160;
                    }
                    else
                    {
                        AttackRange += AttackWidth;
                        if (profile != null)
                        {
                            LowestHeight += profile.AttackRange;
                            HighestHeight -= profile.AttackRange;
                        }
                        else
                        {
                            LowestHeight += ItemSize * companion.HeldItem.height;
                            HighestHeight -= ItemSize* companion.HeldItem.height * 1.5f;
                        }
                    }
                    if(TargetPosition.Y < HighestHeight && ((companion.velocity.Y == 0 && FeetPosition.X - companion.Base.JumpSpeed * companion.Base.JumpHeight < TargetPosition.Y + TargetHeight + companion.height) || companion.jump > 0 || companion.rocketTime > 0 || companion.wingTime > 0 || (companion.releaseJump && companion.AnyExtraJumpUsable())))
                    {
                        Jump = true;
                    }
                    if(HorizontalDistance < AttackRange)
                    {
                        if (TargetPosition.Y - TargetHeight < LowestHeight)
                        {
                            //companion.SaySomething ("In Aim?: " + TargetInAim + "  Can Hit? " + companion.CanHit(Target));
                            if (TargetInAim && companion.CanHit(Target))
                                Attack = true;
                        }
                        bool TooClose = false;
                        if(tactic != CombatTactics.StickClose && HorizontalDistance < 15)//companion.width * 0.5f + 10)
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
                                if(TargetPosition.Y - TargetHeight > LowestHeight)
                                {
                                    companion.Crouching = true;
                                }
                            }
                        }
                    }
                    else if (tactic != CombatTactics.StickClose)
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
                        companion.HeldItem.DamageType.CountsAsClass(DamageClass.Magic) || 
                        CheckDamageClass(companion.HeldItem, ModCompatibility.CalamityModCompatibility.RogueDamage) || 
                        CheckDamageClass(companion.HeldItem, ModCompatibility.ThoriumModCompatibility.BardDamage) || 
                        CheckDamageClass(companion.HeldItem, ModCompatibility.ThoriumModCompatibility.HealerDamage))
                {
                    if (tactic != CombatTactics.StickClose)
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
            if (companion.HasBuff(Terraria.ID.BuffID.Horrified) && Main.wofNPCIndex > -1)
            {
                NPC wof = Main.npc[Main.wofNPCIndex];
                if (Math.Abs(wof.Center.X + wof.velocity.X - companion.Center.X + companion.velocity.X) < 120f)
                {
                    if (wof.Center.X < companion.Center.X)
                    {
                        Right = true;
                        Left = false;
                    }
                    else
                    {
                        Right = false;
                        Left = true;
                    }
                }
            }
            if (AllowMovement)
            {
                if (tactic != CombatTactics.StickClose && Left != Right)
                {
                    if (!IsDangerousAhead(companion, (int)MathF.Min(MathF.Abs(companion.velocity.X * 1.6f) * (1f / 16), 3), Direction: Left ? -1 : 1))
                    {
                        if(Left) companion.controlLeft = true;
                        if(Right) companion.controlRight = true;
                    }
                    else
                    {
                        if (PathFindingDelay == 0)
                        {
                            companion.CreatePathingTo(TargetPosition, CancelOnFail: true);
                            PathFindingDelay = 12;
                        }
                    }
                }
                if(Jump && (companion.velocity.Y == 0 || companion.jump > 0 || (companion.AnyExtraJumpUsable() && companion.releaseJump)))
                    companion.ControlJump = true;
            }
            if (!UsedSummon && Attack)
            {
                if(companion.itemAnimation == 0)
                {
                    if(companion.DoTryAttacking())
                    {
                        companion.direction = companion.GetAimedPosition.X < companion.Center.X ? -1 : 1;
                        companion.MoveLeft = false;
                        companion.MoveRight = false;
                    }
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

        private bool CheckSummons(Companion c)
        {
            //const bool DisableSomeSummonUsage = true; //To disable finch staff while the drawing issue isn't fixed.
            if (ReSummonDelay > 0)
            {
                ReSummonDelay--;
                return false;
            }
            if (c.itemAnimation > 0) return false;
            int NewSummon = 0;
            int SummonPosition = -1;
            int HighestDamage = 0;
            for(int i = 0; i < 10; i++)
            {
                Item item = c.inventory[i];
                if (item.type > 0 && item.DamageType.CountsAsClass<SummonDamageClass>() && !item.DamageType.CountsAsClass<SummonMeleeSpeedDamageClass>())
                {
                    if (!item.sentry)
                    {
                        if (c.statMana >= c.GetManaCost(item) && item.damage > HighestDamage)
                        {
                            //if (DisableSomeSummonUsage && item.type == Terraria.ID.ItemID.BabyBirdStaff)
                            //    continue;
                            HighestDamage = item.damage;
                            SummonPosition = i;
                            NewSummon = item.type;
                        }
                    }
                }
            }
            if (SummonPosition > -1 && SummonItemUsed != NewSummon)
            {
                c.DespawnMinions();
                SummonItemUsed = NewSummon;
            }
            bool UsedWeapon = false;
            if (SummonPosition > -1)
            {
                if (c.numMinions < c.maxMinions)
                {
                    c.selectedItem = SummonPosition;
                    c.controlUseItem = true;
                    UsedWeapon = true;
                }
                else
                {
                    ReSummonDelay = 180;
                }
            }
            return UsedWeapon;
        }

        public virtual void OnTargetChange(Companion companion, Entity NewTarget)
        {
            TargetMemoryTime = MaxTargetMemory;
            EngagedInCombat = true;
        }

        public struct BehaviorFlags
        {
            public bool Left;
            public bool Right;
            public bool Attack;
            public bool Jump;
            public bool Crouch;

            public void SetMoveLeft(bool MoveLeft)
            {
                Left = MoveLeft;
                Right = !MoveLeft;
            }

            public void SetMoveRight(bool MoveRight)
            {
                Left = !MoveRight;
                Right = MoveRight;
            }

            public void ClearMovement()
            {
                Left = false;
                Right = false;
            }
        }
    }
}