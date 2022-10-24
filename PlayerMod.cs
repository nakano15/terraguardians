using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class PlayerMod : ModPlayer
    {
        public Companion TestCompanion = null;
        public Dictionary<uint, CompanionData> MyCompanions = new Dictionary<uint, CompanionData>();

        public override bool IsCloneable => false;
        protected override bool CloneNewInstances => false;
        public Player TalkPlayer { get; internal set; }

        public static bool IsPlayerCharacter(Player player)
        {
            return !(player is Companion) || ((Companion)player).IsPlayerCharacter;
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
                CompanionData RococoData = new CompanionData();
                TestCompanion = MainMod.SpawnCompanion(player.Bottom, RococoData, Player);
            }
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

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> Items = new List<Item>();
            if (Player is Companion)
            {
                Companion companion = (Companion)Player;
                InitialItemDefinition[] InitialInventory;
                int[] InitialEquipments = new int[9];
                companion.Base.InitialInventory(out InitialInventory, ref InitialEquipments);
                if(InitialInventory != null)
                {
                    foreach(InitialItemDefinition i in InitialInventory)
                    {
                        Items.Add(new Item(i.ID, i.Stack));
                    }
                }
                for(int i = 0; i < InitialEquipments.Length; i++)
                {
                    Player.armor[i].SetDefaults(InitialEquipments[i]);
                }
            }
            return Items;
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            if (Player is Companion)
            {
                Companion companion = (Companion)Player;
                InitialItemDefinition[] InitialInventory;
                int[] InitialEquipments = new int[9];
                companion.Base.InitialInventory(out InitialInventory, ref InitialEquipments);
                foreach(string mod in itemsByMod.Keys)
                {
                    if(mod == "Terraria")
                    {
                        for(int i = 0; i < itemsByMod[mod].Count; i++)
                        {
                            switch(itemsByMod[mod][i].type)
                            {
                                case ItemID.CopperShortsword:
                                case ItemID.CopperPickaxe:
                                case ItemID.CopperAxe:
                                case ItemID.IronShortsword:
                                case ItemID.IronPickaxe:
                                case ItemID.IronAxe:
                                case ItemID.IronHammer:
                                case ItemID.BabyBirdStaff:
                                case ItemID.Torch:
                                case ItemID.Rope:
                                case ItemID.MagicMirror:
                                case ItemID.GrapplingHook:
                                case ItemID.CreativeWings:
                                    itemsByMod[mod].RemoveAt(i);
                                    break;
                            }
                        }
                    }
                }
                for(int i = 0; i < InitialEquipments.Length; i++)
                {
                    Player.armor[i].SetDefaults(InitialEquipments[i]);
                }
            }
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
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            TerraGuardianDrawLayersScript.PreDrawSettings(ref drawInfo);
        }
    }
}