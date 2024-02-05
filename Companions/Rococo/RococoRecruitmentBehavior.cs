using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Rococo
{
    public class RococoRecruitmentBehavior : IdleBehavior
    {
        private DialogueResults result = DialogueResults.None;
        private Player SpottedPlayer = null;

        public override string CompanionNameChange(Companion companion)
        {
            return companion.GetTranslation("recruitalias");
        }

        public override void Update(Companion companion)
        {
            if (Companion.Behavior_RevivingSomeone) return;
            if(SpottedPlayer == null)
            {
                Player FoundPlayer = ViewRangeCheck(companion, companion.direction);
                if (FoundPlayer != null)
                {
                    SpottedPlayer = FoundPlayer;
                    companion.SpawnEmote(Terraria.GameContent.UI.EmoteID.EmoteConfused, 30);
                }
                else
                {
                    UpdateIdle(companion);
                    if(!Main.dayTime && !ThereIsPlayerInNpcViewRange(companion))
                    {
                        Main.NewText(companion.GetTranslation("rococoleaves"), 225, 200, 0);
                        WorldMod.RemoveCompanionNPC(companion); //Need better way of despawning npc.
                        MainMod.DespawnCompanion(companion.GetWhoAmID);
                        return;
                    }
                }
            }
            else
            {
                if(companion.velocity.X == 0 && companion.velocity.Y == 0)
                {
                    if(SpottedPlayer.Center.X < companion.Center.X)
                    {
                        companion.direction = -1;
                    }
                    else
                    {
                        companion.direction = 1;
                    }
                }
                if (MathF.Abs(SpottedPlayer.Center.X - companion.Center.X) > 350)
                {
                    SpottedPlayer = null;
                }
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            MessageDialogue mb = new MessageDialogue();
            if(PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, companion.ID, companion.ModID))
            {
                mb.ChangeMessage(companion.GetTranslation("recmes1"));
                companion.PlayerMeetCompanion(MainMod.GetLocalPlayer);
                mb.AddOption(companion.GetTranslation("recans1"), Dialogue.LobbyDialogue);
            }
            else
            {
                switch(result)
                {
                    case DialogueResults.None:
                        mb.ChangeMessage(companion.GetTranslation("recmes2"));
                        mb.AddOption(companion.GetTranslation("recans2-1"), OnAcceptClicked);
                        mb.AddOption(companion.GetTranslation("recans2-2"), OnRejectClicked);
                        break;
                    case DialogueResults.AcceptedOnce:
                        mb.ChangeMessage(companion.GetTranslation("recmes3"));
                        break;
                    case DialogueResults.RejectedOnce:
                        mb.ChangeMessage(companion.GetTranslation("recmes4"));
                        mb.AddOption(companion.GetTranslation("recans4"), Dialogue.EndDialogue);
                        break;
                }
            }
            return mb;
        }

        private void OnAcceptClicked()
        {
            result = DialogueResults.AcceptedOnce;
            MessageDialogue mb = new MessageDialogue(Dialogue.Speaker.GetTranslation("recmes5"));
            mb.AddOption(Dialogue.Speaker.GetTranslation("recans5"), Dialogue.LobbyDialogue);
            mb.RunDialogue();
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
        }

        private void OnRejectClicked()
        {
            result = DialogueResults.RejectedOnce;
            MessageDialogue mb = new MessageDialogue(Dialogue.Speaker.GetTranslation("recmes6"));
            mb.AddOption(Dialogue.Speaker.GetTranslation("recans4"), Dialogue.EndDialogue);
            mb.RunDialogue();
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
        }

        public enum DialogueResults : byte
        {
            None,
            AcceptedOnce,
            RejectedOnce
        }
    }
}
