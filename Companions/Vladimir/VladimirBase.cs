using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class VladimirBase : TerraGuardianBase
    {
        public static List<CompanionID> CarryBlacklist = new List<CompanionID>();
        public override string Name => "Vladimir";
        public override string FullName => "Vladimir Svirepyy Varvar"; //Surnames means Ferocious Barbarian
        public override string Description => "A bear TerraGuardian that likes giving hugs to people.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 44;
        public override int Height => 116;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 128;
        public override int SpriteHeight => 160;
        public override float Scale => 138f / 116f;
        public override int FramesInRow => 15;
        public override int Age => 26;
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 250; //1600
        public override int HealthPerLifeCrystal => 50;
        public override int HealthPerLifeFruit => 30;
        public override float AccuracyPercent => 0.72f;
        public override float Gravity => 0.7f;
        public override float MaxRunSpeed => 4.9f;
        public override float RunAcceleration => 0.14f;
        public override float RunDeceleration => 0.42f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.16f;
        public override bool CanCrouch => true;
        public override CompanionData CreateCompanionData => new VladimirData();
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override PartDrawOrdering MountedDrawOrdering => PartDrawOrdering.InBetween;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0, MountUnlock = 3, MoveInUnlock = 0 };
        protected override CompanionDialogueContainer GetDialogueContainer => new VladimirDialogues();
        public override BehaviorBase DefaultFollowLeaderBehavior => new Vladimir.VladimirFollowerBehavior();
        public override BehaviorBase PreRecruitmentBehavior => new Vladimir.VladimirPreRecruitBehavior();

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.WoodenSword),
                new InitialItemDefinition(ItemID.Mushroom, 3)
            };
        }

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 2; i < 10; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(10);
        protected override Animation SetPlayerMountedArmFrame => new Animation(1);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(13, 1);
                anim.AddFrame(14, 1);
                anim.AddFrame(19, 1);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 13; i < 17; i++)
                {
                    anim.AddFrame(i, 1);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(11);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 17; i < 20; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(20);
        protected override Animation SetChairSittingFrames => new Animation(21);
        protected override Animation SetThroneSittingFrames => new Animation(22);
        protected override Animation SetBedSleepingFrames => new Animation(24);
        protected override Animation SetDownedFrames => new Animation(26);
        protected override Animation SetRevivingFrames => new Animation(27);
        protected override Animation SetBackwardStandingFrames => new Animation(29);
        protected override Animation SetBackwardReviveFrames => new Animation(31);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(20, 0);
                anim.AddFrameToReplace(21, 1);
                anim.AddFrameToReplace(23, 2);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(1, 0);
                right.AddFrameToReplace(12, 1);
                right.AddFrameToReplace(20, 2);
                right.AddFrameToReplace(23, 3);
                right.AddFrameToReplace(25, 4);
                right.AddFrameToReplace(28, 5);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        #endregion

        #region Animation Position
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(20, 30, 62);
                anim.AddFramePoint2X(21, 30, 62);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(13, 21, 14);
                left.AddFramePoint2X(14, 45, 26);
                left.AddFramePoint2X(15, 52, 40);
                left.AddFramePoint2X(16, 44, 56);

                left.AddFramePoint2X(17, 21, 20);
                left.AddFramePoint2X(18, 45, 32);
                left.AddFramePoint2X(19, 44, 62);
                
                left.AddFramePoint2X(23, 32, 58);
                left.AddFramePoint2X(25, 23, 72);
                
                left.AddFramePoint2X(27, 44, 71);
                
                left.AddFramePoint2X(28, 42, 65);
                
                left.AddFramePoint2X(32, 42, 65);

                right.AddFramePoint2X(13, 35, 14);
                right.AddFramePoint2X(14, 48, 26);
                right.AddFramePoint2X(15, 55, 40);
                right.AddFramePoint2X(16, 48, 56);
                
                right.AddFramePoint2X(17, 35, 20);
                right.AddFramePoint2X(18, 48, 32);
                right.AddFramePoint2X(19, 48, 62);
                
                right.AddFramePoint2X(23, 32, 58);
                right.AddFramePoint2X(25, 40, 72);
                
                right.AddFramePoint2X(27, 51, 71);
                
                right.AddFramePoint2X(28, 42, 65);
                
                right.AddFramePoint2X(32, 42, 65);
                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(39, 46), true);
                anim.AddFramePoint2X(11, 39, 52);
                anim.AddFramePoint2X(12, 39, 52);
                
                anim.AddFramePoint2X(23, 32, 58);
                anim.AddFramePoint2X(25, 23, 70);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(30, 28), true);
                anim.AddFramePoint2X(11, 30, 34);
                anim.AddFramePoint2X(12, 30, 34);
                anim.AddFramePoint2X(17, 30, 34);
                anim.AddFramePoint2X(18, 30, 34);
                anim.AddFramePoint2X(19, 30, 34);
                
                anim.AddFramePoint2X(23, -1000, -1000);
                anim.AddFramePoint2X(25, -1000, -1000);
                
                anim.AddFramePoint2X(27, 50, 47);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetWingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(23, -1000, -1000);
                anim.AddFramePoint(25, -1000, -1000);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(21, 8, -8);
                anim.AddFramePoint2X(22, -12, -18);
                anim.AddFramePoint2X(23, -12, -18);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset 
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(0, 2), true);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 48, false);
        #endregion
        #region Animation Overrides
        public override void ModifyAnimation(Companion companion)
        {
            VladimirData data = (VladimirData)companion.Data;
            bool SharingThrone = false;
            if (companion.IsUsingThroneOrBench)
            {
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Main.player[p] != companion && Main.player[p].sitting.isSitting && Main.player[p].Bottom == companion.Bottom)
                    {
                        SharingThrone = true;
                        break;
                    }
                }
            }
            if (SharingThrone || companion.GetCharacterMountedOnMe != null || (data.CarrySomeone && data.PickedUpPerson))
            {
                if (companion.GetGoverningBehavior() is MountDismountCompanionBehavior) return;
                short Frame = 1;
                switch (companion.BodyFrameID)
                {
                    case 11:
                        Frame = 12;
                        break;
                    case 22:
                        Frame = 23;
                        break;
                    case 24:
                        Frame = 25;
                        break;
                    case 27:
                        Frame = 28;
                        break;
                    case 29:
                        Frame = 30;
                        break;
                    case 31:
                        Frame = 32;
                        break;
                }
                if (companion.BodyFrameID == 0 || companion.BodyFrameID == 11 || companion.BodyFrameID == 22 || companion.BodyFrameID == 24)
                    companion.BodyFrameID = Frame;
                if ((companion as TerraGuardian).HeldItems[1].ItemAnimation == 0)
                {
                    companion.ArmFramesID[1] = Frame;
                }
                if (companion.itemAnimation == 0)
                {
                    companion.ArmFramesID[0] = Frame;
                }
            }
        }
        #endregion
        public override void UpdateBehavior(Companion companion)
        {
            UpdateCarryAlly((TerraGuardian)companion, (VladimirData)companion.Data);
        }

        public override void UpdateCompanion(Companion companion)
        {
            UpdateCarriedAllyPosition((TerraGuardian)companion);
            if (companion.GetCharacterMountedOnMe != null)
                companion.GetCharacterMountedOnMe.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
        }

        private void UpdateCarryAlly(TerraGuardian guardian, VladimirData data)
        {
            if (!data.CarrySomeone)
            {
                if (guardian.Owner != null) return;
                TryCarryingSomeone(guardian, data);
                return;
            }
            if (!data.PickedUpPerson)
            {
                if (data.CarriedCharacter == null)
                {
                    data.CarrySomeone = false;
                    return;
                }
                if (guardian.IsRunningBehavior) return;
                if (guardian.Owner != null) guardian.WalkMode = true;
                data.Time++;
                Entity Target = data.CarriedCharacter;
                if (!Target.active)
                {
                    data.CarrySomeone = false;
                    return;
                }
                guardian.MoveLeft = false;
                guardian.MoveRight = false;
                if (guardian.Hitbox.Intersects(Target.Hitbox))
                {
                    data.PickedUpPerson = true;
                    data.Time = 0;
                    guardian.Path.CancelPathing();
                }
                else
                {
                    if (guardian.Path.State != PathFinder.PathingState.TracingPath)
                    {
                        guardian.CreatePathingTo(Target.Bottom, guardian.WalkMode, CancelOnFail: true);
                    }
                    /*if (guardian.Center.X < Target.Center.X)
                    {
                        guardian.MoveRight = true;
                    }
                    else
                    {
                        guardian.MoveLeft = true;
                    }*/
                }
                if (!data.PickedUpPerson) return;
            }
            if (!data.WasFollowingPlayerBefore)
            {
                if (!guardian.TargettingSomething)
                {
                    data.Time++;
                }
                if (data.Time >= data.Duration)
                {
                    PlaceCarriedPersonOnTheFloor(guardian, data, false);
                    return;
                }
            }
            if (guardian.itemAnimation > 0) guardian.controlTorch = false;
            if (data.WasFollowingPlayerBefore)
            {
                if (guardian.Owner == null)
                {
                    guardian.SaySomething("*[nickname] will still need your help, better you go with them.*");
                    PlaceCarriedPersonOnTheFloor(guardian, data, false);
                    return;
                }
            }
            else if (guardian.Owner != null)
            {
                guardian.SaySomething("It might be dangerous, better you stay here.*");
                PlaceCarriedPersonOnTheFloor(guardian, data, false);
                return;
            }
            if (data.CarriedCharacter == guardian.Owner)
            {
                
            }
        }

        private void UpdateCarriedAllyPosition(TerraGuardian Vladimir)
        {
            VladimirData data = (VladimirData)Vladimir.Data;
            if (!data.CarrySomeone) return;
            Entity Target = data.CarriedCharacter;
            if (Target == Vladimir || Target == null)
            {
                data.CarrySomeone = false;
                data.CarriedCharacter = null;
                return;
            }
            else if (data.PickedUpPerson)
            {
                if (Vladimir.KnockoutStates > KnockoutStates.Awake)
                {
                    PlaceCarriedPersonOnTheFloor(Vladimir, data);
                    return;
                }
                DrawOrderInfo.AddDrawOrderInfo(data.CarriedCharacter, Vladimir, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                if (Target is Companion)
                {
                    
                }
                if (Target is NPC)
                {
                    NPC npc = (NPC)Target;
                    if (!npc.active)
                    {
                        data.CarrySomeone = false;
                        data.CarriedCharacter = null;
                        return;
                    }
                    npc.position = Vladimir.GetMountShoulderPosition;
                    npc.position.X -= npc.width * 0.5f;
                    npc.position.Y -= npc.height * 0.5f + 8;
                    if (npc.velocity.X == 0)
                        npc.direction = -Vladimir.direction;
                    if (Vladimir.IsMountedOnSomething)
                        npc.position.X += 4 * Vladimir.direction * Vladimir.Scale;
                    npc.velocity = Vector2.Zero;
                    npc.velocity.Y = -0.3f;
                    npc.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                }
                else if (Target is TerraGuardian)
                {
                    TerraGuardian tg = (TerraGuardian)Target;
                    if (!tg.active)
                    {
                        data.CarrySomeone = false;
                        data.CarriedCharacter = null;
                        return;
                    }
                    if (tg.itemAnimation <= 0)
                        tg.ChangeDir(Vladimir.direction);
                    if (tg.IsMountedOnSomething)
                    {
                        tg.ToggleMount(tg.GetCharacterMountedOnMe, true);
                    }
                    if (tg.UsingFurniture)
                        tg.LeaveFurniture();
                    Vector2 HeldPosition = Vladimir.GetMountShoulderPosition;
                    tg.position = HeldPosition;
                    tg.position.Y -= tg.height * 0.5f;
                    tg.position.X -= tg.width * 0.5f;
                    tg.velocity.X = 0;
                    tg.velocity.Y = -Player.defaultGravity;
                    tg.gfxOffY = 0;
                    tg.SetFallStart();
                    if (tg.KnockoutStates > KnockoutStates.Awake)
                        tg.GetPlayerMod.ChangeReviveStack(3);
                    else
                        tg.IncreaseComfortStack(0.02f);
                    tg.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                }
                else
                {
                    Player p = (Player)Target;
                    if (!p.active)
                    {
                        data.CarrySomeone = false;
                        data.CarriedCharacter = null;
                        return;
                    }
                    p.position = Vladimir.GetMountShoulderPosition;
                    p.position.X -= p.width * 0.5f;
                    p.position.Y -= p.height * 0.5f + 8;
                    if (Vladimir.IsMountedOnSomething)
                        p.position.X += 4 * Vladimir.direction * Vladimir.Scale;
                    p.fallStart = (int)(p.position.Y * Companion.DivisionBy16);
                    p.velocity.X = 0;
                    p.velocity.Y = -Player.defaultGravity;
                    PlayerMod pm = p.GetModPlayer<PlayerMod>();
                    if (pm.KnockoutState > KnockoutStates.Awake)
                        pm.ChangeReviveStack(3);
                    if (p.itemAnimation == 0)
                        p.direction = -Vladimir.direction;
                    p.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                    if (p == MainMod.GetLocalPlayer && p.controlJump)
                    {
                        data.CarrySomeone = false;
                        data.CarriedCharacter = null;
                    }
                }
            }
        }

        private void TryCarryingSomeone(TerraGuardian Vladimir, VladimirData data)
        {
            if (!(Vladimir.HasHouse && !Vladimir.TargettingSomething && !Dialogue.IsParticipatingDialogue(Vladimir) && !Vladimir.IsRunningBehavior && Main.rand.Next(350) == 0))
                return;
            List<Entity> PotentialCharacters = new List<Entity>();
            const float SearchRange = 80;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].townNPC && (Main.npc[i].Center - Vladimir.Center).Length() < SearchRange)
                {
                    PotentialCharacters.Add(Main.npc[i]);
                }
            }
            for (int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if (player.active && !(player is Companion) && !Vladimir.IsHostileTo(player) && player != Vladimir.Owner &&
                    player.velocity.Length() == 0 && PlayerMod.PlayerGetControlledCompanion(player) == null && 
                    player.itemAnimation == 0 && (player.Center - Vladimir.Center).Length() < SearchRange)
                {
                    PotentialCharacters.Add(player);
                }
            }
            foreach (uint i in MainMod.ActiveCompanions.Keys)
            {
                Companion comp = MainMod.ActiveCompanions[i];
                if (i != Vladimir.GetWhoAmID && !Vladimir.IsHostileTo(comp) && comp.Owner == null &&
                    !comp.UsingFurniture && comp.height < Vladimir.height * 0.95f && 
                    !CarryBlacklist.Any(x => x.IsSameID(comp.GetCompanionID)) &&
                    (comp.Center - Vladimir.Center).Length() < SearchRange)
                {
                    PotentialCharacters.Add(comp);
                }
            }
            if (PotentialCharacters.Count > 0)
            {
                int Time = Main.rand.Next(600, 1400) * 3;
                CarrySomeoneAction(Vladimir, data, PotentialCharacters[Main.rand.Next(PotentialCharacters.Count)], Time);
                PotentialCharacters.Clear();
            }
        }

        public void CarrySomeoneAction(TerraGuardian Vladimir, VladimirData data, Entity Target, int Time = 0, bool InstantPickup = false)
        {
            if (data.CarrySomeone)
            {
                PlaceCarriedPersonOnTheFloor(Vladimir, data);
                //Place on the ground
            }
            data.CarrySomeone = true;
            data.PickedUpPerson = InstantPickup;
            data.WasFollowingPlayerBefore = Vladimir.Owner != null;
            data.CarriedCharacter = Target;
            data.Duration = Time;
        }

        public void PlaceCarriedPersonOnTheFloor(TerraGuardian Vladimir, VladimirData data, bool WillPickupLater = false)
        {
            if (!data.CarrySomeone) return;
            if (WillPickupLater) data.PickedUpPerson = false;
            else data.CarrySomeone = false;
            data.CarriedCharacter.Bottom = Vladimir.Bottom;
            if (data.CarriedCharacter is Player)
                (data.CarriedCharacter as Player).fallStart = (int)(data.CarriedCharacter.position.Y * Companion.DivisionBy16);
            if (!data.CarrySomeone)
                data.CarriedCharacter = null;
        }

        public override void ModifyMountedCharacterPosition(Companion companion, Player MountedCharacter, ref Vector2 Position)
        {
            if ((companion.Data as VladimirData).CarrySomeone)
            {
                Position.X -= 6f * companion.direction;
            }
        }
    }
}