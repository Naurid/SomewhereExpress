using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Public Members

    public static InventoryManager m_instance;

    public EventHandler<EventArgs> m_onInventoryChanged;
    public List<string> m_playerInventory;

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

    public void LoadInventory(SaveData data)
    {
        m_playerInventory = data.m_inventoryItems;
    }
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
            
            if (currentSlot.m_item.m_isItemStackable && currentSlot.m_itemCount < currentSlot.m_item.m_stackSize)
            {
                currentSlot.m_itemCount++;
                break;
            }
            
        }
        
        m_playerInventory.Add(item.name);
        m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DropItem(ItemData item)
    {
        Vector3 spawnPoint = _player.transform.position + transform.forward + new Vector3(0, item.m_itemPrefab.transform.localScale.y, 0);
        Instantiate(item.m_itemPrefab, spawnPoint, _player.transform.rotation);
    }

    #endregion


    #region Private and Protected

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerInventoryPanel;

    #endregion
}