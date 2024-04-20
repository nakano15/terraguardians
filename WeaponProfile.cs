using Terraria;

namespace terraguardians
{
    public class WeaponProfile
    {
        public float AttackRange = -1;
        public int LaunchLimit = -1;

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

        public virtual bool IsSpecialWeapon(Companion companion, Item item)
        {
            return false;
        }
        
        public virtual bool CanUseSpecialWeapon(Companion companion, Item item)
        {
            return false;
        }
    }
}