using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPoint : MonoBehaviour
{
    private void OnEnable()
    {
        DebugTP manager = GameObject.FindObjectOfType<DebugTP>();
        manager.m_tpPoints.Add(this);
    }

    private void OnDisable()
    {
        DebugTP manager = GameObject.FindObjectOfType<DebugTP>();
        manager.m_tpPoints.Remove(this);
    }
}
