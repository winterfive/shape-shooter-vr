using System;
using UnityEngine;
using UnityEngine.UI;

public class ReticleManager : MonoBehaviour {    
    
    public Transform camera;
    public float defaultDistance = 2f;
    public GameObject reticle;
    public RaycastManager raycastManager;
    
    private Transform _reticleTransform;
    private Vector3 _originalScale;
    private Quaternion _originalRotation;
    private RaycastHit _currentHit;


    public Transform ReticleTransform { get { return reticle.transform; } }


    private void Awake()
    {
        _reticleTransform = reticle.GetComponent<Transform>();
        _originalScale = reticle.transform.localScale;
        _originalRotation = reticle.transform.localRotation;
    }


    // This overload of SetPosition is used when the the RaycastManager hasn't hit anything.
    // void -> void
    public void SetPosition()
    {
        // Set the position of the reticle to the default distance in front of the camera.
        _reticleTransform.position = camera.position + camera.forward * defaultDistance;

        // Set the scale based on the original and the distance from the camera.
        _reticleTransform.localScale = _originalScale * defaultDistance;

        // The rotation should just be the default.
        _reticleTransform.localRotation = _originalRotation;
    }


    // This overload of SetPosition is used when the RaycastManager has hit something.
    // RaycastHit -> void
    public void SetPosition(RaycastHit hit)
    {
        _reticleTransform.position = hit.point;
        _reticleTransform.localScale = _originalScale * hit.distance;
        
        // ... set it's rotation based on it's forward vector facing along the normal.
        _reticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);        
    }

    
    // Calls SetPosition
    // void -> void
    public void CheckNormalFound()
    {
        _currentHit = raycastManager.GetCurrentHit();
        SetPosition(_currentHit);        
    }
 

    private void OnEnable()
    {
        RaycastManager.OnNewNormalFound += CheckNormalFound;
    }

    private void OnDisable()
    {
        RaycastManager.OnNewNormalFound -= CheckNormalFound;
    }
}
