using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraSwitcher _switcher;
    void Start()
    {
        _switcher = CameraSwitcher.m_instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _switcher.SwitchRig();
        }
    }
}
