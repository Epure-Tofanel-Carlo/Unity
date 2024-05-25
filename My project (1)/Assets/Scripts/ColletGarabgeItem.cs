using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColletGarabgeItem : MonoBehaviour
{
   // [SerializeField] private Canvas parentCanvas;
   // [SerializeField] private ItemGenerate itemGenerate;
    private Image image;
    public List<FoodItemSO> foodItems;
    private FoodItemSO FoodItemSO;
   public  InventorySO InventorySO;
    void Start()
    {
        int randomIndex = Random.Range(0, foodItems.Count-1);
        image = GetComponent<Image>();
        FoodItemSO = foodItems[randomIndex];
        image.sprite = FoodItemSO.ItemImage;
        
    }

    public void colletItem() {
        InventorySO.AddItem(FoodItemSO, 1,null);
    }
}
