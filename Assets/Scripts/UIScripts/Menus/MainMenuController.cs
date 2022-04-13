using Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace UIScripts.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _launchScreenSubtitles;
        [SerializeField] private Animator _FadeOutAnimator;
        [SerializeField] private GameObject _menuButtons;
        [SerializeField] private GameObject _gamepadSchema;
        [SerializeField] private GameObject _xeolIcon;

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
            _shopButton.GetComponent<Button>().onClick.AddListener(ShowSettings);
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

                if ((keyboardButtonPressed || gamepadButtonPressed) && _launchScreenSubtitles.activeSelf)
                {
                    StartSecondPhaseMenu();
                }
            }
        }

        void ShowSettings()
        {
            _xeolIcon.SetActive(false);
            _menuButtons.SetActive(false);
            _gamepadSchema.SetActive(true);
            _title.SetActive(false);
        }

        //void OnGUI() Esto no entiendo por qué quieres meterlo, y además peta, hace un nullreferenceException
        //{
        //    if ((Event.current.Equals(Event.KeyboardEvent("Escape")) || Gamepad.current[GamepadButton.East].isPressed))
        //    {
        //        _xeolIcon.SetActive(true);
        //        _menuButtons.SetActive(true);
        //        _gamepadSchema.SetActive(false);
        //        _title.SetActive(true);
        //    }

        //}

        public void ActivateMenu()
        {
            _menuButtons.SetActive(true);
        }

        private void StartSecondPhaseMenu()
        {
            isPhase2 = true;
            _launchScreenSubtitles.SetActive(false);
            _FadeOutAnimator.SetBool(LiteralToStringParse.FadeOut, true);
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

