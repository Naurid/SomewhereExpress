using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    #region Public Members

    public ItemData m_item;
    public int m_itemCount;

    #endregion

    #region Unity API

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag;
        var draggableItem = droppedItem.GetComponent<DraggableItemDisplay>();

        if (m_item == null)
        {
            m_item = draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item;
            m_itemCount = draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount;

            draggableItem.m_baseParent.GetComponent<InventorySlot>().m_item = null;
            draggableItem.m_baseParent.GetComponent<InventorySlot>().m_itemCount = 0;

            draggableItem.m_baseParent = transform;
        }
    }

    #endregion
}