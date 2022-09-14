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
        public const float DivisionBy16 = 1f / 16;
        public int WhoAmID = 0;
        public static int LastWhoAmID = 0;

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
        public Vector2 AimPosition = Vector2.Zero;
        public bool WalkMode = false;

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner == Main.myPlayer) || (Main.netMode == 2 && Owner == -1);
            }
        }

        private byte AIAction = 0;
        private byte AITime = 0;

        public void UpdateBehaviour()
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

        public void InitializeCompanion()
        {
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            Terraria.GameContent.Creative.CreativePowerManager.Instance.ResetDataForNewPlayer(this);
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