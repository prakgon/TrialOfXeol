using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts.Menus
{
    public class ButtonSelected : MonoBehaviour, ISelectHandler
    {
        [SerializeField] private MainMenuController _mainMenuController;
        private GameObject _trainingButton;
        private GameObject _multiplayerButton;
        private GameObject _settingsButton;
        private GameObject _spectatorButton;
        private GameObject _trainingCursors;
        private GameObject _multiplayerCursors;
        private GameObject _settingsCursors;
        private GameObject _spectatorCursors;

        private void Start()
        {
            _trainingButton = _mainMenuController.TrainingButton;
            _multiplayerButton = _mainMenuController.MultiplayerButton;
            _settingsButton = _mainMenuController.SettingsButton;
            _spectatorButton = _mainMenuController.SpectatorButton;
            _trainingCursors = _mainMenuController.TrainingCursors;
            _multiplayerCursors = _mainMenuController.MultiplayerCursors;
            _settingsCursors = _mainMenuController.SettingsCursors;
            _spectatorCursors = _mainMenuController.SpectatorCursors;
        }
        public void OnSelect(BaseEventData eventData)
        {
            var currentSelectedGO = EventSystem.current.currentSelectedGameObject;

            if (currentSelectedGO == null) return;
            if (currentSelectedGO.Equals(_trainingButton))
            {
                _settingsCursors.SetActive(false);
                _trainingCursors.SetActive(true);
                _multiplayerCursors.SetActive(false);
                _spectatorCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_multiplayerButton))
            {
                _settingsCursors.SetActive(false);
                _multiplayerCursors.SetActive(true);
                _trainingCursors.SetActive(false);
                _spectatorCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_settingsButton))
            {
                _settingsCursors.SetActive(true);
                _multiplayerCursors.SetActive(false);
                _trainingCursors.SetActive(false);
                _spectatorCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_spectatorButton))
            {
                _spectatorCursors.SetActive(true);
                _multiplayerCursors.SetActive(false);
                _trainingCursors.SetActive(false);
                _settingsCursors.SetActive(false);
            }
        }
    }
}
