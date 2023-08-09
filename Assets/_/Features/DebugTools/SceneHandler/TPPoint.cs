using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPoint : MonoBehaviour
{
    private DebugTP _manager;
    private void OnEnable()
    {
        _manager = GameObject.FindObjectOfType<DebugTP>();
        _manager.m_tpPoints.Add(this);
    }

    private void OnDisable()
    {
        _manager.m_tpPoints.Remove(this);
    }
}
