using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using terraguardians.Buffs.GhostFoxHaunts;

namespace terraguardians.Companions.Fluffles
{
    public class FlufflesPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        new Player Target = null;
        byte PlayerChaseTime = 0;
        bool PostBossKillDialogue = false, PassiveAI = false, GrabbingPlayer = false;
        bool IsFamiliarFace = false;
        const byte ChaseTime = 120;

        public override string CompanionNameChange(Companion companion)
        {
            return "Ghost Fox Guardian";
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return PostBossKillDialogue || PassiveAI;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            if (PassiveAI)
            {
                return GetFamiliarFaceDialogue();
            }
            return GetPostBossDialogue();
        }

        MessageBase GetPostBossDialogue()
        {
            IsFamiliarFace = true;
            MessageDialogue md = new MessageDialogue("(She greets you, but seems unable to speak. She seems to be asking of could join your adventures.)");
            md.AddOption("Accept", OnAcceptButtonClicked);
            md.AddOption("Reject", OnRejectButtonClicked);
            return md;
        }

        MessageBase GetFamiliarFaceDialogue()
        {
            IsFamiliarFace = true;
            MessageDialogue md = new MessageDialogue("(She greets you, but seems unable to speak. She seems to be asking of could join your adventures.)");
            md.AddOption("Accept", OnAcceptButtonClicked);
            md.AddOption("Reject", OnRejectButtonClicked);
            return md;
        }

        void OnAcceptButtonClicked()
        {
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
            MessageDialogue md = new MessageDialogue();
            if (IsFamiliarFace)
            {
                md.ChangeMessage("(She nods of acknowledgement, as she smiles at you.)");
            }
            else
            {
                md.ChangeMessage("(She looks happy at you, sketches on the floor her name, " + Dialogue.Speaker.GetNameColored() + ", then places her hand on your shoulder, telling you that you can count on her.)");
            }
            md.AddOption("(continue)", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        void OnRejectButtonClicked()
        {
            WorldMod.AddCompanionMet(Dialogue.Speaker);
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, Dialogue.Speaker);
            MessageDialogue md = new MessageDialogue();
            if (IsFamiliarFace)
            {
                md.ChangeMessage("(She look disappointed, but has a spark of happiness for finding you here too.)");
            }
            else
            {
                md.ChangeMessage("(Her face suddenly changed to a sad look, she sketches her name on the floor, " + Dialogue.Speaker.GetNameColored() + ", then signals saying that If you need her help, you just need to call.)");
            }
            md.AddOption("(continue)", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }

        public override void Update(Companion companion)
        {
            if (PassiveAI)
            {
                WanderAI(companion);
            }
            else if (PostBossKillDialogue)
            {
                if (Target != null)
                {
                    if (Math.Abs(companion.Center.X - Target.Center.X) > 48)
                    {
                        if (companion.Center.X < Target.Center.X)
                            companion.MoveRight = true;
                        else
                            companion.MoveLeft = true;
                    }
                    else
                    {
                        if (companion.Center.X < Target.Center.X)
                            companion.direction = 1;
                        else
                            companion.direction = -1;
                    }
                }
            }
            else if (Target == null)
            {
                WanderAI(companion);
                Target = ViewRangeCheck(companion, companion.direction);
                if (Target != null)
                {
                    PlayerChaseTime = 200;
                }
            }
            else //Chase player and try to catch them
            {
                if (!Target.active || Target.dead)
                {
                    Target = null;
                    return;
                }
                if (GrabbingPlayer)
                {
                    Target.Center = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, 8);
                    Target.direction = -companion.direction;
                    Target.velocity.Y = 0;
                    Target.velocity.X = 0;
                }
                else
                {
                    companion.WalkMode = false;
                    if (Collision.CanHitLine(companion.position, companion.width, companion.height, Target.position, Target.width, Target.height))
                    {
                        if (PlayerChaseTime < ChaseTime)
                            PlayerChaseTime = ChaseTime;
                        else
                            PlayerChaseTime--;
                    }
                    else
                    {
                        PlayerChaseTime--;
                    }
                    if (PlayerChaseTime <= ChaseTime)
                    {
                        if (Target.Center.X < companion.Center.X)
                        {
                            companion.MoveLeft = true;
                        }
                        else
                        {
                            companion.MoveRight = true;
                        }
                        if (Target.position.Y < companion.position.Y)
                        {
                            if (companion.CanDoJumping)
                            {
                                companion.ControlJump = true;
                            }
                        }
                        if (companion.Hitbox.Intersects(Target.Hitbox))
                        {
                            if (PlayerMod.PlayerHasCompanion(Target, companion))
                            {
                                companion.SaySomething(Main.rand.NextDouble() <= 0.5 ? "(She seems to have missed your company.)" : "(She seems really happy for seeing you.)");
                                WorldMod.AddCompanionMet(companion);
                            }
                            else
                            {
                                List<int> CurseDB = new List<int>();
                                CurseDB.Add(ModContent.BuffType<SkullHaunt>());
                                if (NPC.downedBoss2)
                                    CurseDB.Add(ModContent.BuffType<BeeHaunt>());
                                if (Main.hardMode)
                                {
                                    CurseDB.Add(ModContent.BuffType<MeatHaunt>());
                                    CurseDB.Add(ModContent.BuffType<SawHaunt>());
                                }
                                if (NPC.downedPlantBoss)
                                    CurseDB.Add(ModContent.BuffType<ConstructHaunt>());
                                Target.AddBuff(CurseDB[Main.rand.Next(CurseDB.Count)], 5);
                                Target.immune = true;
                                Target.immuneTime = 60 * 30;
                                Target.immuneNoBlink = true;
                                GrabbingPlayer = true;
                                MainMod.MoviePlayer.PlayMovie(new Cutscenes.FlufflesCatchPlayerCutscene());
                            }
                        }
                    }
                    else
                    {
                        if (companion.Center.X < Target.Center.X)
                            companion.direction = 1;
                        else
                            companion.direction = -1;
                    }
                }
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (Target != null && PlayerChaseTime <= ChaseTime)
            {
                companion.ArmFramesID[0] = companion.ArmFramesID[1] = 8;
            }
        }

        internal static void OnMobKill(NPC npc)
        {
            bool LiftedHaunt = false;
            int HauntLiftPosition = -1;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    if (LiftHaunt(Main.player[p], ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>(), NPCID.SkeletronHead, npc))
                    {
                        LiftedHaunt = true;
                    }
                    else if (LiftHaunt(Main.player[p], ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>(), NPCID.QueenBee, npc))
                    {
                        LiftedHaunt = true;
                    }
                    else if (LiftHaunt(Main.player[p], ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>(), NPCID.WallofFlesh, npc))
                    {
                        LiftedHaunt = true;
                    }
                    else if (LiftHaunt(Main.player[p], ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>(), NPCID.TheDestroyer, npc))
                    {
                        LiftedHaunt = true;
                    }
                    else if (LiftHaunt(Main.player[p], ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>(), NPCID.Golem, npc))
                    {
                        LiftedHaunt = true;
                    }
                    if (LiftedHaunt)
                    {
                        HauntLiftPosition = p;
                        break;
                    }
                }
            }
            if (HauntLiftPosition > -1)
            {
                FlufflesBase.FlufflesCompanion Fluffle = WorldMod.SpawnCompanionNPC(Main.player[HauntLiftPosition].Bottom, CompanionDB.Fluffles) as FlufflesBase.FlufflesCompanion;
                if (Fluffle != null)
                {
                    Player player = Main.player[HauntLiftPosition];
                    (Fluffle.preRecruitBehavior as Fluffles.FlufflesPreRecruitBehavior).PostBossKillDialogue = true;
                    Fluffle.velocity.X = 6f * -player.direction;
                    Fluffle.velocity.Y = -8f;
                }
            }
        }

        static bool LiftHaunt(Player player, int HauntID, int RequiredMobID, NPC npc)
        {
            if (player.HasBuff(HauntID) && (npc.type == RequiredMobID || (npc.realLife > -1 && Main.npc[npc.realLife].type == RequiredMobID)))
            {
                player.DelBuff(player.FindBuffIndex(HauntID));
                return true;
            }
            return false;
        }
    }
}