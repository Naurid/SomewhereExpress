using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugTP : MonoBehaviour
{
    public List<TPPoint> m_tpPoints;
    [SerializeField] private GameObject _player;

    private void OnGUI()
    {
        foreach (var point in m_tpPoints)
        {
            if (GUILayout.Button(point.name))
            {
                _player.transform.position = point.transform.position;
            }
        }
    }
}
