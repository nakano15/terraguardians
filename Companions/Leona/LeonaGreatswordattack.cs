using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Leona
{
    internal class LeonaGreatswordAttack : SubAttackBase
    {
        public override string Name => "Greatsword Slice";
        public override string Description => "Leona will use the Greatsword she's carrying to attack a foe.";
        public override SubAttackData GetSubAttackData => new LeonaGreatswordAttackData();
        public override float Cooldown => 5f;

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            LeonaCompanion Leona = User as LeonaCompanion;
            if (User.TargettingSomething && Leona.HoldingSword)
            {
                Vector2 Distance = (User.Target.Center - (Leona.Bottom - Vector2.UnitY * Leona.Base.Height * Leona.Scale * 0.5f));
                int TargetWidth = User.Target.width, TargetHeight = User.Target.height;
                const float CheckDistance = 140;
                /*if (Main.rand.Next(3) == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 Pos = Leona.Bottom - Vector2.UnitY * Leona.Base.Height * Leona.Scale * 0.5f;
                        switch(i)
                        {
                            case 0:
                                Pos.X -= CheckDistance * User.Scale;
                                Pos.Y -= CheckDistance * User.Scale;
                                break;
                            case 1:
                                Pos.X += CheckDistance * User.Scale;
                                Pos.Y -= CheckDistance * User.Scale;
                                break;
                            case 2:
                                Pos.X -= CheckDistance * User.Scale;
                                Pos.Y += CheckDistance * User.Scale;
                                break;
                            case 3:
                                Pos.X += CheckDistance * User.Scale;
                                Pos.Y += CheckDistance * User.Scale;
                                break;
                        }
                        Dust.NewDust(Pos, 1, 1, 5);
                    }
                }*/
                if (MathF.Abs(Distance.X) < TargetWidth + CheckDistance * User.Scale && MathF.Abs(Distance.Y) < TargetHeight + CheckDistance * User.Scale)
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnBeginUse(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            Data.SwingDuration = (int)(MathF.Max(3, 32 * User.GetAttackSpeed<MeleeDamageClass>()));
            Data.Damage = 20;
        }

        public override void Update(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            if (Data.GetTime >= Data.SwingDuration)
            {
                Data.EndUse();
            }
            LeonaCompanion Leona = User as LeonaCompanion;
            if (Leona.velocity.Y == 0)
                Leona.MoveLeft = Leona.MoveRight = Leona.ControlJump = Leona.MoveDown = false;
            else if (Leona.Target != null && Leona.CompanionHasControl)
            {
                float Distance = Leona.Target.Center.X - Leona.Center.X;
                if (MathF.Abs(Distance) < 60 * Leona.Scale)
                {
                    if (Distance > 0)
                    {
                        Leona.MoveLeft = true;
                        Leona.MoveRight = false;
                    }
                    else
                    {
                        Leona.MoveLeft = false;
                        Leona.MoveRight = true;
                    }
                }
            }
            Leona.controlUseItem = false;
            Data.AnimationPercentage = Data.GetTime * (1f / Data.SwingDuration);
            Data.SwordPercentage = MathF.Min(Data.AnimationPercentage * 1.3f, 1);
            Data.SwordPercentage = Data.SwordPercentage * Data.SwordPercentage;
            //Data.AnimationPercentage *= Data.AnimationPercentage;
            Rectangle ItemHitbox = new Rectangle((int)User.Center.X, (int)User.Center.Y, (int)(80 * User.Scale), (int)(80 * User.Scale));
            if (Data.SwordPercentage > 0.666f)
            {
                //ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.3f), (int)(ItemHitbox.Height * 0.3f));
                ItemHitbox.X += (int)(ItemHitbox.Width * 0.8f * User.direction);
                ItemHitbox.Y -= (int)(ItemHitbox.Height * 0.8f * User.gravDir);
            }
            else if (Data.SwordPercentage > 0.333f)
            {
                ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.6f), (int)(ItemHitbox.Height * 0.4f));
                ItemHitbox.X += (int)(ItemHitbox.Width * User.direction);
            }
            else 
            {
                //ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.6f), (int)(ItemHitbox.Height * 0.2f));
                ItemHitbox.Y -= (int)(ItemHitbox.Height * 0.8f * User.gravDir);
            }
            ItemHitbox.X -= (int)(ItemHitbox.Width * 0.5f * Leona.direction);
            ItemHitbox.Y -= (int)(ItemHitbox.Height * 0.5f);
            /*for (int i = 0; i < 4; i++)
            {
                Vector2 Position = new Vector2(ItemHitbox.X, ItemHitbox.Y);
                if (i == 1 || i == 3) Position.X += ItemHitbox.Width;
                if (i >= 2) Position.Y += ItemHitbox.Height;
                Dust.NewDust(Position, 1, 1, 5);
            }*/
            HurtCharactersInRectangle(User, ItemHitbox, Data.Damage, ModContent.GetInstance<MeleeDamageClass>(), 30, Data);
        }

        public override void UpdateAnimation(Companion User, SubAttackData data)
        {
            LeonaCompanion Leona = User as LeonaCompanion;
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            short AnimationFrame = User.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrameFromPercentage(Data.AnimationPercentage);
            User.BodyFrameID = User.ArmFramesID[0] = User.ArmFramesID[1] = AnimationFrame;
            Leona.SwordRotation = MainMod.Deg2Rad * ((30 + 245f * Data.SwordPercentage) * User.direction);
        }

        public class LeonaGreatswordAttackData : SubAttackData
        {
            public float AnimationPercentage = 1f;
            public float SwordPercentage = 1f;
            public int SwingDuration = 12;
            public int Damage = 20;
        }
    }
}