using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    #region Public Members

    public static InventoryManager m_instance;

    public EventHandler<EventArgs> m_onInventoryChanged;
    public List<ItemData> m_playerInventory;

    #endregion

    
    #region Unity API

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

    #endregion

    
    #region Main Methods
    
    public void AddItem(ItemData item)
    {
        foreach (Transform slot in _playerInventoryPanel.transform)
        {
            InventorySlot currentSlot = slot.GetComponent<InventorySlot>();
            
            if (currentSlot.m_item == null)
            {
                currentSlot.m_item = item;
                currentSlot.m_itemCount++;
                Debug.Log( currentSlot.m_item);
                break;
            }
            
            if (item.m_isItemStackable && currentSlot.m_item.m_isItemStackable && currentSlot.m_itemCount < currentSlot.m_item.m_stackSize)
            {
                currentSlot.m_itemCount++;
                break;
            }
            
        }
        
        m_playerInventory.Add(item);
        m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DropItem(ItemData item)
    {
        Vector3 spawnPoint = _player.transform.position + transform.forward + new Vector3(0, item.m_itemPrefab.transform.localScale.y, 0);
        Instantiate(item.m_itemPrefab, spawnPoint, _player.transform.rotation);
    }

    private void LoadInventory(List<ItemData> inventory)
    {
        foreach (var item in inventory.ToList())
        {
           AddItem(item);
        }
        
        InventoryUI.m_instance.RefreshInventory(this, EventArgs.Empty);
    }

    #endregion


    #region Private and Protected

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerInventoryPanel;

    #endregion

    public void LoadData(SaveData data)
    {
        transform.position = data.m_playerPostion;
        transform.rotation = data.m_playerRotation;
        
        LoadInventory(data.m_playerInventory);

    }

    public void SaveData(SaveData data)
    {
        if (m_playerInventory.Count != 0)
        {
            data.m_playerInventory = m_playerInventory;
        }
        data.m_playerPostion = transform.position;
        data.m_playerRotation = transform.rotation;
    }
}