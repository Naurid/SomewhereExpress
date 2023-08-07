using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreasureInventory : MonoBehaviour
{
    [SerializeField] private GameObject _treasureInventoryPanel;
    private InventoryUI _UImanager;
    private InventoryManager _manager;
    public List<ItemData> m_keyItems;
    private int _score;

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
        foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
        {
          
            InventorySlot currentSlot = slot.GetComponent<InventorySlot>();

            if (m_keyItems.Contains(currentSlot.m_item))
            {
                _score++;
            }
        }

        if (m_keyItems.Count == _score)
        {
            foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
            {
                var currentSlot = slot.GetComponent<InventorySlot>();

                if (!m_keyItems.Contains(currentSlot.m_item)) continue;
                currentSlot.m_itemCount--;

                if (currentSlot.m_itemCount > 0) continue;
                currentSlot.m_item = null;
                currentSlot.m_itemCount = 0;
                Destroy(slot.GetChild(0).gameObject);
            }
            RefreshVisuals();
        }
    }

    private void RefreshVisuals()
    {
        foreach (Transform slot in _treasureInventoryPanel.transform.GetChild(0))
        {
            InventorySlot currentSlot = slot.GetComponent<InventorySlot>();

            if (currentSlot.m_item == null) continue;

            slot.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = currentSlot.m_itemCount.ToString();
        }
    }

}
