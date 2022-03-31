using Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UIScripts.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _pressAnyButtonText;
        [SerializeField] private Animator _FadeOutAnimator;

        private bool isPhase2;

        private void Start()
        {
            StartCoroutine(ConcatAnimsWithMusic());
        }

        private void Update()
        {
            if (isPhase2) return;
            if(Keyboard.current.anyKey.wasPressedThisFrame)
            {
                isPhase2 = true;
                StartSecondPhaseMenu();
            }
        }

        private void StartSecondPhaseMenu()
        {
            _pressAnyButtonText.SetActive(false);
            _FadeOutAnimator.SetBool(LiteralToStringParse.FadeOut, true);
        }

        IEnumerator ConcatAnimsWithMusic()
        {
            yield return new WaitForSeconds(3.3f);
            _title.SetActive(true);
            yield return new WaitForSeconds(2f);
            _pressAnyButtonText.SetActive(true);
        }
    }
}

