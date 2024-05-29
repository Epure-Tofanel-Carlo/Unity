using System.Collections.Generic;
using UnityEngine;
using InventoryUI;
using Inventory.Model;

public class FurnaceController : MonoBehaviour
{
    [SerializeField] private UIInventoryItem inputSlot;
    [SerializeField] private UIInventoryItem outputSlot;
    [SerializeField] private List<FurnaceItemSO> recipes;
    private InventorySO inventoryData;
    private FurnaceItemSO currentRecipe = null;
    private float bakingTimer = 0;

    private void Start()
    {
        inventoryData = InventoryManager.Instance.GetInventoryData();

        inputSlot.OnItemDroppedOn += HandleItemDroppedOnInput;
        outputSlot.OnItemClicked += HandleOutputItemClicked;
    }

    private void Update()
    {
        if (currentRecipe != null)
        {
            bakingTimer -= Time.deltaTime;
            if (bakingTimer <= 0)
            {
                CompleteCooking();
            }
        }
    }

    private void HandleItemDroppedOnInput(UIInventoryItem item)
    {
        int inputSlotIndex = inputSlot.transform.GetSiblingIndex();
        int draggedItemIndex = item.transform.GetSiblingIndex();
        CheckRecipe();
    }

    private void HandleOutputItemClicked(UIInventoryItem item)
    {
        if (outputSlot is null)
        {
            InventoryItem inventoryItem = new InventoryItem(currentRecipe.result.Item, 1);
            inventoryData.AddItem(inventoryItem);
            outputSlot.ResetData();
        }
    }

    private void CheckRecipe()
    {
        InventoryItem inputItem = inventoryData.GetItemByIndex(inputSlot.transform.GetSiblingIndex());
        foreach (FurnaceItemSO recipe in recipes)
        {
            if (recipe.result.Item == inputItem.Item)
            {
                StartCooking(recipe);
                break;
            }
        }
    }

    private void StartCooking(FurnaceItemSO recipe)
    {
        currentRecipe = recipe;
        bakingTimer = recipe.bakingTime;
    }

    private void CompleteCooking()
    {
        inputSlot.ResetData();
        outputSlot.SetData(currentRecipe.result.Item.ItemImage, 1);
        currentRecipe = null;
    }

}
