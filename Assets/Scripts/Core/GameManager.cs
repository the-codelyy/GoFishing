using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientID)
    {
        GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }
}
