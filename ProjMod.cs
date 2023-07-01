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
        private static ProjMod UpdateProjectile = null;

        private static ProjectilePlayerMaskHolder PlayerMask = null;

        protected override bool CloneNewInstances => false;
        public override bool InstancePerEntity => true;
        
        public Companion ProjectileOwnerCompanion = null;

        public static bool IsThisCompanionProjectile(int proj, Companion companion)
        {
            return IsThisCompanionProjectile(Main.projectile[proj], companion);
        }

        public static Player GetThisProjectileOwner(int proj)
        {
            Player p = Main.projectile[proj].GetGlobalProjectile<ProjMod>().ProjectileOwnerCompanion;
            if (p == null) p = Main.player[Main.projectile[proj].owner];
            return p;
        }

        public static bool IsThisCompanionProjectile(Projectile proj, Companion companion)
        {
            return proj.GetGlobalProjectile<ProjMod>().ProjectileOwnerCompanion == companion; //Need to rework this when proper script to recognize companions is implemented.
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            ProjectileOwnerCompanion = Companion.GetReferedCompanion;
            if (ProjectileOwnerCompanion == null && !projectile.npcProj)
            {
                if (Main.player[projectile.owner] is Companion)
                    ProjectileOwnerCompanion = Main.player[projectile.owner] as Companion;
            }
        }

        public override void SetDefaults(Projectile projectile)
        {
        }

        public override bool PreAI(Projectile projectile)
        {
            Main.myPlayer = MainMod.MyPlayerBackup;
            if(ProjectileOwnerCompanion != null)
            {
                projectile.owner = ProjectileOwnerCompanion.whoAmI;
                Main.myPlayer = projectile.owner;
                SystemMod.BackupMousePosition();
                Vector2 AimPosition = ProjectileOwnerCompanion.GetAimedPosition;
                Main.mouseX = (int)(AimPosition.X - Main.screenPosition.X);
                Main.mouseY = (int)(AimPosition.Y - Main.screenPosition.Y);
            }
            Companion.GetReferedCompanion = ProjectileOwnerCompanion;
            UpdateProjectile = this;
            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            SystemMod.RevertMousePosition();
            UpdateCompanionHitChecking(projectile);
            UpdateProjectile = null;
            Companion.GetReferedCompanion = null;
        }

        private void UpdateCompanionHitChecking(Projectile proj)
        {
            bool? hitAnything = ProjectileLoader.CanDamage(proj);
            if (hitAnything.HasValue && !hitAnything.Value) return;
            Rectangle hitbox = new Rectangle((int)proj.position.X, (int)proj.position.Y, proj.width, proj.height);
            if (proj.type == 85 || proj.type == 101)
            {
                const int Extension = 30;
                hitbox.X -= Extension;
                hitbox.Y -= Extension;
                hitbox.Width += Extension * 2;
                hitbox.Height += Extension * 2;
            }
            if (proj.type == 188)
            {
                const int Extension = 20;
                hitbox.X -= Extension;
                hitbox.Y -= Extension;
                hitbox.Width += Extension * 2;
                hitbox.Height += Extension * 2;
            }
            if (proj.aiStyle == 29)
            {
                const int Extension = 4;
                hitbox.X -= Extension;
                hitbox.Y -= Extension;
                hitbox.Width += Extension * 2;
                hitbox.Height += Extension * 2;
            }
            ProjectileLoader.ModifyDamageHitbox(proj, ref hitbox);
            int CooldownType = -1;
            switch(proj.type)
            {
                case 452:
                case 454:
                case 455:
                case 462:
                case 871:
                case 872:
                case 873:
                case 874:
                case 919:
                case 923:
                case 924:
                    CooldownType = 1;
                    break;
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(!c.dead && c.IsLocalCompanion && proj.damage > 0 && proj.hostile && !IsThisCompanionProjectile(proj, c))
                {
                    if(proj.Colliding(hitbox, c.getRect()) && ProjectileLoader.CanHitPlayer(proj, c) && PlayerLoader.CanBeHitByProjectile(c, proj))
                    {
                        if (!c.CanParryAgainst(c.Hitbox, proj.Hitbox, proj.velocity))
                        {
                            int direction = c.Center.X < proj.Center.X ? -1 : 1;
                            int damage = Main.DamageVar(proj.damage, -c.luck);
                            int bannerid = proj.bannerIdToRespondTo;
                            if (bannerid > 0 && c.HasNPCBannerBuff(bannerid))
                            {
                                ItemID.BannerEffect effect = ItemID.Sets.BannerStrength[Item.BannerToItem(bannerid)];
                                damage = ((!Main.expertMode) ? (int)((float)damage * effect.NormalDamageReceived) : (int)((float)damage * effect.ExpertDamageReceived));
                            }
                            if(proj.coldDamage && c.resistCold)
                            {
                                damage = (int)(damage * 0.7f);
                            }
                            float damagemult = Main.GameModeInfo.EnemyDamageMultiplier;
                            if (Main.GameModeInfo.IsJourneyMode)
                            {
                                Terraria.GameContent.Creative.CreativePowers.DifficultySliderPower power = Terraria.GameContent.Creative.CreativePowerManager.Instance.GetPower<Terraria.GameContent.Creative.CreativePowers.DifficultySliderPower>();
                                if (power.GetIsUnlocked())
                                    damagemult = power.StrengthMultiplierToGiveNPCs;
                            }
                            damage = (int)(damage * damagemult);
                            damage *= 2;
                            if (proj.type == 961)
                            {
                                if (proj.penetrate == 1)
                                {
                                    proj.damage = 0;
                                    proj.penetrate = -1;
                                }
                                else
                                {
                                    proj.damage = (int)(proj.damage * 0.7f);
                                }
                            }
                            bool crit = false;
                            ProjectileLoader.ModifyHitPlayer(proj, c, ref damage, ref crit);
                            PlayerLoader.ModifyHitByProjectile(c, proj, ref damage, ref crit);
                            int FinalDamage = (int)c.Hurt(PlayerDeathReason.ByProjectile(-1, proj.whoAmI), damage, direction, cooldownCounter: CooldownType);
                            if (FinalDamage > 0 && !c.dead)
                            {
                                proj.StatusPlayer(c.whoAmI);
                            }
                            ProjectileLoader.OnHitPlayer(proj, c, FinalDamage, crit);
                            PlayerLoader.OnHitByProjectile(c, proj, FinalDamage, crit);
                        }
                        switch(proj.type)
                        {
                            case 435:
                            case 436:
                            case 437:
                            case 682:
                                proj.penetrate--;
                                break;
                            case 681:
                                proj.timeLeft = 0;
                                break;
                        }
                    }
                }
            }
        }

        /*public void DoMask(Companion companion)
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
        }*/

        public class ProjectilePlayerMaskHolder
        {
            public Player OriginalPlayer;
            public int PlayerIndex;
        }
    }
}