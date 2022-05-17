using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

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

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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

    public void PlayAtPoint(Literals.AudioType audioType, Vector3 vector3)
    {
        foreach (var audio in audiosStruct)
        {
            if (audio.AudioType == audioType)
            {
                AudioSource.PlayClipAtPoint(audio.AudioClip, vector3);
                Debug.Log("Play " + audio.AudioClip);
            }
        }
    }
}
