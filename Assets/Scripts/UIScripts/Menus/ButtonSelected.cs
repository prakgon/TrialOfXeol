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
                _shopCursors.SetActive(false);
                _quickGameCursors.SetActive(true);
                _customGameCursors.SetActive(false);
                _optionsCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_customGameButton))
            {
                _shopCursors.SetActive(false);
                _customGameCursors.SetActive(true);
                _quickGameCursors.SetActive(false);
                _optionsCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_shopButton))
            {
                _shopCursors.SetActive(true);
                _customGameCursors.SetActive(false);
                _quickGameCursors.SetActive(false);
                _optionsCursors.SetActive(false);
            }

            else if (currentSelectedGO.Equals(_optionsButton))
            {
                _shopCursors.SetActive(false);
                _optionsCursors.SetActive(true);
                _quickGameCursors.SetActive(false);
                _customGameCursors.SetActive(false);
            }
        }
    }
}
