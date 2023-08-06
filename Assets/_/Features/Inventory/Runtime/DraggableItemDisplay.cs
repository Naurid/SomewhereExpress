using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    #region Public Members

    public Transform m_baseParent;

    #endregion
    

    #region Unity API

    private void Start()
    {
        _manager = InventoryManager.m_instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_baseParent = transform.parent;
        transform.SetParent(transform.root);
        _itemDisplayImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(m_baseParent);
        _itemDisplayImage.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _contextMenu.SetActive(true);
            _contextMenu.transform.position = Input.mousePosition;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _contextMenu.SetActive(false);
        }
    }
    #endregion

    
    #region Main Methods

    public void DeleteItem()
    {
        InventorySlot parentSlot = transform.parent.GetComponent<InventorySlot>();
        _manager.RemoveItem(parentSlot.m_item);
        parentSlot.m_item = null;
        parentSlot.m_itemCount = 0;
        _manager.m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DropItem()
    {
        InventorySlot parentSlot = transform.parent.GetComponent<InventorySlot>();
        _manager.DropItem(parentSlot.m_item);
        _manager.RemoveItem(parentSlot.m_item);
        parentSlot.m_item = null;
        parentSlot.m_itemCount = 0;
        _manager.m_onInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem()
    {
        Debug.Log("item used");
    }

    #endregion
    
    
    #region Private and Protected

    [SerializeField] private Image _itemDisplayImage;
    [SerializeField] private GameObject _contextMenu;
    
    private InventoryManager _manager;
    
    #endregion
}