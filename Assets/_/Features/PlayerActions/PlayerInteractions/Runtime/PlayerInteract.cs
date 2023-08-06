using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private bool _canInteract;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactible"))
        {
            _canInteract = true;
            _interactible = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactible"))
        {
            _canInteract = false;
            _interactible = null;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started && _canInteract && !_isInteracting)
        {
            _isInteracting = true;
        }
        else if (context.started && _isInteracting)
        {
            _isInteracting = false;
        }

    }
}
