using System;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterFXManager : MonoBehaviour
    {
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;
        private PhotonView _photonView;

        private void Start()
        {
            _photonView = PhotonView.Get(gameObject);
        }

        [PunRPC]
        public virtual void PlayWeaponTrailFX(bool isLeft, bool isRemote = false)
        {
            if (!isRemote)
            {
                _photonView.RPC("PlayWeaponTrailFX", RpcTarget.Others, isLeft, true);
            }

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