using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using Terraria.ID;

namespace terraguardians.Companions.CaptainStench.Subattacks
{
    public class StenchPhantomAssaultSubAttack : SubAttackBase
    {
        public override string Name => "Phantom Assault";
        public override string Description => "";
        public override int ManaCost => 80;
        //public override float Cooldown => 20;

        int Frame = 0, Duration = 0;
        const int RushTime = 6;
        int Direction = 0;
        Vector2 RushDirection = Vector2.Zero;
        Vector2 HangPosition = Vector2.Zero;
        int Damage = 0;
        Rectangle ZipFrame = Rectangle.Empty;

        public override bool CanUse(Companion User, SubAttackData Data)
        {
            return base.CanUse(User, Data);
        }

        public override void OnBeginUse(Companion User, SubAttackData Data)
        {
            Damage = 0;
            for (int i = 0; i < 10; i++)
            {
                if (User.inventory[i].type > ItemID.None && User.inventory[i].damage > 0 && User.inventory[i].DamageType.CountsAsClass<MeleeDamageClass>())
                {
                    int ThisDamage = (int)(User.inventory[i].damage * ((float)User.inventory[i].useTime / 60));
                    if (ThisDamage > Damage)
                    {
                        Damage = ThisDamage;
                    }
                }
            }
            Damage = 50 + Damage * 2;
            Frame = 0;
            Duration = 0;
            Vector2 RushAim = User.AimDirection;
            if (RushAim.X > 0)
                Direction = 1;
            else if (RushAim.X < 0)
                Direction = -1;
            else
                Direction = User.direction;
            RushAim = RushAim.SafeNormalize(Vector2.UnitY);
            if (MathF.Abs(RushAim.X) > .4f && MathF.Abs(RushAim.Y) > .4f )
            {
                if (RushAim.X > 0)
                {
                    if (RushAim.Y > 0)
                    {
                        RushDirection = Vector2.One;
                    }
                    else
                    {
                        RushDirection = new Vector2(1f, -1);
                    }
                }
                else
                {
                    if (RushAim.Y > 0)
                    {
                        RushDirection = new Vector2(-1f, 1f);
                    }
                    else
                    {
                        RushDirection = new Vector2(-1f, -1);
                    }
                }
            }
            else
            {
                if (MathF.Abs(RushAim.X) > MathF.Abs(RushAim.Y))
                {
                    if (RushAim.X > 0)
                    {
                        RushDirection = Vector2.UnitX;
                    }
                    else
                    {
                        RushDirection = -Vector2.UnitX;
                    }
                }
                else
                {
                    if (RushAim.Y > 0)
                    {
                        RushDirection = Vector2.UnitY;
                    }
                    else
                    {
                        RushDirection = -Vector2.UnitY;
                    }
                }
            }
            HangPosition = User.Bottom;
            User.velocity = Vector2.Zero;
            ZipFrame.X = 0;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            User.immuneTime = 5;
            Duration++;
            if (Duration >= RushTime)
            {
                Duration -= RushTime;
                Frame++;
                if (Frame >= 10)
                {
                    Data.EndUse();
                    User.velocity = Vector2.Zero;
                    return;
                }
            }
            User.direction = Direction;
            User.gfxOffY = 0;
            if (Frame >= 4)
            {
                User.velocity = RushDirection * 22f;
                User.velocity.Y -= .5f;
                HurtCharactersInRectangle(User, User.Hitbox, Damage, DamageClass.Melee, 7f, Data);
            }
            else
            {
                User.velocity = Vector2.Zero;
                User.Bottom = HangPosition;
            }
            ZipFrame = (User as TerraGuardian).GetAnimationFrame(Frame);
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            short frame = (short)(68 + Frame);
            User.BodyFrameID = frame;
            for (int i = 0; i < 2; i++)
            {
                User.ArmFramesID[i] = frame;
            }
        }

        public override void Draw(Companion User, SubAttackData Data, bool DrawingFront, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (DrawingFront)
            {
                DrawData dd = new DrawData(User.Base.GetSpriteContainer.GetExtraTexture("pz"), Holder.DrawPosition + User.BodyOffset, ZipFrame, Color.White, User.fullRotation, Holder.Origin, User.Scale, drawSet.playerEffect, 0);
                DrawDatas.Add(dd);
            }
        }
    }
}