using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarry : PlayerComponent
{
    public Item Item
    {
        get;
        private set;
    }
    
    [SerializeField] private Transform _carryPoint;
    [SerializeField] private MinMaxRange _range;
    
    private void LateUpdate()
    {
        if (Item != null)
        {
            Item.transform.position = _carryPoint.position;
        }
    }

    public void Carry(Item item)
    {
        if (Item == null && !item.IsHeld.Value)
        {
            Item = item;
            Item.GetComponent<Rigidbody>().useGravity = false;
            Item.RequestPickupRpc();
        }
    }

    public void Drop()
    {
        if (Item != null && Item.IsHeld.Value)
        {
            Item.RequestDropRpc();
            Item.GetComponent<Rigidbody>().useGravity = true;
            Item = null;
        }
    }
}
