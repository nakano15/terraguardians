using terraguardians;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Companions.Scaleforth;

public class FireBreathSubAttack : SubAttackBase
{
    public override string Name {get { return "Fire Breath"; }}
    public override string Description { get { return "Scaleforth breathes fire at what he's aiming at."; }}
    //public override float Cooldown { get { return 0; } }
    //public override int ManaCost => 18;
    public override bool AllowItemUsage => true;

    //Debug later.
    Vector2 FirebreathStandingPosition = new Vector2(44, 28 + 9), //47
        FirebreathFlyingPosition = new Vector2(64, 41 + 9); //67

    byte Step = 0;
    byte Time = 0;
    int Damage = 10;


    public override bool AutoUseCondition(Companion User, SubAttackData Data)
    {
        return base.AutoUseCondition(User, Data);
    }

    public override void OnBeginUse(Companion User, SubAttackData Data)
    {
        Step = 0;
        Time = 0;
    }

    public override void Update(Companion User, SubAttackData Data)
    {
        Time++;
        if (Step < 16)
        {
            if (Time >= 6)
            {
                Time = 0;
                if (Step == 0)
                {
                    Damage = (int)MathF.Max(10, GetHighestWeaponDamage(User, DamageClass.Magic));
                }
                Step++;
                Vector2 SpawnPosition = User.velocity.Y != 0 ? FirebreathFlyingPosition : FirebreathStandingPosition;
                SpawnPosition.X *= User.direction;
                SpawnPosition.Y = -User.Base.SpriteHeight + SpawnPosition.Y;
                SpawnPosition = User.Bottom + SpawnPosition * User.Scale;
                Vector2 FiringDirection = (User.GetAimedPosition - SpawnPosition).SafeNormalize(Vector2.UnitX * User.direction);
                Projectile.NewProjectile(User.GetSource_Misc("SubAttack"), SpawnPosition, FiringDirection * 9f, 85, Damage, .3f, User.whoAmI);
            }
        }
        else
        {
            if (Time >= 30)
            {
                Data.EndUse();
            }
        }
    }

    public override void UpdateAnimation(Companion User, SubAttackData Data)
    {
        ReplaceFrameWithSAFrame(ref User.BodyFrameID);
        ReplaceFrameWithSAFrame(ref User.ArmFramesID[0]);
        ReplaceFrameWithSAFrame(ref User.ArmFramesID[1]);
    }

    void ReplaceFrameWithSAFrame(ref short Frame)
    {
        if (Frame <= 12)
        {
            if (Frame >= 10)
            {
                Frame += 19;
            }
            else
            {
                Frame = 28;
            }
        }
    }
}