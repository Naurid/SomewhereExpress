using System.Collections.Generic;
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

        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadData(SaveData data)
    {
        if (SceneManager.GetActiveScene().buildIndex != data.m_sceneIndex)
        {
            SceneManager.LoadScene(data.m_sceneIndex);
        }

        foreach (ItemData item in data.m_playerInventory)
        {
            foreach (ItemContainer container in m_objectsInScene)
            {
                if (container.m_itemData == item)
                {
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
