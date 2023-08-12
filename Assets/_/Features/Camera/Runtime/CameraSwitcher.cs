using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher m_instance;

    [SerializeField] private CinemachineFreeLook _outsideRig;
    [SerializeField] private CinemachineFreeLook _insideRig;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SwitchRig()
    {
        if (_outsideRig.m_Priority > _insideRig.m_Priority)
        {
            while (_outsideRig.m_Priority >= _insideRig.m_Priority)
            {
                _outsideRig.m_Priority--;
            }
        }
        else if (_insideRig.m_Priority > _outsideRig.m_Priority)
        {
            while (_insideRig.m_Priority >= _outsideRig.m_Priority)
            {
                _insideRig.m_Priority--;
            }
        }
    }
}
