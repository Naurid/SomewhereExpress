using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void SaveGame(PlayerSave player)
    {
        InventoryManager inventory = InventoryManager.m_instance;
        SceneHandler sceneManager = SceneHandler.m_instance;
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.shrek";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(inventory, player, sceneManager);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.shrek";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("shrek not happy");
            return null;
        }
    }
}
