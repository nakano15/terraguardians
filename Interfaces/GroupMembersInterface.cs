using System;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using nterrautils;
using nterrautils.Interfaces;

namespace terraguardians
{
    public class GroupMembersInterface : LeftInterfaceElement
    {
        const string InterfaceKey = "Mods.terraguardians.Interface.GroupMembers.";
        private float[] BarValues = new float[4];

        private void SetBarValues(float Value1, float Value2 = 0, float Value3 = 0, float Value4 = 0)
        {
            BarValues[0] = Value1;
            BarValues[1] = Value2;
            BarValues[2] = Value3;
            BarValues[3] = Value4;
        }

        public override string Name => "TerraGuardians Group Infos";

        public GroupMembersInterface()
        {
            
        }

        static string GetTranslation(string Key)
        {
            return Language.GetTextValue(InterfaceKey + Key);
        }

        public override void DrawInternal(ref float PositionY)
        {
            Vector2 DrawPosition = new Vector2(8, PositionY);
            List<Player> GroupMembers = new List<Player>();
            PlayerMod MyCharacter = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            foreach(Companion c in MyCharacter.GetSummonedCompanions)
            {
                if(c != null)
                    GroupMembers.Add(c);
            }
            string MouseOverText = "";
            bool FirstCompanion = true;
            foreach(Player p in GroupMembers)
            {
                DrawPosition.X += 32;
                {
                    DrawPosition.X -= 16;
                    DrawPosition.Y += 16;
                    PlayerMod.DrawPlayerHeadInterface(p, DrawPosition, false, MaxDimension: 32);
                    DrawPosition.X += 16;
                    DrawPosition.Y -= 16;
                }
                {
                    Color color = Color.White;
                    if (p is Companion && PlayerMod.GetIsPlayerBuddy(MainMod.GetLocalPlayer, (p as Companion)))
                        color = Color.Yellow;
                    string Name = GetTranslation("Name").Replace("{name}", p.name).Replace("{health}", p.statLife.ToString()).Replace("{maxhealth}", p.statLifeMax2.ToString());
                    if (FirstCompanion && MainMod.Gameplay2PMode)
                        Name = GetTranslation("PlayerTwoTag").Replace("{name}", Name);
                    Utils.DrawBorderString(Main.spriteBatch, Name, DrawPosition, color);
                }
                if (FirstCompanion && MainMod.Gameplay2PMode)
                {
                    Draw2PInfos(p, ref DrawPosition);
                }
                else if (p is Companion && PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer) == (p as Companion))
                {
                    DrawSubAttackSlots(p, DrawPosition + Vector2.UnitX * 130 + Vector2.UnitY * 22);
                }
                DrawPosition.Y += 22;
                {
                    float HealthValue = Math.Clamp((float)p.statLife / p.statLifeMax2, 0f, 1f);
                    float LCValue = Math.Clamp(p.ConsumedLifeCrystals * 0.066667f, 0f, HealthValue);
                    float LFValue = Math.Clamp(p.ConsumedLifeFruit * 0.05f, 0, HealthValue);
                    SetBarValues(HealthValue, LCValue, LFValue);
                }
                if(DrawBar(PlayerMod.GetPlayerKnockoutState(p) == KnockoutStates.Awake ? (byte)0 : (byte)4, DrawPosition, BarValues))
                {
                    MouseOverText = GetTranslation("Health")
                        .Replace("{health}", p.statLife.ToString())
                        .Replace("{maxhealth}", p.statLifeMax2.ToString())
                        .Replace("{lc}", p.ConsumedLifeCrystals.ToString())
                        .Replace("{lf}", p.ConsumedLifeFruit.ToString());
                }
                DrawPosition.Y += 18;
                DrawPosition.X -= 32;
                if (Main.dontStarveWorld)
                {
                    float BarValue = 1;
                    if (p.starving)
                        BarValue = 0;
                    else if (p.hungry)
                    {
                        int Index = p.FindBuffIndex(Terraria.ID.BuffID.Hunger);
                        if (Index > -1)
                        {
                            BarValue = (float)p.buffTime[Index] / 18000;
                        }
                    }
                    if(BarValue < 1)
                    {
                        SetBarValues(BarValue);
                        if (DrawBar(5, DrawPosition, BarValues))
                        {
                            int s = (int)(BarValue * 300);
                            int m = s / 60;
                            s -= m * 60;
                            if (m > 0)
                                MouseOverText = GetTranslation("HungerMinutes").Replace("{time}", m.ToString());
                            else
                                MouseOverText = GetTranslation("HungerMinutes").Replace("{time}", s.ToString()); 
                        }
                        DrawPosition.Y += 18;
                    }
                }
                {
                    float ManaValue = Math.Clamp((float)p.statMana / p.statManaMax2, 0, 1);
                    float ManaCrystalValue = Math.Clamp((p.ConsumedManaCrystals) * 0.1111115f, 0, ManaValue);
                    SetBarValues(ManaValue, ManaCrystalValue);
                }
                if(DrawBar(3, DrawPosition, BarValues))
                {
                    MouseOverText = GetTranslation("Mana")
                        .Replace("{mana}", p.statMana.ToString())
                        .Replace("{maxmana}", p.statManaMax2.ToString())
                        .Replace("{mc}", (p.ConsumedManaCrystals).ToString());
                }
                DrawPosition.Y += 18;
                if(p.breath< p.breathMax)
                {
                    float BreathValue = Math.Clamp((float)p.breath / p.breathMax, 0, 1);
                    SetBarValues(BreathValue);
                    DrawBar(1, DrawPosition, BarValues);
                    DrawPosition.Y += 18;
                }
                if(p.lavaTime < p.lavaMax)
                {
                    float LavaValue = Math.Clamp((float)p.lavaTime / p.lavaMax, 0, 1);
                    SetBarValues(LavaValue);
                    DrawBar(2, DrawPosition, BarValues);
                    DrawPosition.Y += 18;
                }
                foreach(Func<Player, Vector2, float> hook in MainMod.GroupInterfaceBarsHooks)
                {
                    DrawPosition.Y += hook(p, DrawPosition);
                }
                if (p is Companion)
                {
                    if (DrawCompanionNotifications(p as Companion, DrawPosition, ref MouseOverText))
                    {
                        DrawPosition.Y += 28f;
                    }
                }
                DrawPosition.Y += 4;
                FirstCompanion = false;
            }
            if (!MainMod.Gameplay2PMode && MainMod.Show2PNotification)
            {
                DrawPosition.Y += 4;
                Utils.DrawBorderString(Main.spriteBatch, GetTranslation("SecondPlayerNotification"), DrawPosition, Color.White);
                DrawPosition.Y += 22;
            }
            //for debug
            if (MainMod.IsDebugMode)
            {
                List<string> ExtraMessages = new List<string>();
                /*foreach (Companion c in MainMod.ActiveCompanions.Values)
                {
                    ExtraMessages.Add(c.name + ":");
                    ExtraMessages.Add("  Velocity: " + c.velocity + ".");
                    ExtraMessages.Add("  Jumping?: " + c.controlJump + ".");
                    ExtraMessages.Add("  Moving Right?: " + c.controlRight + ".");
                }*/
                /*foreach (Companion c in PlayerMod.PlayerGetSummonedCompanions(MainMod.GetLocalPlayer))
                {
                    ExtraMessages.Add(c.name + "'s infos: ");
                    ExtraMessages.Add("Has Target? " + (c.Target != null));
                    if (c.Target != null)
                        ExtraMessages.Add("Target: " + c.Target.ToString());
                    //ExtraMessages.Add("Summons: "+c.numMinions+" Max: " + c.maxMinions);
                    //ExtraMessages.Add(c.fullRotationOrigin.ToString() + " : " + c.fullRotation);
                }*/
                /*foreach (ushort k in GenericCompanionInfos.CompanionInfos.Keys)
                {
                    ExtraMessages.Add(k + "#" + GenericCompanionInfos.CompanionInfos[k].Name + " (lifetime: "+GenericCompanionInfos.CompanionInfos[k].LifeTime+" Temporary? "+GenericCompanionInfos.CompanionInfos[k].IsTemporary+")");
                }*/
                foreach(string s in ExtraMessages)
                {
                    Utils.DrawBorderString(Main.spriteBatch, s, DrawPosition, Color.White, 0.7f);
                    DrawPosition.Y += 20;
                }
            }
            PositionY = DrawPosition.Y + 20;
            //
            if(MouseOverText.Length > 0)
            {
                MouseOverInterface.ChangeMouseText(MouseOverText);
            }
        }

        bool DrawCompanionNotifications(Companion c, Vector2 InterfacePosition, ref string MouseOverText)
        {
            bool AnyNotification = false;
            Texture2D texture = MainMod.GuardianInfosNotificationTexture.Value;
            const int IconDimension = 12;
            InterfacePosition.X += 4;
            InterfacePosition.Y += 4;
            CompanionInventoryStatsContainer container = c.InventorySupplyStatus;
            string StatusInfo = "";
            for (int i = 0; i < 6; i++)
            {
                byte StatusValue = 0;
                switch(i)
                {
                    case 0:
                        StatusValue = container.HealthPotionsStatus;
                        break;
                    case 1:
                        StatusValue = container.ManaPotionsStatus;
                        break;
                    case 2:
                        StatusValue = container.ArrowsStatus;
                        break;
                    case 3:
                        StatusValue = container.BulletsStatus;
                        break;
                    case 4:
                        StatusValue = container.RocketsStatus;
                        break;
                    case 5:
                        StatusValue = container.FoodStatus;
                        break;
                }
                bool MouseOver = Main.mouseX >= InterfacePosition.X && Main.mouseX < InterfacePosition.X + 24 && Main.mouseY >= InterfacePosition.Y && Main.mouseY < InterfacePosition.Y + 24;
                switch(StatusValue)
                {
                    case CompanionInventoryStatsContainer.STATUS_NEEDMORE:
                        Main.spriteBatch.Draw(texture, InterfacePosition, new Rectangle(24 * (i + 1), 0, 24, 24), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        if (MouseOver) StatusInfo = "Need more ";
                        InterfacePosition.X += 32;
                        AnyNotification = true;
                        break;
                    case CompanionInventoryStatsContainer.STATUS_HASNONE:
                        Main.spriteBatch.Draw(texture, InterfacePosition, new Rectangle(24 * (i + 1), 0, 24, 24), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(texture, InterfacePosition, new Rectangle(0, 0, 24, 24), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        if (MouseOver) StatusInfo = "Has no more ";
                        InterfacePosition.X += 32;
                        AnyNotification = true;
                        break;
                }
                if (MouseOver && StatusInfo != "")
                {
                    switch(i)
                    {
                        case 0:
                            StatusInfo += "Healing Potions.";
                            break;
                        case 1:
                            StatusInfo += "Mana Potions.";
                            break;
                        case 2:
                            StatusInfo += "Arrows.";
                            break;
                        case 3:
                            StatusInfo += "Ammunition.";
                            break;
                        case 4:
                            StatusInfo += "Rockets.";
                            break;
                        case 5:
                            StatusInfo += "Food.";
                            break;
                    }
                    MouseOverText = StatusInfo;
                }
            }
            return AnyNotification;
        }

        private void Draw2PInfos(Player character, ref Vector2 InterfacePos)
        {
            if (MainMod.Gameplay2PInventory) return;
            int MainPlayerSlotBackup = Main.player[Main.myPlayer].selectedItem;
            //Inventory Slots
            int CurrentSlot = character.selectedItem;
            Main.player[Main.myPlayer].selectedItem = CurrentSlot;
            float ScaleBackup = Main.inventoryScale;
            Main.inventoryScale = 0.5f;
            Dictionary<int, float> SlotIndexAndXPosition = new Dictionary<int, float>();
            float Distance = 0;
            for (int i = 0; i < 10; i++)
            {
                SlotIndexAndXPosition.Add(i, Distance);
                int Dif = i - CurrentSlot;
                if (Dif < -1 || Dif > 0)
                {
                    Distance += 6;
                }
                else
                {
                    Distance += 22;
                }
            }
            Vector2 SlotsStartPos = new Vector2(InterfacePos.X + 140f, InterfacePos.Y + 5 + 22);
            for (byte Rule = 0; Rule < 2; Rule ++)
            {
                switch(Rule)
                {
                    default:
                        for (int i = 0; i < CurrentSlot; i++)
                        {
                            Vector2 SlotPosition = new Vector2(SlotsStartPos.X + SlotIndexAndXPosition[i], SlotsStartPos.Y);
                            ItemSlot.Draw(Main.spriteBatch, character.inventory, 0, i, SlotPosition);
                        }
                        break;
                    case 1:
                        for (int i = 9; i >= CurrentSlot; i--)
                        {
                            Vector2 SlotPosition = new Vector2(SlotsStartPos.X + SlotIndexAndXPosition[i], SlotsStartPos.Y);
                            ItemSlot.Draw(Main.spriteBatch, character.inventory, 0, i, SlotPosition);
                        }
                        break;
                }
            }
            Main.inventoryScale = ScaleBackup;
            Main.player[Main.myPlayer].selectedItem = MainPlayerSlotBackup;
            SlotIndexAndXPosition.Clear();
            SlotsStartPos.Y += 32;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    int Index = x + y * 7;
                    if (Index >= Player.MaxBuffs) break;
                    if (character.buffTime[Index] <= 0 || character.buffType[Index] <= 0) continue;
                    Vector2 BuffPos = new Vector2(SlotsStartPos.X + 2 + 24f * x, SlotsStartPos.Y + 2 + 24f * y);
                    Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Buff[character.buffType[Index]].Value, BuffPos, null, Color.White * 0.6f, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                    if (Main.buffNoTimeDisplay[character.buffType[Index]]) continue;
                    BuffPos.X += 16;
                    BuffPos.Y += 16;
                    string TimeString;
                    int Time = character.buffTime[Index];
                    const float DivisionBy60 = 1f / 60;
                    const float DivisionBy3600 = 1f / 3600;
                    if (Time < 300)
                    {
                        TimeString = GetTranslation("BuffTimeSeconds").Replace("{time}", MathF.Round((float)Time * DivisionBy60, 1).ToString());
                    }
                    else if (Time < 60 * 60)
                    {
                        TimeString = GetTranslation("BuffTimeSeconds").Replace("{time}", ((int)(Time * DivisionBy60)).ToString());
                    }
                    else
                    {
                        TimeString = GetTranslation("BuffTimeMinutes").Replace("{time}", ((int)(Time * DivisionBy3600)).ToString());
                    }
                    Utils.DrawBorderString(Main.spriteBatch, TimeString, BuffPos, Color.White, 0.5f, 1, 1);
                }
            }
            //Sub Attack Slots
            DrawSubAttackSlots(character, InterfacePos + Vector2.UnitX * (140 + 76));
        }

        private void DrawSubAttackSlots(Player character, Vector2 InterfacePos)
        {
            if (character is not Companion) return;
            Companion c = (Companion)character;
            List<int> SubAttackSlotAndXPos = new List<int>();
            float Distance = 0;
            int Current = 0;
            for (int i = 0; i < c.SubAttackIndexes.Length; i++)
            {
                if (c.SubAttackIndexes[i] < 255 && c.SubAttackIndexes[i] < c.SubAttackList.Count)
                {
                    if (i == c.SelectedSubAttack)
                        Current = SubAttackSlotAndXPos.Count;
                    SubAttackSlotAndXPos.Add(c.SubAttackIndexes[i]);
                }
            }
            Vector2 SlotStartPos = new Vector2(InterfacePos.X + 10, InterfacePos.Y + 5);
            Texture2D Background = Terraria.GameContent.TextureAssets.InventoryBack17.Value;
            for (byte Rule = 0; Rule < 2; Rule++)
            {
                switch(Rule)
                {
                    default:
                        Distance = 0;
                        for (int i = 0; i < Current; i++)
                        {
                            int Index = SubAttackSlotAndXPos[i];
                            Vector2 SlotPosition = new Vector2(SlotStartPos.X + Distance, SlotStartPos.Y);
                            bool InCooldown = c.SubAttackInCooldown(Index);
                            Color color = (InCooldown? Color.DarkGray : Color.White);
                            Main.spriteBatch.Draw(Background, SlotPosition, null, color * 0.9f, 0f, Vector2.Zero, Main.inventoryScale, 0, 0);
                            Texture2D IconTexture = c.Base.GetSubAttackBases[Index].GetIcon;
                            if (IconTexture != null)
                            {
                                float MaxIconSize = 42f * Main.inventoryScale;
                                SlotPosition.X += 26 * Main.inventoryScale;
                                SlotPosition.Y += 26 * Main.inventoryScale;
                                float IconScale = 1;
                                if (IconTexture.Width > IconTexture.Height)
                                {
                                    IconScale = MaxIconSize / IconTexture.Width;
                                }
                                else
                                {
                                    IconScale = MaxIconSize / IconTexture.Height;
                                }
                                Vector2 Pivot = new Vector2(IconTexture.Width * 0.5f, IconTexture.Height * 0.5f);
                                Main.spriteBatch.Draw(IconTexture, SlotPosition, null, color, 0, Pivot, IconScale, 0, 0);
                            }
                            Distance += 6;
                        }
                        break;
                    case 1:
                        Distance = (SubAttackSlotAndXPos.Count - 1) * 6;
                        for (int i = SubAttackSlotAndXPos.Count - 1; i >= Current; i--)
                        {
                            if (i >= SubAttackSlotAndXPos.Count) continue;
                            int Index = SubAttackSlotAndXPos[i];
                            Vector2 SlotPosition = new Vector2(SlotStartPos.X + Distance, SlotStartPos.Y);
                            bool InCooldown = c.SubAttackInCooldown(Index);
                            Color color = (InCooldown? Color.DarkGray : Color.White);
                            Main.spriteBatch.Draw(Background, SlotPosition, null, color * 0.9f, 0f, Vector2.Zero, Main.inventoryScale, 0, 0);
                            Texture2D IconTexture = c.Base.GetSubAttackBases[Index].GetIcon;
                            //Main.NewText("W: " + Background.Width + "  H: " + Background.Height);
                            if (IconTexture != null)
                            {
                                float MaxIconSize = 42f * Main.inventoryScale;
                                SlotPosition.X += 26 * Main.inventoryScale;
                                SlotPosition.Y += 26 * Main.inventoryScale;
                                float IconScale = 1;
                                if (IconTexture.Width > IconTexture.Height)
                                {
                                    IconScale = MaxIconSize / IconTexture.Width;
                                }
                                else
                                {
                                    IconScale = MaxIconSize / IconTexture.Height;
                                }
                                Vector2 Pivot = new Vector2(IconTexture.Width * 0.5f, IconTexture.Height * 0.5f);
                                Main.spriteBatch.Draw(IconTexture, SlotPosition, null, color, 0, Pivot, IconScale, 0, 0);
                            }
                            Distance -= 6;
                        }
                        break;
                }
            }
        }

        private bool DrawBar(byte BarID, Vector2 BarPosition, float[] BarValues)
        {
            const int BarSpriteWidth = 124, BarSpriteHeight = 16,
                        DistanceUntilBarStartX = 22, BarWidth = 98;
            Rectangle DrawFrame = new Rectangle(BarSpriteWidth * BarID, 0, BarSpriteWidth, BarSpriteHeight);
            Texture2D BarTexture = MainMod.GuardianHealthBarTexture.Value;
            Main.spriteBatch.Draw(BarTexture, BarPosition, DrawFrame, Color.White);
            BarPosition.X += DistanceUntilBarStartX;
            DrawFrame.X += DistanceUntilBarStartX;
            for(byte i = 0; i < BarValues.Length; i++)
            {
                DrawFrame.Y += BarSpriteHeight;
                if(BarValues[i] <= 0) continue;
                DrawFrame.Width = (int)(BarWidth * BarValues[i]);
                Main.spriteBatch.Draw(BarTexture, BarPosition, DrawFrame, Color.White);
            }
            return Main.mouseX >= BarPosition.X && Main.mouseX < BarPosition.X + BarWidth && 
                Main.mouseY >= BarPosition.Y + 4 && Main.mouseY < BarPosition.Y + 12;
        }
    }
}
