using UnityEngine;

public class TPPoint : MonoBehaviour
{
    #region Unity API

    private void OnEnable()
    {
        _manager = FindObjectOfType<DebugTP>();
        _manager.m_tpPoints.Add(this);
    }

    private void OnDisable()
    {
        _manager.m_tpPoints.Remove(this);
    }
    #endregion
  
    
    #region Private and protected

    private DebugTP _manager;

    #endregion
}