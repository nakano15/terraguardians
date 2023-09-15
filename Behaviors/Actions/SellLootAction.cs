using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Actions
{
    public class SellLootAction : BehaviorBase
    {
        public Steps Step = Steps.Leaving;
        public int Time = 0;
        public int SaleCoinsAcquired = 0;

        public override void OnBegin()
        {
            AllowSeekingTargets = false;
            RunCombatBehavior = false;
            CanTargetNpcs = false;
        }

        public override void UpdateStatus(Companion companion)
        {
            bool CantBeHurt = Step == Steps.SellingItems;
            if (CantBeHurt)
            {
                companion.aggro = -900000;
            }
            CanBeHurtByNpcs = CanBeAttacked = IsVisible = !CantBeHurt;
        }

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake)
            {
                Deactivate();
                return;
            }
            switch(Step)
            {
                case Steps.Leaving:
                    {
                        if (Time < 60)
                        {
                            if (!companion.TargettingSomething && companion.itemAnimation == 0 && companion.Owner != null)
                            {
                                if (companion.Owner.Center.X < companion.Center.X)
                                {
                                    companion.direction = -1;
                                }
                                else
                                {
                                    companion.direction = 1;
                                }
                            }
                            else if (companion.Owner == null)
                            {
                                Deactivate();
                                return;
                            }
                            Time++;
                        }
                        else
                        {
                            if (companion.Owner != null)
                            {
                                if (companion.Owner.Center.X < companion.Center.X)
                                {
                                    companion.MoveRight = true;
                                }
                                else
                                {
                                    companion.MoveLeft = true;
                                }
                                if (Math.Abs(companion.Owner.Center.X - companion.Center.X) > Main.screenWidth * 0.5f + companion.SpriteWidth * companion.Scale || 
                                    Math.Abs(companion.Owner.Center.Y - companion.Center.Y) > Main.screenHeight * 0.5f + companion.SpriteHeight * companion.Scale)
                                {
                                    Step = Steps.SellingItems;
                                    Time = 0;
                                }
                            }
                            else
                            {
                                Deactivate();
                                return;
                            }
                        }
                    }
                    break;
                
                case Steps.SellingItems:
                    {
                        if (companion.Owner == null)
                        {
                            Step = Steps.Returns;
                            Time = 0;
                            return;
                        }
                        companion.Center = companion.Owner.Center;
                        companion.velocity = Vector2.Zero;
                        companion.SetFallStart();
                        if (Time == 150)
                        {
                            SaleCoinsAcquired = GetItemSellingGolds(companion);
                            string Message;
                            if (SaleCoinsAcquired == 0)
                                Message = companion.GetNameColored() + " got nothing from selling the items.";
                            else
                                Message = companion.GetNameColored() + " has sold the items for " + Main.ValueToCoins(SaleCoinsAcquired) + ".";
                            Main.NewText(Message);
                        }
                        if (Time >= 300)
                        {
                            Step = Steps.Returns;
                            Time = 0;
                            companion.Teleport(companion.Owner);
                        }
                        else
                        {
                            Time++;
                        }
                    }
                    break;
                
                case Steps.Returns:
                    {
                        if (Time >= 150)
                        {
                            ConvertToCoins(SaleCoinsAcquired, out int p, out int g, out int s, out int c);
                            Vector2 SpawnPosition = (companion.Owner != null ? companion.Owner.Center : companion.Center);
                            if (p > 0) Item.NewItem(Player.GetSource_None(), SpawnPosition, Vector2.Zero, ItemID.PlatinumCoin, p, noBroadcast: true, noGrabDelay: true);
                            if (g > 0) Item.NewItem(Player.GetSource_None(), SpawnPosition, Vector2.Zero, ItemID.GoldCoin, g, noBroadcast: true, noGrabDelay: true);
                            if (s > 0) Item.NewItem(Player.GetSource_None(), SpawnPosition, Vector2.Zero, ItemID.SilverCoin, s, noBroadcast: true, noGrabDelay: true);
                            if (c > 0) Item.NewItem(Player.GetSource_None(), SpawnPosition, Vector2.Zero, ItemID.CopperCoin, c, noBroadcast: true, noGrabDelay: true);
                            Deactivate();
                        }
                        Time++;
                    }
                    break;
            }
        }

        public static void ConvertToCoins(int Value, out int p, out int g, out int s, out int c)
        {
            c = Value;
            s = 0;
            g = 0;
            p = 0;
            if (c >= 100)
            {
                s += c / 100;
                c -= s * 100;
            }
            if (s >= 100)
            {
                g += s / 100;
                s -= g * 100;
            }
            if (g >= 100)
            {
                p += g / 100;
                g -= p * 100;
            }
        }

        public static int GetItemSellingGolds(Companion c)
        {
            int Coins = 0;
            for (int i = 10; i < 50; i++)
            {
                if(c.inventory[i].type > 0 && (c.inventory[i].type < ItemID.CopperCoin || c.inventory[i].type > ItemID.PlatinumCoin) && !c.inventory[i].favorited)
                {
                    c.GetItemExpectedPrice(c.inventory[i], out var sell, out var buy);
                    if (sell > 0)
                    {
                        Coins += (int)sell * c.inventory[i].stack;
                    }
                    c.inventory[i].SetDefaults(0);
                }
            }
            return Coins;
        }

        public enum Steps : byte
        {
            Leaving = 0,
            SellingItems = 1,
            Returns = 2
        }
    }
}