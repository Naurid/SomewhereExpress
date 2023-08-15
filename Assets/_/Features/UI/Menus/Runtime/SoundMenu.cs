using UnityEngine;
using UnityEngine.Audio;

public class SoundMenu : MonoBehaviour
{
    #region Main Methods

    public void SetMasterVolume(float value)
    {
        _master.SetFloat("MasterVolume", value);
    }
    
    public void SetBGMVolume(float value)
    {
        _master.SetFloat("BGMVolume", value);
    }
    
    public void SetSFXVolume(float value)
    {
        _master.SetFloat("SFXVolume", value);
    }
    
    public void SetVoiceVolume(float value)
    {
        _master.SetFloat("VoiceVolume", value);
    }

    #endregion

    
    #region Private and protected

    [SerializeField] private AudioMixer _master;
    
    #endregion
}


