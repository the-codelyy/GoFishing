using Unity.Netcode;
using UnityEngine;

public class PlayerComponent : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        enabled = IsOwner;
    }
}
