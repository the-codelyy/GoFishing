using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCamera : PlayerComponent
{
    public Camera Camera
    {
        get;
        private set;
    }

    public CinemachineCamera VirtualCamera
    {
        get;
        private set;
    }
    
    [Header("Head Bob")]
    [SerializeField] private float _headBobFrequency = 0.05f;
    [SerializeField] private float _headBobAmplitude = 0.5f;
    [SerializeField] private float _headBobSpeed = 3.0f;
    [SerializeField] private float _headBobTransitionSpeed = 1.0f;
    
    private Vector3 _initialCameraPos;
    private float _headBobTimer = 0.0f;

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(IsOwner);
    }

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        VirtualCamera = GetComponentInChildren<CinemachineCamera>();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _initialCameraPos = VirtualCamera.transform.localPosition;
    }

    public Vector3 GetForward()
    {
        return Vector3.ProjectOnPlane(Camera.transform.forward, Vector3.up).normalized;
    }

    public Vector3 GetRight()
    {
        return Vector3.ProjectOnPlane(Camera.transform.right, Vector3.up).normalized;
    }

    public void HeadBob(Vector3 direction, float speed)
    {
        if (direction == Vector3.zero)
        {
            _headBobTimer = 0.0f;
            VirtualCamera.transform.localPosition = Vector3.Lerp(VirtualCamera.transform.localPosition, _initialCameraPos, _headBobTransitionSpeed * Time.deltaTime);
            return;
        }
        
        Vector3 cameraPos = _initialCameraPos;
        if (direction.magnitude > 0.1f)
        {
            _headBobTimer += (_headBobSpeed * speed) * Time.deltaTime;

            float offsetY = Mathf.Sin(_headBobTimer) * _headBobFrequency;
            float offsetZ = Mathf.Cos(_headBobTimer) * _headBobFrequency * _headBobAmplitude;
            
            cameraPos.y += offsetY;
            cameraPos.z += offsetZ;
        }
        
        VirtualCamera.transform.localPosition = Vector3.Lerp(VirtualCamera.transform.localPosition, cameraPos, _headBobTransitionSpeed * Time.deltaTime);
    }
}
