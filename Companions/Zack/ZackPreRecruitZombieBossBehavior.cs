using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace terraguardians.Companions.Zack
{
    public class ZackPreRecruitZombieBossBehavior : BehaviorBase
    {
        public override bool AllowDespawning => false;
        uint ZackID { get { return CompanionDB.Zacks; } }
        private bool IsKnownCompanion { get { return PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, ZackID); } }
        private byte BossLevel = 255;
        private int Damage = 5;
        private bool IsBossVersion = true;
        private bool Incapacitated = false;
        private bool DoAllowFinishing = false;
        bool TrickedOnce = false;
        private Companion companion;
        private byte AI_State = 0;
        private int AI_Value = 0;
        private List<Player> CharactersInvolved = new List<Player>();
        private Microsoft.Xna.Framework.Vector2 PullStartPosition = Microsoft.Xna.Framework.Vector2.Zero;
        private float PullTime = 0;
        private const int DialogueLineTime = (int)(3.5f * 60);
        private Vector2 SwordPosition = Vector2.Zero;
        float SwordRotation = 0;
        byte StuckCounter = 0, TileStuckCounter = 0;
        const int PullMaxTime = 45;
        const int ItemWidth = 22, ItemHeight = 96, ItemOriginX = 10, ItemOriginY = 88;
        int SwordSwingTime { get { return Main.expertMode? 33 : 38; }}
        int SwordAttackReactionTime { get { return Main.expertMode ? 15 : 25; } }
        int PosSwordAttackRecoveryTime { get { return Main.expertMode ? 15 : 30; } }
        private static Dictionary<Player, byte> BloodVomitHitDelay = new Dictionary<Player, byte>();

        public static bool BloodVomitCanHit(Player Target)
        {
            if (!BloodVomitHitDelay.ContainsKey(Target))
            {
                BloodVomitHitDelay.Add(Target, 40);
                return true;
            }
            return false;
        }

        public ZackPreRecruitZombieBossBehavior()
        {
            CanBeHurtByNpcs = false;
            IsVisible = false;
            RunCombatBehavior = false;
            AllowSeekingTargets = false;
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
                    Damage = 25;
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
            if (Main.masterMode)
            {
                Damage = (int)(Damage * 2f);
                companion.moveSpeed += 0.4f;
            }
            else if (Main.expertMode)
            {
                Damage = (int)(Damage * 1.5f);
                companion.moveSpeed += 0.2f;
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
            {
                Player[] Keys = BloodVomitHitDelay.Keys.ToArray();
                foreach(Player k in Keys)
                {
                    BloodVomitHitDelay[k]--;
                    if(BloodVomitHitDelay[k] == 0)
                        BloodVomitHitDelay.Remove(k);
                }
            }
            if (AI_State == 0)
            {
                RisingFromTheGround = true;
                IsBossVersion = !IsKnownCompanion;
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
                                    if (System.MathF.Abs(player.Center.X - companion.Center.X) <= 368 && System.MathF.Abs(player.Center.Y - companion.Center.Y) <= 256 && companion.CanHit(player))
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
                                        if (System.MathF.Abs(c.Center.X - companion.Center.X) <= 368 && System.MathF.Abs(c.Center.Y - companion.Center.Y) <= 256 && companion.CanHit(c))
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
            CanBeAttacked = !Incapacitated && !RisingFromTheGround && AI_State != 2 && AI_State != 3 && AI_State != 100;
            bool MoveRight = companion.direction == 1;
            if (companion.Target != null)
            {
                MoveRight = companion.Target.Center.X - companion.Center.X >= 0;
            }
            bool MoveForward = false;
            Player Target = companion.Target is Player ? (Player)companion.Target : null;
            switch(AI_State)
            {
                case 1: //Chase target
                    {
                        if (Main.dayTime || companion.ZoneGraveyard)
                        {
                            AI_State = 2;
                            AI_Value = 0;
                        }
                        else if (!IsBossVersion)
                        {
                            AI_State = 200;
                            AI_Value = 0;
                        }
                        else
                        {
                            if (AI_Value == 0)
                            {
                                companion.LookForTargets();
                                Target = companion.Target is Player ? (Player)companion.Target : null;
                            }
                            companion.WalkMode = true;
                            AI_Value++;
                            if (AI_Value >= 180)
                            {
                                if (Target == null || !Target.active || Target.dead)
                                {
                                    AI_State = 1;
                                    AI_Value = 0;
                                }
                                else
                                {
                                    if (companion.velocity.X == 0)
                                    {
                                        if (Main.expertMode)
                                        {
                                            for(int p = 0; p < 255; p++)
                                            {
                                                if (Main.player[p] != companion && Main.player[p].active && !Main.player[p].dead && PlayerMod.IsEnemy(Main.player[p], companion))
                                                {
                                                    if (Main.player[p].getRect().Intersects(companion.getRect()))
                                                    {
                                                        Pull(Main.player[p]);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        //AI_State = 5;
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
                            if (Main.dayTime || companion.ZoneGraveyard)
                            {
                                WorldMod.RemoveCompanionNPC(companion);
                                return;
                            }
                            else
                            {
                                AI_Value = 0;
                                AI_State = 3;
                            }
                        }
                    }
                    break;
                case 3: //Return from underground
                    {
                        RisingFromTheGround = true;
                    }
                    break;
                case 4: //Pull Player
                case 16: //Pull Player Friendly
                    {
                        if(Target == null || !Target.active || Target.dead)
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
                                Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(Target);
                                if (Mount != null) Mount.ToggleMount(Target, true);
                            }
                            float Percentage = (float)(AI_Value - PullMaxTime) / (int)PullTime;
                            if (Percentage >= 1)
                            {
                                Vector2 NewPosition = new Vector2(
                                    companion.Center.X - Target.width * 0.5f + companion.width * 0.5f * companion.direction,
                                    companion.position.Y - Target.height * 0.25f
                                );
                                DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                                Target.position = NewPosition;
                                Target.velocity = Vector2.Zero;
                                Target.direction = -companion.direction;
                                Target.AddBuff(BuffID.Cursed, 5);
                                if (AI_State == 16)
                                {
                                    //Prank
                                    int NewAITime = (int)(AI_Value - (PullMaxTime + (int)PullTime));
                                    AI_Value++;
                                    bool IsBlue = Target is Companion && (Target as Companion).IsSameID(CompanionDB.Blue);
                                    if (NewAITime == 30)
                                    {
                                        if (Target is Companion && (Target as Companion).IsSameID(CompanionDB.Blue))
                                            companion.SaySomething("*Hello Blue, taking a little night walk?.*");
                                        else
                                            companion.SaySomething("*Hahaha, I can feel your heart racing now.*");
                                    }
                                    else if (NewAITime == 30 + DialogueLineTime)
                                    {
                                        if (Target is Companion && (Target as Companion).IsSameID(CompanionDB.Blue))
                                            (Target as Companion).SaySomething("*That's not funny, Zack. Place me on the ground.*");
                                        else
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
                                    PlayerMod.DoHurt(Target, Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Target.name + " has turned into zombie food."), (int)(Target.statLifeMax2 * 0.2f), companion.direction);
                                    if (Main.expertMode)
                                    {
                                        int HealthRecovered = (int)(companion.statLifeMax2 * 0.05f);
                                        if (companion.statLifeMax2 - companion.statLife > HealthRecovered )
                                        {
                                            HealthRecovered = companion.statLifeMax2 - companion.statLife;
                                        }
                                        companion.statLife += HealthRecovered;
                                        companion.HealEffect(HealthRecovered);
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
                            else
                            {
                                Target.position = PullStartPosition + (companion.Center - PullStartPosition) * Percentage;
                                Target.fallStart = (int)(Target.position.Y * (1f / 16));
                                if (Target.itemAnimation == 0)
                                {
                                    if (Target.velocity.X >= 0)
                                        Target.direction = 1;
                                    else
                                        Target.direction = -1;
                                }
                                AI_Value++;
                            }
                        }
                        else
                        {
                            if (AI_Value == 0 && Target.getRect().Intersects(companion.getRect()))
                            {
                                AI_Value = PullMaxTime;
                            }
                            if (PullStartPosition.X != 0 || PullStartPosition.Y != 0)
                            {
                                PullStartPosition = Vector2.Zero;
                            }
                            if (Target.Center.X < companion.Center.X)
                                companion.direction = -1;
                            else
                                companion.direction = 1;
                            AI_Value++;
                        }
                    }
                    break;
                case 5: //Blood Vomit Attack
                    {
                        if (AI_Value == 0)
                        {
                            companion.LookForTargets();
                            Target = companion.Target is Player ? (Player)companion.Target : null;
                            if (Target != null && companion.velocity.X == 0 && companion.velocity.Y == 0)
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
                        if (AI_Value == 0)
                        {
                            companion.LookForTargets();
                            if (companion.Target != null)
                            {
                                if (companion.Target.Center.X < companion.Center.X)
                                    companion.direction = -1;
                                else
                                    companion.direction = 1;
                            }
                        }
                        float Percentage = Math.Clamp((float)(AI_Value - SwordAttackReactionTime) / SwordSwingTime, 0f, 1f);
                        short Frame = 0;
                        if (Percentage < 0.45f)
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(0);
                        else if (Percentage < 0.65f)
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(1);
                        else
                            Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(2);
                        SwordPosition = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, Frame);
                        SwordRotation = (float)Math.Sin(Percentage * 1.35f) * (260 * Percentage);
                        SwordRotation = MathHelper.ToRadians(-120 + SwordRotation) * companion.direction; //-158
                        if (Percentage > 0 && Percentage < 1)
                        {
                            const int ItemWidth = 22, ItemHeight = 96, ItemOriginX = 10, ItemOriginY = 88;
                            Rectangle WeaponCollision = new Rectangle();
                            WeaponCollision.Width = (int)(ItemHeight * Math.Sin(SwordRotation) + ItemWidth * Math.Cos(SwordRotation));
                            WeaponCollision.Height = (int)(ItemHeight * Math.Cos(SwordRotation) + ItemWidth * Math.Sin(SwordRotation)) * -1;
                            WeaponCollision.X -= (int)((ItemHeight - ItemOriginY) * Math.Sin(SwordRotation) + (ItemWidth - ItemOriginX) * Math.Cos(SwordRotation));
                            WeaponCollision.Y -= (int)((ItemHeight - ItemOriginY) * Math.Cos(SwordRotation) + (ItemWidth - ItemOriginX) * Math.Sin(SwordRotation)) * -1;
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
                            for (int i = 0; i < 255; i++)
                            {
                                if (Main.player[i] == companion || !Main.player[i].active || Main.player[i].dead || Main.player[i].ghost || Main.player[i].immuneTime > 0 || !Main.player[i].getRect().Intersects(WeaponCollision))
                                    continue;
                                double damage = PlayerMod.DoHurt(Main.player[i], Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " was sliced in half by a Zombie Guardian."), SlashDamage, companion.direction);
                                if (Main.expertMode && damage > 0)
                                    Main.player[i].AddBuff(Terraria.ID.BuffID.BrokenArmor, 30 * 60);
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
                        if (AI_Value == 0)
                        {
                            companion.LookForTargets();
                            if (companion.Target != null)
                            {
                                if (companion.Target.Center.X < companion.Center.X)
                                    companion.direction = -1;
                                else
                                    companion.direction = 1;
                            }
                        }
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

                case 100:
                    {
                        if (AI_Value == 0)
                        {
                            companion.LookForTargets();
                            Target = companion.Target is Player ? (Player)companion.Target : null;
                            if (Target == null)
                            {
                                AI_State = 2;
                                AI_Value = 0;
                                companion.statLife = (int)(companion.statLifeMax2 * 0.25f);
                                return;
                            }
                            Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(Target);
                            if (Mount != null)
                                Mount.ToggleMount(Target, true);
                        }
                        Vector2 PosCenter = companion.Bottom;
                        Companion Blue = PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Blue);
                        if(Target is TerraGuardian && (Target as Companion).IsSameID(CompanionDB.Blue))
                            Blue = (Target as Companion);
                        bool TargetIsBlue = Blue != null;
                        if (Blue == null)
                        {
                            string Text = "*The zombie got enraged.*";
                            if (Main.netMode == 0)
                                Main.NewText(Text);
                            Pull(Target);
                            companion.statLife = (int)(companion.statLifeMax2 * 0.25f);
                            return;
                        }
                        if (Target.dead || !Target.active)
                        {
                            AI_State = 3;
                            AI_Value = 0;
                            companion.statLife = (int)(companion.statLifeMax2 * 0.25f);
                            return;
                        }
                        if (AI_Value < 5 || (AI_Value >= 120 + 25 && AI_Value < 120 + 30))
                        {
                            Vector2 GrabPosition = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, 17);
                            GrabPosition.X -= Target.width * 0.5f;
                            GrabPosition.Y -= Target.height * 0.25f;
                            DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                            Target.velocity = Vector2.Zero;
                            Target.position = GrabPosition;
                            Target.fallStart = (int)(Target.position.Y * (1f / 16));
                            if (Target.itemAnimation == 0)
                                Target.direction = -companion.direction;
                        }
                        else if (AI_Value < 120 + 25)
                        {
                            Vector2 GrabPosition = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, 16);
                            GrabPosition.X -= Target.width * 0.5f;
                            GrabPosition.Y -= Target.height * 0.25f;
                            DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                            Target.position = GrabPosition;
                            Target.velocity = Vector2.Zero;
                            Target.fallStart = (int)(Target.position.Y * (1f / 16));
                            if (Target.itemAnimation == 0)
                                Target.direction = -companion.direction;
                        }
                        if (AI_Value == 30)
                        {
                            if (TargetIsBlue)
                            {
                                (Target as Companion).SaySomething("*Zack! Zack! Look at my face! Have you forgotten me?*");
                            }
                            else if (Blue != null)
                            {
                                Blue.SaySomething("*Zack! Don't do that! Look at me! Have you forgotten me?*");
                            }
                        }
                        else if (AI_Value == DialogueLineTime + 30)
                        {
                            companion.SaySomething("*B... Blue..? I feel like.. I've just awoken from a nightmare..*");
                        }
                        else if (AI_Value == DialogueLineTime * 2 + 30)
                        {
                            if (TargetIsBlue)
                            {
                                companion.SaySomething("*What was I doing? I nearly... I'm sorry. Thank you all for bringing me to my senses.*");
                            }
                            else if (Blue != null)
                            {
                                companion.SaySomething("*I think I can think by myself now. I missed you so much Blue.*");
                            }
                        }
                        else if (AI_Value == DialogueLineTime * 3 + 30)
                        {
                            companion.SaySomething("*You helped her find me, Terrarian? I may be able to help you on your quest, as long as I can stay by Blue's side.*");
                        }
                        else if (AI_Value == DialogueLineTime * 4 + 30)
                        {
                            companion.SaySomething("*You can call me Zack, that's my nickname.*");
                            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, companion.ID, companion.ModID);
                            WorldMod.AddCompanionMet(companion.ID, companion.ModID);
                        }
                        AI_Value++;
                    }
                    break;
                case 200:
                    {
                        if (AI_Value == 30)
                        {
                            companion.SaySomething("*Boo!*");
                        }
                        else if (AI_Value == 30 + DialogueLineTime)
                        {
                            companion.SaySomething("*Did I scared you?*");
                        }
                        else if (AI_Value == 30 + DialogueLineTime * 2)
                        {
                            companion.SaySomething("*Sorry for scaring you, but It was really fun.*");
                        }
                        else if (AI_Value == 30 + DialogueLineTime * 3)
                        {
                            companion.SaySomething("*If you ever need me, I'm here.*");
                        }
                        else if (AI_Value == 30 + DialogueLineTime * 4)
                        {
                            WorldMod.AddCompanionMet(companion.ID, companion.ModID);
                            return;
                        }
                        AI_Value++;
                    }
                    break;
            }
            if (MoveForward)
            {
                if(companion.velocity.Y < 0 && companion.oldVelocity.Y == 0)
                    StuckCounter++;
                if (StuckCounter >= 3)
                {
                    StuckCounter = 0;
                    AI_State = 2;
                    AI_Value = 0;
                }
                if (Target != null && !Target.dead && MathF.Abs(Target.Center.X - companion.Center.X) >= 8)
                {
                    if (Target.Center.X < companion.Center.X)
                        companion.direction = -1;
                    else
                        companion.direction = 1;
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
                    if(IsBossVersion)
                        companion.LookForTargets();
                    else
                    {
                        Vector2 Center = companion.Center;
                        float NearestDistance = 500;
                        for(int i = 0; i < 255; i++)
                        {
                            if(Main.player[i] != companion && Main.player[i].active && PlayerMod.IsPlayerCharacter(Main.player[i]) && !Main.player[i].dead)
                            {
                                float Distance = (Main.player[i].Center - Center).Length();
                                if (Distance < NearestDistance)
                                {
                                    companion.Target = Main.player[i];
                                    NearestDistance = Distance;
                                }
                            }
                        }
                    }
                    IsVisible = false;
                    if (companion.Target != null && companion.Target is Player)
                    {
                        Target = (Player)companion.Target;
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
                        if (!IsBossVersion)
                        {
                            for(int i = 0; i < 255; i++)
                            {
                                if(Main.player[i] != companion && PlayerMod.IsPlayerCharacter(Main.player[i]) && Main.player[i].active && !Main.player[i].dead && Main.player[i].getRect().Intersects(companion.getRect()))
                                {
                                    companion.Target = Main.player[i];
                                    AI_State = 16;
                                    break;
                                }
                            }
                        }
                        else if (Target.Hitbox.Intersects(companion.Hitbox))
                        {
                            Pull(Target);
                        }
                    }
                }
            }
        }

        public void Pull(Player victim)
        {
            if (victim == companion)
            {
                return;
            }
            companion.Target = victim;
            AI_State = (byte)(IsBossVersion ? 4 : 16);
            AI_Value = 0;
        }

        public override bool IsHostileTo(Player target)
        {
            return (IsBossVersion && AI_State < 100) || (AI_State == 100 && AI_Value == 0);
        }

        public override bool CanKill(Companion companion)
        {
            Incapacitated = true;
            return DoAllowFinishing;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            switch(AI_State)
            {
                case 4:
                case 16:
                    if (AI_Value >= PullMaxTime + PullTime)
                    {
                        companion.ArmFramesID[0] = companion.ArmFramesID[1] = 15;
                    }
                    else if (AI_Value < 5)
                        companion.ArmFramesID[0] = 14;
                    else if (AI_Value < 10)
                        companion.ArmFramesID[0] = 15;
                    else if (AI_Value < 15)
                        companion.ArmFramesID[0] = 16;
                    else if (AI_Value < 20)
                        companion.ArmFramesID[0] = 17;
                    break;
                case 6:
                    float AnimationPercentage = (float)(AI_Value - SwordAttackReactionTime) / SwordSwingTime;
                    if (AnimationPercentage > 1f)
                        AnimationPercentage = 1f;
                    if (AnimationPercentage < 0)
                        AnimationPercentage = 0f;
                    short Frame = 0;
                    if (AnimationPercentage < 0.45f)
                    {
                        Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(0);
                    }
                    else if (AnimationPercentage < 0.65f)
                    {
                        Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(1);
                    }
                    else
                    {
                        Frame = companion.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrame(2);
                    }
                    companion.BodyFrameID = companion.ArmFramesID[0] = companion.ArmFramesID[1] = Frame;
                    break;
                case 8:
                    if(companion.velocity.X == 0 && AI_Value > 90)
                    {
                        companion.BodyFrameID = companion.ArmFramesID[0] = companion.ArmFramesID[1] = companion.Base.GetAnimation(AnimationTypes.RevivingFrames).GetFrame(0);
                    }
                    break;
                case 100:
                    if (AI_Value < 5 || (AI_Value >= 120 + 25 && AI_Value < 120 + 30))
                    {
                        companion.ArmFramesID[0] = companion.ArmFramesID[1] = 17;
                    }
                    else if (AI_Value < 120 + 25)
                    {
                        companion.ArmFramesID[0] = companion.ArmFramesID[1] = 16;
                    }
                    break;
            }
            if(Incapacitated && companion.velocity.Y == 0)
            {
                short frame = companion.Base.GetAnimation(AnimationTypes.DownedFrames).GetFrame(0);
                companion.BodyFrameID = frame;
                for (int i = 0; i < companion.ArmFramesID.Length; i++)
                    companion.ArmFramesID[i] = frame;
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            MessageDialogue m = new MessageDialogue();
            if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Blue))
            {
                m.ChangeMessage("*[nickname], let's try talking to him.*", PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Blue));
            }
            else if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Blue))
            {
                m.ChangeMessage("*The zombie seems less violent due to the presence of something in this world.*");
            }
            else
            {
                m.ChangeMessage("*It turned less violent. Maybe I could try talking to it?*");
            }
            if (!TrickedOnce) m.AddOption("Try talking.", OnTryTalking);
            m.AddOption("Finish him.", OnFinishingHim);
            return m;
        }

        public void OnTryTalking()
        {
            TrickedOnce = true;
            AI_State = 100;
            AI_Value = 0;
            Incapacitated = false;
            Dialogue.EndDialogue();
        }

        public void OnFinishingHim()
        {
            DoAllowFinishing = true;
            companion.statLife = 1;
            PlayerMod.ForceKillPlayer(companion, MainMod.GetLocalPlayer.name + " has ended the Zombie TerraGuardian's misery.", false);
            //companion.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(MainMod.GetLocalPlayer.name + " has ended the Zombie TerraGuardian's misery."), 9999, 0);
            Dialogue.EndDialogue();
            if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Blue))
            {
                PlayerMod.PlayerGetSummonedCompanion(MainMod.GetLocalPlayer, CompanionDB.Blue).SaySomething("*Zack... Is there any way of bringing him back to his sense?*");
            }
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

        public override void CompanionDrawLayerSetup(Companion companion, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (!Incapacitated && IsDrawingFrontLayer)
            {
                if(AI_State == 4)
                    DrawChain(companion, DrawDatas);
                if (AI_State == 6)
                {
                    Vector2 ItemOrigin = new Vector2(ItemOriginX, ItemOriginY);
                    if (companion.direction < 0)
                        ItemOrigin.X = ItemWidth - ItemOrigin.X;
                    DrawData dd = new DrawData(MainMod.IronSwordTexture.Value, SwordPosition - Main.screenPosition, null, Holder.DrawColor, SwordRotation, ItemOrigin, 1f, (companion.direction < 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally : Microsoft.Xna.Framework.Graphics.SpriteEffects.None), 0);
                    DrawDatas.Insert(0, dd);
                }
            }
        }

        private void DrawChain(Companion companion, List<DrawData> DrawDatas)
        {
            Vector2 ChainStartPosition = companion.Center, ChainEndPosition = ChainStartPosition;
            ChainStartPosition.X -= 8 * companion.direction;
            ChainStartPosition.Y -= 8;
            float Percentage = (float)AI_Value / PullMaxTime;
            if (Percentage >= 1f)
                return;
            else
                ChainEndPosition.Y += Bezier(Percentage, 0f, -60f, 0f);
            Player Target = companion.Target as Player;
            ChainEndPosition += (Target.Center - companion.Center) * Percentage;
            float DifX = ChainStartPosition.X - ChainEndPosition.X, DifY = ChainStartPosition.Y - ChainEndPosition.Y;
            bool DrawMoreChain = true;
            float Rotation = (float)Math.Atan2(DifY, DifX) - 1.57f;
            Texture2D texture = Terraria.GameContent.TextureAssets.Chain12.Value;
            Color ChainColor = Color.DarkRed;
            byte MaxChains = 25;
            while (DrawMoreChain && MaxChains > 0)
            {
                float sqrt = (float)Math.Sqrt(DifX * DifX + DifY * DifY);
                if (sqrt < 40)
                    DrawMoreChain = false;
                else
                {
                    sqrt = (float)texture.Height / sqrt;
                    DifX *= sqrt;
                    DifY *= sqrt;
                    ChainEndPosition.X += DifX;
                    ChainEndPosition.Y += DifY;
                    DifX = ChainStartPosition.X - ChainEndPosition.X;
                    DifY = ChainStartPosition.Y - ChainEndPosition.Y;
                    Microsoft.Xna.Framework.Color color = Lighting.GetColor((int)ChainEndPosition.X / 16, (int)ChainEndPosition.Y / 16, ChainColor);
                    DrawDatas.Add(new DrawData(texture, ChainEndPosition - Main.screenPosition, null, color, Rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0));
                }
                MaxChains--;
            }
        }

        public static float Bezier(float t, float a, float b, float c)
        {
            float ab = MathHelper.Lerp(a, b, t);
            float bc = MathHelper.Lerp(b, c, t);
            return MathHelper.Lerp(ab, bc, t);
        }
    }
}