using Unity.Netcode;
using UnityEngine;

public class SystemMap : NetworkBehaviour
{
    [SerializeField] private HologramBody _planetHologram;
    [SerializeField] private StarSystem _starSystem;
    
    public override void OnNetworkSpawn()
    {
        _starSystem.OnStarSystemGenerated += OnStarSystemGenerated;
    }

    public override void OnNetworkDespawn()
    {
        _starSystem.OnStarSystemGenerated -= OnStarSystemGenerated;
    }

    private void OnStarSystemGenerated()
    {
        _planetHologram.SetReferenceBody(_starSystem.Bodies[0]);
    }
}
