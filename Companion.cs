using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace terraguardians
{
    public partial class Companion : Player
    {
        private static Companion ReferedCompanion = null;
        public static Companion GetReferedCompanion { get { return ReferedCompanion; } }
        private static Vector2 NewAimDirectionBackup = Vector2.Zero;

        public const float DivisionBy16 = 1f / 16;
        public ushort GetWhoAmID { get{ return WhoAmID; }}
        private ushort WhoAmID = 0;
        private static ushort LastWhoAmID = 0;

        public CompanionBase Base
        {
            get
            {
                return Data.Base;
            }
        }
        private CompanionData _data = new CompanionData();
        public CompanionData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public uint ID { get { return Data.ID; } }
        public string ModID { get { return Data.ModID; } }
        public bool IsPlayerCharacter = false;
        public int Owner = -1;
        #region Useful getter setters
        public bool MoveLeft{ get{ return controlLeft;} set{ controlLeft = value; }}
        public bool LastMoveLeft{ get{ return releaseLeft;} set{ releaseRight = value; }}
        public bool MoveRight{ get{ return controlRight;} set{ controlRight = value; }}
        public bool LastMoveRight{ get{ return releaseRight;} set{ releaseRight = value; }}
        public bool MoveUp{ get{ return controlUp;} set{ controlUp = value; }}
        public bool LastMoveUp{ get{ return releaseUp;} set{ releaseUp = value; }}
        public bool MoveDown{ get{ return controlDown;} set{ controlDown = value; }}
        public bool LastMoveDown{ get{ return releaseDown;} set{ releaseDown = value; }}
        public bool ControlJump{ get{ return controlJump;} set{ controlJump = value; }}
        public bool LastControlJump{ get{ return releaseJump;} set{ releaseJump = value; }}
        public bool ControlAction { get{ return controlUseItem; } set { controlUseItem = value; }}
        public bool LastControlAction { get{ return releaseUseItem; } set { releaseUseItem = value; }}
        #endregion
        public Vector2 AimDirection = Vector2.Zero;
        public Vector2 GetAimedPosition
        {
            get
            {
                return Center + AimDirection;
            }
            set
            {
                AimDirection = value - Center;
            }
        }
        public bool WalkMode = false;
        public float Scale = 1f;
        public bool Crouching { get{ return MoveDown; } set { MoveDown = value; } }
        public Entity Target = null;

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner == Main.myPlayer) || (Main.netMode == 2 && Owner == -1);
            }
        }

        public bool CanJump
        {
            get
            {
                return velocity.Y == 0 && !controlJump;
            }
        }

        public bool CanDoJumping
        {
            get
            {
                return CanJump || jump > 0;
            }
        }

        private byte AIAction = 0;
        private byte AITime = 0;
        private float Time = 0;

        private float FireDirection = 0;

        public Companion() : base()
        {
            WhoAmID = LastWhoAmID++;
        }

        public void UpdateBehaviour()
        {
            LookForTargets();
            CombatBehaviour();
            if(Target == null)
                ChangeAimPosition(Center + Vector2.UnitX * width * direction);
            if(Owner > -1)
            {
                FollowPlayerBehaviour();
            }
            else 
            {
                if(AITime == 0)
                {
                    WalkMode = true;
                    switch(AIAction)
                    {
                        case 0:
                            AIAction = 1;
                            AITime = 200;
                            direction = Main.rand.Next(2) == 0 ? -1 : 1;
                            break;
                        case 1:
                            AIAction = 0;
                            AITime = 120;
                            break;
                    }
                }
                switch(AIAction)
                {
                    case 1:
                        if(direction == 1)
                            MoveRight = true;
                        else
                            MoveLeft = true;
                        break;        
                }
                AITime--;
            }
            if(MoveLeft || MoveRight)
                CheckIfNeedToJumpTallTile();
        }

        private void LookForTargets()
        {
            if(Target != null && (!Target.active || (Target is Player && ((Player)Target).dead))) Target = null;
            float NearestDistance = 400f;
            Entity NewTarget = null;
            Vector2 MyCenter = Center;
            for (int i = 0; i < 200; i++)
            {
                if(!Main.npc[i].active) continue;
                NPC npc = Main.npc[i];
                if(npc.active && !npc.friendly && npc.CanBeChasedBy(null))
                {
                    float Distance = (MyCenter - npc.Center).Length();
                    if(Distance < NearestDistance)
                    {
                        NewTarget = npc;
                        NearestDistance = Distance;
                    }
                }
            }
            if (NewTarget != null)
                Target = NewTarget;
        }

        private void CombatBehaviour()
        {
            if(Target == null) return;
            Vector2 FeetPosition = Bottom;
            Vector2 TargetPosition = Target.Bottom;
            int TargetWidth = Target.width;
            int TargetHeight = Target.height;
            float HorizontalDistance = MathF.Abs(TargetPosition.X - FeetPosition.X - (TargetWidth + width) * 0.5f);
            if(itemAnimation == 0)
            {
                byte StrongestMelee = 0, StrongestRanged = 0, StrongestMagic = 0;
                int HighestMeleeDamage = 0, HighestRangedDamage = 0, HighestMagicDamage = 0;
                byte StrongestItem = 0;
                int HighestDamage = 0;
                for(byte i = 0; i < 10; i++)
                {
                    Item item = inventory[i];
                    if(item.type > 0 && item.damage > 0)
                    {
                        int DamageValue = GetWeaponDamage(item);
                        if(!HasAmmo(item) || statMana < GetManaCost(item)) continue;
                        if(item.DamageType.CountsAsClass(DamageClass.Melee))
                        {
                            if (DamageValue > HighestMeleeDamage)
                            {
                                HighestMeleeDamage = DamageValue;
                                StrongestMelee = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Ranged))
                        {
                            if (DamageValue > HighestRangedDamage)
                            {
                                HighestRangedDamage = DamageValue;
                                StrongestRanged = i;
                            }
                        }
                        else if(item.DamageType.CountsAsClass(DamageClass.Magic))
                        {
                            if (DamageValue > HighestMagicDamage)
                            {
                                HighestMagicDamage = DamageValue;
                                StrongestMagic = i;
                            }
                        }
                        if(DamageValue > HighestDamage)
                        {
                            HighestDamage = DamageValue;
                            StrongestItem = i;
                        }
                    }
                }
                if (HighestMeleeDamage > 0 && HorizontalDistance < 60 + width * 0.5f)
                {
                    selectedItem = StrongestMelee;
                }
                else if (HighestMagicDamage > 0)
                {
                    selectedItem = StrongestMagic;
                }
                else if (HighestRangedDamage > 0)
                {
                    selectedItem = StrongestRanged;
                }
                else
                {
                    selectedItem = StrongestItem;
                }
            }
            bool TargetInAim = AimAtTarget(Target.position + Target.velocity, Target.width, Target.height);
            bool Left = false, Right = false, Attack = false, Jump = false;
            if(HeldItem.type == 0) //Run for your lives!
            {
                WalkMode = HorizontalDistance < 150;
                if(HorizontalDistance < 200)
                {
                    if (FeetPosition.X > TargetPosition.X)
                        Right = true;
                    else
                        Left = true;
                }
                else
                {
                    if (velocity.X == 0 && velocity.Y == 0)
                    {
                        direction = FeetPosition.X < TargetPosition.X ? 1 : -1;
                    }
                }
            }
            else
            {
                WalkMode = false;
                if(HeldItem.DamageType.CountsAsClass(DamageClass.Melee))
                {
                    //Close Ranged Combat
                    float AttackRange = width * 0.5f + HeldItem.width * HeldItem.scale * 1.2f;
                    if(HorizontalDistance < AttackRange)
                    {
                        Attack = true;
                        if(itemAnimation == 0)
                        {
                            direction = FeetPosition.X < TargetPosition.X ? 1 : -1;
                        }
                        else if(HorizontalDistance < AttackRange * 0.9f)
                        {
                            if (FeetPosition.X > TargetPosition.X)
                            {
                                Right = true;
                            }
                            else
                            {
                                Left = true;
                            }
                        }
                    }
                    else
                    {
                        if (FeetPosition.X < TargetPosition.X)
                        {
                            Right = true;
                        }
                        else
                        {
                            Left = true;
                        }
                    }
                }
                else if(HeldItem.DamageType.CountsAsClass(DamageClass.Ranged) || 
                        HeldItem.DamageType.CountsAsClass(DamageClass.Magic))
                {
                    if(HorizontalDistance < 100)
                    {
                        if(FeetPosition.X < TargetPosition.X)
                            Left = true;
                        else
                            Right = true;
                    }
                    else if(HorizontalDistance >= 250)
                    {
                        if(FeetPosition.X < TargetPosition.X)
                            Right = true;
                        else
                            Left = true;
                    }
                    if(TargetInAim && CanHit(Target))
                    {
                        Attack = true;
                    }
                }
            }

            if (Left != Right)
            {
                if(Left) controlLeft = true;
                if(Right) controlRight = true;
            }
            if(Jump && (velocity.Y == 0 || jumpHeight > 0))
                this.ControlJump = true;
            if (Attack)
            {
                if(itemAnimation == 0)
                    this.ControlAction = true;
            }
        }

        private void FollowPlayerBehaviour()
        {
            if(Target != null) return;
            Player player = Main.player[Owner];
            Vector2 PlayerPosition = player.Center;
            if(Math.Abs(PlayerPosition.X - player.velocity.X - Center.X) > 40)
            {
                if(PlayerPosition.X < Center.X)
                    MoveLeft = true;
                else
                    MoveRight = true;
            }
            if(Math.Abs(PlayerPosition.X - Center.X) >= 500 || 
                Math.Abs(PlayerPosition.Y - Center.Y) >= 400)
                {
                    Teleport(player.Bottom);
                }
            //WalkMode = Math.Abs(PlayerPosition.X - Center.X) < 40;
        }

        public void CheckIfNeedToJumpTallTile()
        {
            if(CanDoJumping)
            {
                float MovementDirection = controlLeft ? -1 : controlRight ? 1 : direction;
                int TileX = (int)((Center.X + (width * 0.5f + 1) * MovementDirection) * DivisionBy16);
                int TileY = (int)((Bottom.Y - 1) * DivisionBy16);
                byte BlockedTiles = 0, Gap = 0;
                for(byte i = 0; i < 9; i++)
                {
                    Tile tile = Main.tile[TileX, TileY - i];
                    if(tile.HasTile && Main.tileSolid[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
                    {
                        BlockedTiles++;
                        Gap = 0;
                    }
                    else
                    {
                        if(i == 1)
                            BlockedTiles = 0;
                        Gap++;
                        if(Gap >= 3)
                        {
                            break;
                        }
                    }
                }
                if(BlockedTiles >= 1 && Gap >= 3)
                {
                    controlJump = true;
                }
            }
        }

        public void InitializeCompanion()
        {
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            name = Base.Name;
        }

        public void Teleport(Vector2 Destination)
        {
            position.X = Destination.X - width * 0.5f;
            position.Y = Destination.Y - height;
            fallStart = (int)(position.Y * DivisionBy16);
            immuneTime = 40;
            immuneNoBlink = true;
        }

        public bool AimAtTarget(Vector2 TargetPosition, int TargetWidth, int TargetHeight)
        {
            ChangeAimPosition(new Vector2(TargetPosition.X + TargetWidth * 0.5f, TargetPosition.Y + TargetHeight * 0.5f));
            Vector2 AimLocation = AimDirection + Center;
            return AimLocation.X >= TargetPosition.X && 
                AimLocation.Y >= TargetPosition.Y && 
                AimLocation.X < TargetPosition.X + TargetWidth && 
                AimLocation.Y < TargetPosition.Y + TargetHeight;
        }

        public void ChangeAimPosition(Vector2 NewPosition)
        {
            NewAimDirectionBackup = NewPosition - Center;
        }

        private void UpdateAimMovement()
        {
            if(NewAimDirectionBackup != AimDirection)
            {
                Vector2 Diference = NewAimDirectionBackup - AimDirection;
                float Distance = Diference.Length();
                if(Distance < 1)
                {
                    AimDirection = NewAimDirectionBackup;
                    return;
                }
                float MoveSpeed = 10;
                if(Distance < MoveSpeed)
                    MoveSpeed = Distance * 0.8f;
                Diference.Normalize();
                AimDirection += Diference * MoveSpeed;
            }
        }

        internal void OnSpawnOrTeleport()
        {
            Target = null;
            AimDirection = NewAimDirectionBackup = Vector2.Zero;
            if(this is TerraGuardian)
            {
                TerraGuardian tg = (TerraGuardian)this;
                tg.DeadBodyPosition = Vector2.Zero;
                tg.DeadBodyVelocity = Vector2.Zero;
            }
        }

        public virtual void HoldItem(Item item)
        {
            
        }

        public virtual void UseItemHitbox(Item item, ref Rectangle hitbox, ref bool noHitbox)
        {

        }

        public virtual void HoldStyle(Item item, Rectangle heldItemFrame)
        {
            
        }

        public virtual void UseStyle(Item item, Rectangle heldItemFrame)
        {
            
        }

        public virtual void DrawCompanion()
        {
            Main.spriteBatch.End();
            IPlayerRenderer rendererbackup = Main.PlayerRenderer;
            Main.PlayerRenderer = new LegacyPlayerRenderer();
            /*Main.PlayerRenderer.DrawPlayer(Main.Camera, pm.TestCompanion, pm.TestCompanion.position, 
            pm.TestCompanion.fullRotation, pm.TestCompanion.fullRotationOrigin);*/
            SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
            Main.PlayerRenderer.DrawPlayers(Main.Camera, new Player[]{ this });
            Main.PlayerRenderer = rendererbackup;
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
        }
    }
}