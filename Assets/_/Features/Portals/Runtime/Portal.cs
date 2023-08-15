using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
  #region Public Members

  public Portal m_targetPortal;
  
  public Transform m_normalVisible;
  public Transform m_normalInvisible;
  
  public Camera m_portalCamera;
  public Renderer m_viewthroughRenderer;

  #endregion

  #region Unity API

  private void OnTriggerEnter(Collider other)
  {
    var portalableObject = other.GetComponent<PortalableObject>();
    if (portalableObject)
    {
      _objectsInPortal.Add(portalableObject);
    }
  }
  
  private void OnTriggerExit(Collider other)
  {
    var portalableObject = other.GetComponent<PortalableObject>();
    if (portalableObject)
    {
      _objectsInPortal.Remove(portalableObject);
    }    
  }
  
  private void Start()
  {
    // Create render texture

    _viewthroughRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
    _viewthroughRenderTexture.Create();
    
    // Assign render texture to portal material (cloned)

    _viewthroughMaterial = m_viewthroughRenderer.material;
    _viewthroughMaterial.mainTexture = _viewthroughRenderTexture;
    // Assign render texture to portal camera

    m_portalCamera.targetTexture = _viewthroughRenderTexture;
    
    // Cache the main camera

    _mainCamera = Camera.main;
    
    // Generate bounding plane

    var plane = new Plane(m_normalVisible.forward, transform.position);
    _vectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
    
    StartCoroutine(WaitForFixedUpdateLoop());
  }

  private void LateUpdate()
  {
    // Calculate portal camera position and rotation
    var virtualPosition = TransformPositionBetweenPortals(this, m_targetPortal, _mainCamera.transform.position);
    var virtualRotation = TransformRotationBetweenPortals(this, m_targetPortal, _mainCamera.transform.rotation);

    // Position camera

    m_portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);
    
    // Calculate projection matrix
    
    var clipThroughSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(m_portalCamera.worldToCameraMatrix)) * m_targetPortal._vectorPlane;
    
    // Set portal camera projection matrix to clip walls between target portal and portal camera
    // Inherits main camera near/far clip plane and FOV settings
    
    var obliqueProjectionMatrix = _mainCamera.CalculateObliqueMatrix(clipThroughSpace);
    m_portalCamera.projectionMatrix = obliqueProjectionMatrix;
    
    // Render camera
    
    m_portalCamera.Render();
  }

  private void OnDestroy()
  {
    // Release render texture from GPU
    
    _viewthroughRenderTexture.Release();

    // Destroy cloned material and render texture
    
    Destroy(_viewthroughMaterial);
    Destroy(_viewthroughRenderTexture);
  }
  
  #endregion

  #region Main Methods

  private IEnumerator WaitForFixedUpdateLoop()
  {
    var waitForFixedUpdate = new WaitForFixedUpdate();
    while (true)
    {
      yield return waitForFixedUpdate;
      try
      {
        CheckForPortalCrossing();
      }
      catch (Exception e)
      {
        // Catch exceptions so our loop doesn't die whenever there is an error
        Debug.LogException(e);
      }
    }
  }

  private void CheckForPortalCrossing()
  {
    // Clear removal queue

    _objectsInPortalToRemove.Clear();

    // Check every touching object

    foreach (var portalableObject in _objectsInPortal)
    {
      // If portalable object has been destroyed, remove it immediately

      if (portalableObject == null)
      {
        _objectsInPortalToRemove.Add(portalableObject);
        continue;
      }
      // Check if portalable object is behind the portal using Vector3.Dot (dot product)
      // If so, they have crossed through the portal.

      var pivot = portalableObject.transform;
      var directionToPivotFromTransform = pivot.position - transform.position;
      directionToPivotFromTransform.Normalize();
      var pivotToNormalDotProduct = Vector3.Dot(directionToPivotFromTransform, m_normalVisible.forward);
      if (pivotToNormalDotProduct > 0) continue;
      
      // Warp object

      var newPosition = TransformPositionBetweenPortals(this, m_targetPortal, portalableObject.transform.position);
      var newRotation = TransformRotationBetweenPortals(this, m_targetPortal, portalableObject.transform.rotation);
      
      portalableObject.transform.SetPositionAndRotation(newPosition, newRotation);
      portalableObject.OnHasTeleported(this, m_targetPortal, newPosition, newRotation);
      // Object is no longer touching this side of the portal

      _objectsInPortalToRemove.Add(portalableObject);
    }
    // Remove all objects queued up for removal

    foreach (var portalableObject in _objectsInPortalToRemove)
    {
      _objectsInPortal.Remove(portalableObject);
    }
    
  }

  #endregion

  #region Utils

  public static Vector3 TransformPositionBetweenPortals(Portal sender, Portal target, Vector3 position)
  {
    return
      target.m_normalInvisible.TransformPoint(
        sender.m_normalVisible.InverseTransformPoint(position));
  }
  
  public static Quaternion TransformRotationBetweenPortals(Portal sender, Portal target, Quaternion rotation)
  {
    return
      target.m_normalInvisible.rotation *
      Quaternion.Inverse(sender.m_normalVisible.rotation) *
      rotation;
  }

  #endregion

  #region Private and protected

  private RenderTexture _viewthroughRenderTexture;
  private Material _viewthroughMaterial;
  
  private Camera _mainCamera;

  private Vector4 _vectorPlane;
  
  private HashSet<PortalableObject> _objectsInPortal = new ();
  
  private HashSet<PortalableObject> _objectsInPortalToRemove = new ();

  #endregion
}
