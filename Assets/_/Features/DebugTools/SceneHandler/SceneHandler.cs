using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour,IDataPersistence
{
    public static SceneHandler m_instance;
    private int _currentSceneIndex;

    public List<ItemContainer> m_objectsInScene;

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
    
    public void LoadData(SaveData data)
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        m_objectsInScene = FindObjectsOfType<ItemContainer>().ToList();
        
        foreach (ItemData item in data.m_playerInventory)
        {
          
            foreach (ItemContainer container in m_objectsInScene)
            {
                Debug.Log("cleaning house bby");
                if (container.m_itemData == item)
                {
                 
                        Debug.Log("cleaning house bby");
                        Destroy(container.gameObject);
                    
                    m_objectsInScene.Remove(container);
                    break;
                }
            }
        }
    }

    public void SaveData(SaveData data)
    {
        data.m_sceneIndex = _currentSceneIndex;
    }
}
