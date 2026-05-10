using System;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractableObject : NetworkBehaviour, IInteractable
{
    public event Action<InteractableObject> OnInteract;
    
    public virtual bool Interact()
    {
        OnInteract?.Invoke(this);
        return true;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void TakeOwnershipServerRpc(ServerRpcParams rpcParams = default)
    {
        NetworkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
    }
}
