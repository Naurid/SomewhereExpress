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
            float _distance = Vector3.Distance(Camera.main.transform.position, _head.position) + _pickupDistance;
            Ray ray = new Ray(Camera.main.transform.position, _head.position - Camera.main.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit, _distance, _layerMask))
            {
                if (hit.transform.CompareTag("item"))
                {
                    _manager.AddItem(hit.transform.GetComponent<ItemContainer>().m_itemData);
                    Destroy(hit.transform.gameObject);
                }
            }
        }   
    }

    #endregion
    
    
    #region Private and Protected

    [SerializeField] private Transform _head;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private LayerMask _layerMask;

    private InventoryManager _manager;

    #endregion
}