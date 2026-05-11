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
    
    [Rpc(SendTo.Server)]
    public void TakeOwnershipRpc(ulong clientID)
    {
        NetworkObject.ChangeOwnership(clientID);
    }
}
