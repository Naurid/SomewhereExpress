using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<string> m_inventoryItems;
    public float[] m_playerPosition;
    public int m_sceneIndex;

    public SaveData(InventoryManager inventory, PlayerSave player, SceneHandler sceneManager)
    {
        m_inventoryItems = inventory.m_playerInventory;
        
        m_playerPosition = new float[3];
        
        var position = player.transform.position;
        
        m_playerPosition[0] = position.x;
        m_playerPosition[1] = position.y;
        m_playerPosition[2] = position.z;

        m_sceneIndex = sceneManager.m_currentSceneIndex;
    }

}
