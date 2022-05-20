using Helpers;
using TOX.Audio;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        #region  Singleton
        private static AudioManager _instance;
    
        public static AudioManager Instance
        {
            get
            {
                return _instance;
            }
        }
    
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;
        }
        #endregion
    
        [SerializeField] private AudioStruct[] audiosStruct;
    
        private AudioSource _audioSource;
    
        [SerializeField] private AudioControllerData audioController;


        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            
            ToggleMute();
            SetVolume();
        }

        public void OneShot(Literals.AudioType audioType)
        {
            foreach (var audio in audiosStruct)
            {
                if (audio.AudioType == audioType)
                {
                    _audioSource.PlayOneShot(audio.AudioClip);
                }
            }
        }

        public void AtPoint(Literals.AudioType audioType, Vector3 vector3)
        {
            if (!audioController.isOn) return;
            foreach (var audio in audiosStruct)
            {
                if (audio.AudioType == audioType)
                {
                    AudioSource.PlayClipAtPoint(audio.AudioClip, vector3, audio.Volume * audioController.volume);
                }
            }
        }
    
        public void ToggleMute() => _audioSource.mute = !audioController.isOn;
        public void SetVolume() => _audioSource.volume = audioController.volume;
    }
}
