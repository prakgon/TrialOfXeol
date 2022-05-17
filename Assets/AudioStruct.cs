using System;
using Helpers;
using UnityEngine;

[Serializable]
public struct AudioStruct
{
    public Literals.AudioType AudioType;
    public AudioClip AudioClip;

    public AudioStruct(Literals.AudioType audioType, AudioClip audioClip)
    {
        AudioType = audioType;
        AudioClip = audioClip;
    }
}