using Unity.Netcode;
using UnityEngine;

public class NetworkGameObject : NetworkBehaviour
{
    [SerializeField] private bool _disableIfOwner = false;
    
    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(_disableIfOwner ? !IsOwner : IsOwner);
    }
}
