using System.Collections.Generic;
using UnityEngine;

public class DebugTP : MonoBehaviour
{
    #region Public Members

    public List<TPPoint> m_tpPoints;

    #endregion

    
    #region Unity API

    private void OnGUI()
    {
        foreach (var point in m_tpPoints)
        {
            if (GUILayout.Button(point.name))
            {
                _player.transform.position = point.transform.position;
            }
        }
    }

    #endregion

    
    #region Private and protected

    [SerializeField] private GameObject _player;

    #endregion
}