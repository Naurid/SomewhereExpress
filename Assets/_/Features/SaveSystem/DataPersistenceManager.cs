using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    #region Public Members
    public static DataPersistenceManager m_instance { get; private set; }

    public bool m_isLoaded;
    
    #endregion

    #region Unity API

    private void Awake() 
    {
        if (m_instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        m_instance = this;
        DontDestroyOnLoad(gameObject);

        if (_disableDataPersistence) 
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);

        _selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
        if (_overrideSelectedProfileId) 
        {
            _selectedProfileId = _testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + _testSelectedProfileId);
        }
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    #endregion

    #region Main Methods

    public void ChangeSelectedProfileId(string newProfileId) 
    {
        // update the profile to use for saving and loading
        _selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void NewGame() 
    {
        _gameData = new SaveData();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (_disableDataPersistence) 
        {
            return;
        }

        // load any saved data from a file using the data handler
        _gameData = _dataHandler.Load(_selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (_gameData == null && _initializeDataIfNull) 
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (_gameData == null) 
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (_disableDataPersistence) 
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (_gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        // timestamp the data so we know when it was last saved
        _gameData.m_lastUpdated = DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData, _selectedProfileId);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public SaveData GetData()
    {
        return _gameData;
    }
    public bool HasGameData() 
    {
        return _gameData != null;
    }

    public Dictionary<string, SaveData> GetAllProfilesGameData() 
    {
        return _dataHandler.LoadAllProfiles();
    }

    #endregion

    #region Private and protected

    [Header("Debugging")]
    [SerializeField] private bool _disableDataPersistence = false;
    [SerializeField] private bool _initializeDataIfNull = false;
    [SerializeField] private bool _overrideSelectedProfileId = false;
    [SerializeField] private string _testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;

    private SaveData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    private string _selectedProfileId = "";

    #endregion
    
    

   

    
}