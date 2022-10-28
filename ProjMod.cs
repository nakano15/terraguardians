using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Audio;
using ReLogic.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class ProjMod : GlobalProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool InstancePerEntity => true;
        
        internal static int BackupMyPlayer = -1;
        public Companion ProjectileOwnerCompanion = null;

        public static bool IsThisCompanionProjectile(int proj, Companion companion)
        {
            return IsThisCompanionProjectile(Main.projectile[proj], companion);
        }

        public static bool IsThisCompanionProjectile(Projectile proj, Companion companion)
        {
            return proj.GetGlobalProjectile<ProjMod>().ProjectileOwnerCompanion == companion; //Need to rework this when proper script to recognize companions is implemented.
        }

        public override void SetDefaults(Projectile projectile)
        {
            ProjectileOwnerCompanion = Companion.GetReferedCompanion;
        }

        public override bool PreAI(Projectile projectile)
        {
            if (BackupMyPlayer == -1)
                BackupMyPlayer = Main.myPlayer;
            else
                Main.myPlayer = BackupMyPlayer;
            if(ProjectileOwnerCompanion != null)
            {
                Main.myPlayer = projectile.owner = ProjectileOwnerCompanion.whoAmI;
            }
            /*if(projectile.aiStyle == 19)
            {
                Main.NewText("Owner: "+Main.player[projectile.owner].name);
            }*/
            return base.PreAI(projectile);
        }
    }
}