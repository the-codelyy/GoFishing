using Unity.Netcode;
using UnityEngine;

public static class GameObjectExtensions
{
    public static uint GetPrefabHash(this GameObject gameObject)
    {
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        if (networkObject == null) 
            return 0;
        
        return networkObject.PrefabIdHash;
    }
}
