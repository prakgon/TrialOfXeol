using Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UIScripts.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _launchScreenSubtitles;
        [SerializeField] private Animator _FadeOutAnimator;
        [SerializeField] private GameObject _menuButtons;

        [SerializeField] protected GameObject _quickGameButton;
        [SerializeField] protected GameObject _customGameButton;
        [SerializeField] protected GameObject _shopButton;
        [SerializeField] protected GameObject _optionsButton;

        [SerializeField] protected GameObject _quickGameCursors;
        [SerializeField] protected GameObject _customGameCursors;
        [SerializeField] protected GameObject _shopCursors;
        [SerializeField] protected GameObject _optionsCursors;
        
        private bool isPhase2;

        private void Start()
        {
            StartCoroutine(ConcatAnimsWithMusic());
        }

        private void Update()
        {
            //TODO: Make this any key check event-listener instead of doing it constantly inside the update
            if (!isPhase2)
            {
                var keyboardButtonPressed = Keyboard.current.anyKey.wasPressedThisFrame;
                var gamepadButtonPressed = false;

                if (Gamepad.current != null)
                {
                    gamepadButtonPressed = Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);
                }

                if (keyboardButtonPressed || gamepadButtonPressed)
                {
                    StartSecondPhaseMenu();
                }
            }
        }

        private void StartSecondPhaseMenu()
        {
            isPhase2 = true;
            _launchScreenSubtitles.SetActive(false);
            _FadeOutAnimator.SetBool(LiteralToStringParse.FadeOut, true);
            _menuButtons.SetActive(true);
        }

        IEnumerator ConcatAnimsWithMusic()
        {
            yield return new WaitForSeconds(3.3f);
            _title.SetActive(true);
            yield return new WaitForSeconds(2f);
            _launchScreenSubtitles.SetActive(true);
        }
    }
}

