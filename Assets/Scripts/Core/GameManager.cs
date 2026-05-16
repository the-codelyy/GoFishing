using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private StarSystemData _systemData;
    [SerializeField] private StarSystem _generator;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        _generator.SpawnSystem(_systemData);
    }

    private void OnClientConnected(ulong clientID)
    {
        GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }
}
