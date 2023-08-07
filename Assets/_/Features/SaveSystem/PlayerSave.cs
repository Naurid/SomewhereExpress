using UnityEngine;

public class PlayerSave : MonoBehaviour
{
   private void Start()
   {
      _inventoryManager = InventoryManager.m_instance;
      _sceneHandler = SceneHandler.m_instance;
   }

   public void SavePlayer()
   {
      SaveSystem.SaveGame(this);
   }

   public void LoadPlayer()
   {
      SaveData data = SaveSystem.LoadGame();
      
      _inventoryManager.LoadInventory(data);

      Vector3 position;
      position.x = data.m_playerPosition[0];
      position.y = data.m_playerPosition[1];
      position.z = data.m_playerPosition[2];

      transform.position = position;
   }

   private InventoryManager _inventoryManager;
   private SceneHandler _sceneHandler;
}
