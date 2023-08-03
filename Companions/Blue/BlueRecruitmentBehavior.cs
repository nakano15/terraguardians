using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Blue
{
    public class BlueRecruitmentBehavior : BehaviorBase
    {
        const byte Bonfire_SearchFor = 0, Bonfire_Found = 1, Bonfire_NonExisting = 2;
        private byte FoundBonfire = 0;
        private float BonfireX = -1, BonfireY = -1;
        private RecruitmentStage Stage = RecruitmentStage.DistractedOnBonfire;
        private Player SpottedPlayer = null;

        public override string CompanionNameChange(Companion companion)
        {
            return "Wolf Guardian";
        }

        private bool ScanForBonfires(Companion blue)
        {
            int TileX = (int)(blue.Center.X * (1f / 16)),
                TileY = (int)(blue.Bottom.Y * (1f / 16));
            for(int x = -16; x < 16; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    if(WorldGen.InWorld(TileX + x, TileY + y))
                    {
                        Tile t = Main.tile[TileX + x, TileY + y];
                        if (t.HasTile && t.TileType == Terraria.ID.TileID.Campfire)
                        {
                            int FrameX = t.TileFrameX % 54, FrameY = t.TileFrameY % 36;
                            if(FrameX < 18)
                                TileX++;
                            if(FrameX > 18)
                                TileX--;
                            if (FrameY < 18)
                                TileY++;
                            BonfireX = TileX * 16 + 8;
                            BonfireY = TileY * 16;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void Update(Companion companion)
        {
            if (FoundBonfire == Bonfire_SearchFor)
            {
                if(ScanForBonfires(companion))
                {
                    FoundBonfire = Bonfire_Found;
                }
                else
                {
                    FoundBonfire = Bonfire_NonExisting;
                }
            }
            if (Companion.Behaviour_AttackingSomething)
                return;
            if (Companion.Behaviour_InDialogue)
            {
                companion.MoveLeft = companion.MoveRight = false;
            }
            else if(Stage == RecruitmentStage.DistractedOnBonfire)
            {
                SpottedPlayer = ViewRangeCheck(companion, companion.direction);
                if(SpottedPlayer != null)
                {
                    Stage = RecruitmentStage.SpottedPlayer;
                    if(PlayerMod.PlayerHasCompanion(SpottedPlayer, companion))
                    {
                        companion.SaySomething("*" + SpottedPlayer.name + "? " + SpottedPlayer.name+ ", over here.*");
                    }
                    else
                    {
                        companion.SaySomething("*Huh? Who are you? Are you friendly? Can you come here?*");
                    }
                }
                else
                {
                    WalkToCampfire(companion);
                }
            }
            else if(Stage == RecruitmentStage.SpottedPlayer)
            {
                if (MathF.Abs(SpottedPlayer.Center.X - companion.Center.X) > 500 || 
                    MathF.Abs(SpottedPlayer.Center.Y - companion.Center.Y) > 400)
                {
                    companion.SaySomething("*"+(SpottedPlayer.Male ? "He" : "She")+" went away...*");
                    SpottedPlayer = null;
                    Stage = RecruitmentStage.DistractedOnBonfire;
                    if(BonfireX < companion.Center.X)
                        companion.direction = -1;
                    else
                        companion.direction = 1;
                }
                else
                {
                    if(SpottedPlayer.Center.X < companion.Center.X)
                        companion.direction = -1;
                    else
                        companion.direction = 1;
                }
            }
            else
            {
                WalkToCampfire(companion);
            }
        }

        private void WalkToCampfire(Companion companion)
        {
            float DistanceFromBonfire = BonfireX - companion.Center.X;
            if(MathF.Abs(DistanceFromBonfire) > 40)
            {
                if(DistanceFromBonfire < 0)
                    companion.MoveLeft = true;
                else
                    companion.MoveRight = true;
                companion.WalkMode = true; //tModLoader is really sure that this is null
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            switch(Stage)
            {
                case RecruitmentStage.DistractedOnBonfire:
                    {
                        if(companion.Center.X < MainMod.GetLocalPlayer.Center.X)
                        {
                            companion.direction = 1;
                            companion.velocity.X = -3.5f;
                        }
                        else
                        {
                            companion.direction = -1;
                            companion.velocity.X = 3.5f;
                        }
                        companion.velocity.Y = -7.5f;
                        companion.SaySomething("*Aaaahhh!!*");
                        MessageDialogue mb = new MessageDialogue("*Aaaahhh!!*");
                        if(PlayerMod.PlayerHasCompanion(Main.LocalPlayer, companion))
                        {
                            mb.AddOption("Did I scare you again?", PostScareMetMessage);
                        }
                        else
                        {
                            mb.AddOption("Continue", PostScareNotMetMessage);
                        }
                        return mb;
                    }
                case RecruitmentStage.SpottedPlayer:
                    {
                        if(!PlayerMod.PlayerHasCompanion(Main.LocalPlayer, companion))
                        {
                            MessageDialogue mb = new MessageDialogue("*Hi. I never expected finding someone else around, even more someone like you. Are you here for camping?*");
                            mb.AddOption("No, I'm not.", FormalDialogueStart);
                            return mb;
                        }
                        else
                        {
                            MessageDialogue mb = new MessageDialogue("*[playername]?! I am so glad to see you again.*");
                            mb.AddOption("Equally.", FormalPlayerMetPreviouslyDialogue);
                            return mb;
                        }
                    }
                case RecruitmentStage.HerRequest:
                    {
                        MessageDialogue mb = new MessageDialogue("Hm.. You said you were an adventurer, right..?");
                        mb.AddOption("Yes, I did.", FormalDialoguePostTellingYoureAdventure);
                        return mb;
                    }
            }
            return base.ChangeStartDialogue(companion);
        }

        private void FormalDialogueStart()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*Then, what are you doing here? Are you an adventurer?*", "Yes, I am.");
            md.AddDialogueStep("*I knew it! You're probably trying to explore the most as possible of this world then.*", "That's right");
            md.AddOption("That's right.", FormalDialoguePostTellingYoureAdventure);
            md.RunDialogue();
        }

        private void FormalDialoguePostTellingYoureAdventure()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("Hm... I wonder...", "Huh?");
            md.AddDialogueStep("*Say, Terrarian. Would you mind if I stayed on this world for a while?*");
            md.AddOption("Yes, you can stay.", OnAcceptHerStaying);
            md.AddOption("No way.", OnRejectHerStaying);
            md.RunDialogue();
            Stage = RecruitmentStage.HerRequest;
        }

        private void FormalPlayerMetPreviouslyDialogue()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*You know, if you ever need my aid, I will be here, alright?*");
            md.AddOption("Thank you, "+Dialogue.Speaker.GetRealName+".", Dialogue.LobbyDialogue);
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            md.RunDialogue();
        }

        private void OnAcceptHerStaying()
        {
            MessageDialogue md = new MessageDialogue("*Yay! In case you need company on your adventures, you can call me.\nYou can call me " + Dialogue.Speaker.GetRealName + ".*");
            md.AddOption("I am [playername]. Welcome.", Dialogue.LobbyDialogue);
            md.RunDialogue();
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            WorldMod.AllowCompanionNPCToSpawn(Dialogue.Speaker);
        }

        private void OnRejectHerStaying()
        {
            MessageDialogue md = new MessageDialogue("*Aww... I'll be leaving then... Anyways, you seem like a good person, so feel free to call me, whenever you change your mind.\nI wouldn't mind coming over in case you need help on your adventures.\nYou can call me " + Dialogue.Speaker.GetRealName + ".*");
            md.AddOption("I am [playername]. Thanks.", Dialogue.LobbyDialogue);
            md.RunDialogue();
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
        }

        private void PostScareNotMetMessage()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*Don't sneak behind me again!*", "I'm sorry.");
            md.AddDialogueStep("*I nearly sliced you in half with my sword because of that!*", "I said that I'm sorry.");
            md.AddDialogueStep("*Okay... I'll try calming down...*");
            md.AddDialogueStep("*I'm sorry... It's just... You gave me a really big scare.*", "O... Okay.");
            md.AddDialogueStep("*Say, what are you doing here? Are you camping too?*", "No, I'm exploring the world.");
            md.AddDialogueStep("*Oh.. So... You're an adventurer? Interesting.*");
            md.AddOption("Continue", FormalDialoguePostTellingYoureAdventure);
            md.RunDialogue();
        }

        private void PostScareMetMessage()
        {
            MultiStepDialogue md = new MultiStepDialogue();
            md.AddDialogueStep("*You really enjoy scaring me, don't you?*", "I'm sorry.");
            md.AddDialogueStep("*You nearly made my heart jump out of my mouth.*", "Again, I said I'm sorry.");
            md.AddDialogueStep("*Anyways, you know that if you ever need my help, I will be around.*", "Thank you.");
            md.AddDialogueStep("*And please, don't scare me again.*");
            md.AddOption("I'll try.", Dialogue.LobbyDialogue);
            Dialogue.Speaker.PlayerMeetCompanion(MainMod.GetLocalPlayer);
            md.RunDialogue();
        }

        private enum RecruitmentStage : byte
        {
            DistractedOnBonfire = 0,
            SpottedPlayer = 1,
            HerRequest = 3,
            JustMetDialogue = 100,
            PreviouslyRecruitedDialogue = 155
        }
    }
}
