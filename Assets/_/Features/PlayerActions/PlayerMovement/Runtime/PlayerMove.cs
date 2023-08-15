using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    #region Unity API

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        portalableObject = GetComponent<PortalableObject>();
        
        if (Camera.main != null) _camTransform = Camera.main.transform;
        
        if(portalableObject != null) portalableObject.HasTeleported += PortalableObjectOnHasTeleported;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    
    
  
    private void FixedUpdate()
    {
        Vector3 camDirection = Vector3.Scale(_camTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 direction = camDirection * _moveY + _camTransform.right * _moveX;
        
        if (direction.magnitude > 1f) direction.Normalize();

        float speed = _moveSpeed * (_isSprinting? _sprintValue : 1f);
        
        _rigidBody.velocity = direction * speed + new Vector3(0,_rigidBody.velocity.y,0);
        
        Vector3 lookDir = Vector3.Scale(_rigidBody.velocity, new Vector3(1, 0, 1));
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation,
                    _rotationSpeed); //Vector3.Lerp(transform.forward, lookDir, _rotationSpeed);
        }

        _playerAnimator.SetFloat(Speed, _rigidBody.velocity.magnitude);
        _playerAnimator.SetFloat(SpeedX, _moveX * speed);
        _playerAnimator.SetFloat(SpeedY, _moveY * speed);
    }

    private void OnDestroy()
    {
        if (portalableObject != null)portalableObject.HasTeleported -= PortalableObjectOnHasTeleported;
    }
    
    #endregion

    #region Main Methods

    private void PortalableObjectOnHasTeleported(Portal sender, Portal destination, Vector3 newposition, Quaternion newrotation)
    {
        Physics.SyncTransforms();
    }
    
    public void GetMovementData(InputAction.CallbackContext context)
    {
        _moveX = context.ReadValue<Vector2>().x;
        _moveY = context.ReadValue<Vector2>().y;

        if (context.ReadValue<Vector2>() == Vector2.zero)
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }

    public void GetSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting = true;
        }
        else if (context.canceled)
        {
            _isSprinting = false;
        }
    }

    #endregion

    #region Private and Protected

    [Header("Main Parameters")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _sprintValue;

    private Animator _playerAnimator;
    private Rigidbody _rigidBody;
    private Transform _camTransform;
    private PortalableObject portalableObject;

    private float _moveX;
    private float _moveY;

    private bool _isSprinting;
    
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int SpeedX = Animator.StringToHash("speedX");
    private static readonly int SpeedY = Animator.StringToHash("speedY");

    #endregion
    
}