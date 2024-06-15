using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private GameObject panelUI;

    private Item currentItem;

    //Functii care trateaza momentul in care jucatorul "se loveste" de obiectul amplasat pe sol
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentItem = collision.GetComponent<Item>();
        if (currentItem != null)
        {
            if (panelUI != null)
            {
                panelUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && item == currentItem)
        {
            currentItem = null;
            if (panelUI != null)
            {
                panelUI.SetActive(false);
            }
        }
    }
    //Functia care trateaza momentul cand jucatorul vrea sa ridice itemul de pe jos
    private void Update()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            int reminder = inventoryData.AddItem(currentItem.InventoryItem, currentItem.Quantity, currentItem.parameters);
            if (reminder == 0) 
            {
                currentItem.DestroyItem();
                currentItem = null;

                if (panelUI != null)
                {
                    panelUI.SetActive(false);
                }
            }
            else
            {
                currentItem.Quantity = reminder;
            }
        }
    }
}
