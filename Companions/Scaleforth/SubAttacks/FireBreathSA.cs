using terraguardians;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.Companions.Scaleforth.SubAttacks;

public class FireBreathSubAttack : SubAttackBase
{
    public override string Name { get { return "Fire Breath"; }}
    public override string Description { get { return "Scaleforth breathes fire at what he's aiming at."; }}
    public override float Cooldown { get { return 50; } }
    public override int ManaCost => 18;
    public override bool AllowItemUsage => true;

    //Debug later.
    Vector2 FirebreathStandingPosition = new Vector2(44, 28 + 28), //47
        FirebreathFlyingPosition = new Vector2(64, 41 + 28); //67

    byte Step = 0;
    byte Time = 0;
    int Damage = 10;


    public override bool AutoUseCondition(Companion User, SubAttackData Data)
    {
        return User.TargettingSomething && (User.GetNearestHostilesCount >= 5 || (User.Target is NPC && Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[(User.Target as NPC).type])) && (User.Center - User.Target.Center).Length() < 200;
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
                Projectile.NewProjectile(User.GetSource_Misc("SubAttack"), SpawnPosition + User.velocity, FiringDirection * 9f, 85, Damage, .3f, User.whoAmI);
                int Dir = MathF.Sign(User.GetAimedPosition.X - User.Center.X);
                if (Dir != 0 && Dir != User.direction)
                {
                    User.ChangeDir(Dir);
                }
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
        ReplaceFrameWithSAFrame(true, ref User.BodyFrameID);
        ReplaceFrameWithSAFrame(false, ref User.ArmFramesID[0]);
        ReplaceFrameWithSAFrame(false, ref User.ArmFramesID[1]);
    }

    void ReplaceFrameWithSAFrame(bool IsBodyFrame, ref short Frame)
    {
        if (Frame <= 16)
        {
            if (Frame < 2 || (IsBodyFrame && Frame >= 13 && Frame < 17))
            {
                Frame = 28;
            }
            else if (Frame >= 2 && Frame < 13)
            {
                Frame += 27;
            }
        }
    }
}