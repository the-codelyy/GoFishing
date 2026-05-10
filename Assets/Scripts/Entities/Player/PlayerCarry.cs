using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarry : PlayerComponent
{
    public InteractableObject CarriedObject
    {
        get;
        private set;
    }
    
    [SerializeField] private Transform _carryPoint;
    [SerializeField] private MinMaxRange _range;
    
    private void LateUpdate()
    {
        if (CarriedObject != null)
        {
            CarriedObject.transform.position = _carryPoint.position;
        }
    }

    public void Carry(InteractableObject interactable)
    {
        CarriedObject = interactable;
        CarriedObject.GetComponent<Rigidbody>().isKinematic = true;
        CarriedObject.TakeOwnershipServerRpc();
    }

    public void Drop()
    {
        CarriedObject.NetworkObject.RemoveOwnership();
        CarriedObject.GetComponent<Rigidbody>().isKinematic = false;
        CarriedObject = null;
    }
}
