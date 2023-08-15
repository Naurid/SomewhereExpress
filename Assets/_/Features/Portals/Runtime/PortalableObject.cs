using UnityEngine;

public class PortalableObject : MonoBehaviour
{
    #region Public Members

    public delegate void HasTeleportedHandler(Portal sender, Portal destination, Vector3 newPosition, Quaternion newRotation);
    public event HasTeleportedHandler HasTeleported;

    #endregion


    #region Main Methods

    public void OnHasTeleported(Portal sender, Portal destination, Vector3 newPosition, Quaternion newRotation)
    {
        HasTeleported?.Invoke(sender, destination, newPosition, newRotation);
    }

    #endregion
}