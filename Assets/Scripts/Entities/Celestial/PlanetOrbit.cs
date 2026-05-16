using System;
using Unity.Netcode;
using UnityEngine;

public class PlanetOrbit : NetworkBehaviour
{
    private float _orbitSpeed;
    
    private NetworkVariable<float> _currentOrbit = new NetworkVariable<float>();
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (!IsHost)
            return;
        
        _currentOrbit.Value %= 360.0f;
        _currentOrbit.Value += _orbitSpeed * Time.deltaTime;

        transform.localPosition = CalculateOrbit();
    }

    public void Setup(float orbitSpeed, float initialOrbit)
    {
        _orbitSpeed = orbitSpeed;
        _currentOrbit.Value = initialOrbit;
    }

    private Vector3 CalculateOrbit()
    {
        float orbitInRadians = _currentOrbit.Value * Mathf.Deg2Rad;
        float x = Mathf.Cos(orbitInRadians) * _initialPosition.x - Mathf.Sin(orbitInRadians) * _initialPosition.z;
        float z = Mathf.Sin(orbitInRadians) * _initialPosition.x - Mathf.Cos(orbitInRadians) * _initialPosition.z;
        
        return new Vector3(x, _initialPosition.y, z);
    }
}
