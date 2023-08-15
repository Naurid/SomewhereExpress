using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    #region Unity API
    
    void Start()
    {
        _switcher = CameraSwitcher.m_instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _switcher.SwitchRig();
        }
    }
    
    #endregion
   

    #region Private and protected

    private CameraSwitcher _switcher;

    #endregion
}