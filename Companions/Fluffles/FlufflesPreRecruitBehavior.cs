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
        bool PostBossKillDialogue = false, PassiveAI = false;
        bool IsFamiliarFace = false;

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
            WorldMod.SetCompanionTownNpc(Dialogue.Speaker);
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
                const byte ChaseTime = 120;
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
                            Target.GetModPlayer<PlayerMod>().EnterKnockoutState(true);
                            Target.statLife = 1;
                            Target.immune = true;
                            Target.immuneTime = 60 * 30;
                            Target.immuneNoBlink = true;
                            WorldMod.RemoveCompanionNPC(companion);
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
}