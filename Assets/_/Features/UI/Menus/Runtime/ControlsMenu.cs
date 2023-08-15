using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsMenu : MenuParent
{
    #region Main Methods

    public void ResetAllBindings()
    {
        foreach (var map in _inputActions.actionMaps) map.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("rebinds");
    }

    public void SetXSensitivity(float value)
    {
        _camera.m_XAxis.m_MaxSpeed = value;
    }

    public void SetYSensitivity(float value)
    {
        _camera.m_YAxis.m_MaxSpeed = value;
    }

    #endregion

    #region Private and protected

    [SerializeField] private InputActionAsset _inputActions;
    private CinemachineFreeLook _camera;

    #endregion
}