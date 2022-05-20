using System.Collections;
using System.Collections.Generic;
using TOX.Audio;
using UnityEngine;

public class AudioMenuManager : MonoBehaviour
{
    #region  Singleton
    private static AudioMenuManager _instance;
    
    public static AudioMenuManager Instance
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
    
    private AudioSource _audioSource;
    [SerializeField] private AudioControllerData audioController;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        ToggleMute();
        SetVolume();
    }

    public void PlayOneShot(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void ToggleMute() => _audioSource.mute = !audioController.isOn;
    public void SetVolume() => _audioSource.volume = audioController.volume;

}
