using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerModel : PlayerComponent
{
    [SerializeField] private Transform _head;
    [SerializeField] private float _maxHeadYaw = 30.0f;
    [SerializeField] private float _bodyRotateSpeed = 1.0f;
    
    private PlayerCamera _camera;

    private void Awake()
    {
        _camera = transform.parent.GetComponentInChildren<PlayerCamera>();
    }

    private void Start()
    {
        int layer = LayerMask.NameToLayer("PlayerModel");
        SetLayerRecursively(gameObject, layer);
    }

    private void LateUpdate()
    {
        const float kCenterYaw = 180.0f;
        
        _head.rotation = Quaternion.LookRotation(-_camera.VirtualCamera.transform.forward);
        
        float yawOffset = Mathf.DeltaAngle(transform.eulerAngles.y, _head.eulerAngles.y);
        if (Mathf.Abs(yawOffset) < kCenterYaw - _maxHeadYaw)
        {
            float excessYaw = yawOffset - Mathf.Sign(yawOffset) * _maxHeadYaw;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - excessYaw, transform.eulerAngles.z), _bodyRotateSpeed * Time.deltaTime);
        }
    }

    public void SetModelDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
	
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
