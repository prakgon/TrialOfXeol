using Audio;
using TOX.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ToggleController : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private GameObject onToggle;
        [SerializeField] private GameObject offToggle;

        [SerializeField] private AudioControllerData audioData;

        void Start()
        {
            toggle.isOn = audioData.isOn;
            
            ToggleValueChanged();
            CallMenuManager();
        }


        public void ToggleValueChanged()
        {
            onToggle.SetActive(toggle.isOn);
            offToggle.SetActive(!toggle.isOn);
        }

        public void SetIsOnData() => audioData.isOn = toggle.isOn;
        
        public void CallManager() => AudioManager.Instance.ToggleMute();

        public void CallMenuManager() => AudioMenuManager.Instance.ToggleMute();
    }
}