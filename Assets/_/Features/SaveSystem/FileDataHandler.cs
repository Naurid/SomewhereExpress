using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    #region Main Methods

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption) 
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _useEncryption = useEncryption;
    }

    public SaveData Load(string profileId) 
    {
        if (profileId == null) 
        {
            return null;
        }
        
        string path = Path.Combine(_dataDirPath, profileId, _dataFileName);
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
                
                if (_useEncryption) 
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + path + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(SaveData data, string profileId) 
    {
        if (profileId == null) 
        {
            return;
        }
        
        string path = Path.Combine(_dataDirPath, profileId, _dataFileName);
        
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            string dataToStore = JsonUtility.ToJson(data, true);
            
            if (_useEncryption) 
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            
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
            Debug.LogError("Error occured when trying to save data to file: " + path + "\n" + e);
        }
    }

    public Dictionary<string, SaveData> LoadAllProfiles()
    {
        Dictionary<string, SaveData> profileDictionary = new();

        IEnumerable<DirectoryInfo> directoryInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo directory in directoryInfos)
        {
            string profileId = directory.Name;

            string path = Path.Combine(_dataDirPath, profileId, _dataFileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Skipping directory when loading all profiles beacause it does not contain data: {profileId}");
                continue;
            }

            SaveData profileData = Load(profileId);

            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError($"Something went horribly wrong. Check profileId {profileId}");
            }
        }
        return profileDictionary;
    }
    
    public string GetMostRecentlyUpdatedProfileId() 
    {
        string mostRecentProfileId = null;

        Dictionary<string, SaveData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, SaveData> pair in profilesGameData) 
        {
            string profileId = pair.Key;
            SaveData gameData = pair.Value;

            // skip this entry if the gamedata is null
            if (gameData == null) 
            {
                continue;
            }

            // if this is the first data we've come across that exists, it's the most recent so far
            if (mostRecentProfileId == null) 
            {
                mostRecentProfileId = profileId;
            }
            // otherwise, compare to see which date is the most recent
            else 
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].m_lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.m_lastUpdated);
                // the greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime) 
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }

    private string EncryptDecrypt(string data) 
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++) 
        {
            modifiedData += (char) (data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    #endregion
    

    #region Private and protected

    private string _dataDirPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "shrek";

    #endregion
}