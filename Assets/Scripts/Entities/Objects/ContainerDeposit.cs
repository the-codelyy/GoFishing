using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerDeposit : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if (root.CompareTag("Container"))
        {
            NetworkController.Instance.DespawnEntity(root);
        }
    }
}
