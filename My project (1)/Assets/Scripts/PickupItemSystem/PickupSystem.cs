using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private  InventorySO inventoryData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>(); // collsion with the item that is on the ground
        if(item != null )
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity, item.parameters);
            if(reminder == 0) // if we can  add the item on the ground we destroy it
            {
                item.DestroyItem();
            }
            else
            {
                item.Quantity = reminder;
            }
        }
    }
}
