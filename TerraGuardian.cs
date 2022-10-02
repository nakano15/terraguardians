using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class TerraGuardian : Companion
    {
        public Rectangle BodyFrame = new Rectangle();
        public Rectangle LeftArmFrame = new Rectangle();
        public Rectangle RightArmFrame = new Rectangle();
        private float BodyFrameTime = 0;
        private AnimationStates PreviousAnimationState = AnimationStates.Standing;
        private short[] HandFrames = new short[2];

        protected override void UpdateAnimations()
        {
            PlayerFrame();
            AnimationStates NewState = AnimationStates.Standing;
            if(swimTime > 0) NewState = AnimationStates.Swiming;
            else if (velocity.Y != 0) NewState = AnimationStates.InAir;
            else if (mount.Active) NewState = AnimationStates.RidingMount;
            else if (sliding) NewState = AnimationStates.WallSliding;
            else if (velocity.X != 0 && (slippy || slippy2 || windPushed) && !controlLeft && !controlRight) NewState = AnimationStates.IceSliding;
            else if (velocity.X != 0) NewState = AnimationStates.Moving;
            if(NewState != PreviousAnimationState)
                BodyFrameTime = 0;
            PreviousAnimationState = NewState;
            short BodyFrameID = 0;
            short[] ArmFramesID = new short[Base.GetHands];
            if (mount.Active)
            {
                Animation anim = Base.GetAnimation(AnimationTypes.SittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.ChairSittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.StandingFrame);
                BodyFrameID = anim.UpdateTimeAndGetFrame(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse, but...
            {
                if (swimTime > 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(2, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.JumpingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(carpetFrame >= 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if (velocity.X != 0)
                {
                    if((slippy || slippy2 || windPushed) && !controlLeft && !controlRight)
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                    else
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(System.Math.Abs(velocity.X) * 1.3f, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
            }
            if(BodyFrameID == -1) BodyFrameID = 0;
            for(int a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFramesID[a] = BodyFrameID;
            }
            bool CanVisuallyHoldItem = this.CanVisuallyHoldItem(HeldItem);
            bool HeldItemTypeIsnt4952 = HeldItem.type != 4952;
            //Item attack animations here
            if(sandStorm)
            {

            }
            else if (itemAnimation > 0 && HeldItem.useStyle != 10 && HeldItemTypeIsnt4952)
            {
                ArmFramesID[0] = GetItemUseArmFrame();
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            LeftArmFrame = GetAnimationFrame(ArmFramesID[0]);
            RightArmFrame = GetAnimationFrame(ArmFramesID[1]);
            HandFrames = ArmFramesID;
        }

        public short GetItemUseArmFrame()
        {
            short Frame = 0;
            if(HeldItem.useStyle == 1 || HeldItem.useStyle == 11 || HeldItem.type == 0)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 7)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.67f + 0.33f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 2)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 9 || HeldItem.useStyle == 8)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.StandingFrame);
                Frame = animation.GetFrame(0);
            }
            else if(HeldItem.useStyle == 6)
            {
                float AnimationPercentage = System.Math.Min(1, (1f - (float)itemAnimation / itemAnimationMax) * 6);
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.5f + 0.5f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 3 || HeldItem.useStyle == 12)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 4)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count * 0.5f));
            }
            else if(HeldItem.useStyle == 13)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 5)
            {
                float RotationValue = 1f - System.Math.Clamp(itemRotation * (1f / (float)System.Math.PI), 0, 1);
                //if(gravDir == -1)
                //    AnimationPercentage = 1f - AnimationPercentage;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(1 + RotationValue * (animation.GetFrameCount - 1)));
            }
            return Frame;
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if(direction < 0)
                Position.X = Base.SpriteWidth - Position.X;
            if(gravDir < 0)
                Position.Y = Base.SpriteHeight - Position.Y;
            Position *= Scale;
            Position.X += (width - Base.SpriteWidth * Scale) * 0.5f;
            Position.Y += height - Base.SpriteHeight * Scale;
            if(AlsoTakePosition)
                Position += position;
            return Position;
        }

        protected override void UpdateItemScript()
        {
            if (PlayerLoader.PreItemCheck(this))
            {
                ItemCheck_Inner();
            }
            PlayerLoader.PostItemCheck(this);
            if (itemAnimation == 0) lastVisualizedSelectedItem = HeldItem.Clone();
        }

        #region Item Use Scripts
        private void ItemCheck_Inner()
        {
            if(CCed)
            {
                channel = false;
                itemAnimation = itemAnimationMax = 0;
                return;
            }
            float HeightOffsetHitboxCenter = this.HeightOffsetHitboxCenter;
            Item item = HeldItem;
            if (IsPlayerCharacter && PlayerInput.ShouldFastUseItem)
            {
                controlUseItem = true;
            }
            Item lastItem = item; //itemAnimation > 0 ? lastVisualizedSelectedItem : item;
            Rectangle drawHitbox = Item.GetDrawHitbox(lastItem.type, this);
            if(itemAnimation > 0)
            {
                if (item.mana > 0)
                {
                    ItemCheck_ApplyManaRegenDelay(item);
                }
                if (Main.dedServ)
                {
                    itemHeight = item.height;
                    itemWidth = item.width;
                }
                else
                {
                    itemHeight = drawHitbox.Height;
                    itemWidth = drawHitbox.Width;
                }
                itemAnimation--;
            }
            if (itemTime > 0)
            {
                itemTime--;
                if (ItemTimeIsZero && IsPlayerCharacter)
                {
                    if (!JustDroppedAnItem)
                    {
                        int type = item.type;
                        switch (type)
                        {
                            case 65:
                            case 676:
                            case 723:
                            case 724:
                            case 989:
                            case 1226:
                            case 1227:
                                EmitMaxManaEffect();
                                break;
                        }
                    }
                    PlayerInput.TryEndingFastUse();
                }
            }
            ItemCheck_HandleMount();
            int damage = GetWeaponDamage(item);
            ItemCheck_HandleMPItemAnimation(item);
            ItemCheck_HackHoldStyles(item);
            if (itemAnimation < 0) itemAnimation = 0;
            if (itemTime < 0) itemTime = 0;
            if (itemAnimation == 0 && reuseDelay > 0)
                ApplyReuseDelay();
            if (IsPlayerCharacter && itemAnimation == 0 && TileObjectData.CustomPlace(item.createTile, item.placeStyle))
            {
                int HackCreateTile = item.createTile;
                int HackPlaceStyle = item.placeStyle;
                if (HackCreateTile == 20)
                {
                    Tile soil = Main.tile[tileRangeX, tileRangeY + 1];
                    if (soil.HasTile)
                    {
                        TileLoader.SaplingGrowthType(soil.TileFrameY, ref HackCreateTile, ref HackPlaceStyle);
                    }
                }
                TileObject.CanPlace(tileTargetX, tileTargetY, HackCreateTile, HackPlaceStyle, direction, out var _, true);
            }
            if (itemAnimation == 0 && altFunctionUse == 2) altFunctionUse = 0;
            bool AllowUsage = true;
            if (gravDir == -1 && GolfHelper.IsPlayerHoldingClub(this))
            {
                AllowUsage = false;
            }
            if(AllowUsage && controlUseItem && releaseUseItem && itemAnimation == 0 && item.useStyle != 0)
            {
                if (altFunctionUse == 1) altFunctionUse = 2;
                if (item.shoot == 0) itemRotation = 0;
                bool CanUse = ItemCheck_CheckCanUse(item);
                if (item.potion && CanUse) ApplyPotionDelay(item);
                if (item.mana > 0 && CanUse && (IsLocalCompanion || IsPlayerCharacter) && item.buffType != 0 && item.buffTime != 0) AddBuff(item.buffType, item.buffTime);
                if (item.shoot <= 0 || !ProjectileID.Sets.MinionTargettingFeature[item.shoot] || altFunctionUse != 2)
                    ItemCheck_ApplyPetBuffs(item);
                if ((IsLocalCompanion || IsPlayerCharacter) && gravDir == 1 && item.mountType != -1 && mount.CanMount(item.mountType, this))
                {
                    mount.SetMount(item.mountType, this);
                }
                if ((item.shoot <= 0 || !ProjectileID.Sets.MinionTargettingFeature[item.shoot]))
                {
                    FreeUpPetsAndMinions(item);
                }
                if(CanUse)
                 ItemCheck_StartActualUse(item);
            }
            if (!controlUseItem) channel = false;
            ItemLoader.HoldItem(item, this);
            if(itemAnimation > 0)
            {
                //ApplyUseStyle script.
                ItemCheck_TerraGuardiansApplyUseStyle(HeightOffsetHitboxCenter, lastItem, drawHitbox, 0);
                ItemLoader.UseStyle(lastItem, this, drawHitbox);
            }
            else
            {
                ItemCheck_ApplyHoldStyle(HeightOffsetHitboxCenter, lastItem, drawHitbox, 0);
                ItemLoader.HoldStyle(lastItem, this, drawHitbox);
                //ApplyHoldStyle script.
            }
            releaseUseItem = !controlUseItem;
            if (!JustDroppedAnItem)
            {
                //Effects
                bool CanShoot = true;
                int type = item.type;
                if(!ItemAnimationJustStarted){
                    switch(type)
                    {
                        case 65:
                        case 676:
                        case 723:
                        case 724:
                        case 757:
                        case 674:
                        case 675:
                        case 989:
                        case 1226:
                        case 1227:
                            CanShoot = false;
                            break;
                    }
                    if(type == 3852 && altFunctionUse == 2)
                        CanShoot = false;
                    if (item.useLimitPerAnimation.HasValue && ItemUsesThisAnimation >= item.useLimitPerAnimation.Value)
                        CanShoot = false;
                }
                else
                {
                    /*switch(type)
                    {
                        case 5097:
                            //Can't set batbat to heal
                            break;
                        case 5094:
                            //Can't set tentacle spikes to spawn
                            break;
                    }*/
                }
                ItemCheck_TurretAltFeatureUse(item, CanShoot);
                ItemCheck_MinionAltFeatureUse(item, CanShoot);
                if (item.shoot > 0 && itemAnimation > 0 && ItemTimeIsZero && CanShoot)
                {
                    ItemCheck_Shoot(item, damage);
                }
                if (IsPlayerCharacter || IsLocalCompanion)
                {
                    if (!channel)
                        toolTime = itemTime;
                    else
                    {
                        toolTime --;
                        if (toolTime < 0) toolTime = item.useTime;
                    }
                    PlaceThing();
                }
                if (((item.damage >= 0 && item.type > 0 && !item.noMelee) || item.type == 1450 || ItemID.Sets.CatchingTool[item.type] || item.type == 3542 || item.type == 3779) && itemAnimation > 0)
                {
                    ItemCheck_GetMeleeHitbox(item, drawHitbox, out bool CantHit, out Rectangle Hitbox);
                    if (!CantHit)
                    {
                        //ItemCheck_EmitUseVisuals()
                        //ItemCheck_CatchCritters
                        //ItemCheck_CutTiles
                        if ((IsLocalCompanion || IsPlayerCharacter) && item.damage > 0)
                        {
                            int MeleeDamage = damage;
                            float kb = GetWeaponKnockback(item, item.knockBack);
                            //CutTiles Ignore list.
                            ItemCheck_MeleeHit(item, Hitbox, MeleeDamage, kb);
                        }
                    }
                }
                if (ItemTimeIsZero && itemAnimation > 0)
                {
                    if (ItemLoader.UseItem(item, this) == true)
                    {
                        ApplyItemTime(item, 1f, false);
                    }
                    if (item.healLife > 0)
                    {
                        int Heal = GetHealLife(item);
                        statLife += Heal;
                        ApplyItemTime(item);
                        if (Heal > 0 && (IsLocalCompanion || IsPlayerCharacter))
                            HealEffect(Heal);
                    }
                    if (item.healMana > 0)
                    {
                        int Heal = GetHealMana(item);
                        statMana += Heal;
                        ApplyItemTime(item);
                        if (Heal > 0 && (IsLocalCompanion || IsPlayerCharacter))
                        {
                            AddBuff(94, manaSickTime);
                            ManaEffect(Heal);
                        }
                    }
                    if (item.buffType > 0)
                    {
                        if ((IsLocalCompanion || IsPlayerCharacter) && item.buffType != 90 && item.buffType != 27)
                        {
                            AddBuff(item.buffType, item.buffTime);
                        }
                        ApplyItemTime(item);
                    }
                    if (item.type == 678 && (IsLocalCompanion || IsPlayerCharacter))
                    {
                        ApplyItemTime(item);
                        if(Main.getGoodWorld)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int b = 0;
                                const int Time = 108000;
                                switch(Main.rand.Next(18)){
                                    default:
                                        b = 16;
                                        break;
                                    case 1:
                                        b = 111;
                                        break;
                                    case 2:
                                        b = 114;
                                        break;
                                    case 3:
                                        b = 8;
                                        break;
                                    case 4:
                                        b = 105;
                                        break;
                                    case 5:
                                        b = 17;
                                        break;
                                    case 6:
                                        b = 116;
                                        break;
                                    case 7:
                                        b = 5;
                                        break;
                                    case 8:
                                        b = 113;
                                        break;
                                    case 9:
                                        b = 7;
                                        break;
                                    case 10:
                                        b = 6;
                                        break;
                                    case 11:
                                        b = 104;
                                        break;
                                    case 12:
                                        b = 115;
                                        break;
                                    case 13:
                                        b = 2;
                                        break;
                                    case 14:
                                        b = 9;
                                        break;
                                    case 15:
                                        b = 3;
                                        break;
                                    case 16:
                                        b = 117;
                                        break;
                                    case 17:
                                        b = 1;
                                        break;
                                }
                                AddBuff(b, Time);
                            }
                        }
                        else
                        {
                            AddBuff(20, 216000);
                            AddBuff(22, 216000);
                            AddBuff(23, 216000);
                            AddBuff(24, 216000);
                            AddBuff(30, 216000);
                            AddBuff(31, 216000);
                            AddBuff(32, 216000);
                            AddBuff(33, 216000);
                            AddBuff(35, 216000);
                            AddBuff(36, 216000);
                            AddBuff(68, 216000);
                        }
                    }
                    if ((item.type == 50 || item.type == 3124 || item.type == 3199) && itemAnimation > 0)
                    {
                        if(Main.rand.Next(2) == 0)
                            Dust.NewDust(position, width, height, 15, 0,0, 150, Scale: 1.1f);
                        if (ItemTimeIsZero) ApplyItemTime(item);
                        else if (itemTime == (int)(itemTimeMax * 0.5f))
                        {
                            for (int i = 0; i < 70; i++)
                            {
                                Dust.NewDust(position, width, height, 15, velocity.X * 0.5f, velocity.Y * 0.5f, 150, Scale: 1.5f);
                            }
                            RemoveAllGrapplingHooks();
                            Spawn(PlayerSpawnContext.RecallFromItem);
                            for (int i = 0; i < 70; i++)
                            {
                                Dust.NewDust(position, width, height, 15, 0, 0, 150, Scale: 1.5f);
                            }
                        }
                    }
                    if (item.type == 4263 && itemAnimation > 0)
                    {
                        //Effect
                        if (ItemTimeIsZero) ApplyItemTime(item);
                        else if (itemTime == 2)
                        {
                            //Should do something else on multiplayer, it seems?
                            MagicConch();
                        }
                    }
                    if (item.type == 4819 && itemAnimation > 0)
                    {
                        //Effect
                        if (ItemTimeIsZero) ApplyItemTime(item);
                        else if (itemTime == 2)
                        {
                            //Should do something else on multiplayer, it seems?
                            DemonConch();
                        }
                    }
                    if (item.type == 4870 && itemAnimation > 0)
                    {
                        if (ItemTimeIsZero)
                        {
                            ApplyItemTime(item);
                            SoundEngine.PlaySound(SoundID.Item3, position);
                            for(byte i = 0; i < 10; i++)
                            {
                               Main.dust[Dust.NewDust(position, width, height, 15, velocity.X * 0.2f ,velocity.Y * 0.2f, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                            }
                        }
                        else if (itemTime == 20)
                        {
                            SoundEngine.PlaySound(HeldItem.UseSound, position);
                            for (int i = 0; i < 70; i++)
                            {
                                Main.dust[Dust.NewDust(position, width, height, 15, velocity.X * 0.5f, velocity.Y * 0.5f, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                            }
                            RemoveAllGrapplingHooks();
                            bool WasImmune = immune;
                            int LastImmuneTime = immuneTime;
                            Spawn(PlayerSpawnContext.RecallFromItem);
                            immune = WasImmune;
                            immuneTime = LastImmuneTime;
                            for (int i = 0; i < 70; i++)
                            {
                                Main.dust[Dust.NewDust(position, width, height, 15, 0, 0, 150, Color.Cyan, Scale: 1.2f)].velocity *= 0.5f;
                            }
                            if (ItemLoader.ConsumeItem(item, this) && item.stack > 0)
                                item.stack --;
                        }
                    }
                    //Recall Potion
                    //Teleportation Potion
                    //Gender Swap Potion
                    if(IsLocalCompanion || IsPlayerCharacter)
                    {
                        if ((itemTimeMax != 0 && itemTime == itemTimeMax) | (!item.IsAir && item.IsNotSameTypePrefixAndStack(lastVisualizedSelectedItem)))
                            lastVisualizedSelectedItem = item.Clone();
                    }else{
                        lastVisualizedSelectedItem = item.Clone();
                    }
                    //Tile wand and coin placement
                    if (itemAnimation == 0) JustDroppedAnItem = false;
                }
            }
        }

        private void ItemCheck_MeleeHit(Item item, Rectangle Hitbox, int Damage, float Knockback)
        {
            for(int i = 0; i < 255; i++) //I am Nakman
            {
                if (i < 200)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.immune[whoAmI] != 0 || attackCD != 0) continue;
                    bool? ModCanHit = CombinedHooks.CanPlayerHitNPCWithItem(this, item, npc);
                    if (ModCanHit == false) continue;
                    npc.position += npc.netOffset;
                    if(ModCanHit == true || (!npc.dontTakeDamage && CanNPCBeHitByPlayerOrPlayerProjectile(npc)))
                    {
                        if (ModCanHit == true || !npc.friendly || (npc.type == 22 && killGuide) || (npc.type == 54 && killClothier))
                        {
                            Rectangle NpcHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                            if(ModCanHit == true || (NpcHitbox.Intersects(Hitbox) && (npc.noTileCollide || CanHit(npc))))
                            {
                                int NewDamage = Damage;
                                bool Critical = Main.rand.Next(1, 101) <= GetWeaponCrit(item);
                                int Banner = Item.NPCtoBanner(Main.npc[i].BannerID());
                                if (Banner > 0 && HasNPCBannerBuff(Banner))
                                {
                                    NewDamage = !Main.expertMode ? (int)(NewDamage * ItemID.Sets.BannerStrength[Item.BannerToItem(Banner)].NormalDamageDealt) : (int)(NewDamage * ItemID.Sets.BannerStrength[Item.BannerToItem(Banner)].ExpertDamageDealt);
                                }
                                if (parryDamageBuff && item.DamageType.CountsAsClass(DamageClass.Melee))
                                {
                                    NewDamage *= 5;
                                    parryDamageBuff = false;
                                    ClearBuff(198);
                                }
                                if (item.type == 426 && npc.life >= npc.lifeMax * 0.9f)
                                    NewDamage *= 2;
                                if (item.type == 5096)
                                {
                                    byte BuffPower = 0;
                                    if (FindBuffIndex(207) != -1)
                                        BuffPower = 3;
                                    else if (FindBuffIndex(206) != -1)
                                        BuffPower = 2;
                                    else if (FindBuffIndex(26) != -1)
                                        BuffPower = 1;
                                    NewDamage = (int)(NewDamage * (1f + 0.05f * BuffPower));
                                }
                                //Item 671 effect
                                int FinalDamage = Main.DamageVar(NewDamage, luck);
                                ItemLoader.ModifyHitNPC(item, this, npc, ref FinalDamage, ref Knockback, ref Critical);
                                NPCLoader.ModifyHitByItem(npc, this, item, ref FinalDamage, ref Knockback, ref Critical);
                                PlayerLoader.ModifyHitNPC(this, item, npc, ref FinalDamage, ref Knockback, ref Critical);
                                StatusToNPC(item.type, i);
                                if (Main.npc[i].life > 5) OnHit(npc.Center.X, npc.Center.Y, npc);
                                if (GetWeaponArmorPenetration(item) > 0)
                                {
                                    FinalDamage += npc.checkArmorPenetration(GetWeaponArmorPenetration(item));
                                }
                                NPCKillAttempt attempt = new NPCKillAttempt(npc);
                                int ResultDamage = (int)npc.StrikeNPC(FinalDamage, Knockback, direction, Critical);
                                ItemLoader.OnHitNPC(item, this, npc, ResultDamage, Knockback, Critical);
                                NPCLoader.OnHitByItem(npc, this, item, ResultDamage, Knockback, Critical);
                                PlayerLoader.OnHitNPC(this, item, npc, ResultDamage, Knockback, Critical);
                                //TODO ApplyNPCOnHitEffects, Very important to port this in the future.
                                int MobBannerItemId = Item.NPCtoBanner(npc.BannerID());
                                if(MobBannerItemId >= 0) lastCreatureHit = MobBannerItemId;
                                if (Main.netMode != 0)
                                {
                                    NetMessage.SendData(28, -1, -1, null, i, FinalDamage, Knockback, direction, Critical ? 1 : 0);
                                }
                                if(accDreamCatcher) addDPS(FinalDamage);
                                npc.immune[whoAmI] = itemAnimation;
                                attackCD = System.Math.Max(1, (int)(itemAnimationMax * 0.33f));
                                if (attempt.DidNPCDie()) OnKillNPC(ref attempt, item);
                            }
                        }
                    }
                    else if (Main.npc[i].type == 63 || Main.npc[i].type == 64 || Main.npc[i].type == 103 || Main.npc[i].type == 242)
                    {
                        Rectangle NpcHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                        if (NpcHitbox.Intersects(Hitbox) && (npc.noTileCollide || CanHit(npc)))
                        {
                            Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage * 1.3f), -direction);
                            Main.npc[i].immune[whoAmI] = itemAnimation;
                            attackCD = (int)(itemAnimationMax * 0.33f);
                        }
                    }
                    npc.position -= npc.netOffset;
                }
                if (hostile)
                {
                    Player player = Main.player[i];
                    if(player == this || !player.active || !player.hostile || player.immune || player.dead || (team != 0 && team == player.team) || !Hitbox.Intersects(player.Hitbox) || !CanHit(player) || !ItemLoader.CanHitPvp(item, this, player) || !PlayerLoader.CanHitPvp(this, item, player))
                    {
                        bool Critical = Main.rand.Next(1, 101) <= 10;
                        int NewDamage = Main.DamageVar(Damage, luck);
                        ItemLoader.ModifyHitPvp(item, this, player, ref NewDamage, ref Critical);
                        PlayerLoader.ModifyHitPvp(this, item, player, ref NewDamage, ref Critical);
                        StatusToPlayerPvP(item.type, i);
                        OnHit(player.Center.X, player.Center.Y, player);
                        PlayerDeathReason deathReason = PlayerDeathReason.ByPlayer(whoAmI);
                        int FinalDamage = (int)player.Hurt(deathReason, NewDamage, direction, true, false, Critical);
                        if (item.type == 3211)
                        {
                            Vector2 ProjSpawnDirection = new Vector2(direction * 100 * Main.rand.Next(-25, 26), Main.rand.Next(-75, 76));
                            ProjSpawnDirection.Normalize();
                            ProjSpawnDirection *= Main.rand.Next(30, 41) * 0.1f;
                            Vector2 ProjSpawnPos = (ProjSpawnDirection + player.Center * 2) * (1f / 3f);
                            Projectile.NewProjectile(GetSource_ItemUse(item), ProjSpawnPos.X, ProjSpawnPos.Y, ProjSpawnDirection.X, ProjSpawnDirection.Y, 524, (int)(Damage * 0.7f), Knockback * 0.7f, whoAmI);
                        }
                        //BatBat leech health, if the method and variable somehow goes unprivate.
                        if(beetleOffense)
                        {
                            beetleCountdown += FinalDamage;
                            beetleCountdown = 0;
                        }
                        if (meleeEnchant == 7) //It's the confetti, right?
                        {

                        }
                        if (item.type == 1123) //The bees!
                        {

                        }
                        if (item.type == 3106)
                        {
                            stealth = 1f;
                        }
                        ItemLoader.OnHitPvp(item, this, player, FinalDamage, Critical);
                        PlayerLoader.OnHitPvp(this, item, player, FinalDamage, Critical);
                        //How to send player hurt of a companion?
                        if(Main.netMode != 0)
                        {
                            NetMessage.SendPlayerHurt(i, deathReason, NewDamage, direction, Critical, true, -1);
                        }
                        attackCD = (int)(itemAnimationMax * 0.33f);
                    }
                }
            }
        }

        private void ItemCheck_GetMeleeHitbox(Item item, Rectangle itemframe, out bool CantHit, out Rectangle Hitbox)
        {
            CantHit = false;
            Hitbox = new Rectangle((int)itemLocation.X, (int)itemLocation.Y, 32, 32);
            if (!Main.dedServ)
            {
                int w = itemframe.Width, h = itemframe.Height;
                switch(item.type)
                {
                    case 5094:
                    case 5095:
                        w -= 10;
                        h -= 10;
                        break;
                    case 5096:
                        w -= 12;
                        h -= 12;
                        break;
                    case 5097:
                        w -= 8;
                        h -= 8;
                        break;
                }
                Hitbox.Width = w;
                Hitbox.Height = h;
            }
            float ItemScale = GetAdjustedItemScale(item);
            Hitbox.Width = (int)(Hitbox.Width * ItemScale);
            Hitbox.Height = (int)(Hitbox.Height * ItemScale);
            if (direction == -1) Hitbox.X -= Hitbox.Width;
            if (gravDir== -1) Hitbox.Y -= Hitbox.Height;
            switch (item.useStyle)
            {
                case 1:
                    if (itemAnimation < itemAnimationMax * 0.333f)
                    {
                        if (direction == -1)
                            Hitbox.X -= (int)(Hitbox.Width * 1.4f - Hitbox.Width);
                        Hitbox.Width = (int)(Hitbox.Width * 1.4f);
                        Hitbox.Y += (int)(Hitbox.Height * 0.5f * gravDir);
                    }
                    else if (itemAnimation >= itemAnimationMax * 0.666f)
                    {
                        if (direction == -1)
                            Hitbox.X -= (int)(Hitbox.Width * 1.2f);
                        Hitbox.Width *= 2;
                        Hitbox.Y -= (int)((Hitbox.Height * 1.4f - Hitbox.Height) * gravDir);
                        Hitbox.Height = (int)(Hitbox.Height * 1.4f);
                    }
                    break;
                case 3:
                    if (itemAnimation > itemAnimationMax * 0.666f)
                    {
                        CantHit = true;
                    }
                    else
                    {
                        if (direction == -1)
                            Hitbox.X -= (int)(Hitbox.Width * 1.4f - Hitbox.Width);
                        Hitbox.Width = (int)(Hitbox.Width * 1.4f);
                        Hitbox.Y += (int)(Hitbox.Height * 0.6f);
                        Hitbox.Height = (int)(Hitbox.Height * 0.6f);
                        if (item.type == 946 || item.type == 4707)
                        {
                            Hitbox.Height += 14;
                            Hitbox.Width -= 10;
                            if (direction == -1) Hitbox.X += 10;
                        }
                    }
                    break;
            }
            ItemLoader.UseItemHitbox(item, this, ref Hitbox, ref CantHit);
            //Item 1450 effect
            if (item.type == 3542) CantHit = true;
            if (item.type == 3779)
            {
                CantHit = true;
                //Its effect script
            }
        }

        private void ItemCheck_Shoot(Item item, int Damage)
        {
            if (!CombinedHooks.CanShoot(this, item)) return;
            int ProjToShoot = item.shoot;
            float ProjSpeed = item.shootSpeed;
            int ProjDamage = Damage;
            float Knockback = item.knockBack;
            if (item.noMelee && ProjToShoot != 699 && ProjToShoot != 707 && ProjToShoot > 879) //It was actually (uint)(ProjToShoot - 877) > 2u, so I thought this would be a good variant.
            {
                ProjSpeed /= 1f / GetTotalAttackSpeed(DamageClass.Melee);
            }
            if (item.CountsAsClass(DamageClass.Throwing) && ProjSpeed < 16)
            {
                ProjSpeed *= ThrownVelocity;
                if (ProjSpeed > 16) ProjSpeed = 16;
            }
            bool CanShoot = true;
            int UsedAmmoItemId = 0;
            if (item.useAmmo > 0)
            {
                CanShoot = PickAmmo(item, out ProjToShoot, out ProjSpeed, out ProjDamage, out Knockback, out UsedAmmoItemId, ItemID.Sets.gunProj[item.type]);
            }
            if (ItemID.Sets.gunProj[item.type])
            {
                Knockback = item.knockBack;
                ProjDamage = Damage;
                ProjSpeed = item.shootSpeed;
            }
            if (item.IsACoin) CanShoot = false;
            if (ProjToShoot == 14)
            {
                if(item.type == 1254 || item.type == 1255 || item.type == 1265)
                    ProjToShoot = 242;
            }
            if (item.type == 3542)
            {
                if (Main.rand.Next(100) < 20)
                {
                    ProjToShoot++;
                    Damage *= 3;
                }
                else
                {
                    ProjSpeed --;
                }
            }
            if (item.type == 1928)
                Damage = (int)(Damage * 0.75f);
            if (ProjToShoot == 73)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == i)
                    {
                        if (Main.projectile[i].type == 73) ProjToShoot = 74;
                        if (ProjToShoot == 74 && Main.projectile[i].type == 74) CanShoot = false;
                    }
                }
            }
            if (CanShoot)
            {
                if (!IsLocalCompanion && !IsPlayerCharacter)
                {
                    ApplyItemTime(item);
                    return;
                }
                Knockback = GetWeaponKnockback(item, Knockback);
                IEntitySource projSource = GetSource_ItemUse_WithPotentialAmmo(item, UsedAmmoItemId);
                if(ProjToShoot == 228) Knockback = 0;
                if (ProjToShoot == 1 && item.type == 120) ProjToShoot = 2;
                switch(item.type)
                {
                    case 682:
                        ProjToShoot = 117;
                        break;
                    case 725:
                        ProjToShoot = 120;
                        break;
                    case 2796:
                        ProjToShoot = 442;
                        break;
                    case 2223:
                        ProjToShoot = 357;
                        break;
                    case 5117:
                        ProjToShoot = 968;
                        break;
                }
                ApplyItemTime(item);
                Vector2 AimDestination = GetAimedPosition;
                direction = Center.X < AimDestination.X ? 1 : -1;
                if (item.useStyle == 5)
                {
                    Vector2 AimDirection = AimDestination - MountedCenter;
                    AimDirection.Normalize();
                    itemRotation = (float)System.Math.Atan2(AimDirection.X, AimDirection.Y) * direction;
                }
                Vector2 FiringPosition = GetAnimationPosition(AnimationPositions.HandPosition, GetItemUseArmFrame(), 0);
                Vector2 FireDirection = AimDestination - FiringPosition;
                FireDirection.Normalize();
                //Test Script
                //TODO Need to make the rest of the method
                Projectile.NewProjectile(projSource, FiringPosition, FireDirection * ProjSpeed, ProjToShoot, ProjDamage, Knockback, 255);
                switch (item.useStyle)
                {
                    case 5:
                        {
                            if(item.type == 3029)
                            {

                            }
                            else if (item.type == 4381)
                            {
                                
                            }
                            else if (item.type == 3779)
                            {
                                itemRotation = 0;
                            }
                            else
                            {
                                itemRotation = (float)System.Math.Atan2(FireDirection.X, FireDirection.Y) * direction;
                            }
                        }
                        break;
                }
            }
            else if ((item.useStyle == 5 || item.useStyle == 13) && (IsLocalCompanion || IsPlayerCharacter))
            {
                itemRotation = 0;
            }
        }

        private void ItemCheck_MinionAltFeatureUse(Item item, bool Shoot)
        {
            if (item.shoot > 0 && ProjectileID.Sets.MinionTargettingFeature[item.shoot] && altFunctionUse == 2 && Shoot && ItemTimeIsZero)
            {
                ApplyItemTime(item);
                if(IsLocalCompanion || IsPlayerCharacter)
                    MinionNPCTargetAim(false);
            }
        }

        private void ItemCheck_TurretAltFeatureUse(Item item, bool Shoot)
        {
            if (item.shoot <= 0 || !ProjectileID.Sets.TurretFeature[item.shoot] || altFunctionUse != 2 || !Shoot || !ItemTimeIsZero)
                return;
            ApplyItemTime(item);
            if (!IsLocalCompanion && !IsPlayerCharacter) return;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && ProjMod.IsThisCompanionProjectile(i, this) && ProjectileID.Sets.TurretFeature[proj.type])
                    proj.Kill();
            }
        }

        private void ItemCheck_ApplyHoldStyle(float MountOffset, Item item, Rectangle HeldItemFrame, byte Arm)
        { //TODO
            //Petting script. Why is this here?
            if (!CanVisuallyHoldItem(item)) return;
            switch (item.holdStyle)
            {
                case 1:
                    if(pulley) break;
                    
                    break;
            }
        }

        private void ItemCheck_TerraGuardiansApplyUseStyle(float MountOffset, Item item, Rectangle HeldItemFrame, byte Hand)
        {
            if(Main.dedServ) return;
            switch(item.useStyle)
            {
                case 1:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(AttackPercentage);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        /*if(item.type > -1 && Item.claw[item.type])
                        {
                            if(AttackPercentage >= 0.666f)
                            {
                                itemLocation.X += (HeldItemFrame.Width * 0.5f - 10) * direction;
                                itemLocation.Y += (HeldItemFrame.Width * 0.5f - 10) * direction;
                            }
                        }*/
                        itemRotation = (AttackPercentage - 0.5f) * -direction * 3.5f - direction * 0.3f;
                    }
                    break;
                case 2: //For now, leave at this.
                case 6:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        itemRotation = (1f - (AttackPercentage * 6)) * direction * 2 - 1.4f * direction;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(System.Math.Clamp((1f - AttackPercentage) * 2, 0, 0.6f));
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                    }
                    break;
                case 3:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(0.7f);
                        if(itemAnimation > itemAnimationMax * 0.666f)
                        {
                            itemLocation.X = itemLocation.Y = -1000f;
                            itemRotation = -1.3f * direction;
                        }
                        else
                        {
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                            float MovementDirection = (float)itemAnimation / itemAnimationMax * HeldItemFrame.Width * direction * GetAdjustedItemScale(item) * 1.2f - 10f * direction;
                            if (MovementDirection * direction > 4f) MovementDirection = 8 * direction;
                            itemLocation.X -= MovementDirection;
                            itemRotation = 0.8f * direction;
                            if(item.type == 946 || item.type == 4707) itemLocation.X -= 6 * direction;
                        }
                    }
                    break;
                case 4:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(0.3f);
                        float OffsetX = 0, OffsetY = 0;
                        if (item.type == 3601) OffsetX = 10;
                        else if (item.type == 5114)
                        {
                            OffsetX = 10;
                            OffsetY = -1;
                        }
                        else if (item.type == 5120) OffsetX = 10;
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        itemLocation.X += OffsetX;
                        itemLocation.Y += OffsetY;
                    }
                    break;
                case 5:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        if(item.type == 3779)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        }
                        else if (item.type == 4262)
                        {
                            itemRotation = 0;
                            short Frame = anim.GetFrameFromPercentage(0.6f);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                            if (Main.rand.Next(20) == 0)
                            {
                                //Snake flute effect
                            }
                        }
                        else if (Item.staff[item.type])
                        {
                            //float ScaleFactor = 6f;
                            //if (item.type == 3476) ScaleFactor = 14f;
                            float Percentage = (itemRotation * direction + 1) * 0.5f;
                            short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        }
                        else
                        {
                            float Percentage = System.Math.Clamp(itemRotation * (float)(1f / MathHelper.Pi), 0, 1); //Still need to fix positioning issues
                            short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                            Main.NewText("Frame ID: " + Frame + "  Percent: " + Percentage);
                            itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                        }
                        //Item 5065 effect script.
                    }
                    break;
                case 7: //Unused, it seems
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        itemRotation = AttackPercentage * direction * 2 + -1.4f * direction;
                    }
                    break;
                case 8: //Golf. Todo
                    {

                    }
                    break;
                case 9:
                    {
                        float AttackPercentage = (float)itemAnimation / itemAnimationMax;
                        float t = Utils.GetLerpValue(0, 0.7f, 1f - AttackPercentage, true);
                        itemRotation = t * -direction * 2 + 0.7f + direction;
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(System.Math.Clamp((1f - AttackPercentage) * 2, 0f, 0.6f));
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame);
                    }
                    break;
                case 11:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        short Frame = anim.GetFrameFromPercentage(1f);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame);
                    }
                    break;
                case 12: //Guitar TODO
                    {

                    }
                    break;
                case 13:
                    {
                        Animation anim = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                        float Percentage = (itemRotation * direction + 1) * 0.5f;
                        short Frame = (short)(1 + (anim.GetFrameCount - 1) * Percentage);
                        itemLocation = GetAnimationPosition(AnimationPositions.HandPosition, Frame, Hand);
                    }
                    break;
                case 14: //Lamp
                    {

                    }
                    break;
            }
            if (gravDir == -1) itemRotation = -itemRotation;
        }

        private void ItemCheck_StartActualUse(Item item)
        {
            bool IsGravediggerShovel = item.type == 4711;
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0 || IsGravediggerShovel)
                toolTime = 1;
            if (grappling[0] > -1)
            {
                pulley = false;
                pulleyDir = 1;
                if(controlRight) direction = 1;
                else if(controlLeft) direction = -1;
            }
            channel = item.channel;
            attackCD = 0;
            ApplyItemAnimation(item);
            bool SkipInitialSound = ItemID.Sets.SkipsInitialUseSound[item.type];
            if (item.UseSound.HasValue && !SkipInitialSound)
                SoundEngine.PlaySound(item.UseSound, Center);
        }

        private void FreeUpPetsAndMinions(Item item)
        {
            if (ProjectileID.Sets.MinionSacrificable[item.shoot])
            {
                List<int> list = new List<int>();
                float MinionSlotsSum = 0;
                for(int i = 0; i < 1000; i++)
                {
                    if(!Main.projectile[i].active || !ProjMod.IsThisCompanionProjectile(i, this) || !Main.projectile[i].minion)
                        continue;
                    int n;
                    for(n = 0; n < list.Count; n++)
                    {
                        if(Main.projectile[list[n]].minionSlots > Main.projectile[i].minionSlots)
                        {
                            list.Insert(n, i);
                            break;
                        }
                    }
                    if(n == list.Count) list.Add(i);
                    MinionSlotsSum += Main.projectile[i].minionSlots;
                }
                float SlotsReq = ItemID.Sets.StaffMinionSlotsRequired[item.type];
                float AltMinionSlots = 0;
                int SummonID = 388;
                int VariantPosition = -1;
                for(int i = 0; i < list.Count; i++)
                {
                    int type = Main.projectile[list[i]].type;
                    if(type == 626) list.RemoveAt(i--);
                    if(type == 627)
                    {
                        if(Main.projectile[(int)Main.projectile[list[i]].localAI[1]].type == 628)
                            VariantPosition = i;
                        list.RemoveAt(i--);
                    }
                }
                if(VariantPosition != -1)
                {
                    list.Add(VariantPosition);
                    list.Add(Projectile.GetByUUID(Main.projectile[VariantPosition].owner, Main.projectile[VariantPosition].ai[0]));
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if(!(MinionSlotsSum - AltMinionSlots > maxMinions - SlotsReq))
                        break;
                    int type = Main.projectile[list[i]].type;
                    if(type == SummonID || type == 625 || type == 628 || type == 623) continue;
                    if(type == 388 && SummonID == 387) SummonID = 388;
                    if(type == 387 && SummonID == 388) SummonID = 388;
                    AltMinionSlots += Main.projectile[list[i]].minionSlots;
                    if(type == 626 || type == 627)
                    {
                        Projectile proj = Main.projectile[list[i]];
                        int byUUID = Projectile.GetByUUID(proj.owner, proj.ai[0]);
                        if(Main.projectile.IndexInRange(byUUID))
                        {
                            Projectile proj2 = Main.projectile[byUUID];
                            if(proj2.type != 625)
                                proj2.localAI[1] = proj.localAI[1];
                            proj2 = Main.projectile[(int)proj.localAI[1]];
                            proj2.ai[0] = proj.ai[0];
                            proj2.ai[1] = 1;
                            proj2.netUpdate = true;
                        }
                    }
                    Main.projectile[list[i]].Kill();
                }
                list.Clear();
                if(IsPlayerCharacter && MinionSlotsSum + SlotsReq >= 9)
                {
                    //9 or more minions achievement
                }
                return;
            }
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this))
                {
                    if(Main.projectile[i].type == item.shoot)
                        Main.projectile[i].Kill();
                    if(item.shoot == 72 && (Main.projectile[i].type == 86 || Main.projectile[i].type == 87))
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
        }

        private void ItemCheck_ApplyPetBuffs(Item item)
        {
            if(!(IsLocalCompanion || IsPlayerCharacter))
                return;
            bool GetPet = false;
            switch(item.type)
            {
                case 603:
                    if(Main.runningCollectorsEdition)
                        GetPet = true;
                    break;
                case 669:
                case 115:
                case 3060:
                case 3628:
                case 3062:
                case 3577:
                case 753:
                case 994:
                case 1169:
                case 1170:
                case 1171:
                case 1172:
                case 1180:
                case 1181:
                case 1182:
                case 1183:
                case 1242:
                case 1157:
                case 1309:
                case 1311:
                case 1837:
                case 1312:
                case 1798:
                case 1799:
                case 1802:
                case 1810:
                case 1927:
                case 1959:
                case 2364:
                case 2365:
                case 3043:
                case 2420:
                case 2535:
                case 2551:
                case 2584:
                case 2587:
                case 2621:
                case 2749:
                case 3249:
                case 3474:
                case 3531:
                case 4269:
                case 4273:
                case 4281:
                case 4607:
                case 4758:
                case 5005:
                case 5069:
                case 5114:
                case 3855:
                case 3856:
                case 3857:
                case 4365:
                case 4366:
                case 4425:
                case 4550:
                case 4551:
                case 4603:
                case 4604:
                case 4605:
                case 4701:
                case 4735:
                case 4736:
                case 4737:
                case 4777:
                case 4960:
                case 5088:
                case 5089:
                case 5090:
                case 5091:
                case 5098:
                    GetPet = true;
                    break;
                case 425:
                    {
                        int PickedPet = Main.rand.Next(3);
                        switch(PickedPet)
                        {
                            default:
                                PickedPet = 27;
                                break;
                            case 1:
                                PickedPet = 101;
                                break;
                            case 2:
                                PickedPet = 102;
                                break;
                        }
                        for(int i = 0; i < MaxBuffs; i++)
                        {
                            if (buffType[i] == 27 || buffType[i] == 101 || buffType[i] == 102)
                                DelBuff(i--);
                        }
                        AddBuff(PickedPet, 3600);
                    }
                    return;
            }
            if(item.type >= 4797 && item.type <= 4817)
                GetPet = true;
            if (GetPet) AddBuff(item.buffType, 3600);
        }

        private void ApplyPotionDelay(Item item)
        {
            if (item.type == 227) AddBuff(21, potionDelay = restorationDelayTime);
            else if (item.type == 5) AddBuff(21, potionDelay = mushroomDelayTime);
            else AddBuff(21, potionDelay = potionDelayTime);
        }

        private bool ItemCheck_CheckCanUse(Item item)
        {
            if (item.IsAir || !CombinedHooks.CanUseItem(this, item)) return false;
            bool Can = true;
            int MouseX = (int)((GetAimedPosition.X) * DivisionBy16);
            int MouseY = gravDir == -1 ? 
                (int)((Main.screenHeight - GetAimedPosition.Y)) : 
                (int)((GetAimedPosition.Y) * DivisionBy16);
            if (item.type == 3335 && (extraAccessory || !Main.expertMode))
                Can = false;
            if (pulley && item.fishingPole > 0) Can = false;
            if (pulley && ItemID.Sets.IsAKite[item.type]) Can = false;
            if (item.type == 3611 && (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode.Red | WiresUI.Settings.MultiToolMode.Green | WiresUI.Settings.MultiToolMode.Blue | WiresUI.Settings.MultiToolMode.Yellow | WiresUI.Settings.MultiToolMode.Yellow)) == 0) Can = false;
            if ((item.type == 3611 || item.type == 3625) && wireOperationsCooldown > 0) Can = false;
            if (!CheckDD2CrystalPaymentLock(item)) Can = false;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && !downedDD2EventAnyDifficulty && !DD2Event.Ongoing) Can=false;
            int PushYUp;
            if (item.shoot > -1 && ProjectileID.Sets.IsADD2Turret[item.shoot] && (IsPlayerCharacter || IsLocalCompanion))
            {
                FindSentryRestingSpot(item.shoot, out int WorldX, out int WorldY, out PushYUp);
                if (DD2Event.Ongoing)
                {
                    if (WouldSpotOverlapWithSentry(WorldX, WorldY, item.shoot == 688 || item.shoot == 689 || item.shoot == 690))
                        Can = false;
                }
                if (Can)
                {
                    WorldX = (int)(WorldX * DivisionBy16);
                    WorldY = (int)(WorldY * DivisionBy16);
                    WorldY--;
                    if (item.shoot == 688 || item.shoot == 689 || item.shoot == 690)
                    {
                        if (Collision.SolidTiles(WorldX, WorldX, WorldY - 2, WorldY)) Can = false;
                    }
                    else if (WorldGen.SolidTile(WorldX,WorldY)) Can = false;
                }
            }
            if (wet && (item.shoot == 85 || item.shoot == 15 || item.shoot == 34)) Can = false;
            if (item.makeNPC > 0 || !NPC.CanReleaseNPCs(whoAmI)) Can = false;
            if((IsLocalCompanion || IsPlayerCharacter) && item.type == 603 && !Main.runningCollectorsEdition) Can = false;
            if (item.type == 1071 || item.type == 1072)
            {
                bool HasPaint = false;
                for(int i = 0; i < 58; i++)
                {
                    if (inventory[i].paint > 0)
                    {
                        HasPaint = true;
                        break;
                    }
                }
                if (!HasPaint) Can = false;
            }
            if (noItems) Can = false;
            if(item.tileWand > 0)
            {
                Can = false;
                for (int i = 0; i < 58; i++)
                {
                    if (item.tileWand == inventory[i].type && inventory[i].stack > 0)
                    {
                        Can = true;
                        break;
                    }
                }
            }
            if (item.shoot == 6 || item.shoot == 19 || item.shoot == 33 || item.shoot == 52 || item.shoot == 113 || item.shoot == 320 || item.shoot == 333 || item.shoot == 383 || item.shoot == 491 || item.shoot == 867 || item.shoot == 902 || item.shoot == 866)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                    {
                        Can = false;
                        break;
                    }
                }
            }
            if (item.shoot == 106 || item.shoot == 272)
            {
                int Discs = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                    {
                        Discs++;
                    }
                }
                if(Discs >= item.stack) Can = false;
            }
            if (item.shoot == 13 || item.shoot == 32 || (item.shoot >= 230 && item.shoot <= 235) || item.shoot == 315 || item.shoot == 331 || item.shoot == 372)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot && Main.projectile[i].ai[0] != 2f)
                    {
                        Can = false;
                        break;
                    }
                }
            }
            if (item.shoot == 332)
            {
                int Hooks = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot && Main.projectile[i].ai[0] != 2)
                    {
                        Hooks++;
                    }
                }
                if(Hooks >= 3) Can = false;
            }
            if (item.potion && potionDelay > 0) Can = false;
            if (item.mana > 0)
            {
                if(silence)
                    Can = false;
                else if(Can) Can = ItemCheck_PayMana(item, Can);
            }
            if ((item.type == 43 || item.type == 544 || item.type == 556 || item.type == 557) && Main.dayTime)
            {
                Can = false;
            }
            if (item.type == 70 && !ZoneCorrupt)
                Can = false;
            if (item.type == 1133 && !ZoneJungle)
                Can = false;
            if (item.type == 5120 && !ZoneSnow)
                Can = false;
            if ((item.type == 1844 || item.type == 1958) && (Main.dayTime || Main.pumpkinMoon || Main.snowMoon || DD2Event.Ongoing))
                Can = false;
            if (item.type == 2767 && (!Main.dayTime || Main.eclipse || !Main.hardMode))
                Can = false;
            if (item.type == 4271 && (Main.dayTime || Main.bloodMoon))
                Can = false;
            if (item.type == 3601 && (!NPC.downedGolemBoss || !Main.hardMode || NPC.AnyDanger() || NPC.AnyoneNearCultists()))
                Can = false;
            if (!SummonItemCheck()) Can = false;
            if (item.shoot == 17 && Can && (IsPlayerCharacter || IsLocalCompanion) && !ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY])) Can = false;
            if (item.fishingPole > 0) Can = ItemCheck_CheckFishingBobbers(Can);
            if (ItemID.Sets.HasAProjectileThatHasAUsabilityCheck[item.type]) Can = ItemCheck_CheckUsabilityOfProjectiles(Can);
            if (item.shoot == 17 && Can && (IsLocalCompanion || IsPlayerCharacter))
            {
                if (ItemCheck_IsValidDirtRodTarget(Main.tile[MouseX, MouseY]))
                {
                    WorldGen.KillTile(MouseX, MouseY, false, false, true);
                    if (!Main.tile[MouseX, MouseY].HasTile)
                    {
                        if (Main.netMode == 1)
                            NetMessage.SendData(17, -1, -1, null, 4, MouseX, MouseY);
                    }
                    else
                        Can = false;
                }
                else
                {
                    Can = false;
                }
            }
            if (Can) Can = HasAmmo(item);
            return Can;
        }

        private bool ItemCheck_CheckUsabilityOfProjectiles(bool CanUse)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && ProjMod.IsThisCompanionProjectile(i, this))
                    proj.CheckUsability(this, ref CanUse);
            }
            return CanUse;
        }

        private bool ItemCheck_CheckFishingBobbers(bool CanUse)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active || ProjMod.IsThisCompanionProjectile(proj, this) || !proj.bobber)
                    continue;
                CanUse = false;
                if (!(IsLocalCompanion || IsPlayerCharacter) || proj.ai[0] != 0)
                    continue;
                proj.ai[0] = 1f;
                float VelocityY = -10f;
                if(proj.wet && proj.velocity.Y > VelocityY)
                    proj.velocity.Y = VelocityY;
                proj.netUpdate2 = true;
                if (proj.ai[1] < 0 && proj.localAI[1] != 0)
                {
                    ItemCheck_CheckFishingBobber_PickAndConsumeBait(proj, out bool PullBobber, out int BaitUsed);
                    if (PullBobber)
                    {
                        ItemCheck_CheckFishingBobber_PullBobber(proj, BaitUsed);
                    }
                }
            }
            return CanUse;
        }

        private void ItemCheck_CheckFishingBobber_PullBobber(Projectile bobber, int BaitTypeUsed)
        {
            if (BaitTypeUsed == 2673)
            {
                if (Main.netMode != 1) NPC.SpawnOnPlayer(whoAmI, 370);
                //else NetMessage.SendData(61, -1, -1, null, whoAmI, 370); //TODO - Need to figure out how to take companion position
                bobber.ai[0] = 2f;
            }
            else if (bobber.localAI[1] < 0)
            {
                Point pos = new Point((int)bobber.position.X, (int)bobber.position.Y);
                int NpcID = (int)(- bobber.localAI[1]);
                if(NpcID == 618) pos.Y += 64;
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(130, -1, -1, null, pos.X / 16, pos.Y / 16, NpcID);
                }
                else
                {
                    NPC.NewNPC(new EntitySource_FishedOut(this), pos.X, pos.Y, NpcID);
                    bobber.ai[0] = 2;
                }
            }
            else if (Main.rand.Next(7) == 0 && !accFishingLine)
            {
                bobber.ai[0] = 2f;
            }
            else
            {
                bobber.ai[1] = bobber.localAI[1];
            }
            bobber.netUpdate = true;
        }

        private void ItemCheck_CheckFishingBobber_PickAndConsumeBait(Projectile bobber, out bool PullTheBobber, out int BaitTypeUsed)
        {
            PullTheBobber = false;
            BaitTypeUsed = 0;
            int BaitPos = -1;
            for (int i = 54; i < 58; i++)
            {
                if (inventory[i].stack > 0 && inventory[i].bait > 0)
                {
                    BaitPos = i;
                    break;
                }
            }
            if(BaitPos == -1)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (inventory[i].stack > 0 && inventory[i].bait > 0)
                    {
                        BaitPos = i;
                        break;
                    }
                }
            }
            if (BaitPos == -1) return;
            Item bait = inventory[BaitPos];
            bool consume = false;
            float consumptionchance = 1f + bait.bait * (1f / 6);

            if(consumptionchance < 1) consumptionchance = 1;
            if(accTackleBox) consumptionchance += 1;
            if(Main.rand.NextFloat() * consumptionchance < 1) consume = true;
            if (bobber.localAI[1] == -1f) consume = true;
            if (bobber.localAI[1] > 0)
            {
                Item item = new Item();
                item.SetDefaults((int)bobber.localAI[1]);
                if (item.rare < 0) consume = false;
            }
            BaitTypeUsed = ((byte)itemAnimation);
            if (BaitTypeUsed == 2673) consume = true;
            if (CombinedHooks.CanConsumeBait(this, bait) ?? consume)
            {
                if (bait.type == 4361 || bait.type == 4362)
                    NPC.LadyBugKilled(Center, bait.type == 4362);
                bait.stack--;
                if (bait.stack <= 0)
                {
                    bait.SetDefaults();
                }
            }
            PullTheBobber = true;
        }

        private static bool ItemCheck_IsValidDirtRodTarget(Tile tile)
        {
            if (tile.HasTile)
            {
                int type = tile.TileType;
                if(type != 0 && type != 2 && type != 23 && type != 109 && type != 199 && type != 477)
                {
                    return type == 492;
                }
                return true;
            }
                return false;
        }

        private bool ItemCheck_PayMana(Item item, bool CanUse)
        {
            bool IsAltUse = altFunctionUse == 2;
            bool NoPay = false;
            int Cost = (int)(item.mana * manaCost);
            if (item.type == 2795) NoPay = true;
            if (item.type == 3852 && IsAltUse)
                Cost = (int)(item.mana * 2 * manaCost);
            if (item.shoot > 0 && IsAltUse)
            {
                if (ProjectileID.Sets.TurretFeature[item.shoot] || ProjectileID.Sets.MinionTargettingFeature[item.shoot])
                    NoPay = true;
            }
            if (item.type == 3006) NoPay = true;
            if (item.type != 3269 && !CheckMana(item, -1, !NoPay))
                return false;
            return CanUse;
        }
        
        private bool CheckDD2CrystalPaymentLock(Item item)
        {
            if (!DD2Event.Ongoing) return true;
            return CountItem(3822) >= GetRequiredDD2CrystalsToUse(item);
        }

        private int GetRequiredDD2CrystalsToUse(Item item)
        {
            switch (item.type)
            {
                case 3818:
                case 3819:
                case 3820:
                case 3824:
                case 3825:
                case 3826:
                case 3832:
                case 3833:
                case 3834:
                case 3829:
                case 3830:
                case 3831:
                    return 10;
                default:
                    return 0;
            }
        }

        private void ApplyReuseDelay()
        {
            itemAnimation = itemTime = reuseDelay;
            reuseDelay = 0;
        }

        private void ItemCheck_HackHoldStyles(Item item)
        {
            if(item.fishingPole > 0)
            {
                item.holdStyle = 0;
                if (ItemTimeIsZero && itemAnimation == 0)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].bobber)
                        {
                            item.holdStyle = 1;
                            break;
                        }
                    }
                }
            }
            if (!ItemID.Sets.IsAKite[item.type]) return;
            item.holdStyle = 0;
            if (!ItemTimeIsZero || itemAnimation != 0)
                return;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && ProjMod.IsThisCompanionProjectile(i, this) && Main.projectile[i].type == item.shoot)
                {
                    item.holdStyle = 1;
                    ChangeDir(!(Main.projectile[i].Center.X - Center.X < 0) ? 1 : -1);
                    break;
                }
            }
        }

        private void ItemCheck_HandleMPItemAnimation(Item item)
        {
            if (CanAutoReuseItem(item)) releaseUseItem = true;
        }

        private void ItemCheck_HandleMount()
        {
            if (!mount.Active) return;
            MountLoader.UseAbility(this, Vector2.Zero, false);
            if (mount.Type == 8)
            {
                noItems = true;
                if (controlUseItem)
                {
                    channel = true;
                    if (releaseUseItem)
                    {
                        mount.UseAbility(this, Vector2.Zero, true);
                        releaseUseItem = false;
                    }
                }
            }
            if (IsPlayerCharacter && gravDir == -1f)
            {
                mount.Dismount(this);
            }
        }

        private void ItemCheck_ApplyManaRegenDelay(Item item)
        {
            if (GetManaCost(item) > 0) manaRegenDelay = (int)maxRegenDelay;
        }

        private void EmitMaxManaEffect()
        {
            SoundEngine.PlaySound(SoundID.SoundByIndex[25]);
            for (byte i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(position, width, height, 45, 0, 0, 255, default(Color), Main.rand.Next(20, 26) * 0.1f);
                Main.dust[d].noLight = true;
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
            }
        }
        #endregion

        public override void HoldItem(Item item)
        {
            //Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            //itemLocation.X = (int)HandPosition.X;
            //itemLocation.Y = (int)HandPosition.Y;
        }

        public override void UseItemHitbox(Item item, ref Rectangle hitbox, ref bool noHitbox)
        { //For item hitbox
            //Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            //hitbox.X = (int)(hitbox.X - position.X + HandPosition.X);
            //hitbox.Y = (int)(hitbox.Y - position.Y + HandPosition.Y);
            //itemLocation.X = (int)(itemLocation.X - position.X + HandPosition.X);
            //itemLocation.Y = (int)(itemLocation.Y - position.Y + HandPosition.Y);
            //itemLocation.X = (int)HandPosition.X;
            //itemLocation.Y = (int)HandPosition.Y;
            //hitbox.X = (int)HandPosition.X;
            //hitbox.Y = (int)HandPosition.Y;
            //hitbox.Width = (int)(hitbox.Width * Scale);
            //hitbox.Height = (int)(hitbox.Height * Scale);
            //itemWidth = (int)(itemWidth * Scale);
            //itemHeight = (int)(itemHeight * Scale);
        }

        public override void HoldStyle(Item item, Rectangle heldItemFrame)
        { //For item holding style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
            itemLocation.X = (int)HandPosition.X;
            itemLocation.Y = (int)HandPosition.Y;
        }

        public override void UseStyle(Item item, Rectangle heldItemFrame)
        { //For item use style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
            itemLocation.X = HandPosition.X;
            itemLocation.Y = HandPosition.Y;
        }

        public Rectangle GetAnimationFrame(int FrameID)
        {
            Rectangle rect = new Rectangle(FrameID, 0, Base.SpriteWidth, Base.SpriteHeight);
            if(rect.X >= Base.FramesInRow)
            {
                rect.Y = rect.Y + rect.X / Base.FramesInRow;
                rect.X = rect.X - rect.Y * Base.FramesInRow;
            }
            rect.X = rect.X * rect.Width;
            rect.Y = rect.Y * rect.Height;
            return rect;
        }

        public enum AnimationStates : byte
        {
            Standing,
            Moving,
            Swiming,
            InAir,
            Defeated,
            RidingMount,
            UsingFurniture,
            WallSliding,
            IceSliding
        }
    }
}