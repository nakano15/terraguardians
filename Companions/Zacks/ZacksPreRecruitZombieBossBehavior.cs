using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace terraguardians.Companions.Zacks
{
    public class ZacksPreRecruitZombieBossBehavior : BehaviorBase
    {
        uint ZacksID { get { return CompanionDB.Zacks; } }
        private bool IsKnownCompanion { get { return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, ZacksID); } }
        private byte BossLevel = 255;
        private int Damage = 5;
        private bool IsBossVersion = true;
        private bool Incapacitated = false;
        private bool DoAllowFinishing = false;
        private Companion companion;
        private byte AI_State = 0;
        private int AI_Value = 0;
        private List<Player> CharactersInvolved = new List<Player>();
        private Microsoft.Xna.Framework.Vector2 PullStartPosition = Microsoft.Xna.Framework.Vector2.Zero;
        private float PullTime = 0;
        private const int DialogueLineTime = (int)(2.5f * 60);
        private Vector2 SwordPosition = Vector2.Zero;
        byte StuckCounter = 0, TileStuckCounter = 0;
        short AttackFrame = -1;

        public ZacksPreRecruitZombieBossBehavior()
        {
            CanBeHurtByNpcs = false;
            IsVisible = false;
            RunCombatBehavior = false;
        }

        public override string CompanionNameChange(Companion companion)
        {
            if(IsBossVersion) return "Zombie Guardian";
            return base.CompanionNameChange(companion);
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return Incapacitated;
        }

        public override void UpdateStatus(Companion companion)
        {
            if (!IsBossVersion) return;
            if(BossLevel == 255)
            {
                BossLevel = GetBossLevel();
            }
            switch(BossLevel)
            {
                default:
                    companion.statLifeMax2 = 3000;
                    Damage = 15;
                    companion.statDefense += 5;
                    break;
                case 1:
                    companion.statLifeMax2 = 4500;
                    Damage = 45;
                    companion.statDefense += 20;
                    break;
                case 2:
                    companion.statLifeMax2 = 9000;
                    Damage = 56;
                    companion.statDefense += 24;
                    break;
                case 3:
                    companion.statLifeMax2 = 18000;
                    Damage = 64;
                    companion.statDefense += 28;
                    break;
                case 4:
                    companion.statLifeMax2 = 36000;
                    Damage = 78;
                    companion.statDefense += 32;
                    break;
                case 5:
                    companion.statLifeMax2 = 42000;
                    Damage = 106;
                    companion.statDefense += 36;
                    break;
            }
            companion.noKnockback = true;
        }

        public override void Update(Companion companion)
        {
            if(Incapacitated)
            {
                companion.MoveLeft = companion.MoveRight = companion.ControlJump = false;
                CanBeAttacked = false;
                return;
            }
            this.companion = companion;
            bool RisingFromTheGround = false;
            AttackFrame = -1;
            if (AI_State == 0)
            {
                RisingFromTheGround = true;
            }
            else
            {
                if (AI_State == 1 && AI_Value % 5 == 0)
                {
                    bool PullingPlayer = false;
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i] != companion && Main.player[i].active)
                        {
                            Player player = Main.player[i];
                            if (CharactersInvolved.Contains(player))
                            {
                                if (player.dead || player.ghost)
                                    CharactersInvolved.Remove(player);
                                else
                                {
                                    if(MathF.Abs(player.Center.X - companion.Center.X) >= 368f || 
                                    MathF.Abs(player.Center.Y - companion.Center.Y) >= 256f)
                                    {
                                        Pull(player);
                                        PullingPlayer = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (!player.dead && !player.ghost)
                                {
                                    if (System.MathF.Abs(player.Center.X - companion.Center.X) <= NPC.sWidth && System.MathF.Abs(player.Center.Y - companion.Center.Y) <= NPC.sHeight)
                                    {
                                        CharactersInvolved.Add(player);
                                    }
                                }
                            }
                        }
                    }
                    if (!PullingPlayer)
                    {
                        foreach(Companion c in MainMod.ActiveCompanions.Values)
                        {
                            if (c != companion)
                            {
                                if (CharactersInvolved.Contains(c))
                                {
                                    if (c.dead || c.ghost)
                                    {
                                        CharactersInvolved.Remove(c);
                                    }
                                    else
                                    {
                                        if(MathF.Abs(c.Center.X - companion.Center.X) >= 368f || 
                                        MathF.Abs(c.Center.Y - companion.Center.Y) >= 256f)
                                        {
                                            Pull(c);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!c.dead && !c.ghost)
                                    {
                                        if (System.MathF.Abs(c.Center.X - companion.Center.X) <= NPC.sWidth && System.MathF.Abs(c.Center.Y - companion.Center.Y) <= NPC.sHeight)
                                        {
                                            CharactersInvolved.Add(c);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            CanBeAttacked = !Incapacitated && !RisingFromTheGround && AI_State != 2 && AI_State != 3;
            bool MoveRight = companion.direction == 1;
            if (companion.Target != null)
            {
                MoveRight = companion.Target.Center.X - companion.Center.X >= 0;
            }
            bool MoveForward = false;
            Player Target = (Player)companion.Target;
            switch(AI_State)
            {
                case 1: //Chase target
                    {
                        if (!IsBossVersion)
                        {
                            AI_State = 200;
                            AI_Value = 0;
                        }
                        else
                        {
                            companion.WalkMode = true;
                            AI_Value++;
                            if (AI_Value >= 180)
                            {
                                if (!Target.active || Target.dead)
                                {
                                    AI_Value = 0;
                                }
                                else
                                {
                                    if (companion.velocity.X == 0)
                                    {
                                        AI_State = (byte)(5 + Main.rand.Next(3));
                                        AI_Value = 0;
                                    }
                                }
                            }
                            else
                            {
                                MoveForward = true;
                            }
                        }
                    }
                    break;
                case 2: //Move underground
                    {
                        AI_Value++;
                        if(AI_Value == companion.height)
                        {
                            AI_Value = 0;
                            AI_State = 3;
                        }
                    }
                    break;
                case 3: //Return from underground
                    {
                        RisingFromTheGround = true;
                    }
                    break;
                case 4: //Pull Player
                    {
                        const int PullMaxTime = 45;
                        if(!Target.active || Target.dead)
                        {
                            AI_State = 1;
                            AI_Value = 0;
                        }
                        else if (AI_Value >= PullMaxTime)
                        {
                            if (PullStartPosition.X == 0 || PullStartPosition.Y == 0)
                            {
                                PullStartPosition = Target.Center;
                                PullTime = (PullStartPosition - companion.Center).Length() / 8;
                            }
                            float Percentage = (AI_Value - PullMaxTime) / PullTime;
                            if (Percentage >= 1)
                            {
                                Vector2 NewPosition = new Vector2(
                                    companion.Center.X - Target.width * 0.5f + companion.width * 0.5f * companion.direction,
                                    companion.position.Y - Target.height * 0.25f);
                                Target.position = NewPosition;
                                Target.velocity = Vector2.Zero;
                                Target.direction = -companion.direction;
                                if (!IsBossVersion)
                                {
                                    //Prank
                                    int NewAITime = (int)(AI_Value - (PullMaxTime + (int)PullTime));
                                    AI_Value++;
                                    if (NewAITime == 30)
                                    {
                                        companion.SaySomething("*Hahaha, I can feel your heart racing now.*");
                                    }
                                    else if (NewAITime == 30 + DialogueLineTime)
                                    {
                                        companion.SaySomething("*I say you on the woods, and thought that would be fun to give you a scare.");
                                    }
                                    else if (NewAITime == 60 + DialogueLineTime * 2)
                                    {
                                        AI_State = 200;
                                        AI_Value = (int)(30 + DialogueLineTime * 2 - 1);
                                    }
                                }
                                else if (Target.immuneTime <= 0)
                                {
                                    int DefBackup = Target.statDefense;
                                    Target.statDefense = 0;
                                    Target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Target.name + " has turned into zombie food."), (int)(Target.statLifeMax2 * 0.2f), companion.direction);
                                    if (Main.expertMode)
                                    {
                                        companion.statLife += (int)(companion.statLifeMax2 * 0.05f);
                                        if (companion.statLife > companion.statLifeMax2)
                                            companion.statLife = companion.statLifeMax2;
                                        Target.AddBuff(Terraria.ID.BuffID.Bleeding, 15 * 60);
                                    }
                                    Target.statDefense = DefBackup;
                                    AI_Value++;
                                    if (Target.dead)
                                    {
                                        AI_State = 1;
                                        AI_Value = 0;
                                    }
                                    else if(AI_Value >= PullMaxTime + (int)PullTime + 3)
                                    {
                                        AI_State = 1;
                                        AI_Value = 0;
                                        Target.velocity = new Vector2(companion.direction * 7.5f, -9.25f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (PullStartPosition.X != 0 || PullStartPosition.Y != 0)
                            {
                                PullStartPosition = Vector2.Zero;
                            }
                            AI_Value++;
                        }
                    }
                    break;
                case 5: //Blood Vomit Attack
                    {
                        if (AI_Value == 0)
                        {
                            if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                            {
                                MoveRight = Target.Center.X >= companion.Center.X;
                                AI_Value++;
                            }
                        }
                        else
                        {
                            Vector2 VomitSpawnPosition = companion.Center;
                            VomitSpawnPosition.Y -= companion.height * 0.25f + 4;
                            VomitSpawnPosition.X += companion.width * 0.25f * companion.direction;
                            const float MaxVomitTime = 90;
                            if (AI_Value == 30)
                            {
                                //play zombie sound
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.ZombieMoan, companion.position);
                            }
                            if (AI_Value < 10)
                            {
                                Dust.NewDust(VomitSpawnPosition, 4, 4, 5, Main.rand.Next(-20, 21) * 0.01f, Main.rand.Next(-20, 21) * 0.01f);
                            }
                            else if(AI_Value >= 30 + MaxVomitTime)
                            {
                                if (AI_Value >= 30 + 20 + MaxVomitTime)
                                {
                                    AI_State = 1;
                                    AI_Value = 0;
                                    break;
                                }
                            }
                            else if(AI_Value >= 30 && AI_Value % 3 == 0)
                            {
                                float SpawnDirection = 1.570796326794897f;
                                float Percentage = (float)(AI_Value - 30) / MaxVomitTime;
                                SpawnDirection -= 3.141592653589793f * Percentage * companion.direction;
                                //if (npc.direction < 0)
                                //    SpawnDirection += 3.141592653589793f;
                                float VomitSpeed = 8f;
                                int Damage = 30 + BossLevel * 10;
                                int Proj = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_SpawnNPC(), VomitSpawnPosition, new Vector2((float)Math.Cos(SpawnDirection) * VomitSpeed, (float)Math.Sin(SpawnDirection) * VomitSpeed), ModContent.ProjectileType<Projectiles.BloodVomit>(), Damage, 16f, companion.whoAmI);
                                Main.projectile[Proj].GetGlobalProjectile<ProjMod>().ProjectileOwnerCompanion = companion;
                            }
                            AI_Value++;
                        }
                    }
                    break;

                case 6: //Heavy Attack Swing
                    {
                        int SwordSwingTime = Main.expertMode? 33 : 38;
                        int SwordAttackReactionTime = Main.expertMode ? 15 : 25;
                        int PosSwordAttackRecoveryTime = Main.expertMode ? 15 : 30;
                        float Percentage = Math.Clamp((AI_Value - SwordAttackReactionTime) / SwordSwingTime, 0f, 1f);
                        short Frame = 0;
                        if (Percentage < 0.45f)
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(0);
                        else if (Percentage < 0.65f)
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(1);
                        else
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(2);
                        SwordPosition = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, Frame);
                        AttackFrame = Frame;
                        float RotationValue = (float)Math.Sin(Percentage * 1.35f) * (300 * Percentage);
                        RotationValue = MathHelper.ToRadians(-158 + RotationValue) * companion.direction;
                        if (Percentage > 0 && Percentage < 1)
                        {
                            const int ItemWidth = 22, ItemHeight = 96, ItemOriginX = 10, ItemOriginY = 88;
                            Rectangle WeaponCollision = new Rectangle();
                            WeaponCollision.Width = (int)(ItemHeight * Math.Sin(RotationValue) + ItemWidth * Math.Cos(RotationValue));
                            WeaponCollision.Height = (int)(ItemHeight * Math.Cos(RotationValue) + ItemWidth * Math.Sin(RotationValue)) * -1;
                            WeaponCollision.X -= (int)((ItemHeight - ItemOriginY) * Math.Sin(RotationValue) + (ItemWidth - ItemOriginX) * Math.Cos(RotationValue));
                            WeaponCollision.Y -= (int)((ItemHeight - ItemOriginY) * Math.Cos(RotationValue) + (ItemWidth - ItemOriginX) * Math.Sin(RotationValue)) * -1;
                            if (WeaponCollision.Width < 0)
                            {
                                WeaponCollision.X += WeaponCollision.Width;
                                WeaponCollision.Width *= -1;
                            }
                            if (WeaponCollision.Height < 0)
                            {
                                WeaponCollision.Y += WeaponCollision.Height;
                                WeaponCollision.Height *= -1;
                            }
                            WeaponCollision.X += (int)SwordPosition.X;
                            WeaponCollision.Y += (int)SwordPosition.Y;

                            int SlashDamage = (int)(Damage * 1.2f);
                            for (int i = 0;i < 255; i++)
                            {
                                if (!Main.player[i].active || Main.player[i].dead || Main.player[i].ghost || Main.player[i].immuneTime > 0 || !Main.player[i].getRect().Intersects(WeaponCollision))
                                {
                                    double damage = Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " was sliced in half by a Zombie Guardian."), SlashDamage, companion.direction);
                                    if (Main.expertMode && damage > 0)
                                        Main.player[i].AddBuff(Terraria.ID.BuffID.BrokenArmor, 30 * 60);
                                }
                            }
                        }
                        AI_Value++;
                        if (AI_Value >= SwordSwingTime + SwordAttackReactionTime + PosSwordAttackRecoveryTime)
                        {
                            AI_State = 1;
                            AI_Value = 0;
                        }
                    }
                    break;

                case 7: //Rear attack.
                    {
                        int TimeUntilUse = 60, TimePosLease = 20;
                        if (Main.expertMode)
                        {
                            TimeUntilUse = 35;
                            TimePosLease = 10;
                        }
                        if (AI_Value < TimeUntilUse)
                        {
                            AI_Value ++;
                            if (AI_Value == (int)(TimeUntilUse * 0.5f))
                                companion.direction *= -1;
                        }
                        else
                        {
                            if (AI_Value == TimeUntilUse)
                            {
                                Vector2 SpawnPosition = companion.Center;
                                SpawnPosition.Y += companion.height * 0.25f;
                                Vector2 InitialVelocity = new Vector2(-0.3f * companion.direction, 0.33f);
                                int Proj = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_SpawnNPC(), SpawnPosition, InitialVelocity, ModContent.ProjectileType<Projectiles.ZombieFart>(), 0, 0, companion.whoAmI);
                                Main.projectile[Proj].GetGlobalProjectile<ProjMod>().ProjectileOwnerCompanion = companion;
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62, companion.position);
                            }
                            else if(AI_Value >= TimePosLease + TimeUntilUse)
                            {
                                AI_State = 1;
                                AI_Value = 0;
                                break;
                            }
                            AI_Value++;
                        }
                    }
                    break;
            }
            if (MoveForward)
            {
                if(companion.velocity.Y != 0 && companion.oldVelocity.Y == 0)
                    StuckCounter++;
                if (StuckCounter >= 3)
                {
                    StuckCounter = 0;
                    AI_State = 2;
                    AI_Value = 0;
                }
                if(companion.direction < 0)
                    companion.MoveLeft = true;
                else
                    companion.MoveRight = true;
                if (companion.position.X == companion.oldPosition.X)
                {
                    TileStuckCounter++;
                    if (TileStuckCounter >= 50)
                    {
                        TileStuckCounter = 0;
                        AI_State = 2;
                        AI_Value = 0;
                    }
                }
            }
            else if(companion.velocity.Y == 0)
            {
                StuckCounter = 0;
                TileStuckCounter = 0;
            }
            if (RisingFromTheGround)
            {
                if (AI_Value == 0)
                {
                    IsVisible = false;
                    if (companion.Target != null)
                    {
                        companion.Center = Target.Center;
                        int TileX = (int)(Target.Center.X * (1f / 16)) + Target.direction * -4,
                            TileY = (int)((Target.Bottom.Y + 1) * (1f / 16));
                        byte Attempts = 5;
                        bool UpClear = false, DownHasTile = false, IsGrassOrDirt = false;
                        while(Attempts > 0)
                        {
                            UpClear = true;
                            for(int y = 0; y < companion.height / 16 + 1; y++)
                            {
                                for(int x = 0; x < companion.width / 16 + 1; x++)
                                {
                                    Tile tile = Main.tile[TileX - 1 + x, TileY - 1 - y];
                                    bool IsClear = !tile.HasTile || !Main.tileSolid[tile.TileType];
                                    if (!IsClear)
                                    {
                                        UpClear = false;
                                        break;
                                    }
                                }
                            }
                            if(UpClear)
                            {
                                DownHasTile = Main.tile[TileX, TileY].HasTile || Main.tile[TileX + 1, TileY].HasTile;
                                if (DownHasTile)
                                {
                                    switch(Main.tile[TileX, TileY].TileType)
                                    {
                                        case TileID.Dirt:
                                        case TileID.Mud:
                                        case TileID.Grass:
                                        case TileID.CorruptGrass:
                                        case TileID.CrimsonGrass:
                                        case TileID.HallowedGrass:
                                        case TileID.JungleGrass:
                                        case TileID.MushroomGrass:
                                            IsGrassOrDirt = true;
                                            break;
                                    }
                                    if (IsGrassOrDirt)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    TileY++;
                                }
                            }
                            else
                            {
                                TileY--;
                            }
                            Attempts--;
                        }
                        if (UpClear && DownHasTile)
                        {
                            if (!IsGrassOrDirt)
                            {
                                return;
                            }
                            companion.position.X = TileX * 16 - companion.width * Target.direction * 0.5f;
                            companion.position.Y = TileY * 16 - companion.height;
                            companion.fallStart = (int)(companion.position.Y * (1f / 16));
                            if(Target.Center.X < companion.Center.X)
                                companion.direction = -1;
                            else
                                companion.direction = 1;
                            AI_Value++;
                            IsVisible = true;
                        }
                    }
                }
                else
                {
                    if (AI_Value < companion.height)
                        AI_Value++;
                    else
                    {
                        AI_State = 1;
                        AI_Value = 0;
                        if (Target.Hitbox.Intersects(companion.Hitbox))
                        {
                            AI_State = 4;
                        }
                    }
                }
            }
        }

        public void Pull(Player victim)
        {

        }

        public override bool IsHostileTo(Player target)
        {
            return IsBossVersion;
        }

        public override bool CanKill(Companion companion)
        {
            Incapacitated = true;
            return DoAllowFinishing;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if(Incapacitated && companion.velocity.Y == 0)
            {
                short frame = companion.Base.GetAnimation(AnimationTypes.DownedFrames).GetFrame(0);
                companion.BodyFrameID = frame;
                for (int i = 0; i < companion.ArmFramesID.Length; i++)
                    companion.ArmFramesID[i] = frame;
            }
            if (AttackFrame > -1)
            {
                companion.BodyFrameID = AttackFrame;
                for (int i = 0; i < companion.ArmFramesID.Length; i++)
                    companion.ArmFramesID[i] = AttackFrame;
                return;
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            MessageDialogue m = new MessageDialogue();
            if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Blue))
            {
                m.ChangeMessage("*[nickname], let's try talking to him.*");
            }
            else if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
            {
                m.ChangeMessage("*The zombie seems less violent due to the presence of something in this world.*");
            }
            else
            {
                m.ChangeMessage("*It turned less violent. Maybe I could try talking to it?*");
            }
            m.AddOption("Try talking.", OnTryTalking);
            m.AddOption("Finish him.", OnFinishingHim);
            return m;
        }

        public void OnTryTalking()
        {
            if(PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Blue))
            {
                
            }
        }

        public void OnFinishingHim()
        {
            DoAllowFinishing = true;
            companion.statLife = 1;
            companion.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(MainMod.GetLocalPlayer.name + " has ended the Zombie TerraGuardian's misery."), 9999, 0);
            Dialogue.EndDialogue();
        }

        public byte GetBossLevel()
        {
            byte BossLevel = 0;
            if (NPC.downedMoonlord)
                BossLevel = 5;
            else if (NPC.downedGolemBoss)
                BossLevel = 4;
            else if (NPC.downedMechBossAny)
                BossLevel = 3;
            else if (Main.hardMode)
                BossLevel = 2;
            else if (NPC.downedBoss3)
                BossLevel = 1;
            return BossLevel;
        }

        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            if (companion == null) return;
            switch(AI_State)
            {
                case 0:
                case 3:
                    Holder.DrawPosition.Y += companion.height - AI_Value;
                    break;
                case 2:
                    Holder.DrawPosition.Y += AI_Value;
                    break;
            }
        }

        public override void ChangeDrawMoment(Companion companion, ref CompanionDrawMomentTypes DrawMomentType)
        {
            switch(AI_State)
            {
                case 0:
                case 3:
                case 2:
                    DrawMomentType = CompanionDrawMomentTypes.AfterTiles;
                    break;
            }
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            
        }
    }
}