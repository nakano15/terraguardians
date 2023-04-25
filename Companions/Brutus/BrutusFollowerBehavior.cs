using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Brutus
{
    public class BrutusFollowerBehavior : FollowLeaderBehavior
    {
        float AfkCounter = 0;
        const float ProtectModeTriggerTime = 3600;
        public bool ProtectionModeActivated { get { return AfkCounter >= ProtectModeTriggerTime; } }

        public override void Update(Companion companion)
        {
            Entity Leader = companion.GetLeaderCharacter();
            if (Leader is Player)
            {
                Player p = Leader as Player;
                if(((p.sitting.isSitting && companion.sitting.isSitting) || (p.sleeping.isSleeping && companion.sleeping.isSleeping)) && companion.Bottom == p.Bottom)
                {
                    p.AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                    AfkCounter = 0;
                }
                else if (!companion.IsMountedOnSomething && !p.controlLeft && !p.controlRight && !p.controlJump && !p.controlDown && !p.controlUp)
                {
                    AfkCounter++;
                }
                else
                {
                    AfkCounter = 0;
                }
            }
            else
            {
                if(Leader.velocity.X == 0 && Leader.velocity.Y == 0) AfkCounter++;
                else AfkCounter = 0;
            }
            if (AfkCounter >= ProtectModeTriggerTime)
            {
                UpdateProtectMode(companion, Leader);
            }
            else
            {
                UpdateFollow(companion);
            }
        }

        public override void UpdateStatus(Companion companion)
        {
            if (ProtectionModeActivated)
            {
                companion.noKnockback = true;
                companion.DefenseRate += 0.05f;
            }
        }

        private void UpdateProtectMode(Companion companion, Entity Leader)
        {
            if (companion.GoingToOrUsingFurniture)
                companion.LeaveFurniture();
            bool UsingFurniture = false;
            if (Leader is Player)
            {
                Player p = Leader as Player;
                Companion othercompanion = PlayerMod.PlayerGetControlledCompanion(p);
                if(othercompanion != null)
                {
                    if (othercompanion.IsMountedOnSomething)
                        Leader = othercompanion.GetCharacterMountedOnMe;
                    else
                        Leader = othercompanion;
                }
                else
                {
                    othercompanion = PlayerMod.PlayerGetMountedOnCompanion(p);
                    if (othercompanion != null)
                        Leader = othercompanion;
                }
                UsingFurniture = ((Leader as Player).sitting.isSitting || (Leader as Player).sleeping.isSleeping);
            }
            Vector2 ProtectPosition = Leader.Bottom - Vector2.UnitX * 4 * Leader.direction;
            if (UsingFurniture)
            {
                Vector2 CheckBehind = Leader.Bottom;
                CheckBehind.X -= companion.width * 0.5f;
                CheckBehind.Y -= companion.height;
                for (int i = 0; i < 5; i++)
                {
                    CheckBehind.X -= 8 * Leader.direction;
                    int CX = (int)(CheckBehind.X * Companion.DivisionBy16),
                        CY = (int)(CheckBehind.Y * Companion.DivisionBy16);
                    bool Blocked = false;
                    for (int y = 0; y < 3; y++)
                    {
                        for(int x = 0; x < 2; x++)
                        {
                            Tile tile = Main.tile[CX +x, CY + y];
                            if (tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !Terraria.ID.TileID.Sets.Platforms[tile.TileType])
                            {
                                Blocked = true;
                                break;
                            }
                        }
                    }
                    if (!Blocked)
                    {
                        ProtectPosition = CheckBehind;
                        ProtectPosition.X += companion.width * 0.5f;
                        ProtectPosition.Y += companion.height;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (Math.Abs(ProtectPosition.X - companion.Center.X) >= 8)
            {
                if (companion.Center.X < ProtectPosition.X)
                    companion.MoveRight = true;
                else
                    companion.MoveLeft = true;
            }
            else
            {
                companion.Bottom = ProtectPosition;
                companion.velocity.X = 0;
                companion.velocity.Y = 0;
                companion.SetFallStart();
                if(companion.itemAnimation == 0)
                {
                    companion.direction = Leader.direction;
                }
                companion.controlDown = true;
                if (Leader is Player)
                {
                    (Leader as Player).AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                }
                else if (Leader is NPC)
                {
                    (Leader as NPC).AddBuff(ModContent.BuffType<Buffs.Defended>(), 5);
                }
            }
        }
    }
}