using Unity.Netcode;
using UnityEngine;

public class NetworkController : Singleton<NetworkController>
{
    [SerializeField] private bool _autoStartHost;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_autoStartHost)
        {
            NetworkManager.Singleton.StartHost();
        }
    }
    
    public override void OnNetworkSpawn()
    {
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
    }
    
    [Rpc(SendTo.Server)]
    public void SpawnEntityRpc(uint hash, Vector3 position, Quaternion rotation = default, Vector3 scale = default, RpcParams rpcParams = default)
    {
        if (NetworkManager.NetworkConfig.Prefabs.NetworkPrefabOverrideLinks.TryGetValue(hash, out NetworkPrefab prefab))
        { 
            NetworkManager.SpawnManager.InstantiateAndSpawn(prefab.Prefab.GetComponent<NetworkObject>(), destroyWithScene: true, position: position, rotation: rotation);
        }
        else
        {
            Debug.LogError($"Failed to obtain network object from hash: {hash}", this);
        }
    }

    public void DespawnEntity(GameObject entity)
    {
        if (entity.TryGetComponent(out NetworkObject networkObject))
        {
            networkObject.Despawn();
        }
    }

    private void OnClientConnected(ulong clientID)
    {
        if (IsOwner)
            return;
        
        Debug.Log($"Client '{clientID}' Has Connected");
    }

    private void OnClientDisconnected(ulong clientID)
    {
        if (IsOwner)
            return;
        
        Debug.Log($"Client '{clientID}' Has Disconnected");
    }
}
