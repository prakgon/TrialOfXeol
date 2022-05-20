using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Helpers;
using UnityEngine;

public class HornAudioPlayer : MonoBehaviour
{
    public float timeInterval = 60f;
    
    private void Start()
    {
        PlayHorn();
    }

    private void PlayHorn()
    {
        AudioManager.Instance.OneShot(Literals.AudioType.Horn);
        StartCoroutine(WaitForHorn());
    }

    private IEnumerator WaitForHorn()
    {
        yield return new WaitForSeconds(timeInterval);
        PlayHorn();
    }
}
    
