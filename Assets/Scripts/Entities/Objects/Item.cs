using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour, IInteractable
{
    [field: SerializeField]
    public string Name
    {
        get;
        private set;
    }

    public NetworkVariable<bool> IsHeld
    {
        get;
        private set;
    } = new NetworkVariable<bool>();
    
    public virtual bool Interact()
    {
        return true;
    }
    
    [Rpc(SendTo.Server)]
    public void RequestPickupRpc(RpcParams rpcParams = default)
    {
        if (!IsHeld.Value)
        {
            NetworkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
            IsHeld.Value = true;
        }
    }

    [Rpc(SendTo.Server)]
    public void RequestDropRpc(RpcParams rpcParams = default)
    {
        if (NetworkObject.OwnerClientId == rpcParams.Receive.SenderClientId && IsHeld.Value)
        {
            NetworkObject.RemoveOwnership();
            IsHeld.Value = false;
        }
    }
}
