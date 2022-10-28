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
        public Companion TestCompanion = null;
        private SortedDictionary<uint, CompanionData> MyCompanions = new SortedDictionary<uint, CompanionData>();
        private uint[] GetCompanionDataKeys{ get{ return MyCompanions.Keys.ToArray(); } }

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

        public CompanionData GetCompanionData(uint ID, string ModID = "")
        {
            return GetCompanionData(GetCompanionDataIndex(ID, ModID));
        }

        public CompanionData GetCompanionData(uint Index)
        {
            if(MyCompanions.ContainsKey(Index)) return MyCompanions[Index];
            return null;
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

        public override void OnRespawn(Player player)
        {
            if(player is Companion)
            {
                ((Companion)player).OnSpawnOrTeleport();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if(IsPlayerCharacter(player)) //Character spawns, but can't be seen on the world.
            {
                for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
                {
                    uint MyKey = SummonedCompanionKey[i];
                    SummonedCompanionKey[i] = 0;
                    if(MyKey > 0)
                        CallCompanion(MyKey);
                }
                if(!HasCompanion(0)) //ID 0 is Rococo
                {
                    AddCompanion(0);
                    CallCompanion(0, "");
                }
                AddCompanion(1);
                if(!IsCompanionSummoned(1)) CallCompanion(1, "");
            }
        }

        public bool AddCompanion(uint CompanionID, string CompanionModID = "")
        {
            if(CompanionModID == "") CompanionModID = MainMod.GetModName;
            uint NewIndex = 1;
            foreach(uint Key in MyCompanions.Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID)) return false;
                if(Key == NewIndex)
                    NewIndex++;
            }
            MyCompanions.Add(NewIndex, new CompanionData(CompanionID, CompanionModID, NewIndex));
            return true;
        }

        public bool HasCompanion(uint CompanionID, string CompanionModID = "")
        {
            foreach(uint Key in MyCompanions.Keys)
            {
                if(MyCompanions[Key].IsSameID(CompanionID, CompanionModID)) return true;
            }
            return false;
        }

        public bool CallCompanion(uint ID, string ModID = "")
        {
            return CallCompanion(GetCompanionDataIndex(ID, ModID));
        }

        public bool CallCompanion(uint Index)
        {
            if(Player is Companion || Index == 0 || !MyCompanions.ContainsKey(Index)) return false;
            foreach(uint i in SummonedCompanionKey)
            {
                if (i == Index) return false;
            }
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if (SummonedCompanionKey[i] == 0)
                {
                    CompanionData data = GetCompanionData(Index);
                    SummonedCompanions[i] = MainMod.SpawnCompanion(Player.Bottom, data, Player);
                    SummonedCompanionKey[i] = Index;
                    return true;
                }
            }
            return false;
        }

        public bool DismissCompanion(uint Index)
        {
            for(int i = 0; i < MainMod.MaxCompanionFollowers; i++)
            {
                if(SummonedCompanionKey[i] == Index)
                {
                    MainMod.DespawnCompanion(SummonedCompanions[i].GetWhoAmID);
                    SummonedCompanions[i] = null;
                    SummonedCompanionKey[i] = 0;
                    return true;
                }
            }
            return false;
        }

        public bool IsCompanionSummoned(uint ID, string ModID = "")
        {
            uint Index = GetCompanionDataIndex(ID, ModID);
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
            }
            return true;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if(!Player.dead && Player is Companion)
            {
                SoundEngine.PlaySound(((Companion)Player).Base.HurtSound, Player.position);
            }
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
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("LastCompanionsSaveVersion", MainMod.CompanionSaveVersion);
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
    }
}