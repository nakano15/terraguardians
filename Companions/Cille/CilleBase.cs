using Terraria;
using Terraria.ModLoader;


namespace terraguardians.Companions.Cille
{
    /* i tried to work on her animation,but...it does not work out well. Sorry but you have to do it yourseft T.T*/
    public class CilleBase : TerraGuardianBase
    {
        public override string Name => "Cille";
        public override string Description => "A young person with a mysterious past, \nafraid of interacting with people.";
        public override int SpriteWidth => 96; 
        public override int SpriteHeight => 96;
        public override int FramesInRow => 20; 
        public override int Width => 24; 
        public override int Height => 90;
        
        public override Sizes Size => Sizes.Large;
        public override int Age => 21;
        public override Genders Gender => Genders.Female; //Allow you to setup the gender. Can be Male, Female, or Genderless.
        public override bool CanChangeGenders => false; //If can change gender. Doesn't change much right now, other than text color. Might be handy in the future.
        public override int InitialMaxHealth => 225; //To setup max health value, use this formula on your calculator: (InitialMaxHealth + (HealthPerLifeCrystal * 15) + (HealthPerLifeFruit * 20)). That will let you know the final max health of companion;
        public override int HealthPerLifeCrystal => 35;
        public override int HealthPerLifeFruit => 10;
        //public override int InitialMaxMana => 20; //For setting up max mana. Use this formula on your calculator to calculate final max mana: (InitialMaxMana + (ManaPerManaCrystal * 9))
        //public override int ManaPerManaCrystal => 20;
        public override bool CanCrouch => false; //If companion can crouch. Also used for some animations.
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange; //Default combat tactic of companion.
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer; //Sets which way the companion can mount/be mounted on, or if cannot.
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0 }; //Allow you to change what each friendship level unlocks.
        protected override CompanionDialogueContainer GetDialogueContainer => new CilleDialogues(); //I have split the companion dialogues to another file. Here, you initialize the object containing companion dialogues.

        public override void UpdateAttributes(Companion companion) //This updates whenever the companion status are reset. If you want to change their status, or give them other benefits, here is the place.
        {
            companion.Accuracy += 0.89f;
            
        }
        #region Animation
        protected override terraguardians.Animation SetStandingFrames => new terraguardians.Animation(0);
        protected override terraguardians.Animation SetWalkingFrames
        {
            get
            {
                terraguardians.Animation anim = new terraguardians.Animation();
                for (short i = 1; i <= 8; i++)
                {
                    anim.AddFrame(i, 24); //The default animation frame duration of companions walking animation is 24, if you want a 1.3 feel.
                }
                return anim;
            }
        }
        protected override terraguardians.Animation SetJumpingFrames => new terraguardians.Animation(9);
        protected override terraguardians.Animation SetItemUseFrames
        {
            get
            {
                terraguardians.Animation anim = new terraguardians.Animation();
                for (short i = 10; i <= 13; i++) //Normally, item use animations have 4 frames. Frames goes from upper left (arm preparing to slash from upwards) to lower right. Mind the frame times.
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override terraguardians.Animation SetSittingFrames => new terraguardians.Animation(14);
        protected override terraguardians.Animation SetChairSittingFrames => new terraguardians.Animation(14); //If a companion has animation for when sitting on a chair, instead.
        protected override terraguardians.Animation SetRevivingFrames => new terraguardians.Animation(15); //When KO system is implemented, this will be used when the companion is reviving.
        protected override terraguardians.Animation SetDownedFrames => new terraguardians.Animation(16); //KO animation
        protected override terraguardians.Animation SetBedSleepingFrames => new terraguardians.Animation(17);
        protected override terraguardians.Animation SetThroneSittingFrames => new terraguardians.Animation(18);
        protected override terraguardians.Animation SetPlayerMountedArmFrame => new terraguardians.Animation(14); //This is used to set the animation frame for when the player is mounted on the companion, or vice versa.
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers //To not clutter the mod with empty frames, this is used to assign which frames will have the body drawn in the foreground. Used in some cases, like when companion is mounted on player.
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection SetSittingPosition => new terraguardians.AnimationPositionCollection(new Microsoft.Xna.Framework.Vector2(16, 27), true); //The position where the companion ass is touching the chair seat. Add a frame for when its using the throne too, if necessary.
        protected override AnimationPositionCollection[] SetHandPositions //Assigns the holding position of each hand (like, item position, and others). Hand 0 is generally the main hand, which is the left (foreground one). Hand 1 is generally offhand.
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 12, 15);
                left.AddFramePoint2X(11, 20, 21);
                left.AddFramePoint2X(12, 21, 24);
                left.AddFramePoint2X(13, 18, 27);
                
                left.AddFramePoint2X(14, 16, 25);
                
                left.AddFramePoint2X(15, 18, 28);

                right.AddFramePoint2X(10, 17, 15);
                right.AddFramePoint2X(11, 22, 21);
                right.AddFramePoint2X(12, 23, 24);
                right.AddFramePoint2X(13, 20, 27);
                
                right.AddFramePoint2X(14, 22, 25);
                
                right.AddFramePoint2X(15, 21, 28);
                return new AnimationPositionCollection[]{ left, right };
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition //Hat position. Might need checking if it shows up on the right place, when I get the system working.
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Microsoft.Xna.Framework.Vector2(16, 21), true);
                anim.AddFramePoint2X(15, 18, 23);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(new Microsoft.Xna.Framework.Vector2(16, 27), true); //When player is mounted on companion, where they will be sitting at.
        protected override AnimationPositionCollection SetPlayerSittingOffset //Changes the player or companion position offset when sharing a chair. It is affected by mount style.
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(14, 0, -1);
                return anim;
            }
        }
        public override bool DrawBehindWhenSharingBed => true;
        #endregion
    }
}