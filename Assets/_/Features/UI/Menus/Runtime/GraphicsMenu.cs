using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphicsMenu : MonoBehaviour
{
    private Resolution[] _resolutions;
    private bool _fullScreen;
    [SerializeField] private TMP_Dropdown _resolutionDropDown;
    private void Start()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropDown.ClearOptions();

        List<string> options = new();

        int index = 0; 
        
        foreach (var resolution in _resolutions)
        {
            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                index = _resolutions.ToList().IndexOf(resolution);
            }
            
        }
        
        _resolutionDropDown.AddOptions(options);
        _resolutionDropDown.value = index;
        _resolutionDropDown.RefreshShownValue();
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        _fullScreen = isFullScreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
    }
}
