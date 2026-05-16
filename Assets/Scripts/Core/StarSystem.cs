using System;
using System.Collections.Generic;
using ParallelCascades.ProceduralPlanetGenerationLite.Runtime;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSystem : NetworkBehaviour
{
    public Transform Star
    {
        get;
        private set;
    }

    public List<Transform> Bodies
    {
        get;
        private set;
    } = new List<Transform>();

    public event Action OnStarSystemGenerated;
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _planetPrefab;

    public void SpawnSystem(StarSystemData data)
    {
        NetworkObject star = null;
        if (NetworkManager.NetworkConfig.Prefabs.NetworkPrefabOverrideLinks.TryGetValue(_starPrefab.GetPrefabHash(), out NetworkPrefab prefab))
        { 
            star = NetworkManager.SpawnManager.InstantiateAndSpawn(prefab.Prefab.GetComponent<NetworkObject>(), destroyWithScene: true, position: transform.position, rotation: Quaternion.identity);
            Star = star.transform;
        }
        
        foreach (SystemBodyData body in data.Satellites)
        {
            SpawnBody(body, star);
        }
        
        OnStarSystemGenerated?.Invoke();
    }

    private void SpawnBody(SystemBodyData body, NetworkObject star)
    {
        GameObject prefab = GetBodyPrefab(body.Body.Type);
        if (NetworkManager.NetworkConfig.Prefabs.NetworkPrefabOverrideLinks.TryGetValue(prefab.GetPrefabHash(), out NetworkPrefab networkPrefab))
        { 
            NetworkObject networkBody = NetworkManager.SpawnManager.InstantiateAndSpawn(networkPrefab.Prefab.GetComponent<NetworkObject>(), destroyWithScene: true, position: transform.position, rotation: Quaternion.identity);
            Bodies.Add(networkBody.transform);
            if (networkBody.TrySetParent(star, false))
            {
                networkBody.transform.localPosition = new Vector3(body.DistanceFromPrimary * body.Size, 0.0f, 0.0f);
                networkBody.transform.localScale = new Vector3(body.Size, body.Size, body.Size);
            }

            if (networkBody.TryGetComponent(out PlanetOrbit orbit))
            {
                orbit.Setup(body.OrbitSpeed, Random.Range(0, 360));
            }
        }
    }

    private GameObject GetBodyPrefab(CelestialBodyType type)
    {
        switch (type)
        {
            case CelestialBodyType.Star: return _starPrefab;
            case CelestialBodyType.Planet: return  _planetPrefab;
        }

        return null;
    }
}
