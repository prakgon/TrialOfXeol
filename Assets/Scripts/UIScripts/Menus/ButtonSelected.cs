using UnityEngine.EventSystems;

namespace UIScripts.Menus
{
    public class ButtonSelected : MainMenuController, ISelectHandler
    {
        private void Start()
        {
            //This empty start avoids a bug, don't ask why
        }
        public void OnSelect(BaseEventData eventData)
        {
            var currentSelectedGO = EventSystem.current.currentSelectedGameObject;

            if (currentSelectedGO == null) return;
            if (currentSelectedGO.Equals(_quickGameButton))
            {
                _quickGameCursors.SetActive(true);
                _customGameCursors.SetActive(false);
                _optionsCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_customGameButton))
            {
                _customGameCursors.SetActive(true);
                _quickGameCursors.SetActive(false);
                _optionsCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_optionsButton))
            {
                _optionsCursors.SetActive(true);
                _quickGameCursors.SetActive(false);
                _customGameCursors.SetActive(false);
            }
        }
    }
}
