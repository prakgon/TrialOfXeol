using System;
using Helpers;
using UnityEngine;

[Serializable]
public struct AudioStruct
{
    public Literals.AudioType AudioType;
    public AudioClip AudioClip;
    [Range(0,1)] public float Volume;

    public AudioStruct(Literals.AudioType audioType, AudioClip audioClip, float volume)
    {
        AudioType = audioType;
        AudioClip = audioClip;
        Volume = volume;
    }
}