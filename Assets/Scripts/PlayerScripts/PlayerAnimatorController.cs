using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace PlayerScripts
{
    public class PlayerAnimatorController : MonoBehaviour, IMediatorUser
    {
        private PlayerMediator _med;
        private Animator _an;

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
        }

        private void Start()
        {
            _an = GetComponent<Animator>();
        }

        private string GetCurrentAnimatorState(byte layer = 0)
        {
            return _an.GetCurrentAnimatorStateInfo(layer).ToString();
        }

        private bool isTransitioning(byte layer = 0)
        {
            return _an.IsInTransition(layer);
        }
    }
}
