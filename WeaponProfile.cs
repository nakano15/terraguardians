using Terraria;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class WeaponProfile
    {
        public float AttackRange = -1;
        public int LaunchLimit = -1;
        public bool IsFlail
        {
            get
            {
                return _weaponType == WT_Flail;
            }
            set
            {
                _weaponType = WT_Flail;
            }
        }
        byte _weaponType = 0;
        const byte WT_NoSpecification = 0, WT_Flail = 1;

        //Need a way of telling how important the item is.
        //Need a way of showing how to use the item and who to target.

        public WeaponProfile SetTilesInRange(float TileRange)
        {
            AttackRange = TileRange * 16;
            return this;
        }

        public WeaponProfile SetLaunchLimit(int NewLimit)
        {
            LaunchLimit = NewLimit;
            return this;
        }

        public WeaponProfile SetFlail()
        {
            IsFlail = true;
            return this;
        }

        public virtual bool IsSpecialWeapon(Companion companion, Item item)
        {
            return false;
        }
        
        public virtual bool CanUseSpecialWeapon(Companion companion, Item item)
        {
            return false;
        }

        public virtual void WeaponUsageCustomBehavior(Companion companion, Item item, ref CombatBehavior.BehaviorFlags Flags)
        {
            
        }
    }
}