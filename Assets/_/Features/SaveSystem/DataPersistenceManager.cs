using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
   [Header("File Storage Config")] 
   [SerializeField] private string _fileName;

   private FileDataHandler _dataHandler;
   public static DataPersistenceManager m_instance { get; private set; }

   private SaveData data;
   private List<IDataPersistence> _dataPersistentObjects;
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

   private void Start()
   {
      this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
      this._dataPersistentObjects = FindAllDataPersistenceObjects();
      //LoadGame();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         LoadGame();
      }
   }

   private List<IDataPersistence> FindAllDataPersistenceObjects()
   {
      IEnumerable<IDataPersistence> dataPersistentObjects =
         FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
      return new List<IDataPersistence>(dataPersistentObjects);
   }

   public void NewGame()
   {
      this.data = new SaveData();
   }

   public void LoadGame()
   {
      this.data = _dataHandler.Load();
      
      if (this.data == null)
      {
         Debug.Log(" No data was found. Initializing new Save...");
         NewGame();
      }

      foreach (IDataPersistence variable in _dataPersistentObjects)
      {
         variable.LoadData(data);
      }
   }

   public void SaveGame()
   {
      foreach (IDataPersistence variable in _dataPersistentObjects)
      {
         variable.SaveData(data);
      }
      
      _dataHandler.Save(data);
   }

   private void OnApplicationQuit()
   {
      SaveGame();
   }
}
