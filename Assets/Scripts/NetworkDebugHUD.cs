using Unity.Netcode;
using UnityEngine;

public class NetworkDebugHUD : MonoBehaviour
{
    public void OnStartHost()
    {
        NetworkManager.Singleton.StartHost();
        DisableHUD();
    }

    public void OnStartClient()
    {
        NetworkManager.Singleton.StartClient();
        DisableHUD();
    }

    private void DisableHUD()
    {
        gameObject.SetActive(false);
    }
}
