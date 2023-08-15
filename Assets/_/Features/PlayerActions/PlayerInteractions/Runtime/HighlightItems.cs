using UnityEngine;

public class HighlightItems : MonoBehaviour
{
    #region Unity API

    private void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<MeshRenderer>();
            selectionRenderer.materials[0].DisableKeyword("_EMISSION");
            _selection = null;
        }
        
        CheckIfItem();
    }

    #endregion

    
    #region Main Methods
    
    private void CheckIfItem()
    {
        if (Physics.Raycast(_head.position, _head.position - Camera.main.transform.position, out RaycastHit hit, _highlightDistance, _layerMask))
        {
            var selected = hit.transform;
            if (selected.CompareTag("item"))
            {
                _selection = selected;
                var selectionRenderer = selected.GetComponent<MeshRenderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.materials[0].EnableKeyword("_EMISSION");
                    selectionRenderer.materials[0].SetColor("_EmissionColor", _color);
                }
            }
        }
    }

    #endregion

    
    #region Private and protected

    [SerializeField] private Transform _head;
    [SerializeField] private float _highlightDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Color _color;
    
    private Color _baseColor;
    private Transform _selection;

    #endregion
}