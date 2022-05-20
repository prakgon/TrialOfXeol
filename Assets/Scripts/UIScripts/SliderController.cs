using System.Collections;
using System.Collections.Generic;
using Audio;
using TOX.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private AudioControllerData audioController;

    [SerializeField] private Slider slider;

    void Start()
    {
        slider.maxValue = 1;
        slider.minValue = 0;

        slider.value = audioController.volume;
    }

    public void OnValueChanged() => audioController.volume = slider.value;


    public void CallManager() => AudioManager.Instance.SetVolume();


    public void CallMenuManager() => AudioMenuManager.Instance.SetVolume();
}