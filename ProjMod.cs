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
        private static ProjectilePlayerMaskHolder PlayerMask = null;

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
                Main.NewText("Owner Item Animation: "+Main.player[projectile.owner].itemAnimation);
            }*/
            return base.PreAI(projectile);
        }

        public void DoMask(Companion companion)
        {
            RevertMasking();
            PlayerMask = new ProjectilePlayerMaskHolder(){ OriginalPlayer = Main.player[companion.whoAmI], PlayerIndex = companion.whoAmI };
            Main.player[companion.whoAmI] = companion;
        }

        public void RevertMasking()
        {
            if (PlayerMask != null)
            {
                Main.player[PlayerMask.PlayerIndex] = PlayerMask.OriginalPlayer;
                PlayerMask = null;
            }
        }

        public override bool PreDrawExtras(Projectile projectile)
        {
            if(ProjectileOwnerCompanion != null)
            {
                DoMask(ProjectileOwnerCompanion);
            }
            return base.PreDrawExtras(projectile);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if(ProjectileOwnerCompanion != null)
            {
                lightColor = Lighting.GetColor((int)(projectile.Center.X * (1f / 16)), (int)(projectile.Center.Y * (1f / 16))); //Necessary for showing projectile lighting correctly. No idea why its native coloring doesn't work.
            }
            return base.PreDraw(projectile, ref lightColor);
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            RevertMasking();
        }

        public class ProjectilePlayerMaskHolder
        {
            public Player OriginalPlayer;
            public int PlayerIndex;
        }
    }
}