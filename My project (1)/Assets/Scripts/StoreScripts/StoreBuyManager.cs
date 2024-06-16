using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreBuyManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private List<ItemSO> items;
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
       
        if (items != null && !itemsCreated)
        {
            objectsPrefab = new List<GameObject>();
            foreach (ItemSO item in items)
            {
                GameObject newObject = Instantiate(itemTemplate);
                newObject.GetComponent<ItemInfo>().message = createText(item);
                newObject.GetComponentsInChildren<Image>()[1].sprite = item.ItemImage; //[0] - e fundalul si [1] - e imaginea propriu zisa

                newObject.GetComponent<ItemTemplateManager>().SetItem(item);
                newObject.GetComponent<ItemTemplateManager>().SetPlayer(playerStats);
                newObject.GetComponent<ItemTemplateManager>().SetInventory(inventory);
                newObject.name = item.Name;
                newObject.transform.SetParent(transform, false);
                objectsPrefab.Add(newObject);
            }
            itemsCreated = true;
        }
        else
        {
            Debug.LogError("Shop Items e o lista goala!! sau itemele sunt deja creeate");
        }
    }

    public void removeObjects()
    {
        if(objectsPrefab != null)
        {
         foreach(GameObject go in objectsPrefab)
                {
                    if(go != null)
                    {
                        Destroy(go);
                    }
                }
        }
        itemsCreated = false;

    }

    private string createText(ItemSO item)
    {
        return "Price: "+item.Price+ "RON \r\nName: " +item.Name+ "\r\nDescription: " +item.Description;
    }



}
