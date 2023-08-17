using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Companion CompanionMountedOnMe = null, MountedOnCompanion = null, ControlledCompanion = null;
        public Companion GetCompanionControlledByMe { get { return ControlledCompanion; } internal set { ControlledCompanion = value; } }
        public Companion GetCompanionMountedOnMe { get { return CompanionMountedOnMe; } internal set { CompanionMountedOnMe = value; } }
        public Companion GetMountedOnCompanion { get { return MountedOnCompanion; } internal set { MountedOnCompanion = value; } }
        private static bool DrawHoldingCompanionArm = false;
        private byte ActiveRequests = 0;
        private Dictionary<Player, int> CharacterHitDelays = new Dictionary<Player, int>();
        private uint TitanGuardianSlot = uint.MaxValue;
        public uint GetTitanGuardianSlot { get { return TitanGuardianSlot; } }

        public override bool IsCloneable => false;
        protected override bool CloneNewInstances => false;
        public Player TalkPlayer { get; internal set; }
        public float FollowBehindDistancing = 0, FollowAheadDistancing = 0;

        public KnockoutStates KnockoutState = KnockoutStates.Awake;
        private sbyte ReviveBoostStack = 0, ReviveBoost = 0;
        private float ReviveStack = 0;
        public float GetReviveStack { get { return ReviveStack; } }
        public float GetReviveBoost { get { return ReviveBoostStack; } }
        public int GetRescueStack { get { return RescueStack; } }
        private int RescueStack = 0;
        private Companion RescueCompanion = null;
        public const int MaxRescueStack = 10 * 60;
        public const float MaxReviveStack = 90, MinReviveStack = -90; //Was -150
        public float GroupDamageMod = 1f;
        public bool IsBuddiesMode { get { return BuddyCompanion > 0; } }
        private uint BuddyCompanion = 0;
        public uint GetBuddyCompanionIndex { get { return BuddyCompanion; } }
        public Companion GetBuddyCompanion
        {
            get
            {
                if (!IsBuddiesMode) return null;
                for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    if (SummonedCompanionKey[i] == BuddyCompanion)
                    {
                        return SummonedCompanions[i];
                    }
                }
                return null;
            }
        }
        public float BuddiesModeEffective = 1f;
        public bool HasFirstSimbol = false, GoldenShowerStance = false;
        private uint PreviousSaveVersion = 0;

        public InteractionTypes InteractionType = InteractionTypes.None;
        public short InteractionDuration = 0, InteractionMaxDuration = 0;
        private bool LastTeleported = false;

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
                    double Result = player.Hurt(damageSource, Damage, hitDirection, pvp, quiet, cooldownCounter: cooldownCounter);
                    Main.myPlayer = MainMod.MyPlayerBackup;
                    return Result;
                }
            }
            return player.Hurt(damageSource, Damage, hitDirection, pvp, quiet, cooldownCounter: cooldownCounter);
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

        public static bool IsLocalCompanion(Player player)
        {
            return player is Companion && (player as Companion).IsLocalCompanion;
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

        public static Companion GetPlayerLeaderCompanion(Player player)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (pm.SummonedCompanions[0] != null)
                return pm.SummonedCompanions[0];
            return null;
        }

        public static Companion GetPlayerMainGuardian(Player player)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(Companion c in pm.SummonedCompanions)
            {
                if (c != null && c.GetGroup.IsTerraGuardian)
                    return c;
            }
            return null;
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

        public static Companion PlayerGetControlledCompanion(Player player)
        {
            return player.GetModPlayer<PlayerMod>().GetCompanionControlledByMe;
        }

        public static Player GetPlayerImportantControlledCharacter(Player player)
        {
            Player Target = player;
            Companion Controlled = PlayerMod.PlayerGetControlledCompanion(Target);
            if (Controlled != null)
            {
                Target = Controlled;
            }
            Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(Target);
            if (Mount != null)
                return Mount;
            return Target;
        }

        public static bool IsPlayerCharacter(Player player)
        {
            return !(player is Companion) || ((Companion)player).IsPlayerCharacter || (player is Companion && (player as Companion).IsBeingControlledBySomeone);
        }

        public static bool IsLocalCharacter(Player player)
        {
            return player.whoAmI == Main.myPlayer || (player is Companion && (player as Companion).IsLocalCompanion);
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

        public static KnockoutStates GetPlayerKnockoutState(Player player)
        {
            return player.GetModPlayer<PlayerMod>().KnockoutState;
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
            HasFirstSimbol = false;
            GoldenShowerStance = false;
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
            KnockoutStateResetStatus();
            if (Player is Companion && (Player as Companion).Owner != null)
            {
                GroupDamageMod = (Player as Companion).Owner.GetModPlayer<PlayerMod>().GroupDamageMod;
                if(PlayerMod.GetIsPlayerBuddy((Player as Companion).Owner, (Player as Companion)))
                {
                    UpdateBuddiesModeStatus(Player as Companion);
                }
            }
            else
            {
                BuddiesModeEffective = 0;
                GroupDamageMod = 0;
                foreach(Companion companion in SummonedCompanions)
                {
                    if (companion != null)
                    {
                        if (GroupDamageMod == 0) GroupDamageMod = MainMod.DamageNerfByCompanionCount;
                        else GroupDamageMod += GroupDamageMod * MainMod.DamageNerfByCompanionCount;
                        BuddiesModeEffective++;
                    }
                }
                GroupDamageMod = 1f - GroupDamageMod;
                if (BuddiesModeEffective > 1)
                    BuddiesModeEffective = 1f / BuddiesModeEffective;
                else
                    BuddiesModeEffective = 1;
                if (IsBuddiesMode && GetBuddyCompanion != null)
                {
                    UpdateBuddiesModeStatus(GetBuddyCompanion);
                }
            }
        }

        private void UpdateBuddiesModeStatus(Companion buddy)
        {
            float HealthMod, DamageMod;
            int DefenseMod;
            buddy.GetBuddiesModeBenefits(out HealthMod, out DamageMod, out DefenseMod);
            Player.statLifeMax2 += (int)(Player.statLifeMax2 * HealthMod);
            Player.GetDamage<GenericDamageClass>() += DamageMod;
            Player.statDefense += DefenseMod;
        }

        private void KnockoutStateResetStatus()
        {
            if (KnockoutState == KnockoutStates.Awake) return;
            Player.noKnockback = true;
            switch(KnockoutState)
            {
                case KnockoutStates.KnockedOut:
                    Player.lifeRegenCount += (int)((1 + ReviveBoost) * 0.5f);
                    break;
                case KnockoutStates.KnockedOutCold:
                    Player.lifeRegenTime = 0;
                    Player.lifeRegenCount = 0;
                    break;
            }

            Player.aggro -= 50;
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (KnockoutState == KnockoutStates.KnockedOutCold)
                regen = 0;
        }

        public override void PreUpdateBuffs()
        {
            Companions.CelesteBase.ApplyPrayerTo(Player);
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

        public override void PostUpdateMiscEffects()
        {
            Player.GetDamage<GenericDamageClass>() *= GroupDamageMod;
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
            if (Player is Companion)
            {
                ModCompatibility.NExperienceModCompatibility.CheckCompanionLevel(Player);
            }
        }

        public override void OnRespawn()
        {
            if(Player is Companion)
            {
                Companion c = (Companion) Player;
                if(!WorldMod.HasMetCompanion(c.Data) && !WorldMod.IsStarterCompanion(c))
                {
                    if (!WorldMod.RemoveCompanionNPC(c))
                        MainMod.DespawnCompanion(c.GetWhoAmID);
                }
                else
                {
                    ((Companion)Player).OnSpawnOrTeleport();
                }
            }
            if (IsPlayerCharacter(Player))
            {
                foreach(Companion c in SummonedCompanions)
                {
                    if (c != null && !c.gross)
                    {
                        c.Teleport(Player);
                    }
                }
            }
            KnockoutState = KnockoutStates.Awake;
            foreach(Companion c in SummonedCompanions)
            {
                if (c != null && !c.dead && c.KnockoutStates >= KnockoutStates.KnockedOut)
                {
                    c.Teleport(Player.position);
                }
            }
            Player.fullRotation = 0;
        }

        public override void OnEnterWorld()
        {
            if(IsPlayerCharacter(Player)) //Character spawns, but can't be seen on the world.
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
                TryForcingBuddyToSpawn();
                //
                /*const uint CompanionID = CompanionDB.Leona;
                if (MainMod.DebugMode && !HasCompanion(CompanionID))
                    AddCompanion(CompanionID);*/
            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath && IsPlayerCharacter(Player))
            {
                bool AnyPlayerHasCompanion =false;
                foreach(Terraria.IO.PlayerFileData pfd in Main.PlayerList)
                {
                    PlayerMod pm = pfd.Player.GetModPlayer<PlayerMod>();
                    foreach (uint key in pm.SummonedCompanionKey)
                    {
                        if (key > 0)
                        {
                            AnyPlayerHasCompanion = true;
                            break;
                        }
                    }
                    if (AnyPlayerHasCompanion)
                        break;
                }
                if (AnyPlayerHasCompanion) return new Item[]{ new Item(ModContent.ItemType<Items.Consumables.PortraitOfAFriend>()) };
            }
            return base.AddStartingItems(mediumCoreDeath);
        }

        private bool TryAddingPortraitOfAFriend()
        {
            foreach(Terraria.IO.PlayerFileData pfd in Main.PlayerList)
            {
                if(pfd.Player != Player && pfd.Player.GetModPlayer<PlayerMod>().GetCompanionDataKeys.Length > 0)
                {
                    return true;
                }
            }
            return false;
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

        public static bool IsPlayerControllingAnyOfThoseCompanions(Player player, params CompanionID[] ID)
        {
            foreach(CompanionID id in ID)
                if( IsPlayerControllingCompanion(player, id.ID, id.ModID))
                    return true;
            return false;
        }

        public static bool IsPlayerControllingCompanion(Player player, CompanionID ID)
        {
            return IsPlayerControllingCompanion(player, ID.ID, ID.ModID);
        }

        public static bool IsPlayerControllingCompanion(Player player, uint CompanionID, string CompanionModID = "")
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            return pm.ControlledCompanion != null && pm.ControlledCompanion.IsSameID(CompanionID, CompanionModID);
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
            CompanionData data = MainMod.GetCompanionBase(CompanionID, CompanionModID).CreateCompanionData(CompanionID, CompanionModID, NewIndex);
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

        public static bool PlayerHasAnyCompanion(Player player, params CompanionID[] ID)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(CompanionID id in ID)
                if (pm.HasCompanion(id.ID, id.ModID))
                    return true;
            return false;
        }

        public bool HasCompanion(uint CompanionID, string CompanionModID = "")
        {
            foreach(uint Key in MyCompanions.Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID)) return true;
            }
            return false;
        }

        public void RemoveCompanion(Companion companion)
        {
            RemoveCompanion(companion.ID, companion.ModID);
        }

        public void RemoveCompanion(CompanionID id)
        {
            RemoveCompanion(id.ID, id.ModID);
        }

        public void RemoveCompanion(CompanionData data)
        {
            RemoveCompanion(data.ID, data.ModID);
        }

        public void RemoveCompanion(uint CompanionID, string CompanionModID = "")
        {
            uint[] Keys = MyCompanions.Keys.ToArray();
            foreach(uint Key in Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID))
                {
                    DismissCompanionByIndex(Key, false);
                    MyCompanions.Remove(Key);
                }
            }
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

        public static bool PlayerChangeLeaderCompanion(Player PlayerCharacter, Companion companion)
        {
            return PlayerCharacter.GetModPlayer<PlayerMod>().ChangeLeaderCompanion(companion);
        }

        public bool ChangeLeaderCompanion(Companion ToSetLeader)
        {
            int CharacterPosition = -1;
            for(byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if (GetSummonedCompanions[i] == ToSetLeader)
                {
                    CharacterPosition = i;
                    break;
                }
            }
            if (CharacterPosition == -1) return false;
            uint[] NewSummonedCompanionIDs = new uint[MainMod.MaxCompanionFollowers];
            Companion[] NewSummonedCompanions = new Companion[MainMod.MaxCompanionFollowers];
            byte Slot = 0;
            NewSummonedCompanionIDs[Slot] = GetSummonedCompanionKeys[CharacterPosition];
            NewSummonedCompanions[Slot] = GetSummonedCompanions[CharacterPosition];
            Slot++;
            for (byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if (i == CharacterPosition) continue;
                NewSummonedCompanionIDs[Slot] = GetSummonedCompanionKeys[i];
                NewSummonedCompanions[Slot] = GetSummonedCompanions[i];
                Slot++;
            }
            SummonedCompanions = NewSummonedCompanions;
            SummonedCompanionKey = NewSummonedCompanionIDs;
            return true;
        }

        public static bool PlayerCallCompanion(Player player, uint ID, string ModID = "", bool TeleportIfExists = false, bool Forced= false)
        {
            return player.GetModPlayer<PlayerMod>().CallCompanion(ID, ModID, TeleportIfExists, Forced);
        }

        public bool CallCompanion(uint ID, string ModID = "", bool TeleportIfExists = false, bool Forced= false)
        {
            return CallCompanionByIndex(GetCompanionDataIndex(ID, ModID), TeleportIfExists, Forced);
        }

        public static bool PlayerCallCompanionByIndex(Player player, uint Index, bool TeleportIfExists = false, bool Forced= false)
        {
            return player.GetModPlayer<PlayerMod>().CallCompanionByIndex(Index, TeleportIfExists, Forced);
        }

        public bool CallCompanionByIndex(uint Index, bool TeleportIfExists = false, bool Forced= false)
        {
            if(Player is Companion || Index == 0 || !MyCompanions.ContainsKey(Index) || MyCompanions[Index].Base.IsInvalidCompanion)
            {
                return false;
            }
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
                    SummonedCompanions[i].savedPerPlayerFieldsThatArentInThePlayerClass = Player.savedPerPlayerFieldsThatArentInThePlayerClass;
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
                    if (IsPlayerBuddy(SummonedCompanions[i])) return false;
                    if(SummonedCompanions[i].IsMountedOnSomething)
                        SummonedCompanions[i].ToggleMount(SummonedCompanions[i].GetCharacterMountedOnMe, true);
                    if (SummonedCompanions[i].IsBeingControlledBySomeone)
                        SummonedCompanions[i].TogglePlayerControl(SummonedCompanions[i].GetCharacterControllingMe, true);
                    if(Despawn && SummonedCompanions[i].GetTownNpcState == null)
                    {
                        MainMod.DespawnCompanion(SummonedCompanions[i].GetWhoAmID);
                    }
                    else
                    {
                        if(!WorldMod.HasCompanionNPCSpawnedWhoAmID(SummonedCompanionKey[i]))
                            WorldMod.SetCompanionTownNpc(SummonedCompanions[i]);
                        SummonedCompanions[i].Owner = null;
                        SummonedCompanions[i].savedPerPlayerFieldsThatArentInThePlayerClass = new Player.SavedPlayerDataWithAnnoyingRules();
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

        public static bool PlayerHasAnyOfThoseCompanionsSummoned(Player player, params CompanionID[] companions)
        {
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            foreach(CompanionID id in companions)
                if(pm.HasCompanionSummoned(id.ID, id.ModID))
                    return true;
            return false;
        }

        public static bool PlayerHasCompanionSummonedByIndex(Player player, uint Index)
        {
            return player.GetModPlayer<PlayerMod>().HasCompanionSummonedByIndex(Index);
        }
        
        public static bool PlayerHasCompanionSummoned(Player player, Companion c)
        {
            return PlayerHasCompanionSummoned(player, c.GetCompanionID);
        }
        
        public static bool PlayerHasCompanionSummoned(Player player, CompanionData data)
        {
            return PlayerHasCompanionSummoned(player, data.GetMyID);
        }

        public static bool PlayerHasCompanionSummoned(Player player, CompanionID id)
        {
            return PlayerHasCompanionSummoned(player, id.ID, id.ModID);
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

        public static void DrawPlayerHeadInterface(Player player, Vector2 Position, float Scale = 1f, float MaxDimension = 0)
        {
            DrawPlayerHeadInterface(player, Position, player.direction == -1, Scale, MaxDimension);
        }

        public static void DrawPlayerHeadInterface(Player player, Vector2 Position, bool FacingLeft, float Scale = 1f, float MaxDimension = 0)
        {
            RasterizerState state = MainMod.GetRasterizerState;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, state, (Effect)null, Main.UIScaleMatrix);
            DrawPlayerHead(player, Position, FacingLeft, Scale, MaxDimension);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, state, (Effect)null, Main.UIScaleMatrix);
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

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            //Revert this later
            Main.myPlayer = Player.whoAmI;
            //
            if(Player is Companion)
            {
                modifiers.DisableSound();
                Companion c = (Companion)Player;
                modifiers.FinalDamage *= 1f - c.DefenseRate;
            }
            if (KnockoutState == KnockoutStates.KnockedOut)
            {
                modifiers.FinalDamage *= 0.5f;
            }
        }
        
        private static int HealthOnHurt = 100;

        public override void OnHurt(Player.HurtInfo info)
        {
            Main.myPlayer = MainMod.MyPlayerBackup; //Reverts above mask.
            int damage = info.Damage;
            if (Player is Companion)
            {
                if(!Player.dead)
                {
                    SoundEngine.PlaySound(((Companion)Player).Base.HurtSound, Player.position);
                    if (damage > 0) (Player as Companion).AddSkillProgress((float)damage * 2, CompanionSkillContainer.EnduranceID);
                }
            }
            if (KnockoutState == KnockoutStates.KnockedOut)
            {
                Player.AddBuff(BuffID.Bleeding, 5 * 60);
            }
            if (info.PvP)
            {
                Player Attacker = Main.player[info.DamageSource.SourcePlayerIndex];
                if (Attacker is Companion)
                {
                    Companion c = (Companion)Player;
                    if (info.DamageSource.SourceProjectileLocalIndex != -1)
                    {
                        Projectile proj = Main.projectile[info.DamageSource.SourceProjectileLocalIndex];
                        if(proj.DamageType is MeleeDamageClass)
                            c.AddSkillProgress(damage, CompanionSkillContainer.StrengthID);
                        else if(proj.DamageType is RangedDamageClass)
                            c.AddSkillProgress(damage, CompanionSkillContainer.MarksmanshipID);
                        else if(proj.DamageType is MagicDamageClass)
                            c.AddSkillProgress(damage, CompanionSkillContainer.MysticismID);
                        else if(proj.DamageType is SummonDamageClass)
                            c.AddSkillProgress(damage, CompanionSkillContainer.LeadershipID);
                        //if (crit)
                        //    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
                        if (Player is Companion)
                            (Player as Companion).Base.OnAttackedByProjectile(Player as Companion, proj, damage, false);
                    }
                    else
                    {
                        Item item = Attacker.HeldItem;
                        c.AddSkillProgress(damage, item.DamageType is MeleeDamageClass ? CompanionSkillContainer.StrengthID : CompanionSkillContainer.LeadershipID);
                        //if (crit)
                        //    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
                        if (Player is Companion)
                            (Player as Companion).Base.OnAttackedByPlayer(Player as Companion, Attacker, damage, false);
                    }
                }
            }
            HealthOnHurt = Player.statLife - info.Damage;
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                if(!c.GetGoverningBehavior().CanBeAttacked)
                    return true;
            }
            if (Player.whoAmI == Main.myPlayer || IsLocalCompanion(Player))
            {
                if (GetCompanionControlledByMe != null) return true;
            }
            return KnockoutState == KnockoutStates.KnockedOutCold;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                if(Main.rand.NextFloat() * 100 < c.DodgeRate)
                {
                    CombatText.NewText(c.getRect(), Color.Silver, "Dodged");
                    if (!c.noKnockback)
                    {
                        c.velocity.X = 4.5f * info.HitDirection;
                        c.velocity.Y = - 3.5f;
                    }
                    c.AddSkillProgress((float)info.Damage * 4, CompanionSkillContainer.AcrobaticsID);
                    c.immuneTime = info.PvP ? 8 : c.longInvince ? 80 : 40;
                    c.immune = true;
                    return false;
                }
                if(Main.rand.NextFloat() * 100 < c.BlockRate)
                {
                    CombatText.NewText(c.getRect(), Color.Silver, "Blocked");
                    c.AddSkillProgress((float)info.Damage * 4, CompanionSkillContainer.EnduranceID);
                    info.SoundDisabled = true;
                    c.immuneTime = info.PvP ? 8 : c.longInvince ? 80 : 40;
                    c.immune = true;
                    return false;
                }
            }
            if (Player.HasBuff<Buffs.TgGodTailBlessing>() && Main.rand.Next(5) == 0)
            {
                Player.immuneTime = Player.longInvince ? 120 : 60;
                Player.immune = true;
                CombatText.NewText(Player.getRect(), Color.Silver, "Protected", true);
                return true;
            }
            return base.FreeDodge(info);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (Player is Companion)
            {
                Companion c = (Companion)Player;
                c.AddSkillProgress(damageDone, item.DamageType is MeleeDamageClass ? CompanionSkillContainer.StrengthID : CompanionSkillContainer.LeadershipID);
                if (hit.Crit)
                    c.AddSkillProgress(damageDone, CompanionSkillContainer.LuckID);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player is Companion)
            {
                int damage = hit.Damage;
                Companion c = (Companion)Player;
                if(proj.DamageType is MeleeDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.StrengthID);
                else if(proj.DamageType is RangedDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MarksmanshipID);
                else if(proj.DamageType is MagicDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.MysticismID);
                else if(proj.DamageType is SummonDamageClass || proj.DamageType is SummonMeleeSpeedDamageClass)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LeadershipID);
                if (hit.Crit)
                    c.AddSkillProgress(damage, CompanionSkillContainer.LuckID);
            }
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (Player is Companion)
            {
                if(!Player.dead)
                {
                    SoundEngine.PlaySound(((Companion)Player).Base.HurtSound, Player.position);
                    if (info.Damage > 0)(Player as Companion).AddSkillProgress((float)info.Damage * 2, CompanionSkillContainer.EnduranceID);
                }
            }
            if (KnockoutState == KnockoutStates.KnockedOut)
            {
                Player.AddBuff(BuffID.Bleeding, 5 * 60);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player is Companion)
            {
                if(!(Player as Companion).GetGoverningBehavior().CanKill(Player as Companion))
                {
                    return false;
                }
                if(!ForcedDeath && MainMod.CompanionKnockoutEnable)
                {
                    if (KnockoutState == KnockoutStates.Awake)
                        EnterKnockoutState(reason: damageSource);
                    return false;
                }
            }
            else
            {
                if (!ForcedDeath && MainMod.PlayerKnockoutEnable)
                {
                    if (KnockoutState == KnockoutStates.Awake)
                        EnterKnockoutState(reason: damageSource);
                    return false;
                }
            }
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if(Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.DeathSound, Player.position);
                (Player as Companion).GetGoverningBehavior().WhenKOdOrKilled(Player as Companion, true);
            }
            if(Player is TerraGuardian)
            {
                ((TerraGuardian)Player).OnDeath();
            }
        }

        public static bool IsPlayerDefeated(Player p)
        {
            return p.dead || GetPlayerKnockoutState(p) == KnockoutStates.KnockedOutCold;
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
            UpdateGroup();
            if(!(Player is Companion))
            {
                UpdateControllingScripts();
            }
            else if (IsLocalCharacter(Player) && Player.statLife <= 0 && !Player.dead)
            {
                int MyPlayerBackup = Main.myPlayer;
                Main.myPlayer = Player.whoAmI;
                (Player as Companion).KillMe(PlayerDeathReason.ByCustomReason(Player.name + " was slain..."), 1, Player.direction);
                Main.myPlayer = MyPlayerBackup;
            }
            if(ControlledCompanion == null)
            {
                UpdateMountedScripts();
                UpdateSittingOffset();
            }
            UpdateKnockout();
            UpdateInteraction();
            //CheckForTeleport();
        }

        /*private void CheckForTeleport() //Doesn't work as intended...
        {
            if (Player != MainMod.GetLocalPlayer) return;
            if (Player.teleportTime > 0 && !LastTeleported)
            {
                Main.NewText("Triggered.");
                foreach(Companion c in SummonedCompanions)
                {
                    if (c != null)
                    {
                        c.Teleport(Player.Bottom, Player.teleportStyle);
                    }
                }
            }
            LastTeleported = Player.teleportTime > 0;
        }*/

        private void UpdateGroup()
        {
            float FollowForwardIndex = 0, FollowBackIndex = 20;
            bool FoundTitanGuardian = false;
            TitanGuardianSlot = uint.MaxValue;
            for(int i = 0; i < SummonedCompanions.Length; i++)
            {
                if (SummonedCompanions[i] != null)
                {
                    if (!FoundTitanGuardian && SummonedCompanions[i].TitanCompanion)
                    {
                        TitanGuardianSlot = SummonedCompanions[i].Index;
                        FoundTitanGuardian = true;
                    }
                    if (!SummonedCompanions[i].IsBeingControlledBySomeone && (!SummonedCompanions[i].IsMountedOnSomething || SummonedCompanions[i].CompanionHasControl))
                    {
                        float ScaleX = SummonedCompanions[i].Base.Width * SummonedCompanions[i].Scale * 0.5f;
                        SummonedCompanions[i].FollorOrder = new FollowOrderSetting(){ Front = false, Distance = FollowBackIndex + ScaleX * 0.5f };
                        FollowBackIndex += ScaleX + 12;
                    }
                }
            }
        }

        public bool StartInteraction(InteractionTypes type)
        {
            InteractionType = type;
            InteractionDuration = 0;
            switch(type)
            {
                case InteractionTypes.Petting:
                case InteractionTypes.PettingAlternative:
                    InteractionMaxDuration = 300;
                    return true;
            }
            return true;
        }

        private void UpdateInteraction()
        {
            if (InteractionType == InteractionTypes.None) return;
            InteractionDuration ++;
            switch(InteractionType)
            {
                case InteractionTypes.Petting:
                    {
                        float Rotation = -((InteractionDuration % 12 < 6 ? -.08f : 0) + 0.5f);
                        const Player.CompositeArmStretchAmount Stretch = Player.CompositeArmStretchAmount.Full;//InteractionDuration % 20 < 10 ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full;
                        Player.SetCompositeArmFront(true, Stretch, (float)MathF.PI * Rotation * Player.direction);
                    }
                    break;
                case InteractionTypes.PettingAlternative:
                    {
                        const float Rotation = -(.66f);
                        Player.CompositeArmStretchAmount Stretch = (InteractionDuration % 12 < 6 ? Player.CompositeArmStretchAmount.Full : Player.CompositeArmStretchAmount.ThreeQuarters);//InteractionDuration % 20 < 10 ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full;
                        Player.SetCompositeArmFront(true, Stretch, (float)MathF.PI * Rotation * Player.direction);
                        if (Player.itemAnimation > 0 || Player.controlLeft || Player.controlRight || Player.controlJump)
                        {
                            InteractionType = 0;
                            return;
                        }
                    }
                    break;
            }
            if (InteractionDuration >= InteractionMaxDuration)
            {
                InteractionType = 0;
            }
        }

        private void UpdateKnockout()
        {
            if (Player.dead || KnockoutState == KnockoutStates.Awake) return;
            Player.eyeHelper.BlinkBecausePlayerGotHurt();
            if (Player.potionDelayTime < 5)
                Player.AddBuff(BuffID.PotionSickness, 5);
            if (ReviveBoost > 0 && Player.breath < Player.breathMax - 1)
            {
                Player.breathCD += 1 + (int)(ReviveBoost * 0.5f);
                Player.breath++;
                if (Player.breathCD >= Player.breathCDMax)
                {
                    Player.breathCD -= 5;
                    Player.breath++;
                }
            }
            if (Player.mount != null && Player.mount.Active)
            {
                Player.mount.Dismount(Player);
            }
            Player.RemoveAllGrapplingHooks();
            if(MountedOnCompanion != null)
            {
                Player.fullRotation = 0;
                Player.fullRotationOrigin.X = 0;
                Player.fullRotationOrigin.Y = 0;
                ChangeReviveStack(1);
            }
            else if (Player.velocity.X != 0)
            {
                Player.fullRotation = Player.velocity.X * 0.05f;
                Player.fullRotationOrigin.X = Player.width * 0.5f;
                Player.fullRotationOrigin.Y = Player.height * 0.5f;
            }
            else
            {
                if (Player is TerraGuardian)
                {
                    Player.fullRotation = 0;
                    Player.fullRotationOrigin.X = 0;
                    Player.fullRotationOrigin.Y = 0;
                }
                else
                {
                    Player.fullRotation = -1.570796326794897f * Player.direction;
                    Player.fullRotationOrigin.X = Player.width * 0.5f;
                    Player.fullRotationOrigin.Y = Player.height * 0.5f + 12;
                    Player.legRotation = 0f;
                }
            }
            ReviveBoost = ReviveBoostStack;
            ReviveBoostStack = 0;
            for(int b = 0; b < Player.MaxBuffs; b++)
            {
                if (Player.buffType[b] > 0 && Player.buffTime[b] > 0 && Main.debuff[Player.buffType[b]] && Player.buffType[b] != BuffID.PotionSickness && !Main.buffNoTimeDisplay[Player.buffType[b]])
                {
                    Player.buffTime[b] -= (int)ReviveBoost;
                    if (Player.buffTime[b] <= 0)
                    {
                        Player.DelBuff(b);
                    }
                }
            }
            if (KnockoutState != KnockoutStates.KnockedOutCold || ReviveBoost != 0)
            {
                float ReviveValue = 0;
                if (ReviveBoost < 0)
                {
                    ReviveValue -= 1f + 0.1f * ReviveBoost;
                }
                else if (Player.bleed)
                {
                    float Power = 1f;
                    float Reduction = -1f;
                    if (Main.masterMode)
                    {
                        Power = 3;
                        Reduction = -0.5f;
                    }
                    else if (Main.expertMode)
                    {
                        Power = 1.5f;
                        Reduction = -0.75f;
                    }
                    ReviveValue = Reduction - (1f / (ReviveBoost * Power + 1));
                }
                else
                {
                    ReviveValue += 1f + 0.15f * ReviveBoost;
                }
                ReviveStack += ReviveValue;
                if (ReviveStack >= MaxReviveStack)
                {
                    int RecoverValue = (int)Math.Max(1, Player.statLifeMax2 * 0.05f);
                    Player.HealEffect(RecoverValue);
                    Player.statLife += RecoverValue;
                    ReviveStack -= MaxReviveStack;
                }
                else if (ReviveStack <= MinReviveStack)
                {
                    int DamageValue = (int)Math.Max(1, Player.statLifeMax2 * 0.05f);
                    CombatText.NewText(Player.getRect(), CombatText.DamagedHostile, DamageValue, false, true);
                    Player.statLife -= DamageValue;
                    ReviveStack -= MinReviveStack;
                }
            }
            if (Player.statLife >= Player.statLifeMax2)
            {
                LeaveKnockoutState();
                return;
            }
            if (Player.statLife <= 0 && KnockoutState != KnockoutStates.KnockedOutCold)
            {
                EnterKnockoutColdState();
            }
            UpdateRescueStack();
            if (Player.tongued && Main.wofNPCIndex > -1)
            {
                if (!(Player is Companion))
                {
                    Vector2 WofCenter = Main.npc[Main.wofNPCIndex].Center;
                    Vector2 MoveDirection = (WofCenter - Player.Center);
                    float Distance = MoveDirection.Length();
                    if (Distance < 11f * 2)
                    {
                        /*if (Player is Companion)
                        {
                            Player.AddBuff(ModContent.BuffType<Buffs.WofFoodDebuff>(), 666);
                        }
                        else*/
                        {
                            foreach (Companion c in GetSummonedCompanions)
                            {
                                if (c != null && c.HasBuff<Buffs.WofFoodDebuff>())
                                {
                                    ForceKillPlayer(c, " was devoured by the Wall of Flesh.");
                                }
                            }
                            Player.Center = WofCenter;
                            ForceKillPlayer(Player, " was devoured by the Wall of Flesh.");
                            Player.immuneAlpha = 255;
                            return;
                        }
                    }
                    else
                    {
                        MoveDirection.Normalize();
                        MoveDirection *= 11f;
                        Player.position += MoveDirection;
                    }
                }
            }
            if (Player.gross && Main.wofNPCIndex > -1)
            {
                float Distance = (Main.npc[Main.wofNPCIndex].Center - Player.Center).Length();
                if (Distance >= 3000)
                {
                    ForceKillPlayer(Player, " couldn't survive Wall of Flesh's curse.");
                    return;
                }
            }
            if (KnockoutState == KnockoutStates.KnockedOutCold)
            {
                if (Player.statLife < 0) Player.statLife = 0;
                /*if (PlayerMod.IsLocalCharacter(Player))
                {
                    bool HasDPSDebuff = Player.onFire || Player.poisoned || Player.suffocating;
                    bool HasCompanionAwake = false;
                    foreach(Companion c in GetSummonedCompanions)
                    {
                        if (c != null && !c.dead && c.KnockoutStates == KnockoutStates.Awake)
                        {
                            HasCompanionAwake = true;
                            break;
                        }
                    }
                    bool ForceReviveCooldown = Player.lavaWet || Player.breath <= 0 || Player.controlHook;
                }*/
            }
        }

        private void UpdateRescueStack()
        {
            if (Player.dead || KnockoutState != KnockoutStates.KnockedOutCold) return;
            if (RescueStack >= MaxRescueStack / 2)
            {
                RescueStack++;
            }
            if(RescueStack >= MaxRescueStack)
            {
                Vector2 SpawnPosition = new Vector2();
                if (RescueCompanion == null)
                {
                    foreach (Companion c in MainMod.ActiveCompanions.Values)
                    {
                        if (!c.dead && c.Owner == null && HasCompanion(c.ID, c.ModID) && PlayerMod.GetPlayerKnockoutState(c) == KnockoutStates.Awake && !c.IsHostileTo(Player) && (SpawnPosition.X == 0 || Main.rand.Next(2) == 0))
                        {
                            SpawnPosition = c.position;
                        }
                    }
                }
                else
                {
                    SpawnPosition = RescueCompanion.position;
                    SpawnPosition.Y += RescueCompanion.height;
                }
                if(SpawnPosition.X == 0)
                {
                    Player.FindSpawn();
                    if (Player.CheckSpawn(Player.SpawnX, Player.SpawnY))
                    {
                        SpawnPosition = new Vector2(Player.SpawnX * 16 + 8 - Player.width * 0.5f, Player.SpawnY * 16 - Player.height);
                    }
                    else
                    {
                        SpawnPosition = new Vector2(Main.spawnTileX * 16 + 8 - Player.width * 0.5f, Main.spawnTileY * 16 - Player.height);
                    }
                }
                Player.Teleport(SpawnPosition - Vector2.UnitY * Player.height, TeleportationStyleID.TeleportationPylon);
                foreach(Companion c in SummonedCompanions)
                {
                    if (c != null)
                    {
                        c.Teleport(SpawnPosition);
                        //c.position.Y += Player.defaultHeight - Player.height;
                        if (c.KnockoutStates > KnockoutStates.Awake)
                        {
                            c.KnockoutStates = KnockoutStates.KnockedOut;
                            c.statLife = Main.rand.Next(1, (int)(c.statLifeMax2 * 0.8f));
                        }
                    }
                }
                KnockoutState = KnockoutStates.KnockedOut;
                Player.statLife = 1;
                RescueStack = 0;
                RescueCompanion = null;
            }
        }

        private static bool ForcedDeath = false;
        public static void ForceKillPlayer(Player player, string DeathMessage = "", bool ShowCharacterNameBefore = true)
        {
            ForcedDeath = true;
            player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason((ShowCharacterNameBefore ? player.name : "") + DeathMessage), 1, 0);
            ForcedDeath = false;
        }

        public void EnterKnockoutState(bool Friendly = false, PlayerDeathReason reason = null)
        {
            Player.statLife = (int)MathF.Min(HealthOnHurt + Player.statLifeMax2 * 0.5f, Player.statLifeMax2 * 0.5f);
            if ((Player.lavaWet && !Player.lavaImmune) || Player.starving || Player.burned || (reason != null && 
                (reason.SourceOtherIndex == 11 || reason.SourceOtherIndex == 12 || //Wof Related
                reason.SourceOtherIndex == 19) || //Left world by top
                (Player.statLife <= 0 && (reason.SourceOtherIndex == 5 || //Petrified
                reason.SourceOtherIndex == 0)))) // Fall)))
            {
                EnterKnockoutColdState(reason: reason);
                return;
            }
            if (Player.statLife <= 0)
            {
                EnterKnockoutColdState(reason: reason);
                return;
            }
            else
            {
                KnockoutState = KnockoutStates.KnockedOut;
            }
            if (Player.mount.Active) Player.mount.Dismount(Player);
            if (Player is Companion)
            {
                Companion c = Player as Companion;
                if (c.GetCharacterMountedOnMe != null)
                    c.ToggleMount(c.GetCharacterMountedOnMe, true);
                c.GetGoverningBehavior().WhenKOdOrKilled(c, false);
            }
            if (!Friendly && !Player.HasBuff(BuffID.Bleeding))
            {
                Player.AddBuff(BuffID.Bleeding, 5 * 60);
            }
            if (Player.talkNPC > -1)
                Player.SetTalkNPC(-1);
            Player.chest = -1;
            Player.sign = -1;
            Player.pulley = false;
        }

        public void EnterKnockoutColdState(bool AllowDeath = true, PlayerDeathReason reason = null)
        {
            bool Kill = false;
            if (Player is Companion)
            {
                Kill = !MainMod.CompanionKnockoutColdEnable;
            }
            else
            {
                Kill = !MainMod.PlayerKnockoutColdEnable;
            }
            if ((Player.lavaWet && !Player.lavaImmune) || Player.burned || 
                (reason != null && 
                (reason.SourceOtherIndex == 11 || reason.SourceOtherIndex == 12 || //Wof Related
                reason.SourceOtherIndex == 19))) //Fall
                Kill = true;
            if(!Kill)
            {
                if (!Player.dead)
                {
                    Player.statLife = 0;
                    KnockoutState = KnockoutStates.KnockedOutCold;
                }
            }
            else
            {
                if (Player.lavaWet || Player.burned)
                    ForceKillPlayer(Player, " melted.");
                else
                    ForceKillPlayer(Player, " didn't survived.");
            }
        }

        public void LeaveKnockoutState()
        {
            KnockoutState = KnockoutStates.Awake;
            Player.fullRotation = 0;
            float HealthRestoreValue = 0.5f;
            Player.statLife = (int)(Player.statLifeMax2 * HealthRestoreValue);
            CombatText.NewText(Player.getRect(), Color.Cyan, "Revived", true);
            Player.immuneTime = (Player.longInvince ? 120 : 60) * 3;
            RescueStack = 0;
            if (Player is Companion)
            {
                (Player as Companion).SaySomething((Player as Companion).GetDialogues.ReviveMessages(Player as Companion, Player, ReviveStack > 0 ? ReviveContext.ReviveWithOthersHelp : ReviveContext.RevivedByItself));
            }
        }

        public void ChangeReviveStack(sbyte NewChange)
        {
            ReviveBoostStack = (sbyte)System.Math.Clamp(ReviveBoostStack + NewChange, -100, 100);
        }

        public void UpdateSittingOffset()
        {
            if (IsPlayerCharacter(Player)) DrawHoldingCompanionArm = false;
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
            if (!(Player is Companion))
            {
                foreach(Companion c in MainMod.ActiveCompanions.Values)
                {
                    int FurnitureX = c.GetFurnitureX;
                    if(c.sleeping.isSleeping)
                    {
                        FurnitureX += c.direction;
                    }
                    if(c is TerraGuardian && c.UsingFurniture && FurnitureX == TileCenter.X && c.GetFurnitureY == TileCenter.Y)
                    {
                        if (c.MountStyle == MountStyles.PlayerMountsOnCompanion)
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
                        else if(c.sitting.isSitting && c.MountStyle == MountStyles.CompanionRidesPlayer)
                        {
                            if (IsPlayerCharacter(Player)) DrawHoldingCompanionArm = true;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (MountedOnCompanion != null)
                {
                    TerraGuardian tg = (TerraGuardian)MountedOnCompanion;
                    Vector2 Offset;
                    if(Player.sitting.isSitting)
                    {
                        Offset = tg.GetAnimationPosition(AnimationPositions.PlayerSittingOffset, tg.BodyFrameID, AlsoTakePosition: false, DiscountCharacterDimension: false, DiscountDirections: false, ConvertToCharacterPosition: false);
                    }
                    else
                        Offset = tg.GetAnimationPosition(AnimationPositions.PlayerSleepingOffset, tg.BodyFrameID, AlsoTakePosition: false, DiscountCharacterDimension: false, DiscountDirections: false, ConvertToCharacterPosition: false);
                    //Offset.X *= Direction;
                    /*if(Player.sitting.isSitting || (Offset.X > 0 && Offset.Y > 0))
                    {
                        Offset.X += ExtraOffsetX - (ExtraOffsetX * (1 - tg.Scale));
                        Offset.X += 4;
                        if(IsThroneOrBench)
                            Offset.Y += 24 - (24 * (1 - tg.Scale));
                        else
                            Offset.Y += 4;
                    }*/
                    if(Player.sitting.isSitting)
                        Player.sitting.offsetForSeat += Offset;
                    else
                    {
                        Player.sleeping.visualOffsetOfBedBase += Offset;
                    }
                }
            }
        }

        public void UpdateControllingScripts()
        {
            if(ControlledCompanion == null || (Player.teleporting && !LastTeleported)) return;
            if (Player.sitting.isSitting)
            {
                Point TilePosition = (Player.Bottom - Vector2.UnitY * 2).ToTileCoordinates();
                Player.sitting.SitUp(Player);
                ControlledCompanion.UseFurniture(TilePosition.X, TilePosition.Y, true);
            }
            if (Player.sleeping.isSleeping)
            {
                Point TilePosition = (Player.Bottom - Vector2.UnitY * 2).ToTileCoordinates();
                Player.sleeping.StopSleeping(Player);
                ControlledCompanion.UseFurniture(TilePosition.X, TilePosition.Y, true);
            }
            Player.position.X = ControlledCompanion.Center.X - Player.width * 0.5f;
            Player.position.Y = ControlledCompanion.Center.Y - Player.height * 0.5f;
            Player.immuneTime = 5;
            Player.velocity.X = 0;
            Player.velocity.Y = 0;
            Player.aggro = -10000000;
            //Player.immuneAlpha = 0;
            Player.invis = true;
            Player.gills = true;
            Player.statLife = Player.statLifeMax2;
            Player.buffImmune[BuffID.Suffocation] = Player.buffImmune[BuffID.OnFire] = true;
            if (Player.mount.Active)
                Player.mount.Dismount(Player);
            if (Player.chatOverhead.timeLeft > 0)
            {
                ControlledCompanion.chatOverhead.NewMessage(Player.chatOverhead.chatText, Player.chatOverhead.timeLeft);
                Player.chatOverhead.timeLeft = 0;
            }
        }

        public void UpdateMountedScripts()
        {
            if (MountedOnCompanion != null)
            {
                MountedOnCompanion.UpdateMountedBehavior();
            }
            if((Player.teleporting && !LastTeleported) || MountedOnCompanion == null || !(MountedOnCompanion is TerraGuardian))
                return;
            TerraGuardian guardian = (TerraGuardian)MountedOnCompanion;
            if(guardian.dead || guardian.KnockoutStates > KnockoutStates.Awake)
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
            /*if (false && KnockoutState >= KnockoutStates.KnockedOut && MountedOnCompanion.itemAnimation == 0 && MountedOnCompanion.velocity.X == 0 && MountedOnCompanion.velocity.Y == 0)
            {
                Player.position = guardian.Bottom;
                Player.position.X += Player.width * 0.5f * MountedOnCompanion.direction;
                Player.position.Y -= Player.height - 12;
            }
            else*/
            {
                Player.position = guardian.GetMountShoulderPosition;
                if (guardian.whoAmI > Player.whoAmI)
                    Player.position += guardian.velocity;
                Player.position.X -= Player.width * 0.5f;
                Player.position.Y -= Player.height * 0.5f + 8 - guardian.gfxOffY;
            }
            guardian.Base.ModifyMountedCharacterPosition(guardian, Player, ref Player.position);
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
            if(Player is Companion)
            {
                DrawOrderInfo.AddDrawOrderInfo(guardian, Player, DrawOrderInfo.DrawOrderMoment.InFrontOfParent);
            }
        }

        public override void SetControls()
        {
            if (!IsPlayerCharacter(Player))
            {
                return;
            }
            if (KnockoutState >= KnockoutStates.KnockedOut)
            {
                if (KnockoutState == KnockoutStates.KnockedOutCold && Player.controlHook && MountedOnCompanion == null)
                {
                    if (!MainMod.PlayerKnockoutColdEnable)
                    {
                        ForceKillPlayer(Player, " succumbed to its injuries.");
                    }
                    else if (RescueStack < MaxRescueStack / 2)
                    {
                        RescueStack++;
                        if (RescueStack == MaxRescueStack / 2)
                        {
                            RescueCompanion = null;
                            foreach (Companion c in MainMod.ActiveCompanions.Values)
                            {
                                if (!c.dead && c.Owner == null && HasCompanion(c.ID, c.ModID) && PlayerMod.GetPlayerKnockoutState(c) == KnockoutStates.Awake && !c.IsHostileTo(Player) && (RescueCompanion == null || Main.rand.Next(2) == 0))
                                {
                                    RescueCompanion = c;
                                }
                            }
                            if (RescueCompanion != null)
                            {
                                Main.NewText(RescueCompanion.GetDialogues.ReviveMessages(RescueCompanion, Player, ReviveContext.HelpCallReceived));
                            }
                        }
                    }
                }
                Player.controlLeft = Player.controlRight = Player.controlUp = Player.controlDown = Player.controlJump = Player.controlMount =
                    Player.controlQuickMana = Player.controlSmart = Player.controlThrow = Player.controlUseTile = false;
                Player.controlUseItem = false;
                Player.releaseQuickHeal = Player.releaseQuickMana = false;
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (KnockoutState == KnockoutStates.Awake)
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
            }
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("LastCompanionsSaveVersion", MainMod.ModVersion);
            tag.Add("IsKnockedOut", KnockoutState > KnockoutStates.Awake);
            uint[] Keys = MyCompanions.Keys.ToArray();
            tag.Add("TotalCompanions", Keys.Length);
            for(int k = 0; k < Keys.Length; k++)
            {
                uint Key = Keys[k];
                tag.Add("CompanionKey_" + k, Key);
                tag.Add("CompanionID_" + Key, MyCompanions[Key].ID);
                tag.Add("CompanionModID_" + Key, MyCompanions[Key].ModID);
                MyCompanions[Key].Save(tag, Key);
            }
            tag.Add("BuddyCompanionIndex", BuddyCompanion);
            tag.Add("LastSummonedCompanionsCount", MainMod.MaxCompanionFollowers);
            for(int i = 0; i < SummonedCompanions.Length; i++)
                tag.Add("FollowerIndex_" + i, SummonedCompanionKey[i]);
        }

        public override void LoadData(TagCompound tag)
        {
            MyCompanions.Clear();
            if(!tag.ContainsKey("LastCompanionsSaveVersion")) return;
            uint LastCompanionVersion = tag.Get<uint>("LastCompanionsSaveVersion");
            PreviousSaveVersion = LastCompanionVersion;
            if (LastCompanionVersion >= 18)
            {
                bool IsKOd = tag.GetBool("IsKnockedOut");
                if (IsKOd)
                    EnterKnockoutState(true);
            }
            int TotalCompanions = tag.GetInt("TotalCompanions");
            for (int k = 0; k < TotalCompanions; k++)
            {
                uint Key = tag.Get<uint>("CompanionKey_" + k);
                uint NewID = tag.Get<uint>("CompanionID_" + Key);
                string NewModID = tag.GetString("CompanionModID_" + Key);
                CompanionData data = MainMod.GetCompanionBase(NewID, NewModID).CreateCompanionData(NewID, NewModID, Key);
                data.Load(tag, Key, LastCompanionVersion);
                MyCompanions.Add(Key, data);
                //data.SetSaveData(Player);
            }
            if (LastCompanionVersion >= 19)
            {
                BuddyCompanion = tag.Get<uint>("BuddyCompanionIndex"); //Doesn't seem to load...
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
            Companion FocusCameraOn = null;
            if (ControlledCompanion != null)
            {
                if (ControlledCompanion.GetPlayerMod.MountedOnCompanion != null)
                    FocusCameraOn = ControlledCompanion.GetPlayerMod.MountedOnCompanion;
                else
                    FocusCameraOn = ControlledCompanion;
            }
            else if (MountedOnCompanion != null)
            {
                FocusCameraOn = MountedOnCompanion;
            }
            if (FocusCameraOn != null)
            {
                Main.screenPosition = new Vector2(FocusCameraOn.Center.X - Main.screenWidth * 0.5f, FocusCameraOn.Center.Y + FocusCameraOn.gfxOffY - Main.screenHeight * 0.5f);
            }
        }

        public override bool PreItemCheck()
        {
            if (!(Player is Companion)) SystemMod.BackupAndPlaceCompanionsOnPlayerArray();
            if (ControlledCompanion != null) return false;
            return base.PreItemCheck();
        }

        public override void PostItemCheck()
        {
            if (!(Player is Companion)) SystemMod.RestoreBackedUpPlayers(true);
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

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            
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

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (Player is TerraGuardian)
                TerraGuardianDrawLayersScript.HideLayers(Player);
            else
            {
                switch(TerraGuardiansPlayerRenderer.GetDrawRule)
                {
                    case DrawContext.BackLayer:
                        TerraGuardianDrawLayersScript.HideFrontLayers(Player);
                        break;
                    case DrawContext.FrontLayer:
                        TerraGuardianDrawLayersScript.HideBackLayers(Player);
                        break;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (Player is Companion)
                (Player as Companion).Base.OnAttackedByNpc(Player as Companion, npc, hurtInfo.Damage, false);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (Player is Companion)
            {
                (Player as Companion).Base.OnAttackedByProjectile(Player as Companion, proj, hurtInfo.Damage, false);
            }
        }

        public bool SetPlayerBuddy(CompanionID candidate, bool Forced = false)
        {
            if (IsBuddiesMode) return false;
            if (!HasCompanion(candidate.ID, candidate.ModID))
            {
                AddCompanion(candidate.ID, candidate.ModID, false);
            }
            if (!HasCompanionSummoned(candidate.ID, candidate.ModID))
            {
                int LastSlot = -1;
                for(byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    if(SummonedCompanions[i] == null)
                    {
                        LastSlot = -1;
                        break;
                    }
                    LastSlot = i;
                }
                if (LastSlot > -1)
                {
                    DismissCompanionByIndex(SummonedCompanionKey[LastSlot], false);
                }
                if (!CallCompanion(candidate.ID, candidate.ModID, true, true))
                {
                    return false;
                }
            }
            Companion c = PlayerGetSummonedCompanion(Player, candidate.ID, candidate.ModID);
            c.IncreaseFriendshipPoint(1);
            ChangeLeaderCompanion(c);
            WorldMod.AddCompanionMet(candidate.ID, candidate.ModID);
            WorldMod.SetCompanionTownNpc(c);
            BuddyCompanion = c.Index;
            Dialogue.ChangeCurrentSpeaker(c);
            Main.NewText(Dialogue.ParseText("<" + c.GetNameColored() + "> " + c.GetDialogues.BuddiesModeMessage(c, BuddiesModeContext.PlayerSaysYes)));
            return true;
        }

        public bool SetPlayerBuddy(Companion candidate, bool Forced = false)
        {
            if (IsBuddiesMode || !HasCompanion(candidate.ID, candidate.ModID)) return false;
            if (!HasCompanionSummonedByIndex(candidate.Index))
            {
                int LastSlot = -1;
                for(byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    if(SummonedCompanions[i] == null)
                    {
                        LastSlot = -1;
                        break;
                    }
                    LastSlot = i;
                }
                if (LastSlot > -1)
                {
                    DismissCompanionByIndex(SummonedCompanionKey[LastSlot], false);
                }
                if (!CallCompanionByIndex(candidate.Index, true, true))
                {
                    return false;
                }
            }
            ChangeLeaderCompanion(candidate);
            WorldMod.AddCompanionMet(candidate.ID, candidate.ModID);
            WorldMod.SetCompanionTownNpc(candidate);
            BuddyCompanion = candidate.Index;
            Main.NewText(Player.name + " has appointed " + candidate.GetNameColored() + " as their buddy.");
            return true;
        }

        public static bool GetIsBuddiesMode(Player player)
        {
            return player.GetModPlayer<PlayerMod>().IsBuddiesMode;
        }

        public static bool GetIsPlayerBuddy(Player player, Companion companion)
        {
            return player.GetModPlayer<PlayerMod>().IsPlayerBuddy(companion);
        }

        public bool IsPlayerBuddy(Companion companion)
        {
            return companion.Index == BuddyCompanion;
        }

        private void TryForcingBuddyToSpawn()
        {
            if (!IsBuddiesMode) return;
            if (!HasCompanionSummonedByIndex(BuddyCompanion))
            {
                BuddyCompanion = 0;
                return;
            }
            if(!HasCompanionSummonedByIndex(BuddyCompanion))
            {
                int LastSlot = -1;
                for(byte i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    if(SummonedCompanions[i] == null)
                    {
                        LastSlot = -1;
                        break;
                    }
                    LastSlot = i;
                }
                if (LastSlot > -1)
                {
                    DismissCompanionByIndex(SummonedCompanionKey[LastSlot]);
                }
                CallCompanionByIndex(BuddyCompanion, true, true);
                ChangeLeaderCompanion(GetBuddyCompanion);
            }
        }
    }

    public enum KnockoutStates : byte
    {
        Awake = 0,
        KnockedOut = 1,
        KnockedOutCold = 2
    }

    public enum InteractionTypes : byte
    {
        None = 0,
        Petting = 1,
        PettingAlternative = 2
    }
}