using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _master;

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
}


