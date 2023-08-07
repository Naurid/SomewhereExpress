using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private bool _isInteracting;
    private GameObject _interactible;

    private void Update()
    {
        if (_interactible == null) return;
    
        if (_isInteracting)
        {
            _interactible.GetComponent<TreasureInventory>().OpenTreasureInventoryPanel();
        }
        else
        {
            _interactible.GetComponent<TreasureInventory>().CloseTreasureInventoryPanel();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!_isInteracting)
            {
                if (Camera.main == null) return;
                var camPosition = Camera.main.transform.position;
                var headPosition = _head.position;
                var distance = Vector3.Distance(camPosition, headPosition) + _pickupDistance;
                Ray ray = new Ray(camPosition,  headPosition - camPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMask))
                {
                    if (hit.transform.CompareTag("interactible"))
                    {
                        _interactible = hit.transform.gameObject;
                        _isInteracting = true;
                        return;
                    }
                }
            }
           
            _isInteracting = false;
        }

        
    }
    
    [SerializeField] private Transform _head;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private LayerMask _layerMask;
}
