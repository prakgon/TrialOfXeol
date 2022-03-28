using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterFXManager : MonoBehaviour
    {
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;
        public virtual void PlayWeaponTrailFX(bool isLeft)
        {
            if (isLeft == false)
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponTrailFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponTrailFX();
                }
            }
        }
        public virtual void PlayWeaponGlowFX(bool isLeft)
        {
            if (isLeft == false)
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponGlowFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponGlowFX();
                }
            }
        }
    }
}