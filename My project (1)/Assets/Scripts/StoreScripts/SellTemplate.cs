using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTemplate : MonoBehaviour
{
    [SerializeField]private ItemSO item;
    [SerializeField] private PlayerBluePrint playerStats;
    public InventorySO inventorySO;
   [SerializeField] private int counter;


 

    public void sellThisItem()
    {
        int newPrice = Mathf.RoundToInt(item.Price * 0.75f);
        playerStats.giveMoney(newPrice);
        int index = inventorySO.inventoryItems.FindIndex(0, p => p.Item == item);
        inventorySO.RemoveItem(index, 1);
    }

    public void setCounter(int counter) { this.counter = counter; }

    public void clickedCounter() { this.counter--; }

    public void counterZero() {  
        if(this.counter == 0)
        {
            gameObject.SetActive(false);
        }
    
    }

    public void SetPlayer(PlayerBluePrint playerSt) {  playerStats = playerSt; }
    public void SetItem(ItemSO item) {  this.item = item; }
    public void SetInventory(InventorySO inv) { this.inventorySO = inv; }
}
