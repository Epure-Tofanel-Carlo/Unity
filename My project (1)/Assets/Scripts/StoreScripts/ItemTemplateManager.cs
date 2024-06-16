using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplateManager : MonoBehaviour
{
    [SerializeField]private ItemSO item;
    [SerializeField] private PlayerBluePrint playerStats;
    public InventorySO inventorySO;



    public void buyThisItem()
    {
        if(playerStats.updateMoney(item.Price)) ///functia verifica daca are si ii si ia direct
        {
            inventorySO.AddItem(item, 1, null);
            Debug.Log("Ai cumparat un " + item.Name);
        }
        else
        {
            Debug.Log("Nu ai bani!");
        }
    }

    public void sellThisItem()
    {
        int newPrice = 75 / 100 * item.Price;
        playerStats.giveMoney(newPrice);
        Debug.Log(inventorySO.inventoryItems.FindIndex(0, p => p.Item == item));
    }

    public void SetPlayer(PlayerBluePrint playerSt) {  playerStats = playerSt; }
    public void SetItem(ItemSO item) {  this.item = item; }
    public void SetInventory(InventorySO inv) { this.inventorySO = inv; }
}
