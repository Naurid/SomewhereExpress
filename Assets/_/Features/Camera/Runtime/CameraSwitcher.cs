using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    #region Public Members

    public static CameraSwitcher m_instance;

    #endregion


    #region Unity API

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    
    #region Main Methods

    public void SwitchRig()
    {
        if (_outsideRig.m_Priority > _insideRig.m_Priority)
        {
            while (_outsideRig.m_Priority >= _insideRig.m_Priority)
            {
                _outsideRig.m_Priority--;
            }
        }
        else if (_insideRig.m_Priority > _outsideRig.m_Priority)
        {
            while (_insideRig.m_Priority >= _outsideRig.m_Priority)
            {
                _insideRig.m_Priority--;
            }
        }
    }

    #endregion
    

    #region Private and protected

    [SerializeField] private CinemachineFreeLook _outsideRig;
    [SerializeField] private CinemachineFreeLook _insideRig;

    #endregion
}