using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public ItemData m_itemData;

    private SceneHandler _manager;

    private void Start()
    {
        _manager = SceneHandler.m_instance;
        _manager.m_objectsInScene.Add(this);
    }

    private void OnDestroy()
    {
        _manager.m_objectsInScene.Remove(this);
    }
}
