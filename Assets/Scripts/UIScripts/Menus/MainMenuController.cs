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
        [SerializeField] private GameObject _fogVfx;
        
        [SerializeField] private GameObject _trainingButton;
        [SerializeField] private GameObject _multiplayerButton;
        [SerializeField] private GameObject _settingsButton;
        [SerializeField] private GameObject _spectatorButton;

        [SerializeField] private GameObject _trainingCursors;
        [SerializeField] private GameObject _multiplayerCursors;
        [SerializeField] private GameObject _settingsCursors;
        [SerializeField] private GameObject _spectatorCursors;


        private bool isPhase2;

        public GameObject TrainingButton { get => _trainingButton; set => _trainingButton = value; }
        public GameObject MultiplayerButton { get => _multiplayerButton; set => _multiplayerButton = value; }
        public GameObject SettingsButton { get => _settingsButton; set => _settingsButton = value; }
        public GameObject SpectatorButton { get => _spectatorButton; set => _spectatorButton = value; }
        public GameObject TrainingCursors { get => _trainingCursors; set => _trainingCursors = value; }
        public GameObject MultiplayerCursors { get => _multiplayerCursors; set => _multiplayerCursors = value; }
        public GameObject SettingsCursors { get => _settingsCursors; set => _settingsCursors = value; }
        public GameObject SpectatorCursors { get => _spectatorCursors; set => _spectatorCursors = value; }

        private void Start()
        {
            StartCoroutine(ConcatAnimsWithMusic());
            SettingsButton.GetComponent<Button>().onClick.AddListener(ShowSettings);
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
            StartCoroutine(ActivateFog());
        }

        IEnumerator ActivateFog()
        {
            yield return new WaitForSeconds(1.325f);
            _fogVfx.SetActive(true);
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

