using Unity.Netcode;
using UnityEngine;

public class NetworkController : Singleton<NetworkController>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

    [ServerRpc]
    public void SpawnEntityServerRpc(NetworkObjectReference entityRef, Vector3 position, ServerRpcParams serverParams = default)
    {
        if (entityRef.TryGet(out NetworkObject networkObj))
        {
            GameObject entity = Instantiate(networkObj.gameObject, position, Quaternion.identity);
            entity.GetComponent<NetworkObject>().Spawn(true);
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
