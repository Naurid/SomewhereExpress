using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInventory : MonoBehaviour
{
    [SerializeField] private GameObject _treasureInventoryPanel;
    private InventoryUI _UImanager;
    private InventoryManager _manager;
    public List<ItemData> m_items = new();
    public List<ItemData> m_keyItems;

    private void Start()
    {
        _UImanager = InventoryUI.m_instance;
        _manager = InventoryManager.m_instance;
    }

    public void OpenTreasureInventoryPanel()
    {
        _treasureInventoryPanel.SetActive(true);
        _UImanager.ShowInventory();
    }

    public void CloseTreasureInventoryPanel()
    {
        _UImanager.HideInventory();
        _treasureInventoryPanel.SetActive(false);
    }

    public void CheckForItems()
    {
        m_items.Clear();
        
        foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
        {
            if (slot.GetComponent<InventorySlot>().m_item != null)
            {
                for (int i = 0; i < slot.GetComponent<InventorySlot>().m_itemCount; i++)
                {
                    m_items.Add(slot.GetComponent<InventorySlot>().m_item);
                }
            }
        }

        foreach (var item in m_items)
        {
            if (m_keyItems.Contains(item))
            {
                m_keyItems.Remove(item);
            }
        }

        if (m_keyItems.Count == 0)
        {
            foreach (var item in m_items)
            {
                _manager.RemoveItem(item);
                _manager.m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
            }
            
            foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
            {
                slot.GetComponent<InventorySlot>().m_item = null;
                slot.GetComponent<InventorySlot>().m_itemCount = 0;
                if (slot.childCount != 0)
                {
                    Destroy(slot.GetChild(0).gameObject);
                    _manager.m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            
            m_items.Clear();
            
            Debug.Log("you win");
        }
        else
        {
            foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
            {
                slot.GetComponent<InventorySlot>().m_item = null;
                slot.GetComponent<InventorySlot>().m_itemCount = 0;
                
                if (slot.childCount != 0)
                {
                    Destroy(slot.GetChild(0).gameObject);
                    _manager.m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
    
}
