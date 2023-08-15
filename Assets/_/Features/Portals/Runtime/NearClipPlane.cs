using UnityEngine;

public class NearClipPlane : MonoBehaviour
{
    #region Public Members

    public float nearClipPlane = 0.00001f;

    #endregion

    #region Unity API

    private void Start()
    {
        GetComponent<Camera>().nearClipPlane = nearClipPlane;
    }

    #endregion
    
}
