using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour
{
    #region Unity API

    private void Start()
    {
        _manager = InventoryManager.m_instance;
    }

    #endregion


    #region Main Methods

    public void PickUpItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Collider[] items = Physics.OverlapBox(transform.position + transform.forward, new Vector3(0.5f, 1.8f, 0.5f), transform.rotation, _layerMask);
            if (items.Length != 0)
            {
                foreach (var item in items)
                {
                    ItemData itemData = item.transform.GetComponent<ItemContainer>().m_itemData;
                    _manager.AddItem(itemData);
                    Destroy(item.gameObject);
                }
            }
        }   
    }

    #endregion
    
    
    #region Private and Protected

    [SerializeField] private LayerMask _layerMask;

    private InventoryManager _manager;

    #endregion
}
