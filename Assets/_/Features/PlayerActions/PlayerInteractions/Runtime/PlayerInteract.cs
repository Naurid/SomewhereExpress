using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{

    #region Unity API

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

    #endregion

    #region Main Methods

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!_isInteracting)
            {
                if (Camera.main == null) return;
                
                var camPosition = Camera.main.transform.position;
                var headPosition = _head.position;
                
                var distance = Vector3.Distance(camPosition, headPosition) + _interactDistance;
                Ray ray = new Ray(camPosition,  headPosition - camPosition);

                if (!Physics.Raycast(ray, out RaycastHit hit, distance, _layerMask)) return;
                if (!hit.transform.CompareTag("interactible")) return;
               
                _interactible = hit.transform.gameObject;
                _isInteracting = true;
                return;
                
            }
            _isInteracting = false;
        }
    }

    #endregion
    

    #region Private and protected

    [SerializeField] private Transform _head;
    [SerializeField] private float _interactDistance;
    [SerializeField] private LayerMask _layerMask;
    
    private bool _isInteracting;
    private GameObject _interactible;

    #endregion
    
}