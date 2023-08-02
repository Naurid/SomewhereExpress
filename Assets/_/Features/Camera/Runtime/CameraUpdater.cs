using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraUpdater : MonoBehaviour
{
    void Start()
    {
        Brain = GetComponent<CinemachineBrain>();
    }
    
    void Update()
    {
        Brain.ManualUpdate();
    }

    private CinemachineBrain Brain;
}
