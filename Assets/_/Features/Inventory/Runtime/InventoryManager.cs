using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Public Members

    public static InventoryManager m_instance;

    public EventHandler<EventArgs> m_onInventoryChanged;
    public List<ItemData> _items = new();

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
        _items.Add(item);
        m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(ItemData item)
    {
        _items.Remove(item);
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

    #endregion
}