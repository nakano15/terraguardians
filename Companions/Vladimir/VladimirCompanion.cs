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
using System.Linq;

namespace terraguardians.Companions.Vladimir
{
    public class VladimirCompanion : TerraGuardian
    {
        public bool PickedUpPerson = false, CarrySomeone = false;
        public Entity CarriedCharacter = null;
        public int Duration = 0, Time = 0;
        public bool WasFollowingPlayerBefore = false;

        public string GetCarriedOneName
        {
            get
            {
                if (CarriedCharacter == null) return "Nobody";
                if (CarriedCharacter is NPC) return (CarriedCharacter as NPC).GivenOrTypeName;
                if (CarriedCharacter is Player)
                {
                    if (CarriedCharacter is Companion)
                        return (CarriedCharacter as Companion).GetNameColored();
                    return (CarriedCharacter as Player).name;
                }
                return "Unknown";
            }
        }

        public override void ModifyAnimation()
        {
            bool SharingThrone = false;
            if (IsUsingThroneOrBench)
            {
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Main.player[p] != this && Main.player[p].sitting.isSitting && Main.player[p].Bottom == Bottom)
                    {
                        SharingThrone = true;
                        break;
                    }
                }
            }
            if (SharingThrone || GetCharacterMountedOnMe != null || (CarrySomeone && PickedUpPerson))
            {
                if (GetGoverningBehavior() is Behaviors.Actions.MountDismountCompanionBehavior) return;
                short Frame = 1;
                switch (BodyFrameID)
                {
                    case 11:
                        Frame = 12;
                        break;
                    case 22:
                        Frame = 23;
                        break;
                    case 24:
                        Frame = 25;
                        break;
                    case 27:
                        Frame = 28;
                        break;
                    case 29:
                        Frame = 30;
                        break;
                    case 31:
                        Frame = 32;
                        break;
                }
                if (BodyFrameID == 0 || BodyFrameID == 11 || BodyFrameID == 22 || BodyFrameID == 24)
                    BodyFrameID = Frame;
                if (HeldItems[1].ItemAnimation == 0)
                {
                    ArmFramesID[1] = Frame;
                }
                if (itemAnimation == 0)
                {
                    ArmFramesID[0] = Frame;
                }
            }
        }

        public override void UpdateBehaviorHook()
        {
            if (dead || KnockoutStates > 0) return;
            UpdateCarryAlly();
        }

        public override void UpdateCompanionHook()
        {
            UpdateCarriedAllyPosition();
            if (GetCharacterMountedOnMe != null)
                GetCharacterMountedOnMe.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
        }

        private void UpdateCarryAlly()
        {
            if (!CarrySomeone)
            {
                if (Owner != null) return;
                TryCarryingSomeone();
                return;
            }
            if (!PickedUpPerson)
            {
                if (CarriedCharacter == null)
                {
                    CarrySomeone = false;
                    return;
                }
                if (IsRunningBehavior) return;
                if (Owner != null) WalkMode = true;
                Time++;
                Entity Target = CarriedCharacter;
                if (!Target.active)
                {
                    CarrySomeone = false;
                    return;
                }
                MoveLeft = false;
                MoveRight = false;
                if (Hitbox.Intersects(Target.Hitbox))
                {
                    PickedUpPerson = true;
                    Time = 0;
                    Path.CancelPathing(false);
                }
                else
                {
                    if (Path.State != PathFinder.PathingState.TracingPath)
                    {
                        CreatePathingTo(Target, WalkMode, CancelOnFail: true);
                    }
                    /*if (guardian.Center.X < Target.Center.X)
                    {
                        guardian.MoveRight = true;
                    }
                    else
                    {
                        guardian.MoveLeft = true;
                    }*/
                }
                if (!PickedUpPerson) return;
            }
            if (CarriedCharacter != null && CarriedCharacter is Player && IsBeingControlledBy((Player)CarriedCharacter))
            {
                PlaceCarriedPersonOnTheFloor();
                return;
            }
            if (!WasFollowingPlayerBefore)
            {
                if (!TargettingSomething)
                {
                    Time++;
                }
                if (Time >= Duration)
                {
                    PlaceCarriedPersonOnTheFloor(false);
                    return;
                }
            }
            if (itemAnimation > 0) controlTorch = false;
            if (WasFollowingPlayerBefore)
            {
                if (Owner == null)
                {
                    SaySomething("*[nickname] will still need your help, better you go with them.*");
                    PlaceCarriedPersonOnTheFloor(false);
                    return;
                }
            }
            else if (Owner != null)
            {
                SaySomething("It might be dangerous, better you stay here.*");
                PlaceCarriedPersonOnTheFloor(false);
                return;
            }
            if (CarriedCharacter == Owner)
            {
                
            }
        }

        private void UpdateCarriedAllyPosition()
        {
            if (!CarrySomeone) return;
            Entity Target = CarriedCharacter;
            if (Target == this || Target == null || !Target.active)
            {
                CarrySomeone = false;
                CarriedCharacter = null;
                return;
            }
            else if (PickedUpPerson)
            {
                if (KnockoutStates > KnockoutStates.Awake)
                {
                    PlaceCarriedPersonOnTheFloor();
                    return;
                }
                DrawOrderInfo.AddDrawOrderInfo(CarriedCharacter, this, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                if (Target is Companion)
                {
                    
                }
                if (Target is NPC)
                {
                    NPC npc = (NPC)Target;
                    if (!npc.active)
                    {
                        CarrySomeone = false;
                        CarriedCharacter = null;
                        return;
                    }
                    npc.position = GetMountShoulderPosition;
                    npc.position.X -= npc.width * 0.5f;
                    npc.position.Y -= npc.height * 0.5f + 8;
                    if (npc.velocity.X == 0)
                        npc.direction = -direction;
                    if (IsMountedOnSomething)
                        npc.position.X += 4 * direction * Scale;
                    npc.velocity = Vector2.Zero;
                    npc.velocity.Y = -0.3f;
                    npc.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                }
                else if (Target is TerraGuardian)
                {
                    TerraGuardian tg = (TerraGuardian)Target;
                    if (!tg.active)
                    {
                        CarrySomeone = false;
                        CarriedCharacter = null;
                        return;
                    }
                    //if (tg.itemAnimation <= 0)
                    //    tg.ChangeDir(direction);
                    if (tg.IsMountedOnSomething)
                    {
                        tg.ToggleMount(tg.GetCharacterMountedOnMe, true);
                    }
                    if (tg.UsingFurniture)
                        tg.LeaveFurniture();
                    Vector2 HeldPosition = GetMountShoulderPosition;
                    tg.position = HeldPosition;
                    tg.position.Y -= tg.height * 0.5f;
                    tg.position.X -= tg.width * 0.5f;
                    tg.velocity.X = 0;
                    tg.velocity.Y = -Player.defaultGravity;
                    if (IsMountedOnSomething)
                    {
                        tg.position.X += 4 * direction * Scale;
                    }
                    tg.gfxOffY = 0;
                    tg.SetFallStart();
                    if (tg.KnockoutStates > KnockoutStates.Awake)
                        tg.GetPlayerMod.ChangeReviveStack(3);
                    else
                        tg.IncreaseComfortStack(0.02f);
                    tg.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                }
                else
                {
                    Player p = (Player)Target;
                    if (!p.active)
                    {
                        CarrySomeone = false;
                        CarriedCharacter = null;
                        return;
                    }
                    p.position = GetMountShoulderPosition;
                    p.position.X -= p.width * 0.5f;
                    p.position.Y -= p.height * 0.5f + 8;
                    if (IsMountedOnSomething)
                        p.position.X += 4 * direction * Scale;
                    p.fallStart = (int)(p.position.Y * Companion.DivisionBy16);
                    p.velocity.X = 0;
                    p.velocity.Y = -Player.defaultGravity;
                    PlayerMod pm = p.GetModPlayer<PlayerMod>();
                    if (pm.KnockoutState > KnockoutStates.Awake)
                        pm.ChangeReviveStack(3);
                    if (p.itemAnimation == 0)
                        p.direction = -direction;
                    p.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                    if (p == MainMod.GetLocalPlayer && p.controlJump)
                    {
                        CarrySomeone = false;
                        CarriedCharacter = null;
                    }
                }
            }
        }

        private void TryCarryingSomeone()
        {
            if (!HasHouse || TargettingSomething || Dialogue.IsParticipatingDialogue(this) || IsRunningBehavior || Main.rand.Next(350) > 0)
                return;
            List<Entity> PotentialCharacters = new List<Entity>();
            const float SearchRange = 80;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].townNPC && (Main.npc[i].Center - Center).Length() < SearchRange + width * .5f)
                {
                    PotentialCharacters.Add(Main.npc[i]);
                }
            }
            for (int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && !(player is Companion) && PlayerMod.GetPlayerKnockoutState(player) == KnockoutStates.Awake && 
                    !IsHostileTo(player) && player != Owner && player.velocity.Length() == 0 && PlayerMod.PlayerGetControlledCompanion(player) == null && 
                    player.itemAnimation == 0 && (player.Center - Center).Length() < SearchRange)
                {
                    PotentialCharacters.Add(player);
                }
            }
            foreach (uint i in MainMod.ActiveCompanions.Keys)
            {
                Companion comp = MainMod.ActiveCompanions[i];
                if (comp.Owner == null && !comp.dead && comp.KnockoutStates == KnockoutStates.Awake &&
                    !comp.UsingFurniture && VladimirCanCarryThisCompanion(comp) &&
                    (comp.Center - Center).Length() < SearchRange)
                {
                    PotentialCharacters.Add(comp);
                }
            }
            if (PotentialCharacters.Count > 0)
            {
                int Time = Main.rand.Next(600, 1400) * 3;
                CarrySomeoneAction(PotentialCharacters[Main.rand.Next(PotentialCharacters.Count)], Time);
                PotentialCharacters.Clear();
            }
        }

        public bool VladimirCanCarryThisCompanion(Companion c)
        {
            return !c.IsSameID(CompanionDB.Vladimir) && !IsHostileTo(c) && c.height < height * .95f && !VladimirBase.CarryBlacklist.Any(x => x.IsSameID(c.GetCompanionID));
        }

        public void CarrySomeoneAction(Entity Target, int Time = 0, bool InstantPickup = false)
        {
            if (CarrySomeone)
            {
                PlaceCarriedPersonOnTheFloor();
                //Place on the ground
            }
            CarrySomeone = true;
            PickedUpPerson = InstantPickup;
            WasFollowingPlayerBefore = Owner != null;
            CarriedCharacter = Target;
            Duration = Time;
        }

        public void PlaceCarriedPersonOnTheFloor(bool WillPickupLater = false)
        {
            if (!CarrySomeone) return;
            if (WillPickupLater) PickedUpPerson = false;
            else CarrySomeone = false;
            CarriedCharacter.Bottom = Bottom;
            if (CarriedCharacter is Player)
                (CarriedCharacter as Player).fallStart = (int)(CarriedCharacter.position.Y * Companion.DivisionBy16);
            if (!CarrySomeone)
                CarriedCharacter = null;
        }

        public override void ModifyMountedCharacterPosition(Player MountedCharacter, ref Vector2 Position)
        {
            if (CarrySomeone)
            {
                Position.X -= 6f * direction;
            }
        }
    }
}