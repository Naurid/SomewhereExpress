using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int m_sceneIndex;
    public long m_lastUpdated;
    public Vector3 m_playerPostion;
    public Quaternion m_playerRotation;
    public List<ItemData> m_playerInventory;

    public SaveData()
    {
        m_sceneIndex = 1;
        m_playerPostion = Vector3.zero;
        m_playerInventory = new List<ItemData>();
        m_playerRotation = Quaternion.identity;
        
    }
}
