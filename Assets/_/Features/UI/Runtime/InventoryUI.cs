using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
   #region Public Members

   public static InventoryUI m_instance;

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

   private void OnEnable()
   {
   }

   private void Start()
   {
      _inventoryManager = InventoryManager.m_instance;
      _inventoryManager.m_onInventoryChanged += RefreshInventory;
   }

   #endregion


   #region Main Methods

   public void OpenInventory(InputAction.CallbackContext context)
   {

      if (context.started && !_inventoryPanel.activeInHierarchy)
      {
         ShowInventory();
      }
      else if(context.started && _inventoryPanel.activeInHierarchy)
      {
         HideInventory();
      }
   }

   public void HideInventory()
   {
      Time.timeScale = 1f;
      _inventoryPanel.SetActive(false);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Confined;
   }

   public void ShowInventory()
   {
      Time.timeScale = 0f;
      _inventoryPanel.SetActive(true);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
   }

   public void RefreshInventory(object sender, EventArgs e)
   {
      SetInventoryVisuals();
   }

   private void CleanInventory()
   {
      foreach (Transform slot in _inventoryPanel.transform)
      {
         slot.GetComponent<InventorySlot>().m_item = null;
         slot.GetComponent<InventorySlot>().m_itemCount = 0;
         
         foreach (Transform child in slot)
         {
            Destroy(child.gameObject);
         }
      }
   }

   private void SetInventoryVisuals()
   {
      foreach (Transform slot in _inventoryPanel.transform)
      {
         InventorySlot currentSlot = slot.GetComponent<InventorySlot>();
         
         if (currentSlot.m_item != null)
         {
            if (slot.childCount != 0)
            {
               Destroy(slot.GetChild(0).gameObject);
            }
            
            GameObject itemDisplay = Instantiate(_itemDisplayPrefab, slot);

            itemDisplay.transform.GetChild(0).GetComponent<Image>().sprite = currentSlot.m_item.m_itemSprite;
            itemDisplay.transform.GetChild(1).GetComponent<TMP_Text>().text = currentSlot.m_item.m_isItemStackable ? currentSlot.m_itemCount.ToString() : "";
         }
         
         
      }
   }
   #endregion
   
   
   #region Private and Protected

   [SerializeField] private GameObject _inventoryPanel;
   [SerializeField] private GameObject _itemDisplayPrefab;
   
   private InventoryManager _inventoryManager;

   #endregion
}