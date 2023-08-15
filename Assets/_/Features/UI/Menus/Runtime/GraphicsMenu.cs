using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphicsMenu : MenuParent
{
    #region Unity API

    private void Start()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropDown.ClearOptions();

        List<string> options = new();

        var index = 0;

        foreach (var resolution in _resolutions)
        {
            var option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height) index = _resolutions.ToList().IndexOf(resolution);
        }

        _resolutionDropDown.AddOptions(options);
        _resolutionDropDown.value = index;
        _resolutionDropDown.RefreshShownValue();
    }

    #endregion

    #region Main Methods

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
        var resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
    }

    #endregion

    #region Private and protected

    private Resolution[] _resolutions;
    private bool _fullScreen;
    [SerializeField] private TMP_Dropdown _resolutionDropDown;

    #endregion
}