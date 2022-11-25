using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Rococo
{
    public class RococoRecruitmentBehavior : IdleBehavior
    {
        private DialogueResults result = DialogueResults.None;
        private Player SpottedPlayer = null;

        public override void Update(Companion companion)
        {
            if(SpottedPlayer == null)
            {
                Player FoundPlayer = ViewRangeCheck(companion, companion.direction);
                if (FoundPlayer != null)
                {
                    SpottedPlayer = FoundPlayer;
                }
                else
                {
                    UpdateIdle(companion);
                    if(!Main.dayTime && !ThereIsPlayerInNpcViewRange(companion))
                    {
                        Main.NewText("The Raccoon vanished in the sunset.", 225, 200, 0);
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
                mb.ChangeMessage("*He tells you that it's good to see you again.*");
                companion.PlayerMeetCompanion(MainMod.GetLocalPlayer);
                mb.AddOption("Hello.", Dialogue.LobbyDialogue);
            }
            else
            {
                switch(result)
                {
                    case DialogueResults.None:
                        mb.ChangeMessage("*The creature is surprised for seeing me, said that has been travelling over and over looking for a place with cool people to live with. Should I let It live in my world?*");
                        mb.AddOption("You may live here.", OnAcceptClicked);
                        mb.AddOption("Sorry, but no.", OnRejectClicked);
                        break;
                    case DialogueResults.AcceptedOnce:
                        mb.ChangeMessage("*It asks if It can settle in the world already.*");
                        break;
                    case DialogueResults.RejectedOnce:
                        mb.ChangeMessage("*The raccoon creature looks sad now. Said that maybe other time he can return and ask.*");
                        mb.AddOption("Close", Dialogue.EndDialogue);
                        break;
                }
            }
            return mb;
        }

        private void OnAcceptClicked()
        {
            result = DialogueResults.AcceptedOnce;
            MessageDialogue mb = new MessageDialogue("*It got very happy after I said that It can move to my world, and said that his name is [name].*");
            mb.AddOption("Welcome. I'm [playername].", Dialogue.LobbyDialogue);
            mb.RunDialogue();
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
        }

        private void OnRejectClicked()
        {
            result = DialogueResults.RejectedOnce;
            MessageDialogue mb = new MessageDialogue("*It got saddened after hearing my refusal. But says that wont feel bad for that. He told you that you can call him anytime, if you change your mind, and that his name is [name].*");
            mb.AddOption("Close", Dialogue.EndDialogue);
            mb.RunDialogue();
        }

        public enum DialogueResults : byte
        {
            None,
            AcceptedOnce,
            RejectedOnce
        }
    }
}