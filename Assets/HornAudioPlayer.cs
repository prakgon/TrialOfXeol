using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornAudioPlayer : MonoBehaviour
{
    public AudioClip hornClip;
    public AudioSource hornAudioSource;
    public float timeInterval = 60f;
    
    private void Start()
    {
        PlayHorn();
    }

    private void PlayHorn()
    {
        hornAudioSource.PlayOneShot(hornClip);
        StartCoroutine(WaitForHorn());
    }

    private IEnumerator WaitForHorn()
    {
        yield return new WaitForSeconds(timeInterval);
        PlayHorn();
    }
}
    
