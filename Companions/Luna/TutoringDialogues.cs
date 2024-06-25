using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Luna
{
    public class TutoringDialogues
    {
        public static void StartTutoringDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Yes, [nickname]? What do you need help with?*");
            SetDialogueTopics(md);
            md.RunDialogue();
        }

        private static void ReturnToDialogueLobby()
        {
            MessageDialogue md = new MessageDialogue("*Do you want to know about anything else?*");
            SetDialogueTopics(md);
            md.RunDialogue();
        }

        private static void SetDialogueTopics(MessageDialogue md)
        {
            md.AddOption("Tell me about the TerraGuardians.", AboutTerraGuardians);
            md.AddOption("Tell me what is a Bond.", AboutTheBond);
            md.AddOption("I want to know about befriending TerraGuardians.", AboutBefriendingTerraGuardians);
            md.AddOption("About Follower Companions", AboutLeadingTerraGuardians);
            //md.AddOption("About giving Orders.", AboutOrdersMain);
            //md.AddOption("About Buddy TerraGuardians.", AboutBuddyGuardian);
            md.AddOption("About Skill.", AboutTerraGuardiansSkills);
            md.AddOption("About TerraGuardians living in my world.", AboutCompanionLivingInTheWorld);
            md.AddOption("About other kinds of TerraGuardians.", AboutDemiTerraGuardians);
            md.AddOption("About Bond-Merge.", AboutTerraGuardiansBondMerge);
            
            md.AddOption("I don't have any other question.", EndTutoring);
        }

        private static void AboutTerraGuardiansBondMerge()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Ah, you want to know about Bond-Merge? Very well, I'll tell you.*",
                "*Companions that make use of Bond, like TerraGuardians, can allow other creatures to Bond-Merge with them.*",
                "*The Bond-Merge allows that creature to take full control of the one merged with's body.*",
                "*Due to how that work, the Companions will only allow Bond-Merging with those they trust the most.*",
                "*The ones merged with a TerraGuardian can still speak with them through their mind, but the one whose body is being controlled can't do much else.*",
                "*Anyone trying Bond-Merge should be careful too, since if the one whose Body is merged on dies, the one controlling will die too.*",
                "*Keep that in mind if you happen to have a companion entrust you with themself.*"
            });
            md.AddOption("Got it.", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void AboutTerraGuardiansSkills()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string
            []
            {
                "*Companions have different skills that grow stronger as they do activities related to them.*",
                "*As their skills get stronger, they gain benefits on their status based on their skill levels.*",
                "*Having a companion follow you on your travels, will make them get stronger at what they do during it.*"
            });
            md.AddOption("Return", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void AboutTerraGuardians()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string
            []
            {
                "*We TerraGuardians are inhabitants of the Ether Realm.*",
                "*Many Terrarians say that we look like a mix of human with animal, but we don't see ourselves like that.*",
                "*There are villages and cities in the Ether Realm too, with TerraGuardians living in them, but since recently, people have been coming to the Terra Realm too.*",
                "*We mostly are good people, so you mostly won't have trouble when meeting a new TerraGuardian. I think many of them actually like meeting Terrarians.*",
                "*TerraGuardians like me, speak with other creatures through creating a bond with them. That's how I'm speaking to you right now, and why you can understand me.*",
                "*There are some sub-types of TerraGuardians that don't speak using bond, so they speak verbally just like you. The reason for that is that they are somehow... \'different\'...*"
            });
            md.AddOption("Return", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void AboutBefriendingTerraGuardians()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string
            []
            {
                "*Just like anyone else, TerraGuardians like receiving attention.*",
                "*Living in a comfortable house, and helping when they're in need is a good way of making them like you more.*",
                "*As you become better friends with them, they will not mind doing some new things for you. Like letting you ride on their shoulder, or maybe even bond-merge with you*"
            });
            md.AddOption("Return", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void AboutLeadingTerraGuardians()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string
            []
            {
                "*Depending on how much of a friend you are with someone, they may agree to follow you on your adventures. Some of them may not even mind joining your adventures at all, regardless of whether they know you or not.*",
                "*You can also give them orders during your adventures, so they can take specific actions that you may find the need.*",
                "*Pay attention to your group size. A companion may feel uncomfortable joining you if there are too many people in your group. Currently, you can "+(MainMod.MaxCompanionFollowers)+" members in your group at once.*"
            });
            md.AddOption("Return", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void AboutOrdersMain()
        {
            OrdersInfoHub("*So, which order do you want to know about?*");
        }

        private static void ReturnToOrdersQuestion()
        {
            OrdersInfoHub("*So, which order do you want to know about?*");
        }

        private static void OrdersInfoHub(string Text)
        {
            MessageDialogue md = new MessageDialogue(Text);
            md.AddOption("What are Orders?", OrdersAboutOrders);
            md.AddOption("Pull to Me", OrdersAboutPullToMe);
            md.AddOption("Orders", OrdersAboutOrdersBehavior);
            md.AddOption("Action", OrdersAboutAction);
            md.AddOption("Item", OrdersAboutItemAction);
            md.AddOption("Interaction", OrdersAboutInteraction);
            md.AddOption("Tactics", OrdersAboutTactics);
            md.AddOption("That's all.", OrdersThatsAll);
            md.RunDialogue();
        }

        private static void OrdersAboutOrders()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*You can give orders to TerraGuardians at any time they are following you.*",
                "(You neeed to have setup a key for calling the Orders list, on the control settings of the game.)",
                "*That way you can tell your companions to do things for you or change how they should behave in combat*",
                "(You can navigate through the orders by pressing the number key shown to the left of the order.)",
                "*Not all orders may be possible to be used on a companion, either due to limitations or not being friends enough to use it.*",
                "*They are very useful on your adventure. You should give it a try.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutPullToMe()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Pull to Me forces pull companions to your position by a binding chain. They are useful if you want to take them off places they can't reach you, or for other reasons.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutOrdersBehavior()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Orders allow you to change how they will behave. They can follow you, wait, avoid combat, or more.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutAction()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Action allows you to tell companions to take some action. The Free Control action is useful when you're mounted on them, or vice versa.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutItemAction()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Item action tells them to use specific items. It's useful when you're going to prepare to face strong challenges.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutInteraction()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Interactions contain orders you tell the companion to do with you. Asking them to let you mount on their shoulder is an example.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersAboutTactics()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Tactic allows you to force change your companion combat behavior until you change that again. That will not alter their set behavior and will be reset back to normal upon leaving the world.*"
            });
            md.AddOption("Ok", ReturnToOrdersQuestion);
            md.RunDialogue();
        }

        private static void OrdersThatsAll()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*That's all your questions about orders? Alright.*"
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }
        
        private static void AboutLeaderGuardians()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*The leading companion has an important role in your group.*",
                "*They are always the first companion you have summoned.*",
                "*Whenever you give an order to the group, that only one can execute, they will be the first ones to try using it.*",
                "*They are also the only ones you can take control of, so if you want to control a TerraGuardian, you need them as leader guardians.*",
                "*There is no Weight cost for summoning one, and when they're given permission to also take action in your adventures, half of your following companions will follow them.*",
                "(A second player can take control of the leader TerraGuardian, as long as there are 2 controllers plugged in, and they use the second. The controller index for the second player can be changed on mod options. Setting it to 2 works.)",
                "*That's only just a few importance that a leader guardian has on your group.*"
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }
        
        private static void AboutTheBond()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*TerraGuardians like me can only speak with other creatures by creating a bond with them.*",
                "*Once we create a bond with a creature, we can use it to express ourselves to the person, and also allow them to understand what we say.*",
                "*We can still understand you if you express yourself in other ways, but if you want to speak in private, you can also use it too.*",
                "*You Terrarians say that it's like as if our voices come from inside your head, but don't worry, we can't read your mind, so don't fear that.*",
                "*Once the bond is created, It can't be broken, unless either dies.*",
                "*In a number of occasions, a bond may be created accidentally, so a TerraGuardian may not even know you're listening to them.*",
                "*It is said that we can speak with each other through the bond from far distances too, but It seems to only happen on a number of occasions.*",
                "*A bond can be strengthened too. Maybe if you be more friends of the TerraGuardian you want to strengthen bonds? Some benefits may be offered if you do that.*"
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }
        
        private static void AboutCompanionLivingInTheWorld()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*Depending on the companion, they may move to your world if you ask them to.*",
                "*Having a companion live in the world with you, will allow you to keep contact with it, and make use of its services, if they have any.*",
                "*It's always good to furnish their houses since they can make use of the furniture.*",
                "*If there are chairs in their houses, or around them, they will use them whenever they need to rest. And when it's their sleep time, they will use beds.*",
                "*Their friendship towards you grows passively more as they use furniture since you cared about them to build them such houses.*",
                "*The amount of companions that can live in the world is limited, so you can try to alternate who will spend time in your world.*",
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }
        
        private static void AboutBuddyGuardian()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*That's like... The highest honor a TerraGuardian could have.*",
                "*When a TerraGuardian gets someone as their buddy, it intensifies the bond between both.*",
                "*Both the TerraGuardian and the one they created a Bond with, will grow stronger as their friendship rises.*",
                "*Due to being a Buddy to that TerraGuardian, both have their fates sealed together, so one must not leave the other company.*",
                "*Having more TerraGuardians tagging along will weaken the bond between the two, making the benefits of the friendship drop.*",
                "*If you see someone who has picked a TerraGuardian as a buddy, or a TerraGuardian who has been picked as someone's buddy, be sure to give them their congratulations.*",
                "*Just thinking of this makes my heart pound. I wonder if someday I will be picked as a buddy by someone?*",
                "*Oh, sorry.. I guess I got carried away by my thoughts. Do you have any other questions?*"
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }
        
        private static void AboutDemiTerraGuardians()
        {
            MultiStepDialogue md = new MultiStepDialogue(new string[]
            {
                "*There are some people that aren't 100% TerraGuardians.*",
                "*They don't have all the characteristics of a TerraGuardian, but somehow they still are one, in parts.*",
                "*For example, their appearance may be different, or they can express themselves verbally just like you.*",
                "*The way they surged may be of a variety of ways. Either they were created in some other way, or they're the result of the time Ether and Terra Realm citizens lived together, or maybe even any other reason.*",
                "*They generally have their own distinctive names, but we can simply distinguish them as TerraGuardians as well.*",
                "(You can see the group a TerraGuardian belongs to on the list where the met companions can be seen. There you can also find if they're recognized as TerraGuardians or not. Just check the icons.)"
            });
            md.AddOption("Ok", ReturnToDialogueLobby);
            md.RunDialogue();
        }

        private static void EndTutoring()
        {
            Dialogue.ChatDialogue("*That's enough questions? Alright.\n" +
                "Do you want to speak about something else?*");
        }
    }
}
