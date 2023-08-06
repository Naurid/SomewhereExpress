using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,IDropHandler
{
   #region Public Members

   public ItemData m_item;
   public int m_itemCount = 0;

   #endregion

   #region Unity API

   private void Start()
   {
      _manager = InventoryManager.m_instance;
   }

   public void OnDrop(PointerEventData eventData)
   {
      GameObject droppedItem = eventData.pointerDrag;
      DraggableItemDisplay draggableItem = droppedItem.GetComponent<DraggableItemDisplay>();

      if (m_item == null)
      {
         m_item = draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item;
         m_itemCount = draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount;
       
         draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item = null;
         draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount = 0;
       
         draggableItem.m_baseParent = transform;
         
      }
    
      // if (m_item != null && m_item == draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item && m_item.m_isItemStackable && m_itemCount < m_item.m_stackSize)
      // {
      //    m_itemCount += draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount;
      //     
      //    draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item = null;
      //    draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount = 0;
      //     
      //    draggableItem.m_baseParent = transform;
      // }
   }
   
   #endregion


   #region Private and Protected

   private InventoryManager _manager;

   #endregion
}