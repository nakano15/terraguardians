using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class PlayerMod : ModPlayer
    {
        private Companion[] SummonedCompanions = new Companion[MainMod.MaxCompanionFollowers];
        private uint[] SummonedCompanionKey = new uint[MainMod.MaxCompanionFollowers];
        public Companion[] GetSummonedCompanions { get{ return SummonedCompanions; } }
        public uint[] GetSummonedCompanionKeys { get { return SummonedCompanionKey; } }
        private SortedDictionary<uint, CompanionData> MyCompanions = new SortedDictionary<uint, CompanionData>();
        public uint[] GetCompanionDataKeys{ get{ return MyCompanions.Keys.ToArray(); } }
        private Companion CompanionMountedOnMe = null, MountedOnCompanion = null;
        public Companion GetCompanionMountedOnMe { get { return CompanionMountedOnMe; } internal set { CompanionMountedOnMe = value; } }
        public Companion GetMountedOnCompanion { get { return MountedOnCompanion; } internal set { MountedOnCompanion = value; } }
        private static bool DrawHoldingCompanionArm = false;
        private byte ActiveRequests = 0;
        private Dictionary<Player, int> CharacterHitDelays = new Dictionary<Player, int>();

        public override bool IsCloneable => false;
        protected override bool CloneNewInstances => false;
        public Player TalkPlayer { get; internal set; }
        public float FollowBehindDistancing = 0, FollowAheadDistancing = 0;

        public PlayerMod()
        {
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                SummonedCompanions[i] = null;
                SummonedCompanionKey[i] = 0;
            }
        }

        public static double DoHurt(Player player, PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp = false, bool quiet = false, bool Crit = false, int cooldownCounter = -1)
        {
            if(player is Companion)
            {
                Companion c = (Companion)player;
                if (c.IsLocalCompanion)
                {
                    Main.myPlayer = c.whoAmI;
                    double Result = player.Hurt(damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter);
                    Main.myPlayer = MainMod.MyPlayerBackup;
                    return Result;
                }
            }
            return player.Hurt(damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter);
        }

        public static int PlayerGetTerrarianCompanionsMet(Player player)
        {
            int Count = 0;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(Companion c in pm.SummonedCompanions)
            {
                if(c != null && !c.GetGroup.IsTerraGuardian) Count++;
            }
            return Count;
        }

        public static int PlayerGetTerraGuardianCompanionsMet(Player player)
        {
            int Count = 0;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(Companion c in pm.SummonedCompanions)
            {
                if(c != null && c.GetGroup.IsTerraGuardian) Count++;
            }
            return Count;
        }

        public static Companion[] PlayerGetSummonedCompanions(Player player)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            List<Companion> companions = new List<Companion>();
            foreach(Companion c in pm.SummonedCompanions)
            {
                if(c != null) companions.Add(c);
            }
            return companions.ToArray();
        }

        public static Companion PlayerGetSummonedCompanion(Player player, uint ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.GetModName;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            List<Companion> companions = new List<Companion>();
            foreach(Companion c in pm.SummonedCompanions)
            {
                if (c != null && c.IsSameID(ID, ModID)) return c;
            }
            return null;
        }

        public static bool PlayerCanTakeNewRequest(Player player)
        {
            return player.GetModPlayer<PlayerMod>().ActiveRequests < RequestData.MaxActiveRequests;
        }

        public static Companion PlayerGetMountedOnCompanion(Player player)
        {
            return player.GetModPlayer<PlayerMod>().GetMountedOnCompanion;
        }

        public static Companion PlayerGetCompanionMountedOnMe(Player player)
        {
            return player.GetModPlayer<PlayerMod>().GetCompanionMountedOnMe;
        }

        public static bool IsPlayerCharacter(Player player)
        {
            return !(player is Companion) || ((Companion)player).IsPlayerCharacter;
        }

        public uint GetCompanionDataIndex(uint ID, string ModID = "")
        {
            foreach(uint k in MyCompanions.Keys)
            {
                if(MyCompanions[k].IsSameID(ID, ModID)) return k;
            }
            return 0;
        }

        public static CompanionData PlayerGetCompanionData(Player player, uint ID, string ModID = "")
        {
            return player.GetModPlayer<PlayerMod>().GetCompanionData(ID, ModID);
        }

        public static CompanionData PlayerGetCompanionDataByIndex(Player player, uint Index)
        {
            return player.GetModPlayer<PlayerMod>().GetCompanionDataByIndex(Index);
        }

        public CompanionData GetCompanionData(uint ID, string ModID = "")
        {
            return GetCompanionDataByIndex(GetCompanionDataIndex(ID, ModID));
        }

        public CompanionData GetCompanionDataByIndex(uint Index)
        {
            if(MyCompanions.ContainsKey(Index)) return MyCompanions[Index];
            return null;
        }

        public static void UpdatePlayerMobKill(Player player, NPC npc)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(uint key in pm.GetCompanionDataKeys)
            {
                pm.GetCompanionDataByIndex(key).GetRequest.OnKillNpc(npc);
            }
        }

        internal static bool PlayerTalkWith(Player Subject, Player Target)
        {
            PlayerMod sub = Subject.GetModPlayer<PlayerMod>();
            PlayerMod tar = Target.GetModPlayer<PlayerMod>();
            if(sub.TalkPlayer != null)
            {
                EndDialogue(Subject);
            }
            sub.TalkPlayer = Target;
            tar.TalkPlayer = Subject;
            return true;
        }

        public static void EndDialogue(Player Subject)
        {
            PlayerMod pm = Subject.GetModPlayer<PlayerMod>();
            if(pm.TalkPlayer != null)
            {
                PlayerMod Target = pm.TalkPlayer.GetModPlayer<PlayerMod>();
                Target.TalkPlayer = null;
                pm.TalkPlayer = null;
                if(pm.Player == Main.LocalPlayer || Target.Player == Main.LocalPlayer)
                    Dialogue.EndDialogue();
            }
        }

        public override void ResetEffects()
        {
            Player MountedCompanion = null;
            if (MountedOnCompanion != null)
            {
                MountedCompanion = MountedOnCompanion;
            }
            else if ((Player is Companion) && (Player as Companion).IsMountedOnSomething)
            {
                MountedCompanion = (Player as Companion).GetCharacterMountedOnMe;
            }
            if (MountedCompanion != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    int ID = BuffID.Suffocation;
                    switch(i)
                    {
                        case 1:
                            ID = BuffID.Burning;
                            break;
                    }
                    /*if (MountedCompanion.HasBuff(ID))
                        Player.AddBuff(ID, 5);
                    else
                    {
                        Player.ClearBuff(ID);
                        Player.buffImmune[ID] = true;
                    }*/
                    Player.buffImmune[ID] = true;
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            if (MountedOnCompanion != null)
            {
                if (Player.tongued)
                {
                    MountedOnCompanion.ToggleMount(Player, true);
                }
            }
        }

        public override void PreUpdate()
        {
            if(Main.netMode == 0)
                Player.hostile = true;
            ActiveRequests = 0;
            foreach(CompanionData cd in MyCompanions.Values)
            {
                cd.Update(Player);
                if(cd.GetRequest.IsActive)
                    ActiveRequests++;
            }
        }

        public override void OnRespawn(Player player)
        {
            if(player is Companion)
            {
                Companion c = (Companion) player;
                if(!WorldMod.HasMetCompanion(c.Data) && !WorldMod.IsStarterCompanion(c))
                {
                    if (!WorldMod.RemoveCompanionNPC(c))
                        MainMod.DespawnCompanion(c.GetWhoAmID);
                }
                else
                {
                    ((Companion)player).OnSpawnOrTeleport();
                }
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if(IsPlayerCharacter(player)) //Character spawns, but can't be seen on the world.
            {
                MainMod.MyPlayerBackup = Main.myPlayer;
                for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    uint MyKey = SummonedCompanionKey[i];
                    SummonedCompanionKey[i] = 0;
                    if(MyKey > 0)
                    {
                        CallCompanionByIndex(MyKey, true);
                    }
                }
                MainMod.CheckForFreebies(this);
                
                const uint CompanionID = CompanionDB.Bree;
                if (!HasCompanion(CompanionID))
                    AddCompanion(CompanionID);
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if(Player is Companion)
            {
                return (Player as Companion).GetGoverningBehavior().CanBeHurtByNpcs;
            }
            return true;
        }

        public static bool IsEnemy(Player player, Player otherPlayer)
        {
            if(player is Companion)
            {
                if ((player as Companion).IsHostileTo(otherPlayer))
                    return true;
                if ((player as Companion).Owner == otherPlayer) return false;
            }
            if(otherPlayer is Companion)
            {
                if ((otherPlayer as Companion).IsHostileTo(player))
                    return true;
                if ((otherPlayer as Companion).Owner == player) return false;
            }
            return false; //player.hostile && otherPlayer.hostile && (player.team == 0 || otherPlayer.team == 0 || player.team != otherPlayer.team);
        }

        public static bool CanHitHostile(Player player, Player otherPlayer)
        {
            return IsEnemy(player, otherPlayer);
        }

        public override bool CanHitPvp(Item item, Player target)
        {
            return CanHitHostile(Player, target);
        }

        public override bool CanHitPvpWithProj(Projectile proj, Player target)
        {
            return CanHitHostile(Player, target);
        }

        public static bool IsCompanionLeader(Player player, Companion companion)
        {
            return player.GetModPlayer<PlayerMod>().SummonedCompanions[0] == companion;
        }

        public static bool PlayerAddCompanion(Player player, Companion companion)
        {
            return PlayerAddCompanion(player, companion.ID, companion.ModID);
        }

        public static bool PlayerAddCompanion(Player player, uint CompanionID, string CompanionModID = "")
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.HasCompanion(CompanionID, CompanionModID))
            {
                pm.AddCompanion(CompanionID, CompanionModID);
                return true;
            }
            return false;
        }

        public bool AddCompanion(uint CompanionID, string CompanionModID = "", bool IsStarter = false)
        {
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            uint NewIndex = 1;
            foreach(uint Key in MyCompanions.Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID)) return false;
                if(Key == NewIndex)
                    NewIndex++;
            }
            CompanionData data = new CompanionData(CompanionID, CompanionModID, NewIndex);
            data.IsStarter = IsStarter;
            MyCompanions.Add(NewIndex, data);
            return true;
        }

        public static bool PlayerHasCompanion(Player player, Companion companion)
        {
            return player.GetModPlayer<PlayerMod>().HasCompanion(companion.ID, companion.ModID);
        }

        public static bool PlayerHasCompanion(Player player, CompanionID ID)
        {
            return player.GetModPlayer<PlayerMod>().HasCompanion(ID.ID, ID.ModID);
        }

        public static bool PlayerHasCompanion(Player player, uint CompanionID, string CompanionModID = "")
        {
            return player.GetModPlayer<PlayerMod>().HasCompanion(CompanionID, CompanionModID);
        }

        public bool HasCompanion(uint CompanionID, string CompanionModID = "")
        {
            foreach(uint Key in MyCompanions.Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID)) return true;
            }
            return false;
        }

        public static bool PlayerHasEmptyFollowerSlot(Player player)
        {
            return player.GetModPlayer<PlayerMod>().HasEmptyFollowerSlot();
        }

        public bool HasEmptyFollowerSlot()
        {
            for(byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if(SummonedCompanionKey[i] == 0) return true;
            }
            return false;
        }

        public static bool PlayerCallCompanion(Player player, uint ID, string ModID = "", bool TeleportIfExists = false)
        {
            return player.GetModPlayer<PlayerMod>().CallCompanion(ID, ModID, TeleportIfExists);
        }

        public bool CallCompanion(uint ID, string ModID = "", bool TeleportIfExists = false)
        {
            return CallCompanionByIndex(GetCompanionDataIndex(ID, ModID), TeleportIfExists);
        }

        public static bool PlayerCallCompanionByIndex(Player player, uint Index, bool TeleportIfExists = false)
        {
            return player.GetModPlayer<PlayerMod>().CallCompanionByIndex(Index, TeleportIfExists);
        }

        public bool CallCompanionByIndex(uint Index, bool TeleportIfExists = false)
        {
            if(Player is Companion || Index == 0 || !MyCompanions.ContainsKey(Index) || MyCompanions[Index].Base.IsInvalidCompanion) return false;
            foreach(uint i in SummonedCompanionKey)
            {
                if (i == Index) return false;
            }
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if (SummonedCompanionKey[i] == 0)
                {
                    CompanionData data = GetCompanionDataByIndex(Index);
                    bool SpawnCompanion = true;
                    foreach(Companion c in WorldMod.CompanionNPCs)
                    {
                        if(c.IsSameID(data.ID, data.ModID) && (c.Index == 0 || c.Index == Index) && c.Owner == null)
                        {
                            c.Data = data;
                            c.InitializeCompanion();
                            c.Owner = Player;
                            SpawnCompanion = false;
                            SummonedCompanions[i] = c;
                            break;
                        }
                    }
                    if (SpawnCompanion)
                        SummonedCompanions[i] = MainMod.SpawnCompanion(Player.Bottom, data, Player);
                    else if(TeleportIfExists)
                        SummonedCompanions[i].Teleport(Player.Bottom);
                    SummonedCompanionKey[i] = Index;
                    WorldMod.AddCompanionMet(data);
                    return true;
                }
            }
            return false;
        }

        public static bool PlayerDismissCompanionByIndex(Player player, uint Index, bool Despawn = true)
        {
            return player.GetModPlayer<PlayerMod>().DismissCompanionByIndex(Index, Despawn);
        }

        public static bool PlayerDismissCompanion(Player player, uint ID, string ModID = "", bool Despawn = true)
        {
            return player.GetModPlayer<PlayerMod>().DismissCompanion(ID, ModID, Despawn);
        }

        public bool DismissCompanion(uint ID, string ModID = "", bool Despawn = true)
        {
            if (ModID == "") ModID = MainMod.GetModName;
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if (SummonedCompanionKey[i] > 0 && SummonedCompanions[i].IsSameID(ID, ModID))
                {
                    return DismissCompanionByIndex(SummonedCompanionKey[i], Despawn);
                }
            }
            return false;
        }

        public bool DismissCompanionByIndex(uint Index, bool Despawn = true)
        {
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if(SummonedCompanionKey[i] == Index)
                {
                    if(SummonedCompanions[i].IsMountedOnSomething)
                        SummonedCompanions[i].ToggleMount(SummonedCompanions[i].GetCharacterMountedOnMe, true);
                    if(Despawn && SummonedCompanions[i].GetTownNpcState == null)
                    {
                        MainMod.DespawnCompanion(SummonedCompanions[i].GetWhoAmID);
                    }
                    else
                    {
                        if(!WorldMod.HasCompanionNPCSpawnedWhoAmID(SummonedCompanionKey[i]))
                            WorldMod.SetCompanionTownNpc(SummonedCompanions[i]);
                        SummonedCompanions[i].Owner = null;
                    }
                    SummonedCompanions[i] = null;
                    SummonedCompanionKey[i] = 0;
                    ArrangeFollowerCompanionsOrder();
                    return true;
                }
            }
            return false;
        }

        private void ArrangeFollowerCompanionsOrder()
        {
            for(int i = 0; i < SummonedCompanions.Length; i++)
            {
                if(SummonedCompanionKey[i] == 0)
                {
                    for(int j = i + 1; j < SummonedCompanions.Length; j++)
                    {
                        if(SummonedCompanionKey[j] > 0)
                        {
                            SummonedCompanionKey[i] = SummonedCompanionKey[j];
                            SummonedCompanionKey[j] = 0;
                            SummonedCompanions[i] = SummonedCompanions[j];
                            SummonedCompanions[j] = null;
                            break;
                        }
                    }
                }
            }
        }

        public static bool PlayerHasCompanionSummonedByIndex(Player player, uint Index)
        {
            return player.GetModPlayer<PlayerMod>().HasCompanionSummonedByIndex(Index);
        }

        public static bool PlayerHasCompanionSummoned(Player player, uint ID, string ModID = "")
        {
            return player.GetModPlayer<PlayerMod>().HasCompanionSummoned(ID, ModID);
        }

        public bool HasCompanionSummoned(uint ID, string ModID = "")
        {
            return HasCompanionSummonedByIndex(GetCompanionDataIndex(ID, ModID));
        }

        public bool HasCompanionSummonedByIndex(uint Index)
        {
            if(Index == 0) return false;
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if(SummonedCompanionKey[i] == Index)
                {
                    return true;
                }
            }
            return false;
        }

        public static void DrawPlayerHead(Player player, Vector2 Position, float Scale = 1f, float MaxDimension = 0)
        {
            DrawPlayerHead(player, Position, player.direction == -1, Scale, MaxDimension);
        }

        public static void DrawPlayerHead(Player player, Vector2 Position, bool FacingLeft, float Scale = 1f, float MaxDimension = 0)
        {
            if(player is Companion && !((Companion)player).DrawCompanionHead(Position, FacingLeft))
                return;
            float DimX = player.width * 0.5f;
            if(MaxDimension > 0)
            {
                if(DimX * Scale > MaxDimension)
                {
                    Scale *= MaxDimension / (DimX * Scale);
                }
            }
            Position.X -= DimX * Scale;
            Position.Y -= 8 * Scale;
            int LastDirection = player.direction;
            player.direction = FacingLeft ? -1 : 1;
            Main.PlayerRenderer.DrawPlayerHead(Main.Camera, player, Position, scale: Scale);
            player.direction = LastDirection;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if(Player is Companion)
            {
                playSound = false;
                if(!(Player as Companion).GetGoverningBehavior().CanBeAttacked)
                    return false;
                Companion c = (Companion)Player;
                if(Main.rand.NextFloat() * 100 < c.DodgeRate)
                {
                    CombatText.NewText(c.getRect(), Color.Silver, "Dodged");
                    if (!c.noKnockback)
                    {
                        c.velocity.X = 4.5f * hitDirection;
                        c.velocity.Y = - 3.5f;
                    }
                    c.AddSkillProgress((float)damage * 4, CompanionSkillContainer.AcrobaticsID);
                    c.immuneTime = pvp ? 8 : c.longInvince ? 80 : 40;
                    c.immune = true;
                    return false;
                }
                if(Main.rand.NextFloat() * 100 < c.BlockRate)
                {
                    CombatText.NewText(c.getRect(), Color.Silver, "Blocked");
                    c.AddSkillProgress((float)damage * 4, CompanionSkillContainer.EnduranceID);
                    c.immuneTime = pvp ? 8 : c.longInvince ? 80 : 40;
                    c.immune = true;
                    return false;
                }
                damage = (int)(damage * (1f - c.DefenseRate));
            }
            return true;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if(!Player.dead && Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.HurtSound, Player.position);
                if (damage > 0)(Player as Companion).AddSkillProgress((float)damage * 2, CompanionSkillContainer.EnduranceID);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player is Companion) return (Player as Companion).GetGoverningBehavior().CanKill(Player as Companion);
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if(Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.DeathSound, Player.position);
            }
            if(Player is TerraGuardian)
            {
                ((TerraGuardian)Player).OnDeath();
            }
        }

        public override void PostUpdate()
        {
            if(TalkPlayer != null)
            {
                if(TalkPlayer == Main.LocalPlayer && System.Math.Abs(TalkPlayer.Center.X - Player.Center.X) > 52 + (TalkPlayer.width + Player.width) * 0.5f)
                {
                    Dialogue.EndDialogue();
                }
            }
            FollowBehindDistancing = 0;
            FollowAheadDistancing = 0;
            if(!(Player is Companion))
            {
                UpdateMountedScripts();
                UpdateSittingOffset();
            }
        }

        public void UpdateSittingOffset()
        {
            DrawHoldingCompanionArm = false;
            if(!Player.sitting.isSitting && !Player.sleeping.isSleeping) return;
            Point TileCenter = (Player.Bottom - Vector2.UnitY * 2).ToTileCoordinates();
            Tile tile = Main.tile[TileCenter.X, TileCenter.Y];
            sbyte Direction = 0;
            bool IsThroneOrBench = false;
            float ExtraOffsetX = 0;
            if (tile != null)
            {
                switch(tile.TileType)
                {
                    case TileID.Benches:
                    case TileID.Thrones:
                        IsThroneOrBench = true;
                        int FramesY = tile.TileType == TileID.Thrones ? 4 : 2;
                        int FrameX = (int)((tile.TileFrameX * (1f / 18)) % 3);
                        Direction = (sbyte)(Direction - 1);
                        ExtraOffsetX = 16f;
                        TileCenter.X += 1 - FrameX;
                        TileCenter.Y += (int)((FramesY - 1) - (tile.TileFrameY * (1f / 18)) % FramesY);
                        break;
                }
            }
            if(Direction == 0)
            {
                Direction = (sbyte)Player.direction;
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                int FurnitureX = c.GetFurnitureX;
                if(c.sleeping.isSleeping)
                {
                    FurnitureX += c.direction;
                }
                if(c is TerraGuardian && c.UsingFurniture && FurnitureX == TileCenter.X && c.GetFurnitureY == TileCenter.Y)
                {
                    if (c.Base.MountStyle == MountStyles.PlayerMountsOnCompanion)
                    {
                        TerraGuardian tg = (TerraGuardian)c;
                        Vector2 Offset;
                        if(Player.sitting.isSitting)
                        {
                            Offset = c.GetAnimationPosition(AnimationPositions.PlayerSittingOffset, tg.BodyFrameID, AlsoTakePosition: false, DiscountCharacterDimension: false, DiscountDirections: false, ConvertToCharacterPosition: false);
                        }
                        else
                            Offset = c.GetAnimationPosition(AnimationPositions.PlayerSleepingOffset, tg.BodyFrameID, AlsoTakePosition: false, DiscountCharacterDimension: false, DiscountDirections: false, ConvertToCharacterPosition: false);
                        //Offset.X *= Direction;
                        if(Player.sitting.isSitting || (Offset.X > 0 && Offset.Y > 0))
                        {
                            Offset.X += ExtraOffsetX - (ExtraOffsetX * (1 - c.Scale));
                            Offset.X += 4;
                            if(IsThroneOrBench)
                                Offset.Y += 24 - (24 * (1 - c.Scale));
                            else
                                Offset.Y += 4;
                        }
                        if(Player.sitting.isSitting)
                            Player.sitting.offsetForSeat += Offset;
                        else
                        {
                            Player.sleeping.visualOffsetOfBedBase += Offset;
                        }
                    }
                    else if(c.sitting.isSitting && c.Base.MountStyle == MountStyles.CompanionRidesPlayer)
                    {
                        DrawHoldingCompanionArm = true;
                    }
                    break;
                }
            }
        }

        public void UpdateMountedScripts()
        {
            if(MountedOnCompanion == null || !(MountedOnCompanion is TerraGuardian))
                return;
            TerraGuardian guardian = (TerraGuardian)MountedOnCompanion;
            if(guardian.dead)
            {
                guardian.ToggleMount(Player, true);
                return;
            }
            if(guardian.sleeping.isSleeping)
            {
                if(!Player.sleeping.isSleeping)
                {
                    //Player.Bottom = guardian.Bottom - Vector2.UnitY * 2;
                    int fx = guardian.GetFurnitureX, fy = guardian.GetFurnitureY; //Workaround, kinda bugs first time tho
                    //guardian.LeaveFurniture();
                    Player.sleeping.StartSleeping(Player, fx, fy);
                    //guardian.UseFurniture(fx, fy);
                }
                return;
            }
            if(guardian.sitting.isSitting)
            {
                if (!Player.sitting.isSitting)
                {
                    //Player.Bottom = guardian.Bottom - Vector2.UnitY * 2;
                    int fx = guardian.GetFurnitureX, fy = guardian.GetFurnitureY;
                    //guardian.LeaveFurniture();
                    Player.sitting.SitDown(Player, fx, fy);
                    //guardian.UseFurniture(fx, fy);
                }
                return;
            }
            if (Player.mount.Active)
                Player.mount.Dismount(Player);
            Player.velocity = Vector2.Zero;
            Player.fullRotation = guardian.fullRotation;
            Player.position = guardian.GetMountShoulderPosition + guardian.velocity;
            Player.position.X -= Player.width * 0.5f;
            Player.position.Y -= Player.height * 0.5f + 8 - guardian.gfxOffY;
            Player.gfxOffY = 0;
            Player.itemLocation += guardian.velocity;
            Player.fallStart = Player.fallStart2 = (int)(Player.position.Y * (1f / 16));
            if (Player.itemAnimation == 0 && Player.direction != guardian.direction)
                Player.direction = guardian.direction;
            if (Player.stealth > 0 && guardian.velocity.X * guardian.direction > 0.1f)
            {
                Player.stealth += 0.2f;
                if (Player.stealth > 1) Player.stealth = 1f;
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if(MountedOnCompanion != null && !(Player is TerraGuardian) && !Player.sitting.isSitting && !Player.sleeping.isSleeping)
            {
                Player.legFrame.Y = Player.legFrame.Height * 6;
                Player.legFrameCounter = Player.bodyFrameCounter = Player.headFrameCounter =  0;
                if (Player.itemAnimation == 0)
                {
                    Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                }
                Player.headFrame.Y = Player.bodyFrame.Y;
            }
            if(DrawHoldingCompanionArm && Player.itemAnimation == 0)
            {
                Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
            }
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("LastCompanionsSaveVersion", MainMod.ModVersion);
            uint[] Keys = MyCompanions.Keys.ToArray();
            tag.Add("TotalCompanions", Keys.Length);
            for(int k = 0; k < Keys.Length; k++)
            {
                uint Key = Keys[k];
                tag.Add("CompanionKey_" + k, Key);
                MyCompanions[Key].Save(tag, Key);
            }
            tag.Add("LastSummonedCompanionsCount", MainMod.MaxCompanionFollowers);
            for(int i = 0; i < SummonedCompanions.Length; i++)
                tag.Add("FollowerIndex_" + i, SummonedCompanionKey[i]);
        }

        public override void LoadData(TagCompound tag)
        {
            MyCompanions.Clear();
            if(!tag.ContainsKey("LastCompanionsSaveVersion")) return;
            uint LastCompanionVersion = tag.Get<uint>("LastCompanionsSaveVersion");
            int TotalCompanions = tag.GetInt("TotalCompanions");
            for (int k = 0; k < TotalCompanions; k++)
            {
                uint Key = tag.Get<uint>("CompanionKey_" + k);
                CompanionData data = new CompanionData(Index: Key);
                data.Load(tag, Key, LastCompanionVersion);
                MyCompanions.Add(Key, data);
            }
            int TotalFollowers = tag.GetInt("LastSummonedCompanionsCount");
            for(int i = 0; i < TotalFollowers; i++)
                SummonedCompanionKey[i] = tag.Get<uint>("FollowerIndex_" + i);
        }

        public override void FrameEffects()
        {
            if(MountedOnCompanion != null && !(Player is TerraGuardian))
            {
                Player.velocity = Vector2.Zero;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (MountedOnCompanion != null)
            {
                Main.screenPosition = new Vector2(MountedOnCompanion.Center.X - Main.screenWidth * 0.5f, Player.Center.Y - Main.screenHeight * 0.5f);
            }
        }

        //Called before melee hit
        public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
        {
            Main.myPlayer = target.whoAmI; //Mask this character as the actual player, so the scripts for hit play for them.
        }

        //Called after melee hit
        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            Main.myPlayer = MainMod.MyPlayerBackup; //Reverts above mask.
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                c.AddSkillProgress(damage, item.DamageType is MeleeDamageClass ? CompanionSkillContainer.StrengthID : CompanionSkillContainer.LeadershipID);
                if (crit)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                c.AddSkillProgress(damage, item.DamageType is MeleeDamageClass ? CompanionSkillContainer.StrengthID : CompanionSkillContainer.LeadershipID);
                if (crit)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                if(proj.DamageType is MeleeDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.StrengthID);
                else if(proj.DamageType is RangedDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MarksmanshipID);
                else if(proj.DamageType is MagicDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MysticismID);
                else if(proj.DamageType is SummonDamageClass || proj.DamageType is SummonMeleeSpeedDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LeadershipID);
                if (crit)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
            }
        }

        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            Main.myPlayer = target.whoAmI; 
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            Main.myPlayer = MainMod.MyPlayerBackup;
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                if(proj.DamageType is MeleeDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.StrengthID);
                else if(proj.DamageType is RangedDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MarksmanshipID);
                else if(proj.DamageType is MagicDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MysticismID);
                else if(proj.DamageType is SummonDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LeadershipID);
                if (crit)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
            }
        }

        public override bool PreItemCheck()
        {
            if (!(Player is Companion)) SystemMod.BackupAndPlaceCompanionsOnPlayerArray();
            return base.PreItemCheck();
        }

        public override void PostItemCheck()
        {
            if (!(Player is Companion)) SystemMod.RestoreBackedUpPlayers();
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            foreach(Companion c in SummonedCompanions)
            {
                if (c != null)
                {
                    price += c.statLifeMax2 - c.statLife;
                    for (int b = 0; b < c.buffType.Length; b++)
                    {
                        if (Main.debuff[c.buffType[b]] && c.buffTime[b] > 0 && 
                            !BuffID.Sets.NurseCannotRemoveDebuff[c.buffType[b]])
                        {
                            price += 50;
                        }
                    }
                }
            }
        }

        public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
        {
            bool CanHeal = true;
            bool CompanionNeedsHealing = false;
            foreach(Companion c in SummonedCompanions)
            {
                if (c != null && c.statLife < c.statLifeMax2)
                {
                    CanHeal = true;
                    CompanionNeedsHealing = true;
                    break;
                }
            }
            if (Player.statLife == Player.statLifeMax2 && CompanionNeedsHealing)
            {
                switch(Main.rand.Next(3))
                {
                    default:
                        chatText = "I think I can try stitching your companions.";
                        break;
                    case 1:
                        chatText = "Yes, I can try fixing your friends, as long as you pay me to.";
                        break;
                    case 2:
                        chatText = "I have no idea why you're whole while your friends aren't. Have you been letting them fighting for you? At least pay their fixing.";
                        break;
                }
            }
            return CanHeal;
        }

        public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
        {
            foreach(Companion c in SummonedCompanions)
            {
                if (c != null)
                {
                    int HealthRestored = c.statLifeMax2 - c.statLife;
                    if (HealthRestored > 0)
                    {
                        c.Heal(HealthRestored);
                    }
                    for (int b = 0; b < c.buffType.Length; b++)
                    {
                        if (Main.debuff[c.buffType[b]] && c.buffTime[b] > 0 && 
                            !BuffID.Sets.NurseCannotRemoveDebuff[c.buffType[b]])
                        {
                            c.DelBuff(b);
                        }
                    }
                }
            }
        }
    }
}