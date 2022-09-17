using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions
{
    public class RococoBase : CompanionBase
    {
        public override string Name => "Rococo";
        public override string Description => "";
        public override int Age => 15;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override int Width => 28;
        public override int Height => 86;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override Animation WalkingFrames {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 22); //8
                return anim;
            }
        }
        public override Animation JumpingFrames => new Animation(9);
    }
}