using UnityEngine;

public class HighlightItems : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _layerMask;
    
    public Color m_color;
    private Color _baseColor;
    private Transform _selection;
    
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

    private void CheckIfItem()
    {
        if (Physics.Raycast(Camera.main.transform.position, _head.position - Camera.main.transform.position,
                out RaycastHit hit, Vector3.Distance(Camera.main.transform.position, _head.position) + _distance,
                _layerMask))
        {
            var selected = hit.transform;
            if (selected.CompareTag("item"))
            {
                _selection = selected;
                var selectionRenderer = selected.GetComponent<MeshRenderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.materials[0].EnableKeyword("_EMISSION");
                    selectionRenderer.materials[0].SetColor("_EmissionColor", m_color);
                }
            }
        }
    }
}
