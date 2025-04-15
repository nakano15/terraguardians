using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Malisha
{
    public class MalishaPreRecruitBehavior : PreRecruitNoMonsterAggroBehavior
    {
        int AiStage = 0;
        int DialogueTime = 0;
        Companion companion;
        new Player Target = null;

        public override string CompanionNameChange(Companion companion)
        {
            return "Panther Guardian";
        }

        public override void Update(Companion companion)
        {
            this.companion = companion;
            if (AiStage > 1)
            {
                if (Target == null || !Target.active)
                {
                    AiStage = 1;
                    SayMessage("*Funny, where did the Terrarian I was talking with to went?*");
                    Target = null;
                }
                else if (Target.dead)
                {
                    AiStage = 1;
                    SayMessage("*Well, that was an awful sight.*");
                    Target = null;
                }
            }
            switch(AiStage)
            {
                case 0:
                    {
                        AiStage = 1;
                    }
                    break;
                case 1:
                    {
                        Target = ViewRangeCheck(companion, companion.direction);
                        if (Target == null)
                        {
                            WanderAI(companion);
                        }
                        else
                        {
                            if (PlayerMod.PlayerHasCompanion(Target, companion))
                            {
                                DialogueTime = SayMessage("*Hey, It's you again.*");
                                AiStage = 150;
                            }
                            else
                            {
                                AiStage = 2;
                                DialogueTime = SayMessage("*A Terrarian! I must have really arrived.*");
                            }
                            companion.FaceSomething(Target);
                        }
                    }
                    break;
                case 2:
                    {
                        companion.FaceSomething(Target);
                        if (DialogueTime > 0)
                            DialogueTime--;
                        else
                        {
                            switch(GetPlayerSetType(Target))
                            {
                                default:
                                    DialogueTime = SayMessage("*Wait, why is It using clothes?*");
                                    break;
                                case SetTypes.DryadSet:
                                    DialogueTime = SayMessage("*Look at how "+PlayerMod.GetPronounLower(Target, PronounTypes.Nominative)+" is dressed! This must be the right place.*");
                                    break;
                            }
                            AiStage = 3;
                        }
                    }
                    break;
                case 3:
                    {
                        if (DialogueTime > 0)
                            DialogueTime--;
                        else
                        {
                            float Distance = Target.Center.X - companion.Center.X;
                            if (System.Math.Abs(Distance) > 96)
                            {
                                companion.WalkMode = true;
                                if (Distance > 0)
                                    companion.MoveRight = true;
                                else
                                    companion.MoveLeft = true;
                            }
                        }
                    }
                    break;
                case 100:
                    Target = ViewRangeCheck(companion, companion.direction);
                    WanderAI(companion);
                    break;
                case 150:
                    companion.FaceSomething(Target);
                    if (DialogueTime > 0)
                        DialogueTime --;
                    else
                    {
                        DialogueTime = SayMessage("*I think this isn't a naturalist colony, either, right?*");
                        AiStage = 151;
                    }
                    break;
                case 151:
                    companion.FaceSomething(Target);
                    if (DialogueTime > 0)
                        DialogueTime --;
                    else
                    {
                        DialogueTime = SayMessage("*Well, whatever. I'm here If you need my knowledge. Or feel like wanting to be a guinea pig. You pick.*");
                        AiStage = 152;
                    }
                    break;
                case 152:
                    companion.FaceSomething(Target);
                    if (DialogueTime > 0)
                        DialogueTime --;
                    else
                    {
                        WorldMod.AddCompanionMet(companion);
                    }
                    break;
                default:
                    if (!Dialogue.InDialogue)
                    {
                        DialogueTime = SayMessage("*How rude! Speak to me when possible then.*");
                        AiStage = 100;
                    }
                    break;
            }
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            if (AiStage == 3)
            {
                AiStage = 4;
            }
            if (AiStage == 100)
                AiStage = 4;
            return GetMessageByStep();
        }

        SetTypes GetPlayerSetType(Player player)
        {
            if (player.body == Terraria.ID.ArmorIDs.Body.DryadCoverings ||
                player.legs == Terraria.ID.ArmorIDs.Legs.DryadLoincloth)
                return SetTypes.DryadSet;
            return SetTypes.None;
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return AiStage == 1 || AiStage == 100 || AiStage == 3 || AiStage == 4;
        }

        int SayMessage(string Message)
        {
            return companion.SaySomething(Message);
        }

        enum SetTypes : byte
        {
            None = 0,
            DryadSet = 1
        }

        public MessageBase GetMessageByStep()
        {
            MessageDialogue md = new MessageDialogue();
            switch(AiStage)
            {
                case 1:
                    md.ChangeMessage("*Oh, at Terrarian. This must be the place of the naturalist colony I've heard then.*");
                    md.AddOption("A what?", ButtonClickAction);
                    break;
                case 3:
                case 4: //Initial message.
                    switch(GetPlayerSetType(MainMod.GetLocalPlayer))
                    {
                        default:
                            md.ChangeMessage("*Funny, I thought this was a naturalist colony world, why are you using clothes?*");
                            break;
                        case SetTypes.DryadSet:
                            md.ChangeMessage("*I thought this was a naturalist world, why are you using leaves?*");
                            break;
                    }
                    md.AddOption("This isn't a naturalist colony.", ButtonClickAction);
                    break;
                case 5: //When player doesn't has Dryad outfit and Familiar wig. Jump to 8 after that.
                    md.ChangeMessage("*Then why the TerraGuardians aren't using clothes?*");
                    md.AddOption("I don't know", ButtonClickAction);
                    break;
                case 6: //When player is using dryad outfit and familiar wig.
                    md.ChangeMessage("*Then why you are using such outfit?*");
                    md.AddOption("Hey! That's my style.", ButtonClickAction);
                    break;
                case 7: //Jump to 9
                    md.ChangeMessage("*And the TerraGuardians aren't using clothes.*");
                    md.AddOption("We are not naturalists. At least I'm not.", ButtonClickAction);
                    break;
                case 8:
                    md.ChangeMessage("*Then you're the one who's not in the trend.*");
                    md.AddOption("There's no trend. This isn't a naturalist colony.", ButtonClickAction);
                    break;
                case 9:
                    md.ChangeMessage("*That's quite sad, I was thinking of taking some vacations here. Oh... Well.*");
                    md.AddOption("If that's it, you may stay here.", ButtonClickAction);
                    break;
                case 10:
                    md.ChangeMessage("*I may? Alright, I'll move in to some empty house. Maybe I'll be able to do some experiements too.*");
                    md.AddOption("Uh... Experiments?", ButtonClickAction);
                    break;
                case 11:
                    md.ChangeMessage("*Don't worry, maybe an explosion or two may happen, or familiar critters running around but nothing too serious.*");
                    md.AddOption("No, wait.", ButtonClickAction);
                    break;
            }
            return md;
        }

        public void ButtonClickAction()
        {
            switch(AiStage)
            {
                default:
                    AiStage++;
                    break;
                case 1:
                    AiStage = 4;
                    break;
                case 3:
                case 4:
                    if (GetPlayerSetType(MainMod.GetLocalPlayer) != SetTypes.None)
                    {
                        AiStage = 6;
                    }
                    else
                    {
                        if (PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer) == null)
                            AiStage = 9;
                        else
                            AiStage = 5;
                    }
                    break;
                case 5:
                    AiStage = 8;
                    break;
                case 6:
                    if (PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer) == null)
                        AiStage = 9;
                    else
                        AiStage++;
                    break;
                case 7:
                    AiStage = 9;
                    break;
                case 11:
                    {
                        PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, companion);
                        WorldMod.AddCompanionMet(CompanionDB.Malisha);
                        WorldMod.AllowCompanionNPCToSpawn(CompanionDB.Malisha);
                        Dialogue.LobbyDialogue("*I'm Malisha, by the way. I'll try enjoying my time here.*");
                    }
                    return;
            }
            GetMessageByStep().RunDialogue();
        }
    }
}