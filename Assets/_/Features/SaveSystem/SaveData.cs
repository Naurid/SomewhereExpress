using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SaveData
{
    public int m_sceneIndex;

    public int m_numberOfObjects;
    public Vector3 m_playerPostion;
    public Quaternion m_playerRotation;
    public List<ItemData> m_playerInventory;

    public SaveData()
    {
        m_numberOfObjects = 0;
        m_sceneIndex = 0;
        m_playerPostion = Vector3.zero;
        m_playerInventory = new List<ItemData>();
        m_playerRotation = Quaternion.identity;
        
    }
}
