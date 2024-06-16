using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSellMananger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private List<InventoryItem> items;
    [SerializeField] private PlayerBluePrint playerStats;
    [SerializeField] private InventorySO inventory;
    public List<GameObject> objectsPrefab;

    private bool itemsCreated;

    void Start()
    {
       
        itemsCreated = false;

        //  createShopItems();
    }

    public void createShopItems()
    {
        items = new List<InventoryItem>();

        foreach (InventoryItem inventoryItem in inventory.inventoryItems)
        {
            if (inventoryItem.Item != null)
            {
                items.Add(inventoryItem);
            }

        }

        if (items != null && !itemsCreated)
        {
            objectsPrefab = new List<GameObject>();
            foreach (InventoryItem item in items)
            {
                ItemSO itemS = item.Item;
                GameObject newObject = Instantiate(itemTemplate);
                newObject.GetComponent<ItemInfo>().message = createText(itemS);
                newObject.GetComponentsInChildren<Image>()[1].sprite = itemS.ItemImage; //[0] - e fundalul si [1] - e imaginea propriu zisa

                newObject.GetComponent<SellTemplate>().SetItem(itemS);
                newObject.GetComponent<SellTemplate>().SetPlayer(playerStats);
                newObject.GetComponent<SellTemplate>().SetInventory(inventory);
                newObject.GetComponent <SellTemplate>().setCounter(item.itemQuantity);
                newObject.name = itemS.Name;
                newObject.transform.SetParent(transform, false);
                objectsPrefab.Add(newObject);
            }
            itemsCreated = true;
        }
        else
        {
            Debug.LogError("Shop Items e o lista goala!!");
        }
    }
    public void removeObjects()
    {
        if(objectsPrefab != null)
        {
            foreach (GameObject go in objectsPrefab)
            {
                if (go != null)
                {
                    Destroy(go);
                }
            }
        }
        itemsCreated = false;
        
    }
    private string createText(ItemSO item)
    {
        int newPrice = Mathf.RoundToInt(item.Price * 0.75f);
        return "Price: " + newPrice + "RON \r\nName: " + item.Name;
    }

}
