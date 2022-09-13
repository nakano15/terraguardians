using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
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

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner == Main.myPlayer) || (Main.netMode == 2 && Owner == -1);
            }
        }

        public void UpdateBehaviour()
        {

        }

        public void InitializeCompanion()
        {
            savedPerPlayerFieldsThatArentInThePlayerClass = new SavedPlayerDataWithAnnoyingRules();
            Terraria.GameContent.Creative.CreativePowerManager.Instance.ResetDataForNewPlayer(this);
        }
    }
}