using Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UIScripts.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _pressAnyButtonText;
        [SerializeField] private Animator _FadeOutAnimator;
        [SerializeField] private GameObject _menuButtons;

        [SerializeField] private GameObject _justPlayButton;
        [SerializeField] private GameObject _customGameButton;
        [SerializeField] private GameObject _optionsButton;

        [SerializeField] private GameObject _justPlayCursors;
        [SerializeField] private GameObject _customGameCursors;
        [SerializeField] private GameObject _optionsCursors;
        private bool isPhase2;

        private void Start()
        {
            StartCoroutine(ConcatAnimsWithMusic());
        }

        private void Update()
        {
            if(!isPhase2)
            {
                var gamepadButtonPressed = Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);
                var keyboardButtonPressed = Keyboard.current.anyKey.wasPressedThisFrame;
                if (keyboardButtonPressed || gamepadButtonPressed)
                {
                    StartSecondPhaseMenu();
                }
            }

            else
            {
                var currentSelectedGO = EventSystem.current.currentSelectedGameObject;

                if(currentSelectedGO.Equals(_justPlayButton))
                {
                    _customGameCursors.SetActive(false);
                    _optionsCursors.SetActive(false);
                }

                else if (currentSelectedGO.Equals(_customGameButton))
                {
                    _justPlayCursors.SetActive(false);
                    _optionsCursors.SetActive(false);
                }

                else if(currentSelectedGO.Equals(_optionsButton))
                {
                    _justPlayCursors.SetActive(false);
                    _customGameCursors.SetActive(false);
                }
            }
        }

        private void StartSecondPhaseMenu()
        {
            isPhase2 = true;
            _pressAnyButtonText.SetActive(false);
            _FadeOutAnimator.SetBool(LiteralToStringParse.FadeOut, true);
            _menuButtons.SetActive(true);
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

