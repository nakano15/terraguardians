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
using System.Collections.Generic;

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
        private CompanionTownNpcState _TownNpcState;
        public CompanionTownNpcState GetTownNpcState
        {
            get
            {
                return _TownNpcState;
            }
        }
        public bool IsTownNpc
        {
            get
            {
                return _TownNpcState != null;
            }
        }

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
        public uint Index { get{ return Data.Index; } }
        public bool IsPlayerCharacter = false;
        public byte OutfitID { get { return Data.OutfitID; } set { Data.OutfitID = value; } }
        public byte SkinID { get { return Data.SkinID; } set { Data.SkinID = value; } }
        public Entity Owner = null;
        #region Useful getter setters
        public bool MoveLeft { get { return controlLeft; } set { controlLeft = value; } }
        public bool LastMoveLeft { get { return releaseLeft; } set { releaseRight = value; } }
        public bool MoveRight { get { return controlRight; } set { controlRight = value; } }
        public bool LastMoveRight { get { return releaseRight; } set { releaseRight = value; } }
        public bool MoveUp { get { return controlUp; } set { controlUp = value; } }
        public bool LastMoveUp { get { return releaseUp; } set { releaseUp = value; } }
        public bool MoveDown { get { return controlDown; } set { controlDown = value; } }
        public bool LastMoveDown { get { return releaseDown; } set { releaseDown = value; } }
        public bool ControlJump { get { return controlJump; } set { controlJump = value; } }
        public bool LastControlJump { get { return releaseJump; } set{ releaseJump = value; } }
        public bool ControlAction { get { return controlUseItem; } set { controlUseItem = value; } }
        public bool LastControlAction { get { return releaseUseItem; } set { releaseUseItem = value; } }
        #endregion
        #region Behaviors
        public BehaviorBase idleBehavior = new IdleBehavior(),
            combatBehavior = new CombatBehavior(),
            followBehavior = new FollowLeaderBehavior(),
            preRecruitBehavior = null;
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

        public bool IsFollower { get{ return Owner != null; }}

        public bool TargettingSomething { get { return Target != null; } }

        public string GetPlayerNickname(Player player)
        {
            return Data.GetPlayerNickname(player);
        }

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner is Player && ((Player)Owner).whoAmI == Main.myPlayer) || (Main.netMode == 2 && (Owner == null || Owner is NPC));
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

        public bool IsSameID (uint ID, string ModID = "")
        {
            return Data.IsSameID(ID, ModID);
        }

        public bool IsSameID (CompanionID ID)
        {
            return Data.IsSameID(ID);
        }

        private byte AIAction = 0;
        private byte AITime = 0;
        private float Time = 0;

        private float FireDirection = 0;

        public Companion() : base()
        {
            WhoAmID = LastWhoAmID++;
        }

        public string GetNameColored()
        {
            return Data.GetNameColored();
        }

        private static BitsByte _Behaviour_Flags;
        public static bool Behaviour_AttackingSomething
        {
            get
            {
                return _Behaviour_Flags[0];
            }
            set
            {
                _Behaviour_Flags[0] = value;
            }
        }

        public static bool Behaviour_InDialogue
        {
            get
            {
                return _Behaviour_Flags[1];
            }
            set
            {
                _Behaviour_Flags[1] = value;
            }
        }

        public void UpdateBehaviour()
        {
            _Behaviour_Flags = new BitsByte();
            MoveLeft = MoveRight = MoveUp = MoveDown = ControlJump = controlUseItem = false;
            LookForTargets();
            combatBehavior.Update(this);
            UpdateDialogueBehaviour();
            if(!Behaviour_AttackingSomething)
                ChangeAimPosition(Center + Vector2.UnitX * width * direction);
            if(Owner != null)
            {
                followBehavior.Update(this);
            }
            else 
            {
                if(!WorldMod.HasMetCompanion(ID, ModID) && preRecruitBehavior != null)
                {
                    preRecruitBehavior.Update(this);
                }
                else
                {
                    idleBehavior.Update(this);
                }
            }
            if(MoveLeft || MoveRight)
                CheckIfNeedToJumpTallTile();
        }

        private void UpdateDialogueBehaviour()
        {
            if(Dialogue.InDialogue && Dialogue.IsParticipatingDialogue(this))
            {
                if(Behaviour_AttackingSomething)
                {
                    if(Dialogue.Speaker == this)
                    {
                        Dialogue.EndDialogue();
                    }
                    return;
                }
                Behaviour_InDialogue = true;
                float CenterX = position.X + width * 0.5f;
                float WaitLocationX = MainMod.GetLocalPlayer.position.X + MainMod.GetLocalPlayer.width * 0.5f;
                float WaitDistance = width * 0.8f + 8;
                bool ToLeft = false;
                if(CenterX < WaitLocationX)
                {
                    WaitLocationX -= Dialogue.DistancingLeft + WaitDistance * 0.5f + 12;
                    Dialogue.DistancingLeft += WaitDistance;
                    ToLeft = true;
                }
                else
                {
                    WaitLocationX += Dialogue.DistancingRight + WaitDistance * 0.5f + 12;
                    Dialogue.DistancingRight += WaitDistance;
                }
                if((ToLeft && CenterX < WaitLocationX) || (!ToLeft && CenterX > WaitLocationX))
                {
                    if(CenterX < WaitLocationX)
                    {
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                    }
                }
                else
                {
                    if(velocity.X == 0 && velocity.Y == 0)
                    {
                        direction = ToLeft ? 1 : -1;
                    }
                }
            }
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
                    if(Distance < NearestDistance && CanHit(npc))
                    {
                        NewTarget = npc;
                        NearestDistance = Distance;
                    }
                }
            }
            if (NewTarget != null)
                Target = NewTarget;
        }

        private void FollowPlayerBehaviour()
        {
            if(Target != null) return;
            Vector2 OwnerPosition = Owner.Center;
            if(Math.Abs(OwnerPosition.X - Center.X) >= 500 || 
                Math.Abs(OwnerPosition.Y - Center.Y) >= 400)
            {
                Teleport(Owner.Bottom);
            }
            if(Behaviour_InDialogue || Behaviour_AttackingSomething)
                return;
            float Distancing = 0;
            if (Owner is Player)
            {
                PlayerMod pm = ((Player)Owner).GetModPlayer<PlayerMod>();
                float MyFollowDistance = width * 0.8f + 12;
                bool TakeBehindPosition = (Owner.direction > 0 && OwnerPosition.X < Center.X) || (Owner.direction < 0 && OwnerPosition.X > Center.X);
                if(TakeBehindPosition)
                {
                    Distancing = pm.FollowBehindDistancing + MyFollowDistance * 0.5f;
                    pm.FollowBehindDistancing += MyFollowDistance;
                }
                else
                {
                    Distancing = pm.FollowAheadDistancing + MyFollowDistance * 0.5f;
                    pm.FollowAheadDistancing += MyFollowDistance;
                }
            }
            if(Math.Abs((OwnerPosition.X - Center.X) - Owner.velocity.X) > 40 + Distancing)
            {
                if(OwnerPosition.X < Center.X)
                    MoveLeft = true;
                else
                    MoveRight = true;
            }
            WalkMode = false;
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
            name = Data.GetName;
            Male = Data.Gender == Genders.Male;
            inventory = Data.Inventory;
            armor = Data.Equipments;
            miscEquips = Data.MiscEquipment;
            dye = Data.EquipDyes;
            miscDyes = Data.MiscEquipDyes;
            statLifeMax = Data.MaxHealth;
            statManaMax = Data.MaxMana;
            buffType = Data.BuffType;
            buffTime = Data.BuffTime;
            DoResetEffects();
            statLife = statLifeMax2;
            statMana = statManaMax2;
            CheckIfHasNpcState();
            idleBehavior = Base.DefaultIdleBehavior;
            combatBehavior = Base.DefaultCombatBehavior;
            followBehavior = Base.DefaultFollowLeaderBehavior;
            preRecruitBehavior = Base.PreRecruitmentBehavior;
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

        ///<summary><para>
        ///Allows making a custom companion head drawing script.
        ///The custom drawing script is used by many of the mod scripts to draw a character head.</para>
        ///<param name="Position">The centered position of the head.</param>
        ///<param name="FacingLeft"> Wether the sprite should be facing left or not.</param>
        ///<param name="Scale"> The scale of the sprite.</param>
        ///<param name="MaxDimension"> If value is higher than 0, the sprite should be downscaled if either width or height is bigger than this value.</param>
        ///</summary>
        ///<returns>Return true if you made a custom head drawing script, to avoid drawing default Terrarian version.</returns>
        public virtual bool DrawCompanionHead(Vector2 Position, bool FacingLeft, float Scale = 1f, float MaxDimension = 0)
        {
            return false;
        }

        public void DrawCompanionInterfaceOnly(DrawContext context = DrawContext.AllParts, bool UseSingleDrawScript = false)
        {
            DoResetEffects();
            ResetVisibleAccessories();
            UpdateMiscCounter();
            UpdateDyes();
            UpdateAnimations();
            DrawCompanion(context, UseSingleDrawScript);
        }

        public virtual void DrawCompanion(DrawContext context = DrawContext.AllParts, bool UseSingleDrawScript = false)
        {
            if (!UseSingleDrawScript) Main.spriteBatch.End();
            //IPlayerRenderer rendererbackup = Main.PlayerRenderer;
            LegacyPlayerRenderer renderer = new LegacyPlayerRenderer();
            //Main.PlayerRenderer = new LegacyPlayerRenderer();
            SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
            TerraGuardianDrawLayersScript.Context = context;
            if(!UseSingleDrawScript)
            {
                renderer.DrawPlayers(Main.Camera, new Player[]{ this });
            }
            else
            {
                renderer.DrawPlayer(Main.Camera, this, position, fullRotation, fullRotationOrigin);
            }
            //Main.PlayerRenderer = rendererbackup;
            if (!UseSingleDrawScript) Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if(direction < 0)
                Position.X = Base.SpriteWidth - Position.X;
            if(gravDir < 0)
                Position.Y = Base.SpriteHeight - Position.Y;
            Position *= Scale;
            Position.X += (width - Base.SpriteWidth * Scale) * 0.5f;
            Position.Y += height - Base.SpriteHeight * Scale;
            if(AlsoTakePosition)
                Position += position + Vector2.UnitY * HeightOffsetHitboxCenter;
            return Position;
        }

        public Vector2 GetBetweenAnimationPosition(AnimationPositions Animation, short Frame, bool AlsoTakePosition = true)
        {
            if(Base.GetHands <= 1)
                return GetAnimationPosition(Animation, Frame, 0, AlsoTakePosition);
            Vector2 OriginPosition = GetAnimationPosition(Animation, Frame, 0, false);
            Vector2 Position = OriginPosition + (GetAnimationPosition(Animation, Frame, 1, false) - OriginPosition) * 0.5f;
            if (AlsoTakePosition)
                Position += position + Vector2.UnitY * HeightOffsetHitboxCenter;
            return Position;
        }

        public void CheckIfHasNpcState()
        {
            foreach(CompanionTownNpcState npcState in WorldMod.CompanionNPCsInWorld)
            {
                if (npcState != null && npcState.CharID.IsSameID(npcState.CharID))
                {
                    if(IsSameID(npcState.CharID))
                    {
                        ChangeTownNpcState(npcState);
                        return;
                    }
                }
            }
            ChangeTownNpcState(null);
        }

        public void ChangeTownNpcState(CompanionTownNpcState NewState)
        {
            _TownNpcState = NewState;
        }

        public void DrawOverheadMessage()
        {
            if(chatOverhead.timeLeft > 0 && !dead)
            {
                Vector2 MessageSize = chatOverhead.messageSize;
                Vector2 Position = Top.ToScreenPosition();
                if(gravDir == -1f) Position.Y += 2;
                Position.X -= MessageSize.X * 0.5f;
                Position.Y += gfxOffY - (MessageSize.Y + 2);
                Position = Position.Floor();
                Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, 
                    chatOverhead.snippets, Position, 0, chatOverhead.color, Vector2.Zero, Vector2.One, out int hover);
            }
        }

        public CompanionDrawMomentTypes GetDrawMomentType()
        {
            if(Owner != null)
            {
                return CompanionDrawMomentTypes.DrawBehindOwner;
            }
            return CompanionDrawMomentTypes.AfterTiles;
        }
    }

    public enum CompanionDrawMomentTypes : byte
    {
        AfterTiles,
        DrawBehindOwner,
        DrawInBetweenOwner,
        DrawInFrontOfOwner
    }
}