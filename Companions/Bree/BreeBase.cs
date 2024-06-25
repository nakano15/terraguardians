using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.Xml;

namespace terraguardians.Companions
{
    public class BreeBase : TerraGuardianBase
    {
        static bool WearingBag = true;
        public override string Name => "Bree";
        public override string FullName => "Bree Mair"; //Means Steward, warden or bailiff
        public override string Description => "Her trek begun after her husband has disappeared. Even after she find him, she might stay for a while, until she remembers in which world they lived on.";
        public override Sizes Size => Sizes.Small;
        public override TalkStyles TalkStyle => TalkStyles.Normal;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 18);
        public override CompanionID? IsPartnerOf => new CompanionID(CompanionDB.Sardine);
        public override int Width => 16;
        public override int Height => 46;
        public override int SpriteWidth => 64;
        public override int SpriteHeight => 64;
        public override float Scale => 42f / 46;
        public override int FramesInRow => 24;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int Age => 23;
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 85; //375
        public override int HealthPerLifeCrystal => 14;
        public override int HealthPerLifeFruit => 4;
        public override float AccuracyPercent => 0.73f;
        public override float MaxRunSpeed => 4.76f;
        public override float RunAcceleration => 0.14f;
        public override float RunDeceleration => 0.6f;
        public override int JumpHeight => 14;
        public override float JumpSpeed => 9.88f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer;
        public override CompanionGroup GetCompanionGroup => MainMod.GetCaitSithGroup;
        public override bool CanUseHeavyItem => true;
        public override bool AllowSharingBedWithPlayer => false;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.NPCHit51;
        public override SoundStyle DeathSound => Terraria.ID.SoundID.NPCDeath54;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks()
        {
            FollowerUnlock = 2,
            MountUnlock = 4,
            MoveInUnlock = 0,
            ControlUnlock = 14
        };
        protected override CompanionDialogueContainer GetDialogueContainer => new BreeDialogue();
        public override BehaviorBase PreRecruitmentBehavior => new Bree.BreeRecruitmentBehavior();
        public override CompanionData CreateCompanionData => new BreeData();
        public override void UpdateAttributes(Companion companion)
        {
            companion.DodgeRate += 35;
            companion.GetDamage<MeleeDamageClass>() += 0.05f;
            companion.DefenseRate += 0.05f;
            companion.statDefense += 5;
        }

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]{
                new InitialItemDefinition(ItemID.PlatinumBroadsword),
                new InitialItemDefinition(ItemID.FlintlockPistol),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            };
        }
        public override bool CanSpawnNpc()
        {
            return NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++) anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 13; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(14);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetChairSittingFrames => new Animation(15);
        protected override Animation SetThroneSittingFrames => new Animation(16);
        protected override Animation SetBedSleepingFrames => new Animation(17);
        protected override Animation SetRevivingFrames => new Animation(18);
        protected override Animation SetDownedFrames => new Animation(19);
        protected override Animation SetPetrifiedFrames => new Animation(20);
        protected override Animation SetBackwardStandingFrames => new Animation(21);
        protected override Animation SetBackwardReviveFrames => new Animation(22);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                anim.AddFrameToReplace(15, 1);
                anim.AddFrameToReplace(23, 1);
                return anim;
            }
        }
        public override void ModifyAnimation(Companion companion)
        {
            if(companion.sitting.isSitting && companion.Owner != null)
            {
                Player p = companion.Owner;
                if(p.sitting.isSitting && p.Bottom == companion.Bottom)
                {
                    companion.BodyFrameID = 23;
                    for(byte i = 0; i < companion.ArmFramesID.Length; i++)
                        companion.ArmFramesID[i] = 23;
                }
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),right = new AnimationPositionCollection();

                left.AddFramePoint2X(10, 10, 13);
                left.AddFramePoint2X(11, 19, 16);
                left.AddFramePoint2X(12, 23, 22);
                left.AddFramePoint2X(13, 21, 28);
                
                left.AddFramePoint2X(14, 13, 20);

                right.AddFramePoint2X(10, 13, 13);
                right.AddFramePoint2X(11, 22, 16);
                right.AddFramePoint2X(12, 26, 22);
                right.AddFramePoint2X(13, 24, 28);

                return new AnimationPositionCollection[]{ left , right };
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(14, 16, 29);
                anim.AddFramePoint2X(15, 16, 29);
                anim.AddFramePoint2X(23, 16, 29);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(13, 19), true);
                anim.AddFramePoint2X(14, 16, 29);
                anim.AddFramePoint2X(18, 13, 20);
                anim.AddFramePoint2X(22, 13, 20);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(17, 20), true);
                anim.AddFramePoint2X(16, 15, 13);
                anim.AddFramePoint2X(18, 17, 22);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(15, -1, 0);
                anim.AddFramePoint2X(23, -1, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(17, 16, 0);
                return anim;
            }
        }
        #endregion

        #region Sprite Management
        public const string BagSpriteID = "bag";
        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(BagSpriteID, "bags");
        }
        #endregion

        public override void UpdateCompanion(Companion companion)
        {
            WearingBag = !nterrautils.PlayerMod.GetPlayerQuestData(MainMod.GetLocalPlayer, QuestDB.Stay, MainMod.GetModName).IsCompleted || (companion.Data as BreeData).IsWearingBag;
        }

        #region Skins
        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if(!IsDrawingFrontLayer && WearingBag)
            {
                TerraGuardian tg = Holder.GetCompanion as TerraGuardian;
                Rectangle BodyFrame = tg.BodyFrame;
                Texture2D bag = GetSpriteContainer.GetExtraTexture(BagSpriteID);
                /*if (bag == null)
                    Main.NewText("Texture loaded incorrectly.");
                else
                    Main.NewText("Texture size: " + bag.Width + "x" + bag.Height);*/
                DrawData dd = new DrawData(bag, Holder.DrawPosition, BodyFrame, Holder.DrawColor, tg.fullRotation, Holder.Origin, tg.Scale, drawSet.playerEffect, 0);
                DrawDatas.Add(dd);
                BodyFrame.Y += BodyFrame.Height;
                dd = new DrawData(bag, Holder.DrawPosition, BodyFrame, Holder.DrawColor, tg.fullRotation, Holder.Origin, tg.Scale, drawSet.playerEffect, 0);
                DrawDatas.Insert(0, dd);
                BodyFrame.Y -= BodyFrame.Height;
            }
        }
        protected override void SetupSkinsOutfitsContainer(ref Dictionary<byte, CompanionSkinInfo> Skins, ref Dictionary<byte, CompanionSkinInfo> Outfits)
        {
            Skins.Add(1, new Bree.WitchSkin());
            Outfits.Add(1, new Bree.DamselOutfit());
            Outfits.Add(2, new Bree.WitchOutfit());
        }
        #endregion

        public class BreeData : CompanionData
        {
            protected override uint CustomSaveVersion => 1;
            public bool IsWearingBag = true;

            public override void CustomSave(TagCompound save, uint UniqueID)
            {
                save.Add("WearingBag_"+UniqueID, IsWearingBag);
            }

            public override void CustomLoad(TagCompound tag, uint UniqueID, uint LastVersion)
            {
                if (LastVersion == 0) return;
                IsWearingBag = tag.GetBool("WearingBag_"+UniqueID);
            }
        }
    }
}