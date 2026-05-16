using Unity.Netcode;
using UnityEngine;

[DisallowMultipleComponent]
public class HologramBody : NetworkBehaviour
{
    [SerializeField] private Transform _referenceBody;
    
    private LineRenderer _lineRenderer;
    private TrailRenderer _trailRenderer;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!IsHost || _referenceBody == null)
            return;
        
        transform.localPosition = _referenceBody.localPosition;
    }
    
    public void SetReferenceBody(Transform referenceBody)
    {
        _referenceBody = referenceBody;
    }
}
