using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    
    public SaveData Load()
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        SaveData loadedData = null;

        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("nope" + e);
            }
        }

        return loadedData;
    }

    public void Save(SaveData data)
    {
        string path = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(dataDirPath);

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"error : {e}");
        }
    }
}
