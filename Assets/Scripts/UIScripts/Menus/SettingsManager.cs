using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject gamepadPanel;
    [SerializeField] private GameObject keyboardPanel;
    [SerializeField] private Button close;
    [SerializeField] private Button audio;
    [SerializeField] private Slider volume;
    [SerializeField] private Toggle mute;
    private bool _currentPanel;
    
    public void MoveToPanel(GameObject origin, GameObject destination)
    {
        origin.SetActive(false);
        destination.SetActive(true);
    }
    
    public void DisablePanel(GameObject panel) => panel.SetActive(false);
    public void EnablePanel(GameObject panel) => panel.SetActive(true);

    public void SwitchControlPanel()
    {
        EnablePanel(!_currentPanel ? gamepadPanel : keyboardPanel);
        DisablePanel(_currentPanel ? gamepadPanel : keyboardPanel);
        _currentPanel = !_currentPanel;
    }


    
    public void ControlsNavToClose(Button button)
    {
        var buttonNavigation = button.navigation;
        buttonNavigation.selectOnDown = close;
    } 
    
    public void ControlsNavFromClose(Button button)
    {
        var buttonNavigation = button.navigation;
        buttonNavigation.selectOnUp = audio;
    }
    
    
    public void AudioNavToVolume(Button button)
    {
        var buttonNavigation = button.navigation;
        buttonNavigation.selectOnDown = volume;
    }
    
    public void AudioNavFromClose(Button button)
    {
        var buttonNavigation = button.navigation;
        buttonNavigation.selectOnUp = mute;
    } 
    
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    } 
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void SetShadowDistance(float shadowDistance)
    {
        QualitySettings.shadowDistance = shadowDistance;
    }
    
    public void SetShadowCascades(int shadowCascades)
    {
        QualitySettings.shadowCascades = shadowCascades;
    }
    
    public void SetShadowResolution(int shadowResolution)
    {
        switch (shadowResolution)
        {
            case 0:
                QualitySettings.shadowResolution = ShadowResolution.Low;
                break;
            case 1:
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case 2:
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
            case 3:
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                break;
        }
    }
    
    public void SetShadowProjection(int shadowProjection)
    {
        switch (shadowProjection)
        {
            case 0:
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                break;
            case 1:
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
                break;
        }
    }
}

