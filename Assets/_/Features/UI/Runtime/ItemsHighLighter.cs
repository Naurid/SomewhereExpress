using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsHighLighter : MonoBehaviour
{
    public Color m_color;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item"))
        {
            StartHighlighting(other);
        }
        
    }

    private void StartHighlighting(Collider other)
    {
        other.transform.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
        other.transform.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", m_color);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item"))
        {
            StopHighlighting(other);
        }
        
    }

    private void StopHighlighting(Collider other)
    {
        other.transform.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
    }
}
